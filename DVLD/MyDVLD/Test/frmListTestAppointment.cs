using DVLD_Business;
using MyDVLD.Properties;
using System;
using System.Data;
using System.Windows.Forms;

namespace MyDVLD.Test
{
    public partial class frmListTestAppointment : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private DataTable _dtLicenseTestAppointments = new DataTable();
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        public frmListTestAppointment(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestType;
        }
        private void _LoadTestTypeTitleAndImage()
        {
            switch(_TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblTitle.Text = "Vision Test Appointment";
                    this.Text = lblTitle.Text;
                    pbTestAppointmentImage.Image = Resources.Vision_512;
                    break;
                case clsTestType.enTestType.WrittenTest:
                    lblTitle.Text = "Written Test Appointment";
                    pbTestAppointmentImage.Image = Resources.Written_Test_512;
                    this.Text = lblTitle.Text;

                    break;
                case clsTestType.enTestType.StreetTest:
                    lblTitle.Text = "Street Test Appointment";
                    pbTestAppointmentImage.Image = Resources.driving_test_512;
                    this.Text = lblTitle.Text;
                    break;
            }   
        }

        private void frmListTestAppointment_Load(object sender, EventArgs e)
        {
            _LoadTestTypeTitleAndImage();
            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsBytestType(_LocalDrivingLicenseApplicationID,_TestTypeID);
            dgvTestAppointments.DataSource = _dtLicenseTestAppointments;
            ctrlLocalDrivingLicenseApplicationInfo1.LoadLocalDrivingLicenseApplicationInfo(_LocalDrivingLicenseApplicationID);
            lblRecordsCount.Text = dgvTestAppointments.Rows.Count.ToString();
            if(dgvTestAppointments.Rows.Count > 0 )
            {
                dgvTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvTestAppointments.Columns[0].Width = 150;

                dgvTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvTestAppointments.Columns[1].Width = 150;

                dgvTestAppointments.Columns[2].HeaderText = "Appointment Fees";
                dgvTestAppointments.Columns[2].Width = 150;

                dgvTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvTestAppointments.Columns[3].Width = 150;

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplicationApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            if(LocalDrivingLicenseApplicationApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clsTest LastTest = LocalDrivingLicenseApplicationApplication.GetLastTestPerTestType(_TestTypeID);

            if(LastTest == null)
            {
                frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID);
                frm.ShowDialog();
                frmListTestAppointment_Load(null,null);
                return;
            }
            if(LastTest.TestResult ==true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm1 = new frmScheduleTest(LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID,_TestTypeID); 
            frm1.ShowDialog();
            frmListTestAppointment_Load(null, null);



        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value; 
            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestTypeID,TestAppointmentID);
            frm.ShowDialog();
            frmListTestAppointment_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvTestAppointments.CurrentRow.Cells[0].Value;
            frmTakeTest frm = new frmTakeTest(TestAppointmentID, _TestTypeID);
            frm.ShowDialog();
            frmListTestAppointment_Load(null, null);
        }
    }
}
