using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enrollment_System
{
    public partial class StudentEnrollmentEntryForm : Form
    {
        public StudentEnrollmentEntryForm()
        {
            InitializeComponent();
            DBConnect();
        }
        ArrayList[] startTime = new ArrayList[10];

        OleDbConnection dbConnection;
        OleDbCommand dbCommand;
        OleDbDataReader dbDataReader;
        private void DBConnect()
        {
            dbConnection = new OleDbConnection(connectionString);
            dbConnection.Open();
        }
        private void DBRead(string query)
        {
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = query;
            dbDataReader = dbCommand.ExecuteReader();
        }

        private void DBUpdate(string query)
        {
            dbConnection = new OleDbConnection(connectionString);
            dbConnection.Open();
            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = query;
            dbCommand.ExecuteReader();
            dbConnection.Close();
        }

        bool searched = false;
        int index = 0;
        int units = 0;
        int totalUnits = 0;
        string day = "";
        DateTime start = DateTime.MinValue;
        DateTime end = DateTime.MinValue;
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\CODES\C# CODES\Velayo.accdb";
        private void IDNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    //Connect to Student File
                    DBRead("SELECT * FROM STUDENTFILE");
                    bool trap = false;
                    while (dbDataReader.Read())
                    {
                        if (dbDataReader["STFSTUDID"].ToString().Trim().ToUpper() == IDNumberTextBox.Text.Trim().ToUpper())
                        {
                            trap = true; break;
                        }
                    }
                    if (trap == true)
                    {
                        NameLabel.Text = dbDataReader["STFSTUDFNAME"].ToString()+ " " + dbDataReader["STFSTUDMNAME"].ToString() + " " + dbDataReader["STFSTUDLNAME"].ToString();
                        CourseLabel.Text = dbDataReader["STFSTUDCOURSE"].ToString();
                        YearLabel.Text = dbDataReader["STFSTUDYEAR"].ToString();
                        searched = true;
                        EDPCodeTextBox.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("No student found!", "Error!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            MenuForm menuForm = new MenuForm();
            menuForm.Show();
            this.Hide();
        }

        private void StudentEnrollmentEntryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void EDPCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (MessageBox.Show("Are you sure you want to add this subject?", "Add?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        DBRead("SELECT * FROM SUBJECTSCHEDFILE");
                        //Connect to Subject File
                        OleDbConnection subjectConnection = new OleDbConnection(connectionString);
                        subjectConnection.Open();
                        OleDbCommand subjectCommand = subjectConnection.CreateCommand();

                        string sqlsubject = "SELECT * FROM SUBJECTFILE";
                        subjectCommand.CommandText = sqlsubject;

                        OleDbDataReader subjectDataReader = subjectCommand.ExecuteReader();
                        
                        bool trap = false;
                        bool conflict = false;

                        while (dbDataReader.Read())
                        {
                            if (dbDataReader["SSFEDPCODE"].ToString().Trim().ToUpper() == EDPCodeTextBox.Text.Trim().ToUpper())
                            {
                                while (subjectDataReader.Read())
                                {
                                    if (subjectDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == dbDataReader["SSFSUBJCODE"].ToString().Trim().ToUpper())
                                    {
                                        units = Convert.ToInt32(subjectDataReader["SFSUBJUNITS"]);
                                    }
                                }
                                trap = true; break;
                            }
                        }
                        DateTime startTimeSpan = DateTime.Parse(dbDataReader["SSFSTARTTIME"].ToString());
                        DateTime endTimeSpan = DateTime.Parse(dbDataReader["SSFENDTIME"].ToString());
                        string DaysDb = dbDataReader["SSFDAYS"].ToString();

                        if (trap == true)
                        {
                            foreach (DataGridViewRow existingrows in EnrollmentDataGridView.Rows)
                            { 
                                if (!existingrows.IsNewRow)
                                {
                                    start = DateTime.Parse(existingrows.Cells[2].Value.ToString());
                                    end = DateTime.Parse(existingrows.Cells[3].Value.ToString());
                                    day = existingrows.Cells[4].Value.ToString();
                                    //MessageBox.Show(start.ToString());
                                    //MessageBox.Show(end.ToString());
                                    //MessageBox.Show(ConflictChecker(day, DaysDb).ToString());
                                    if (ConflictChecker(DaysDb, day) && ((start.TimeOfDay > startTimeSpan.TimeOfDay && start.TimeOfDay <= endTimeSpan.TimeOfDay) || (end.TimeOfDay > startTimeSpan.TimeOfDay && end.TimeOfDay <= endTimeSpan.TimeOfDay) || (start.TimeOfDay < startTimeSpan.TimeOfDay && end.TimeOfDay > endTimeSpan.TimeOfDay)))
                                    {
                                        conflict = true;
                                    }
                                }
                            }
                            if (conflict == false)
                            {
                                index = EnrollmentDataGridView.Rows.Add();
                                EnrollmentDataGridView.Rows[index].Cells["EDPCodeColumn"].Value = dbDataReader["SSFEDPCODE"].ToString();
                                EnrollmentDataGridView.Rows[index].Cells["SubjectCodeColumn"].Value = dbDataReader["SSFSUBJCODE"].ToString();
                                EnrollmentDataGridView.Rows[index].Cells["StartTimeColumn"].Value = startTimeSpan.ToString("hh:mm tt");
                                EnrollmentDataGridView.Rows[index].Cells["EndTimeColumn"].Value = endTimeSpan.ToString("hh:mm tt");
                                EnrollmentDataGridView.Rows[index].Cells["DaysColumn"].Value = dbDataReader["SSFDAYS"].ToString();
                                EnrollmentDataGridView.Rows[index].Cells["RoomColumn"].Value = dbDataReader["SSFROOM"].ToString();
                                EnrollmentDataGridView.Rows[index].Cells["UnitsColumn"].Value = units.ToString();
                                totalUnits += units;
                                UnitsLabel.Text = totalUnits.ToString();
                                EDPCodeTextBox.Text = "";
                            }
                            else
                            {
                                MessageBox.Show("Conflict!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No EDP Code found!", "Error!");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        static bool ConflictChecker(string sched1, string sched2)
        {
            if (sched1.Contains("TTHS") || sched1.Contains("TTH"))
            {
                if (sched2.Contains("TUE"))
                {
                    return true;
                }else if (sched2.Contains("THU"))
                {
                    return true;
                }else if (sched2.Contains("SAT"))
                {
                    return true;
                }
            }
            if (sched2.Contains("TTHS") || sched2.Contains("TTH"))
            {
                if (sched1.Contains("TUE"))
                {
                    return true;
                }
                else if (sched1.Contains("THU"))
                {
                    return true;
                }
                else if (sched1.Contains("SAT"))
                {
                    return true;
                }
            }
            if (sched1.Contains("MWF"))
            {
                if (sched2.Contains("MON"))
                {
                    return true;
                }
                else if (sched2.Contains("WED"))
                {
                    return true;
                }
                else if (sched2.Contains("FRI"))
                {
                    return true;
                }
            }
            if (sched2.Contains("MWF"))
            {
                if (sched1.Contains("MON"))
                {
                    return true;
                }
                else if (sched1.Contains("WED"))
                {
                    return true;
                }
                else if (sched1.Contains("FRI"))
                {
                    return true;
                }
            }
            if (sched1 == sched2)
            {
                return true;
            }
            return false;
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            bool added = false;
            if (IDNumberTextBox.Text != "")
            {
                if (EncoderTextBox.Text != "")
                {
                    OleDbConnection enDetailConnection = new OleDbConnection(connectionString);
                    string sql = "SELECT * FROM ENROLLMENTDETAILFILE";
                    enDetailConnection.Open();
                    OleDbDataAdapter enDetailAdapter = new OleDbDataAdapter(sql, enDetailConnection);
                    OleDbCommandBuilder enDetailBuilder = new OleDbCommandBuilder(enDetailAdapter);

                    DataSet enDetailDataSet = new DataSet();
                    enDetailAdapter.Fill(enDetailDataSet, "EnrollmentDetailFile");

                    foreach (DataGridViewRow existingrows in EnrollmentDataGridView.Rows)
                    {
                        DataRow enDetailRow = enDetailDataSet.Tables["EnrollmentDetailFile"].NewRow();
                        enDetailRow["ENRDFSTUDID"] = IDNumberTextBox.Text;
                        enDetailRow["ENRDFSTUDSUBJCODE"] = existingrows.Cells[1].Value.ToString();
                        enDetailRow["ENRDFSTUDEDPCODE"] = existingrows.Cells[0].Value.ToString();
                        DBUpdate("UPDATE SUBJECTSCHEDFILE set SSFCLASSSIZE=" + (Convert.ToInt32(dbDataReader["SSFCLASSSIZE"]) + 1) + " where SSFEDPCODE='" + existingrows.Cells[0].Value.ToString() + "'");

                        enDetailDataSet.Tables["EnrollmentDetailFile"].Rows.Add(enDetailRow);
                    }
                    enDetailAdapter.Update(enDetailDataSet, "EnrollmentDetailFile");

                    MessageBox.Show("Entries Recorded!");
                }
                else
                {
                    MessageBox.Show("Please enter name of the Encoder!");
                }
            }
            else
            {
                MessageBox.Show("Please enter student ID Number first!");
            }
           
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all text?", "Clear?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IDNumberTextBox.Text = "";
                NameLabel.Text = "";
                CourseLabel.Text = "";
                YearLabel.Text = "";
                EDPCodeTextBox.Text = "";
                UnitsLabel.Text = "";
                units = 0;
                EncoderTextBox.Text = "";
                EDPCodeTextBox.Enabled = false;
                EnrollmentDataGridView.Rows.Clear();
            }
        }
    }
}
