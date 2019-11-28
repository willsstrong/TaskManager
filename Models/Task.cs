using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager.Models
{
    public class Task
    {
        public int ID { get; set; }
        public string TaskName { get; set; }
        public string TaskNotes { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
    }
}
