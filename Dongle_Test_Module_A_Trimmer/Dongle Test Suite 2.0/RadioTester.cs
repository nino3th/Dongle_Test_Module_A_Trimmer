using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dongle_Test_Suite_2._1
{
    class RadioTester
    {
        const string ZTCcommand_setshortaddrAABB = "85 09 09 53 BB AA 00 00 00 00 00 00";
        const string ZTCcommand_MACstart =         "85 0A 09 35 12 0B 0F 0F 01 00 00 00";      
        const string ZTCcommand_SetLongModeAddr =   "A3 DB 08 22 22 22 22 22 22 22 22";
        const string ZTCcommand_SetChannel11 =      "85 09 09 21 0B 00 00 00 00 00 00 00";
        const string ZTCcommand_setshortaddrCCDD =  "85 09 08 53 DD CC 00 00 00 00 00";
        const string ZTCcommand_SetPowerLevel09 =   "95 0F 03 09 00 00";
        const string dataReqBeforePackets =         "87 00 1D BB AA 00 00 00 00 00 00 35 12 02 DD CC 00 00 00 00 00 00 35 12 02 04 02 00 ";

        Parameters parameters;
        FTDIdevice thisUSB;
        FTDIdevice referenceRadio;
        public RadioTester(Parameters pars, FTDIdevice USB, FTDIdevice refradio)
        {
            parameters = pars;
            thisUSB = USB;
            referenceRadio = refradio;
        }
        public int RunRadioTest(int lengthofeachtest)
        {
            // run a radiotest with dongle as sender
            StartCoordinator(referenceRadio);
            StartEndDevice(thisUSB);
            int PER = ConnectAndTest(referenceRadio, thisUSB, lengthofeachtest);// lengthofeachtest packets long

            // run a radiotest with dongle as receiver
            StartCoordinator(thisUSB);
            StartEndDevice(referenceRadio);
            PER += ConnectAndTest(thisUSB, referenceRadio, lengthofeachtest);// lengthofeachtest packets long

            return PER;
            //return 0;
        }
        public void StartCoordinator(FTDIdevice thisdevice)
        {
            thisdevice.poll();  //clear out anything sitting on the port
            //Command(FTinti, "A3 00 0A 01 01 01 02 00 00 00 00 00 00");      // mode select 
            //Command(FTinti, "85 06 01 01");                                 // mac reset
            //Command(FTinti, "85 09 09 21 0B 00 00 00 00 00 00 00");         // channel
            //Command(FTinti, "A3 DB 08 11 11 11 11 11 11 11 11");            // longmode
            Utils.SendZTCCommand(thisdevice, ZTCcommand_setshortaddrAABB);         // set short address AA BB
                Utils.WaitForZTCResponse(thisdevice,parameters);
            //Command(FTinti, "85 09 08 41 01 00 00 00 00 00 00");            // Permit association
            //Command(FTinti, "85 09 08 52 01 00 00 00 00 00 00");            // recieve while idle
            Utils.SendZTCCommand(thisdevice, ZTCcommand_MACstart);         // mac start  
            Utils.WaitForZTCResponse(thisdevice, parameters);
        }
        public void StartEndDevice(FTDIdevice thisdevice)
        {
            thisdevice.poll();  //clear out anything sitting on the port
            //Command(FTinti, "A3 00 0A 01 01 01 02 00 00 00 00 00 00");      // mode select
            //Command(FTinti, "85 06 01 01");                                 // mac reset       
            Utils.SendZTCCommand(thisdevice, ZTCcommand_SetLongModeAddr);            // longmode 
                //no ZTC response expected
            Utils.SendZTCCommand(thisdevice, ZTCcommand_SetChannel11);         // channel
                Utils.WaitForZTCResponse(thisdevice, parameters);
            Utils.SendZTCCommand(thisdevice, ZTCcommand_setshortaddrCCDD);            // set short address
                Utils.WaitForZTCResponse(thisdevice, parameters);
            Utils.SendZTCCommand(thisdevice, ZTCcommand_SetPowerLevel09);                           // power level 09
                Utils.WaitForZTCResponse(thisdevice, parameters);
        }
        public int ConnectAndTest(FTDIdevice Coordinator, FTDIdevice EndDevice, int testlength)
        {
            //CONNECT
            //Command(FTintA, "A3 DB 08 11 11 11 11 11 11 11 11");            // longmode
            //Command(FTintA, "85 09 09 53 BB AA 00 00 00 00 00 00");         // set short address AA BB
            //CommandWithoutListen(FTintA, "85 01 0C 22 22 22 22 22 22 22 22 DD CC 00 00"); //macresponse preparation from coordinator
            //System.Threading.Thread.Sleep(100);
            //Command(FTintB, "85 00 0E 11 11 11 11 11 11 11 11 34 12 03 0B 00 80");  //macassociate.request from end device
            //we could use CommandWithoutListen, except for some reason it seems to succeed quickly only if we do listen (without listening, even sleep 360 isn't enough)
            System.Threading.Thread.Sleep(160);  // a sleep time of 140 results in the first packet being missed
            
            
            //Byte[] x = Coordinator.read();   //just clearing the buffer by reading from it, so if there's a response it doesn't mess us up
            //x = EndDevice.read();

            int p = 0;
            for (int i = 0; i < testlength; i++)
            {
                string macdatarequest = dataReqBeforePackets + PayloadPacketPrep(i);
                Utils.SendZTCCommand(EndDevice, macdatarequest);
                if (Utils.WaitForZTCResponseAndIndication(EndDevice, Coordinator, macdatarequest))
                    p++;
            }

            if (p < 2) throw new Exception_Red("Radio test unsuccessful (less than 20% of packets transmitted). Testing fails. Insert a new board and press \"Run Test\" or Enter.");

            return p;
        }
        public string PayloadPacketPrep(int i)
        {
            string s = i.ToString("X");
            int l = s.Length;
            while (l < 6)
            {
                s = "0" + s;
                l = s.Length;
            }
            s += s.Substring(s.Length - 2, 2);

            return s;
        }

    }
}
