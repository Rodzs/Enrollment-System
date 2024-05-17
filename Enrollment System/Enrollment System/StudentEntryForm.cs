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
    public partial class StudentEntryForm : Form
    {
        public StudentEntryForm()
        {
            InitializeComponent();
        }
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\SHADOW\Desktop\Enrollment-System-main\Velayo.accdb";
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\Server2\second semester 2023-2024\LAB802\79866_CC_APPSDEV22_1200_0130_PM_TTH\79866-23243801\Desktop\FINALS\Velayo.accdb";
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\CODES\C# CODES\Velayo.accdb";
        private void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                OleDbConnection thisConnection = new OleDbConnection(connectionString);
                string sql = "SELECT * FROM STUDENTFILE";
                thisConnection.Open();
                OleDbDataAdapter thisAdapter = new OleDbDataAdapter(sql, thisConnection);
                OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);

                DataSet thisDataSet = new DataSet();
                thisAdapter.Fill(thisDataSet, "StudentFile");

                DataRow thisRow = thisDataSet.Tables["StudentFile"].NewRow();
                thisRow["STFSTUDID"] = IDNumberTextBox.Text;
                thisRow["STFSTUDFNAME"] = FirstNameTextBox.Text;
                thisRow["STFSTUDMNAME"] = MiddleInitialTextBox.Text;
                thisRow["STFSTUDLNAME"] = LastNameTextBox.Text;
                thisRow["STFSTUDCOURSE"] = CourseTextBox.Text;
                thisRow["STFSTUDYEAR"] = YearTextBox.Text;
                thisRow["STFSTUDREMARKS"] = RemarksComboBox.Text;

                thisDataSet.Tables["StudentFile"].Rows.Add(thisRow);
                thisAdapter.Update(thisDataSet, "StudentFile");

                MessageBox.Show("Entries Recorded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            MenuForm menuForm = new MenuForm();
            menuForm.Show();
            this.Hide();
        }

        private void StudentEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            IDNumberTextBox.Text = null;
            FirstNameTextBox.Text = null;
            MiddleInitialTextBox.Text = null;
            LastNameTextBox.Text = null;
            CourseTextBox.Text = null;
            YearTextBox.Text = null;
            RemarksComboBox.Text = null;
        }
    }
}
