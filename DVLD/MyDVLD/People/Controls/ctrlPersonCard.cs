using DVLD_Business;
using MyDVLD.Global_Classes;
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
namespace MyDVLD.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private int _PersonID = -1;
        public int PersonID { get { return _PersonID; } }
        private clsPerson _Person;
        public clsPerson SelectedPerson {  get { return _Person; } }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }
        public void ResetPersonInfo()
        {
            lblPersonID.Text = "[???]";
            lblFullName.Text= "[???]";
            lblNationalNo.Text = "[???]";
            lblGendor.Text = "[???]";
            lblAddress.Text = "[???]";
            lblPhone.Text = "[???]";
            lblEmail.Text= "[???]";
            pbGendorImage.Image = Resources.Man_32;
            lblDateOfBirth.Text= "[???]";
            lblCountry.Text= "[???]";
            pbPersonImage.Image = Resources.Male_512;
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if(_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("Coud Not Find Person With Person ID  = "+PersonID.ToString(),"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();
        }
        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("Coud Not Find Person With NationalNo  = " + NationalNo, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FillPersonInfo();
        }
        private void _FillPersonInfo()
        {
            llUpdatePersonInfo.Visible = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _PersonID.ToString();
            lblName.Text = _Person.FullName.ToString();
            lblNationalNo.Text = _Person.NationalNo.ToString();
            lblGendor.Text = _Person.Gendor == 0 ? "Male" : "Female";
            lblPhone.Text = _Person.Phone;
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = clsFormat.ConvertDateToShortString(_Person.DateOfBirth);
            lblCountry.Text = _Person.CountryInfo.CountryName;
            _LoadPersonImage();
           
        }

        private void _LoadPersonImage()
        {
            if(_Person.Gendor ==0)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
                pbGendorImage.Image = Resources.Woman_32;
            }

            string ImagePath = _Person.ImagePath;

           if(ImagePath !="")
           {
                if (File.Exists(ImagePath))
                {
                    pbPersonImage.ImageLocation = ImagePath;
                }
                else
                {
                    MessageBox.Show("Could Not Find This Image : " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
           }
                
        }
        private void llUpdatePersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();

            LoadPersonInfo(_PersonID);
        }
    }
}
