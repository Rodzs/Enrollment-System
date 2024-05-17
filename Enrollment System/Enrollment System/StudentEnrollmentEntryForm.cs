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
            
        }
        Connection connector = new Connection();

        bool max = false;
        bool searched = false;
        int index = 0;
        int units = 0;
        int totalUnits = 0;
        string day = "";
        string schoolYr = "";
        DateTime start = DateTime.MinValue;
        DateTime end = DateTime.MinValue;

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\Server2\second semester 2023-2024\LAB802\79866_CC_APPSDEV22_1200_0130_PM_TTH\79866-23243801\Desktop\FINALS\Velayo.accdb";
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\Server2\second semester 2023-2024\LAB802\79866_CC_APPSDEV22_1200_0130_PM_TTH\79866-23243801\Desktop\Enrollment-System-main\Velayo.accdb";
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\CODES\C# CODES\Velayo.accdb";
        private void IDNumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {
                    //Connect to Student File
                    connector.DBRead("SELECT * FROM STUDENTFILE");
                    bool trap = false;

                    while (connector.dbDataReader.Read())
                    {
                        if (connector.dbDataReader["STFSTUDID"].ToString().Trim().ToUpper() == IDNumberTextBox.Text.Trim().ToUpper())
                        {
                            trap = true; break;
                        }
                    }
                    if (trap == true)
                    {
                        schoolYr = connector.dbDataReader["STFSTUDYEAR"].ToString();
                        NameLabel.Text = connector.dbDataReader["STFSTUDFNAME"].ToString() + " " + connector.dbDataReader["STFSTUDMNAME"].ToString() + " " + connector.dbDataReader["STFSTUDLNAME"].ToString();
                        CourseLabel.Text = connector.dbDataReader["STFSTUDCOURSE"].ToString();
                        YearLabel.Text = connector.dbDataReader["STFSTUDYEAR"].ToString();
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
                        connector.DBRead("SELECT * FROM SUBJECTSCHEDFILE");
                        //Connect to Subject File
                        OleDbConnection subjectConnection = new OleDbConnection(connectionString);
                        subjectConnection.Open();
                        OleDbCommand subjectCommand = subjectConnection.CreateCommand();

                        string sqlsubject = "SELECT * FROM SUBJECTFILE";
                        subjectCommand.CommandText = sqlsubject;

                        OleDbDataReader subjectDataReader = subjectCommand.ExecuteReader();

                        bool trapEDP = false;
                        bool conflict = false;

                        while (connector.dbDataReader.Read())
                        {
                            //EDP Code Trapper
                            if (connector.dbDataReader["SSFEDPCODE"].ToString().Trim().ToUpper() == EDPCodeTextBox.Text.Trim().ToUpper())
                            {
                                //Max Class Trapper
                                if (Convert.ToInt32(connector.dbDataReader["SSFCLASSSIZE"].ToString()) >= Convert.ToInt32(connector.dbDataReader["SSFMAXSIZE"].ToString()))
                                {
                                    max = true;
                                }
                                while (subjectDataReader.Read())
                                {
                                    if (subjectDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == connector.dbDataReader["SSFSUBJCODE"].ToString().Trim().ToUpper())
                                    {
                                        units = Convert.ToInt32(subjectDataReader["SFSUBJUNITS"]);
                                        break;
                                    }
                                }
                                trapEDP = true; break;
                            }
                        }
                        if (trapEDP == true)
                        {
                            DateTime startTimeSpan = DateTime.Parse(connector.dbDataReader["SSFSTARTTIME"].ToString());
                            DateTime endTimeSpan = DateTime.Parse(connector.dbDataReader["SSFENDTIME"].ToString());
                            string DaysDb = connector.dbDataReader["SSFDAYS"].ToString();

                            foreach (DataGridViewRow existingrows in EnrollmentDataGridView.Rows)
                            {
                                if (!existingrows.IsNewRow)
                                {
                                    start = DateTime.Parse(existingrows.Cells[2].Value.ToString());
                                    end = DateTime.Parse(existingrows.Cells[3].Value.ToString());
                                    day = existingrows.Cells[4].Value.ToString();
                                    if (ConflictChecker(DaysDb, day) && ((start.TimeOfDay > startTimeSpan.TimeOfDay && start.TimeOfDay < endTimeSpan.TimeOfDay) || (end.TimeOfDay > startTimeSpan.TimeOfDay && end.TimeOfDay < endTimeSpan.TimeOfDay) || (start.TimeOfDay < startTimeSpan.TimeOfDay && end.TimeOfDay > endTimeSpan.TimeOfDay)))
                                    {
                                        conflict = true;
                                    }
                                }
                            }
                            if (max == false)
                            {
                                if (conflict == false)
                                {
                                    index = EnrollmentDataGridView.Rows.Add();
                                    EnrollmentDataGridView.Rows[index].Cells["EDPCodeColumn"].Value = connector.dbDataReader["SSFEDPCODE"].ToString();
                                    EnrollmentDataGridView.Rows[index].Cells["SubjectCodeColumn"].Value = connector.dbDataReader["SSFSUBJCODE"].ToString();
                                    EnrollmentDataGridView.Rows[index].Cells["StartTimeColumn"].Value = startTimeSpan.ToString("hh:mm tt");
                                    EnrollmentDataGridView.Rows[index].Cells["EndTimeColumn"].Value = endTimeSpan.ToString("hh:mm tt");
                                    EnrollmentDataGridView.Rows[index].Cells["DaysColumn"].Value = connector.dbDataReader["SSFDAYS"].ToString();
                                    EnrollmentDataGridView.Rows[index].Cells["RoomColumn"].Value = connector.dbDataReader["SSFROOM"].ToString();
                                    EnrollmentDataGridView.Rows[index].Cells["UnitsColumn"].Value = units.ToString();
                                    totalUnits += units;
                                    UnitsLabel.Text = totalUnits.ToString();
                                    EDPCodeTextBox.Text = "";
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Conflicted Time!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("This EDP Code already has the maximum class size!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("No EDP Code found!", "Error!");
                        }
                        max = false;
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
                }
                else if (sched2.Contains("THU"))
                {
                    return true;
                }
                else if (sched2.Contains("SAT"))
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
            bool found = false;
            bool idTrap = false;
            if (IDNumberTextBox.Text != "")
            {
                if (EncoderTextBox.Text != "")
                {
                    //connect detail file
                    OleDbConnection enDetailConnection = new OleDbConnection(connectionString);
                    enDetailConnection.Open();
                    OleDbCommand enDetailCommand = enDetailConnection.CreateCommand();

                    string enDetail = "SELECT * FROM ENROLLMENTDETAILFILE";
                    enDetailCommand.CommandText = enDetail;

                    OleDbDataReader enDetailDataReader = enDetailCommand.ExecuteReader();
                    while (enDetailDataReader.Read())
                    {
                        foreach (DataGridViewRow existingrows in EnrollmentDataGridView.Rows)
                        {
                            if ((enDetailDataReader["ENRDFSTUDID"].ToString().Trim() == IDNumberTextBox.Text.Trim()) && (enDetailDataReader["ENRDFSTUDEDPCODE"].ToString().Trim() == existingrows.Cells[0].Value.ToString().Trim()))
                            {

                                found = true; break;
                            }
                        }
                    }
                    //connect header file
                    OleDbConnection headerConnection = new OleDbConnection(connectionString);
                    headerConnection.Open();
                    OleDbCommand headerCommand = headerConnection.CreateCommand();

                    string header = "SELECT * FROM ENROLLMENTHEADERFILE";
                    headerCommand.CommandText = header;

                    OleDbDataReader headerDataReader = headerCommand.ExecuteReader();
                    while (headerDataReader.Read())
                    {
                        if (headerDataReader["ENRHFSTUDID"].ToString().Trim() == IDNumberTextBox.Text.Trim())
                        {
                            idTrap = true; break;
                        }
                    }

                    if (found == false)
                    {
                        if (max == false)
                        {
                            if (idTrap == false)
                            {
                                string sql = "SELECT * FROM ENROLLMENTDETAILFILE";
                                OleDbDataAdapter enDetailAdapter = new OleDbDataAdapter(sql, connector.dbConnection);
                                OleDbCommandBuilder enDetailBuilder = new OleDbCommandBuilder(enDetailAdapter);

                                DataSet enDetailDataSet = new DataSet();
                                enDetailAdapter.Fill(enDetailDataSet, "EnrollmentDetailFile");

                                foreach (DataGridViewRow existingrows in EnrollmentDataGridView.Rows)
                                {
                                    DataRow enDetailRow = enDetailDataSet.Tables["EnrollmentDetailFile"].NewRow();
                                    enDetailRow["ENRDFSTUDID"] = IDNumberTextBox.Text;
                                    enDetailRow["ENRDFSTUDSUBJCODE"] = existingrows.Cells[1].Value.ToString();
                                    enDetailRow["ENRDFSTUDEDPCODE"] = existingrows.Cells[0].Value.ToString();
                                    connector.DBUpdate("UPDATE SUBJECTSCHEDFILE set SSFCLASSSIZE=" + (Convert.ToInt32(connector.dbDataReader["SSFCLASSSIZE"]) + 1) + " where SSFEDPCODE='" + existingrows.Cells[0].Value.ToString() + "'");

                                    enDetailDataSet.Tables["EnrollmentDetailFile"].Rows.Add(enDetailRow);
                                }
                                enDetailAdapter.Update(enDetailDataSet, "EnrollmentDetailFile");

                                string headerSql = "SELECT * FROM ENROLLMENTHEADERFILE";
                                OleDbDataAdapter headerAdapter = new OleDbDataAdapter(headerSql, connector.dbConnection);
                                OleDbCommandBuilder headerBuilder = new OleDbCommandBuilder(headerAdapter);

                                DataSet headerDataSet = new DataSet();
                                headerAdapter.Fill(headerDataSet, "EnrollmentHeaderFile");

                                DataRow thisRow = headerDataSet.Tables["EnrollmentHeaderFile"].NewRow();
                                thisRow["ENRHFSTUDID"] = IDNumberTextBox.Text;
                                thisRow["ENRHFSTUDDATEENROLL"] = DatesDateTimePicker.Text;
                                thisRow["ENRHFSTUDSCHLYR"] = schoolYr.ToString();
                                thisRow["ENRHFSTUDENCODER"] = EncoderTextBox.Text;
                                thisRow["ENRHFSTUDTOTALUNITS"] = totalUnits.ToString();

                                headerDataSet.Tables["EnrollmentHeaderFile"].Rows.Add(thisRow);
                                headerAdapter.Update(headerDataSet, "EnrollmentHeaderFile");

                                MessageBox.Show("Entries Recorded!");
                                clearText();
                                totalUnits = 0;
                            }
                            else
                            {
                                MessageBox.Show("This Student is already Enrolled!");
                            }

                        }
                        else
                        {
                            MessageBox.Show("This EDP Code already has the maximum class size!");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Inputted ID number and EDP Code is duplicate!");
                    }

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
        private void clearText()
        {
            IDNumberTextBox.Text = "";
            NameLabel.Text = "";
            CourseLabel.Text = "";
            YearLabel.Text = "";
            EDPCodeTextBox.Text = "";
            UnitsLabel.Text = "";
            DatesDateTimePicker.Value = DateTime.Now;
            units = 0;
            totalUnits = 0;
            EncoderTextBox.Text = "";
            EDPCodeTextBox.Enabled = false;
            EnrollmentDataGridView.Rows.Clear();
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all text?", "Clear?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clearText();
            }
        }
    }
}