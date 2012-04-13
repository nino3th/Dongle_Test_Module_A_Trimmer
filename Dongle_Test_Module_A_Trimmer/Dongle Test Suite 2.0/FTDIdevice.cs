//History
//===================================================================================================
//===================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FTD2XX_NET;
using System.Text.RegularExpressions;
using System.IO;

namespace Dongle_Test_Suite_2._1
{
    class FTDIdevice
    {
        FTDI ftdi;
        Parameters parameters;
        int ComPort = 0;
        Exception yellow = new Exception();
        private MainForm mf_parent;
        public FTDIdevice(Parameters pars, MainForm mf)
        {
            parameters = pars;
            ftdi = new FTDI();
            mf_parent = mf;
        }
        public bool isOpen()
        {
            if (ftdi.IsOpen) return true;
            else return false;
        }
        public void OpenPort(uint pid, int timeout_ms)
        {
            if (ftdi.IsOpen) throw new Exception_Yellow("Program error: port communication to this device is already open.");

            DateTime startopen = new DateTime(2010, 2, 26);
            DateTime timecheck = new DateTime(2010, 2, 26);
            double time = 0;
            startopen = DateTime.Now;
            string comportstring = "";
            int sleeptime = 50;
            bool checkedwrong = false; //int i;

            //now keep trying to open it until windows actually recognizes the port.  
            //  Windows does not fully recognize, for our purposes at least, until even after the port is "open." Once you can get a valid COM port string, you're good to go.
            while(time < timeout_ms && comportstring == "")//for(i = 0; i < parameters.portopentries && comportstring == ""; i++)
            {
                OpenByPID(pid);
                    mf_parent.UpdateProgressBar_Detail(100*(int)(time/timeout_ms));
                comportstring = getCom();
                //if (comportstring != null && comportstring.Length > 4 && comportstring.Substring(0, 3) != "COM") 
                if (comportstring != "")
                    if(comportstring.Substring(0, 3) != "COM") comportstring = "";
                timecheck = DateTime.Now;
                time = timecheck.Subtract(startopen).TotalMilliseconds;

                System.Threading.Thread.Sleep(sleeptime);
                if (time > 1500 && !checkedwrong)//(i == 20) 
                {
                    checkedwrong = true;
                }
            }
            if (time >= timeout_ms)//(i == parameters.portopentries)
            {
                if (pid == parameters.reference_radio_pid) throw new Exception_Yellow("Reference radio port could not be opened after " + (timeout_ms / 1000).ToString() + " seconds.  Please check that the reference radio is properly connected to a USB port, then close and re-start the program and retry.");
                else if (pid == parameters.USB_under_test_pid) throw new Exception_Yellow("USB under test could not be opened after " + (timeout_ms / 1000).ToString() + " seconds.  Please check that the USB is properly connected and retry.  If error persists, USB fails.");
                else throw new Exception_Yellow("Port for pid " + pid.ToString("X") + " could not be opened after " + (timeout_ms / 1000).ToString() + " seconds.");
            }
            mf_parent.UpdateProgressBar_Detail(100);
            ComPort = Convert.ToInt32(comportstring.Substring(3, comportstring.Length - 3));
        }
        public void OpenByPID(uint pid)
        {
            uint locID = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            uint numDevices = CountDevices();
            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[numDevices];
            try
            {
                ftStatus = ftdi.GetDeviceList(ftdiDeviceList);  //the source of the mysterious buffering error!  at long last!
            }
            catch (Exception e)
            {
                if (e.Message.Contains("supplied buffer is not big enough")) //retry.  
                {
                    System.Threading.Thread.Sleep(80);
                    numDevices = CountDevices();
                    ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[numDevices];
                    ftStatus = ftdi.GetDeviceList(ftdiDeviceList);
                }
                else throw new Exception(e.Message + "7");
            }
            if (ftStatus != FTDI.FT_STATUS.FT_OK) throw new Exception_Yellow("couldn't get device list; no device attached?");
            if (pid != parameters.reference_radio_pid)
            {
                checkForOldFWPID(numDevices, ftdiDeviceList);  //@@@@does this take extra time?
                checkForWrongPID(numDevices, ftdiDeviceList, parameters.PIDtoavoidopening);
                if (parameters.reprogramming_oldFW_HW3_SW2) pid = parameters.finalOldFWPID;
            }
            for (int k = 0; k < numDevices; k++) // look at all devices and get the LocID of the one which has the desired PID
            {
                if (ftdiDeviceList[k] != null)
                {
                    //if (ftdiDeviceList[k].ID.ToString("X") == pid.ToString()) locID = ftdiDeviceList[k].LocId;      // if the device has the specified Product ID, find its Location ID     
                    if (ftdiDeviceList[k].ID.ToString() == pid.ToString()) locID = ftdiDeviceList[k].LocId;      // if the device has the specified Product ID, find its Location ID     
                }// this only works if there is only one board with the specified PID
            }
            if (locID != 0)
            {
                if (FTDI.FT_STATUS.FT_OK != ftdi.OpenByLocation(locID)) //actually open the device, and if it doesn't work after many tries, fail.
                {
                    //if (i > 70) 
                    throw new Exception_Yellow("Cannot find testing board. If an untested board is plugged in, unplug and plug in again. If error persists, board fails.  Starting over:"); //database: cannot open at given locID (status of OpenByLocation was not OK).  include pid so we know which device it was
                }
                if (FTDI.FT_STATUS.FT_OK != ftdi.SetBaudRate(Parameters.BaudRate_UARTandTesting) ||
                    FTDI.FT_STATUS.FT_OK != ftdi.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE) ||
                    FTDI.FT_STATUS.FT_OK != ftdi.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0x11, 0x13))
                {
                    //if (ftdi.IsOpen) ftdi.Close();
                    //if (i > 70) 
                    throw new Exception_Yellow("Cannot connect to set FTDI port settings."); //database: cannot connect to device (status of OpenByLocation was not OK).  include pid so we know which device it was
                }

            }
        }
        public void checkForOldFWPID(uint numDevices, FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList)
        {
            for (int k = 0; k < numDevices; k++) 
            {
                if (ftdiDeviceList[k] != null)
                    if (ftdiDeviceList[k].ID.ToString() == parameters.finalOldFWPID.ToString())
                    {
                        parameters.reprogramming_oldFW_HW3_SW2 = true;
                    }
            }
        }
        public void checkForWrongPID(uint numDevices, FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList, uint wrongpid)
        {
            //FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            //uint numDevices = CountDevices();
            //FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[numDevices]; try
            //{
            //    ftStatus = ftdi.GetDeviceList(ftdiDeviceList);  //the source of the mysterious buffering error!  at long last!
            //}
            //catch (Exception e)
            //{
            //    if (e.Message.Contains("supplied buffer is not big enough")) //retry.  
            //    {
            //        System.Threading.Thread.Sleep(80);
            //        numDevices = CountDevices();
            //        ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[numDevices];
            //        ftStatus = ftdi.GetDeviceList(ftdiDeviceList);
            //    }
            //    else throw new Exception(e.Message + "7");
            //}
            //if (ftStatus != FTDI.FT_STATUS.FT_OK) throw new Exception_Yellow("couldn't get device list; no device attached?");

            for (int k = 0; k < numDevices; k++) // look at all devices and get the LocID of the one which has the desired PID
            {
                if (ftdiDeviceList[k] != null)
                    if (ftdiDeviceList[k].ID.ToString() == wrongpid.ToString())
                    {
                        StreamReader testTxt = new StreamReader(Parameters.logfilepath);
                        string allRead = testTxt.ReadToEnd();            //Reads the whole text file to the end
                        testTxt.Close();                                 //Closes the text file after it is fully read.
                        if (Regex.IsMatch(allRead, parameters.MAC))        // and if this MAC address is in the log file as having been loaded already
                            throw new Exception_Red("Wrong board ID number -- this board's firmware has already been successfully loaded.  Remove and set aside.");
                        else
                            throw new Exception_Red("Wrong board ID number -- this USB is corrupted.  Set aside.");
                    }
            }
        }
        public void WaitforPlug()
        {
            UInt32 ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

            //OpenByPID(pid);

            int numdevicesrequired = 1;
            if (parameters.refradioplugged) numdevicesrequired = 2;
            int i;
            for (i=0;i<3000;i++)
            {
                if (i == 20) mf_parent.AppendToOutputText(" " + '\n' + '\n' + "USB to be tested is not yet plugged in -- please plug it in now.");
                ftStatus = ftdi.GetNumberOfDevices(ref ftdiDeviceCount);

                if (ftdiDeviceCount < numdevicesrequired)
                    System.Threading.Thread.Sleep(10);
                else if (ftdiDeviceCount == numdevicesrequired)
                    break;
                else
                    throw new Exception_Yellow("Too many devices connected.");
            }
            if (i == 3000) throw new Exception_Yellow("Timed out -- USB under test was never plugged in.  Scan again to start.");
            mf_parent.AppendToOutputText(" " + '\n' + '\n' + "Device plugged in, opening port...");

        }
        public void WaitForUnplug()
        {
            //ftdi.Close();

            int numdevicesleftafterunplug = 0;
            if (parameters.refradioplugged) numdevicesleftafterunplug = 1;
            UInt32 ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            for (; ; )
            {
                ftStatus = ftdi.GetNumberOfDevices(ref ftdiDeviceCount);
                if (ftdiDeviceCount <= numdevicesleftafterunplug) break;
                System.Threading.Thread.Sleep(100);
            }
        }
        public void resetporttemp()
        {
            //ftdi.CyclePort();
            //ftdi.Reload(0403, 6001);
            //ftdi.ResetDevice();
            //ftdi.RestartInTask();
            //ftdi.ResetPort();
        }

        public bool resetLineIsEnabled_pluschecks()
        {
            FTDI.FT232R_EEPROM_STRUCTURE eep = new FTDI.FT232R_EEPROM_STRUCTURE();
            ftdi.ReadFT232REEPROM(eep);

            if (parameters.testing)  //loading-only checks: same MAC as tested, and was there a testing error
            {
                if (eep.Description.Contains("Tested"))
                {
                    mf_parent.UpdateOutputText("WARNING: This USB has already been tested.  Test will continue in 4 seconds.");
                    mf_parent.UpdateColorDisplay("orange");
                    System.Threading.Thread.Sleep(4000);
                    mf_parent.UpdateColorDisplay("white");
                }
            }
            if(parameters.loading && !parameters.testing)  //loading-only checks: same MAC as tested, and was there a testing error
            {
                if (eep.Description.Contains("Err"))
                {
                    mf_parent.UpdateOutputText("WARNING: The most recent test of this USB ended with an error.  Test will continue in 4 seconds.");
                    mf_parent.UpdateColorDisplay("orange");
                    System.Threading.Thread.Sleep(4000);
                    mf_parent.UpdateColorDisplay("white");
                }
                else if (eep.Description.Contains("Tested") && !eep.Description.Contains(parameters.MAC))
                {
                    mf_parent.UpdateOutputText("WARNING: This USB was assigned a different MAC address during testing.  Test will continue in 3 seconds.");
                    mf_parent.UpdateColorDisplay("orange");
                    System.Threading.Thread.Sleep(3000);
                    mf_parent.UpdateColorDisplay("white");
                }
                //else if (!eep.Description.Contains("Tested"))
                //{
                //    mf_parent.UpdateOutputText("WARNING: This USB does not appear to have been tested.  Test will continue in 2 seconds.");
                //    mf_parent.UpdateColorDisplay("orange");
                //    System.Threading.Thread.Sleep(2000);
                //    mf_parent.UpdateColorDisplay("white");
                //}
            }

            parameters.FTDISerialNum = eep.SerialNumber;

            if (eep.Cbus3 == FTDI.FT_CBUS_OPTIONS.FT_CBUS_IOMODE) return true;
            else return false;
        }
        public void enableResetLine()
        {
            //build an EEPROM structure with the CBUS3 line (which we've hard-wired to the Freescale chip's Reset line) set to bitbang mode (so we can toggle on command using ResetChip function)
            FTDI.FT232R_EEPROM_STRUCTURE data = new FTDI.FT232R_EEPROM_STRUCTURE();
            ftdi.ReadFT232REEPROM(data);
            data.Cbus3 = FTDI.FT_CBUS_OPTIONS.FT_CBUS_IOMODE;

            //read serial number from board so when we re-write it it doesn't change
            //string SerialNumber = string.Empty;
            //ftdi.GetSerialNumber(out SerialNumber);
            //data.SerialNumber = SerialNumber;
            //parameters.FTDISerialNum = SerialNumber;

            //write the EEPROM structure
            if (ftdi.WriteFT232REEPROM(data) != FTDI.FT_STATUS.FT_OK)
                throw new Exception_Red("Error enabling reset line -- Dongle fails. Insert a new dongle and press \"Run Test\" or Enter."); 
        }
        public void disableResetLine()
        {
            //build an EEPROM structure with the CBUS3 line (which we've hard-wired to the Freescale chip's Reset line) set to sleep mode (so we can no longer toggle on command using ResetChip function)
            FTDI.FT232R_EEPROM_STRUCTURE data = new FTDI.FT232R_EEPROM_STRUCTURE();
            ftdi.ReadFT232REEPROM(data);
            data.Cbus3 = FTDI.FT_CBUS_OPTIONS.FT_CBUS_SLEEP;

            //read serial number from board so when we re-write it it doesn't change
            //string SerialNumber = string.Empty;
            //ftdi.GetSerialNumber(out SerialNumber);
            //data.SerialNumber = SerialNumber;

            //write the EEPROM structure
            if (ftdi.WriteFT232REEPROM(data) != FTDI.FT_STATUS.FT_OK)
                throw new Exception_Red("Error disabling reset line -- Dongle fails. Insert a new dongle and press \"Run Test\" or Enter.");
        }
        public void PortCycle()
        {
            ftdi.CyclePort();
        }
        public void ChipReset()
        {
            byte Mask = 0xC0;                       //sets CBUS3 to output and others to input and sends all low (0b 1100 0000).  
            byte BitMode = 0x20;                    // says we're in CBUS bit bang mode

            if (ftdi.SetBitMode(Mask, BitMode) != FTDI.FT_STATUS.FT_OK) throw new Exception_Red("Freescale chip reset line error");
            System.Threading.Thread.Sleep(400);

            Mask = 0xC8;                            //sets CBUS3 to output and others to input and sends CBUS3 only high (0b 1100 1000)  Freescale chip's reset line has now been toggled.  
            BitMode = 0x20;
            if (ftdi.SetBitMode(Mask, BitMode) != FTDI.FT_STATUS.FT_OK) throw new Exception_Red("Freescale chip reset line error");
            System.Threading.Thread.Sleep(100);
        }
        public void adjustBaudRate(uint newBaud)
        {
            if (FTDI.FT_STATUS.FT_OK != ftdi.SetBaudRate(newBaud))
                throw new Exception_Yellow("Setting new baud rate failed."); //database: cannot connect to device (status of OpenByLocation was not OK).  include pid so we know which device it was
        }
        public void Close()
        {
            if (ftdi.Close() != FTDI.FT_STATUS.FT_OK) throw new Exception_Yellow("FTDI close function unresponsive."); 
        }
        public void SetEEPROMaftertestonly()  //leave cbus3 in bitbang, leave serial number alone, leave PID alone, and program "Tested [MAC]" as description
        {
            //build an EEPROM structure and fill it with our settings
            FTDI.FT232R_EEPROM_STRUCTURE data = new FTDI.FT232R_EEPROM_STRUCTURE();
            ftdi.ReadFT232REEPROM(data);

            data.Cbus3 = FTDI.FT_CBUS_OPTIONS.FT_CBUS_IOMODE;
            data.Manufacturer = "ThinkEco, Inc.";
            //data.Description = "Tested " + parameters.SN + parameters.MAC;
            data.Description = "Tested " + parameters.MAC;
            //data.Description = "Tested " + parameters.MAC;                        
            data.MaxPower = 80;                  //80 mAmps

            //read serial number from board so when we re-write it it doesn't change
            //string SerialNumber = string.Empty;
            //ftdi.GetSerialNumber(out SerialNumber);
            //data.SerialNumber = SerialNumber;


            //write the EEPROM structure
            if (ftdi.WriteFT232REEPROM(data) != FTDI.FT_STATUS.FT_OK)
                throw new Exception_Yellow("Error writing final EEPROM -- Dongle fails. Insert a new dongle and press \"Run Test\" or Enter.");
        }
        public void SetEEPROMafterloading()  //put CBUS3 back to #SLEEP, leave ser num alone, program production PID from settings file, program final description
        {
            //build an EEPROM structure and fill it with our settings
            FTDI.FT232R_EEPROM_STRUCTURE data = new FTDI.FT232R_EEPROM_STRUCTURE();
            ftdi.ReadFT232REEPROM(data);

            data.Cbus3 = FTDI.FT_CBUS_OPTIONS.FT_CBUS_SLEEP;
            data.Manufacturer = "ThinkEco, Inc.";
            data.Description = "ThinkEco" + parameters.SN + " " + parameters.Temp_mac;
            //data.Description = "ThinkEco" + parameters.MAC;        
            data.ProductID = (ushort)(parameters.finalNewFWPID & 0xFFFF);              
            data.MaxPower = 80;                  //80 mAmps

            //read serial number from board so when we re-write it it doesn't change
            //string SerialNumber = string.Empty;
            //ftdi.GetSerialNumber(out SerialNumber);
            //data.SerialNumber = SerialNumber;

            
            //write the EEPROM structure
            if (ftdi.WriteFT232REEPROM(data) != FTDI.FT_STATUS.FT_OK)
                throw new Exception_Yellow("error writing final EEPROM -- Dongle fails. Insert a new dongle and press \"Run Test\" or Enter."); 
        }
        public void SetEEPROMaftererror()  //put CBUS3 back to #SLEEP, leave serial number alone, leave PID alone, and program "Err [MAC]" as description
        {
            //build an EEPROM structure and fill it with our settings
            FTDI.FT232R_EEPROM_STRUCTURE data = new FTDI.FT232R_EEPROM_STRUCTURE();
            ftdi.ReadFT232REEPROM(data);

            data.Cbus3 = FTDI.FT_CBUS_OPTIONS.FT_CBUS_SLEEP;
            data.Manufacturer = "ThinkEco, Inc.";
            data.Description = "Err " + parameters.MAC;
            data.MaxPower = 80;                  //80 mAmps

            //read serial number from board so when we re-write it it doesn't change
            //string SerialNumber = string.Empty;
            //ftdi.GetSerialNumber(out SerialNumber);
            //data.SerialNumber = SerialNumber;


            //write the EEPROM structure
            if (ftdi.WriteFT232REEPROM(data) != FTDI.FT_STATUS.FT_OK)
                throw new Exception_Yellow("error writing final EEPROM -- Dongle fails. Insert a new dongle and press \"Run Test\" or Enter.");
        }

        public int getComPort()
        {
            if (ComPort == 0) throw new Exception_Yellow("Program error: COM port has not yet been read / opened");
            return ComPort;
        }
        public uint CountDevices()
        {
            uint ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            ftStatus = ftdi.GetNumberOfDevices(ref ftdiDeviceCount);
            if (ftStatus != FTDI.FT_STATUS.FT_OK) throw new Exception_Yellow("Could not get device count");
            return ftdiDeviceCount;
        }
        public string getCom()
        {
            string COM = null;
            ftdi.GetCOMPort(out COM);
            if (COM == null) throw new Exception_Yellow("No COM port found");
            return COM;
        }


        //reading and writing functions:
        public void WriteByte(byte[] toWriteByte)
        {
            UInt32 numBytesWritten = 0;
            if (FTDI.FT_STATUS.FT_OK != ftdi.Write(toWriteByte, toWriteByte.Length, ref numBytesWritten))
            {
                numBytesWritten = 0;
                throw new Exception_Yellow("Error writing byte.  Please unplug and re-plug both the test USB and the reference radio and re-start the program.");
            }
        }
        public void DummyRead(ref UInt32 numBytesRead)
        {
            UInt32 numBytesAvailable = 0;
            //UInt32 numBytesRead = 0;

            ftdi.GetRxBytesAvailable(ref numBytesAvailable);
            byte[] readData = new byte[numBytesAvailable];

            //for (int i = 0; i < numBytesAvailable; i++)
            //{
            ftdi.Read(readData, numBytesAvailable, ref numBytesRead);
        }
        public bool WriteCharacter(string toWrite)
        {
            UInt32 numBytesWritten = 0;

            if (FTDI.FT_STATUS.FT_OK != ftdi.Write(toWrite, toWrite.Length, ref numBytesWritten)) return false;

            return true;
        }
        public bool ReadSpecificString(string expectedstring)
        {
            //byte[] received = new byte[expectedstring.Length];
            UInt32 numBytesAvailable = 0;
            UInt32 numBytesRead = 0;
            //Queue<byte> FreshBytes = new Queue<byte>();

            ftdi.GetRxBytesAvailable(ref numBytesAvailable);
            byte[] readData = new byte[numBytesAvailable];
            //byte[] readData = new byte[1];

            //for (int i = 0; i < numBytesAvailable; i++)
            //{
                ftdi.Read(readData, numBytesAvailable, ref numBytesRead);
                //FreshBytes.Enqueue(readData[0]);
            //}
                string receivedstring = System.Text.ASCIIEncoding.ASCII.GetString(readData);
                //if (receivedstring != expectedstring) 
                //{
                //    if (receivedstring.Contains(expectedstring))
                //    {
                //        bool dummy = true;
                //    }
                //    return false;
                //}
                if (!receivedstring.Contains(expectedstring)) return false;  //
                else return true;
        }
        public byte[] ReadOneByte()
        {
            UInt32 numBytesAvailable = 0;
            UInt32 numBytesRead = 0;
            byte[] nothing = null;

            ftdi.GetRxBytesAvailable(ref numBytesAvailable);
            if (numBytesAvailable == 0) return nothing;

            byte[] readData = new byte[1];
            ftdi.Read(readData, 1, ref numBytesRead);
            return readData;
        }
        public byte[] poll()
        {
            UInt32 numBytesAvailable = 0;
            UInt32 numBytesRead = 0;
            //byte[] nothing = null;

            ftdi.GetRxBytesAvailable(ref numBytesAvailable);
            //if (numBytesAvailable == 0) return nothing;

            byte[] readData = new byte[numBytesAvailable];
            ftdi.Read(readData, numBytesAvailable, ref numBytesRead);
            return readData;
        }
    }
}
