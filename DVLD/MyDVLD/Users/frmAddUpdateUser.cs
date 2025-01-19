using DVLD_Business;
using MyDVLD.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.Users
{
    public partial class frmAddUpdateUser : Form
    {
        public enum enMode { AddNew = 0 ,Update = 1 }

        private enMode _Mode;

        private int _UserID = -1;

        private clsUser _User;
        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;    
        }
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _UserID = UserID;
        }

        private void _ResetDefaultValue()
        {
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New ";
                _User = new clsUser();

                tpLoginInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update";

                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            lblUserID.Text = "[???]";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            
        }

        private void _LoadData()
        {
            _User = clsUser.FindUserByUserID( _UserID );
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            if (_User == null)
            {
                MessageBox.Show("No User With User ID = "+_UserID.ToString(),"User Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            if(_Mode == enMode.Update) 
                _LoadData();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
           if(_Mode ==enMode.Update)
           {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUser.SelectedTab = tcUser.TabPages["tpLoginInfo"];
                return;

           }
           if(ctrlPersonCardWithFilter1.PersonID!=-1)
           {
                if(clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("User Already Exists Select Another Person","User Exists",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUser.SelectedTab = tcUser.TabPages["tpLoginInfo"];
                }
           }
            else
            {
                MessageBox.Show("Please Select A Person", "User Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();

            }



        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtUserName.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "This Feild Is Required");
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            }
           if(_Mode ==enMode.AddNew)
           {
                if (clsUser.IsUserExist(txtUserName.Text))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "This UserName Is Used  Please Select Another ");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }
           }
            else
            {
                if(_User.UserName !=txtUserName.Text.Trim())
                {
                    if (clsUser.IsUserExist(txtUserName.Text))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "This UserName Is Used  Please Select Another ");
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    }
                }
            }

        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "This Field Can Not Be Empty");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                e.Cancel =true;
                errorProvider1.SetError(txtConfirmPassword, "This Field Can Not Be Empty");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
            if(txtConfirmPassword.Text != txtPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Does not Matches ");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!ValidateChildren())
            {
                MessageBox.Show("Some Feild Are Wrong Put The Mouse Over The Red Icon to See The Error"," Validation Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            string Password = clsDataHelper.ComputeHash(txtPassword.Text);
            _User.Password =Password;
            _User.IsActive = chkIsActive.Checked;

            if(_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = "Update ";
                MessageBox.Show("User Saved Succefully", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            else
            {
                MessageBox.Show("Failed To Save User ","Failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
