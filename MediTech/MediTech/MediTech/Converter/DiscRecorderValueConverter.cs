using System;
using System.Windows.Data;
using IMAPI2.Interop;

namespace iDesign.Media.Burner.Converters
{
    [ValueConversion(typeof(IDiscRecorder2), typeof(String))]
    public class DiscRecorderValueConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var discRecorder2 = value as IDiscRecorder2;

            var devicePaths = string.Empty;
            if (discRecorder2 != null)
            {
                var volumePath = (string)discRecorder2.VolumePathNames.GetValue(0);

                foreach (string volPath in discRecorder2.VolumePathNames)
                {
                    if (!string.IsNullOrEmpty(devicePaths))
                    {
                        devicePaths += ",";
                    }
                    devicePaths += volumePath;
                }
            }

            if (discRecorder2 != null) return string.Format("{0} [{1}]", devicePaths, discRecorder2.ProductId);
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}