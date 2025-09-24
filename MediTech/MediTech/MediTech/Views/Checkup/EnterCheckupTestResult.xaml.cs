using MediTech.Model;
using MediTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EnterOccuVisionTest.xaml
    /// </summary>
    public partial class EnterCheckupTestResult : UserControl
    {
        public EnterCheckupTestResult()
        {
            InitializeComponent();
            gcResult.PreviewKeyDown += GcResult_PreviewKeyDown;
            gcResult.Focus();
            gvResult.DataControl.CurrentColumn = colResultValue;
        }

        private void GcResult_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                var dataSource = (gcResult.ItemsSource as ObservableCollection<ResultComponentModel>);
                if (dataSource != null)
                {
                    if (gvResult.FocusedRowHandle != dataSource.Count -1)
                    {
                        gvResult.MoveNextRow();
                        e.Handled = true;
                    }
                    else
                    {
                        btnSave.Focus();
                    }
                }
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
