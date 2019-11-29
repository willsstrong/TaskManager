using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

        public MainWindow()
        {

            //Display Date and Time
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, 
                delegate{
                this.Time.Text = DateTime.Now.ToString("h:mm tt");
                this.Date.Text = DateTime.Now.ToString("dddd, \n MMMM dd, yyyy");
            }, 
            this.Dispatcher);

            var tasks = from s in DbContext.Tasks select s;
            var TaskList = tasks.ToList();




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
            var newTask = new Tasks { TaskName = TxtTaskName.Text , DueDate = NewDueDate.SelectedDate.Value};

            DbContext.Tasks.Add(newTask);
            DbContext.SaveChanges();
                
           NewTaskOverLay.Visibility = Visibility.Collapsed;
        }
    }

}
