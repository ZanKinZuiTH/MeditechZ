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
using MediTech.ViewModels;
using DevExpress.Xpf.Grid;
using System.Collections.ObjectModel;
using MediTech.Model;
using System.Media;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for RadiologyExamList.xaml
    /// </summary>
    public partial class RadiologyExamList : UserControl
    {
        SoundPlayer sound;
        public RadiologyExamList()
        {
            InitializeComponent();
            //grdExamList.MouseMove += grdExamList_MouseMove;
            grvExamList.MouseDoubleClick += GrvExamList_MouseDoubleClick;
            grdExamList.CustomColumnDisplayText += grdExamList_CustomColumnDisplayText;
            if (this.DataContext is RadiologyExamListViewModel)
            {
                (this.DataContext as RadiologyExamListViewModel).UpdateEvent += RadiologyExamList_UpdateEvent;
            }
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
                    RadiologyExamListViewModel viewModel = (this.DataContext as RadiologyExamListViewModel);

                    ReviewRISResult review = new ReviewRISResult();
                    (review.DataContext as ReviewRISResultViewModel).AssignModel(row.PatientUID, row.PatientVisitUID, row.RequestUID, row.RequestDetailUID);
                    ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)viewModel.LaunchViewDialog(review, "RESTREV", false, true);

                    if (reviewViewModel == null)
                    {
                        return;
                    }
                    if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                    {
                        row.OrderStatus = reviewViewModel.ResultOrderStatus;
                        row.DoctorName = reviewViewModel.DoctorName;
                        row.ResultStatus = reviewViewModel.ResultedStatus;
                        row.IsSelected = false;
                       // viewModel.CheckSelectAll();
                        grdExamList.RefreshData();
                        grvExamList.BestFitColumns();
                    }


                }
            }
        }

        void RadiologyExamList_UpdateEvent(object sender, EventArgs e)
        {
            grdExamList.RefreshData();
           // grvExamList.BestFitColumns();
        }



        private void GrvExamList_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.Equals(colSelected))
            {
                RadiologyExamListViewModel viewModel = (DataContext as RadiologyExamListViewModel);
                if (viewModel.RequestExamList != null)
                {
                    var SelectRequestExams = viewModel.RequestExamList.Where(p => p.IsSelected).ToList();
                    int countSelect = SelectRequestExams.Count + ((bool)e.Value == true ? 1 : -1);
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

        private void grdExamList_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
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
            RadiologyExamListViewModel viewModel = (DataContext as RadiologyExamListViewModel);
            if (viewModel.RequestExamList != null)
            {

                var SelectRequestExams = viewModel.RequestExamList.Where(p => p.IsSelected).ToList();
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


        public void ShowNotification(string textLine1,string textLine2,string textLine3,string textLine4)
        {
            CustomToastViewModel viewModel = ViewModelSource.Create(() => new CustomToastViewModel
            {
                TextLine1 = textLine1,
                TextLine2 = textLine2,
                TextLine3 = textLine3,
                TextLine4 = textLine4
            });

            INotification notification = notificationService.CreateCustomNotification(viewModel);
            notification.ShowAsync().ContinueWith(OnNotificationShown, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            System.IO.Stream str = Properties.Resources.soundforTERC;
            sound = new SoundPlayer(str);
            sound.PlayLooping();
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sound != null)
            {
                sound.Stop();
                sound.Dispose();
            }
        }

        void OnNotificationShown(Task<NotificationResult> task)
        {
            try
            {
                switch (task.Result)
                {
                    case NotificationResult.Activated:
                        Console.WriteLine("Activated");
                        break;
                    case NotificationResult.TimedOut:
                        Console.WriteLine("Timed out");
                        //sound.Stop();
                        break;
                    case NotificationResult.UserCanceled:
                        Console.WriteLine("Canceled by user");
                        //sound.Stop();
                        break;
                    case NotificationResult.Dropped:
                        Console.WriteLine("Dropped (the queue is full)");
                        break;
                }
            }
            catch (AggregateException e)
            {
                Console.WriteLine("Error: " + e.InnerException.Message);
            }
        }
    }

    public class CustomToastViewModel
    {
        public virtual string TextLine1 { get; set; }
        public virtual string TextLine2 { get; set; }
        public virtual string TextLine3 { get; set; }
        public virtual string TextLine4 { get; set; }
    }
}
