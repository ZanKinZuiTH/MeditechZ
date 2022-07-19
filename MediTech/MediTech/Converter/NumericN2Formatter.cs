using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MediTech.Converter
{
    public class NumericN2Formatter : IValueConverter
    {
        #region IValueConverter Members
        //
        #region Convert()

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value != null)
            {
                double intValue;

                double.TryParse(value.ToString(), out intValue);

                if (intValue == 0)
                    return string.Empty;
                else
                    return string.Format("{0:N2}", value);

            }
            return value;
        }
        #endregion
        //
        #region ConvertBack()
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
        #endregion
        //
        #endregion
    }
}
