
using MediTech.ViewModels;
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
    /// Interaction logic for EcountImportFile.xaml
    /// </summary>
    public partial class EcountImportFile : UserControl
    {

        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private UpdateProgressBarDelegate _updatePbDelegate;


        public EcountImportFile()
        {
            InitializeComponent();

            _updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
            if (this.DataContext is TranslateXrayViewModel)
            {
                (this.DataContext as TranslateXrayViewModel).UpdateEvent += GridControl_UpdateEvent;
            }
        }

        void GridControl_UpdateEvent(object sender, EventArgs e)
        {
           
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
