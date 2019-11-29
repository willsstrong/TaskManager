namespace Task_Manager.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class TaskModel : DbContext
    {
        // Your context has been configured to use a 'Model1' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Task_Manager.Models.Model1' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Model1' 
        // connection string in the application configuration file.
        public TaskModel()
            : base("name=TaskModel")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public DbSet<Tasks> Tasks { get; set; }
    }

    public class Tasks
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TaskName { get; set; }
        public string TaskNotes { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsComplete { get; set; }
    }
}