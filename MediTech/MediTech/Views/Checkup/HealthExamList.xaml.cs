using DevExpress.Xpf.Grid;
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
    /// Interaction logic for HealthExamList.xaml
    /// </summary>
    public partial class HealthExamList : UserControl
    {
        public HealthExamList()
        {
            InitializeComponent();
            grvExamList.MouseDoubleClick += GrvExamList_MouseDoubleClick;
            if (this.DataContext is HealthExamListViewModel)
            {
                (this.DataContext as HealthExamListViewModel).UpdateEvent += HealthExamList_UpdateEvent; ;
            }
        }

        private void HealthExamList_UpdateEvent(object sender, EventArgs e)
        {
            grdExamList.RefreshData();
        }

        private void GrvExamList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TableView view = sender as TableView;
            TableViewHitInfo hitInfo = view.CalcHitInfo(e.OriginalSource as DependencyObject);
            if (hitInfo.InRow)
            {
                var item = view.GetRowElementByRowHandle(hitInfo.RowHandle);
                RowData rowData = (RowData)item.DataContext;

                RequestListModel row = rowData.Row as RequestListModel;

                if (null != row)
                {
                    HealthExamListViewModel viewModel = (this.DataContext as HealthExamListViewModel);

                    MediTechViewModelBase reviewViewModel = null;
                    switch (row.PrintGroup)
                    {
                        case "Physical examination":
                            EnterPhysicalExam review = new EnterPhysicalExam();
                            (review.DataContext as EnterPhysicalExamViewModel).AssignModel(row);
                            reviewViewModel = (EnterPhysicalExamViewModel)viewModel.LaunchViewDialogNonPermiss(review, false, true);
                            break;
                        default:
                            EnterOccmedResult review2 = new EnterOccmedResult();
                            (review2.DataContext as EnterOccmedResultViewModel).AssignModel(row);
                            reviewViewModel = (EnterOccmedResultViewModel)viewModel.LaunchViewDialogNonPermiss(review2, false, true);
                            break;
                    }
                    if (reviewViewModel == null)
                    {
                        return;
                    }
                    if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                    {
                        //row.OrderStatus = reviewViewModel.ResultOrderStatus;
                        //row.DoctorName = reviewViewModel.DoctorName;
                        //row.ResultStatus = reviewViewModel.ResultedStatus;
                        row.IsSelected = false;
                        // viewModel.CheckSelectAll();
                        grdExamList.RefreshData();
                        grvExamList.BestFitColumns();
                    }


                }
            }
        }

        private void GridControl_CustomColumnDisplayText(object sender, DevExpress.Xpf.Grid.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "RowHandle")
            {
                e.DisplayText = (e.RowHandle + 1).ToString();
                if (e.Row != null)
                {
                    (e.Row as RequestListModel).RowHandle = (e.RowHandle + 1);
                }
            }
        }

        private void CheckEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            HealthExamListViewModel viewModel = (DataContext as HealthExamListViewModel);
            if (viewModel.CheckupExamList != null)
            {

                var SelectRequestExams = viewModel.CheckupExamList.Where(p => p.IsSelected).ToList();
                int countSelect = SelectRequestExams.Count;
                if (SelectRequestExams != null && countSelect > 1)
                {
                    viewModel.VisibilityCount = System.Windows.Visibility.Visible;
                    viewModel.CountSelect = "Count : " + countSelect;
                }
                else
                {
                    viewModel.VisibilityCount = System.Windows.Visibility.Hidden;
                    viewModel.CountSelect = "";
                }

            }
        }
    }
}
