using DVLD_Business;
using MyDVLD.Global_Classes;
using MyDVLD.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private int _ApplicationID = -1;
        public int ApplicationID
        {
            get { return _ApplicationID; }
        }
        private clsApplication _Application;
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }
        public void _ResetDeafultData()
        {
            _ApplicationID = -1;
            lblApplicationID.Text = "[???]";
            lblStatus.Text = "[???]";
            lblFees.Text= "[???]";
            lblType.Text = "[???]";
            lblApplicant.Text = "[???]";
            lblDate.Text = "[???]";
            lblStatusDate.Text = "[???]";
            lblCreatedByUser.Text = "[???]";
        }
        private void _FillData()
        {
            _ApplicationID = _Application.ApplicationID;
            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblStatus.Text = _Application.ApplicationStatus.ToString();
            lblFees.Text = _Application.PaidFees.ToString();
            lblType.Text = _Application.ApplicationTypeInfo.ApplicationTypeTitle;
            lblApplicant.Text = _Application.ApplicantPersonInfo.FullName;
            lblDate.Text = clsFormat.ConvertDateToShortString(_Application.ApplicationDate);
            lblStatusDate.Text = clsFormat.ConvertDateToShortString(_Application.LastStatusDate); ;
            lblCreatedByUser.Text = _Application.CreatedByUserInfo.UserName;
            llViewPersonInfo.Enabled = true;
        }
        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplication.Find(ApplicationID);
            if(_Application == null )
            {
                _ResetDeafultData();
                MessageBox.Show("No Application With Application ID = "+ApplicationID.ToString(),"Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            _FillData();
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_Application.ApplicantPersonID);
            frm.ShowDialog();
            LoadApplicationInfo(_ApplicationID);
        }
    }
}
