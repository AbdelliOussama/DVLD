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

namespace MyDVLD.Applications.ApplicationTypes
{
    public partial class frmListApplicationType : Form
    {
        private DataTable _dtAllApplicationTypes;
        public frmListApplicationType()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListApplicationType_Load(object sender, EventArgs e)
        {
            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationTypes();
            dgvAllApplicationTypes.DataSource = _dtAllApplicationTypes;
            lblRecordsCount.Text = dgvAllApplicationTypes.Rows.Count.ToString();
            if(dgvAllApplicationTypes.Rows.Count > 0 )
            {
                dgvAllApplicationTypes.Columns[0].HeaderText = "ID";
                dgvAllApplicationTypes.Columns[0].Width = 100;

                dgvAllApplicationTypes.Columns[1].HeaderText = "Title";
                dgvAllApplicationTypes.Columns[1].Width = 350;

                dgvAllApplicationTypes.Columns[2].HeaderText = "Fees";
                dgvAllApplicationTypes.Columns[2].Width = 150;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ApplicationTypeID =(int) dgvAllApplicationTypes.CurrentRow.Cells[0].Value;
            frmAddEditApplicationType frm =new frmAddEditApplicationType(ApplicationTypeID);
            frm.ShowDialog();

            frmListApplicationType_Load(null,null);
        }
    }
}
