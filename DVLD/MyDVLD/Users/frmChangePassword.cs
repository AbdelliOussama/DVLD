using DVLD_Business;
using MyDVLD.Global_Classes;
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

namespace MyDVLD.Users
{
    public partial class frmChangePassword : Form
    {
        private int _UserID = -1;
        private clsUser _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
           
        }

        private void _ResetDefaultValue()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            _User = clsUser.FindUserByUserID(_UserID);
            if(_User==null)
            {
                MessageBox.Show("No User  With User ID  = "+_UserID.ToString()," User Not Found ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ctrlUserCard1.LoadUserInfo(_UserID);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtCurrentPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "This Field Can Not Be Empty");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            }
            string Password  = clsDataHelper.ComputeHash(txtCurrentPassword.Text);
            if(_User.Password !=Password)
            {
                e.Cancel=true;
                errorProvider1.SetError(txtCurrentPassword, " Entered Password Does Not Matches With Actual Password");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtNewPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "This Field Is Required");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "This Field Is Required");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
            if(txtConfirmPassword.Text != txtNewPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Does Not Matches ");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
               
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show("Some Field Are Wrong Put The Mouse Over The Red Icon(s) To See The Error","Validate Error ",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            string Password = clsDataHelper.ComputeHash(txtNewPassword.Text);
            _User.Password = Password;

            if(_User.Save())
            {
                MessageBox.Show("Password Changed Succesfully", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                MessageBox.Show("Failed To Change Password", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
    }
}
