using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MediTech.Converter
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string date = "";
            if (value != null)
            {
                date = ((DateTime)value).ToString("dd/MM/yyyy");
            }

            return date;      
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime? dateTime = null;
            if (value != null)
            {
                DateTime date;
                if (DateTime.TryParseExact(value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None
                     , out date))
                {
                    dateTime = date;
                }
            }



            return dateTime;
        }
    }
}
