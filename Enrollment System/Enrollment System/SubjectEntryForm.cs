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

namespace Enrollment_System
{
    public partial class SubjectEntryForm : Form
    {
        public SubjectEntryForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'velayoDataSet.SubjectFile' table. You can move, or remove it, as needed.
            //this.subjectFileTableAdapter.Fill(this.velayoDataSet.SubjectFile);

        }

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\CODES\C# CODES\Velayo.accdb";
        private void SaveButton_Click(object sender, EventArgs e)
        {
            OleDbConnection thisConnection = new OleDbConnection(connectionString);
            string sql = "SELECT * FROM SUBJECTFILE";
            thisConnection.Open();
            OleDbCommand thisCommand = thisConnection.CreateCommand();
            thisCommand.CommandText = sql;
            OleDbDataReader thisDataReader = thisCommand.ExecuteReader();

            Boolean dupe = false;
            for (;thisDataReader.Read();)
            {
                if (thisDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == SubjectCodeTextBox.Text.Trim().ToUpper())
                {
                    dupe = true;
                    break;
                }
            }
            thisConnection.Close();
            if (dupe == true)
            {
                MessageBox.Show("This Subject Code has a duplicate!");
            }
            else
            {
                try
                {

                    OleDbDataAdapter thisAdapter = new OleDbDataAdapter(sql, thisConnection);
                    OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);

                    DataSet thisDataSet = new DataSet();
                    thisAdapter.Fill(thisDataSet, "SubjectFile");

                    DataRow thisRow = thisDataSet.Tables["SubjectFile"].NewRow();
                    thisRow["SFSUBJCODE"] = SubjectCodeTextBox.Text;
                    thisRow["SFSUBJDESC"] = DescriptionTextBox.Text;
                    thisRow["SFSUBJUNITS"] = Convert.ToInt16(UnitsTextBox.Text);
                    thisRow["SFSUBJREGOFRNG"] = Convert.ToInt16(OfferingComboBox.Text.Substring(0, 1));
                    thisRow["SFSUBJCATEGORY"] = CategoryComboBox.Text.Substring(0, 3);
                    thisRow["SFSUBJSTATUS"] = StatusComboBox.Text.Substring(0, 2);
                    thisRow["SFSUBJCOURSECODE"] = CourseCodeComboBox.Text;
                    thisRow["SFSUBJCURRCODE"] = CurriculumYearTextBox.Text;

                    thisDataSet.Tables["SubjectFile"].Rows.Add(thisRow);
                    thisAdapter.Update(thisDataSet, "SubjectFile");

                    //TODO: Save DATA TO SUBJPREQFILE

                    if ((PrerequisiteRadioButton.Checked || CorequisiteRadioButton.Checked) && 
                        (SubjectCodeRequisiteTextBox.Text != null && SubjectCodeRequisiteTextBox.Text != ""))
                    {

                        OleDbConnection requisiteConnection = new OleDbConnection(connectionString);
                        requisiteConnection.Open();
                        string[] prerequisites = SubjectCodeRequisiteTextBox.Text.Split(',');
                        foreach (string prerequisite in prerequisites)
                        {
                            string requisiteSql = "INSERT INTO SubjectPreqFile (SUBJCODE, SUBJPRECODE, SUBJCATEGORY) VALUES " +
                                "("+ SubjectCodeTextBox.Text +", "+ prerequisite.Trim() + ", @SUBJCATEGORY)";
                            OleDbCommand cmd = new OleDbCommand(requisiteSql, requisiteConnection);
                            cmd.Parameters.AddWithValue("SUBJCATEGORY", PrerequisiteRadioButton.Checked ? "PR" : "CO");
                            cmd.ExecuteNonQuery();
                        }
                        //string requisite = "SELECT * FROM SUBJECTPREQFILE";
                        //OleDbDataAdapter requisiteAdapter = new OleDbDataAdapter(requisite, requisiteConnection);
                        //OleDbCommandBuilder requisiteBuilder = new OleDbCommandBuilder(requisiteAdapter);

                        //DataSet requisiteDataSet = new DataSet();
                        //requisiteAdapter.Fill(requisiteDataSet, "SubjectPreqFile");

                        //DataRow requisiteRow = requisiteDataSet.Tables["SubjectPreqFile"].NewRow();
                        //requisiteRow["SUBJCODE"] = SubjectCodeTextBox.Text;
                        //requisiteRow["SUBJPRECODE"] = SubjectCodeRequisiteTextBox.Text;
                        //if (PrerequisiteRadioButton.Checked)
                        //{
                        //    requisiteRow["SUBJCATEGORY"] = "PR";
                        //}
                        //else if (CorequisiteRadioButton.Checked)
                        //{
                        //    requisiteRow["SUBJCATEGORY"] = "CO";
                        //}

                        //requisiteDataSet.Tables["SubjectPreqFile"].Rows.Add(requisiteRow);
                        //requisiteAdapter.Update(requisiteDataSet, "SubjectPreqFile");
                    }
                    PrerequisiteRadioButton.Checked = false;
                    CorequisiteRadioButton.Checked = false;
                    MessageBox.Show("Entries Recorded");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SubjectCodeRequisiteTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SubjectDataGridView.Rows.Clear();

                //Connect to SubjectFile
                OleDbConnection thisConnection = new OleDbConnection(connectionString);
                thisConnection.Open();
                OleDbCommand thisCommand = thisConnection.CreateCommand();

                string sql = "SELECT * FROM SUBJECTFILE";
                thisCommand.CommandText = sql;

                OleDbDataReader thisDataReader = thisCommand.ExecuteReader();

                //Connect to SubjectPreqFile
                OleDbConnection requisiteConnection = new OleDbConnection(connectionString);
                requisiteConnection.Open();
                OleDbCommand requisiteCommand = requisiteConnection.CreateCommand();

                string requisite = "SELECT * FROM SUBJECTPREQFILE";
                requisiteCommand.CommandText = requisite;

                OleDbDataReader requisiteDataReader = requisiteCommand.ExecuteReader();
                

                bool found = false;
                string subjectCode = "";
                string description = "";
                string units = "";
                string copre = "";

                while (thisDataReader.Read())
                {
                    // MessageBox.Show(thisDataReader["SFSUBJCODE"].ToString());
                    if (thisDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == SubjectCodeRequisiteTextBox.Text.Trim().ToUpper())
                    {
                        found = true;
                        subjectCode = thisDataReader["SFSUBJCODE"].ToString();
                        description = thisDataReader["SFSUBJDESC"].ToString();
                        units = thisDataReader["SFSUBJUNITS"].ToString();
                        break;
                    }
                }
                while (requisiteDataReader.Read())
                {
                    if (requisiteDataReader["SUBJCODE"].ToString().Trim().ToUpper() == SubjectCodeRequisiteTextBox.Text.Trim().ToUpper())
                    {
                        copre = requisiteDataReader["SUBJPRECODE"].ToString();
                        //subjectpreCode = requisiteDataReader["SUBJPRECODE"].ToString();
                        break;
                    }
                }

                int index = 0;
                
                if (found == false)
                {
                    MessageBox.Show("Subject Code Not Found");
                }
                else
                {
                    index = SubjectDataGridView.Rows.Add();
                    SubjectDataGridView.Rows[index].Cells["SubjectCodeColumn"].Value = subjectCode;
                    SubjectDataGridView.Rows[index].Cells["DescriptionColumn"].Value = description;
                    SubjectDataGridView.Rows[index].Cells["UnitsColumn"].Value = units;
                    SubjectDataGridView.Rows[index].Cells["CoPrerequisiteColumn"].Value = copre;
                    
                }
            }
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            MenuForm menu = new MenuForm();
            menu.Show();
            this.Hide();
        }

        private void SubjectEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            SubjectCodeTextBox.Text = null;
            DescriptionTextBox.Text = null;
            UnitsTextBox.Text = null;
            OfferingComboBox.Text = null;
            CategoryComboBox.Text = null;
            StatusComboBox.Text = null;
            CourseCodeComboBox.Text = null;
            CurriculumYearTextBox.Text = null;
            SubjectCodeRequisiteTextBox.Text = null;
        }
    }
}
