using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ListPaymentDetailsViewModel : MediTechViewModelBase
    {
        #region Properties

        public long PatientBillUID { get; set; }

        private List<PatientPaymentDetailModel> _PaymentDetailLists;

        public List<PatientPaymentDetailModel> PaymentDetailLists
        {
            get { return _PaymentDetailLists; }
            set { Set(ref _PaymentDetailLists, value); }
        }


        #endregion

        #region Command

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        #endregion

        #region Method

        public override void OnLoaded()
        {
            base.OnLoaded();
            PaymentDetailLists = DataService.Billing.GetPatientPaymentDetailByBillUID(PatientBillUID);
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion

    }
}
