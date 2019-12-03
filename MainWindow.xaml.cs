using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.OleDb;
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
        public static string ConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;//uses connection string from App.config
        TaskDB TaskDB = new TaskDB(ConnectionString);   //TaskBD.cs contains driver code for interacting with the database

        public MainWindow()
        {
            InitializeComponent();
            
            //Display Date and Time
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), priority: DispatcherPriority.Normal, 
                delegate{
                Time.Text = DateTime.Now.ToString("h:mm:ss tt"); //Showing Seconds to demonstrate that the Time TextBlock is updating in realtime
                Date.Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy");
            }, Dispatcher);

            BindingList<Models.Tasks> testItem = new BindingList<Models.Tasks>();
            testItem.Add(new Models.Tasks()
            {
                TaskName = "Test task",
                DueDate = DateTime.Now.Date,
                IsComplete = false
            }) ;

            TaskListBox.ItemsSource = TaskDB.ListTasks(); //Loads table data into ListBox

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

            List<Models.Tasks> taskItems = new List<Models.Tasks>();
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
    }
}
