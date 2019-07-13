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
    public partial class POD : Form
    {


        //Connection Object
        SqlConnection myConnection;
        //Table Command 
        SqlCommand commandBaby;
        //Data Adapter Object

        SqlDataAdapter dataAdapterBaby;

        //Data Table
        DataTable TokenWordTable;

        //Manage next, previous, last, first
        CurrencyManager TokenWordManager;

        SqlCommandBuilder builderCommandBaby;

        string state = " ";
        public int CurrentPosition { get; set; }

        public POD()
        {
            InitializeComponent();
        }

        private void POD_Load(object sender, EventArgs e)
        {
            //ConnecyionStrings.com according to the database that we want to connect
            myConnection = new SqlConnection(
               "Server=35.239.55.42;" +
               "Database=PODS_DB_1; " +
               "User id=analytics-admin;" +
               "Password=project2019.;");

            myConnection.Open();

            SqlCommand command;
            SqlDataReader dataReader;
            String sql;

            sql = "Select * from POD";
            command = new SqlCommand(sql, myConnection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            da.Fill(table);


            //Query that you want to perform (data that is needed from the table)
            commandBaby = new SqlCommand(sql, myConnection);
            //
            dataAdapterBaby = new SqlDataAdapter();

            dataAdapterBaby.SelectCommand = commandBaby;

            //
            TokenWordTable = new DataTable();
            dataAdapterBaby.Fill(TokenWordTable);

            //BIND CONTROLS (this are the columns names as they apper on Access Db)
            textID.DataBindings.Add("Text", TokenWordTable, "ID");
            textDescription.DataBindings.Add("Text", TokenWordTable, "Description");
            textStatus.DataBindings.Add("Text", TokenWordTable, "status");
            textExplanation.DataBindings.Add("Text", TokenWordTable, "explaination");

            //Establish currency manager (Naviigate thouthg the records)
            TokenWordManager = (CurrencyManager)BindingContext[TokenWordTable];
        }



        private void buttonFirst_Click(object sender, EventArgs e)
        {
            //First position on list 
            TokenWordManager.Position = 0;

        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            TokenWordManager.Position--;

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            TokenWordManager.Position++;

        }

        private void buttonLast_Click(object sender, EventArgs e)
        {//Last Record on list

            TokenWordManager.Position = TokenWordManager.Count - 1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

         // Once edit / update is done, save wiill commit changes to the db
            TokenWordManager.EndCurrentEdit();

            builderCommandBaby = new SqlCommandBuilder(dataAdapterBaby);

            dataAdapterBaby.Update(TokenWordTable);

            MessageBox.Show("Record saved", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DialogResult response;
            response = MessageBox.Show("Sure ??", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (response == DialogResult.No)
            {
                return;
            }
            else
            {
                //Remove a record from a specified index
                TokenWordManager.RemoveAt(TokenWordManager.Position);
                builderCommandBaby = new SqlCommandBuilder(dataAdapterBaby);
                dataAdapterBaby.Update(TokenWordTable);


                return;


            }
        }

        private void AddNewButton_Click(object sender, EventArgs e)
        {
            //Add new 
            CurrentPosition = TokenWordManager.Position;
            TokenWordManager.AddNew();
            state = "add";
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            textDescription.ReadOnly = false;
            textDescription.Focus();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {

            if (state == "add")
            {
                TokenWordManager.CancelCurrentEdit();
                TokenWordManager.Position = CurrentPosition;
            }
            else
            {
                TokenWordManager.CancelCurrentEdit();

            }

            MessageBox.Show("Transaction aborted ", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void GoBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            myConnection.Close();
            myConnection.Dispose();
        }
    }
}
