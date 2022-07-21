using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
  public  class CancelDischargeViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<LookupReferenceValueModel> _CancelDischarge;
        public List<LookupReferenceValueModel> CancelDischarge
        {
            get { return _CancelDischarge; }
            set { Set(ref _CancelDischarge, value); }
        }

        private LookupReferenceValueModel _SelectCancelDischarge;
        public LookupReferenceValueModel SelectCancelDischarge
        {
            get { return _SelectCancelDischarge; }
            set { Set(ref _SelectCancelDischarge, value); }
        }

        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { Set(ref _Comment, value); }
        }

        private string _CancelType;
        public string CancelType
        {
            get { return _CancelType; }
            set { Set(ref _CancelType, value); }
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


        #endregion

        DischargeEventModel DischargeEvent;
        BedStatusModel bedModel;

        #region Method
        public CancelDischargeViewModel()
        {
            CancelDischarge = DataService.Technical.GetReferenceValueMany("CANRS");
        }

        private void Save()
        {
            if(SelectCancelDischarge == null)
            {
                WarningDialog("กรุณาใส่ Cancel Reason");
                return;
            }

            if(CancelType == "Discharge")
            {
                PropertiesToDischargeModel();
                DataService.PatientIdentity.CancelDischargeEvent(DischargeEvent, AppUtil.Current.UserID);
            }

            if (CancelType == "Admission")
            {
                int statusuid = DataService.Technical.GetReferenceValueByCode("VISTS", "CANCD").Key ?? 0;
                DataService.PatientIdentity.CancelAdmission(Convert.ToInt32(bedModel.PatientVisitUID ?? 0),bedModel.LocationUID, statusuid, AppUtil.Current.UserID);
            }

            CloseViewDialog(ActionDialog.Save);
        }

        public void AssingModel(BedStatusModel model, string type)
        {
            SelectPatientVisit = DataService.PatientIdentity.GetPatientVisitByUID(model.PatientVisitUID ?? 0);
            CancelType = type;
            bedModel = model;
        }

        private void PropertiesToDischargeModel()
        {
            if (DischargeEvent == null)
                DischargeEvent = new DischargeEventModel();

            DischargeEvent.AdmissionEventUID = bedModel.AdmissionEventUID ?? 0;
            DischargeEvent.PatientUID = bedModel.PatientUID;
            DischargeEvent.PatientVisitUID = bedModel.PatientVisitUID ?? 0;
            DischargeEvent.CancelledBy = AppUtil.Current.UserName;
            DischargeEvent.CancelledDttm = DateTime.Now;
            DischargeEvent.CARNSUID = SelectCancelDischarge.Key;
            DischargeEvent.ENSTAUID = DataService.Technical.GetReferenceValueByCode("ENSTA", "ADMIT").Key;
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        #endregion
    }
}
