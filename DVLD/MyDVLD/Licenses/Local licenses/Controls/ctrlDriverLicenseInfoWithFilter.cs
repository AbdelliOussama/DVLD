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

namespace MyDVLD.Licenses.Local_licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int>Handler = OnLicenseSelected;
            if(Handler!=null)
            {
                Handler(LicenseID);
            }
        }
        private bool _FilterEnabled = true; 
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilter.Enabled =_FilterEnabled;
            }
        }

        private int _LicenseID = -1;
        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }
        public clsLicense SelectedLicense
        {
            get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; }
        }
        public void LoadLicenseInfo(int LicenseID)
        {
            txtLicenseID.Text =LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadLicenseInfo(LicenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;
            if(OnLicenseSelected!=null && FilterEnabled)
            {
                OnLicenseSelected(LicenseID);
            }
        }


        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if(e.KeyChar ==(char)13)
            {
                btnFind.PerformClick();
            }
        }

        public void TxtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }
        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This Feild Is Required ");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, null);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Feild Are Wrong Put The Mouse Over The Red Icon To See The Error ","Validation Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                TxtLicenseIDFocus();
                return;
            }
            _LicenseID = int.Parse(txtLicenseID.Text.Trim());
            LoadLicenseInfo(_LicenseID);
        }
    }
}
