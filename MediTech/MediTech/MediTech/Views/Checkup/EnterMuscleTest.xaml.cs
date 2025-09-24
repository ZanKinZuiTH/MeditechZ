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
    public partial class EnterMuscleTest : UserControl
    {
        public EnterMuscleTest()
        {
            InitializeComponent();
            gcResult.PreviewKeyDown += GcResult_PreviewKeyDown;
            gvResult.CellValueChanged += GvResult_CellValueChanged;
            gcResult.Focus();
            gvResult.DataControl.CurrentColumn = colResultValue;
        }

        private void GcResult_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                btnSave.Focus();
            }
            base.OnPreviewKeyDown(e);
        }

        private void GvResult_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var rowData = e.Row as ResultComponentModel;
            if (rowData != null)
            {
                if (rowData.ResultItemCode == "MUCS1" || rowData.ResultItemCode == "MUCS3" || rowData.ResultItemCode == "MUCS5")
                {
                    if (this.DataContext is EnterMuscleTestViewModel)
                    {
                        (this.DataContext as EnterMuscleTestViewModel).CalculateMuscleValue();
                    }

                }
            }
        }
    }
}
