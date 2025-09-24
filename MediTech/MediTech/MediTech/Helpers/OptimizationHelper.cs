using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.Helpers
{
    public class OptimizationHelper
    {
        public static bool GetDisableOptimizedMode(DependencyObject obj)
        {
            return (bool)obj.GetValue(DisableOptimizedModeProperty);
        }
        public static void SetDisableOptimizedMode(DependencyObject obj, bool value)
        {
            obj.SetValue(DisableOptimizedModeProperty, value);
        }
        public static readonly DependencyProperty DisableOptimizedModeProperty =
            DependencyProperty.RegisterAttached("DisableOptimizedMode", typeof(bool), typeof(OptimizationHelper), new PropertyMetadata(DataViewBase.DisableOptimizedModeVerification, new PropertyChangedCallback((d, e) => {
                DataViewBase.DisableOptimizedModeVerification = (bool)e.NewValue;
            })));
    }
}
