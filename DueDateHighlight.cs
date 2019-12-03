using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;



namespace Task_Manager
{

    [ValueConversion(typeof(DateTime),typeof(Brush))]
    public class DueDateHighlight : IValueConverter
    //Return a preset value of Brushes.[colour] depending on relation of current date to DueDate 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dueDate = (DateTime)value;
            var currentDate = DateTime.Now;
            int daysBeforeDue = (dueDate - currentDate).Days;  //abount of time between current date and due-date

            if (daysBeforeDue <= 0) return Brushes.Red;
            else if (daysBeforeDue <= 5) return Brushes.Orange;  
            return Brushes.Silver;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
