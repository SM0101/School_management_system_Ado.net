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
using System.IO;
using School_Management_System.Forms;

namespace School_Management_System
{
    public partial class frmDetails : Form
    {
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adapter;
        DataSet ds;
        SqlCommandBuilder scd;
        SqlTransaction transact;



        public frmDetails()
        {
            InitializeComponent();
            string myConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            conn = new SqlConnection(myConnectionString);
        }


        private void frmDetails_Load(object sender, EventArgs e)
        {
            displayGrid();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmHome hm = new frmHome();
            hm.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBoxName.Text != "" && txtBoxAddress.Text != "" && dateTimePickerSt.Text != "" && cmbBoxGender.Text != "" && cmbBoxClass.Text != "" && txtBoxPhotoSt.Text != "")
                {
                    conn.Open();
                    transact = conn.BeginTransaction();
                    cmd = new SqlCommand();
                    cmd.CommandText = "sm.sp_StudentInsert";
                    cmd.Connection= conn;
                    cmd.CommandType = CommandType.StoredProcedure;

               
                    cmd.Parameters.AddWithValue("@studentname", txtBoxName.Text);
                    cmd.Parameters.AddWithValue("@birthdate", DateTime.Parse(dateTimePickerSt.Text));
                    cmd.Parameters.AddWithValue("@gender", cmbBoxGender.SelectedItem);
                    cmd.Parameters.AddWithValue("@class", cmbBoxClass.SelectedItem);
                    cmd.Parameters.AddWithValue("@address", txtBoxAddress.Text);
                    cmd.Parameters.AddWithValue("@photo", getImage());

                    transact.Commit();
                    cmd.ExecuteNonQuery();
                    
                    MessageBox.Show("Data Inserted Successfully");
                  

                    conn.Close();
                    displayGrid();
                    resetAll();
                }
                else
                {
                    MessageBox.Show("Please Provide All information");
                }
            }
            catch (Exception ex)
            {
                transact.Rollback();
                MessageBox.Show("Error......." + ex.Message);
            }
        }

        private byte[] getImage()
        {
            Image img = picBoxPhotoSt.Image;
            byte[] arr;
            ImageConverter conv = new ImageConverter();
            arr = (byte[])conv.ConvertTo(img, typeof(byte[]));
            return arr;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            if (open.ShowDialog() == DialogResult.OK)
            {
                string imagePath = open.FileName;
                picBoxPhotoSt.Image = new Bitmap(imagePath);
                txtBoxPhotoSt.Text = imagePath;

                string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
                //string correctName = System.IO.Path.GetFileName(open.FileName);
                System.IO.File.Copy(open.FileName, path + "\\Photo\\" + System.IO.Path.GetRandomFileName());
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            displayGrid();
            resetAll();
        }
  
        private void displayGrid()
        {
            conn.Open();
            // TODO: This line of code loads data into the 'schoolMgtDBDataSet.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter.Fill(this.schoolMgtDBDataSet.Students);
            adapter = new SqlDataAdapter("SELECT * FROM sm.Students", conn);
            scd = new SqlCommandBuilder(adapter);
            ds = new DataSet();
            adapter.Fill(ds, "sm.Students");
            dataGridView1.DataSource = ds.Tables[0].DefaultView;
            conn.Close();
        }

    
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = new DataGridView();
            dgv = dataGridView1;
            txtBoxName.Text = dgv.CurrentRow.Cells[1].Value.ToString();
            dateTimePickerSt.Text = dgv.CurrentRow.Cells[2].Value.ToString();
            cmbBoxGender.SelectedItem = dgv.CurrentRow.Cells[3].Value.ToString();
            cmbBoxClass.SelectedItem = dgv.CurrentRow.Cells[4].Value.ToString();
            txtBoxAddress.Text = dgv.CurrentRow.Cells[5].Value.ToString();
            MemoryStream ms = new MemoryStream((byte[])dgv.CurrentRow.Cells[6].Value);
            picBoxPhotoSt.Image = Image.FromStream(ms);

        }
       

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
              
                if (txtBoxName.Text != "")
                {
                    DialogResult confirm = MessageBox.Show("Are you sure?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (confirm == DialogResult.Yes)
                    {
                        int id;
                        id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                        conn.Open();
                        cmd = new SqlCommand("DELETE FROM sm.Students WHERE StudentID= @stId", conn);

                        cmd.Parameters.AddWithValue("@stId", id);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Successfully Deleted");

                        conn.Close();
                        displayGrid();
                        resetAll();
                    }
                }
                else
                {
                    MessageBox.Show("First Select from list");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error......." + ex.Message);

            }
        }

        private void resetAll()
        {
            txtBoxName.Text = "";
            txtBoxAddress.Text = "";
            txtBoxPhotoSt.Text = "";
            dateTimePickerSt.Text = "";
            cmbBoxClass.SelectedIndex = -1;
            cmbBoxGender.SelectedIndex = -1;
            picBoxPhotoSt.Image= null;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBoxName.Text != "" && txtBoxAddress.Text != "" && dateTimePickerSt.Text != "" && cmbBoxGender.Text != "" && cmbBoxClass.Text != "")
                {
                    int id;
                    id = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                    conn.Open();
                    cmd = new SqlCommand("UPDATE sm.Students SET StudentName=@studentname, BirthDate=@birthdate, Gender=@gender, Class=@class, Address=@address, Photo=@photo WHERE StudentID= @stId", conn);

                    cmd.Parameters.AddWithValue("@stId", id);

                    cmd.Parameters.AddWithValue("@studentname", txtBoxName.Text);
                    cmd.Parameters.AddWithValue("@birthdate", DateTime.Parse(dateTimePickerSt.Text));
                    cmd.Parameters.AddWithValue("@gender", cmbBoxGender.SelectedItem);
                    cmd.Parameters.AddWithValue("@class", cmbBoxClass.SelectedItem);
                    cmd.Parameters.AddWithValue("@address", txtBoxAddress.Text);
                    cmd.Parameters.AddWithValue("@photo", getImage());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Data Updated Successfully");


                    conn.Close();
                    displayGrid();
                    resetAll();
                }
                else
                {
                    MessageBox.Show("First Select from list");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error......." + ex.Message);
            }
        }

        private void btnR_Click(object sender, EventArgs e)
        {
            this.Hide();
            Result r = new Result();
            r.ShowDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Report r = new Report();
            r.Show();
            conn.Open();
            cmd = new SqlCommand("SELECT * FROM sm.Students",conn);
            adapter= new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds);
            CrystalReport cr = new CrystalReport(); 
            cr.SetDataSource(ds);
            r.crystalReportViewer1.ReportSource = cr;
            r.crystalReportViewer1.Refresh();
            conn.Close();
        }

        private void btnMarks_Click(object sender, EventArgs e)
        {
            this.Hide();
            Marksheet m = new Marksheet();
            m.ShowDialog();
        }
    }
}

