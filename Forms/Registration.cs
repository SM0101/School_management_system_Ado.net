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
using System.Configuration;

namespace School_Management_System
{
    public partial class Registration : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader data_reader;
        public Registration()
        {
            InitializeComponent();
            string myConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            conn = new SqlConnection(myConnectionString);
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBoxUser.Text != "" && txtBoxPass.Text != "")
                {
                    conn.Open();
                    string tsql = "INSERT INTO sm.Registration VALUES(@user, @pass)";
                    cmd = new SqlCommand(tsql, conn);
                    cmd.Parameters.AddWithValue("@user", txtBoxUser.Text);
                    cmd.Parameters.AddWithValue("@pass", txtBoxPass.Text);
 
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Succesfully Registered");

                    frmHome hm = new frmHome();
                    hm.ShowDialog();
                    this.Hide();

                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Failed to registered....");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error....." + ex.Message);
            }
            finally
            {
                this.Hide();
                frmHome log = new frmHome();
                log.ShowDialog();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
            frmHome log = new frmHome();
            log.ShowDialog();
        }
    }
}
