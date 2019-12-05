using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Task_Manager.Models;
using Task_Manager.ViewModels;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;//uses connection string from App.config
        private static  TaskDB TaskDB = new TaskDB(ConnectionString);   //TaskBD.cs contains driver code for interacting with the database
        private BindingList<Tasks> taskList = TaskDB.ListTasks();   //Create Binding List and load from database

        public MainWindow()
        {
            InitializeComponent();
            
            //Display Date and Time
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), priority: DispatcherPriority.Normal, 
                delegate{
                Time.Text = DateTime.Now.ToString("h:mm:ss tt"); //Showing Seconds to demonstrate that the Time TextBlock is updating in realtime
                Date.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            }, Dispatcher);

            TaskListBox.ItemsSource = TaskDB.ListTasks(); //Loads TaskList data into ListBox

        }

        void NewTask_Click(object sender, RoutedEventArgs e)
        {
            NewTaskOverLay.Visibility = Visibility.Visible;
        }

        private void CmdCloseAdd_Click(object sender, RoutedEventArgs e)
        {
            NewTaskOverLay.Visibility = Visibility.Collapsed;
        }


        private void CmdSaveNew_Click(object sender, RoutedEventArgs e)
        {
            //Create TaskItem List of value from New Task fields

            List<Tasks> taskItems = new List<Tasks>();
            taskItems.Add(new Models.Tasks()
            {
                TaskName = NewTaskName.Text,
                DueDate = NewDueDate.SelectedDate.Value.Date,
                TaskNotes = NewTaskNotes.Text
            });

            TaskDB.SaveTask(taskItems);

            TaskListBox.ItemsSource = TaskDB.ListTasks(); //ReLoad table data into ListBox
            NewTaskOverLay.Visibility = Visibility.Collapsed;
        }

      

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxButton.OKCancel;
            MessageBox.Show("Are you sure you want to delete this task?");

            TaskDB.DeleteTask(TaskListBox.SelectedValue);
            TaskListBox.ItemsSource = TaskDB.ListTasks(); //ReLoad table data into ListBox
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            //TODO  Enable text control on form to be editable
            //      Load into List
            //      SaveTask(<List>);
            //reload TaskListBox
        }

        private void TaskComplete_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void TaskComplete_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void TaskComplete_Click(object sender, RoutedEventArgs e)
        {
            List<Tasks> SelectedTask = new List<Tasks>();
            SelectedTask.Add(new Tasks
            {
                ID = (int)TaskListBox.SelectedValue,
                TaskName = (TaskListBox.SelectedItem as Tasks).TaskName,
                DueDate = (TaskListBox.SelectedItem as Tasks).DueDate,
                TaskNotes = (TaskListBox.SelectedItem as Tasks).TaskNotes,
                IsComplete = (TaskListBox.SelectedItem as Tasks).IsComplete
            });

            string sqlCom = "UPDATE Tasks SET IsComplete = @IsComplete WHERE ID = @ID";


            using (OleDbConnection connection = new OleDbConnection(ConnectionString))
            {
                connection.Open();
                using (OleDbCommand command = new OleDbCommand(sqlCom, connection))
                {
                    command.Parameters.AddWithValue("@IsComplete", (TaskListBox.SelectedItem as Tasks).IsComplete);
                    command.Parameters.AddWithValue("@ID", TaskListBox.SelectedValue);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
            TaskListBox.ItemsSource = TaskDB.ListTasks(); //ReLoad table data into ListBox
            //TaskDB.SaveTask(SelectedTask);
        }
    }
}
