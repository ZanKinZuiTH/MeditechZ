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
    public class EnterPulmonaryResultViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _RequestItemName;

        public string RequestItemName
        {
            get { return _RequestItemName; }
            set { Set(ref _RequestItemName, value); }
        }

        private ObservableCollection<ResultComponentModel> _ResultComponentItems;

        public ObservableCollection<ResultComponentModel> ResultComponentItems
        {
            get { return _ResultComponentItems; }
            set { Set(ref _ResultComponentItems, value); }
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
            (this.View as EnterPulmonaryResult).patientBanner.SetPatientBanner(RequestModel);
        }

        public void AssignModel(RequestListModel request)
        {
            this.RequestModel = request;
            RequestItemName = this.RequestModel.RequestItemName;
            var dataList = DataService.Checkup.GetResultItemByRequestDetailUID(request.RequestDetailUID);
            if (dataList != null)
            {
                ResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList);
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

                reviewRequestDetail.ResultComponents = ResultComponentItems;
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
