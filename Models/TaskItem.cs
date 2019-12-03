using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Task_Manager.Models
{
    public class Tasks
    {
        public int ID { get; set; } = -1;       //all new records will have ID-1 until saved
        public string TaskName { get; set; }
        public DateTime DueDate { get; set; }      
        public string TaskNotes { get; set; }
        public bool IsComplete { get; set; } = false;   //  all new items will be set as not complete
    }

}
