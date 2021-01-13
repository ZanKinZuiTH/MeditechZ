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
    public partial class EnterOccuVisionTestResult : UserControl
    {
        public EnterOccuVisionTestResult()
        {
            InitializeComponent();
            gcOccVision.PreviewKeyDown += GcOccVision_PreviewKeyDown;
            gvOccVision.CellValueChanged += GvOccVision_CellValueChanged;
        }

        private void GcOccVision_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Tab)
            {
                gvOccVision.MoveNextRow();
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        private void GvOccVision_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            if (this.DataContext is EnterOccuVisionTestResultViewModel)
            {
                var rowData = e.Row as ResultComponentModel;

                if (rowData.ResultItemCode != "TIMUS19" && rowData.ResultItemCode != "TIMUS20" && rowData.ResultItemCode != "TIMUS21"
                    && rowData.ResultItemCode != "TIMUS22" && rowData.ResultItemCode != "TIMUS23" && rowData.ResultItemCode != "TIMUS24")
                {
                    (this.DataContext as EnterOccuVisionTestResultViewModel).CalculateOccuVisionResult();
                }
            }
        }

        private void ListBoxEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (this.DataContext is EnterOccuVisionTestResultViewModel)
            {
                var rowData = (gvOccVision.DataControl.CurrentItem as ResultComponentModel);
                if (rowData != null && rowData.ResultItemName.ToLower().Trim() == "color")
                {
                    if (rowData.CheckDataList != null)
                    {
                        object value = rowData.CheckDataList.Last<object>();
                        if (value.ToString() == "มองไม่เห็น")
                        {
                            rowData.CheckDataList = new ObservableCollection<object>();
                            rowData.CheckDataList.Add("มองไม่เห็น");
                        }
                        else if (value.ToString() == "ไม่ได้ตรวจ")
                        {
                            rowData.CheckDataList = new ObservableCollection<object>();
                            rowData.CheckDataList.Add("ไม่ได้ตรวจ");
                        }
                        else if (value.ToString() == "X")
                        {
                            rowData.CheckDataList = new ObservableCollection<object>();
                            rowData.CheckDataList.Add("12");
                            rowData.CheckDataList.Add("5");
                            rowData.CheckDataList.Add("26");
                            rowData.CheckDataList.Add("6");
                            rowData.CheckDataList.Add("16");
                            rowData.CheckDataList.Add("X");
                        }
                        else if (value.ToString() == "16")
                        {
                            rowData.CheckDataList = new ObservableCollection<object>();
                            rowData.CheckDataList.Add("12");
                            rowData.CheckDataList.Add("5");
                            rowData.CheckDataList.Add("26");
                            rowData.CheckDataList.Add("6");
                            rowData.CheckDataList.Add("16");
                        }
                        else if (value.ToString() == "6")
                        {
                            rowData.CheckDataList = new ObservableCollection<object>();
                            rowData.CheckDataList.Add("12");
                            rowData.CheckDataList.Add("5");
                            rowData.CheckDataList.Add("26");
                            rowData.CheckDataList.Add("6");
                        }
                        else if (value.ToString() == "26")
                        {
                            rowData.CheckDataList = new ObservableCollection<object>();
                            rowData.CheckDataList.Add("12");
                            rowData.CheckDataList.Add("5");
                            rowData.CheckDataList.Add("26");
                        }
                        else if (value.ToString() == "5")
                        {
                            rowData.CheckDataList = new ObservableCollection<object>();
                            rowData.CheckDataList.Add("12");
                            rowData.CheckDataList.Add("5");
                        }

                        gcOccVision.RefreshData();
                    }

                }
                (this.DataContext as EnterOccuVisionTestResultViewModel).CalculateOccuVisionResult();
            }
        }
    }
}
