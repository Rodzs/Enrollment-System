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
    public partial class SubjectScheduleEntryForm : Form
    {
        public SubjectScheduleEntryForm()
        {
            InitializeComponent();
        }
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\SHADOW\Documents\Velayo.accdb";
        private void SaveButton_Click(object sender, EventArgs e)
        {
            TimeSpan startDate = TimeStartDateTimePicker.Value.TimeOfDay;
            string startTime = TimeStartDateTimePicker.Value.ToString("hh:mm");
            TimeSpan endDate = TimeEndDateTimePicker.Value.TimeOfDay;
            string endTime = TimeEndDateTimePicker.Value.ToString("hh:mm");

            try
            {
                OleDbConnection thisConnection = new OleDbConnection(connectionString);
                string sql = "SELECT * FROM SUBJECTSCHEDFILE";
                thisConnection.Open();
                OleDbDataAdapter thisAdapter = new OleDbDataAdapter(sql, thisConnection);
                OleDbCommandBuilder thisBuilder = new OleDbCommandBuilder(thisAdapter);

                DataSet thisDataSet = new DataSet();
                thisAdapter.Fill(thisDataSet, "SubjectSchedFile");

                DataRow thisRow = thisDataSet.Tables["SubjectSchedFile"].NewRow();
                thisRow["SSFEDPCODE"] = SubjectEDPCodeTextBox.Text;
                thisRow["SSFSUBJCODE"] = SubjectCodeTextBox.Text;
                thisRow["SSFSTARTTIME"] = startTime;
                thisRow["SSFENDTIME"] = endTime;
                thisRow["SSFDAYS"] = DaysTextBox.Text;
                thisRow["SSFROOM"] = RoomTextBox.Text;
                thisRow["SSFMAXSIZE"] = MaxSizeTextBox.Text;
                thisRow["SSFCLASSSIZE"] = ClassSizeTextBox.Text;
                //thisRow["SSFSTATUS"] = SubjectCodeTextBox.Text;
                thisRow["SSFXM"] = AmPmComboBox.Text;
                thisRow["SSFSECTION"] = SectionTextBox.Text;
                thisRow["SSFSCHOOLYEAR"] = SchoolYearTextBox.Text;


                thisDataSet.Tables["SubjectSchedFile"].Rows.Add(thisRow);
                thisAdapter.Update(thisDataSet, "SubjectSchedFile");

                MessageBox.Show("Entries Recorded!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SubjectScheduleEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            MenuForm menu = new MenuForm();
            menu.Show();
            this.Hide();
        }

        private void SubjectScheduleEntryForm_Load(object sender, EventArgs e)
        {
            TimeStartDateTimePicker.Format = DateTimePickerFormat.Time;
            TimeStartDateTimePicker.ShowUpDown = true;
            TimeEndDateTimePicker.Format = DateTimePickerFormat.Time;
            TimeEndDateTimePicker.ShowUpDown = true;
        }
    }
}
