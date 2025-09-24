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
    public class CancelBillPopupViewModel : MediTechViewModelBase
    {
        #region Properties

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }


        private ObservableCollection<PatientBillModel> _PatientBillLists;

        public ObservableCollection<PatientBillModel> PatientBillLists
        {
            get { return _PatientBillLists; }
            set { Set(ref _PatientBillLists, value); }
        }

        private ObservableCollection<PatientBillModel> _SelectedPatientBills;

        public ObservableCollection<PatientBillModel> SelectedPatientBills
        {
            get { return _SelectedPatientBills ?? (_SelectedPatientBills = new ObservableCollection<PatientBillModel>()); }
            set { Set(ref _SelectedPatientBills, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set
            {
                Set(ref _Comments, value);
                if (!string.IsNullOrEmpty(_Comments))
                {
                    foreach (var bill in PatientBillLists)
                    {
                        if (string.IsNullOrEmpty(bill.CancelReason))
                        {
                            bill.CancelReason = _Comments;
                        }
                    }
                    (this.View as CancelBillPopup).grdPatientBillLists.RefreshData();
                }
            }
        }


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


        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        #endregion

        #region Method

        public override void OnLoaded()
        {
            if (SelectPatientVisit != null)
            {
                PatientBillLists = new ObservableCollection<PatientBillModel>(DataService.Billing.GetPatientBill(SelectPatientVisit.PatientUID, SelectPatientVisit.PatientVisitUID));
            }
        }

        public void AssignPatientVisit(PatientVisitModel patientVisit)
        {
            this.SelectPatientVisit = patientVisit;
        }

        void Save()
        {
            try
            {
                if (SelectedPatientBills == null || SelectedPatientBills.Count <= 0)
                {
                    WarningDialog("กรุณาเลือกใบเสร็จที่จะยกเลิก");
                    return;
                }
                if (SelectedPatientBills.Count(p => string.IsNullOrEmpty(p.CancelReason?.Trim())) > 0)
                {
                    WarningDialog("กรุณาระบุเหตุผลในการยกเลิกให้ครบถ้วน");
                    return;
                }
                DataService.Billing.CancelBillLists(SelectedPatientBills.ToList());
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
