using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMS
{
    public partial class Rental : Form
    {
        public Rental()
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
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Rental", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    guna2DataGridView5.DataSource = dt;
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
                MessageBox.Show("Text box ID cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int id;
                if (!int.TryParse(txtId.Text, out id))
                {
                    MessageBox.Show("ID must be a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Check if the ID already exists in the database
                if (CheckIfIdExists(id))
                {
                    MessageBox.Show("A record with this ID already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method to prevent further execution
                }

                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("insert into Rental (CarID, Customer_ID, Customer_Name, Rental_Fee, Due_Date) values (@CarID, @Customer_ID, @Customer_Name, @Rental_Fee, @Due_Date)", con))
                    {
                        cmd.Parameters.AddWithValue("@CarID", id);
                        cmd.Parameters.AddWithValue("@Customer_ID", txtDrivName.Text);
                        cmd.Parameters.AddWithValue("@Customer_Name", txtNicNo.Text);
                        cmd.Parameters.AddWithValue("@Rental_Fee", txtDrivNo.Text);
                        cmd.Parameters.AddWithValue("@Due_Date", txtVehiNo.Text);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Successfully Saved");
                    LoadData(); // Assuming LoadData is a method to refresh the data shown in your UI
                }
            }
        }

        private bool CheckIfIdExists(int id)
        {
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Rental WHERE CarID = @CarID", con))
                {
                    cmd.Parameters.AddWithValue("@CarID", id);
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
                SqlCommand cmd = new SqlCommand("Delete Rental where CarID=@CarID", con);
                cmd.Parameters.AddWithValue("@CarID", int.Parse(txtId.Text));
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Successfully Deleted");
                LoadData();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void frmStaff_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
