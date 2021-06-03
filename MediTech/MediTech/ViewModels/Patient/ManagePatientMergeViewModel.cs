using GalaSoft.MvvmLight.Command;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace MediTech.ViewModels
{
    public class ManagePatientMergeViewModel : MediTechViewModelBase
    {
        private char Addr;
        private char BGroup;
        private char Sex;
        private char Photo;
        private char Phone;
        #region Properties

        #region PatientPrimarySearch

        private string _SearchPatientPrimaryCriteria;

        public string SearchPatientPrimaryCriteria
        {
            get { return _SearchPatientPrimaryCriteria; }
            set
            {
                Set(ref _SearchPatientPrimaryCriteria, value);
                PatientsPrimarySearchSource = null;
            }
        }


        private List<PatientInformationModel> _PatientsPrimarySearchSource;

        public List<PatientInformationModel> PatientsPrimarySearchSource
        {
            get { return _PatientsPrimarySearchSource; }
            set { Set(ref _PatientsPrimarySearchSource, value); }
        }

        private PatientInformationModel _SelectedPateintPrimarySearch;

        public PatientInformationModel SelectedPateintPrimarySearch
        {
            get { return _SelectedPateintPrimarySearch; }
            set
            {
                Set(ref _SelectedPateintPrimarySearch, value);

                Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/Other/Blue-Pictures-icon.png");
                BitmapImage image = new BitmapImage(uri);

                if (SelectedPateintPrimarySearch != null)
                {
                    VisitPrimaryList = DataService.PatientIdentity.GetPatientVisitByPatientUID(SelectedPateintPrimarySearch.PatientUID);
                    if (SelectedPateintPrimarySearch.PatientImage != null)
                    {
                        PatientPrimaryImage = ImageHelpers.ConvertByteToBitmap(SelectedPateintPrimarySearch.PatientImage);
                    }
                    else
                    {
                        PatientPrimaryImage = image;
                    }
                }
                else
                {
                    PatientPrimaryImage = image;
                }
            }
        }


        #endregion

        #region PatientSecondarySearch

        private string _SearchPatientSecondaryCriteria;

        public string SearchPatientSecondaryCriteria
        {
            get { return _SearchPatientSecondaryCriteria; }
            set
            {
                Set(ref _SearchPatientSecondaryCriteria, value);
                PatientsSecondarySearchSource = null;
            }
        }


        private List<PatientInformationModel> _PatientsSecondarySearchSource;

        public List<PatientInformationModel> PatientsSecondarySearchSource
        {
            get { return _PatientsSecondarySearchSource; }
            set { Set(ref _PatientsSecondarySearchSource, value); }
        }

        private PatientInformationModel _SelectedPateintSecondarySearch;

        public PatientInformationModel SelectedPateintSecondarySearch
        {
            get { return _SelectedPateintSecondarySearch; }
            set
            {
                Set(ref _SelectedPateintSecondarySearch, value);

                Uri uri = new Uri(@"pack://application:,,,/MediTech;component/Resources/Images/Other/Blue-Pictures-icon.png");
                BitmapImage image = new BitmapImage(uri);

                if (SelectedPateintSecondarySearch != null)
                {

                    VisitSecondaryList = DataService.PatientIdentity.GetPatientVisitByPatientUID(SelectedPateintSecondarySearch.PatientUID);
                    if (SelectedPateintSecondarySearch.PatientImage != null)
                    {
                        PatientSecondaryImage = ImageHelpers.ConvertByteToBitmap(SelectedPateintSecondarySearch.PatientImage);
                    }
                    else
                    {

                        PatientSecondaryImage = image;
                    }
                }
                else
                {
                    PatientSecondaryImage = image;
                }
            }
        }


        #endregion

        private BitmapImage _PatientPrimaryImage;
        public BitmapImage PatientPrimaryImage
        {
            get
            {
                return _PatientPrimaryImage;
            }
            set
            {
                Set(ref _PatientPrimaryImage, value);
            }
        }

        private List<PatientVisitModel> _VisitPrimaryList;

        public List<PatientVisitModel> VisitPrimaryList
        {
            get { return _VisitPrimaryList; }
            set { Set(ref _VisitPrimaryList, value); }
        }


        private BitmapImage _PatientSecondaryImage;
        public BitmapImage PatientSecondaryImage
        {
            get
            {
                return _PatientSecondaryImage;
            }
            set
            {
                Set(ref _PatientSecondaryImage, value);
            }
        }

        private List<PatientVisitModel> _VisitSecondaryList;

        public List<PatientVisitModel> VisitSecondaryList
        {
            get { return _VisitSecondaryList; }
            set { Set(ref _VisitSecondaryList, value); }
        }

        private bool _IsAddrCheck;

        public bool IsAddrCheck
        {
            get { return _IsAddrCheck; }
            set
            {
                Set(ref _IsAddrCheck, value);
                if (IsAddrCheck)
                {
                    Addr = 'Y';
                }
                else
                {
                    Addr = 'N';
                }
            }
        }

        private bool _IsAddrMinCheck;

        public bool IsAddrMinCheck
        {
            get { return _IsAddrMinCheck; }
            set
            {
                Set(ref _IsAddrMinCheck, value);
                if (IsAddrMinCheck)
                {
                    Addr = 'N';
                }
                else
                {
                    Addr = 'Y';
                }
            }
        }


        private bool _IsBGroupCheck;

        public bool IsBGroupCheck
        {
            get { return _IsBGroupCheck; }
            set
            {
                Set(ref _IsBGroupCheck, value);
                if (IsBGroupCheck)
                {
                    Addr = 'Y';
                }
                else
                {
                    Addr = 'N';
                }
            }
        }

        private bool _IsBGroupMinCheck;

        public bool IsBGroupMinCheck
        {
            get { return _IsBGroupMinCheck; }
            set
            {
                Set(ref _IsBGroupMinCheck, value);
                if (IsBGroupMinCheck)
                {
                    Addr = 'N';
                }
                else
                {
                    Addr = 'Y';
                }
            }
        }

        private bool _IsSexCheck;

        public bool IsSexCheck
        {
            get { return _IsSexCheck; }
            set
            {
                Set(ref _IsSexCheck, value);
                if (IsSexCheck)
                {
                    Addr = 'Y';
                }
                else
                {
                    Addr = 'N';
                }
            }
        }

        private bool _IsSexMinCheck;

        public bool IsSexMinCheck
        {
            get { return _IsSexMinCheck; }
            set
            {
                Set(ref _IsSexMinCheck, value);
                if (IsSexMinCheck)
                {
                    Addr = 'N';
                }
                else
                {
                    Addr = 'Y';
                }
            }
        }

        private bool _IsPhotoCheck;

        public bool IsPhotoCheck
        {
            get { return _IsPhotoCheck; }
            set
            {
                Set(ref _IsPhotoCheck, value);
                if (IsPhotoCheck)
                {
                    Addr = 'Y';
                }
                else
                {
                    Addr = 'N';
                }
            }
        }

        private bool _IsPhotoMinCheck;

        public bool IsPhotoMinCheck
        {
            get { return _IsPhotoMinCheck; }
            set
            {
                Set(ref _IsPhotoMinCheck, value);
                if (IsPhotoMinCheck)
                {
                    Addr = 'N';
                }
                else
                {
                    Addr = 'Y';
                }
            }
        }

        private bool _IsPhoneCheck;

        public bool IsPhoneCheck
        {
            get { return _IsPhoneCheck; }
            set
            {
                Set(ref _IsPhoneCheck, value);
                if (IsPhoneCheck)
                {
                    Addr = 'Y';
                }
                else
                {
                    Addr = 'N';
                }
            }
        }

        private bool _IsPhoneMinCheck;

        public bool IsPhoneMinCheck
        {
            get { return _IsPhoneMinCheck; }
            set
            {
                Set(ref _IsPhoneMinCheck, value);
                if (IsPhoneMinCheck)
                {
                    Addr = 'N';
                }
                else
                {
                    Addr = 'Y';
                }

            }
        }

        private Visibility _IsVisibilityPatientMerge;

        public Visibility IsVisibilityPatientMerge
        {
            get { return _IsVisibilityPatientMerge; }
            set { Set(ref _IsVisibilityPatientMerge, value); }
        }

        #endregion

        #region Command

        private RelayCommand _PatientPrimarySearchCommand;

        public RelayCommand PatientPrimarySearchCommand
        {
            get { return _PatientPrimarySearchCommand ?? (_PatientPrimarySearchCommand = new RelayCommand(PatientPrimarySearch)); }
        }

        private RelayCommand _PatientSecondarySearchCommand;

        public RelayCommand PatientSecondarySearchCommand
        {
            get { return _PatientSecondarySearchCommand ?? (_PatientSecondarySearchCommand = new RelayCommand(PatientSecondarySearch)); }
        }

        private RelayCommand _MergeCommand;

        public RelayCommand MergeCommand
        {
            get { return _MergeCommand ?? (_MergeCommand = new RelayCommand(Merge)); }
        }


        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        private RelayCommand _SwapDataCommand;

        public RelayCommand SwapDataCommand
        {
            get { return _SwapDataCommand ?? (_SwapDataCommand = new RelayCommand(SwapData)); }
        }
        #endregion

        #region Method

        public ManagePatientMergeViewModel()
        {

            IsVisibilityPatientMerge = Visibility.Visible;
            IsAddrCheck = true;
            IsBGroupCheck = true;
            IsSexCheck = true;
            IsPhotoCheck = true;
            IsPhoneCheck = true;
        }

        public override void OnLoaded()
        {
            (this.View as ManagePatientMerge).colSecondarySelect.Visible = false;
        }
        public virtual void Merge()
        {
            try
            {
                if (SelectedPateintPrimarySearch == null)
                {
                    WarningDialog("กรุณาเลือก Primary Patient");
                    return;
                }
                if (SelectedPateintSecondarySearch == null)
                {
                    WarningDialog("กรุณาเลือก Secondary Patient");
                    return;
                }
                long majorPatientUID = SelectedPateintPrimarySearch.PatientUID;
                long minorPatientUID = SelectedPateintSecondarySearch.PatientUID;
                char address = IsAddrCheck ? 'Y' : IsAddrMinCheck ? 'N' : 'N';
                char gender = IsSexCheck ? 'Y' : IsSexMinCheck ? 'N' : 'N';
                char phone = IsPhoneCheck ? 'Y' : IsPhotoMinCheck ? 'N' : 'N';
                char photo = IsPhotoCheck ? 'Y' : IsPhotoMinCheck ? 'N' : 'N';
                char blood = IsBGroupCheck ? 'Y' : IsBGroupMinCheck ? 'N' : 'N';

                DataService.PatientIdentity.MergePatient(majorPatientUID, minorPatientUID, address, gender, phone, photo, blood, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ChangeViewPermission(new ListPatMerge());
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }


        }

        void SwapData()
        {
            SearchPatientPrimaryCriteria = string.Empty;
            SearchPatientSecondaryCriteria = string.Empty;
            var temp1 = SelectedPateintPrimarySearch;
            var temp2 = SelectedPateintSecondarySearch;

            SelectedPateintPrimarySearch = temp2;
            SelectedPateintSecondarySearch = temp1;
        }
        void Cancel()
        {
            ListPatMerge view = new ListPatMerge();
            ChangeViewPermission(view);
        }

        public void PatientPrimarySearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            if (SearchPatientPrimaryCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientPrimaryCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientPrimaryCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientPrimaryCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientPrimaryCriteria;
                        lastName = "empty";
                    }

                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "");
                PatientsPrimarySearchSource = searchResult;
            }
            else
            {
                PatientsPrimarySearchSource = null;
            }

        }

        public void PatientSecondarySearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty;
            string lastName = string.Empty;
            if (SearchPatientSecondaryCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientSecondaryCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientSecondaryCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientSecondaryCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientSecondaryCriteria;
                        lastName = "empty";
                    }

                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "");
                PatientsSecondarySearchSource = searchResult;
            }
            else
            {
                PatientsSecondarySearchSource = null;
            }

        }

        #endregion
    }
}
