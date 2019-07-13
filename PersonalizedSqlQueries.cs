//Imports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;
using System.Data.SqlClient;


namespace frmTitles
{
    public partial class PersonalizedSqlQueries : Form
    {
      //Connection Object
        SqlConnection myConnection;
        public PersonalizedSqlQueries()
        {
            InitializeComponent();
        }

        private void PersonalizedSqlQueries_Load(object sender, EventArgs e)
        {
            //ConnecyionStrings.com according to the database that we want to connect
            myConnection = new SqlConnection(
               "Server=35.239.55.42;" +
               "Database=PODS_DB_1; " +
               "User id=analytics-admin;" +
               "Password=project2019.;");

            myConnection.Open();

            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Execute_Click(object sender, EventArgs e)
        {
            
            SqlCommand commando;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            try
            {
                commando = new SqlCommand(Command.Text, myConnection);
                adapter.SelectCommand = commando;

                adapter.Fill(table);
                dataGridView1.DataSource = table;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in SQL COMMAND", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
    }
}
