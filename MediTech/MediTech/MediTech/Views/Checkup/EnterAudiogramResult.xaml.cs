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
    /// Interaction logic for EnterOccmedResult.xaml
    /// </summary>
    public partial class EnterAudiogramResult : UserControl
    {
        public EnterAudiogramResult()
        {
            InitializeComponent();
            grdRightEar.PreviewKeyDown += GrdRightEar_PreviewKeyDown;
            grdLeftEar.PreviewKeyDown += GrdLeftEar_PreviewKeyDown;
            gvRightEar.CellValueChanged += GvRightEar_CellValueChanged;
            gvLeftEar.CellValueChanged += GvLeftEar_CellValueChanged;
            grdRightEar.Focus();
            gvRightEar.DataControl.CurrentColumn = colRightValue;
        }

        private void GvRightEar_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var rowData = e.Row as ResultComponentModel;
            if (rowData != null)
            {
                if (this.DataContext is EnterAudiogramResultViewModel)
                {
                    if (rowData.ResultItemCode != "AUDIO8" && rowData.ResultItemCode != "AUDIO17")
                    {
                        (this.DataContext as EnterAudiogramResultViewModel).CalculateRightResult();
                    }

                }
            }
        }

        private void GvLeftEar_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var rowData = e.Row as ResultComponentModel;
            if (rowData != null)
            {
                if (this.DataContext is EnterAudiogramResultViewModel)
                {
                    if (rowData.ResultItemCode != "AUDIO16" && rowData.ResultItemCode != "AUDIO18")
                    {
                        (this.DataContext as EnterAudiogramResultViewModel).CalculateLeftResult();
                    }

                }
            }
        }


        private void GrdRightEar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                var dataSource = (grdRightEar.ItemsSource as ObservableCollection<ResultComponentModel>);
                if (dataSource != null)
                {
                    if(gvRightEar.FocusedRowHandle != dataSource.Count() - 3)
                    {
                        gvRightEar.MoveNextRow();
                        e.Handled = true;
                    }
                    else
                    {
                        gvLeftEar.Focus();
                        gvLeftEar.FocusedRowHandle = 0;
                        gvLeftEar.DataControl.CurrentColumn = colLeftValue;
                    }
                }
            }
            base.OnPreviewKeyDown(e);
        }

        private void GrdLeftEar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                var dataSource = (grdLeftEar.ItemsSource as ObservableCollection<ResultComponentModel>);
                if (dataSource != null)
                {
                    if (gvLeftEar.FocusedRowHandle != dataSource.Count() - 3)
                    {
                        gvLeftEar.MoveNextRow();
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
