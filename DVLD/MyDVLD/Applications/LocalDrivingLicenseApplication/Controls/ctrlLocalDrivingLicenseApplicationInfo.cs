using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.Applications.LocalDrivingLicenseApplication.Controls
{
    public partial class ctrlLocalDrivingLicenseApplicationInfo : UserControl
    {
        private int _LocalDrivingLicenseID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public ctrlLocalDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }
        public void ResetDefaultValue()
        {
            lblLocalDrivingLicenseApplicationID.Text = "[???]";
            lblAppliedForLicense.Text = "[???]";
            lblPassedTests.Text = "0";
            ctrlApplicationBasicInfo1._ResetDeafultData();
        }

        private void _FillLocalDrivingLicenseAppInfo()
        {
            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblPassedTests.Text = _LocalDrivingLicenseApplication.GetPassedTests().ToString()+" /3";
            lblAppliedForLicense.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            llShowLicenseInfo.Enabled = true;
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);
        }
        public void LoadLocalDrivingLicenseApplicationInfo(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicenseApplication == null)
            {
                ResetDefaultValue();
                MessageBox.Show("There is No Local Driving License Application With Local Driving License Application ID  = "+_LocalDrivingLicenseID.ToString(),"Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            _FillLocalDrivingLicenseAppInfo();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet ","Not Implemented",MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
