using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private List<LocationModel> _BedWardView;
        public List<LocationModel> BedWardView
        {
            get { return _BedWardView; }
            set { Set(ref _BedWardView, value); }
        }

        private LocationModel _SelectedBedWardView;
        public LocationModel SelectedBedWardView
        {
            get { return _SelectedBedWardView; }
            set { Set(ref _SelectedBedWardView, value); }
        }

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

        private RelayCommand _DirectAdmitCommand;

        public RelayCommand DirectAdmitCommand
        {
            get { return _DirectAdmitCommand ?? (_DirectAdmitCommand = new RelayCommand(DirectAdmit)); }
        }

        private RelayCommand _BedStatusChangeCommand;

        public RelayCommand BedStatusChangeCommand
        {
            get { return _BedStatusChangeCommand ?? (_BedStatusChangeCommand = new RelayCommand(BedStatusChange)); }
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
            BedWardView = DataService.PatientIdentity.GetBedWardView(SelectedWard.LocationUID);
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

        public void Discharge()
        {

        }

        public void DirectAdmit()
        {

        }

        public void BedStatusChange()
        {

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
