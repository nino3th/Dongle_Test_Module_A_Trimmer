//History
// TE updated another version after released newly version program for 2 station test, this version project 
// program base was provided by Liteon NinoLiu which version number is v2.1.15, and then TE separated 2 part 
// Module A-Trimmer v1.2 and Module B-Test and Load v1.2
//==========================================================================================================
// 20120413 |  1.2.1   | Nino Liu   |  TE updated another version after released newly program
//---------------------------------------------------------------------------------------------------------
//==========================================================================================================

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace Dongle_Test_Suite_2._1
{
    public partial class MainForm : Form
    {
        Thread MainThread;
        Parameters parameters = new Parameters();
        delegate void StringParameterDelegate(string value);  //for invoking one thread from another, to change things on the screen
        delegate void IntParameterDelegate(int value);  //for invoking one thread from another, to change things on the screen
        FTDIdevice thisUSB;
        FTDIdevice referenceRadio;
        Exception yellow = new Exception();
        Exception red = new Exception();      

        public MainForm()
        {
            
            InitializeComponent();
            parameters = new Parameters();
            parameters.ReadSettingsFile();  //must happen after new parameters is built in order for settings to be saved as params

            if (parameters.testing && !parameters.loading) progressBar_overall.Maximum = 60;  //adjust progress bar for testing only case
            else progressBar_overall.Maximum = 100;

            bool refradioattached = true;
            bool counterattached = true;
            CheckConnections(ref counterattached, ref refradioattached);
            if(refradioattached && counterattached) UpdateOutputText("Welcome to the ThinkEco USB Tester Module A. Please attach the dongle to be tested and scan its bottom housing MAC address label to begin.");
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.TaskManagerClosing) return;
            if (parameters.testing)
            {
                try
                {
                    referenceRadio.Close();
                }
                catch (Exception exc)
                {
                    UpdateOutputText(exc.Message);
                }
            }


            if (e.CloseReason == CloseReason.WindowsShutDown) return;

        }

        public void RunButton_Click(object sender, EventArgs e)
        {
            UpdateOutputText("Process has begun.");
            UpdateColorDisplay("white");
            UpdateProgressBar_Overall(0);
            UpdateProgressBar_Detail(0);

            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            
            parameters = new Parameters();
            parameters.ReadSettingsFile();  //must happen after new parameters is built in order for settings to be saved as params
            if (parameters.testing) parameters.refradioplugged = true; //we need to re-set this to true because we've made a new set of parameters, and if testing is true but the ref radio isn't there, we would already have seen an error.
            //open dialog box for filename, if option is selected
            if (!parameters.takingFWfilenamefromsettingsfile)
            {
                CancelEventArgs e2 = new CancelEventArgs();
                parameters.FWimagefilepath = openFileDialog1_FileOk(sender, e2);
                int lastslash = parameters.FWimagefilepath.LastIndexOf('\\') + 1;
                parameters.FWimagefilename = parameters.FWimagefilepath.Substring(lastslash, parameters.FWimagefilepath.Length-lastslash);
            }

            thisUSB = new FTDIdevice(parameters, this); //must happen after settings file is read

            MainThread = new Thread(new ThreadStart(MainProcess));
            MainThread.Start();
        }

        public void MainProcess()      //THIS IS THE MAIN CODE THAT RUNS WHEN THE BUTTON IS CLICKED
        {
            int BufferErrorIncrementer = 0;
            GoToForBufferingErrorRetry:
            try
            {
                Setup();
                ReadCounterID();
                CheckRequirements();                                //check that necessary stuff is plugged in
                ReadBarcode(); //come back to this                  //read scanned barcode (MAC address) from text box
                parameters.SetSerialNumber(); 
                UpdateOutputText("Opening USB port...");
                thisUSB.OpenPort(parameters.USB_under_test_pid, 6000);    //open up the usb port

               
                enableResetLine();                                  //enable control of the reset line on the dongle by applying new EEPROM settings to the FTDI chip (takes a few sec)

                thisUSB.ChipReset();
                //load ssl using RAMLoader
                thisUSB.adjustBaudRate(Parameters.BaudRate_SSL);
                RAMLoader loadSSL = new RAMLoader(parameters, thisUSB, parameters.SSLfilepath, this);
                loadSSL.Run(true);
                thisUSB.adjustBaudRate(Parameters.BaudRate_SSL);  //increase the baud rate 8x, saves six seconds on loading (goes from 14 to 8)


                SSLInterface ssl = new SSLInterface(parameters, thisUSB);
                FirmwareLoader fwloader = new FirmwareLoader(parameters, thisUSB, this);
                fwloader.EraseSector24(ssl);

                SetCrystalTrim();                                   //program the trim adjustment values for the crystal clock (depending on options set in Settings.txt)

                fwloader.LoadTrimsOnly(ssl);

                thisUSB.Close();
                Finish();

                //items to only do once totally successful:
                Logger();
                UpdateProgressBar_Overall(progressBar_overall.Maximum);
                SaveSerialNumber();
                UpdateColorDisplay("green");
                UpdatePictureDisplay("pass");//add pic @20120402 by Nino Liu
                //add serial number @20120328 by nino
                if (parameters.testing && parameters.loading) UpdateOutputText("Test and load with MAC: " + parameters.MAC + '\n' + "Serial number: " + parameters.SN + '\n' +
                         "Spent time: " + parameters.totaltesttime + " seconds."+ '\n' + "Optimal Frequency: " + parameters.frequency_measured);
                else if (parameters.loading) UpdateOutputText("Loading  with MAC: " + parameters.MAC + '\n' + "Serial number: " + parameters.SN + '\n' +
                        "Spent time: " + parameters.totaltesttime + " seconds." + '\n' + "Optimal Frequency: " + parameters.frequency_measured);
                else
                    UpdateOutputText("Testing with MAC: " + parameters.MAC + '\n' + "Serial number: " + parameters.SN + '\n' + 
                        "Spent time: " + parameters.totaltesttime + " seconds." + '\n' + "Optimal Frequency: " + parameters.frequency_measured);
            }
            catch (Exception_Yellow e)
            {
                ExceptionHandler(e, "yellow");
            }
            catch (Exception_Red e)
            {
                ExceptionHandler(e, "red");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("supplied buffer is not big enough"))
                {
                    BufferErrorIncrementer++;
                    if (BufferErrorIncrementer > 3) Output.Text = "Persistent buffering error -- unplug and replug dongle and try again.  If error persists, dongle fails.";
                    else
                    {
                        Output.Text = "Buffering error -- restarting after 2 seconds.";
                        UpdateColorDisplay("orange");
                        System.Windows.Forms.Application.DoEvents();
                        System.Threading.Thread.Sleep(2000);
                        goto GoToForBufferingErrorRetry;
                    }
                }
                ExceptionHandler(e, "generic");
            }
        }

        public void ExceptionHandler(Exception ex, string category)
        {
            try
            {
                thisUSB.SetEEPROMaftererror();    //temp useful for debugging if there are write issues, avoids using FT_prog if program stops before SetEEPROM. But causes trouble if there is no board attached.
                thisUSB.Close();
            }
            catch(Exception ee){}

            Finish();
            SelectScannerInputBox("");
            RunButtonEnabled(1);
            if (ex.Message.Contains("GPIB") || ex.Message.Contains("handle is out of range"))
            {
                ex = new Exception_Yellow("Error involving the frequency counter or GPIB connection (" + ex.Message + " 2).  Confirm that the GPIB cable is plugged into both the computer and the frequency counter, or turn off crystal trimming in the settings file.");    
                category = "yellow";
            }
            UpdateOutputText(ex.Message);
            UpdateColorDisplay(category);
            UpdatePictureDisplay("fail");
            if (category == "yellow")
                AppendToOutputText("\n\nPlease unplug and re-plug USB and try test again.");
            else if (category == "red")
                AppendToOutputText("\n" +  "\n" +  "USB fails; please remove and discard.");
            else if (category == "generic")
            {
                if (ex.Message.Contains("NationalInstruments")) UpdateOutputText("Trimming error. Most likely cause is that this computer does not have the National Instruments GPIB software necessary to communicate with the frequency counter.  Disable trimming in the Settings.txt file to avoid this error.");
                AppendToOutputText("\n\nPlease unplug and re-plug USB and try test again.");
            }
            ErrorLogger(ex.Message);
        }

        public void Setup()
        {
            RunButtonEnabled(0);
            parameters.StartTime = new DateTime(2010, 1, 18);
            parameters.StartTime = DateTime.Now;
            parameters.MachineName = System.Environment.MachineName.Replace(' ', '_');
            //parameters.SetSerialNumber(); 
        }
        public void CheckRequirements()
        {
            //uint deviceCount = thisUSB.CountDevices();
            //if (deviceCount == 0) throw new Exception_Yellow("No devices attached.");
            //if (deviceCount == 1 && parameters.testing) throw new Exception_Yellow("Either reference board or USB under test is missing.");
            if (referenceRadio == null && parameters.testing) throw new Exception_Yellow("Reference radio is not responding; please re-start test program.");
            if (parameters.testing && !referenceRadio.isOpen())
            {
                referenceRadio.OpenPort(parameters.reference_radio_pid, 3000);
                parameters.refradioplugged = true;
            }
            //if (parameters.crystaltrimming && !parameters.testing) throw new Exception("Error in settings file: if crystal trimming is enabled, testing must also be enabled.");
        }
        public void CheckConnections(ref bool counterattached, ref bool refradioattached)
        {
            if (parameters.crystaltrimming)
            {
                try
                {
                    Trimmer_NI4882.testtrimmerisattached(parameters.counter_id);
                }
                catch (Exception exc)
                {
                    if (exc.Message.Contains("GPIB") || exc.Message.Contains("handle is out of range")) exc = new Exception_Yellow("Error involving the frequency counter or GPIB connection (" + exc.Message + " 1).  Confirm that the GPIB cable is plugged into both the computer and the frequency counter, or turn off crystal trimming in the settings file.");
                    UpdateOutputText(exc.Message);
                    counterattached = false;
                }
            }
            if (parameters.testing)
            {
                try
                {
                    referenceRadio = new FTDIdevice(parameters, this);
                    referenceRadio.OpenPort(parameters.reference_radio_pid, 3000);
                    parameters.refradioplugged = true;
                    UpdateProgressBar_Detail(0);
                }
                catch (Exception exc)
                {
                    UpdateOutputText(exc.Message);
                    refradioattached = false;
                }
            }
        }
        public void ReadCounterID()
        {
//            if (CounterID.Text.Length != null && CounterID.Text.Length <= 2 )
            if(parameters.counter_id != 0 )
            {
                UpdateOutputText("Set counter ID.");
                System.Threading.Thread.Sleep(50);
            }
            else
            {
                throw new Exception_Red("error: Please enter correct Counter ID !! ");
            }
            //counterid = Convert.ToByte(CounterID.Text.ToUpper());
        }
        public void ReadBarcode()
        {
            if (ScannerInputBox.Text.Length < 41)
            {
                UpdateOutputText("Please Scan Barcode.");
                SelectScannerInputBox("");
            }

            string Mac = null;
            string sn = null;
            string temp_mac = null;
            string CandidateMac = null;
            int x = 1200;
            while (x-- > 0)
            {
                DoEvents("");
                CandidateMac = ScannerInputBox.Text.ToUpper();  // Candidate mac address is whatever's in the text box (but change it to uppercase for consistency)

                if (CandidateMac.Length == 41)
                {                    
                    sn = CandidateMac.Substring(7, 8);                   
                    Mac = CandidateMac.Substring(16,16);
                    //temp_mac = CandidateMac.Substring(28, 4);//Store mac end code for simple oqc test                        
                    if (Mac.StartsWith(Parameters.MACheader))                        
                    {   
                        temp_mac = Mac.Substring(12,4);
                        break;                        
                    }                        
                    else throw new Exception_Yellow("Error: MAC address entered does not begin with the Liteon MAC header (0x80 0x4F 0x58).  Be careful not to type with the keyboard while using the barcode scanner.");
                }
                else
                {                    
                    if (CandidateMac.Length > 41) UpdateOutputText("More than 41 digits have been entered -- this MAC address cannot be correct.  Please erase and re-scan or re-type.");
                    System.Threading.Thread.Sleep(50);
                }
            }
            if (x <= 0) throw new Exception_Yellow("Timed out after waiting 1 minute for barcode to scan.  Try again.");
            parameters.MAC = Mac;
            parameters.SN = sn;
            parameters.Temp_mac = temp_mac;//for simple OQC test 
            UpdateOutputText("Barcode accepted.");
            UpdateProgressBar_Overall(5);
        }
        private void EraseDongleHW3SW2firmware()
        {
            UpdateOutputText("erasing standard HW3_SW2 firmware from dongle...");
            thisUSB.adjustBaudRate(57600);
            byte[] x = { Convert.ToByte('<'), Convert.ToByte('0'), Convert.ToByte('F'), Convert.ToByte('1'), Convert.ToByte('2'), Convert.ToByte('4'), Convert.ToByte('8'), Convert.ToByte('A'), Convert.ToByte('A'), Convert.ToByte('B'), Convert.ToByte('B'), Convert.ToByte('>') };
            thisUSB.WriteByte(x);
            System.Threading.Thread.Sleep(2000);
            thisUSB.adjustBaudRate(Parameters.BaudRate_UARTandTesting);
        }
        public void TestUSB()
        {
            UpdateProgressBar_Overall(32);
            UpdateProgressBar_Detail(0);
            UpdateOutputText("Resetting chip...");
            thisUSB.ChipReset();
            //thisUSB.resetporttemp();  for experimenting with putting usb into suspend mode for faster chipreset

            LoadTestingFirmware();
            //TrimCrystal();
            RadioTest();
            //Utils.SendZTCCommand(thisUSB, "95 0A 02 " + Trimmer.TrimPacketPrep(4) + Trimmer.TrimPacketPrep(4));
            //Utils.SendZTCCommand(thisUSB, "95 0A 02 " + Trimmer.TrimPacketPrep(24) + Trimmer.TrimPacketPrep(11));
        }
        private void LoadTestingFirmware()
        {
            thisUSB.adjustBaudRate(Parameters.BaudRate_UARTandTesting);
            //thisUSB.adjustBaudRate(Parameters.loadingBaudRate);  //increase the baud rate 8x, saves about 2 seconds on loading 
            UpdateOutputText("Loading testing firmware...");
            RAMLoader loadZTC = new RAMLoader(parameters, thisUSB, parameters.ZTCfilepath, this);
            loadZTC.Run(false);
            UpdateProgressBar_Overall(38);
            System.Threading.Thread.Sleep(10);                  //for some reason this is necessary or else radio test fails, don't know why :-/
            thisUSB.adjustBaudRate(Parameters.BaudRate_UARTandTesting);  //return to baud rate that the ZTC is written for for testing.  consider speeding ZTC up later and testing to see if it affects quality/speed.
        }
        private void SetCrystalTrim()
        {
            //pick our trim values, either by calculating them based on repeated frequency measurements and newton's method, or by looking them up in a database, or by choosing the default ones that are in the settings file.
            if (parameters.crystaltrimming)
            {
                System.Threading.Thread.Sleep(50);  //added in because first ZTC packet in trimming wasn't getting a response, prob bc ZTC was still booting
                UpdateOutputText("Calculating optimal crystal trim values...");
                Trimmer_NI4882 trimmer = new Trimmer_NI4882(parameters, thisUSB);  //trimmer_NI4882 uses the ssl to trim, not ztc.
                trimmer.Run();                      //Communicates with the frequency counter via GPIB connection to take frequency measurements and adjust the trim values until frequency is close to 12 MHz.
            }
            else if (parameters.lookinguptrimsbool)
            {
                LookupTrimValues();
            }
            else if (parameters.settingdefaulttrimsbool)
            {                
                SetDefaultTrimValues();
            }
            //if set to measure the freqency once (presumably after setting looked-up or default trims), do so.
            if (parameters.checkingfreqafternottrimmingbool)
            {
                Trimmer_NI4882 trimmer = new Trimmer_NI4882(parameters, thisUSB);
                trimmer.TakeSingleMeasurement();
            }


            parameters.TwoTrimBytes[0] = (byte)parameters.coarsetrim_SET;
            parameters.TwoTrimBytes[1] = (byte)parameters.finetrim_SET;

            UpdateProgressBar_Overall(47);
        }
        private void RadioTest()
        {

            UpdateOutputText("Testing radio communication quality...");
            RadioTester radiotest = new RadioTester(parameters, thisUSB, referenceRadio);
            parameters.radioTestsuccesses = radiotest.RunRadioTest(parameters.numberofradiotestsEachWay);
            parameters.radiotestsuccesspercent = 100 * parameters.radioTestsuccesses / (2 * parameters.numberofradiotestsEachWay);  //do i need to cast these as doubles?
            parameters.radiotestsuccesspercentstring = parameters.radiotestsuccesspercent.ToString();
            //referenceRadio.Close();  //possible to only do this on formclosing?  see comment above at opening

            UpdateProgressBar_Overall(55);
        }
        public void LoadFirmware()
        {
            UpdateOutputText("Resetting chip...");
            thisUSB.ChipReset();
            UpdateOutputText("Loading secondary stage loader...");
            FirmwareLoader loadFW = new FirmwareLoader(parameters, thisUSB, this);//, parameters.FWimagefilepath);
            loadFW.Run();
            //loadFW.LoadHardwareSettings();
            UpdateProgressBar_Overall(95);
        }
        public void Finish()
        {
            parameters.TimeStamp = new DateTime(2010, 1, 18);
            parameters.TimeStamp = DateTime.Now;
            System.TimeSpan diff = parameters.TimeStamp.Subtract(parameters.StartTime);
            double testtime = diff.TotalSeconds;
            if (testtime < 86400) parameters.totaltesttime = (Math.Round(testtime, 3)).ToString();  //don't record the test time if it doesn't make sense (which would happen if we hit an error before recording the StartTime)
            UpdateProgressBar_Overall(progressBar_overall.Maximum);
            UpdateProgressBar_Detail(100);
            SelectScannerInputBox("");
            RunButtonEnabled(1);
        }
        public void Logger()
        {
            UpdateOutputText("Saving test results to log...");

            //LOGGING
            using (StreamWriter writer = new StreamWriter(Parameters.logfilepath, true))
            {
                writer.WriteLine(parameters.logfilestring());
                //writer.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t{10}\tc\t{11}\tf\t{12}\t{13}\t{14}", 
                //    parameters.TimeStamp.Date, parameters.TimeStamp.TimeOfDay, parameters.MAC, VID, PID, parameters.FTDISerialNum, parameters.testingandorloading, 
                //    parameters.totaltesttime, parameters.MachineName, parameters.FWimagefilename, parameters.radio, FTintTestingBoard.GetFinalCoarse(), FTintTestingBoard.GetFinalFine(), FinalFreq, TrimSeq);
                ////fw filename, version number of this program, radiotest success % and number of tests, 
            }
            using (StreamWriter writer = new StreamWriter(Parameters.backuplogfilepath, true))
            {
                writer.WriteLine(parameters.logfilestring());
            }
        }
        public void ErrorLogger(string exceptionmessage)
        {
            using (StreamWriter writer = new StreamWriter(Parameters.errorlogfilepath, true))
            {
                writer.WriteLine(parameters.logfilestring()+'\t'+exceptionmessage.Replace(' ','_'));
            }
        }

        public void enableResetLine()
        {
                UpdateProgressBar_Overall(5);
                if (!thisUSB.resetLineIsEnabled_pluschecks())  //reads eeprom and checks if CBUS3 reset line needs to be enabled or not, also checks prior testing info - status and mac
                {
                    UpdateOutputText("Enabling chip reset line...");
                    thisUSB.enableResetLine();
                        UpdateProgressBar_Overall(8);
                        UpdateProgressBar_Detail(20);
                    thisUSB.PortCycle();
                        UpdateProgressBar_Overall(14);
                        UpdateProgressBar_Detail(50);
                    System.Threading.Thread.Sleep(500);  //necessary?
                        UpdateProgressBar_Overall(19);
                        UpdateProgressBar_Detail(100);
                    thisUSB.OpenPort(parameters.USB_under_test_pid, 8000);
                        UpdateProgressBar_Overall(30);
                }
                else
                { }
        }
        private string openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filepathFromDialog = "";
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Parameters.loadingfilepath;
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            filepathFromDialog = openFileDialog1.FileName;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
            return filepathFromDialog;
        }
        public void LookupTrimValues()
        {
            StreamReader SR;
            string S;
            SR = File.OpenText(parameters.trimsdatabasefilepath);
            S = SR.ReadToEnd();
            int coarseindex, fineindex;
            try
            {
                int thisMACindex = S.IndexOf(parameters.MAC);
                coarseindex = S.IndexOf("c", thisMACindex, 120) + 2;  //within the next 145 characters, look for "c" and use it to find coarse trim
                fineindex = S.IndexOf("f", thisMACindex, 120) + 2;      //these will throw some kind of count error if we're too close to the end of the file.  "Count must be positive and count must refer to a location within the string/array/collection."
            }
            catch (System.ArgumentOutOfRangeException exce)
            {
                throw new Exception_Yellow("No record of this MAC address was found in the trim database file (or this is a short final record).");
            }
            //fix the following once log file format is finalized. ****************
            parameters.coarsetrim_SET = Convert.ToInt16(S.Substring(coarseindex, 2));
            parameters.finetrim_SET = Convert.ToInt16(S.Substring(fineindex, 2));
        }
        public void SetDefaultTrimValues()
        {
            parameters.coarsetrim_SET = parameters.coarsetrim_default;
            parameters.finetrim_SET = parameters.finetrim_default;
        }
        public void SaveSerialNumber()
        {
            uint nextSNafterthisone = parameters.serialnumberlower3bytes + 1;
            using (StreamWriter s = new StreamWriter(Parameters.nextSNfilepath, false)) //overwrite mode
            {
                s.WriteLine(nextSNafterthisone.ToString());
            }
        }

        //functions updating window appearance:
        public void UpdateOutputText(string value)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(UpdateOutputText), new object[] { value });
                return;
            }
            // Must be on the UI thread if we've got this far
            Output.Text = value;
            System.Windows.Forms.Application.DoEvents();
        }
        public void AppendToOutputText(string value)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(AppendToOutputText), new object[] { value });
                return;
            }
            // Must be on the UI thread if we've got this far
            Output.Text += value;
            System.Windows.Forms.Application.DoEvents();
        }
        public void DoEvents(string value)  //just for keeping window alive and closeable
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(DoEvents), new object[] { value });
                return;
            }
            // Must be on the UI thread if we've got this far
            System.Windows.Forms.Application.DoEvents();
        }

        public void SelectScannerInputBox(string value)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(SelectScannerInputBox), new object[] { value });
                return;
            }
            // Must be on the UI thread if we've got this far
            ScannerInputBox.Select();
            ScannerInputBox.Text = "";
            System.Windows.Forms.Application.DoEvents();
        }
        public void UpdateColorDisplay(string color)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(UpdateColorDisplay), new object[] { color });
                return;
            }
            // Must be on the UI thread if we've got this far
            if (color == "white") StatusIndicator.BackColor = System.Drawing.Color.White;
            else if (color == "red") StatusIndicator.BackColor = System.Drawing.Color.Red;
            else if (color == "yellow") StatusIndicator.BackColor = System.Drawing.Color.Yellow;
            else if (color == "green") StatusIndicator.BackColor = System.Drawing.Color.LimeGreen;
            else if (color == "orange") StatusIndicator.BackColor = System.Drawing.Color.Orange;
            else StatusIndicator.BackColor = System.Drawing.Color.MediumOrchid;
            System.Windows.Forms.Application.DoEvents();
        }
        public void UpdatePictureDisplay(string value) //add @20120402 by Nino Liu
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new StringParameterDelegate(UpdatePictureDisplay), new object[] { value });
                return;
            }
            if (value == "pass")
            {
                pictureBox1.Image = new Bitmap("Pass.jpg");
                pictureBox1.Visible = true;
            }
            else if (value == "fail")
            {
                pictureBox2.Image = new Bitmap("Fail.jpg");
                pictureBox2.Visible = true;
            }
        }
        public void UpdateProgressBar_Detail(int progress)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new IntParameterDelegate(UpdateProgressBar_Detail), new object[] { progress });
                return;
            }
            // Must be on the UI thread if we've got this far
            progressBar_detail.Value = progress;
            System.Windows.Forms.Application.DoEvents();
        }
        void UpdateProgressBar_Overall(int progress)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new IntParameterDelegate(UpdateProgressBar_Overall), new object[] { progress });
                return;
            }
            // Must be on the UI thread if we've got this far
            progressBar_overall.Value = progress;
            System.Windows.Forms.Application.DoEvents();
        }
        void RunButtonEnabled(int enabled)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke(new IntParameterDelegate(RunButtonEnabled), new object[] { enabled });
                return;
            }
            // Must be on the UI thread if we've got this far
            if (enabled == 1) RunButton.Enabled = true;
            else RunButton.Enabled = false;
            System.Windows.Forms.Application.DoEvents();
        }
    }
}
