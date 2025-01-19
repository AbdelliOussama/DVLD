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

namespace MyDVLD.Test.TestTypes
{
    public partial class frmAddEditTestType : Form
    {
        public enum enMode { AddNew = 0,Update =1 }
        private enMode _Mode;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestType _TestType;

        public frmAddEditTestType()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddEditTestType(clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestTypeID = (clsTestType.enTestType)TestTypeID;
            _Mode = enMode.Update;
        }
        private void _ResetDefaultValue()
        {
            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Test Type";
                this.Text = "Add New";
                _TestType = new clsTestType();
            }
            else
            {
                lblTitle.Text = "Update Test Type";
                this.Text = "Update";
            }
            lblTestTypeID.Text = " [???]";
            txtTestTypeTitle.Text = "";
            txtDescription.Text = "";
            txtTestTypeFees.Text = "";
        }
        private void _LoadData()
        {
            _TestType = clsTestType.Find(_TestTypeID);
            if(_TestType == null )
            {
                MessageBox.Show("No Test Type With Test Type ID = "+((int)_TestTypeID).ToString(),"Test Type Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            lblTestTypeID.Text = ((int)_TestType.TestTypeID).ToString();
            txtTestTypeTitle.Text= _TestType.TestTypeTitle;
            txtDescription.Text = _TestType.TestTypeDescription;
            txtTestTypeFees.Text= _TestType.TestTypeFees.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddEditTestType_Load(object sender, EventArgs e)
        {
            _ResetDefaultValue();
            if(_Mode==enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!ValidateChildren())
            {
                MessageBox.Show("Some Field Are Not Valid Put The Red Icon(s) To See The error","Validating Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            _TestType.TestTypeTitle = txtTestTypeTitle.Text.Trim();
            _TestType.TestTypeDescription = txtDescription.Text.Trim();
            _TestType.TestTypeFees = Convert.ToSingle(txtTestTypeFees.Text.Trim());
            if(_TestType.Save())
            {
                lblTestTypeID.Text = _TestType.TestTypeID.ToString();
                _Mode=enMode.Update;
                lblTitle.Text = "Update Test Type";
                this.Text = "Update";
                MessageBox.Show("Test Type Saved Succesfully ","Succes",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Test Type Not Saved  ", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
