using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business;
using MyDVLD.Properties;
using System.IO;
using MyDVLD.Global_Classes;

namespace MyDVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public delegate void DataBackEventHandeler(object sender, int PersonID);
        public event DataBackEventHandeler DataBack;

        public enum enMode { AddNew = 0,Update = 1 }
        private enMode _Mode;

        public enum enGendor { Male = 0, Female = 1 }

        private int _PersonID = -1;

        private clsPerson _Person;
        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _PersonID = PersonID;
        }
        private void _FillCountriesInComboBox()
        {
            DataTable dt = new DataTable();
            dt = clsCountry.GetAllCountries();
            foreach (DataRow row in dt.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetDefaultValue()
        {
            _FillCountriesInComboBox();
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
                lblTitle.Text = "Update Person";

            if (rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
                pbPersonImage.Image = Resources.Female_512;
            llRemoveImage.Visible = (pbPersonImage.ImageLocation!=null);
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtAddress.Text = "";
            txtEmail.Text = "";
            rbMale.Checked = true;
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            txtPhone.Text = "";
            cbCountry.SelectedIndex = cbCountry.FindString("Tunisia");
        }
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);
            if(_Person == null)
            {
                MessageBox.Show("There is No Person With Person ID = " + _PersonID.ToString(),"Person Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                txtNationalNo.Text = _Person.NationalNo.ToString();
                txtFirstName.Text = _Person.FirstName;
                txtSecondName.Text= _Person.SecondName;
                txtThirdName.Text= _Person.ThirdName;
                txtLastName.Text= _Person.LastName;
                txtAddress.Text = _Person.Address;
                txtEmail.Text = _Person.Email;
                txtPhone.Text = _Person.Phone;
                dtpDateOfBirth.Value = _Person.DateOfBirth;
                cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);
                if(_Person.Gendor ==0)
                {
                    rbMale.Checked = true;
                }
                else
                {
                    rbFemale.Checked = true;
                }
                if(_Person.ImagePath!=null)
                {
                    pbPersonImage.ImageLocation = _Person.ImagePath;
                }
                llRemoveImage.Visible = (_Person.ImagePath !="");

            }
        }
        

        /// <summary>
        /// Handel The Person Image
        /// </summary>
        /// <returns></returns>
        private bool HandelPersonImage()
        {
            if(_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if(_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(pbPersonImage.ImageLocation);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("Fail To Delete Image");
                    }
                }
                if(pbPersonImage.ImageLocation!= null)
                {
                    string SourceFile = pbPersonImage.ImageLocation.ToString();
                    if(clsUtil.CopyImageToImageFolder(ref SourceFile))
                    {
                        pbPersonImage.ImageLocation = SourceFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error While  Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Field Are Wrong Put The Mouse Over The Red Icon To See The Error "," Validation Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            if(!HandelPersonImage())
            {
                return;
            }
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            if(rbMale.Checked)
            {
                _Person.Gendor = (int)enGendor.Male;
            }
            else
            {
                _Person.Gendor = (int)enGendor.Female;
            }
            _Person.NationalityCountryID = clsCountry.Find(cbCountry.Text).CountryID;
            if(pbPersonImage.ImageLocation!=null)
            {
                _Person.ImagePath = pbPersonImage.ImageLocation.ToString();
            }
            else
            {
                _Person.ImagePath = "";
            }
            if(_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update Person";
                MessageBox.Show("Person Saved Successfuly with Person ID  = " + _Person.PersonID.ToString(), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this,_Person.PersonID);
            }
            else
                MessageBox.Show("Person Not Saved  " , "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                pbPersonImage.Load(FileName);
                llRemoveImage.Visible = true;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;
            if(rbMale.Checked)
            {
                pbPersonImage.Image = Resources.Male_512;
            }
            else
            {
                pbPersonImage.Image = Resources.Female_512;
            }
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            if(pbPersonImage.ImageLocation ==null)
                pbPersonImage.Image =Resources.Male_512;
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = (TextBox)sender;
            if(string.IsNullOrEmpty(Temp.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This Field Is Required");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtEmail.Text))
            {
                return;
            }
            if(!clsValidations.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Wrong Email Format");
            }
            else
            {
                errorProvider1.SetError(txtEmail,null);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty (txtNationalNo.Text))
            {
                e.Cancel= true;
                errorProvider1.SetError(txtNationalNo, "This Field Is Required");

            }
            else
            {
                errorProvider1.SetError (txtNationalNo, null);
            }
            if(txtNationalNo.Text == _Person.NationalNo && clsPerson.ISPersonExist(txtNationalNo.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This National No Is Used By Another Person Choose Another One");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo , null);
            }
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
        }
    }
}
