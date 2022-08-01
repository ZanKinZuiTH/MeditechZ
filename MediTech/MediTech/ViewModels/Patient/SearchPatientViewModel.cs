using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MediTech.DataService;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class SearchPatientViewModel : MediTechViewModelBase
    {
        #region Event

        public event PropertyChangedEventHandler SelectedPatientChanged;
        public event PropertyChangedEventHandler SelectedBookingChanged;
        protected void OnSelectedPatientChanged()
        {
            PropertyChangedEventHandler handler = SelectedPatientChanged;
            if (handler != null)
            {
                handler(SelectedPatient, new PropertyChangedEventArgs("SelectedPatient"));
            }
        }

        protected void OnSelectedBookingChanged()
        {
            PropertyChangedEventHandler handler = SelectedBookingChanged;
            if (handler != null)
            {
                handler(SelectBooking, new PropertyChangedEventArgs("SelectBooking"));
            }
        }

        #endregion


        #region Properties

        private string _PatientID;

        public string PatientID
        {
            get { return _PatientID; }
            set { _PatientID = value; RaisePropertyChanged("PatientID"); }
        }

        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; RaisePropertyChanged("FirstName"); }
        }

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; RaisePropertyChanged("LastName"); }
        }

        private string _MiddleName;

        public string MiddleName
        {
            get { return _MiddleName; }
            set { _MiddleName = value; RaisePropertyChanged("MiddleName"); }
        }

        private string _NickName;

        public string NickName
        {
            get { return _NickName; }
            set { _NickName = value; RaisePropertyChanged("NickName"); }
        }

        private DateTime? _BirthDate = null;

        public DateTime? BirthDate
        {
            get { return _BirthDate; }
            set { _BirthDate = value; RaisePropertyChanged("BirthDate"); }
        }

        private string _NationalID;

        public string NationalID
        {
            get { return _NationalID; }
            set { _NationalID = value; RaisePropertyChanged("NationalID"); }
        }

        private string _MobilePhone;
        public string MobilePhone
        {
            get { return _MobilePhone; }
            set { _MobilePhone = value; RaisePropertyChanged("MobilePhone"); }
        }

        private DateTime? _LastVisitDate = null;

        public DateTime? LastVisitDate
        {
            get { return _LastVisitDate; }
            set { _LastVisitDate = value; RaisePropertyChanged("LastVisitDate"); }
        }


        public List<LookupReferenceValueModel> GenderSource { get; set; }

        private LookupReferenceValueModel _SelectedGender;
        public LookupReferenceValueModel SelectedGender
        {
            get { return _SelectedGender; }
            set { Set(ref _SelectedGender, value); }
        }
        private List<PatientInformationModel> _PatientSource;

        public List<PatientInformationModel> PatientSource
        {
            get { return _PatientSource; }
            set { _PatientSource = value; RaisePropertyChanged("PatientSource"); }
        }

        private PatientInformationModel _SelectedPatient;
        public PatientInformationModel SelectedPatient
        {
            get { return _SelectedPatient; }
            set
            {
                _SelectedPatient = value;
                RaisePropertyChanged("SelectedPatient");
                OnSelectedPatientChanged();
                if (_SelectedPatient != null)
                {
                    DateTime now = DateTime.Now;
                    BookingSource = DataService.PatientIdentity.SearchBookingNotExistsVisit(now, null, null, SelectedPatient.PatientUID, 2944,null,AppUtil.Current.OwnerOrganisationUID);
                    SelectBooking = BookingSource.FirstOrDefault();
                    //PastVisits = DataService.PatientIdentity.GetPatientVisitByPatientUID(SelectedPatient.PatientUID);
                }
            }
        }

        private List<BookingModel> _BookingSource;

        public List<BookingModel> BookingSource
        {
            get { return _BookingSource; }
            set { Set(ref _BookingSource, value); }
        }

        private BookingModel _SelectBooking;

        public BookingModel SelectBooking
        {
            get { return _SelectBooking; }
            set
            {
                Set(ref _SelectBooking, value);
                OnSelectedBookingChanged();
            }
        }

        //private List<PatientVisitModel> _PastVisits;

        //public List<PatientVisitModel> PastVisits
        //{
        //    get { return _PastVisits; }
        //    set { Set(ref _PastVisits, value); }
        //}


        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPatient)); }
        }

        private RelayCommand _ResetCommand;

        public RelayCommand ResetCommand
        {
            get { return _ResetCommand ?? (_ResetCommand = new RelayCommand(ResetInput)); }
        }


        #endregion

        public SearchPatientViewModel()
        {
            GenderSource = DataService.Technical.GetReferenceValueMany("SEXXX");
        }

        public void SearchPatient()
        {
            string gender = "";
            if (SelectedGender != null)
            {
                gender = SelectedGender.Display;
            }
            SearchPatient(PatientID, FirstName, LastName, MiddleName, NickName, BirthDate, NationalID, LastVisitDate, gender, MobilePhone);
            if (PatientSource == null || PatientSource.Count <= 0)
            {
                InformationDialog("ไม่พบข้อมูลปู้ป่วย");
                SelectedPatient = null;
            }
            else
            {
                SelectedPatient = PatientSource.FirstOrDefault();
            }
        }

        public void SearchPatient(string pateintID, string firstName, string lastName, string middleName, string nickName, DateTime? birthDate, string nationalID, DateTime? lastVisitData, string gender, string mobilePhone)
        {
            int? sexxxUID = null;
            if (GenderSource != null)
            {
               var  selectGender = GenderSource.FirstOrDefault(p => p.Display.Contains(gender) && !string.IsNullOrEmpty(gender));
               if (selectGender != null)
               {
                   sexxxUID = selectGender.Key;
               }
            }

            if (String.IsNullOrEmpty(pateintID) && String.IsNullOrEmpty(firstName) && String.IsNullOrEmpty(lastName) && String.IsNullOrEmpty(middleName) && String.IsNullOrEmpty(nickName) && birthDate == null && String.IsNullOrEmpty(nationalID) && lastVisitData == null && sexxxUID == null && mobilePhone == null)
            {
                PatientSource = null;
                return;
            }


            PatientSource = DataService.PatientIdentity.SearchPatient(pateintID, firstName, middleName, lastName, nickName, birthDate, sexxxUID, nationalID, lastVisitData, MobilePhone);
            SelectedPatient = PatientSource?.FirstOrDefault();
        }

        public void ResetInput()
        {
            PatientID = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            MiddleName = string.Empty;
            NickName = string.Empty;
            BirthDate = null;
            NationalID = string.Empty;
            LastVisitDate = null;
            SelectedGender = null;
            MobilePhone = null;

            PatientSource = null;
        }

    }
}
