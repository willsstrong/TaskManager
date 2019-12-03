/*  TaskDB.cs
    Contains Methods for interacting with the attached database
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.OleDb;
using Task_Manager.Models;

namespace Task_Manager.ViewModels
{
    class TaskDB
    {

        private string ConnectionString;
        public string ErrorMsg { get; private set; }

        public TaskDB(string connectionString)
        {
            ConnectionString = connectionString;
        }


        /**         GET TASK DATE       **/
        public BindingList<Models.Tasks> ListTasks()
        {
            ErrorMsg = null;
            BindingList<Models.Tasks> taskItems = new BindingList<Models.Tasks>();
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
                                new Models.Tasks
                                {
                                    ID = dbDataReader.GetInt32(0),
                                    TaskName = dbDataReader.GetString(1),
                                    DueDate = dbDataReader.GetDateTime(2),
                                    TaskNotes = dbDataReader.GetString(3),
                                    IsComplete = dbDataReader.GetBoolean(4)
                                });
                            }
                        }
                    }
                }
                catch (Exception ReadError)
                {

                    ErrorMsg = $"Error reading Database, Exeption: {ReadError.Message}";
                }
            }
            return taskItems;
        }
        /**         SAVE NEW/UPDATED TASK TO DB          **/
        public void SaveTask(List<Models.Tasks> taskList)
        {
            ErrorMsg = null;

            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string sqlInsert = "INSERT INTO Tasks (TaskName, DueDate, TaskNotes, IsComplete) VALUES (@TaskName,@DueDate,@TaskNotes,@IsComplete)";
                    string sqlUpade = "UPDATE TasksDB " +
                                      "SET..." +
                                      "WHERE TaskID = @TaskID";
                    //string sqlDelete = "DELETE FROM TasksDB";

                    foreach(Models.Tasks task in taskList)
                    {
                        using(OleDbCommand command = new OleDbCommand(task.ID == -1? sqlInsert : sqlUpade, connection))
                        //An ID of -1 indicates that this is a new Item and an INSERT command will execute, otherwise UPDATE
                        {
                            if (task.ID != -1) command.Parameters.AddWithValue("@ID", task.ID);
                            command.Parameters.AddWithValue("@TaskName", task.TaskName);
                            command.Parameters.AddWithValue("@DueDate", task.DueDate);
                            command.Parameters.AddWithValue("@TaskNotes", task.TaskNotes);
                            command.Parameters.AddWithValue("@IsComplete", task.IsComplete);

                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception SaveError)
                {

                    ErrorMsg = $"Error Saving to database, Exception:{SaveError.Message}";
                }
            }
        }
        /**         Remove Task From Database       **/
        public void DeleteTask(object taskItem)
        {
            ErrorMsg = null;
            using(OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string sqlDelete = "DELETE FROM Tasks where ID = @ID";

                        using (OleDbCommand command = new OleDbCommand(sqlDelete, connection))
                        {
                            command.Parameters.AddWithValue("@ID", taskItem);
                            command.ExecuteScalar();
                        }
                    
                }
                catch (Exception DelError)
                {

                    ErrorMsg = $"Error deleting from database, Exception: {DelError.Message}";
                }
            }
        }
    }
}
