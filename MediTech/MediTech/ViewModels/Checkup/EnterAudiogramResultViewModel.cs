using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class EnterAudiogramResultViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _RequestItemName;

        public string RequestItemName
        {
            get { return _RequestItemName; }
            set { Set(ref _RequestItemName, value); }
        }

        private ObservableCollection<ResultComponentModel> _LeftEarResultComponentItems;

        public ObservableCollection<ResultComponentModel> LeftEarResultComponentItems
        {
            get { return _LeftEarResultComponentItems; }
            set { Set(ref _LeftEarResultComponentItems, value); }
        }

        private ObservableCollection<ResultComponentModel> _RightEarResultComponentItems;

        public ObservableCollection<ResultComponentModel> RightEarResultComponentItems
        {
            get { return _RightEarResultComponentItems; }
            set { Set(ref _RightEarResultComponentItems, value); }
        }

        public string OrderStatus { get; set; }

        private RequestListModel RequestModel;
        #endregion

        #region Command
        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
            }
        }


        private RelayCommand _CloseCommand;

        public RelayCommand CloseCommand
        {
            get
            {
                return _CloseCommand
                    ?? (_CloseCommand = new RelayCommand(Close));
            }
        }
        #endregion

        #region Method

        public override void OnLoaded()
        {
            base.OnLoaded();
            (this.View as EnterAudiogramResult).patientBanner.SetPatientBanner(RequestModel);
        }

        public void AssignModel(RequestListModel request)
        {
            this.RequestModel = request;
            RequestItemName = this.RequestModel.RequestItemName;
            var dataList = DataService.Checkup.GetResultItemByRequestDetailUID(request.RequestDetailUID);
            if (dataList != null)
            {
                LeftEarResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList.Where(p => p.ResultItemName.EndsWith("L")));
                RightEarResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList.Where(p => p.ResultItemName.EndsWith("R")));
            }
        }

        private void Save()
        {
            try
            {
                RequestDetailItemModel reviewRequestDetail = new RequestDetailItemModel();
                reviewRequestDetail.RequestUID = RequestModel.RequestUID;
                reviewRequestDetail.RequestDetailUID = RequestModel.RequestDetailUID;
                reviewRequestDetail.PatientUID = RequestModel.PatientUID;
                reviewRequestDetail.PatientVisitUID = RequestModel.PatientVisitUID;
                reviewRequestDetail.RequestItemCode = RequestModel.RequestItemCode;
                reviewRequestDetail.RequestItemName = RequestModel.RequestItemName;

                reviewRequestDetail.ResultComponents = new ObservableCollection<ResultComponentModel>();
                foreach (var item in LeftEarResultComponentItems)
                {
                    if (!string.IsNullOrEmpty(item.ResultValue))
                        reviewRequestDetail.ResultComponents.Add(item);
                }

                foreach (var item in RightEarResultComponentItems)
                {
                    if (!string.IsNullOrEmpty(item.ResultValue))
                        reviewRequestDetail.ResultComponents.Add(item);
                }

                if (reviewRequestDetail.ResultComponents == null || reviewRequestDetail.ResultComponents.Count <=0)
                {
                    WarningDialog("กรุณากรอกข้อมูล");
                    return;
                }

                DataService.Checkup.SaveOccmedExamination(reviewRequestDetail, AppUtil.Current.UserID);
                OrderStatus = "Reviewed";
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Close()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
