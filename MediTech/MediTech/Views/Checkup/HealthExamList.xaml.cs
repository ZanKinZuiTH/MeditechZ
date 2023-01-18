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
                            EnterPhysicalExam reviewPhyexam = new EnterPhysicalExam();
                            (reviewPhyexam.DataContext as EnterPhysicalExamViewModel).AssignModel(row);
                            reviewViewModel = (EnterPhysicalExamViewModel)viewModel.LaunchViewDialogNonPermiss(reviewPhyexam, false, true);
                            break;
                        case "Audiogram":
                            EnterAudiogramResult reviewAudioGram = new EnterAudiogramResult();
                            (reviewAudioGram.DataContext as EnterAudiogramResultViewModel).AssignModel(row);
                            reviewViewModel = (EnterAudiogramResultViewModel)viewModel.LaunchViewDialogNonPermiss(reviewAudioGram, false, true);
                            break;
                        case "Elektrokardiogram":
                            EnterEKGResult reviewEKG = new EnterEKGResult();
                            (reviewEKG.DataContext as EnterEKGResultViewModel).AssignModel(row);
                            reviewViewModel = (EnterEKGResultViewModel)viewModel.LaunchViewDialogNonPermiss(reviewEKG, false, true);
                            break;
                        case "Occupational Vision Test":
                            EnterOccuVisionTestResult reviewOccu = new EnterOccuVisionTestResult();
                            (reviewOccu.DataContext as EnterOccuVisionTestResultViewModel).AssignModel(row);
                            reviewViewModel = (EnterOccuVisionTestResultViewModel)viewModel.LaunchViewDialogNonPermiss(reviewOccu, false, true);
                            break;
                        case "Pulmonary Function Test":
                            EnterPulmonaryResult reviewPulmonary = new EnterPulmonaryResult();
                            (reviewPulmonary.DataContext as EnterPulmonaryResultViewModel).AssignModel(row);
                            reviewViewModel = (EnterPulmonaryResultViewModel)viewModel.LaunchViewDialogNonPermiss(reviewPulmonary, false, true);
                            break;
                        case "Muscle Test":
                            EnterMuscleTest reviewMuscleTest = new EnterMuscleTest();
                            (reviewMuscleTest.DataContext as EnterMuscleTestViewModel).AssignModel(row);
                            reviewViewModel = (EnterMuscleTestViewModel)viewModel.LaunchViewDialogNonPermiss(reviewMuscleTest, false, true);
                            break;
                        case "Physical Fitness Test":
                            EnterFitnessTestResult reviewFitnessTest = new EnterFitnessTestResult();
                            (reviewFitnessTest.DataContext as EnterFitnessTestResultViewModel).AssignModel(row);
                            reviewViewModel = (EnterFitnessTestResultViewModel)viewModel.LaunchViewDialogNonPermiss(reviewFitnessTest, false, true);
                            break;
                        case "PAP Smear Test":
                            EnterPapSmear reviewPapSmear = new EnterPapSmear();
                            (reviewPapSmear.DataContext as EnterPapSmearViewModel).AssignModel(row);
                            reviewViewModel = (EnterPapSmearViewModel)viewModel.LaunchViewDialogNonPermiss(reviewPapSmear, false, true);
                            break;
                        default:
                            EnterCheckupTestResult reviewCheckup = new EnterCheckupTestResult();
                            (reviewCheckup.DataContext as EnterCheckupTestResultViewModel).AssignModel(row);
                            reviewViewModel = (EnterCheckupTestResultViewModel)viewModel.LaunchViewDialogNonPermiss(reviewCheckup, false, true);
                            break;
                    }
                    if (reviewViewModel == null)
                    {
                        return;
                    }
                    if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                    {
                        System.Reflection.PropertyInfo OrderStatus = reviewViewModel.GetType().GetProperty("OrderStatus");
                        row.OrderStatus = (String)(OrderStatus.GetValue(reviewViewModel));
                        row.IsSelected = false;
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
