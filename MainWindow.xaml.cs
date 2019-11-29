using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Task_Manager.Models;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskModel DbContext = new TaskModel();
        private List<TaskItem> taskItems = new List<TaskItem>();

        public MainWindow()
        {

            //Display Date and Time
            InitializeComponent();
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer(new TimeSpan(0, 0, 1), System.Windows.Threading.DispatcherPriority.Normal, 
                delegate{
                this.Time.Text = DateTime.Now.ToString("h:mm tt");
                this.Date.Text = DateTime.Now.ToString("dddd, \n MMMM dd, yyyy");
            }, 
            this.Dispatcher);

            //var tasks = from s in DbContext.Tasks select s;
            //var TaskList = tasks.ToList();

            //Test Data for Task ListBox
            taskItems.Add(new TaskItem() 
            {
                    TaskName = "Build Task Manager",
                    DueDate = new DateTime(2019,12,02),
                    IsComplete = true,
                    TaskNotes = 
                    " - See Current Data in the App \n" +
                    " - Create a new Task \n" +
                    " - Name and Rename Taskt \n" +
                    " - Assign a due date \n" +
                    " - Mark as Completed \n" +
                    " - Delete Tasks (that are no longer important)\n" +
                    " - Highlight completed tasks in green, and over due in red"
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
