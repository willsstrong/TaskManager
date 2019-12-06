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
        public void SaveTask(Tasks task)
        {
            ErrorMsg = null;

            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string sqlInsert = "INSERT INTO Tasks (TaskName, DueDate, TaskNotes, IsComplete) VALUES (@TaskName,@DueDate,@TaskNotes,@IsComplete)";
                    using (OleDbCommand command = new OleDbCommand(sqlInsert, connection))
                    {
                        command.Parameters.AddWithValue("@TaskName", task.TaskName);
                        command.Parameters.AddWithValue("@DueDate", task.DueDate);
                        command.Parameters.AddWithValue("@TaskNotes", task.TaskNotes);
                        command.Parameters.AddWithValue("@IsComplete", task.IsComplete);
                        string query = SqlString(command); //for debugging 

                        command.ExecuteNonQuery();
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

        public void UpdateTask(Tasks task)
        {
            ErrorMsg = null;

            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {                    
                string sqlUpade = "UPDATE Tasks SET TaskName = @TaskName, DueDate = @DueDate, TaskNotes = @TaskNotes, IsComplete = @IsComplete WHERE ID = @ID";

                try
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(sqlUpade, connection))
                    {
                        command.Parameters.AddWithValue("@TaskName", task.TaskName);        //UpdateTaskName.Text);
                        command.Parameters.AddWithValue("@DueDate", task.DueDate);          //UpdateDueDate.SelectedDate.Value);
                        command.Parameters.AddWithValue("@TaskNotes", task.TaskNotes);      //UpdateTaskNotes.Text);
                        command.Parameters.AddWithValue("@IsComplete", task.IsComplete);      //UpdateTaskNotes.Text);
                        command.Parameters.AddWithValue("@ID", task.ID);                    //TaskListBox.SelectedValue);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();
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
            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
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
