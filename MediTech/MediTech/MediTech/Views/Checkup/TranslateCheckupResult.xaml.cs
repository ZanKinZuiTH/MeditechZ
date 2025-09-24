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
    /// Interaction logic for CheckupRule.xaml
    /// </summary>
    public partial class TranslateCheckupResult : UserControl
    {
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        private UpdateProgressBarDelegate _updatePbDelegate;

        public TranslateCheckupResult()
        {
            InitializeComponent();
            _updatePbDelegate = new UpdateProgressBarDelegate(progressBar1.SetValue);
            //gvResultList.CustomCellAppearance += GvResultList_CustomCellAppearance;
            if (this.DataContext is TranslateCheckupResultViewModel)
            {
                (this.DataContext as TranslateCheckupResultViewModel).UpdateEvent += TranslateCheckupResult_UpdateEvent; ; ;
            }
        }

        //private void GvResultList_CustomCellAppearance(object sender, DevExpress.Xpf.Grid.CustomCellAppearanceEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Property == TextBlock.ForegroundProperty)
        //        {
        //            var cellValue = gcResultList.GetCellValue(e.RowHandle, e.Column.FieldName);
        //            if (cellValue != null && cellValue.ToString() != "")
        //            {
        //                string[] values = cellValue.ToString().Split(' ');
        //                if (values != null && values.Count() > 1)
        //                {
        //                    string IsAbnormal = values?[1];
        //                    if (IsAbnormal == "H")
        //                    {
        //                        e.Result = new SolidColorBrush(Colors.Red);
        //                        e.Handled = true;
        //                    }
        //                    else if (IsAbnormal == "L")
        //                    {
        //                        e.Result = new SolidColorBrush(Colors.Blue);
        //                        e.Handled = true;
        //                    }
        //                }
        //            }

        //        }

        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        private void TranslateCheckupResult_UpdateEvent(object sender, EventArgs e)
        {
            gcGroupResult.RefreshData();
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
