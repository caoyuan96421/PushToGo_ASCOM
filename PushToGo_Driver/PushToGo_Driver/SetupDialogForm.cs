using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.PushToGo;
using ASCOM.PushToGo.Properties;

namespace ASCOM.PushToGo
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        private Telescope ts = null;

        public SetupDialogForm(Telescope telescope)
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();

            // If we're already connected
            if(telescope != null)
            {
                ts = telescope;

                textBoxLatitude.ReadOnly = false;
                textBoxLatitude.Text = new ASCOM.Utilities.Util().DegreesToDMS(ts.SiteLatitude);

                textBoxLongitude.ReadOnly = false;
                textBoxLongitude.Text = new ASCOM.Utilities.Util().DegreesToDMS(ts.SiteLongitude);

                textBoxGuideSpeed.ReadOnly = false;
                textBoxGuideSpeed.Text = (ts.GuideRateRightAscension/Telescope.sidereal_speed).ToString();

                buttonSyncTime.Enabled = true;

                // COM port cannot be changed when connected
                comboBoxComPort.Enabled = false;
            }
            else
            {
                textBoxLatitude.Text = "";
                textBoxLongitude.Text = "";
                textBoxGuideSpeed.Text = "";
            }
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            Settings.Default.Save();
            Close();
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Settings.Default.Reload();
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            // set the list of com ports to those that are currently available
            comboBoxComPort.Items.Clear();
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(Settings.Default.comPort))
            {
                comboBoxComPort.SelectedItem = Settings.Default.comPort;
            }

            textBoxFL.Text = Settings.Default.focalLength.ToString();
            textBoxAperture.Text = Settings.Default.aperture.ToString();
            textBoxArea.Text = Settings.Default.area.ToString();
            textBoxElevation.Text = Settings.Default.elevation.ToString();
            textBoxTemp.Text = Settings.Default.temperature.ToString();
        }

        private void textBoxFL_TextChanged(object sender, EventArgs e)
        {
            if (Double.TryParse(textBoxFL.Text, out double fl))
                Settings.Default.focalLength = fl;
        }

        private void textBoxAperture_TextChanged(object sender, EventArgs e)
        {
            if(Double.TryParse(textBoxAperture.Text, out double ap))
                Settings.Default.aperture = ap;
        }

        private void textBoxArea_TextChanged(object sender, EventArgs e)
        {
            if (Double.TryParse(textBoxArea.Text, out double area))
                Settings.Default.area = area;
        }

        private void textBoxElevation_TextChanged(object sender, EventArgs e)
        {
            if (Double.TryParse(textBoxElevation.Text, out double elev))
                Settings.Default.elevation = elev;
        }

        private void textBoxTemp_TextChanged(object sender, EventArgs e)
        {
            if (Double.TryParse(textBoxTemp.Text, out double t))
                Settings.Default.temperature = t;
        }

        private void textBoxLatitude_TextChanged(object sender, EventArgs e)
        {
            if (ts != null)
            {
                var util = new ASCOM.Utilities.Util();
                ts.SiteLatitude = util.DMSToDegrees(textBoxLatitude.Text);
                textBoxLatitude.Text = util.DegreesToDMS(ts.SiteLatitude);
            }
        }

        private void textBoxLongitude_TextChanged(object sender, EventArgs e)
        {
            if (ts != null)
            {
                var util = new ASCOM.Utilities.Util();
                ts.SiteLongitude = util.DMSToDegrees(textBoxLongitude.Text);
                textBoxLongitude.Text = util.DegreesToDMS(ts.SiteLongitude);
            }
        }

        private void textBoxGuideSpeed_TextChanged(object sender, EventArgs e)
        {
            if (ts != null && Double.TryParse(textBoxGuideSpeed.Text, out double gs) && gs > 0)
            {
                ts.GuideRateRightAscension = ts.GuideRateDeclination = gs * Telescope.sidereal_speed;
                textBoxGuideSpeed.Text = gs.ToString();
            }
        }

        private void buttonSyncTime_Click(object sender, EventArgs e)
        {
            if (ts != null)
            {
                ts.UTCDate = DateTime.UtcNow;
            }
        }
    }
}