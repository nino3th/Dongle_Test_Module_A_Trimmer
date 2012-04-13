using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Dongle_Test_Suite_2._1
{
    class RAMLoader
    {
        Parameters parameters;
        FTDIdevice thisUSB;
        string filepathtoload;
        int max_size = 30 * 1024;
        private MainForm mf_parent;

        public RAMLoader(Parameters pars, FTDIdevice USB, string binfilepath, MainForm mf)
        {
            parameters = pars;
            thisUSB = USB;
            filepathtoload = binfilepath;
            mf_parent = mf;
        }
        public void Run(bool loadingSSL)
        {
            Connect();
            LoadtoRAM(loadingSSL);
        }
        void Connect()
        {
            for(int tries = 30; tries > 0; tries--)  //originally 20 tries, changed to 50 after getting errors here
            {
                ////Pull the RTS low
                //UInt32 bytesRead = 0;
                //do
                //{
                //    thisUSB.DummyRead(ref bytesRead);
                //    //ReadFile(hCom, &dummy, 1, &bytesRead, NULL);
                //} while (bytesRead > 0);

                //Send autobaud data
                byte[] ZeroByte = new byte[1];
                ZeroByte[0] = 0;
                thisUSB.WriteByte(ZeroByte);

                System.Threading.Thread.Sleep(25);
                //Expect CONNECT response
                if (thisUSB.ReadSpecificString("CONNECT")) 
                    return;
            }
            throw new Exception_Red("USB under test did not respond to null character -- firmware may already be loaded.");
        }
        void LoadtoRAM(bool checkingforREADYstring)
        {
            //check the filesize of the image to be loaded to RAM (ZTC or SSL)
            FileInfo f = new FileInfo(filepathtoload);
            int filesize = (int)f.Length;  //this cast is only okay as long as our file is short enough; we should be fine.
            if (filesize > max_size) throw new Exception_Yellow("File being loaded into RAM is too big");

            //read the file into a buffer
                //to be rigorous, use:  //byte[] buffer = ReadFile(filepathtoload);  which refers to the commented-out function below.
                //but we can read this into a buffer all at once cause it's small, so:
            byte[] buffer = new byte[filesize];
            FileStream fileStream = new FileStream(filepathtoload, FileMode.Open, FileAccess.Read);
            fileStream.Read(buffer, 0, filesize);

            //write the file length and then the buffer itself
            byte[] filesizebytes = BitConverter.GetBytes(filesize);
            thisUSB.WriteByte(filesizebytes);
            thisUSB.WriteByte(buffer);


            if (checkingforREADYstring)
            {
                thisUSB.adjustBaudRate(Parameters.BaudRate_SSL);
                System.Threading.Thread.Sleep(100);
                if (!thisUSB.ReadSpecificString("READY")) 
                    throw new Exception_Yellow("error loading ssl");
            }
        }

        //probably unnecessary:
        //public static byte[] ReadFile(string filePath)
        //{
        //    byte[] buffer;
        //    FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //    try
        //    {
        //        int length = (int)fileStream.Length;  // get file length
        //        buffer = new byte[length];            // create buffer
        //        int count;                            // actual number of bytes read
        //        int sum = 0;                          // total number of bytes read

        //        // read until Read method returns 0 (end of the stream has been reached)
        //        while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
        //            sum += count;  // sum is a buffer offset for next reading
        //    }
        //    finally
        //    {
        //        fileStream.Close();
        //    }
        //    return buffer;
        //}
    }
}
