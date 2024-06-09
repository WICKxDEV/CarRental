using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Guna.UI2.WinForms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace TMS
{
    public partial class frmVehicles : Form
    {
        public frmVehicles()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM VehiclesTab", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    guna2DataGridView1.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {
                MessageBox.Show("Text box cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // First, check if the ID already exists in the database
                if (CheckIfIdExists(int.Parse(txtId.Text)))
                {
                    MessageBox.Show("A record with this ID already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method to prevent further execution
                }

                // If the ID does not exist, proceed with the insert operation
                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("insert into VehiclesTab (CarID, Make, Moadel) values (@CarID, @Make, @Moadel)", con))
                    {
                        cmd.Parameters.AddWithValue("@CarID", int.Parse(txtId.Text));
                        cmd.Parameters.AddWithValue("@Make", txtVehiNo.Text);
                        cmd.Parameters.AddWithValue("@Moadel", txtDrivName.Text);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Successfully Saved");
                    LoadData(); // Assuming LoadData is a method to refresh the data shown in the UI
                }
            }
        }

        private bool CheckIfIdExists(int ID)
        {
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM VehiclesTab WHERE CarID = @CarID", con))
                {
                    cmd.Parameters.AddWithValue("@CarID", ID);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


       

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtId.Text))
            {

                MessageBox.Show("Text box cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False");

                con.Open();
                SqlCommand cmd = new SqlCommand("Delete VehiclesTab where CarID=@CarID", con);
                cmd.Parameters.AddWithValue("@CarID", int.Parse(txtId.Text));
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Successfully Deleted");
                LoadData();
            }
        }

        private void frmVehicles_Load(object sender, EventArgs e)
        {

            LoadData();

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {

            LoadData();

        }

    }
}
