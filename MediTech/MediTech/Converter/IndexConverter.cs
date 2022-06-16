using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MediTech.Converter
{
    public class IndexConverter : IMultiValueConverter
    {
        public object Convert(
            object[] values, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            var customers = values[1] as IEnumerable<Object>;
            List<Object> objectList = customers.ToList();
            return (objectList.FindIndex(c => c == values[0]) + 1).ToString();
        }

        public object[] ConvertBack(
            object value, Type[] targetTypes,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
