using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using MediTech.Helpers;

namespace MediTech.Converter
{
    [ValueConversion(typeof(Bitmap), typeof(BitmapImage))]
    public class BitmapToBitmapImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {

                BitmapImage bitMapImage = ((Bitmap)value).ToBitmapImage();
                return bitMapImage;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
