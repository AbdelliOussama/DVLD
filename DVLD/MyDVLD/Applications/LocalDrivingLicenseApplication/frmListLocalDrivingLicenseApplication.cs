using DVLD_Business;
using MyDVLD.Licenses;
using MyDVLD.Licenses.Local_licenses;
using MyDVLD.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.Applications.LocalDrivingLicenseApplication
{
    public partial class frmListLocalDrivingLicenseApplication : Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplications;
        public frmListLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvAllLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrivingLicenseApplications;
            lblRecordsCount.Text = dgvAllLocalDrivingLicenseApplications.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;
            if(dgvAllLocalDrivingLicenseApplications.Rows.Count>0)
            {
                dgvAllLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvAllLocalDrivingLicenseApplications.Columns[0].Width = 110;

                dgvAllLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvAllLocalDrivingLicenseApplications.Columns[1].Width = 250;

                dgvAllLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No";
                dgvAllLocalDrivingLicenseApplications.Columns[2].Width = 110;

                dgvAllLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvAllLocalDrivingLicenseApplications.Columns[3].Width = 340;

                dgvAllLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvAllLocalDrivingLicenseApplications.Columns[4].Width = 300;

                dgvAllLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvAllLocalDrivingLicenseApplications.Columns[5].Width = 110;
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.Text != "None";
            if (txtFilterValue.Visible )
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvAllLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if(FilterColumn == "None" || txtFilterValue.Text=="")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvAllLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }
            if(FilterColumn == "LocalDrivingLicenseApplicationID")
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1} ",FilterColumn,txtFilterValue.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE  '{1}%' ", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvAllLocalDrivingLicenseApplications.Rows.Count.ToString();


        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text =="L.D.L.AppID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmShowLocalDrivingLicenseAppInfo frm = new frmShowLocalDrivingLicenseAppInfo(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
        }

        private void btnAddNewLocalDrivingLicenseApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplication_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure You Want To Delete This Local Driving License Application","Confiramtion",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
            {
                return;
            }
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if(localDrivingLicenseApplication != null )
            {
                if (localDrivingLicenseApplication.DeleteLocalDrivingLicenseApplication())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplication_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not delete application, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Cancel This Local Driving License Application", "Confiramtion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            if (localDrivingLicenseApplication != null)
            {
                if (localDrivingLicenseApplication.CancelApplication())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplication_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not Cancel application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmListTestAppointment frm = new frmListTestAppointment(LocalDrivingLicenseApplicationID, clsTestType.enTestType.VisionTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplication_Load(null, null);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmListTestAppointment frm = new frmListTestAppointment(LocalDrivingLicenseApplicationID, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplication_Load(null, null);

        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmListTestAppointment frm = new frmListTestAppointment(LocalDrivingLicenseApplicationID, clsTestType.enTestType.StreetTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplication_Load(null, null);

        }

        private void cmsLocalDrivingLicenseApplication_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalDrivingLicenseApplication.IsLicesneIssued();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled  = (TotalPassedTests==3) && !LicenseExists;
            
            ShowLicenseToolStripMenuItem.Enabled = LicenseExists;
            editToolStripMenuItem.Enabled = !LicenseExists &&(LocalDrivingLicenseApplication.ApplicationStatus== clsApplication.enApplicationStatus.New);
            scheduleTestsToolStripMenuItem.Enabled = !LicenseExists;
            cancelApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus==clsApplication.enApplicationStatus.New);    
            deleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest);
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            scheduleTestsToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest ||!PassedStreetTest ) && LocalDrivingLicenseApplication.ApplicationStatus==clsApplication.enApplicationStatus.New;

            if(scheduleTestsToolStripMenuItem.Enabled)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                scheduleWrittenTestToolStripMenuItem.Enabled =  PassedVisionTest && !PassedWrittenTest; 

                scheduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }
        }

        private void ShowLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            int LicenseID = LocalDrivingLicenseApplication.GetActiveLicenseID();
            if(LicenseID!=-1)
            {
                frmShowLicense frm = new frmShowLicense(LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplication_Load(null,null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvAllLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicense = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(LocalDrivingLicense.ApplicantPersonID);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplication_Load(null,null);
        }
    }
}
