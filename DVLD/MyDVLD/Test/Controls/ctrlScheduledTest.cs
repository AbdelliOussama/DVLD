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
    public partial class ctrlScheduledTest : UserControl
    {
        
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _TestAppointmentID = -1;
        public int TestAppointmentID { get { return _TestAppointmentID; } }
        private clsTestAppointment _TestAppointment;

        private clsTestType.enTestType _TestTypeID ;
        public clsTestType.enTestType TestType
        {
            get { return _TestTypeID; }
            set 
            {
                _TestTypeID = value;
                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        gbScheduledTest.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        return;
                    case clsTestType.enTestType.WrittenTest:
                        gbScheduledTest.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        return;
                    case clsTestType.enTestType.StreetTest:
                        gbScheduledTest.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        return;
                           
                }
            
            }
        }
        private int _TestID = -1;
        public int TestID { get { return _TestID; } }

        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public void LoadInfo(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID;
            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            if(_TestAppointment == null)
            {
                MessageBox.Show("No TestAppointment With Test Appointment ID = "+TestAppointmentID.ToString(),"Not Found",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _TestID = _TestAppointment.TestID;
            _LocalDrivingLicenseApplicationID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                  "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.PersonFullName;
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();
            lblDate.Text = clsFormat.ConvertDateToShortString(_TestAppointment.AppointmentDate);
            lblFees.Text = _TestAppointment.PaidFees.ToString();
            lblTestID.Text = ( _TestAppointment.TestID == -1) ? "Not Taken Yet" : _TestAppointment.TestID.ToString();

        }
       
    }
}
