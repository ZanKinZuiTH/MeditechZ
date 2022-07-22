using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MediTech.ViewModels
{
    public class WardViewModel : MediTechViewModelBase
    {
        #region Propoties
        public List<LocationModel> WardSource { get; set; }

        private LocationModel _SelectedWard;
        public LocationModel SelectedWard
        {
            get { return _SelectedWard; }
            set
            {
                Set(ref _SelectedWard, value);
                if (SelectedWard != null)
                {
                    BedWardView = DataService.PatientIdentity.GetBedWardView(SelectedWard.LocationUID);
                    WardName = SelectedWard.Name;
                }
            }
        }

        private String _WardName;
        public String WardName
        {
            get { return _WardName; }
            set { Set(ref _WardName, value); }
        }

        private List<BedStatusModel> _BedWardView;
        public List<BedStatusModel> BedWardView
        {
            get { return _BedWardView; }
            set { Set(ref _BedWardView, value); }
        }

        private BedStatusModel _SelectedBedWardView;
        public BedStatusModel SelectedBedWardView
        {
            get { return _SelectedBedWardView; }
            set { Set(ref _SelectedBedWardView, value); }
           
        }

        private String _DischageType;
        public String DischageType
        {
            get { return _DischageType; }
            set { Set(ref _DischageType, value); }
        }

        public string PatientName { get; set; }
        public string PatientID { get; set; }

        #endregion

        #region Command

        private RelayCommand _EditExpDischargeCommand;
        public RelayCommand EditExpDischargeCommand
        {
            get { return _EditExpDischargeCommand ?? (_EditExpDischargeCommand = new RelayCommand(EditExpDischarge)); }
        }

        private RelayCommand _LabResultCommand;
        public RelayCommand LabResultCommand
        {
            get { return _LabResultCommand ?? (_LabResultCommand = new RelayCommand(LabResult)); }
        }

        private RelayCommand<string> _DischargeCommand;
        public RelayCommand<string> DischargeCommand
        {
            get { return _DischargeCommand ?? (_DischargeCommand = new RelayCommand<string>(Discharge)); }
        }

        private RelayCommand _TranferCommand;
        public RelayCommand TranferCommand
        {
            get { return _TranferCommand ?? (_TranferCommand = new RelayCommand(BedTranfer)); }
        }

        private RelayCommand _DirectAdmitCommand;
        public RelayCommand DirectAdmitCommand
        {
            get { return _DirectAdmitCommand ?? (_DirectAdmitCommand = new RelayCommand(DirectAdmit)); }
        }

        private RelayCommand _BedStatusChangeCommand;
        public RelayCommand BedStatusChangeCommand
        {
            get { return _BedStatusChangeCommand ?? (_BedStatusChangeCommand = new RelayCommand(BedChangeStatus)); }
        }

        private RelayCommand _VitalSignCommand;
        public RelayCommand VitalSignCommand
        {
            get { return _VitalSignCommand ?? (_VitalSignCommand = new RelayCommand(VitalSign)); }
        }

        private RelayCommand _NewEMRCommand;
        public RelayCommand NewEMRCommand
        {
            get { return _NewEMRCommand ?? (_NewEMRCommand = new RelayCommand(NewEMR)); }
        }

        private RelayCommand _EventlogCommand;
        public RelayCommand EventlogCommand
        {
            get { return _EventlogCommand ?? (_EventlogCommand = new RelayCommand(Eventlog)); }
        }


        private RelayCommand _ArrivedCommand;
        public RelayCommand ArrivedCommand
        {
            get { return _ArrivedCommand ?? (_ArrivedCommand = new RelayCommand(Arrived)); }

        }
        private RelayCommand _CreateOrderCommand;
        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
        }


        private RelayCommand _PrintDocumentCommand;
        public RelayCommand PrintDocumentCommand
        {
            get { return _PrintDocumentCommand ?? (_PrintDocumentCommand = new RelayCommand(PrintDocument)); }
        }


        private RelayCommand _PatientRecordsCommand;
        public RelayCommand PatientRecordsCommand
        {
            get { return _PatientRecordsCommand ?? (_PatientRecordsCommand = new RelayCommand(PatientRecords)); }
        }

        private RelayCommand _PatientTrackingCommand;
        public RelayCommand PatientTrackingCommand
        {
            get { return _PatientTrackingCommand ?? (_PatientTrackingCommand = new RelayCommand(PatientTracking)); }
        }

        private RelayCommand _CancelDischargeCommand;
        public RelayCommand CancelDischargeCommand
        {
            get { return _CancelDischargeCommand ?? (_CancelDischargeCommand = new RelayCommand(CancelDischarge)); }
        }

        private RelayCommand _CancelAdmissionCommand;
        public RelayCommand CancelAdmissionCommand
        {
            get { return _CancelAdmissionCommand ?? (_CancelAdmissionCommand = new RelayCommand(CancelAdmission)); }
        }
        
        private RelayCommand _ModifyPayorCommand;
        public RelayCommand ModifyPayorCommand
        {
            get { return _ModifyPayorCommand ?? (_ModifyPayorCommand = new RelayCommand(ModifyPayor)); }
        }

        #endregion

        #region Method

        public WardViewModel()
        {
            WardSource = DataService.Technical.GetLocationByTypeUID(3152); //แก้
            SelectedWard = WardSource.FirstOrDefault(p => p.LocationUID == 35);
            AllBedStatus();

        }

        private void PatientRecords()
        {
            if (SelectedBedWardView != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectedBedWardView.PatientVisitUID ?? 0);
                EMRView pageview = new EMRView();
                (pageview.DataContext as EMRViewViewModel).AssingPatientVisit(patientVisit);
                EMRViewViewModel result = (EMRViewViewModel)LaunchViewDialog(pageview, "EMRVE", false, true);
            }
        }


        private void AllBedStatus()
        {
            BedWardView = DataService.PatientIdentity.GetBedWardView(SelectedWard.LocationUID);
        }

        private void VitalSign()
        {
            if (SelectedBedWardView != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectedBedWardView.PatientVisitUID ?? 0);
                PatientVitalSign pageview = new PatientVitalSign();
                (pageview.DataContext as PatientVitalSignViewModel).AssingPatientVisit(patientVisit);
                PatientVitalSignViewModel result = (PatientVitalSignViewModel)LaunchViewDialog(pageview, "PTVAT", false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    //SearchPatientVisit();
                }
            }

        }

        private void Arrived()
        {

            if (SelectedBedWardView != null)
            {
             
                PatientVisitModel patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectedBedWardView.PatientVisitUID ?? 0);
                //if (patientVisit.VISTSUID == CHKOUT || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL)
                //{
                //    WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                //    SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
                //    SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
                //    OnUpdateEvent();
                //    return;
                //}
   
                PatientStatus arrived = new PatientStatus(patientVisit, PatientStatusType.WardArrived, Convert.ToInt32(SelectedBedWardView.AdmissionEventUID));
                arrived.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                arrived.Owner = MainWindow;
                arrived.ShowDialog();
                ActionDialog result = arrived.ResultDialog;
                if (result == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    AllBedStatus();
                }
            }
        }

        private void PrintDocument()
        {
            if (SelectedBedWardView != null)
            {
                PatientVisitModel visitModel = new PatientVisitModel();
                visitModel.PatientID = SelectedBedWardView.PatientID;
                visitModel.PatientUID = SelectedBedWardView.PatientUID;
                visitModel.PatientVisitUID = SelectedBedWardView.PatientVisitUID ?? 0;
                visitModel.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                ShowModalDialogUsingViewModel(new RunPatientReports(), new RunPatientReportsViewModel() { SelectPatientVisit = visitModel }, true);
            }
        }

        private void CreateOrder()
        {
            if (SelectedBedWardView != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectedBedWardView.PatientVisitUID ?? 0);
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(patientVisit);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);
            }
        }
        private void PatientTracking()
        {
            if (SelectedBedWardView != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectedBedWardView.PatientVisitUID ?? 0);
                PatientTracking pageview = new PatientTracking();
                (pageview.DataContext as PatientTrackingViewModel).AssingModel(patientVisit);
                PatientTrackingViewModel result = (PatientTrackingViewModel)LaunchViewDialog(pageview, "PATRCK", false);
            }
        }

        public void LabResult()
        {

        }

        public void EditExpDischarge()
        {
            if (SelectedBedWardView != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectedBedWardView.PatientVisitUID ?? 0);
                var admission = DataService.PatientIdentity.GetAdmissionEventByPatientVisitUID(SelectedBedWardView.PatientVisitUID ?? 0);
                EditExpDischargeDate pageview = new EditExpDischargeDate();
                (pageview.DataContext as EditExpDischargeDateViewModel).AssignModel(patientVisit, admission);
                EditExpDischargeDateViewModel result = (EditExpDischargeDateViewModel)LaunchViewDialog(pageview, "EDDTTM", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    AllBedStatus();
                }
            }
        }

        public void Discharge(string type)
        {
            if (SelectedBedWardView != null)
                {
                DischargeEventModel model = new DischargeEventModel();

                    if (type == "DischargeAdvice")
                    {
                        IPDMedicalDischarge pageview = new IPDMedicalDischarge();
                        (pageview.DataContext as IPDMedicalDischargeViewModel).AssingModel(SelectedBedWardView, model, type);
                        IPDMedicalDischargeViewModel result = (IPDMedicalDischargeViewModel)LaunchViewDialog(pageview, "IPDDD", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            SaveSuccessDialog();
                        }
                    }

                    if (type == "MedicalDischarge")
                    {
                        model = DataService.PatientIdentity.GetDischargeEventByAdmissionUID(SelectedBedWardView.AdmissionEventUID ?? 0); ;
                        
                        IPDMedicalDischarge pageview = new IPDMedicalDischarge();
                        (pageview.DataContext as IPDMedicalDischargeViewModel).AssingModel(SelectedBedWardView, model, type);
                        IPDMedicalDischargeViewModel result = (IPDMedicalDischargeViewModel)LaunchViewDialog(pageview, "IPDDD", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            SaveSuccessDialog();
                        }
                    }

                    if (type == "Discharge")
                    {
                        model = DataService.PatientIdentity.GetDischargeEventByAdmissionUID(SelectedBedWardView.AdmissionEventUID ?? 0); ;

                        IPDMedicalDischarge pageview = new IPDMedicalDischarge();
                        (pageview.DataContext as IPDMedicalDischargeViewModel).AssingModel(SelectedBedWardView, model, type);
                        IPDMedicalDischargeViewModel result = (IPDMedicalDischargeViewModel)LaunchViewDialog(pageview, "IPDDD", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            SaveSuccessDialog();
                        }
                    }

                AllBedStatus();
            }
        }

        public void CancelDischarge()
        {
            if (SelectedBedWardView != null)
            {
               
                CancelDischarge pageview = new CancelDischarge();
                (pageview.DataContext as CancelDischargeViewModel).AssingModel(SelectedBedWardView, "Discharge");
                CancelDischargeViewModel result = (CancelDischargeViewModel)LaunchViewDialog(pageview, "CCDHRG", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    AllBedStatus();
                }
            }
        }

        public void CancelAdmission()
        {
            if (SelectedBedWardView != null)
            {
                PatientADTEventModel eventModel = DataService.PatientIdentity.GetPatientADTEventType(SelectedBedWardView.PatientVisitUID ?? 0, "ADMISSIONEVENT");

                var time = eventModel.EventOccuredDttm.AddDays(1);
                DateTime now = DateTime.Now;
                if (now > time)
                {
                    WarningDialog("Can't cancel Admission : AdmitDate more than duration Cancel Admission ");
                    return;
                }

                CancelDischarge pageview = new CancelDischarge();
                (pageview.DataContext as CancelDischargeViewModel).AssingModel(SelectedBedWardView, "Admission");
                CancelDischargeViewModel result = (CancelDischargeViewModel)LaunchViewDialog(pageview, "CCDHRG", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    AllBedStatus();
                }
            }
        }

        private void ModifyPayor()
        {
            if (SelectedBedWardView != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectedBedWardView.PatientVisitUID ?? 0);
                ModifyVisitPayor pageview = new ModifyVisitPayor();
                (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(patientVisit);
                ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    AllBedStatus();
                }
            }
        }


        public void DirectAdmit()
        {
            if (SelectedBedWardView != null)
            { 
                AdmissionDetail pageview = new AdmissionDetail();
                (pageview.DataContext as AdmissionDetailViewModel).SendbedWard(SelectedBedWardView);
                AdmissionDetailViewModel result = (AdmissionDetailViewModel)LaunchViewDialogNonPermiss(pageview, false);
            }
        }

        public void BedTranfer()
        {
            if (SelectedBedWardView != null)
            {
                TranferBed pageview = new TranferBed();
                (pageview.DataContext as TranferBedViewModel).SendingBed(SelectedBedWardView);
                TranferBedViewModel result = (TranferBedViewModel)LaunchViewDialogNonPermiss(pageview, false);
            }
        }


        public void BedChangeStatus()
        {
            if (SelectedBedWardView != null)
            {
                BedStatusChanged pageview = new BedStatusChanged();
                 (pageview.DataContext as BedChangedStatusViewModel).SendingBed(SelectedBedWardView);
                BedChangedStatusViewModel result = (BedChangedStatusViewModel)LaunchViewDialogNonPermiss(pageview, false);
            }
          
        }

        public void NewEMR()
        {
            AdmissionRequest pageview = new AdmissionRequest();
            AdmissionRequestViewModel result = (AdmissionRequestViewModel)LaunchViewDialog(pageview, "ADRQST", false);
        }

        public void Eventlog()
        {

        }
        

        #endregion
    }
}
