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

namespace MyDVLD.Applications.ApplicationTypes
{
    public partial class frmAddEditApplicationType : Form
    {
        public enum enMode { AddNew =0, Edit = 1 }
        private enMode _Mode;
        private int _ApplicationTypeID;
        private clsApplicationType _ApplicationType;
        public frmAddEditApplicationType()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _Mode = enMode.Edit;
            _ApplicationTypeID = ApplicationTypeID;
        }
        private void _ResetDefaultValue()
        {
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Application Type";
                this.Text = "Add New ";
                _ApplicationType = new clsApplicationType();
            }
            else
            {
                lblTitle.Text = "Update Application Type";
                this.Text = "Update ";
            }
            lblApplicationTypeID.Text = "[???]";
            txtApplicationTypeFees.Text = "";
            txtApplicationTypeTitle.Text = "";
        }

        private void _LoadData()
        {
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);
            if(_ApplicationType == null)
            {
                MessageBox.Show("There Is No ApplicationType With Application Type ID = "+_ApplicationTypeID.ToString(),"Application Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
                return;
            }
            else
            {
                lblApplicationTypeID.Text = _ApplicationType.ApplicationTypeID.ToString();
                txtApplicationTypeFees.Text = _ApplicationType.ApplicationFees.ToString();
                txtApplicationTypeTitle.Text = _ApplicationType.ApplicationTypeTitle.ToString();
            }
        }

        private void frmAddEditApplicationType_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            if(_Mode ==enMode.Edit)
            {
                _LoadData();
            }
        }

        private void txtApplicationTypeFees_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtApplicationTypeFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeFees, "This Field Is Required");
            }
            else
            {
                errorProvider1.SetError(txtApplicationTypeFees, null);
            }
        }

        private void txtApplicationTypeTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtApplicationTypeTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtApplicationTypeTitle, "This Field Is Required");
            }
            else
            {
                errorProvider1.SetError(txtApplicationTypeTitle, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!ValidateChildren())
            {
                MessageBox.Show("Some Field Are Wrong Put The Mouse Over The Red Icon(s) To See The Error ","Validation Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _ApplicationType.ApplicationTypeTitle = txtApplicationTypeTitle.Text.Trim();
            _ApplicationType.ApplicationFees =Convert.ToSingle(txtApplicationTypeFees.Text.Trim());

            if(_ApplicationType.Save())
            {
                lblApplicationTypeID.Text = _ApplicationType.ApplicationTypeID.ToString();
                _Mode = enMode.Edit;
                lblTitle.Text = "Update Application Type";
                MessageBox.Show("Application Type Saved Succesfully ","Succes",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Application Type Not  Saved ", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtApplicationTypeFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
