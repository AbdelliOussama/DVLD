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

namespace MyDVLD.Test
{
    public partial class frmTakeTest : Form
    {
        private int _TestAppointmentID = -1;
        private clsTestType.enTestType _TestTypeID;

        private int _TestID = -1;
        private clsTest _Test;
        public frmTakeTest(int TestAppointmentID,clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _TestAppointmentID = TestAppointmentID;
            _TestTypeID = TestTypeID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestType= _TestTypeID;
            ctrlScheduledTest1.LoadInfo(_TestAppointmentID);
            if(ctrlScheduledTest1.TestAppointmentID ==-1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;
                
            _TestID = ctrlScheduledTest1.TestID;
            if(_TestID !=-1)
            {
                _Test = clsTest.Find(_TestID);
                if(_Test.TestResult)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
                    txtNotes.Text = _Test.Notes;

                rbFail.Enabled = false;
                rbPass.Enabled = false;
                lblUserMessage.Visible = true;
            }
            else
                _Test = new clsTest();


        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure You Want To Save Test Result Yes/No","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
            {
                return;
            }
            _Test.Notes = txtNotes.Text;
            _Test.TestResult = rbPass.Checked;
            _Test.TestAppointmentID =_TestAppointmentID;
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
