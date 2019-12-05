using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;

namespace TaskManagerCommon.Data
{
    public class TaskData
    {
        public int TaskId { get; set; } = -1; // all new records will have a -1 until saved to database
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
        public string TaskNotes { get; set; }
        public bool IsComplete { get; set; }
        public bool IsRemoved { get; set; } // is this flagged for removal
    }

    public class TaskManagerDatabase
    {
        public string ErrorMsg { get; private set; }
        public string DatabaseFile { get; private set; } = "TaskManager.sdf";
        private string Password = "TaskManager";
        private string ConnectionString { get { return $"DataSource=\"{DatabaseFile}\"; Password=\"{Password}\""; } }

        public TaskManagerDatabase()
        {
            CreateDatabase();
        }

        private void CreateDatabase()
        {
            ErrorMsg = null;
            // Does the DB exist
            if (!File.Exists(DatabaseFile))
            {
                using (SqlCeEngine en = new SqlCeEngine(ConnectionString))
                {
                    try
                    {
                        en.CreateDatabase();

                        using (SqlCeConnection con = new SqlCeConnection(ConnectionString))
                        {
                            try
                            {
                                con.Open();
                                string sql = "create table Tasks ("
                                    + "TaskId int identity(1, 1) not null primary key,"
                                    + "TaskName nvarchar(40) not null,"
                                    + "DueDate datetime,"
                                    + "TaskNotes nvarchar(1024),"
                                    + "IsComplete nchar(3))"; // will use "No" and "Yes" as boolean

                                using (SqlCeCommand cmd = new SqlCeCommand(sql, con))
                                    cmd.ExecuteNonQuery();
                            }
                            catch (Exception exCreateTable)
                            {
                                ErrorMsg = $"Error adding table to {DatabaseFile} database, Excpetion: {exCreateTable.Message}";
                            }
                        }
                    }
                    catch (Exception exEngine)
                    {
                        ErrorMsg = $"Error creating local {DatabaseFile} database, Excpetion: {exEngine.Message}";
                    }

                }
            }
        }

        public List<TaskData> ReadTaskData()
        {
            ErrorMsg = null;
            List<TaskData> TaskDataList = new List<TaskData>();
            using (SqlCeConnection con = new SqlCeConnection(ConnectionString))
            {
                try
                {
                    con.Open();
                    string sql = "select TaskId, TaskName, DueDate, TaskNotes, IsComplete from Tasks";

                    using (SqlCeCommand cmd = new SqlCeCommand(sql, con))
                    {
                        int result = cmd.ExecuteNonQuery();
                        using (SqlCeDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TaskData task = new TaskData();
                                task.TaskId = (int)reader["TaskId"];
                                task.TaskName = reader["TaskName"].ToString().Trim();
                                string temp = reader["DueDate"]?.ToString();
                                if (temp != null && temp.Length > 0)
                                    task.DueDate = (DateTime)reader["DueDate"];
                                task.TaskNotes = reader["TaskNotes"]?.ToString().Trim();
                                task.IsComplete = reader["IsComplete"]?.ToString() == "Yes" ? true : false;
                                TaskDataList.Add(task);
                            }
                        }
                    }
                }
                catch (Exception exRead)
                {
                    ErrorMsg = $"Error reading database, Excpetion: {exRead.Message}";
                }
            }
            return TaskDataList;
        }

        public bool SaveTaskData(List<TaskData> TaskDataList)
        {
            ErrorMsg = null;
            bool bResult = false;

            using (SqlCeConnection connection = new SqlCeConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    //TaskId is auto incremented by DB
                    string sqlinsertfields = "insert Tasks (TaskName, DueDate, TaskNotes, IsComplete) values (@TaskName, @DueDate, @TaskNotes, @IsComplete)";
                    string sqlupdatefields = "update Tasks set TaskName = @TaskName, DueDate = @DueDate, TaskNotes = @TaskNotes, IsComplete = @IsComplete where TaskId = @TaskId";
                    string sqldeletefields = "delete from Tasks where TaskId = @TaskId";

                    foreach (TaskData task in TaskDataList)
                    {
                        if (!task.IsRemoved) // insert or update record
                        {
                            using (SqlCeCommand cmd = new SqlCeCommand(task.TaskId == -1 ? sqlinsertfields : sqlupdatefields, connection))
                            {
                                if (task.TaskId != -1) // update record
                                    cmd.Parameters.Add("@TaskId", SqlDbType.Int).Value = task.TaskId;
                                cmd.Parameters.Add("@TaskName", SqlDbType.NVarChar, 40).Value = task.TaskName ?? (object)DBNull.Value; // this will fail if null since the DB is set to not allow null
                                cmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = task.DueDate;
                                cmd.Parameters.Add("@TaskNotes", SqlDbType.NVarChar, 1024).Value = task.TaskNotes ?? (object)DBNull.Value;
                                cmd.Parameters.Add("@IsComplete", SqlDbType.NChar, 3).Value = task.IsComplete == true ? "Yes" : "No";

                                cmd.ExecuteNonQuery();
                            }
                        }
                        else // delete record
                        {
                            using (SqlCeCommand cmd = new SqlCeCommand(sqldeletefields, connection))
                            {
                                if (task.TaskId != -1) // make it is not new which does not exist in DB
                                {
                                    try
                                    {
                                        cmd.Parameters.Add("@TaskId", SqlDbType.Int).Value = task.TaskId;
                                        cmd.ExecuteNonQuery();
                                    }
                                    catch (Exception exDelete)
                                    {
                                        ErrorMsg = $"Error deleting from database, Excpetion: {exDelete.Message}";
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exRead)
                {
                    ErrorMsg = $"Error saving to database, Excpetion: {exRead.Message}";
                }
            }
            return bResult;
        }
    }
}
