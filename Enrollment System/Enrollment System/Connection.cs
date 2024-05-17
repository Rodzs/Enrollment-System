using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enrollment_System
{
    class Connection
    {
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\CODES\C# CODES\Velayo.accdb";
        //string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\SHADOW\Desktop\Enrollment-System-main\Velayo.accdb";
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\\Server2\second semester 2023-2024\LAB802\79866_CC_APPSDEV22_1200_0130_PM_TTH\79866-23243801\Desktop\FINALS\Velayo.accdb";

        
        public OleDbConnection dbConnection;
        public OleDbCommand dbCommand;
        public OleDbDataReader dbDataReader;

        public void DBRead(string query)
        {

            dbConnection = new OleDbConnection(connectionString);
            dbConnection.Open();
            dbCommand = dbConnection.CreateCommand();
            dbCommand.CommandText = query;
            dbDataReader = dbCommand.ExecuteReader();
        }

        public void DBUpdate(string query)
        {
            dbConnection = new OleDbConnection(connectionString);
            dbConnection.Open();
            dbCommand.Connection = dbConnection;
            dbCommand.CommandText = query;
            dbCommand.ExecuteReader();
            dbConnection.Close();
        }
    }
}
