using DVLD_Business;
using MyDVLD.Global_Classes;
using MyDVLD.Licenses;
using MyDVLD.Licenses.International_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.Applications.International_License_Application
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        private int _InternationalLicenseID = -1;
        private int _SelectedLicesneID = -1;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.ConvertDateToShortString(DateTime.Now);
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblExpirationDate.Text = clsFormat.ConvertDateToShortString(DateTime.Now.AddYears(1));
            lblIssueDate.Text = lblApplicationDate.Text;
        }
        private void frmNewInternationalLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.TxtLicenseIDFocus();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicense.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsInternationalLicense internationalLicense = new clsInternationalLicense();
            internationalLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicense.DriverInfo.PersonID;
            internationalLicense.ApplicationDate = DateTime.Now;
            internationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Cancelled;
            internationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).ApplicationFees;
            internationalLicense.LastStatusDate = DateTime.Now;
            internationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            internationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicense.DriverID;
            internationalLicense.IssueDate = DateTime.Now;
            internationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            internationalLicense.IssedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicense.LicenseID;

            if(!internationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblApplicationID.Text = internationalLicense.ApplicationID.ToString();
            _InternationalLicenseID = internationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text = _InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + internationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnIssueLicense.Enabled = false;
            llShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected_1(int obj)
        {
            _SelectedLicesneID = obj;
            lblLocalLicenseID.Text = _SelectedLicesneID.ToString();
            llShowLicenseHistory.Enabled = (_SelectedLicesneID != -1);
            if (_SelectedLicesneID == -1)
            {
                return;
            }
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicense.LicenseClass != 3)
            {
                MessageBox.Show("Selected License should be Class 3, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int ActiveInternationalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicense.DriverID);
            if (ActiveInternationalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternationalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternationalLicenseID;
                btnIssueLicense.Enabled = false;
                return;
            }
            btnIssueLicense.Enabled = true;
        }
    }
}
