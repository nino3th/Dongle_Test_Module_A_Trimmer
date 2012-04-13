using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NationalInstruments.NI4882;
using FTD2XX_NET;

//using System;
//using System.Drawing;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Data;
//using System.Linq;
//using System.Text;
////using System.Web;
//using NationalInstruments.NI4882;
//using USBClassLibrary;
//using FTD2XX_NET;
//using System.IO;
//using System.Diagnostics;
//using System.Text.RegularExpressions;



namespace Dongle_Test_Suite_2._1
{
    class Trimmer_NI4882
    {
        Parameters parameters;
        FTDIdevice thisUSB;
        Device FREQ_COUNTER;

        //move some of these to the parameters class
        //private double FreqAtCoarse8;
        private double FinalFreq;
        private int FinalCoarse;
        private int FinalFine;
        private string TrimSequence = "";
        //private string filepath;
        //private string filename;
        private int previouscoarseforchecking = -12345;
        private double previousfineforchecking = -12345;
        private double previousfreqforchecking = -12345;
        double finetrimsizeinHz = -16.2;   //experimentally determined avg # of Hz one fine trim changes clock by
        double targetfrequency = 12000000;

        public Trimmer_NI4882(Parameters pars, FTDIdevice USB)
        {
            parameters = pars;
            thisUSB = USB;
            FREQ_COUNTER = new Device(0, 9, 0); // create an object to hold the frequency counter, with GPIB ID 0, primary addr 9, and no secondary addr.  the primary addr was manually set on the counter itself.
        }
        public void Run()
        {
            NewTrimmer(thisUSB, 10, 14, 5); //8, 22, 30); //10, 14, 5

            parameters.coarsetrim_SET = FinalCoarse;
            parameters.finetrim_SET = FinalFine; 
            parameters.frequency_measured = FinalFreq;
            parameters.TrimSequence = TrimSequence;
        }
        public void NewTrimmer(FTDIdevice thisUSB, int coarse, double finetrim, double backupfinetrim)//    newtrimmer(initialcoarse, initialtrim1, initialtrim2)
        {
            SSLInterface ssl = new SSLInterface(parameters, thisUSB);

            thisUSB.poll();  //clear out anything sitting on the port

            double fine1 = finetrim;        //should be the mode of the trimming results
            double fine2 = backupfinetrim;  //if newton's method doesn't get it on the first try we use secant method with this backupfinetrim and finetrim
            bool done = false;
            double temp;
            double freq2 = -12345;  //dummy value to avoid error
            double freq1 = FreqAtTrim(thisUSB, ssl, coarse, Convert.ToInt32(fine1));
            checkFreqValidity(fine1, ref freq1, coarse, thisUSB, FREQ_COUNTER, ssl);
            if (freqWithin8(coarse, fine1, freq1)) done = true;
            TrimSequence += (" | " + coarse.ToString() + " " + Math.Round(fine1, 3).ToString() + " " + freq1.ToString());
            if (!done) // this if statement is here just in case we got it right the first time
            {
                temp = fine1 - (freq1 - targetfrequency) / finetrimsizeinHz;    //use Newton's method to find the second point, so we finish after 2 measurements more often
                if (temp <= 31 && temp >= 0) fine2 = temp;                      //but only use it if it gives a value between 0 and 31, otherwise revert to input fine2
                freq2 = FreqAtTrim(thisUSB, ssl, coarse, Convert.ToInt32(fine2));         // in the future, could adjust this to save a step when adjusting coarse, but we expect to adjust coarse only very rarely
                checkFreqValidity(fine2, ref freq2, coarse, thisUSB, FREQ_COUNTER, ssl);
                if (freqWithin8(coarse, fine2, freq2)) done = true;
                TrimSequence += (" | " + coarse.ToString() + " " + Math.Round(fine2,3).ToString() + " " + freq2.ToString());
            }
            while (!done && Math.Abs(fine2 - fine1) > 1)  //while not done and trims are more than 1 apart
            {

                temp = fine2 - ((freq2 - targetfrequency) * (fine2 - fine1)) / (freq2 - freq1);  //use the secant method
                fine1 = fine2;
                fine2 = temp;
                freq1 = freq2;
                int intfine2 = Convert.ToInt16(fine2);
                if (intfine2 <= 31 && intfine2 >= 0)
                {
                    //temp = fine2 - ((freq2 - targetfrequency) * (fine2 - fine1)) / (freq2 - freq1);  //use the secant method
                    //fine1 = fine2;
                    //fine2 = temp;
                    //freq1 = freq2;
                    freq2 = FreqAtTrim(thisUSB, ssl, coarse, Convert.ToInt32(fine2));
                    checkFreqValidity(fine2, ref freq2, coarse, thisUSB, FREQ_COUNTER, ssl);
                    if (freqWithin8(coarse, fine2, freq2)) done = true;
                    TrimSequence += (" | " + coarse.ToString() + " " + Math.Round(fine2,3).ToString() + " " + freq2.ToString());
                }
                else if (intfine2 > 31)
                {
                    if (intfine2 > 36)
                    {
                        while (freq2 > targetfrequency && coarse < 31)
                        {
                            coarse++; //could replace this with coarse += (intfine2-31)/7 if we ever saw cases of needing to adjust by >1 coarse trim
                            if (coarse == 16) coarse = 28;  //accounting for the overlap in coarse trim values due to the 5th cap being only 4 pf
                            freq2 = FreqAtTrim(thisUSB, ssl, coarse, 31);
                            checkFreqValidity(fine2, ref freq2, coarse, thisUSB, FREQ_COUNTER, ssl);
                            if (freqWithin8(coarse, fine2, freq2)) done = true;
                            TrimSequence += (" | " + coarse.ToString() + " " + Math.Round(fine2,3).ToString() + " " + freq2.ToString());
                        }
                    }
                    else coarse++;
                    NewTrimmer(thisUSB, coarse, 31, 24);
                    done = true;
                }
                else if (intfine2 < 0)
                {
                    if (intfine2 < -5)
                    {
                        while (freq2 < targetfrequency && coarse > 0)
                        {
                            coarse--; //could replace this with coarse += (intfine2)/7 if we ever saw cases of needing to adjust by >1 coarse trim
                            freq2 = FreqAtTrim(thisUSB, ssl, coarse, 0);
                            checkFreqValidity(fine2, ref freq2, coarse, thisUSB, FREQ_COUNTER, ssl);
                            if (freqWithin8(coarse, fine2, freq2)) done = true;
                            TrimSequence += (" | " + coarse.ToString() + " " + Math.Round(fine2,3).ToString() + " " + freq2.ToString());
                        }
                    }
                    else coarse--;
                    NewTrimmer(thisUSB, coarse, 0, 7);
                    done = true;
                }
            }
            if (!done)  //(could have become done from coarse changing / recursive call; this code is only reached if we've finished trimming with fine trims 1 away from each other but freqs not within 8)
            {
                //if we've trimmed as well as possible but no frequency was within 8 Hz, choose which one was closer (and if it was fine1, re-set that trim).
                if (Math.Abs(freq1 - targetfrequency) < Math.Abs(freq2 - targetfrequency))
                {
                    //FTinti.Command(FTinti, "95 0A 02 " + FTinti.TrimPacketPrep(Convert.ToInt32(fine1)) + FTinti.TrimPacketPrep(coarse));
                    FinishTrim(coarse, fine1, freq1);
                }
                else FinishTrim(coarse, fine2, freq2);
            }
        }
        public bool freqWithin8(int coarse, double fine, double freq)
        {
            //double targetfrequency = 12000000;
            //double finetrimsizeinHz = 16;
            if (Math.Abs(freq - targetfrequency) < .5 * Math.Abs(finetrimsizeinHz))
            {
                FinishTrim(coarse, fine, freq);
                return true;
            }
            else return false;
        }
        public void FinishTrim(int coarse, double fine, double freq)
        {
            FinalCoarse = coarse;
            FinalFine = Convert.ToInt16(fine);
            FinalFreq = freq;
        }
        public void checkFreqValidity(double fine, ref double freq, int coarse, FTDIdevice thisUSB, Device device, SSLInterface ssl)
        {
            //makes sure freq isn't out of range or a duplicate measurement, using the following general procedure: if it's invalid, measure it again; if it's still invalid, throw error.
            if (freq > 12001400 || freq < 11998000) // if out of range of trimming
            {
                //first, try again
                freq = FreqAtTrim(thisUSB, ssl, coarse, Convert.ToInt32(fine));
                //if freq is wildly off, probably unseated
                if (freq > 12010000 || freq < 11990000) throw new Exception_Yellow("Frequency counter range error (coarse " + coarse.ToString() + " fine " + fine.ToString() + " " + freq.ToString() + "). Please ensure that board is properly seated and try again.  If error persists, testing fails.");
                if (freq > 12001400 || freq < 11998000) throw new Exception_Red("Crystal frequency is outside acceptable range (coarse " + FinalCoarse.ToString() + " fine " + FinalFine.ToString() + "). Testing fails. Insert a new board and press \"Run Test\" or Enter.");  // freqency is too far from 12 MHz to be trimmed
            }    //        (make sure that this catches a null freq)
            //if this frequency is more than twice as close as we'd expect to the previous measurement, probably unseated:
            if (previouscoarseforchecking != -12345) //if this is not the first time we're running a check
            {
                if (coarse == previouscoarseforchecking && Convert.ToInt32(fine) != Convert.ToInt32(previousfineforchecking))  //if fine trims can be compared on their own
                {
                    if ((freq - previousfreqforchecking) / (finetrimsizeinHz * (fine - previousfineforchecking)) < .5)
                    {
                        freq = FreqAtTrim(thisUSB, ssl, coarse, Convert.ToInt32(fine));
                        if ((freq - previousfreqforchecking) / (finetrimsizeinHz * (Convert.ToInt32(fine) - Convert.ToInt32(previousfineforchecking))) < .5) throw new Exception_Yellow("Frequency counter repeat or anomaly error (coarse " + coarse.ToString() + " fine " + fine.ToString() + " " + freq.ToString() + "). Please ensure that board is properly seated and try again.  If error persists, testing fails.");
                    }
                }
                else if (Math.Abs(freq - previousfreqforchecking) < .3 * finetrimsizeinHz)
                {
                    freq = FreqAtTrim(thisUSB, ssl, coarse, Convert.ToInt32(fine));
                    if (Math.Abs(freq - previousfreqforchecking) < .3 * finetrimsizeinHz) throw new Exception_Yellow("Frequency counter repeat or anomaly error (coarse " + coarse.ToString() + " fine " + fine.ToString() + " " + freq.ToString() + "). Please ensure that board is properly seated and try again.  If error persists, testing fails.");
                }
            }
            previousfreqforchecking = freq;
            previousfineforchecking = fine;
            previouscoarseforchecking = coarse;
        }
        public double FreqAtTrim(FTDIdevice thisUSB, SSLInterface ssl, int coarse, int fine) //checks frequency over GPIB at given trim values
        {
            double freq = 0;
            //setting trim when talking to SSL:
            try
            {
                ssl.SSL_SetTrims((byte)fine, (byte)coarse);
            }
            catch (Exception_Yellow exc)
            {
                if (!exc.Message.Contains("retry failed")) throw new Exception_Yellow(exc.Message); //if it's some other exception, don't catch it
                else ssl.SSL_SetTrims((byte)fine, (byte)coarse); //retry once, if the exception comes back again, we won't catch it and it'll go on to be displayed as "retry failed"
            }
            //unused code for setting trim if talking to ZTC:
            //Utils.SendZTCCommand(thisUSB, "95 0A 02 " + TrimPacketPrep(fine) + TrimPacketPrep(coarse));
            //    Utils.WaitForZTCResponse(thisUSB, parameters);
            ////System.Threading.Thread.Sleep(as long as settling takes);


            FREQ_COUNTER.Write(":MEASURE:FREQ? 12, 0.0000001");
            //FREQ_COUNTER.Write(":meas:freq? 12,0.0000001"); 

            //System.Threading.Thread.Sleep(2000);
            freq = Convert.ToDouble(FREQ_COUNTER.ReadString());
            return freq;
        }
        public void TakeSingleMeasurement()
        {
            FREQ_COUNTER.Write(":meas:freq? 12,0.0000001");
            //System.Threading.Thread.Sleep(2000);
            parameters.frequency_measured = Convert.ToDouble(FREQ_COUNTER.ReadString());
        }
        public static string TrimPacketPrep(int i)
        {
            string s = i.ToString("X");
            if (s.Length < 2) s = "0" + s;
            return s;
        }
        public static void testtrimmerisattached(byte counterid) //add by nino @20120309
        {
            Device d = new Device(0, counterid, 0);
            //d.Write(":meas:freq? 12,0.0000001");
            d.Write(":MEASURE:FREQ? 12,0.0000001");
        }
    }
}
