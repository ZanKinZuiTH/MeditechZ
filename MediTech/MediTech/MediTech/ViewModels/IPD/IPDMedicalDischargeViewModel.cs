using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public  class IPDMedicalDischargeViewModel : MediTechViewModelBase
    {
        #region properties

        private List<LookupReferenceValueModel> _DischargeStatus;
        public List<LookupReferenceValueModel> DischargeStatus
        {
            get { return _DischargeStatus; }
            set { Set(ref _DischargeStatus, value); }
        }

        private LookupReferenceValueModel _SelectDischargeStatus;
        public LookupReferenceValueModel SelectDischargeStatus
        {
            get { return _SelectDischargeStatus; }
            set { Set(ref _SelectDischargeStatus, value); }
        }

        private DateTime _DischargeDate;
        public DateTime DischargeDate
        {
            get { return _DischargeDate; }
            set {Set(ref _DischargeDate, value);}
        }

        private List<CareproviderModel> _DoctorsRecommended;
        public List<CareproviderModel> DoctorsRecommended
        {
            get { return _DoctorsRecommended; }
            set { Set(ref _DoctorsRecommended, value); }
        }

        private CareproviderModel _SelectDoctorsRecommended;
        public CareproviderModel SelectDoctorsRecommended
        {
            get { return _SelectDoctorsRecommended; }
            set { Set(ref _SelectDoctorsRecommended, value); }
        }

        private List<LookupReferenceValueModel> _InfectionTypes;
        public List<LookupReferenceValueModel> InfectionTypes
        {
            get { return _InfectionTypes; }
            set { Set(ref _InfectionTypes, value); }
        }

        private LookupReferenceValueModel _SelectInfectionTypes;
        public LookupReferenceValueModel SelectInfectionTypes
        {
            get { return _SelectInfectionTypes; }
            set { Set(ref _SelectInfectionTypes, value); }
        }

        private List<LookupReferenceValueModel> _DispositionTypes;
        public List<LookupReferenceValueModel> DispositionTypes
        {
            get { return _DispositionTypes; }
            set { Set(ref _DispositionTypes, value); }
        }

        private LookupReferenceValueModel _SelectDispositionTypes;
        public LookupReferenceValueModel SelectDispositionTypes
        {
            get { return _SelectDispositionTypes; }
            set { Set(ref _SelectDispositionTypes, value); }
        }

        private List<LookupReferenceValueModel> _DischargeTypes;
        public List<LookupReferenceValueModel> DischargeTypes
        {
            get { return _DischargeTypes; }
            set { Set(ref _DischargeTypes, value); }
        }

        private LookupReferenceValueModel _SelectDischargeTypes;
        public LookupReferenceValueModel SelectDischargeTypes
        {
            get { return _SelectDischargeTypes; }
            set { Set(ref _SelectDischargeTypes, value); }
        }

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { Set(ref _Comment, value); }
        }

        private List<LookupReferenceValueModel> _ModeTransport;
        public List<LookupReferenceValueModel> ModeTransport
        {
            get { return _ModeTransport; }
            set { Set(ref _ModeTransport, value); }
        }

        private LookupReferenceValueModel _SelectModeTransport;
        public LookupReferenceValueModel SelectModeTransport
        {
            get { return _SelectModeTransport; }
            set { Set(ref _SelectModeTransport, value); }
        }

        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
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


        #region method

        DischargeEventModel dischargeModel;
        BedStatusModel bedmodel;
        string dischargeType;
        public IPDMedicalDischargeViewModel()
        {
            DischargeStatus = DataService.Technical.GetReferenceValueMany("DCSTS");
            DischargeDate = DateTime.Now;
            DoctorsRecommended = DataService.UserManage.GetCareproviderDoctor();
            InfectionTypes = DataService.Technical.GetReferenceValueMany("INFCT");
            DispositionTypes = DataService.Technical.GetReferenceValueMany("DSOCM");
            DischargeTypes = DataService.Technical.GetReferenceValueMany("DSCTYP");
            ModeTransport = DataService.Technical.GetReferenceValueMany("MDTRN");
        }

        private void Save()
        {
            if(SelectDischargeStatus == null)
            {
                WarningDialog("กรุณาเลือก Status of Medical Discharge");
                return;
            }
            AssingPropertiesToModel();

            if (dischargeType == "DischargeAdvice")
            {
                dischargeModel.AdvicedBy = AppUtil.Current.UserID;
                dischargeModel.DischargeAdviceDttm = DischargeDate;
                dischargeModel.ENSTAUID = DataService.Technical.GetReferenceValueByCode("ENSTA", "DSGADV").Key;
            }

            if (dischargeType == "Discharge")
            {
                MessageBoxResult result = QuestionDialog("Do you really want to Discahrge the Patient ?");
                if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                {
                    return;
                }
            }

            DischargeEventModel returnData = DataService.PatientIdentity.ManageIPDDischargeEvent(dischargeModel, AppUtil.Current.UserID);

            CloseViewDialog(ActionDialog.Save);
        }

        public void AssingModel(BedStatusModel model, DischargeEventModel discharge, string type)
        {
            bedmodel = model;
            SelectPatientVisit = DataService.PatientIdentity.GetPatientVisitByUID(model.PatientVisitUID ?? 0);
            if (discharge == null)
                discharge = new DischargeEventModel();
            dischargeModel = discharge;
            dischargeType = type;

            //if (type == "DischargeAdvice")
            //{
            //    dischargeModel.ENSTAUID = DataService.Technical.GetReferenceValueByCode("ENSTA", "DSGADV").Key;
            //}

            if (type == "MedicalDischarge")
            {
                dischargeModel.VISTSUID = DataService.Technical.GetReferenceValueByCode("VISTS", "CHKOUT").Key ?? 0;
                dischargeModel.ENSTAUID = DataService.Technical.GetReferenceValueByCode("ENSTA", "FTDSG").Key;
                AssingModelToProperties();
            }

            if (type == "Discharge")
            {
                //dischargeModel.VISTSUID = DataService.Technical.GetReferenceValueByCode("VISTS","CHKOUT").Key ?? 0;
                dischargeModel.ENSTAUID = DataService.Technical.GetReferenceValueByCode("ENSTA","DISCH").Key ?? 0;
                AssingModelToProperties();
            }

            SelectDoctorsRecommended = DoctorsRecommended.FirstOrDefault(p => p.CareproviderUID == bedmodel.CareproviderUID);
        }

        private void AssingPropertiesToModel()
        {
            if (dischargeModel == null)
                dischargeModel = new DischargeEventModel();

            dischargeModel.AdmissionEventUID = bedmodel.AdmissionEventUID ?? 0;
            dischargeModel.PatientUID = bedmodel.PatientUID;
            dischargeModel.PatientVisitUID = bedmodel.PatientVisitUID ?? 0;

            dischargeModel.DSCSTUID = SelectDischargeStatus != null ? SelectDischargeStatus.Key ?? 0 : 0;
            dischargeModel.MedicalDischargeDttm = DischargeDate;
            dischargeModel.ActualDischargeDttm = DischargeDate;
            dischargeModel.RecordedBy = AppUtil.Current.UserID;
            dischargeModel.MDTRNUID = SelectModeTransport != null ? SelectModeTransport.Key ?? 0 : 0; 
            dischargeModel.DischargeComments = Comment;
            dischargeModel.DSCTYUID = SelectDischargeTypes != null ? SelectDischargeTypes.Key ?? 0 : 0;
            dischargeModel.INFCTUID = SelectInfectionTypes != null ? SelectInfectionTypes.Key : (int?)null;
            dischargeModel.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            dischargeModel.DSOCMUID = SelectDispositionTypes != null ? SelectDispositionTypes.Key : (int?)null;
        }

        private void AssingModelToProperties()
        {
            SelectDischargeStatus = dischargeModel.DSCSTUID != 0 ? DischargeStatus.FirstOrDefault(p => p.Key == dischargeModel.DSCSTUID) : null;
            SelectModeTransport = dischargeModel.MDTRNUID != 0 ? ModeTransport.FirstOrDefault(p => p.Key == dischargeModel.MDTRNUID) : null;
            SelectDischargeTypes = dischargeModel.DSCTYUID != 0 ? DischargeTypes.FirstOrDefault(p => p.Key == dischargeModel.DSCTYUID) : null;
            SelectInfectionTypes = dischargeModel.INFCTUID != 0 ? InfectionTypes.FirstOrDefault(p => p.Key == dischargeModel.INFCTUID) : null;
            SelectDispositionTypes = dischargeModel.DSOCMUID != null ? DispositionTypes.FirstOrDefault(p => p.Key == dischargeModel.DSOCMUID) : null;
            Comment = dischargeModel.DischargeComments;
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion

    }
}
