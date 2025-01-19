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

namespace MyDVLD.Test
{
    public partial class frmScheduleTest : Form
    {
        private clsTestType.enTestType _TestTypeID  = clsTestType.enTestType.VisionTest;
        private int _AppointmentID = -1;
        private int _LocalDrivingLicenseApplicationID = -1;
        public frmScheduleTest(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestType, int  AppointmentID = -1)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestType;
            _AppointmentID = AppointmentID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestTypeID;
            ctrlScheduleTest1.LoadData(_LocalDrivingLicenseApplicationID,_AppointmentID);
        }
    }
}
