using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class CloseOrderPopUpViewModel : MediTechViewModelBase
    {

        #region Properties
        private List<PatientOrderDetailModel> _ListOrderCloseLists;

        public List<PatientOrderDetailModel> ListOrderCloseLists
        {
            get { return _ListOrderCloseLists; }
            set { _ListOrderCloseLists = value; }
        }


        private DateTime? _CloseDate;

        public DateTime? CloseDate
        {
            get { return _CloseDate; }
            set { Set(ref _CloseDate , value); }
        }

        private DateTime? _CloseTime;

        public DateTime? CloseTime
        {
            get { return _CloseTime; }
            set { Set(ref _CloseTime, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }


        #endregion

        #region Command


        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save));
            }
        }


        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        #endregion

        #region Method
        public CloseOrderPopUpViewModel()
        {
            CloseDate = DateTime.Now;
            CloseTime = CloseDate;
        }

        void Save()
        {
            try
            {
                string patientOrderDetails = "";
                foreach (var orderDetailItem in ListOrderCloseLists)
                {
                    if (patientOrderDetails == "")
                    {
                        patientOrderDetails = orderDetailItem.PatientOrderDetailUID.ToString();
                    }
                    else
                    {
                        patientOrderDetails += "," + orderDetailItem.PatientOrderDetailUID.ToString();

                    }
                }
                var endDttm = DateTime.Parse(CloseDate?.ToString("dd/MM/yyyy") + " " + CloseTime?.ToString("HH:mm"));
                var comments = Comments;
                DataService.OrderProcessing.ClosureStandingOrder(patientOrderDetails, AppUtil.Current.UserID, endDttm, comments);
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }

        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
