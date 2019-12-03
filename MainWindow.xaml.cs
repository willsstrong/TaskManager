using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Threading;
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

            BindingList<TaskItem> testItem = new BindingList<TaskItem>();
            testItem.Add(new TaskItem()
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
           
            string ConString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
            string sqlString = "INSERT INTO Tasks (TaskName, DueDate,TaskNotes) Values ('" + NewTaskName.Text + "','" + NewDueDate.Text + "','" + NewTaskNotes.Text + "')";
            OleDbConnection connection = new OleDbConnection(ConString);
            OleDbCommand command = new OleDbCommand(sqlString, connection);
            DataSet newDataSet = new DataSet();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            TaskListBox.ItemsSource = TaskDB.ListTasks(); //ReLoad table data into ListBox
            NewTaskOverLay.Visibility = Visibility.Collapsed;
        }
    }

}


//Test Data for Task ListBox
//taskItems.Add(new TaskItem()
//{
//    TaskName = "Build Task Manager",
//                    DueDate = new DateTime(2019, 12, 04),
//                    IsComplete = false,
//                    TaskNotes =
//                    " - See Current Data in the App \n" +
//                    " - Create a new Task \n" +
//                    " - Name and Rename Taskt \n" +
//                    " - Assign a due date \n" +
//                    " - Mark as Completed \n" +
//                    " - Delete Tasks (that are no longer important)\n" +
//                    " - Highlight completed tasks in green, and over due in red"
//            });
//            taskItems.Add(new TaskItem()
//{
//    TaskName = "Overdue Task",
//                DueDate = new DateTime(2019, 11, 29),
//                IsComplete = false,
//            });
//            taskItems.Add(new TaskItem()
//{
//    TaskName = "Completed Task",
//                DueDate = new DateTime(2019, 12, 05),
//                IsComplete = true,
//            });
//            taskItems.Add(new TaskItem()
//{
//    TaskName = "No Rush Task",
//                DueDate = new DateTime(2019, 12, 15),
//                IsComplete = false,
//            });
