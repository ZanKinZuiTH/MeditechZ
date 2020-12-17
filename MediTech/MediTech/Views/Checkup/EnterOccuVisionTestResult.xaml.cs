using MediTech.Model;
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
    /// Interaction logic for EnterOccuVisionTest.xaml
    /// </summary>
    public partial class EnterOccuVisionTestResult : UserControl
    {
        public EnterOccuVisionTestResult()
        {
            InitializeComponent();
            gvOccVision.CellValueChanged += GvOccVision_CellValueChanged;
        }

        private void GvOccVision_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var rowData = e.Row as ResultComponentModel;
            if (rowData != null)
            {
                if (this.DataContext is EnterOccuVisionTestResultViewModel)
                {
                    (this.DataContext as EnterOccuVisionTestResultViewModel).CalculateOccuVisionResult();
                }
            }
        }
    }
}
