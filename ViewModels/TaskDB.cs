using System;
using System.ComponentModel;
using System.Data.OleDb;
using Task_Manager.Models;

namespace Task_Manager.ViewModels
{
    class TaskDB
    {

        private string ConnectionString;

        public TaskDB(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public BindingList<TaskItem> ListTasks()
        {
            BindingList<TaskItem> taskItems = new BindingList<TaskItem>();
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string sqlString = "SELECT * FROM Tasks";

                    using (OleDbCommand command = new OleDbCommand(sqlString, connection))
                    {
                        int result = command.ExecuteNonQuery();
                        using (OleDbDataReader dbDataReader = command.ExecuteReader())
                        {
                            while (dbDataReader.Read())
                            {
                                taskItems.Add(
                                new TaskItem
                                {
                                    TaskName = dbDataReader.GetString(1),
                                    DueDate = dbDataReader.GetDateTime(2),
                                    TaskNotes = dbDataReader.GetString(3),
                                    IsComplete = dbDataReader.GetBoolean(4)
                                });
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            return taskItems;
        }

    }
}
