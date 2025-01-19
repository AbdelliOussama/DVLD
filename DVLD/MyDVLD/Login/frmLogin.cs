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

namespace MyDVLD.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "";
            string Password = "";
            if(clsGlobal.GetStoredCredential(ref UserName,ref Password))
            {
                txtUserName.Text = UserName;   
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
            {
                chkRememberMe.Checked = false;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string Password  = clsDataHelper.ComputeHash(txtPassword.Text.Trim());
            clsUser User = clsUser.FindUserByUserNameAndPassword(txtUserName.Text.Trim(), Password);
            if (User!=null)
            {
                if(chkRememberMe.Checked)
                {
                    clsGlobal.RememberUserNameAndPassword(txtUserName.Text.Trim(),txtPassword.Text.Trim());
                }
                else
                {
                    clsGlobal.RememberUserNameAndPassword("","");
                }


                if(!User.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("User Is Inactive Please Contact Your Admin", "Inactive User", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }             
                clsGlobal.CurrentUser = User;
                this.Hide();
                frmMain frm = new frmMain();
                frm.ShowDialog();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Wrong Credential Please Verify ", "Wrong  UserName  Or Password",MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
                
                
        }
    }
}
