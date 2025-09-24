using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MediTech.Converter
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class ImageModuleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string image = (string)value;

            Uri uri = new Uri(@"/MediTech;component/Resources/Images/Module/" + image, UriKind.Relative);
            BitmapImage imageSource = new BitmapImage(uri);
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
