using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Task_Manager
{
    public class TaskItem
    {
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }
        public string TaskNotes { get; set; }
        public bool IsComplete { get; set; }
    }

}
