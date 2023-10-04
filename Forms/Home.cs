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
    public partial class frmHome : Form
    {

        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader data_reader;

        public frmHome()
        {
            InitializeComponent();
            string myConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            conn = new SqlConnection(myConnectionString);
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            Registration rg = new Registration();
            rg.ShowDialog();
            this.Hide();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBoxUser.Text != "" && txtBoxPass.Text != "")
                {
                    conn.Open();
                    string tsql = "SELECT* FROM sm.Registration WHERE UserName=@user and Passcode=@pass";
                    cmd = new SqlCommand(tsql, conn);
                    cmd.Parameters.AddWithValue("@user", txtBoxUser.Text);
                    cmd.Parameters.AddWithValue("@pass", txtBoxPass.Text);

                    cmd.ExecuteNonQuery();
                    data_reader = cmd.ExecuteReader();
                    int count = 0;
                    while (data_reader.Read())
                    {
                        count++;
                    }
                    if (count == 1)
                    {
                        MessageBox.Show("Welcome!");
                        this.Hide();
                        frmDetails form = new frmDetails();
                        form.ShowDialog();
                    }
                    else if (count > 1)
                    {
                        MessageBox.Show("Duplicate value");
                        this.Hide();
                        frmHome log = new frmHome();
                        log.ShowDialog();
                    }
                    else if (count < 1)
                    {
                        MessageBox.Show("Please provide valid information!");
                        this.Hide();
                        frmHome log = new frmHome();
                        log.ShowDialog();
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error....." + ex.Message);
            }
        }
    }
}
