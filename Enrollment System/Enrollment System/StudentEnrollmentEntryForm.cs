using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }

        OleDbConnection dbConnection;
        OleDbCommand dbCommand;
        OleDbDataReader dbDataReader;
        private void DBRead(string query)
        {
            dbConnection = new OleDbConnection(connectionString);
            dbConnection.Open();
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
        DateTime? start = null;
        DateTime? end = null;
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\CODES\C# CODES\Velayo.accdb";
        private void IDNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    //Connect to Student File
                    OleDbConnection studentConnection = new OleDbConnection(connectionString);
                    studentConnection.Open();
                    OleDbCommand studentCommand = studentConnection.CreateCommand();

                    string sql = "SELECT * FROM STUDENTFILE";
                    studentCommand.CommandText = sql;

                    OleDbDataReader studentDataReader = studentCommand.ExecuteReader();
                    bool trap = false;
                    while (studentDataReader.Read())
                    {
                        if (studentDataReader["STFSTUDID"].ToString().Trim().ToUpper() == IDNumberTextBox.Text.Trim().ToUpper())
                        {
                            trap = true; break;
                        }
                    }
                    if (trap == true)
                    {
                        NameLabel.Text = studentDataReader["STFSTUDFNAME"].ToString()+ " " + studentDataReader["STFSTUDMNAME"].ToString() + " " + studentDataReader["STFSTUDLNAME"].ToString();
                        CourseLabel.Text = studentDataReader["STFSTUDCOURSE"].ToString();
                        YearLabel.Text = studentDataReader["STFSTUDYEAR"].ToString();
                        searched = true;

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
                        bool added = false;

                        //DateTime startDateTime = DateTime.Parse(subschedDataReader["SSFSTARTTIME"].ToString());
                        //DateTime endDateTime = DateTime.Parse(subschedDataReader["SSFENDTIME"].ToString());
                        //string startTime = startDateTime.ToString("hh:mm tt");
                        //string endTime = endDateTime.ToString("hh:mm tt");

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
                        //Work around for date problems
                        dbCommand.Parameters.AddWithValue("@StartTime", DateTime.Parse("1/1/2001 " + startTimeSpan.ToString("hh:mm tt")));
                        dbCommand.Parameters.AddWithValue("@EndTime", DateTime.Parse("1/1/2001 " + endTimeSpan.ToString("hh:mm tt")));
                        dbCommand.Parameters.AddWithValue("@EDPCODE", dbDataReader["SSFEDPCODE"].ToString());
                        DBUpdate("UPDATE SUBJECTSCHEDFILE SET SSFSTARTTIME = @StartTime, SSFENDTIME = @EndTime WHERE SSFEDPCODE = @EDPCODE");
                        //MessageBox.Show(startTime.ToString());
                        //MessageBox.Show(endTime.ToString());

                        if (trap == true)
                        {
                            if ((start >= startTimeSpan && start <= endTimeSpan) || (end > startTimeSpan && end <= endTimeSpan) || (start < startTimeSpan && end > endTimeSpan))
                            {
                                MessageBox.Show("Conflict!!!");
                            }
                            else
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
                            }
                        }
                        else
                        {
                            MessageBox.Show("No EDP Code found!", "Error!");
                        }

                        
                        //MessageBox.Show(startTimeSpan.ToString());
                        //MessageBox.Show(endTimeSpan.ToString());
                        //MessageBox.Show(start.ToString());
                        //MessageBox.Show(end.ToString());
                        start = startTimeSpan;
                        end = endTimeSpan;
                        added = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            
        }
    }
}
