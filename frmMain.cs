using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMS
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
 
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            
        }

        private void btnVehicles_Click(object sender, EventArgs e)
        {
            frmVehicles frm = new frmVehicles();
            frm.Show();

        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            frmManagement frm = new frmManagement();
            frm.Show();

        }

        private void btnPos_Click(object sender, EventArgs e)
        {
            Return frm = new Return();
            frm.Show();

        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            Rental frm = new Rental();
            frm.Show();

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmLogin frm = new frmLogin();
            frm.Show();
        }
    }
}
