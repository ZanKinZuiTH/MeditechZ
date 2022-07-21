using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class EditExpDischargeDateViewModel : MediTechViewModelBase
    {
        #region Properties
        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        private bool _IsVisitor;
        public bool IsVisitor
        {
            get { return _IsVisitor; }
            set { Set(ref _IsVisitor, value); }
        }

        private List<LookupReferenceValueModel> _Classification;
        public List<LookupReferenceValueModel> Classification
        {
            get { return _Classification; }
            set { Set(ref _Classification, value); }
        }

        private LookupReferenceValueModel _SelectClassification;
        public LookupReferenceValueModel SelectClassification
        {
            get { return _SelectClassification; }
            set { Set(ref _SelectClassification, value); }
        }

        private DateTime _ExpectedDate;
        public DateTime ExpectedDate
        {
            get { return _ExpectedDate; }
            set { Set(ref _ExpectedDate, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        AdmissionEventModel admissionModel;
        #endregion

        #region Method
        public EditExpDischargeDateViewModel()
        {
            ExpectedDate = DateTime.Now;
            Classification = DataService.Technical.GetReferenceValueMany("CRCLS");
        }

        public void AssignModel(PatientVisitModel patientVisit,AdmissionEventModel model)
        {
            SelectPatientVisit = patientVisit;
            admissionModel = model;
        }

        private void Save()
        {
            admissionModel.ExpectedDischargeDttm = ExpectedDate;
            DataService.PatientIdentity.EditExpDischarged(admissionModel, AppUtil.Current.UserID);
            CloseViewDialog(ActionDialog.Save);
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
