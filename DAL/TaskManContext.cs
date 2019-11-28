namespace Task_Manager.DAL
{
    using Task_Manager.Models;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class TaskManContext : DbContext
    {
        public TaskManContext() 
            : base("TaskManContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Task> Task { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }

}