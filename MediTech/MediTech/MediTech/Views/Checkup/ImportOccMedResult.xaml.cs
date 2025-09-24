using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for ImportOccmedResult.xaml
    /// </summary>
    public partial class ImportOccMedResult : UserControl
    {
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private UpdateProgressBarDelegate _updatePbDelegate;
        public ImportOccMedResult()
        {
            InitializeComponent();
            _updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
        }

        public void SetProgressBarValue(double value)
        {
            Dispatcher.Invoke(_updatePbDelegate,
                    System.Windows.Threading.DispatcherPriority.Background,
                    new object[] { ProgressBar.ValueProperty, value });

        }

        #region SetProgressBarValues()
        public void SetProgressBarLimits(int minValue, int maxValue)
        {
            progressBar1.Minimum = minValue;
            progressBar1.Maximum = maxValue;
        }

        #endregion
    }
}
