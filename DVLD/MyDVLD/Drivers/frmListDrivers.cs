using DVLD_Business;
using MyDVLD.Licenses;
using MyDVLD.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable _dtAllDrivers = new DataTable();
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _dtAllDrivers = clsDriver.GetAllDrivers();
            dgvAllDrivers.DataSource = _dtAllDrivers;
            cbFilterBy.SelectedIndex = 0;
            if(dgvAllDrivers.Rows.Count > 0 )
            {
                dgvAllDrivers.Columns[0].HeaderText = "Driver ID";
                dgvAllDrivers.Columns[0].Width = 110;

                dgvAllDrivers.Columns[1].HeaderText = "Person ID";
                dgvAllDrivers.Columns[1].Width = 110;

                dgvAllDrivers.Columns[2].HeaderText = "National No";
                dgvAllDrivers.Columns[2].Width = 110;

                dgvAllDrivers.Columns[3].HeaderText = "Full Name";
                dgvAllDrivers.Columns[3].Width = 300;

                dgvAllDrivers.Columns[4].HeaderText = "Date";
                dgvAllDrivers.Columns[4].Width = 200;

                dgvAllDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvAllDrivers.Columns[5].Width = 110;
            }
            lblRecordsCount.Text = dgvAllDrivers.Rows.Count.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = cbFilterBy.Text != "None";

            if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Enabled = false;
            }
            else
                txtFilterValue.Enabled = true;

            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilterBy.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "National No":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "None";
                    break;               
            }
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllDrivers.DefaultView.RowFilter ="";
                lblRecordsCount.Text = dgvAllDrivers.Rows.Count.ToString();
                return;
            }
            if(FilterColumn !="NationalNo" && FilterColumn != "FullName")
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1} ", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvAllDrivers.Rows.Count.ToString();

        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID =(int) dgvAllDrivers.CurrentRow.Cells[1].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);

        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet","Not Ready ",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory frm  = new frmShowPersonLicenseHistory((int)dgvAllDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
            frmListDrivers_Load(null, null);
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text =="Driver ID" || cbFilterBy.Text =="Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }
    }
}
