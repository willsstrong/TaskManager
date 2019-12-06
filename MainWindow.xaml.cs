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
            taskItems.Add(new Tasks()
            {
                TaskName = NewTaskName.Text,
                DueDate = NewDueDate.SelectedDate.Value.Date,
                TaskNotes = NewTaskNotes.Text
            });

            Tasks task = new Tasks()
            {
                TaskName = NewTaskName.Text,
                DueDate = NewDueDate.SelectedDate.Value.Date,
                TaskNotes = NewTaskNotes.Text
            };

            TaskDB.SaveTask(task);
            taskItems.Clear();
            TaskListBox.ItemsSource = TaskDB.ListTasks(); //ReLoad table data into ListBox
            NewTaskOverLay.Visibility = Visibility.Collapsed;
        }

      

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxButton.OKCancel;
            MessageBoxResult option = MessageBox.Show("Are you sure you want to delete this task?","Delete",MessageBoxButton.OKCancel);
            if (!(option == MessageBoxResult.Cancel))
            {
                TaskDB.DeleteTask(TaskListBox.SelectedValue);
                TaskListBox.ItemsSource = TaskDB.ListTasks(); //ReLoad table data into ListBox
            }
        }


        private void TaskEdit_Click(object sender, RoutedEventArgs e)
        {
            //create task object.
            
            Tasks tasks = (TaskListBox.SelectedItem as Tasks);  //Create Tasks object from Selected TaskList Item
            //TaskToUpdate.Add(tasks);
            //poputate form
            UpdateTaskName.Text = tasks.TaskName;
            UpdateDueDate.SelectedDate = tasks.DueDate;
            UpdateTaskNotes.Text = tasks.TaskNotes;
            EditTaskOverLay.Visibility = Visibility.Visible;
        }
        private void TaskComplete_Click(object sender, RoutedEventArgs e)
        {
            Tasks task = (TaskListBox.SelectedItem as Tasks);

            TaskDB.UpdateTask(task);
            TaskListBox.ItemsSource = TaskDB.ListTasks(); //ReLoad table data into ListBox
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

            Tasks task = new Tasks()
            {
                ID = (int)TaskListBox.SelectedValue,
                TaskName = UpdateTaskName.Text,
                DueDate = UpdateDueDate.SelectedDate.Value,
                TaskNotes = UpdateTaskNotes.Text
            };

            //string sqlCom = "UPDATE Tasks SET TaskName = @TaskName, DueDate = @DueDate,TaskNotes = @TaskNotes WHERE ID = @ID";

            TaskDB.UpdateTask(task);
                        
            TaskListBox.ItemsSource = TaskDB.ListTasks(); //ReLoad table data into ListBox
            EditTaskOverLay.Visibility = Visibility.Collapsed;
        }

        private void CloseUpdate_Click(object sender, RoutedEventArgs e)
        {
            EditTaskOverLay.Visibility = Visibility.Collapsed;
        }

    }
}
