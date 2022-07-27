using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediTech.ViewModels
{
   public class TranferBedViewModel : MediTechViewModelBase
    {

        #region properties

        public List<LocationModel> WardSource { get; set; }

        private LocationModel _SelectedWard;
        public LocationModel SelectedWard
        {
            get { return _SelectedWard; }
            set
            {
                Set(ref _SelectedWard, value);
                if (_SelectedWard != null)
                {
                    ListWard = DataService.PatientIdentity.GetBedLocation(SelectedWard.LocationUID, null).Where(p => p.BedIsUse == "N").ToList();
                }
            }
        }

        private string _SearchPatientCriteria;

        public string SearchPatientCriteria
        {
            get { return _SearchPatientCriteria; }
            set
            {
                Set(ref _SearchPatientCriteria, value);
                PatientsSearchSource = null;
            }
        }

        private List<PatientInformationModel> _PatientsSearchSource;

        public List<PatientInformationModel> PatientsSearchSource
        {
            get { return _PatientsSearchSource; }
            set { Set(ref _PatientsSearchSource, value); }
        }



        private PatientInformationModel _SelectedPateintSearch;

        public PatientInformationModel SelectedPateintSearch
        {
            get { return _SelectedPateintSearch; }
            set
            {
                _SelectedPateintSearch = value;
                if (_SelectedPateintSearch != null)
                {
                    SearchPatientVisit();
                }
            }
        }




        private List<LocationModel> _ListWard;
        public List<LocationModel> ListWard
        {
            get { return _ListWard; }
            set { Set(ref _ListWard, value); }
        }

        private LocationModel _SelectedListWard;
        public LocationModel SelectedListWard
        {
            get { return _SelectedListWard; }
            set { Set(ref _SelectedListWard, value); }
        }


        private List<LookupReferenceValueModel> _BedCatagory;

        public List<LookupReferenceValueModel> BedCatagory
        {
            get { return _BedCatagory; }
            set { Set(ref _BedCatagory, value); }
        }


        private LookupReferenceValueModel _SelectBedCatagory;

        public LookupReferenceValueModel SelectBedCatagory
        {
            get { return _SelectBedCatagory; }
            set { Set(ref _SelectBedCatagory, value); }
        }

        public List<CareproviderModel> Doctors { get; set; }


        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { _SelectDoctor = value; }
        }

        public List<LocationModel> BedTranfer { get; set; }


        private LocationModel _SelectBedTranfer;

        public LocationModel SelectBedTranfer
        {
            get { return _SelectBedTranfer; }
            set { _SelectBedTranfer = value; }
        }


        private List<LocationModel> _ListBedStatus;

        public List<LocationModel> ListBedStatus
        {
            get { return _ListBedStatus; }
            set { Set(ref _ListBedStatus, value); }
        }


        public List<LocationModel> Location 
        { 
            get; set; 
        }


        private LocationModel _Bedsending;

        public LocationModel Bedsending
        {
            get { return _Bedsending; }
            set { Set(ref _Bedsending, value); }
        }


        private LocationModel _SelectLocation;

        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set { _SelectLocation = value; }
        }


        #endregion

        #region command


        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }


        private RelayCommand _SaveTranferCommand;

        public RelayCommand SaveTranferCommand
        {

            get { return _SaveTranferCommand ?? (_SaveTranferCommand = new RelayCommand(SaveTranfer)); }
        }


        public List<LookupReferenceValueModel> StatusSource { get; set; }

        private LookupReferenceValueModel _SelectedStatus;
        public LookupReferenceValueModel SelectedStatus
        {
            get { return _SelectedStatus; }
            set { Set(ref _SelectedStatus, value); }
        }


        #endregion

        #region method


        public TranferBedViewModel()
        {
            BedCatagory = DataService.Technical.GetReferenceValueMany("BEDCAT");
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            var locationAll = DataService.InPatientService.GetBedALL();
            WardSource = locationAll.Where(w => w.LOTYPUID == 3152).ToList();
            StatusSource = DataService.Technical.GetReferenceValueList("BKTYP");

        }


        private void SaveTranfer() 
        {

            //LocationModel locamodel = new LocationModel();
            //locamodel.LocationUID = Bedsending.LocationUID;
            //locamodel.LCTSTUID = SelectStatusBed.Key;
            //locamodel.Comment = Comment;
            //locamodel.ActiveTo = ActiveToDate;

            //DataService.IPDService.ChangeBedStatus(locamodel, AppUtil.Current.UserID);

            SaveSuccessDialog();
            CloseViewDialog(ActionDialog.Save);


            //SaveSuccessDialog();


        }


        private void SearchPatientVisit()
        {
            long UIDPatient;
            if (SearchPatientCriteria != "" && SelectedPateintSearch != null)
            {
                UIDPatient = SelectedPateintSearch.PatientUID;
                var patientInfo = DataService.PatientIdentity.GetPatientByUID(UIDPatient);
                (this.View as TranferBed).patientBanner.SetPatientBanner(patientInfo.PatientUID, 0);
            }


        }



        public void SendingBed(BedStatusModel SelectBed)
        {
            //BedHeader = SelectBed.Name + ": " + SelectBed.Description;
           // List<LocationModel> beddata = new List<LocationModel>();
            Bedsending = DataService.Technical.GetLocationByUID(SelectBed.LocationUID);
            //beddata.Add(Bedsending);
            //ListBedStatus = beddata;
            // LocationModel bedaddsign = DataService.Technical.GetLocationByUID(SelectBed.LocationUID);
            //ListBedStatus.Add(bedaddsign);

        }

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty; ;
            string lastName = string.Empty;
            if (SearchPatientCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientCriteria;
                        lastName = "empty";
                    }

                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, null);
                PatientsSearchSource = searchResult;
            }
            else
            {

                PatientsSearchSource = null;

            }

        }



        #endregion


    }
}
