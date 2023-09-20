using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace Push2Talk_NahIWantLeVoiceActivation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MMDeviceEnumerator enumlator = new MMDeviceEnumerator();
            var deviuces = enumlator.EnumerateAudioEndPoints(DataFlow.All,DeviceState.Active);
            
            comboBox1.Items.AddRange(deviuces.ToArray());
            foreach (MMDevice item in comboBox1.Items)
            {
                if (item.FriendlyName.ToLower().Contains("microphone"))
                {
                    comboBox1.Text = item.FriendlyName;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null )
            {
                var device = (MMDevice)comboBox1.SelectedItem;
                progressBar1.Value = (int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100));
                textBox1.Text = (Math.Round(device.AudioMeterInformation.MasterPeakValue * 100)).ToString();

                try
                {
                    if (((int)(Math.Round(device.AudioMeterInformation.MasterPeakValue * 100)) > double.Parse(textBox3.Text)))
                    {
                        activate();
                    }
                    else
                    {
                      
                        checkBox1.Checked = false;
                    }
                }
                catch (Exception)
                {
                }
            }
          
        }
        public void activate()
        {
            activemic = 3;
            breek = true;
            backgroundWorker1.RunWorkerAsync();
            
            
        }
        bool breek;
        int activemic = 0; 
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (activemic != 0)
            {
                if (!breek)
                {
                    break;
                }
                else
                {
                    
                    backgroundWorker1.ReportProgress(1);
                    activemic = activemic - 1;
                }
                breek = false;
            }
            if (activemic == 0)
            {
                backgroundWorker1.ReportProgress(0);
            }
            

        }
        public void PressUpKey()
        {
         

                InputSimulator dh = new InputSimulator();
                dh.Keyboard.KeyUp(VirtualKeyCode.END);
            
        }
        public void PressDownKey()
        {
            timer2.Enabled = true;
            timer2.Interval = 500;
            timeLeft = int.Parse(textBox2.Text);
            InputSimulator dh = new InputSimulator();
            dh.Keyboard.KeyDown(VirtualKeyCode.END);
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                PressDownKey();
                checkBox1.Checked = true;
            }
            else if (e.ProgressPercentage == 0)
            {
                checkBox1.Checked = false;
            }
        }
        int timeLeft;
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
            
                timeLeft = timeLeft - 1;
                label1.Text = timeLeft + " seconds";

            }
            else
            {
                PressUpKey();
            }
        }
    }
}
