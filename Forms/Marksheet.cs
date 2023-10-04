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
    public partial class Marksheet : Form
    {
        SqlConnection conn;
        DataSet ds;

        SqlDataAdapter dP;
        SqlDataAdapter dC;
        public Marksheet()
        {
            InitializeComponent();
            string myConnectionString = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString;
            conn = new SqlConnection(myConnectionString);
        }

        private void Marksheet_Load(object sender, EventArgs e)
        {

            dP = new SqlDataAdapter("Select * From sm.Students", conn);
            dC = new SqlDataAdapter("Select * From sm.Marks", conn);
            ds = new DataSet();
            dP.Fill(ds, "sm.Students");
            dC.Fill(ds, "sm.Marks");
            ds.Tables["sm.Students"].Constraints.Add("StudentID_PK", ds.Tables["sm.Students"].Columns["StudentID"], true);
            ds.Relations.Add("Students_Marks", ds.Tables["sm.Students"].Columns["StudentID"], ds.Tables["sm.Marks"].Columns["stdID"]);
            dataGridViewParent.DataSource = ds.Tables["sm.Students"];
            LoadChildData(0);
        }
        private void LoadChildData(int rowIndex)
        {
            var parentRow = ds.Tables["sm.Students"].Rows[rowIndex];
            var childRows = parentRow.GetChildRows("Students_Marks");
            DataTable childTable = ds.Tables["sm.Marks"].Clone();
            foreach (var row in childRows)
            {
                childTable.ImportRow(row);
            }
            dataGridViewChild.DataSource = childTable;
        }

        private void btnResult_Click(object sender, EventArgs e)
        {
            this.Hide();
            Result r = new Result();
            r.ShowDialog();
        }

        private void dataGridViewParent_SelectionChanged(object sender, EventArgs e)
        {
            LoadChildData(dataGridViewParent.CurrentRow.Index);
        }
    }
}
