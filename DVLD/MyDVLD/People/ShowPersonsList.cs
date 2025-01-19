using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyDVLD.People
{
    public partial class frmListPeople : Form
    {
        public static DataTable _dtAllPeople = clsPerson.GetAllPersons();

        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                      "FirstName", "SecondName", "ThirdName", "LastName",
                                                      "GendorCaption", "DateOfBirth", "CountryName",
                                                      "Phone", "Email");

        private void _Refresh()
        {
            _dtAllPeople = clsPerson.GetAllPersons();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                      "FirstName", "SecondName", "ThirdName", "LastName",
                                                      "GendorCaption", "DateOfBirth", "CountryName",
                                                      "Phone", "Email");
            dgvAlPeoples.DataSource = _dtPeople;    
            lblRecordsCount.Text = dgvAlPeoples.Rows.Count.ToString();
        }
        public frmListPeople()
        {
            InitializeComponent();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            dgvAlPeoples.DataSource = _dtPeople;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvAlPeoples.Rows.Count.ToString();
            if (dgvAlPeoples.Rows.Count > 0 )
            {
                dgvAlPeoples.Columns[0].HeaderText = "Person ID";
                dgvAlPeoples.Columns[0].Width =100;

                dgvAlPeoples.Columns[1].HeaderText = "National No";
                dgvAlPeoples.Columns[1].Width = 100;

                dgvAlPeoples.Columns[2].HeaderText = "First Name";
                dgvAlPeoples.Columns[2].Width = 100;

                dgvAlPeoples.Columns[3].HeaderText = "Second Name";
                dgvAlPeoples.Columns[3].Width = 150;

                dgvAlPeoples.Columns[4].HeaderText = "Third Name";
                dgvAlPeoples.Columns[4].Width = 100;

                dgvAlPeoples.Columns[5].HeaderText = "Last Name";
                dgvAlPeoples.Columns[5].Width = 100;

                dgvAlPeoples.Columns[6].HeaderText = "Gendor";
                dgvAlPeoples.Columns[6].Width = 100;

                dgvAlPeoples.Columns[7].HeaderText = "Date Of Birth";
                dgvAlPeoples.Columns[7].Width = 150;

                dgvAlPeoples.Columns[8].HeaderText = "Nationality";
                dgvAlPeoples.Columns[8].Width = 100;

                dgvAlPeoples.Columns[9].HeaderText = "Phone";
                dgvAlPeoples.Columns[9].Width = 170;

                dgvAlPeoples.Columns[10].HeaderText = "Email";
                dgvAlPeoples.Columns[10].Width = 250;
            }

        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");
            if(txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
           
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "National No":
                    FilterColumn = "NationalNo";
                    break;
                case "First Name":
                    FilterColumn = "FirstName";
                    break;
                case "Second Name":
                    FilterColumn = "SecondName";
                    break;
                case "Third Name":
                    FilterColumn = "ThirdName";
                        break;
                case "Last Name":
                    FilterColumn = "LastName";
                        break;
                case "Gendor":
                    FilterColumn = "GendorCaption";
                    break;
                case "Nationality":
                    FilterColumn = "CountryName";
                    break;
                case "Phone":
                    FilterColumn = "Phone";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if(FilterColumn == "None" || txtFilterValue.Text.Trim() =="")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvAlPeoples.Rows.Count.ToString();
                return;
            }
            if(FilterColumn =="PersonID")
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}",FilterColumn,txtFilterValue.Text.Trim()); 
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
            }

            lblRecordsCount.Text =dgvAlPeoples.Rows.Count.ToString();
        }




        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _Refresh();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvAlPeoples.CurrentRow.Cells[0].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.ShowDialog();
            _Refresh();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvAlPeoples.CurrentRow.Cells[0].Value;
            frmAddUpdatePerson frm = new frmAddUpdatePerson(PersonID);
            frm.ShowDialog();
            _Refresh();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvAlPeoples.CurrentRow.Cells[0].Value;
            if (MessageBox.Show($"Are You Sure You Want To Delete Person With Person ID = {PersonID}  ? ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if(clsPerson.DeletePerson(PersonID))
                {
                    MessageBox.Show($"Person With Person ID = {PersonID} Deleted Succesfully","Succes",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    _Refresh();
                }
                else
                {
                    MessageBox.Show($"Failed To Delete Person  with Person ID  =  {PersonID}  Because There is Data Linked To It", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet :)","Not Implemented",MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet :)", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Warning);

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text == "Person ID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void dgvAlPeoples_DoubleClick(object sender, EventArgs e)
        {
            int PersonID = (int)dgvAlPeoples.CurrentRow.Cells[0].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

    }
}
