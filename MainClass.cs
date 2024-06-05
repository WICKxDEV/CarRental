using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TMS
{
    internal class MainClass
    {
        //connecting database
        public static readonly string con_string= "Data Source=DESKTOP-1P7HTT7;Initial Catalog=TMSDB;Integrated Security=True;Pooling=False";
        public static SqlConnection con = new SqlConnection(con_string);


        //user validation
        public static bool IsValidUser(string user, string pass)
        {
            bool isvalid = false;

            string qry = @"Select * from UsersTab where Job = '" + user + "' and upass = '" + pass + "' ";
            SqlCommand cmd = new SqlCommand(qry, con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                isvalid = true;
                USER = dt.Rows[0]["uName"].ToString();
            
            }


            return isvalid;
        }

        //create property for username
        public static string user;
        public static string USER
        { 
            get { return user; }
            private set { user = value; }
        }

        public static int SQL(string qry, Hashtable ht)
        {
            int res = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;

                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if (con.State == ConnectionState.Closed) { con.Open(); }
                res = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)  { con.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }

            return res;
        }

        //for loading data from database
        public static void  LoadData(string qry , DataGridView gv,ListBox lb)
        {
            try 
            {
                SqlCommand cmd = new SqlCommand(qry ,con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string colName1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colName1].DataPropertyName = dt.Columns[i].ToString();
                }

                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }
           
        }
     
    }
}
