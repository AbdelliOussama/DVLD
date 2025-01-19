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

namespace MyDVLD.Applications.LocalDrivingLicenseApplication
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 }   
        private enMode _Mode = enMode.AddNew;

        private int _LocalDrivingLicenseApplicationID= -1;
        private int _SelectedPerson = -1;
        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID =LocalDrivingLicenseApplicationID ;
            _Mode = enMode.Update;
        }
        private void _FillLicenseClassesInComboBox()
        {
            DataTable dtAllLicenseClasses = clsLicenseClass.GetAllLicenseClasses();
            foreach(DataRow row  in dtAllLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }
        public void ResetDefaultValue ()
        {
            _FillLicenseClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Local Driving License App";
                this.Text = lblTitle.Text;
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                btnSave.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;
                lblLocalDrivingLicenseApplicationID.Text = "[???]";
                lblApplicationDate.Text = clsFormat.ConvertDateToShortString(DateTime.Now);
                cbLicenseClass.SelectedIndex = 2;
                lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).ApplicationFees.ToString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License App";
                this.Text = lblTitle.Text;
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
            }
            
           
        }
        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if( _LocalDrivingLicenseApplication == null )
            {
                MessageBox.Show("No Local Driving License Application With LocalDriving License Application ID = "+_LocalDrivingLicenseApplicationID.ToString(),"Not Found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.ConvertDateToShortString(_LocalDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.FindUserByPersonID(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ResetDefaultValue();
            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void _DataBackEvent(object sender, int personID)
        {
            _SelectedPerson = personID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_SelectedPerson);
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode ==enMode.Update)
            {
                tcLocalDrivingLicenses.SelectedTab = tcLocalDrivingLicenses.TabPages["tpApplicationInfo"];
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
                return;
            }
            if(ctrlPersonCardWithFilter1.PersonID !=-1)
            {
                tcLocalDrivingLicenses.SelectedTab = tcLocalDrivingLicenses.TabPages["tpApplicationInfo"];
                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Please Select A Person ", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;
            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPerson,clsApplication.enApplicationType.NewDrivingLicense,LicenseClassID);
            if(ActiveApplicationID !=-1)
            {
                MessageBox.Show("  choose Another License Class Person Aleardy Have An Active Application With The Same LicenseClass", "Errro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }
            //We Have Also To Verify If The Slected Person Already Have License With The Selected LicenseClass


            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.ApplicationTypeID = clsApplication.enApplicationType.NewDrivingLicense;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;

            if(_LocalDrivingLicenseApplication.Save())
            {
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = lblTitle.Text;
                lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                MessageBox.Show("Local Driving License Application Saved Sucesfully","Succes",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Local Driving License Application Not  Saved y", "Failed ", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPerson = obj;
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
