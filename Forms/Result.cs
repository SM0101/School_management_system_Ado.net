using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace School_Management_System.Forms
{
    public partial class Result : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataReader data_reader;
        SqlDataAdapter adapter;
        DataSet ds;
        SqlCommandBuilder scd;
        public Result()
        {
            InitializeComponent();
            string myConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            conn = new SqlConnection(myConnectionString);
            DisplayResult();
            FillStudent();
        }

        private void FillStudent()
        {
            conn.Open();
            cmd = new SqlCommand("SELECT studentID FROM sm.Students",conn);
            data_reader= cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("studentID", typeof(int));
            dt.Load(data_reader);
            cmbBoxId.ValueMember = "studentID";
            cmbBoxId.DataSource = dt;
            conn.Close();
        }

        private void DisplayResult()
        {
            conn.Open();
            string q = "SELECT * FROM sm.Marks";
            adapter = new SqlDataAdapter(q, conn);
            scd= new SqlCommandBuilder(adapter);
            ds= new DataSet();
            adapter.Fill(ds);
            dataGridView2.DataSource= ds.Tables[0];
            conn.Close();
        }

        private void GetName()
        {
            conn.Open();
            cmd = new SqlCommand("SELECT * FROM sm.Students WHERE studentID=@stdId",conn);
            cmd.Parameters.AddWithValue("@stdId",cmbBoxId.SelectedValue.ToString());
            DataTable dt = new DataTable();
            adapter = new SqlDataAdapter(cmd);
            adapter.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                txtBoxN.Text = dr["StudentName"].ToString();
            }
            conn.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmDetails d = new frmDetails();
            d.ShowDialog();
        }

        private void cmbBoxId_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetName();
        }

        private void btnAddR_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBoxN.Text != "" && cmbBoxId.Text != "" && numB.Text != "" && numE.Text != "" && numM.Text != "")
                {
                    conn.Open();
                    cmd = new SqlCommand("INSERT INTO sm.Marks VALUES(@stdID,@bangla,@english,@math)",conn);
                   


                    cmd.Parameters.AddWithValue("@stdID", cmbBoxId.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@bangla",Convert.ToInt32(numB.Value));
                    cmd.Parameters.AddWithValue("@english", Convert.ToInt32(numE.Value));
                    cmd.Parameters.AddWithValue("@math", Convert.ToInt32(numM.Value));


                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Inserted Successfully");


                    conn.Close();
                    DisplayResult();
                    resetAll();
                }
                else
                {
                    MessageBox.Show("Please Provide All information");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error......." + ex.Message);
            }
        }

        private void resetAll()
        {
            cmbBoxId.SelectedIndex = -1;
            txtBoxN.Text = "";
            numB.Text = "";
            numE.Text = "";
            numM.Text = "";
        }

        
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = new DataGridView();
            dgv = dataGridView2;
            cmbBoxId.SelectedItem = dgv.CurrentRow.Cells[0].Value.ToString();
            txtBoxN.Text = dgv.CurrentRow.Cells[1].Value.ToString();
            numB.Value = Convert.ToInt32(dgv.CurrentRow.Cells[2].Value.ToString());
            numE.Value = Convert.ToInt32(dgv.CurrentRow.Cells[3].Value.ToString());
            numM.Value = Convert.ToInt32(dgv.CurrentRow.Cells[4].Value.ToString());

        }
    }
}
