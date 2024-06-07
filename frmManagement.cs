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
    public partial class frmManagement : Form
    {
        public frmManagement()
        {
            InitializeComponent();
        }
        private void LoadDataH()
        {
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Manage", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    guna2DataGridView2.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }




        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceNo.Text))
            {
                MessageBox.Show("Text box cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Check if the Invoice_No already exists in the database
                if (CheckIfInvoiceNoExists(int.Parse(txtInvoiceNo.Text)))
                {
                    MessageBox.Show("An invoice with this number already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; // Exit the method to prevent further execution
                }

                using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("insert into Manage (Customer_ID, Customer_Name, Address, Mobile) values (@Customer_ID, @Customer_Name, @Address, @Mobile)", con))
                    {
                        cmd.Parameters.AddWithValue("@Customer_ID", int.Parse(txtInvoiceNo.Text));
                        cmd.Parameters.AddWithValue("@Customer_Name", txtVehicleNo.Text);
                        cmd.Parameters.AddWithValue("@Address", txtInvoiceValue.Text);
                        cmd.Parameters.AddWithValue("@Mobile", txtGarage.Text);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Successfully Saved");
                    LoadDataH(); // Assuming LoadDataH is a method to refresh the data shown in the UI
                }
            }
        }

        private bool CheckIfInvoiceNoExists(int invoiceNo)
        {
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Manage WHERE Customer_ID = @Customer_ID", con))
                {
                    cmd.Parameters.AddWithValue("@Customer_ID", invoiceNo);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        private void btnDel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceNo.Text))
            {

                MessageBox.Show("Text box cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False");

                con.Open();
                SqlCommand cmd = new SqlCommand("Delete Manage where Customer_ID=@Customer_ID", con);
                cmd.Parameters.AddWithValue("@Customer_ID", int.Parse(txtInvoiceNo.Text));
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Successfully Deleted");
                LoadDataH();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtInvoiceNo.Text))
            {

                MessageBox.Show("Text box cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False");

                con.Open();
                SqlCommand cmd = new SqlCommand("Select * from Manage where Customer_ID=@Customer_ID", con);
                cmd.Parameters.AddWithValue("@Customer_ID", int.Parse(txtInvoiceNo.Text));
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                guna2DataGridView2.DataSource = dt;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadDataH();
        }

        private void frmManagement_Load(object sender, EventArgs e)
        {
            LoadDataH();
        }

    }
}
