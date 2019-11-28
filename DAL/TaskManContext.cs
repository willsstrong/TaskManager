namespace Task_Manager.DAL
{
    using Task_Manager.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TaskManContext : DbContext
    {
        // Your context has been configured to use a 'TaskManContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'Task_Manager.DAL.TaskManContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'TaskManContext' 
        // connection string in the application configuration file.
        public TaskManContext()
            : base("name=TaskManContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Task> MyEntities { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }

}