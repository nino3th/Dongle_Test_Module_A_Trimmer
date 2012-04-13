using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Dongle_Test_Suite_2._1
{

    public class ContinuousProgressBar : ProgressBar
    {
        public ContinuousProgressBar()
        {
            this.Style = ProgressBarStyle.Continuous;
        }
        protected override void CreateHandle()
        {
            base.CreateHandle();
            try { SetWindowTheme(this.Handle, "", ""); }
            catch { }
        }
        [System.Runtime.InteropServices.DllImport("uxtheme.dll")]
        private static extern int SetWindowTheme(IntPtr hwnd, string appname, string idlist);
    } 
}
