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
using static System.Windows.Forms.MonthCalendar;

namespace TMS
{
    public partial class Return : Form
    {
        public Return()
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
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Returnvehi", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    guna2DataGridView3.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHireNo.Text))
            {
                MessageBox.Show("Text box ID cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int id;
                if (!int.TryParse(txtHireNo.Text, out id))
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
                    using (SqlCommand cmd = new SqlCommand("insert into Returnvehi (Car_ID, Customer_ID, Date, Days_Elapped, Fine) values (@Car_ID, @Customer_ID, @Date, @Days_Elapped, @Fine)", con))
                    {
                        cmd.Parameters.AddWithValue("@Car_ID", id);
                        cmd.Parameters.AddWithValue("@Customer_ID", txtDrSalary.Text);
                        cmd.Parameters.AddWithValue("@Date", txtMaintainCost.Text);
                        cmd.Parameters.AddWithValue("@Days_Elapped", txtDcost.Text);
                        cmd.Parameters.AddWithValue("@Fine", txtProfit.Text);
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
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Returnvehi WHERE Car_ID = @Car_ID", con))
                {
                    cmd.Parameters.AddWithValue("@Car_ID", id);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        private bool CheckIfHireIdExists(int hireId)
        {
            using (SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False"))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Returnvehi WHERE Car_ID = @Car_ID", con))
                {
                    cmd.Parameters.AddWithValue("@Car_ID", hireId);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }


        private void frmPOS_Load(object sender, EventArgs e)
        {
            LoadData();


        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHireNo.Text))
            {

                MessageBox.Show("Text box cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                SqlConnection con = new SqlConnection("Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False");

                con.Open();
                SqlCommand cmd = new SqlCommand("Delete Returnvehi where Car_ID=@Car_ID", con);
                cmd.Parameters.AddWithValue("@Car_ID", int.Parse(txtHireNo.Text));
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("Successfully Deleted");
                LoadData();
            }
        }

    }
}
