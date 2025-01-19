using DVLD_Business;
using MyDVLD.Applications.LocalDrivingLicenseApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID = -1;
        private clsDriver _Driver;
        private DataTable _dtLocalLicenses = new DataTable();
        private DataTable _dtInternationalLicenses = new DataTable();

        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        private void _LoadLocalLicensesData()
        {
            _dtLocalLicenses = clsDriver.GetAllLicenses(_DriverID);
            dgvLocalLicenses.DataSource = _dtLocalLicenses;
            if(dgvLocalLicenses.Rows.Count > 0 )
            {
                dgvLocalLicenses.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicenses.Columns[0].Width = 110;

                dgvLocalLicenses.Columns[1].HeaderText = "App.ID";
                dgvLocalLicenses.Columns[1].Width = 110;

                dgvLocalLicenses.Columns[2].HeaderText = "Class Name";
                dgvLocalLicenses.Columns[2].Width = 270;

                dgvLocalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicenses.Columns[3].Width = 170;

                dgvLocalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicenses.Columns[4].Width = 170;

                dgvLocalLicenses.Columns[5].HeaderText = "Is Active";
                dgvLocalLicenses.Columns[5].Width = 110;
            }
            lblLocalLicensesRecords.Text = dgvLocalLicenses.Rows.Count.ToString();
        }

        private void _LoadInternationalLicensesData()
        {
            _dtInternationalLicenses = clsDriver.GetAllInternationalLicenses(_DriverID);
            dgvInternationalLicenses.DataSource = _dtInternationalLicenses;
            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 160;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 130;

                dgvInternationalLicenses.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[2].Width = 130;

                dgvInternationalLicenses.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[3].Width = 180;

                dgvInternationalLicenses.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[4].Width = 180;

                dgvInternationalLicenses.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[5].Width = 120;
            }
            lblInternationalLicensesRecordsCount.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }

        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDriver.FindDriverByDriverID(DriverID);

            _LoadLocalLicensesData();
            _LoadInternationalLicensesData();

        }
        public void LoadInfoByPersonID(int  PersonID)
        {
            _Driver = clsDriver.FindDriverByPersonID(PersonID);
            if( _Driver != null )
            {
                _DriverID = _Driver.DriverID;
            }
            _LoadLocalLicensesData();
            _LoadInternationalLicensesData();
        }

        private void showInternationalLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature Not Implemented Yet", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void showLocalLiceseInfoToolStripMenuItem_Click_Click(object sender, EventArgs e)
        {
            int LocalLicenseID = (int)dgvLocalLicenses.CurrentRow.Cells[0].Value;
            frmShowLocalDrivingLicenseAppInfo frm = new frmShowLocalDrivingLicenseAppInfo(LocalLicenseID);
            frm.ShowDialog();
            
        }
        public void CLear()
        {
            _dtInternationalLicenses.Clear();
            _dtLocalLicenses.Clear();
        }
    }
}
