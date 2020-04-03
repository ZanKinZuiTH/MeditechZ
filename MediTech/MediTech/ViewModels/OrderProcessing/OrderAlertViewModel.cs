using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class OrderAlertViewModel : MediTechViewModelBase
    {
        #region Properties


        private List<PatientOrderAlertModel> _OrderAlerts;

        public List<PatientOrderAlertModel> OrderAlerts
        {
            get { return _OrderAlerts; }
            set { Set(ref _OrderAlerts, value); }
        }


        private List<LookupReferenceValueModel> _OverrrideReason;

        public List<LookupReferenceValueModel> OverrrideReason
        {
            get { return _OverrrideReason; }
            set { Set(ref _OverrrideReason, value); }
        }

        private LookupReferenceValueModel _SelectOverrideReason;

        public LookupReferenceValueModel SelectOverrideReason
        {
            get { return _SelectOverrideReason; }
            set { Set(ref _SelectOverrideReason, value); }
        }

        private string _OverrideRemark;

        public string OverrideRemark
        {
            get { return _OverrideRemark; }
            set { Set(ref _OverrideRemark, value); }
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

        public OrderAlertViewModel(List<PatientOrderAlertModel> orderAlerts)
        {
            OrderAlerts = orderAlerts;
            OverrrideReason = DataService.Technical.GetReferenceValueMany("ALGNT");
        }

        void Save()
        {
            if (SelectOverrideReason == null)
            {
                WarningDialog("กรุณาระบุเหตุผล");
                return;
            }

            foreach (var item in OrderAlerts)
            {
                item.OverrideRemarks = OverrideRemark;
                item.OverrideRSNUID = SelectOverrideReason.Key;
            }
            CloseViewDialog(ActionDialog.Save);
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
