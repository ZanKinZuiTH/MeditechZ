using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace MediTech.Converter
{
     [ValueConversion(typeof(byte), typeof(BitmapImage))]
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                byte[] image = (byte[])value;

                BitmapImage bitMapImage = new BitmapImage();
                MemoryStream mem = new MemoryStream(image);
                bitMapImage.BeginInit();
                bitMapImage.StreamSource = mem;
                bitMapImage.EndInit();
                return image;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
