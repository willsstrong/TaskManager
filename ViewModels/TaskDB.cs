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

        private string SqlString(OleDbCommand commandText)      // Returns text of 
        {
            string query = commandText.CommandText;

            foreach (OleDbParameter p in commandText.Parameters)
            {
                query = query.Replace(p.ParameterName, p.Value.ToString());
            }

            return query;
        }


        /**         GET TASK DATE       **/
        public BindingList<Tasks> ListTasks()
        {
            ErrorMsg = null;
            BindingList<Tasks> taskItems = new BindingList<Tasks>();
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

                    connection.Close();
                }
                catch (Exception ReadError)
                {

                    ErrorMsg = $"Error reading Database, Exeption: {ReadError.Message}";
                }
            }
            return taskItems;
        }
        /**         SAVE NEW/UPDATED TASK TO DB          **/
        public void SaveTask(List<Tasks> taskList)
        {
            //plan B for updating records:      Delete record matching ID then save Edited record as new one.
            ErrorMsg = null;

            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string sqlInsert = "INSERT INTO Tasks (TaskName, DueDate, TaskNotes, IsComplete) VALUES (@TaskName,@DueDate,@TaskNotes,@IsComplete)";
                    string sqlUpade = "UPDATE Tasks SET TaskName = @TaskName, DueDate = @DueDate, TaskNotes = @TaskNotes, IsComplete = @IsComplete WHERE ID = @ID";

                    foreach(Tasks task in taskList)
                    {
                        using(OleDbCommand command = new OleDbCommand(task.ID == -1? sqlInsert : sqlUpade, connection))
                        //An ID of -1 indicates that this is a new Item and an INSERT command will execute, otherwise UPDATE
                        {
                            if (task.ID != -1) 
                                command.Parameters.AddWithValue("@ID", task.ID);
                            command.Parameters.AddWithValue("@TaskName", task.TaskName);
                            command.Parameters.AddWithValue("@DueDate", task.DueDate);
                            command.Parameters.AddWithValue("@TaskNotes", task.TaskNotes);
                            command.Parameters.AddWithValue("@IsComplete", task.IsComplete);



                            string query = SqlString(command); //for debugging 
                                
                            command.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception SaveError)
                {

                    ErrorMsg = $"Error Saving to database, Exception:{SaveError.Message}";
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        /**         Remove Task From Database       **/
        public void DeleteTask(object Task_SelectedValue)
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
                            command.Parameters.AddWithValue("@ID", Task_SelectedValue);
                            command.ExecuteScalar();
                        }
                    
                }
                catch (Exception DelError)
                {
                    ErrorMsg = $"Error deleting from database, Exception: {DelError.Message}";
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
