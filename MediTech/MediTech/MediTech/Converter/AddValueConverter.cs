using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MediTech.Converter
{
    public class AddValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string number = "";
            if (value != null)
            {
                int num = (int)value;
                int para  = 1;
                if (parameter != null)
                {
                    para = int.Parse(parameter.ToString());

                }
               
                if (num >= 0)
                {
                    number = (num + para).ToString();
                }
            }
            return number;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
