using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Task_Manager.Models;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskModel DbContext = new TaskModel();
        private BindingList<TaskItem> taskItems = new BindingList<TaskItem>();

        public MainWindow()
        {

            //Display Date and Time
            InitializeComponent();
            DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(new TimeSpan(0, 0, 1), priority: DispatcherPriority.Normal, 
                delegate{
                Time.Text = DateTime.Now.ToString("h:mm:ss tt"); //Showing Seconds to demonstrate that the Time TextBlock is updating in realtime
                Date.Text = DateTime.Now.ToString("dddd, \n MMMM dd, yyyy");
            }, Dispatcher);




            //Test Data for Task ListBox
            taskItems.Add(new TaskItem() 
            {
                    TaskName = "Build Task Manager",
                    DueDate = new DateTime(2019,12,04),
                    IsComplete = false,
                    TaskNotes = 
                    " - See Current Data in the App \n" +
                    " - Create a new Task \n" +
                    " - Name and Rename Taskt \n" +
                    " - Assign a due date \n" +
                    " - Mark as Completed \n" +
                    " - Delete Tasks (that are no longer important)\n" +
                    " - Highlight completed tasks in green, and over due in red"
            });
            taskItems.Add(new TaskItem()
            {
                TaskName = "Overdue Task",
                DueDate = new DateTime(2019, 11, 29),
                IsComplete = false,
            });
            taskItems.Add(new TaskItem()
            {
                TaskName = "Completed Task",
                DueDate = new DateTime(2019, 12, 05),
                IsComplete = true,
            });
            taskItems.Add(new TaskItem()
            {
                TaskName = "No Rush Task",
                DueDate = new DateTime(2019, 12, 15),
                IsComplete = false,
            });

            TaskListBox.ItemsSource = taskItems;        //Display task data in ListBox
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
            var newTask = new TaskItem { TaskName = TxtTaskName.Text , DueDate = NewDueDate.SelectedDate.Value};
            taskItems.Add(newTask);

            //DbContext.Tasks.Add(newTask);
            //DbContext.SaveChanges();
             
            NewTaskOverLay.Visibility = Visibility.Collapsed;
        }
    }

}
