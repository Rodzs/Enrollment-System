﻿using System;
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
            TimeFormater();
        }
        bool edpTrap = false;

        Connection connector = new Connection();
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\SHADOW\Desktop\Enrollment-System-main\Velayo.accdb";
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\CODES\C# CODES\Velayo.accdb";
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\Server2\second semester 2023-2024\LAB802\79866_CC_APPSDEV22_1200_0130_PM_TTH\79866-23243801\Desktop\FINALS\Velayo.accdb";
        private void SaveButton_Click(object sender, EventArgs e)
        {
            TimeSpan startDate = TimeStartDateTimePicker.Value.TimeOfDay;
            string startTime = TimeStartDateTimePicker.Value.ToString("hh:mm");
            TimeSpan endDate = TimeEndDateTimePicker.Value.TimeOfDay;
            string endTime = TimeEndDateTimePicker.Value.ToString("hh:mm");
            try
            {
                connector.DBRead("SELECT * FROM SUBJECTSCHEDFILE");
                while (connector.dbDataReader.Read())
                {
                    if (connector.dbDataReader["SSFEDPCODE"].ToString().Trim() == SubjectEDPCodeTextBox.Text.Trim())
                    {
                        edpTrap = true; break;
                    }
                }
                if (SubjectEDPCodeTextBox.Text != "" || SubjectCodeTextBox.Text != "" || DaysTextBox.Text != "" || RoomTextBox.Text != "" ||
                TimeStartAmPmComboBox.Text != "" || TimeEndAmPmComboBox.Text != "" || SectionTextBox.Text != "" || SchoolYearTextBox.Text != "")
                {
                    if (edpTrap == false)
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
                        thisRow["SSFSUBJCODE"] = SubjectCodeTextBox.Text.ToUpper();
                        thisRow["SSFSTARTTIME"] = startTime + TimeStartAmPmComboBox.Text;
                        thisRow["SSFENDTIME"] = endTime + TimeEndAmPmComboBox.Text;
                        thisRow["SSFDAYS"] = DaysTextBox.Text;
                        thisRow["SSFROOM"] = RoomTextBox.Text;
                        thisRow["SSFMAXSIZE"] = "50";
                        thisRow["SSFCLASSSIZE"] = "0";
                        thisRow["SSFSTATUS"] = "AC";
                        thisRow["SSFSECTION"] = SectionTextBox.Text;
                        thisRow["SSFSCHOOLYEAR"] = SchoolYearTextBox.Text;


                        thisDataSet.Tables["SubjectSchedFile"].Rows.Add(thisRow);
                        thisAdapter.Update(thisDataSet, "SubjectSchedFile");

                        MessageBox.Show("Entries Recorded!");
                    }
                    else
                    {
                        MessageBox.Show("This EDP Code is already enrolled!");
                    }
                }
                else
                {
                    MessageBox.Show("Please input necessary details in the textbox!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            edpTrap = false;
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
        private void TimeFormater()
        {
            TimeStartDateTimePicker.Format = DateTimePickerFormat.Custom;
            TimeStartDateTimePicker.CustomFormat = "hh:mm";
            TimeStartDateTimePicker.ShowUpDown = true;
            TimeEndDateTimePicker.Format = DateTimePickerFormat.Custom;
            TimeEndDateTimePicker.CustomFormat = "hh:mm";
            TimeEndDateTimePicker.ShowUpDown = true;
        }
        private void SubjectScheduleEntryForm_Load(object sender, EventArgs e)
        {
            
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear all data?", "Clear?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SubjectEDPCodeTextBox.Text = null;
                SubjectCodeTextBox.Text = null;
                TimeStartDateTimePicker.Text = "12:00";
                TimeEndDateTimePicker.Text = "12:00";
                DaysTextBox.Text = null;
                RoomTextBox.Text = null;
                DescriptionLabel.Text = null;
                TimeStartAmPmComboBox.Text = null;
                TimeEndAmPmComboBox.Text = null;
                SectionTextBox.Text = null;
                SchoolYearTextBox.Text = null;
            }
        }

        private void SubjectCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                try
                {


                    OleDbConnection subjectConnection = new OleDbConnection(connectionString);
                    subjectConnection.Open();
                    OleDbCommand subjectCommand = subjectConnection.CreateCommand();

                    string sql = "SELECT * FROM SUBJECTFILE";
                    subjectCommand.CommandText = sql;

                    OleDbDataReader subjectDataReader = subjectCommand.ExecuteReader();
                    bool trap = false;
                    while (subjectDataReader.Read())
                    {
                        if (subjectDataReader["SFSUBJCODE"].ToString().Trim().ToUpper() == SubjectCodeTextBox.Text.Trim().ToUpper())
                        {
                            trap = true; break;
                        }
                    }
                    
                    if (trap == true)
                    {
                        DescriptionLabel.Text = subjectDataReader["SFSUBJDESC"].ToString();
                    }
                    else
                    {

                        MessageBox.Show("Subject not found!", "Error!");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SubjectEDPCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }
    }
}
