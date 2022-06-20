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
                    BedWardView = DataService.PatientIdentity.GetBedWardView(SelectedWard.LocationUID,"IPD");
                    WardName = SelectedWard.Name;
                    

                }
            }
        }


        private BedStatusModel _SelectBedData;

        public BedStatusModel SelectBedData
        {
            get { return _SelectBedData; }
            set
            {
                Set(ref _SelectBedData, value);
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


        public string PatientName { get; set; }
        public string PatientID { get; set; }

        #endregion

        #region Command

        private RelayCommand _NewRequestrCommand;

        public RelayCommand NewRequestrCommand
        {
            get { return _NewRequestrCommand ?? (_NewRequestrCommand = new RelayCommand(NewRequest)); }
        }

        private RelayCommand _LabResultCommand;

        public RelayCommand LabResultCommand
        {
            get { return _LabResultCommand ?? (_LabResultCommand = new RelayCommand(LabResult)); }
        }

        private RelayCommand _DischargeCommand;

        public RelayCommand DischargeCommand
        {
            get { return _DischargeCommand ?? (_DischargeCommand = new RelayCommand(Discharge)); }
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

        #endregion

        #region Method
        public WardViewModel()
        {
            WardSource = DataService.Technical.GetLocationByTypeUID(3152); //แก้
            SelectedWard = WardSource.FirstOrDefault(p => p.LocationUID == 35);
            BedWardView = DataService.PatientIdentity.GetBedWardView(SelectedWard.LocationUID,"IPD");
        }

        public void NewRequest()
        {

        }

        public void LabResult()
        {

        }

        public void VitalSign()
        {

        }
        public void wardveiewpge()
        {

        }

        public void Discharge()
        {
            if (SelectedBedWardView != null)
            {
                // SelectedBedWardView.
                // int parentlocaion = (SelectedBedWardView.ParentLocationUID ?? 0);
                //SelectBedData = DataService.PatientIdentity.GetBedByPatientVisit(parentlocaion).ToList();

                //IPDMedicalDischarge pageview = new IPDMedicalDischarge();
                //(pageview.DataContext as IPDMedicalDischargeViewModel).closeMed();
                //IPDMedicalDischargeViewModel result = (IPDMedicalDischargeViewModel)LaunchViewDialogNonPermiss(pageview, false);

                PatientVisitModel patietnvisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectedBedWardView.PatientVisitUID ?? 0);

                PatientStatus medicalDischarge = new PatientStatus(patietnvisit, PatientStatusType.MedicalDischarge);
                medicalDischarge.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                medicalDischarge.Owner = MainWindow;
                medicalDischarge.ShowDialog();
                ActionDialog result = medicalDischarge.ResultDialog;
                if (result == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    wardveiewpge();
                    
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
