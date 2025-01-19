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

namespace MyDVLD.Test.TestTypes
{
    public partial class frmListTestTypes : Form
    {
        private DataTable _dtAllTestTypes;
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _dtAllTestTypes = clsTestType.GetAllTestTypes();
            dgvAllTestTypes.DataSource = _dtAllTestTypes;
            lblRecordsCount.Text = dgvAllTestTypes.Rows.Count.ToString();
            if(dgvAllTestTypes.Rows.Count > 0 )
            {
                dgvAllTestTypes.Columns[0].HeaderText = "ID";
                dgvAllTestTypes.Columns[0].Width = 100;

                dgvAllTestTypes.Columns[1].HeaderText = "Title";
                dgvAllTestTypes.Columns[1].Width = 250;

                dgvAllTestTypes.Columns[2].HeaderText = "Description";
                dgvAllTestTypes.Columns[2].Width = 500;

                dgvAllTestTypes.Columns[3].HeaderText = "Fees";
                dgvAllTestTypes.Columns[3].Width = 100;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestTypeID = (int)dgvAllTestTypes.CurrentRow.Cells[0].Value;
            frmAddEditTestType frm = new frmAddEditTestType((clsTestType.enTestType)TestTypeID);
            frm.ShowDialog();
        }

        private void cmsTestTypes_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
