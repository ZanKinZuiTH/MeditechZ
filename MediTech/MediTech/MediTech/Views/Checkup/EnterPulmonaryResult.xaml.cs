using DevExpress.Xpf.Grid;
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
    /// Interaction logic for EnterPulmonaryResult.xaml
    /// </summary>
    public partial class EnterPulmonaryResult : UserControl
    {
        public EnterPulmonaryResult()
        {
            InitializeComponent();
            gcPulmonary.PreviewKeyDown += GcPulmonary_PreviewKeyDown;
            if (this.DataContext is EnterPulmonaryResultViewModel)
            {
                (this.DataContext as EnterPulmonaryResultViewModel).UpdateEvent += EnterPulmonaryResult_UpdateEvent; ;
            }
            gcPulmonary.Focus();
            gvPulmonary.DataControl.CurrentColumn = colResultValue;
        }

        private void GcPulmonary_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                var dataSource = (gcPulmonary.ItemsSource as ObservableCollection<ResultComponentModel>);
                if (dataSource != null)
                {
                    if (gvPulmonary.FocusedRowHandle != 1)
                    {
                        gvPulmonary.MoveNextRow();
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

        private void EnterPulmonaryResult_UpdateEvent(object sender, EventArgs e)
        {
            gcPulmonary.RefreshData();
        }

        private void GvPulmonary_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var rowData = e.Row as ResultComponentModel;
            if (rowData != null)
            {
                if (rowData.ResultItemCode == "SPIRO1" || rowData.ResultItemCode == "SPIRO4"
                    || rowData.ResultItemName == "FVC (Meas.)" || rowData.ResultItemName == "FEV1 (Meas.)")
                {
                    if (this.DataContext is EnterPulmonaryResultViewModel)
                    {
                        (this.DataContext as EnterPulmonaryResultViewModel).CalculateSpiroValue();
                    }

                }
            }
        }
    }


}

