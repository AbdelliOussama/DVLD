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

namespace MyDVLD.Test.Controls
{
    public partial class ctrlScheduleTest : UserControl
    { 
        public enum enMode { AddNew=0,Update = 1};
        private enMode _Mode = enMode.AddNew;
        public enum enCreationMode { ScheduleFirstTime = 1,ScheduleRetakeTest = 2};
        private enCreationMode _CreationMode = enCreationMode.ScheduleFirstTime;

        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private int _TestAppointmentID = -1;
        private clsTestAppointment _TestAppointment;

        private clsTestType.enTestType _TestTypeID;

        [
            Category("Test Type Info"),
            Description("Test Type ID")
        ]
        public clsTestType.enTestType TestTypeID
        {
            get { return _TestTypeID; }
            set
            {
                _TestTypeID = value;
                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512; 
                        break;
                    case clsTestType.enTestType.StreetTest:
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;                      
                }
            }
        }

        public void LoadData(int LocalDrivingLicenseApplicationID,int TestAppointmentID = -1)
        {
            if (TestAppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = TestAppointmentID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Local Driving License Application with Local Driving License Application ID = " + LocalDrivingLicenseApplicationID.ToString(), "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }
            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
            {
                _CreationMode = enCreationMode.ScheduleRetakeTest;
            }
            else
                _CreationMode = enCreationMode.ScheduleFirstTime;

            if(_CreationMode == enCreationMode.ScheduleRetakeTest)
            {
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees.ToString();
                lblRetakeTestAppID.Text = "0";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeTestAppID.Text = "N/A";
                lblRetakeAppFees.Text = "0";
            }
            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.PersonFullName;
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();

            if(_Mode==enMode.AddNew)
            {
                _TestAppointment = new clsTestAppointment();
                dtpTestDate.Value = DateTime.Now;
                lblFees.Text = clsTestType.Find(_TestTypeID).TestTypeFees.ToString();
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }
            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text)+Convert.ToSingle(lblRetakeAppFees.Text)).ToString();

            if (!_HandelActiveTestAppointmentConstraint())
                return;
            if (!_HandelAppointmentIsLockedConstriant())
                return;
            if(!_HandelPreviousTestConstraint())
                return ;

        }
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }
        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            if(_TestAppointment == null)
            {
                MessageBox.Show("No Appointment With TestAppointmentID = " + _TestAppointmentID.ToString(), "Appointment Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;    
                return false;
            }
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpTestDate.Value = DateTime.Now;
            else
                dtpTestDate.Value = _TestAppointment.AppointmentDate;

            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if(_TestAppointment.RetakeTestApplicationID ==-1)
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
                lblRetakeAppFees.Text = _TestAppointment.RetakeTestApplicationInfo.PaidFees.ToString();
                lblTitle.Text = " Schedule Retake Test ";
                gbRetakeTestInfo.Enabled = true;
            }
            return true;
            
        }
        private bool _HandelActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && _LocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                return false;
                    
            }
            return true;   
        }
        private bool _HandelAppointmentIsLockedConstriant()
        {
            if(_TestAppointment.IsLocked)
            {
                btnSave.Enabled = false;
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked.";
                dtpTestDate.Enabled = false;
                return false;
            }
            else
                lblUserMessage.Visible = false;
            return true;
        }
        private bool _HandelPreviousTestConstraint()
        {
            switch(TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblUserMessage.Visible = false;
                    return true;
                case clsTestType.enTestType.WrittenTest:
                    if(_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Visible = false;
                        dtpTestDate.Enabled = true;
                        btnSave.Enabled=true;
                    }
                    else
                    {
                        lblUserMessage.Visible=false;
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        dtpTestDate.Enabled=false;
                        btnSave.Enabled=false;
                        return false;
                    }
                    return true;
                case clsTestType.enTestType.StreetTest:
                    if (_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Visible = false;
                        dtpTestDate.Enabled = true;
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        dtpTestDate.Enabled = false;
                        btnSave.Enabled = false;
                        return false;
                    }
                    return true;
            }
            return true;
        }

        private bool _HandelRetakeTestApplication()
        {
            if(_Mode ==enMode.AddNew && _CreationMode ==enCreationMode.ScheduleRetakeTest)
            {
                clsApplication application = new clsApplication();
                application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                application.ApplicationDate = DateTime.Now;
                application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                application.LastStatusDate = DateTime.Now;
                application.ApplicationTypeID = clsApplication.enApplicationType.RetakeTest;
                application.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).ApplicationFees;

                if(!application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    _TestAppointment.RetakeTestApplicationID = application.ApplicationID;
                }               
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandelRetakeTestApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

       
    }
}
