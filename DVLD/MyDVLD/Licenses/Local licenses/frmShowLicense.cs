using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.Licenses.Local_licenses
{
    public partial class frmShowLicense : Form
    {
        private int _LicenseID = -1;

        public frmShowLicense(int licenseID)
        {
            InitializeComponent();
            _LicenseID = licenseID; 
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowLicense_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfo1.LoadLicenseInfo(_LicenseID);
        }
    }
}
