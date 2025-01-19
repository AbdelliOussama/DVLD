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

namespace MyDVLD.Users.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        private int _UserID = -1;
        public int UserID {  get { return _UserID; } }

        private clsUser _User;
        public clsUser SelectedUser { get { return _User; } }   
        public ctrlUserCard()
        {
            InitializeComponent();
        }

        private void _ResetDeafultValue()
        {
            ctrlPersonCard1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]"; 
            lblIsActive.Text = "[???]";  

        }

        private void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            _UserID = _User.UserID;
            lblUserID.Text = _UserID.ToString();
            lblUserName.Text = _User.UserName;
            lblIsActive.Text = (_User.IsActive == true ? "Yes" : "No");

        }
        public void LoadUserInfo(int UserID)
        {
            _User = clsUser.FindUserByUserID(UserID);
            if (_User == null)
            {
                MessageBox.Show("No User With User ID = " + UserID.ToString(), "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetDeafultValue();
                return;
            }
            _FillUserInfo();

        }
       
    }
}
