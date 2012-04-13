//History
//===================================================================================================
// 20120413 |  1.2.2   | Nino Liu   |  Modified the path of reference files
//===================================================================================================
#define GCOM

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Dongle_Test_Suite_2._1
{
    class Parameters
    {
        //constants
#if !GCOM
        public const string mainfilepath = "C:\\Program Files\\ThinkEco Test Suite\\Dongle Test Module A 1.2";
#else
        public const string mainfilepath = "D:\\Project\\ThinkEco dongle test procedure update\\Dongle Test Module A - Trimmer Source 1.2\\Dongle_Test_Module_A_Trimmer\\Dongle Test Suite 2.0\\res\\";
#endif
        public const string settingsfilepath = mainfilepath + "\\Settings.txt";
        public const string testingfilepath = mainfilepath + "\\testing";
        public const string loadingfilepath = mainfilepath + "\\loading";
        public const string modifiedFWimagefilepath = loadingfilepath + "\\ModifiedFWImageForLoading.bin";
        public const string logfilepath = mainfilepath + "\\log.txt";
        public const string backuplogfilepath = mainfilepath + "\\backuplog.txt";
        public const string errorlogfilepath = mainfilepath + "\\errorlog.txt";
        public const string nextSNfilepath = mainfilepath + "\\nextSN.txt";


        public bool reprogramming_oldFW_HW3_SW2 = false;
        public bool reprogramming_newFW = false; //remember that this only works on new firmware, and if the pin64 program is in the specific directory it's in on my computer now.
        public const string MACheader = "804F58";

        public const uint HwInfAdr = 0x18000;
        public const uint TrimAdr = 0x18014;
        public byte[] TwoTrimBytes = new byte[2];

        public const int SSL_ENG_BUFFER_SIZE =  0x200;  //read/write maximum buffer size
        public const int SSL_write_timeout = 2500; //ms timeout

        public int portopentries = 126; //number of 50-ms tries before port is declared un-openable
        public const int BaudRate_UARTandTesting = 115200;
        public const int BaudRate_SSL = 921600;//115200; //

        public string TestSuiteVersionNumber = "USBTestModuleA1.2_" + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year;

        //to be filled
        public string MAC = "_";
        public string SN = "_"; //add 20120323 by nino
        public string Temp_mac = "_"; //add 20120323 by nino
        public int radioTestsuccesses;
        public double radiotestsuccesspercent;
        public string radiotestsuccesspercentstring = "_";
        public DateTime StartTime;
        public DateTime TimeStamp;
        public string totaltesttime = "-";
        public string FTDISerialNum = "-";
        public string testingandorloading = "-";
        public string MachineName = "-";
        public string FinalCoarsestring = "_";
        public string FinalFinestring = "_";
        public string FinalFreqstring = "_";
        public string TrimSequence = "_";
        public int coarsetrim_measured;
        public int finetrim_measured;
        public int coarsetrim_lookup;
        public int finetrim_lookup;
        public int coarsetrim_SET;
        public int finetrim_SET;
        public double frequency_measured;
        public string PINstring = "_";
        public byte counter_id;//add 20120322 by Nino

        //to be in settings file (default values, if any, set here)
        public bool testing;
        public uint USB_under_test_pid = 0x04036001;
        public uint reference_radio_pid = 0x04036006;
        public uint finalOldFWPID = 0x04038C80;
        public uint finalNewFWPID = 0x04038C81;
        public uint PIDtoavoidopening = 99;
        public bool loading;
        public bool crystaltrimming = false;
        public string ZTCfilename;
        public string SSLfilename;
        public string FWimagefilename;
        public string ZTCfilepath;
        public string SSLfilepath;
        public string FWimagefilepath;
        public int numberofradiotestsEachWay = 5;
        public bool takingFWfilenamefromsettingsfile = true;
        public bool settingdefaulttrimsbool = false;
        public int coarsetrim_default = 10;
        public int finetrim_default = 13;
        public bool lookinguptrimsbool = false;
        public string trimsdatabasefilename;
        public string trimsdatabasefilepath;
        public bool checkingfreqafternottrimmingbool = false;
        public int listenlooptimeout = 50;

        public bool refradioplugged = false;

        public uint serialnumberprefix = 0x00;
        public uint serialnumberlower3bytes;
        public string SerialNumberHexString = "_";


        //hardware parameters:
        public byte DeviceID = 1;// 1 - ThinkEco usb receiver  
        public byte MajorVersion = 0;// 0 - reference 
        public byte MinorVersion = 0;// 0 - reference
        public uint SerialNumber;
        public byte[] SerialNumberbytes = new byte[4];// S/N, little-endian
        public byte ZigBeeSocID = 0;// 0 - MC13224V
        public byte[] FirmwarePin = new byte[4];// FW upgrade PIN, no endianness, stored in order 0-3
        public byte[] IeeeAddr = new byte[8]; //IEEE address, little-endian
        public byte XtalCTune; //xtal trim
        public byte XtalFTune;
        public byte[] HardwareParametersArray = new byte[22];
        public void fillHardwarePars() //must run after MAC has been scanned and final trim values have been chosen
        {
            HardwareParametersArray[0] = DeviceID;
            HardwareParametersArray[1] = MajorVersion;
            HardwareParametersArray[2] = MinorVersion;
            HardwareParametersArray[3] = SerialNumberbytes[3];
            HardwareParametersArray[4] = SerialNumberbytes[2];
            HardwareParametersArray[5] = SerialNumberbytes[1];
            HardwareParametersArray[6] = SerialNumberbytes[0];
            HardwareParametersArray[7] = ZigBeeSocID;
            storeRandomPIN();
            HardwareParametersArray[8] = FirmwarePin[0];
            HardwareParametersArray[9] = FirmwarePin[1];
            HardwareParametersArray[10] = FirmwarePin[2];
            HardwareParametersArray[11] = FirmwarePin[3];
            for (int i = 0; i < 8; i++)
                HardwareParametersArray[12 + i] += Convert.ToByte(MAC.Substring(MAC.Length - (i + 1) * 2, 2), 16);
            XtalCTune = (byte)coarsetrim_SET;
            XtalFTune = (byte)finetrim_SET;
            HardwareParametersArray[20] = XtalCTune;
            HardwareParametersArray[21] = XtalFTune;
        }
        public void storeRandomPIN()
        {
            FirmwarePin = randomPIN();

            //make this into a string for the log file and make sure it includes a 16's digit if that digit is 0 (so 0x0C becomes "0C" not "C")
            PINstring = "";
            for(int i = 0; i<4;i++)
                PINstring += FirmwarePin[i].ToString("X2");
        }
        private byte[] randomPIN()
        {
            byte[] PIN = new byte[4];
            RandomNumberGenerator randgen = RandomNumberGenerator.Create();
            randgen.GetBytes(PIN);
            return PIN;
        }
        public void SetSerialNumber()
        {
            SerialNumber = NextSerialNumber();                     
            SerialNumberbytes[0] = Convert.ToByte((SerialNumber & 0xFF000000) >> 24);  //split 64-bit address into 4 bytes, little-Endian
            SerialNumberbytes[1] = Convert.ToByte((SerialNumber & 0x00FF0000) >> 16);
            SerialNumberbytes[2] = Convert.ToByte((SerialNumber & 0x0000FF00) >> 8);
            SerialNumberbytes[3] = Convert.ToByte(SerialNumber & 0x000000FF);
            SerialNumberHexString = SerialNumberbytes[0].ToString("X2") + SerialNumberbytes[1].ToString("X2") + SerialNumberbytes[2].ToString("X2") + SerialNumberbytes[3].ToString("X2");
        }
        public uint NextSerialNumber()
        {
            uint nextSN;
            nextSN = Convert.ToUInt32(SN,16);
            if (nextSN > 0xFFFFFFFF) throw new Exception_Yellow("Serial number in nextSN storage file is too large to continue.  You've made a lot of products!  Contact ThinkEco for assistance.");
            serialnumberlower3bytes = nextSN;
            nextSN += serialnumberprefix << 32;
            return nextSN;
        }



        //settings file strings
        public const string SettingsString_testingbool = "Testing?";
        public const string SettingsString_loadingbool = "Loading?";
        public const string SettingString_Frequency_counter_address = "Frequency Counter Address:"; //add 20120322 by Nino
        public const string SettingsString_USBUnderTestPID = "PID expected for USB under test (if unspecified, defaults to 04036001):";
        public const string SettingsString_RefRadioPID = "PID expected for reference radio dongle (if unspecified, defaults to 04036006):";
        public const string SettingsString_finalPID = "PID set on completion (if unspecified, defaults to 04038C80):";
        public const string SettingsString_trimmingbool = "Trimming crystal? (defaults to false, ignored if testing = false) (this line must be first of the trimming settings)";
        public const string SettingsString_ZTCfilename = "ZTC image filename:";
        public const string SettingsString_SSLfilename = "SSL bin filename:";
        public const string SettingsString_FWimagefilename = "Firmware image filename:";
        public const string SettingsString_takingFWfilenamefromsettingsfile = "Reading FW image filename from this settings file (true/false) (prompts for file selection if false):";
        public const string SettingsString_numberofradiotestsEachWay = "Number of radio tests in each direction:";
        public const string SettingsString_PIDtoavoidopening = "Throw an error if a test is attempted on a board with this PID (if unspecified, defaults to dummy value 99):";
        public const string SettingsString_settingdefaulttrimsbool = "-Setting default trim values?";
        public const string SettingsString_lookinguptrimsbool = "-Looking up trim values by MAC address in external log file?";
        public const string SettingsString_checkingfreqafternottrimmingbool = "-Measuring frequency once at the chosen trim values?";
        public const string SettingsString_listenlooptimeout = "Listen loop timeout (number of times to loop through 20-ms listen before timing out, default = 50):";

        public void ReadSettingsFile()
        {
            StreamReader SR;
            string S;
            SR = File.OpenText(Parameters.settingsfilepath);
            while (true)
            {
                S = SR.ReadLine();
                if (S.Contains("END SETTINGS FILE")) break;


                switch (S)
                {
                    case Parameters.SettingsString_testingbool:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//"))
                        {
                            if (S == "true") testing = true;
                            else if (S == "false") testing = false;
                            else throw new Exception_Yellow("The \"" + Parameters.SettingsString_testingbool + "\" line in settings file must say either true or false, no spaces.");
                        }
                        break;
                    case Parameters.SettingsString_loadingbool:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//"))
                        {
                            if (S == "true") loading = true;
                            else if (S == "false") loading = false;
                            else throw new Exception_Yellow("The \"" + Parameters.SettingsString_loadingbool + "\" line in settings file must say either true or false, no spaces.");
                        }
                        break;
                    case Parameters.SettingString_Frequency_counter_address: //Read Counter id from Setting file added 20120322 by Nino
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) counter_id = Convert.ToByte(S); 
                        break;
                    case Parameters.SettingsString_USBUnderTestPID:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) USB_under_test_pid = Convert.ToUInt32(S, 16);//Convert.ToUInt32(String.Format("{0:X}", Convert.ToUInt32(S)));
                        break;
                    case Parameters.SettingsString_RefRadioPID:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) reference_radio_pid = Convert.ToUInt32(S, 16);
                        break;
                    case Parameters.SettingsString_finalPID:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) finalNewFWPID = Convert.ToUInt32(S, 16);
                        break;
                    case Parameters.SettingsString_PIDtoavoidopening:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) PIDtoavoidopening = Convert.ToUInt32(S, 16);
                        break;
                    case Parameters.SettingsString_numberofradiotestsEachWay:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) numberofradiotestsEachWay = Convert.ToInt32(S);
                        break;
                    case Parameters.SettingsString_ZTCfilename:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) ZTCfilename = S;
                        break;
                    case Parameters.SettingsString_SSLfilename:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) SSLfilename = S;
                        break;
                    case Parameters.SettingsString_FWimagefilename:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) FWimagefilename = S;
                        break;
                    case Parameters.SettingsString_takingFWfilenamefromsettingsfile:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//"))
                        {
                            if (S == "true") takingFWfilenamefromsettingsfile = true;
                            else if (S == "false") takingFWfilenamefromsettingsfile = false;
                            else throw new Exception_Yellow("The \"" + Parameters.SettingsString_takingFWfilenamefromsettingsfile + "\" line in settings file must say either true or false, no spaces.");
                        }
                        break;
                    case Parameters.SettingsString_trimmingbool:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//"))
                        {
                            if (S == "true") crystaltrimming = true;
                            else if (S == "false") crystaltrimming = false;
                            else throw new Exception_Yellow("The \"" + Parameters.SettingsString_trimmingbool + "\" line in settings file must say either true or false, no spaces.");
                        }
                        break;
                    case Parameters.SettingsString_settingdefaulttrimsbool:
                        if (!crystaltrimming)
                        {
                            S = SR.ReadLine();
                            if (S != "" && !S.StartsWith("//"))
                            {
                                if (S.Contains("true")) settingdefaulttrimsbool = true;
                                else if (S.Contains("false")) settingdefaulttrimsbool = false;
                                else throw new Exception_Yellow("The \"" + Parameters.SettingsString_settingdefaulttrimsbool + "\" line in settings file must say either true or false, no spaces.");
                            }
                            SR.ReadLine();
                            S = SR.ReadLine();
                            coarsetrim_default = Convert.ToInt32(S.Substring(11, 2));
                            S = SR.ReadLine();
                            finetrim_default = Convert.ToInt32(S.Substring(9, 2));
                        }
                        break;
                    case Parameters.SettingsString_lookinguptrimsbool:
                        if (!crystaltrimming)
                        {
                            S = SR.ReadLine();
                            if (S != "" && !S.StartsWith("//"))
                            {
                                if (S.Contains("true")) lookinguptrimsbool = true;
                                else if (S.Contains("false")) lookinguptrimsbool = false;
                                else throw new Exception_Yellow("The \"" + Parameters.SettingsString_lookinguptrimsbool + "\" line in settings file must say either true or false, no spaces.");
                            }
                            S = SR.ReadLine();
                            trimsdatabasefilename = S.Substring(31, S.Length-31);
                        }
                        break;
                    case Parameters.SettingsString_checkingfreqafternottrimmingbool:
                        if (!crystaltrimming)
                        {
                            S = SR.ReadLine();
                            if (S != "" && !S.StartsWith("//"))
                            {
                                if (S.Contains("true")) checkingfreqafternottrimmingbool = true;
                                else if (S.Contains("false")) checkingfreqafternottrimmingbool = false;
                                else throw new Exception_Yellow("The \"" + Parameters.SettingsString_checkingfreqafternottrimmingbool + "\" line in settings file must say either true or false, no spaces.");
                            }
                        }
                        break;
                    case Parameters.SettingsString_listenlooptimeout:
                        S = SR.ReadLine();
                        if (S != "" && !S.StartsWith("//")) listenlooptimeout = Convert.ToInt32(S);
                        break;
                }
            }
            ZTCfilepath = Parameters.testingfilepath + "\\" + ZTCfilename;
            SSLfilepath = Parameters.loadingfilepath + "\\" + SSLfilename;
            FWimagefilepath = Parameters.loadingfilepath + "\\" + FWimagefilename;
            trimsdatabasefilepath = Parameters.mainfilepath + "\\" + trimsdatabasefilename;
            if (testing && !loading) FWimagefilename = "_";  //if testing only, doesn't make sense to record name of image loaded

            if (!testing) numberofradiotestsEachWay = 0;

            if (testing && loading) testingandorloading = "Testing+Loading";
            else if (testing) testingandorloading = "Testing_Only";
            else if (loading) testingandorloading = "Loading_Only";
            //else throw new Exception_Yellow("Program is set to neither test nor load this USB.  Change options in Settings.txt file.");
        }
        public string logfilestring()
        {
            string VID = ((finalNewFWPID & 0xFFFF0000)>>16).ToString("X");
            if (VID.Length == 3) VID = "0" + VID;
            string PID = (finalNewFWPID & 0x0000FFFF).ToString("X");

            return
                //TimeStamp.DayOfYear.ToString() + '\t' +
                TimeStamp.ToString() + '\t' +
                MAC + '\t' +
                VID + '\t' +
                PID + '\t' +
                FTDISerialNum + '\t' +
                totaltesttime + '\t' +
                TestSuiteVersionNumber + '\t' + 
                SerialNumberHexString + '\t' +
                "c " + coarsetrim_SET + '\t' +
                "f " + finetrim_SET + '\t' +
                frequency_measured + '\t' +
                TrimSequence;
            //fw filename, version number of this program, radiotest success % and number of tests, FTintTestingBoard.GetFinalCoarse(), FTintTestingBoard.GetFinalFine(), FinalFreq, TrimSeq);
        }

    }
    public class Exception_Yellow : System.Exception
    {
        // The default constructor needs to be defined
        // explicitly now since it would be gone otherwise.
        public Exception_Yellow()
        {
        }
        public Exception_Yellow(string message) : base(message)
        {
        }
        public Exception_Yellow(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    public class Exception_Red : System.Exception
    {
        // The default constructor needs to be defined
        // explicitly now since it would be gone otherwise.
        public Exception_Red()
        {
        }
        public Exception_Red(string message) : base(message)
        {
        }
        public Exception_Red(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
