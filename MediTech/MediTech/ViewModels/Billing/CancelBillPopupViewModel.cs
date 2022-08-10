using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
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


        private List<PatientBillModel> _PatientBillLists;

        public List<PatientBillModel> PatientBillLists
        {
            get { return _PatientBillLists; }
            set { Set(ref _PatientBillLists, value); }
        }

        private List<PatientBillModel> _SelectedPatientBills;

        public List<PatientBillModel> SelectedPatientBills
        {
            get { return _SelectedPatientBills ?? (_SelectedPatientBills = new List<PatientBillModel>()); }
            set { Set(ref _SelectedPatientBills, value); }
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


        void Save()
        {

        }

        void Cancel()
        {

        }

        #endregion
    }
}
