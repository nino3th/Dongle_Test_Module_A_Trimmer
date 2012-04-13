using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dongle_Test_Suite_2._1
{
    class FirmwareLoader
    {
        Parameters parameters;
        FTDIdevice thisUSB;
        MainForm mf_parent;

        uint FLASH_IMAGE_HEADER = 8;
        int max_image_size = (96 * 1024 - 8);
        //int BIN_FILE_BUFFER_SIZE = 4 * 1024;    //Buffer allocated for reading the SSL binary image

        public FirmwareLoader(Parameters pars, FTDIdevice USB, MainForm mf)//, string imagefilepath)
        {
            parameters = pars;
            thisUSB = USB;
            mf_parent = mf;
        }
        public void Run()
        {
            thisUSB.adjustBaudRate(Parameters.BaudRate_SSL);
            //InsertCustomParameters();

            //load ssl using RAMLoader
            RAMLoader loadSSL = new RAMLoader(parameters, thisUSB, parameters.SSLfilepath, mf_parent);
            loadSSL.Run(true);

            thisUSB.adjustBaudRate(Parameters.BaudRate_SSL);  //increase the baud rate 8x, saves six seconds on loading (goes from 14 to 8)

            SSLInterface sslinterface = new SSLInterface(parameters, thisUSB);
            try
            {
                //Read the firmware image into memory and check its length
                int filesize = 0;
                byte[] fullfilebuffer = readimagefromfile(parameters.FWimagefilepath, ref filesize);
                //try
                //{
                //    sslinterface.SSL_FULLERASE(); //cut this out before giving to controltek, this is here just to clear the hw info pars out while we're debugging.
                //}
                //catch (Exception_Yellow exc)
                //{
                //    if (!exc.Message.Contains("retry failed")) throw new Exception_Yellow(exc.Message); //if it's some other exception, don't catch it
                //    else sslinterface.SSL_FULLERASE(); //retry once, if the exception comes back again, we won't catch it and it'll go on to be displayed as "retry failed"
                //}
                LoadFW(sslinterface, fullfilebuffer, filesize);
                LoadHardwareSettings(sslinterface);
                Commit(sslinterface, filesize);
            }
            catch (Exception_Yellow ey)
            {
                if (ey.Message.Contains("CRC"))
                {
                    mf_parent.UpdateOutputText("Error! Reverting to empty state, do not unplug...");
                    sslinterface.SSL_FULLERASE();
                    throw new Exception_Yellow("SSL throws CRC check error; packet was corrupted over UART. Memory has been erased, please try again.");
                }
                else if (ey.Message.Contains("Write"))
                {
                    mf_parent.UpdateOutputText("Error! Reverting to empty state, do not unplug...");
                    sslinterface.SSL_FULLERASE();
                    throw new Exception_Yellow("SSL throws write error; write operation fails. Memory has been erased, please try again.");
                }
                else throw ey;
            }
            thisUSB.adjustBaudRate(Parameters.BaudRate_UARTandTesting);
        }
        public void LoadHardwareSettings(SSLInterface sslinterface)
        {
            parameters.fillHardwarePars();
            //thisUSB.poll();  //just in case we need to clear junk off the port
            try
            {
                sslinterface.SSL_WRITE(Parameters.HwInfAdr, parameters.HardwareParametersArray, (UInt16)parameters.HardwareParametersArray.Length);
            }
            catch (Exception_Yellow exc)
            {
                if (!exc.Message.Contains("retry failed")) throw new Exception_Yellow(exc.Message); //if it's some other exception, don't catch it
                else sslinterface.SSL_WRITE(Parameters.HwInfAdr, parameters.HardwareParametersArray, (UInt16)parameters.HardwareParametersArray.Length); //retry once, if the exception comes back again, we won't catch it and it'll go on to be displayed as "retry failed"
            }
        }
        public void LoadTrimsOnly(SSLInterface sslinterface)
        {
            //parameters.fillHardwarePars();
            //thisUSB.poll();  //just in case we need to clear junk off the port
            try
            {
                sslinterface.SSL_WRITE(Parameters.TrimAdr, parameters.TwoTrimBytes, (UInt16)parameters.TwoTrimBytes.Length);
            }
            catch (Exception_Yellow exc)
            {
                if (!exc.Message.Contains("retry failed")) throw new Exception_Yellow(exc.Message); //if it's some other exception, don't catch it
                else sslinterface.SSL_WRITE(Parameters.TrimAdr, parameters.TwoTrimBytes, (UInt16)parameters.TwoTrimBytes.Length);
            }
        }

        private void LoadFW(SSLInterface sslinterface, byte[] fullfilebuffer, int filesize)
        {
            int currentReadAddress = 0;
            uint currentWriteAddress = FLASH_IMAGE_HEADER;  //Initialize the write address with the first byte after the FLASH header
            UInt16 bytesToRead;

            //only used in testing to erase partially-cleared dongles:
            if (parameters.reprogramming_oldFW_HW3_SW2 || parameters.reprogramming_newFW)
            {
                try
                {
                    mf_parent.UpdateOutputText("Finalizing erase of old firmware...");
                    sslinterface.SSL_FULLERASE();
                }
                catch (Exception_Yellow exc)
                {
                    if (!exc.Message.Contains("retry failed")) throw new Exception_Yellow(exc.Message); //if it's some other exception, don't catch it
                    else sslinterface.SSL_FULLERASE(); //retry once, if the exception comes back again, we won't catch it and it'll go on to be displayed as "retry failed"
                }
            }

            mf_parent.UpdateOutputText("Loading production firmware, filename \"" + parameters.FWimagefilename + "\"...");

                //now chop it up into chunks the SSL will be able to handle, and write each chunk to the ssl, which then writes it to RAM and sends back a confirm
                while (currentReadAddress != filesize)
                {
                    if (filesize - currentReadAddress > Parameters.SSL_ENG_BUFFER_SIZE) bytesToRead = Parameters.SSL_ENG_BUFFER_SIZE;  //make chunks in the max buffer size the SSL can take (512 bytes)
                    else
                        bytesToRead = (UInt16)(filesize - currentReadAddress); //the last chunk will be smaller than the max buffer size
                    byte[] tempbuffer = new byte[bytesToRead];
                    for (int i = 0; i < bytesToRead; i++)
                    {
                        tempbuffer[i] = fullfilebuffer[currentReadAddress];
                        currentReadAddress++;
                    }
                    try
                    {
                        sslinterface.SSL_WRITE(currentWriteAddress, tempbuffer, bytesToRead);
                    }
                    catch (Exception_Yellow exc)
                    {
                        if (!exc.Message.Contains("retry failed")) throw new Exception_Yellow(exc.Message); //if it's some other exception, don't catch it
                        else sslinterface.SSL_WRITE(currentWriteAddress, tempbuffer, bytesToRead); //retry once, if the exception comes back again, we won't catch it and it'll go on to be displayed as "retry failed"
                    }
                    //catch (Exception e)
                    //{
                    //    if (!e.Message.Contains("reference not set")) throw new Exception(e.Message); //if it's some other exception, don't catch it
                    //    else throw new Exception(e.Message + "  it happened inside SSL_WRITE");
                    //}

                    currentWriteAddress += bytesToRead;
                    mf_parent.UpdateProgressBar_Detail((int)(100 * (currentWriteAddress - FLASH_IMAGE_HEADER) / filesize));
                }

        }
        private void Commit(SSLInterface sslinterface, int filesize)
        {
            //now that we're done loading, commit the image by writing the image header
            try
            {
                sslinterface.SSL_COMMIT((uint)filesize);
            }
            catch (Exception_Yellow exc)
            {
                if (!exc.Message.Contains("retry failed")) throw new Exception_Yellow(exc.Message); //if it's some other exception, don't catch it
                else sslinterface.SSL_COMMIT((uint)filesize); //retry once, if the exception comes back again, we won't catch it and it'll go on to be displayed as "retry failed"
            }
        }
        private byte[] readimagefromfile(string filepath, ref int filesize)
        {
            FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            filesize = (int)fileStream.Length;  //this cast is only okay as long as our file is short enough; we should be fine.
            if (filesize > max_image_size) throw new Exception_Yellow("Image is too big to be loaded into flash; select another image to load.");
            byte[] fullfilebuffer = new byte[filesize];
            fileStream.Read(fullfilebuffer, 0, filesize);
            fileStream.Close();
            return fullfilebuffer;
        }
        public void EraseSector24(SSLInterface sslinterface)
        {
            try
            {
                sslinterface.SSL_ERASE_Sector(0x18000);  //also write-unprotects
            }
            catch (Exception_Yellow exc)
            {
                if (!exc.Message.Contains("retry failed")) throw new Exception_Yellow(exc.Message); //if it's some other exception, don't catch it
                else sslinterface.SSL_ERASE_Sector(0x18000); //retry once, if the exception comes back again, we won't catch it and it'll go on to be displayed as "retry failed"
            }
        }


        //private void InsertCustomParameters()
        //{

        //    byte[] fullimagebytes = File.ReadAllBytes(parameters.FWimagefilepath);
        //    string fullimagestring = Utils.ConvertByteArrayToSpacedHexString(fullimagebytes);
        //    string crystalTrimInjection = "";

        //    //USE TRIM VALUES FROM EITHER TRIMMING, DATABASE, OR SETTINGS FILE
        //    //if (trimming || getTrimFromDB)
        //    crystalTrimInjection = Trimmer.TrimPacketPrep(parameters.coarsetrim_SET) + " " + Trimmer.TrimPacketPrep(parameters.finetrim_SET);  //set to the value arrived at by trimmer
        //    //else if (!trimming)
        //    //{
        //    //    //StreamReader SR = File.OpenText(@"C:\Program Files\ThinkEco\ThinkEco Dongle Test Suite\Settings.txt");
        //    //    //string s = SR.ReadToEnd();
        //    //    int coarseindex = s.IndexOf("coarse") + 8;
        //    //    int fineindex = s.IndexOf("fine") + 6;
        //    //    int coarsefromsettings = Convert.ToInt32(s.Substring(coarseindex, 2));
        //    //    int finefromsettings = Convert.ToInt32(s.Substring(fineindex, 2));
        //    //    crystalTrimInjection = TrimPacketPrep(coarsefromsettings) + " " + TrimPacketPrep(finefromsettings);  // set trim to whatever's in settings file
        //    //    FinalCoarse = coarsefromsettings;
        //    //    FinalFine = finefromsettings;
        //    //}
        //    string littleEndianMAC = "";
        //    //int w = GetFinalCoarse(); int z = GetFinalFine();
        //    for (int i = 0; i < 8; i++)
        //        littleEndianMAC += parameters.MAC.Substring(parameters.MAC.Length - (i + 1) * 2, 2) + " ";

        //    //INSERT MAC ADDRESS AND TRIM SETTINGS
        //    string modifiedimagestring = fullimagestring.Remove(6165, 24);
        //    modifiedimagestring = modifiedimagestring.Insert(6165, littleEndianMAC);
        //    ////string checkthis = modifiedimagestring.Substring(6230, 50);
        //    modifiedimagestring = modifiedimagestring.Remove(6252, 5);   //6252
        //    modifiedimagestring = modifiedimagestring.Insert(6252, crystalTrimInjection);
        //    ////string checkthistoo = binfileMod.Substring(6230, 50);
        //    byte[] modifiedimagebytes = Utils.ConvertHexStringToByteArray(modifiedimagestring);
        //    string modfilepath = Parameters.modifiedFWimagefilepath;
        //    File.WriteAllBytes(@modfilepath, modifiedimagebytes);
        //}


        //public void ReadImageBack()
        //{

        //    //@@@@
        //    // //first check that we the header -- first 8 bytes -- is what we want it to be (added 7/12/2011)
        //    // uint32_t startAddress = 0;
        //    // toRead = FLASH_IMAGE_HEADER;
        //    //   status = ENG_Read(pInputParams->hCom, startAddress, (uint8_t*)flashHeaderBuffer, toRead, READ_DATA_TO);
        //    //   if(status != gEngSuccessOp_c)
        //    //   {
        //    //     printf("\nError trying to read a data chunck from the FLASH: %d", status);
        //    //	//CloseHandle(hFile);
        //    //     return -1;
        //    //   }

        //    //FILE *out; 

        //    //out = fopen("first8bytes.bin", "wb"); 

        //    //for(int i=0;i<8;i++)fprintf(out, "%c", flashHeaderBuffer[i]); 

        //    //fclose(out); 


        //    //Initialize the write address with the first byte after the FLASH header
        //    currentReadAddress = FLASH_IMAGE_HEADER;
        //    //Start the read and write loop. Print progress information.
        //    printf("\n");
        //    bytesOps = 0;
        //    binReminder = fileSize;
        //    while (binReminder != 0)
        //    {
        //        //Determine how much can we read
        //        if (binReminder > SSL_ENG_BUFFER_SIZE)
        //        {
        //            toRead = SSL_ENG_BUFFER_SIZE;
        //        }
        //        else
        //        {
        //            toRead = binReminder;
        //        }
        //        //Update the remaining data to read
        //        binReminder -= toRead;
        //        //Read the data

        //        status = ENG_Read(pInputParams->hCom, currentReadAddress, (uint8_t*)readFlashBuffer, toRead, READ_DATA_TO);
        //        if (status != gEngSuccessOp_c)
        //        {
        //            printf("\nError trying to read a data chunck from the FLASH: %d", status);
        //            //CloseHandle(hFile);
        //            return -1;
        //        }
        //        //if(sizeof(readFlashBuffer) == toRead)  //this causes errors and is kind of silly because we pass in the length to read and get it back out anyway
        //        //{
        //        //Read operation finished sucessfully, now compare to written image
        //        ReadFile(pInputParams->hBinFile2, binFileBuffer2, toRead, &bytesOps, NULL);
        //        if (bytesOps == toRead)
        //        {
        //            for (int i = 0; i < toRead; i++)
        //            {
        //                if (readFlashBuffer[i] != binFileBuffer2[i])
        //                {
        //                    printf("Write error: image on flash does not match image on computer");
        //                    return -1;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //Error reading the data. Inform the user and exit the function.
        //            printf("\nError: Could not read the binary.\n");
        //            PrintHelpText();
        //            return -1;
        //        }

        //        //Update the write address for the next write
        //        currentReadAddress += toRead;
        //        printf(".");
        //        //}
        //        //else
        //        //{
        //        //  //Error reading the data. Inform the user and exit the function.
        //        //  printf("\nError: Could not read the binary.\n");
        //        //  PrintHelpText();
        //        //  return -1;
        //        //}
        //    }
  
        //}


        

    }
}
