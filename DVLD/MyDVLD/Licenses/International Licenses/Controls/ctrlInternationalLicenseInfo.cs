using DVLD_Business;
using MyDVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MyDVLD.Global_Classes;
namespace MyDVLD.Licenses.International_Licenses.Controls
{
    public partial class ctrlInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID = -1;
        private clsInternationalLicense _InternationalLicense;
        public ctrlInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        private void _LoadPersonImage()
        {
            if(_InternationalLicense.DriverInfo.PersonInfo.Gendor ==0)
            {
                pbGendor.Image = Resources.Man_32;
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbGendor.Image = Resources.Woman_32;
                pbPersonImage.Image= Resources.Female_512;
            }
            string ImagePath = _InternationalLicense.ApplicantPersonInfo.ImagePath;
            if( ImagePath!="" )
            {
                if(File.Exists(ImagePath))
                {
                    pbPersonImage.Load(ImagePath);
                }
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        public void LoadLicenseInfo(int InternationalLicenseID)
        {
            _InternationalLicenseID  = InternationalLicenseID;
            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseID);
            if( _InternationalLicense == null )
            {
                MessageBox.Show($"No InternationalLicensse With International LicesneID = {InternationalLicenseID} ","Not Found",MessageBoxButtons.OK, MessageBoxIcon.Error);
                InternationalLicenseID = -1;
                return;
            }
            lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
            lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
            lblIsActive.Text = _InternationalLicense.IsActive ? "Yes" : "No";
            lblLocalLicenseID.Text = _InternationalLicense.IssedUsingLocalLicenseID.ToString();
            lblFullName.Text = _InternationalLicense.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = _InternationalLicense.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = _InternationalLicense.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblDateOfBirth.Text = clsFormat.ConvertDateToShortString(_InternationalLicense.DriverInfo.PersonInfo.DateOfBirth);

            lblDriverID.Text = _InternationalLicense.DriverID.ToString();
            lblIssueDate.Text = clsFormat.ConvertDateToShortString(_InternationalLicense.IssueDate);
            lblExpirationDate.Text = clsFormat.ConvertDateToShortString(_InternationalLicense.ExpirationDate);

            _LoadPersonImage();
        }



    }
}
