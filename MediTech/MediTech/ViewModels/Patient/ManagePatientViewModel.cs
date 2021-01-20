using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MediTech.DataService;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ShareLibrary;
using System.Windows.Media.Imaging;
using System.IO;
using MediTech.Helpers;
using MediTech.Views;
using System.Drawing;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ManagePatientViewModel : MediTechViewModelBase
    {
        #region Properites
        public bool SuppressZipCodeEvent { get; set; }


        private BookingModel _Booking;

        public BookingModel Booking
        {
            get { return _Booking; }
            set
            {
                _Booking = value;
                if (_Booking != null)
                {
                    SelectedCareprovider = CareproviderSource.FirstOrDefault(p => p.CareproviderUID == Booking.CareProviderUID);
                }
            }
        }

        private bool _CheckBuddhist = true;

        public bool CheckBuddhist
        {
            get { return _CheckBuddhist; }
            set
            {
                Set(ref _CheckBuddhist, value);
                if (BirthDate != null)
                {
                    if (_CheckBuddhist == true)
                    {
                        BirthDate = BirthDate.Value.AddYears(543);
                    }
                    else
                    {
                        BirthDate = BirthDate.Value.AddYears(-543);
                    }
                }

            }
        }


        #region PatientAddress
        private List<PostalCode> _ZipCodeSource;

        public List<PostalCode> ZipCodeSource
        {
            get { return _ZipCodeSource; }
            set { Set(ref _ZipCodeSource, value); }
        }

        private PostalCode _SelectedZipCode;

        public PostalCode SelectedZipCode
        {
            get { return _SelectedZipCode; }
            set
            {
                _SelectedZipCode = value;
                if (_SelectedZipCode != null)
                {
                    SelectedProvince = ProvinceSource.FirstOrDefault(p => p.Key == _SelectedZipCode.ProvinceUID);
                    SelectedAmphur = AmphurSource.FirstOrDefault(p => p.Key == _SelectedZipCode.AmphurUID);
                    SelectedDistrict = DistrictSource.FirstOrDefault(p => p.Key == _SelectedZipCode.DistrictUID);
                }
            }
        }


        private string _ZipCode;

        public string ZipCode
        {
            get { return _ZipCode; }
            set
            {
                Set(ref _ZipCode, value);
                if (!SuppressZipCodeEvent)
                {
                    if (!string.IsNullOrEmpty(_ZipCode) && _ZipCode.Length == 5)
                    {

                        ZipCodeSource = DataService.Technical.GetPostalCode(_ZipCode);

                    }
                    else
                    {
                        ZipCodeSource = null;
                    }
                }
                SuppressZipCodeEvent = false;
            }
        }

        public List<LookupItemModel> ProvinceSource { get; set; }
        private LookupItemModel _SelectedProvince;

        public LookupItemModel SelectedProvince
        {
            get { return _SelectedProvince; }
            set
            {
                Set(ref _SelectedProvince, value);
                if (_SelectedProvince != null)
                {
                    AmphurSource = DataService.Technical.GetAmphurByPronvince(_SelectedProvince.Key);
                    DistrictSource = null;
                    ZipCode = string.Empty;
                }
                else
                {
                    AmphurSource = null;
                    DistrictSource = null;
                }
            }
        }


        private List<LookupItemModel> _AmphurSource;

        public List<LookupItemModel> AmphurSource
        {
            get { return _AmphurSource; }
            set { Set(ref _AmphurSource, value); }
        }


        private LookupItemModel _SelectedAmphur;

        public LookupItemModel SelectedAmphur
        {
            get { return _SelectedAmphur; }
            set
            {
                Set(ref _SelectedAmphur, value);
                if (_SelectedAmphur != null)
                {
                    DistrictSource = DataService.Technical.GetDistrictByAmphur(_SelectedAmphur.Key);
                    ZipCode = string.Empty;
                }
                else
                {
                    DistrictSource = null;
                }
            }
        }

        private List<LookupReferenceValueModel> _DistrictSource;

        public List<LookupReferenceValueModel> DistrictSource
        {
            get { return _DistrictSource; }
            set { Set(ref _DistrictSource, value); }
        }
        private LookupReferenceValueModel _SelectedDistrict;

        public LookupReferenceValueModel SelectedDistrict
        {
            get { return _SelectedDistrict; }
            set
            {
                Set(ref _SelectedDistrict, value);
                if (_SelectedDistrict != null)
                {
                    SuppressZipCodeEvent = true;
                    ZipCode = _SelectedDistrict.ValueCode;
                }
                else
                {
                    SuppressZipCodeEvent = true;
                    ZipCode = "";
                }
            }
        }

        private string _Line1;

        public string Line1
        {
            get { return _Line1; }
            set { Set(ref _Line1, value); }
        }

        private string _Line2;

        public string Line2
        {
            get { return _Line2; }
            set { Set(ref _Line2, value); }
        }

        private string _Line3;

        public string Line3
        {
            get { return _Line3; }
            set { Set(ref _Line3, value); }
        }

        #endregion

        #region PatientVisit

        public List<CareproviderModel> CareproviderSource { get; set; }
        private CareproviderModel _SelectedCareprovider;

        public CareproviderModel SelectedCareprovider
        {
            get { return _SelectedCareprovider; }
            set { Set(ref _SelectedCareprovider, value); }
        }


        public List<LookupReferenceValueModel> VisitTypeSource { get; set; }
        private LookupReferenceValueModel _SelectedVisitType;

        public LookupReferenceValueModel SelectedVisitType
        {
            get { return _SelectedVisitType; }
            set
            {
                Set(ref _SelectedVisitType, value);
                if (_SelectedVisitType != null)
                {
                    VisibiltyCheckupCompany = Visibility.Collapsed;
                    if (SelectedVisitType.ValueCode == "MBCHK" || SelectedVisitType.ValueCode == "CHKUP" || SelectedVisitType.ValueCode == "CHKIN")
                    {
                        VisibiltyCheckupCompany = Visibility.Visible;
                    }
                    else
                    {
                        CheckupJobSource = null;
                    }
                }
            }
        }

        public List<LookupReferenceValueModel> PrioritySource { get; set; }
        private LookupReferenceValueModel _SelectedPriority;

        public LookupReferenceValueModel SelectedPriority
        {
            get { return _SelectedPriority; }
            set { _SelectedPriority = value; }
        }

        public List<PayorDetailModel> PayorDetailSource { get; set; }
        private PayorDetailModel _SelectedPayorDetail;

        public PayorDetailModel SelectedPayorDetail
        {
            get { return _SelectedPayorDetail; }
            set
            {
                Set(ref _SelectedPayorDetail, value);
                if (_SelectedPayorDetail != null)
                {
                    PayorAgreementSource = DataService.MasterData.GetAgreementByPayorDetailUID(_SelectedPayorDetail.PayorDetailUID);
                    CheckupJobSource = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectedPayorDetail.PayorDetailUID);
                    if (PayorAgreementSource != null)
                    {
                        SelectedPayorAgreement = PayorAgreementSource.FirstOrDefault();
                    }
                    if (CheckupJobSource != null)
                    {
                        SelectedCheckupJob = CheckupJobSource.OrderByDescending(p => p.StartDttm).FirstOrDefault();
                    }
                }
            }
        }

        private List<PayorAgreementModel> _PayorAgreementSource;

        public List<PayorAgreementModel> PayorAgreementSource
        {
            get { return _PayorAgreementSource; }
            set { Set(ref _PayorAgreementSource, value); }
        }

        private PayorAgreementModel _SelectedPayorAgreement;

        public PayorAgreementModel SelectedPayorAgreement
        {
            get { return _SelectedPayorAgreement; }
            set { Set(ref _SelectedPayorAgreement, value); }
        }

        private DateTime _StartDate;

        public DateTime StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
        }

        private DateTime _StartTime;
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
        }

        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
        }

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (SelectOrganisation != null)
                {
                    if (SelectOrganisation.HealthOrganisationUID == 5)
                    {
                        SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.ValueCode == "MBCHK");
                    }
                    //else
                    //{
                    //    SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.ValueCode == "DCPAT");
                    //}
                }
            }
        }


        #endregion

        #region Patient

        public List<LookupReferenceValueModel> TitleSource { get; set; }
        private LookupReferenceValueModel _SelectedTitle;

        public LookupReferenceValueModel SelectedTitle
        {
            get { return _SelectedTitle; }
            set
            {
                Set(ref _SelectedTitle, value);
                if (_SelectedTitle != null)
                {
                    if (GenderSource != null && referenceRealationShipTitle != null)
                    {
                        int? targetUID = referenceRealationShipTitle.FirstOrDefault(p => p.SourceReferenceValueUID == _SelectedTitle.Key)?.TargetReferenceValueUID;
                        SelectedGender = GenderSource.FirstOrDefault(p => p.Key == targetUID);
                    }
                }
            }
        }

        public List<LookupReferenceValueModel> GenderSource { get; set; }
        private LookupReferenceValueModel _SelectedGender;

        public LookupReferenceValueModel SelectedGender
        {
            get { return _SelectedGender; }
            set { Set(ref _SelectedGender, value); }
        }

        public List<LookupReferenceValueModel> BloodGroupSource { get; set; }
        private LookupReferenceValueModel _SelectedBloodGroup;

        public LookupReferenceValueModel SelectedBloodGroup
        {
            get { return _SelectedBloodGroup; }
            set { Set(ref _SelectedBloodGroup, value); }
        }

        public List<LookupReferenceValueModel> NationalSource { get; set; }
        private LookupReferenceValueModel _SelectedNational;

        public LookupReferenceValueModel SelectedNational
        {
            get { return _SelectedNational; }
            set { Set(ref _SelectedNational, value); }
        }

        public List<LookupReferenceValueModel> PreferredLanguageSource { get; set; }
        private LookupReferenceValueModel _SelectedPreferredLanguage;

        public LookupReferenceValueModel SelectedPreferredLanguage
        {
            get { return _SelectedPreferredLanguage; }
            set { Set(ref _SelectedPreferredLanguage, value); }
        }

        public List<LookupReferenceValueModel> RegionSource { get; set; }
        private LookupReferenceValueModel _SelectedRegion;

        public LookupReferenceValueModel SelectedRegion
        {
            get { return _SelectedRegion; }
            set { Set(ref _SelectedRegion, value); }
        }

        public List<LookupReferenceValueModel> MarryStatusSource { get; set; }
        private LookupReferenceValueModel _SelectedMarryStatus;

        public LookupReferenceValueModel SelectedMarryStatus
        {
            get { return _SelectedMarryStatus; }
            set { Set(ref _SelectedMarryStatus, value); }
        }

        public List<LookupReferenceValueModel> OccupationSource { get; set; }
        private LookupReferenceValueModel _SelectedOccupation;

        public LookupReferenceValueModel SelectedOccupation
        {
            get { return _SelectedOccupation; }
            set { Set(ref _SelectedOccupation, value); }
        }


        public List<LookupReferenceValueModel> VIPTypeSources { get; set; }
        private LookupReferenceValueModel _SelectVIPType;

        public LookupReferenceValueModel SelectVIPType
        {
            get { return _SelectVIPType; }
            set { Set(ref _SelectVIPType, value); }
        }

        private bool _IsVIP;

        public bool IsVIP
        {
            get { return _IsVIP; }
            set
            {
                Set(ref _IsVIP, value);
                if (IsVIP)
                {
                    IsEnableVIP = true;
                }
                else
                {
                    IsEnableVIP = false;
                    SelectVIPType = null;
                    VIPActiveFrom = null;
                    VIPActiveTo = null;
                }
            }
        }

        private bool _IsEnableVIP;

        public bool IsEnableVIP
        {
            get { return _IsEnableVIP; }
            set { Set(ref _IsEnableVIP, value); }
        }


        private DateTime? _VIPActiveFrom;

        public DateTime? VIPActiveFrom
        {
            get { return _VIPActiveFrom; }
            set { Set(ref _VIPActiveFrom, value); }
        }

        private DateTime? _VIPActiveTo;

        public DateTime? VIPActiveTo
        {
            get { return _VIPActiveTo; }
            set { Set(ref _VIPActiveTo, value); }
        }

        private string _PatientID;

        public string PatientID
        {
            get { return _PatientID; }
            set { Set(ref _PatientID, value); }
        }

        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set { Set(ref _FirstName, value); }
        }

        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set { Set(ref _LastName, value); }
        }

        private string _NickName;

        public string NickName
        {
            get { return _NickName; }
            set { Set(ref _NickName, value); }
        }

        private DateTime? _BirthDate;

        public DateTime? BirthDate
        {
            get { return _BirthDate; }
            set
            {
                Set(ref _BirthDate, value);
                Age = string.Empty;
                CalculateBrithDate = false;

            }
        }


        private bool _CalculateBrithDate;

        public bool CalculateBrithDate
        {
            get { return _CalculateBrithDate; }
            set { Set(ref _CalculateBrithDate, value); }
        }

        private string _Age;

        public string Age
        {
            get { return _Age; }
            set
            {
                if (value != null)
                {
                    string age = value.ToString().Trim();
                    if (!string.IsNullOrEmpty(age) && (CheckValidate.IsNumber(age)))
                    {
                        if (CheckBuddhist)
                        {
                            BirthDate = DateTime.Parse("01/01/" + Convert.ToString(int.Parse(DateTime.Now.ToString("yyyy")) + 543 - int.Parse(age)));
                        }
                        else
                        {
                            BirthDate = DateTime.Parse("01/01/" + Convert.ToString(int.Parse(DateTime.Now.ToString("yyyy")) - int.Parse(age)));
                        }
                        CalculateBrithDate = true;
                    }
                    Set(ref _Age, value);
                }

            }
        }

        private string _OtherID;

        public string OtherID
        {
            get { return _OtherID; }
            set { Set(ref _OtherID, value); }
        }


        private string _NatinonalID;

        public string NatinonalID
        {
            get { return _NatinonalID; }
            set { Set(ref _NatinonalID, value); }
        }

        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { Set(ref _Email, value); }
        }

        private string _IDLine;

        public string IDLine
        {
            get { return _IDLine; }
            set { Set(ref _IDLine, value); }
        }

        private string _SecondPhone;

        public string SeconePhone
        {
            get { return _SecondPhone; }
            set { Set(ref _SecondPhone, value); }
        }

        private string _MobilePhone;

        public string MobilePhone
        {
            get { return _MobilePhone; }
            set { Set(ref _MobilePhone, value); }
        }

        private string _CommentDoctor;

        public string CommentDoctor
        {
            get { return _CommentDoctor; }
            set { Set(ref _CommentDoctor, value); }
        }



        private BitmapImage _PatientImage;
        public BitmapImage PatientImage
        {
            get
            {
                return _PatientImage;
            }
            set
            {
                Set(ref _PatientImage, value);
            }
        }

        #endregion

        #region PatientSearch

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
                    AssingModel(SelectedPateintSearch);
                }
            }
        }

        #endregion

        #region PatientAllergy

        private List<PatientAllergyModel> _PatientAllergyList;

        public List<PatientAllergyModel> PatientAllergyList
        {
            get { return _PatientAllergyList; }
            set { Set(ref _PatientAllergyList, value); }
        }

        #endregion

        #endregion

        #region Varible

        List<ReferenceRelationShipModel> referenceRealationShipTitle;
        public PatientInformationModel patientModel;

        #endregion

        private Visibility _VisibiltyCheckupCompany = Visibility.Collapsed;

        public Visibility VisibiltyCheckupCompany
        {
            get { return _VisibiltyCheckupCompany; }
            set { Set(ref _VisibiltyCheckupCompany, value); }
        }

        private List<CheckupJobContactModel> _CheckupJobSource;

        public List<CheckupJobContactModel> CheckupJobSource
        {
            get { return _CheckupJobSource; }
            set { Set(ref _CheckupJobSource, value); }
        }

        private CheckupJobContactModel _SelectedCheckupJob;

        public CheckupJobContactModel SelectedCheckupJob
        {
            get { return _SelectedCheckupJob; }
            set { Set(ref _SelectedCheckupJob, value); }
        }

        #region Command



        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SavePatientData)); }
        }


        private RelayCommand _RegisterCommand;

        public RelayCommand RegisterCommand
        {
            get { return _RegisterCommand ?? (_RegisterCommand = new RelayCommand(RegisterPatient)); }
        }

        private RelayCommand _RegisterToDoctorCommand;

        public RelayCommand RegisterToDoctorCommand
        {
            get { return _RegisterToDoctorCommand ?? (_RegisterToDoctorCommand = new RelayCommand(RegisterToDoctor)); }
        }



        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }



        #endregion

        #region Method
        public ManagePatientViewModel()
        {
            DateTime now = DateTime.Now;

            List<LookupReferenceValueModel> dataLookupSource = DataService.Technical.GetReferenceValueList("SEXXX,TITLE,BLOOD,MARRY,RELGN,NATNL,VISTY,RQPRT,OCCUP,SPOKL,VIPTP");
            GenderSource = dataLookupSource.Where(p => p.DomainCode == "SEXXX").ToList();
            TitleSource = dataLookupSource.Where(p => p.DomainCode == "TITLE").ToList();
            BloodGroupSource = dataLookupSource.Where(p => p.DomainCode == "BLOOD").ToList();
            MarryStatusSource = dataLookupSource.Where(p => p.DomainCode == "MARRY").ToList();
            OccupationSource = dataLookupSource.Where(p => p.DomainCode == "OCCUP").ToList();
            RegionSource = dataLookupSource.Where(p => p.DomainCode == "RELGN").OrderBy(p => p.DisplayOrder).ToList();
            NationalSource = dataLookupSource.Where(p => p.DomainCode == "NATNL").OrderBy(p => p.DisplayOrder).ToList();
            PreferredLanguageSource = dataLookupSource.Where(p => p.DomainCode == "SPOKL").OrderBy(p => p.DisplayOrder).ToList();
            VisitTypeSource = dataLookupSource.Where(p => p.DomainCode == "VISTY").OrderBy(p => p.DisplayOrder).ToList();
            PrioritySource = dataLookupSource.Where(P => P.DomainCode == "RQPRT").OrderBy(p => p.DisplayOrder).ToList();
            VIPTypeSources = dataLookupSource.Where(P => P.DomainCode == "VIPTP").OrderBy(p => p.DisplayOrder).ToList();
            PayorDetailSource = DataService.MasterData.GetPayorDetail();
            referenceRealationShipTitle = DataService.Technical.GetReferenceRealationShip("TITLE", "SEXXX");
            Organisations = GetHealthOrganisationRoleMedical();

            ProvinceSource = DataService.Technical.GetProvince();

            CareproviderSource = DataService.UserManage.GetCareproviderDoctor();
            //SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.Key == 430);

            if (Organisations != null)
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            SelectedPayorDetail = PayorDetailSource.FirstOrDefault(p => p.PayorDetailUID == 1);
            SelectedPriority = PrioritySource.FirstOrDefault(p => p.Key == 440);

            StartDate = now.Date;
            StartTime = now;

        }

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty;
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null);
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        public void SavePatientData()
        {
            try
            {
                if (String.IsNullOrEmpty(PatientID))
                {
                    WarningDialog("กรุณาเลือกผู้ป่วย");
                    return;
                }

                if (ValidatePatientData())
                {
                    return;
                }

                PatientInformationModel patientInfo = AssingPropertiesToModel();
                PatientInformationModel resultPatient = DataService.PatientIdentity.RegisterPatient(patientInfo, AppUtil.Current.UserID, AppUtil.Current.OwnerOrganisationUID);

                if (resultPatient == null)
                {
                    ErrorDialog("ไม่สามารถบันทึกข้อมูลคนไข้ได้ ติดต่อ Admin");
                    return;
                }

                SaveSuccessDialog();

                ClearPropertiesControl();

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void RegisterPatient()
        {
            try
            {
                CreateVisit("register");

            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }

        private void RegisterToDoctor()
        {
            try
            {
                if (SelectedCareprovider == null)
                {
                    WarningDialog("กรุณาเลือก แพทย์");
                    return;
                }
                CreateVisit("sendtodoctor");

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void CreateVisit(string visitStatus)
        {
            try
            {
                if (ValidatePatientData())
                {
                    return;
                }

                if (ValidateVisitData())
                {
                    return;
                }


                #region CheckVisitDuplicate
                if (patientModel != null)
                {
                    if (patientModel.PatientUID != 0)
                    {
                        List<PatientVisitModel> visitData = DataService.PatientIdentity.GetPatientVisitByPatientUID(patientModel.PatientUID);
                        if (visitData != null && visitData.Count > 0)
                        {
                            var notCloseVisit = visitData.FirstOrDefault(p => p.VisitType != "Mobile X-ray" && p.EndDttm == null);
                            if (notCloseVisit != null)
                            {
                                MessageBoxResult dialogResult = System.Windows.MessageBox.Show("ผู้ป่วยมีการลงทะเบียนที่ยังไม่ปิดไว้วันที่ " + notCloseVisit.StartDttm.Value.ToString("dd/MM/yyyy") + "\r\n คุณต้องการลงทะเบียนต่อหรือไม่ ? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                                if (dialogResult == MessageBoxResult.No)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }

                #endregion

                #region CheckPatientDuplicate

                if (patientModel == null)
                {
                    DateTime? birthDttm;
                    CheckBirthDttm(out birthDttm);
                    if (birthDttm == null)
                    {
                        birthDttm = BirthDate != null ? BirthDate.Value : (DateTime?)null;
                    }

                    PatientInformationModel patAlready = DataService.PatientIdentity.CheckDupicatePatient(FirstName, LastName, birthDttm.Value, SelectedGender.Key);
                    if (patAlready != null)
                    {
                        MessageBoxResult dialogResult = System.Windows.MessageBox.Show("มีผู้ป่วยนี้ มี ชื่อ นามสกุล เพศ วันเดือนปีเกิด ซ้ำในะระบบ" + " \r\n คุณต้องการลงทะเบียนต่อหรือไม่ ? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dialogResult == MessageBoxResult.No)
                        {
                            return;
                        }
                    }
                }

                #endregion

                PatientInformationModel patientInfo = AssingPropertiesToModel();
                PatientInformationModel resultPatient = DataService.PatientIdentity.RegisterPatient(patientInfo, AppUtil.Current.UserID, AppUtil.Current.OwnerOrganisationUID);

                if (resultPatient == null)
                {
                    ErrorDialog("ไม่สามารถบันทึกข้อมูลคนไข้ได้ ติดต่อ Admin");
                    return;
                }

                PatientVisitModel visitInfo = new PatientVisitModel();
                visitInfo.StartDttm = DateTime.Parse(StartDate.ToString("dd/MM/yyyy") + " " + StartTime.ToString("HH:mm"));
                visitInfo.PatientUID = resultPatient.PatientUID;
                visitInfo.VISTYUID = SelectedVisitType.Key;

                if (visitStatus == "register")
                {
                    visitInfo.VISTSUID = 417;   //Registered Status
                }
                else
                {
                    visitInfo.VISTSUID = 419;   //Registered Status
                }

                if (Booking != null)
                    visitInfo.BookingUID = Booking.BookingUID; //Appointment

                visitInfo.PRITYUID = SelectedPriority.Key;
                visitInfo.PayorDetailUID = SelectedPayorDetail.PayorDetailUID;
                visitInfo.PayorAgreementUID = SelectedPayorAgreement.PayorAgreementUID;
                visitInfo.Comments = CommentDoctor;
                visitInfo.OwnerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                visitInfo.CheckupJobUID = SelectedCheckupJob != null ? SelectedCheckupJob.CheckupJobContactUID : (int?)null;
                if (SelectedCareprovider != null)
                    visitInfo.CareProviderUID = SelectedCareprovider.CareproviderUID;

                PatientVisitModel returnData = DataService.PatientIdentity.SavePatientVisit(visitInfo, AppUtil.Current.UserID);
                if (string.IsNullOrEmpty(returnData.VisitID))
                {
                    ErrorDialog("ไม่สามารถบันทึกข้อมูล Visit คนไข้ได้ ติดต่อ Admin");
                    return;
                }
                else
                {
                    if (Booking != null)
                    {
                        DataService.PatientIdentity.UpdateBookingArrive(Booking.BookingUID, AppUtil.Current.UserID);
                    }
                }

                SaveSuccessDialog("BN : " + resultPatient.PatientID);

                if (visitStatus == "register")
                {
                    PatientList patientList = new PatientList();
                    ChangeViewPermission(patientList);   //Registered Status
                }
                else
                {
                    if (AppUtil.Current.IsDoctor ?? false)
                    {
                        DoctorRoom doctorRoom = new DoctorRoom();
                        ChangeViewPermission(doctorRoom);   //Registered Status
                    }
                    else
                    {
                        PatientList patientList = new PatientList();
                        ChangeViewPermission(patientList);   //Registered Status
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public bool ValidatePatientData()
        {

            if (SelectedTitle == null)
            {
                WarningDialog("กรุณาใส่ คำนำหน้า");
                return true;
            }

            if (SelectedGender == null)
            {
                WarningDialog("กรุณาใส่ เพศ");
                return true;
            }

            if (String.IsNullOrEmpty(FirstName))
            {
                WarningDialog("กรุณาใส่ ชื่อ");
                return true;
            }

            if (String.IsNullOrEmpty(LastName))
            {
                WarningDialog("กรุณาใส่ นามสกุล");
                return true;
            }

            if (IsVIP)
            {
                if (SelectVIPType == null)
                {
                    WarningDialog("กรุณาใส่ VIP Type");
                    return true;
                }

                if (VIPActiveTo == null)
                {
                    WarningDialog("กรุณาใส่วัน สิ้นสุด VIP");
                    return true;
                }
            }

            if (this.View != null && this.View is ManagePatient)
            {
                if ((this.View as ManagePatient).lytBithDate.Visibility != Visibility.Collapsed)
                {
                    if (BirthDate == null)
                    {
                        WarningDialog("กรุณาใส่ วัน เดือน ปี เกิด");
                        return true;
                    }
                }
            }


            return false;
        }

        public bool ValidateVisitData()
        {
            if (SelectOrganisation == null)
            {
                WarningDialog("กรุณาเลือก สถานที่");
                return true;
            }
            if (SelectedVisitType == null)
            {
                WarningDialog("กรุณาเลือก ประเภท Visit");
                return true;
            }

            if (SelectedPayorDetail == null)
            {
                WarningDialog("กรุณาเลือก Payor");
                return true;
            }

            if (SelectedPayorAgreement == null)
            {
                WarningDialog("กรุณาเลือก Agreemnet");
                return true;
            }

            if (VisibiltyCheckupCompany == Visibility.Visible)
            {
                if (SelectedCheckupJob == null)
                {
                    WarningDialog("กรุณาเลือก Checkup Job");
                    return true;
                }
            }

            if (SelectedPriority == null)
            {
                WarningDialog("กรุณาเลือก ความสำคัญ");
                return true;
            }

            return false;
        }
        public void ClearPropertiesControl()
        {
            patientModel = null;
            SearchPatientCriteria = string.Empty;

            PatientID = string.Empty;
            SelectedTitle = null;
            SelectedGender = null;
            FirstName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            BirthDate = null;
            OtherID = string.Empty;
            CalculateBrithDate = false;
            NatinonalID = string.Empty;
            SelectedBloodGroup = null;
            SelectedNational = null;
            SelectedPreferredLanguage = null;
            SelectedMarryStatus = null;
            SelectedRegion = null;
            Line1 = string.Empty;
            Line2 = string.Empty;
            Line3 = string.Empty;
            SelectedProvince = null;
            SelectedAmphur = null;
            SelectedDistrict = null;

            SuppressZipCodeEvent = true;
            ZipCode = string.Empty;

            Email = string.Empty;
            IDLine = string.Empty;
            SeconePhone = string.Empty;
            MobilePhone = string.Empty;
            PatientImage = null;


            PatientAllergyList = null;
            //SelectedPayorDetail = PayorDetailSource.FirstOrDefault(p => p.PayorDetailUID == 1);
            //SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.Key == 430);
            //SelectedPriority = PrioritySource.FirstOrDefault(p => p.Key == 440);
            SelectedCareprovider = null;
            SelectedOccupation = null;
            CommentDoctor = string.Empty;
            IsVIP = false;

        }

        public void AssingModel(PatientInformationModel patientData)
        {
            patientModel = patientData;
            AssingModelToProperties();
        }
        public void AssingModelToProperties()
        {
            PatientID = patientModel.PatientID;
            SelectedTitle = TitleSource.FirstOrDefault(p => p.Key == patientModel.TITLEUID);
            SelectedGender = GenderSource.FirstOrDefault(p => p.Key == patientModel.SEXXXUID);
            FirstName = patientModel.FirstName;
            LastName = patientModel.LastName;
            NickName = patientModel.NickName;

            BirthDate = patientModel.BirthDttm;
            if (BirthDate != null)
            {
                if (CheckBuddhist == true)
                {
                    BirthDate = BirthDate.Value.AddYears(543);
                }
                else
                {
                    BirthDate = BirthDate.Value.AddYears(-543);
                }
            }

            OtherID = patientModel.EmployeeID;
            CalculateBrithDate = patientModel.DOBComputed;
            NatinonalID = patientModel.NationalID;
            SelectedBloodGroup = BloodGroupSource.FirstOrDefault(p => p.Key == patientModel.BLOODUID);
            SelectedNational = NationalSource.FirstOrDefault(p => p.Key == patientModel.NATNLUID);
            SelectedPreferredLanguage = PreferredLanguageSource.FirstOrDefault(p => p.Key == patientModel.SPOKLUID);
            SelectedMarryStatus = MarryStatusSource.FirstOrDefault(p => p.Key == patientModel.MARRYUID);
            SelectedRegion = RegionSource.FirstOrDefault(p => p.Key == patientModel.RELGNUID);
            SelectedOccupation = OccupationSource.FirstOrDefault(p => p.Key == patientModel.OCCUPUID);


            Line1 = patientModel.Line1;
            Line2 = patientModel.Line2;
            Line3 = patientModel.Line3;

            if (ProvinceSource != null)
                SelectedProvince = ProvinceSource.FirstOrDefault(p => p.Key == patientModel.ProvinceUID);

            if (AmphurSource != null)
                SelectedAmphur = AmphurSource.FirstOrDefault(p => p.Key == patientModel.AmphurUID);

            if (DistrictSource != null)
                SelectedDistrict = DistrictSource.FirstOrDefault(p => p.Key == patientModel.DistrictUID);

            SuppressZipCodeEvent = true;
            ZipCode = patientModel.ZipCode;

            Email = patientModel.Email;
            IDLine = patientModel.IDLine;
            SeconePhone = patientModel.SecondPhone;
            MobilePhone = patientModel.MobilePhone;
            if (patientModel.PatientImage != null)
            {
                if (patientModel.PatientImage.Length > 0)
                {
                    PatientImage = ImageHelpers.ConvertByteToBitmap(patientModel.PatientImage);
                }

            }
            else
            {
                PatientImage = null;
            }

            if (patientModel.PatientUID != 0)
            {
                PatientAllergyList = DataService.PatientIdentity.GetPatientAllergyByPatientUID(patientModel.PatientUID);
            }

            IsVIP = patientModel.IsVIP;
            if (IsVIP)
            {
                SelectVIPType = VIPTypeSources.FirstOrDefault(p => p.Key == patientModel.VIPTPUID);
                VIPActiveFrom = patientModel.VIPActiveFrom;
                VIPActiveTo = patientModel.VIPActiveTo;

                if (patientModel.VIPActiveTo?.Date < DateTime.Now.Date)
                {
                    MessageBoxResult resultDiag = QuestionDialog("สถานะ VIP ผู้ป่วยหมดอายุแล้ว จะต่อวันหมดอายุหรือไม่");
                    if (resultDiag == MessageBoxResult.Yes)
                    {
                        VIPActiveTo = null;
                    }
                    else if (resultDiag == MessageBoxResult.No)
                    {
                        IsVIP = false;
                    }
                }
            }

        }
        public PatientInformationModel AssingPropertiesToModel()
        {
            if (patientModel == null || String.IsNullOrEmpty(patientModel.PatientID))
            {
                patientModel = new PatientInformationModel();
            }
            patientModel.PatientID = PatientID;
            patientModel.FirstName = FirstName;
            patientModel.LastName = LastName;
            patientModel.NickName = NickName;

            if (SelectedGender != null)
                patientModel.SEXXXUID = SelectedGender.Key;

            if (SelectedTitle != null)
                patientModel.TITLEUID = SelectedTitle.Key;
            patientModel.PatientID = PatientID;

            DateTime? birthDttm;
            CheckBirthDttm(out birthDttm);

            if (birthDttm != null)
            {
                patientModel.BirthDttm = birthDttm;
            }
            else
            {
                patientModel.BirthDttm = BirthDate != null ? BirthDate.Value : (DateTime?)null;
            }


            patientModel.DOBComputed = CalculateBrithDate;

            patientModel.NationalID = NatinonalID;
            patientModel.EmployeeID = OtherID;

            if (SelectedBloodGroup != null)
                patientModel.BLOODUID = SelectedBloodGroup.Key;

            if (SelectedNational != null)
                patientModel.NATNLUID = SelectedNational.Key;

            if (SelectedPreferredLanguage != null)
                patientModel.SPOKLUID = SelectedPreferredLanguage.Key;

            if (SelectedMarryStatus != null)
                patientModel.MARRYUID = SelectedMarryStatus.Key;

            if (SelectedRegion != null)
                patientModel.RELGNUID = SelectedRegion.Key;

            if (SelectedOccupation != null)
                patientModel.OCCUPUID = SelectedOccupation.Key;



            patientModel.SecondPhone = SeconePhone;
            patientModel.MobilePhone = MobilePhone;
            patientModel.Email = Email;
            patientModel.IDLine = IDLine;

            patientModel.Line1 = Line1;
            patientModel.Line2 = Line2;
            patientModel.Line3 = Line3;

            if (SelectedProvince != null)
                patientModel.ProvinceUID = SelectedProvince.Key;

            if (SelectedAmphur != null)
                patientModel.AmphurUID = SelectedAmphur.Key;

            if (SelectedDistrict != null)
                patientModel.DistrictUID = SelectedDistrict.Key;

            patientModel.ZipCode = ZipCode;

            if (PatientImage != null)
            {
                byte[] patImage = ImageHelpers.ResizeImage(ImageHelpers.ConvertBitmapToByte(PatientImage), 800, 600, true);
                patientModel.PatientImage = patImage;
            }

            patientModel.IsVIP = IsVIP;
            if (IsVIP)
            {
                patientModel.VIPActiveFrom = VIPActiveFrom;
                patientModel.VIPActiveTo = VIPActiveTo;
                if (SelectVIPType != null)
                {
                    patientModel.VIPTPUID = SelectVIPType.Key;
                }
            }



            return patientModel;
        }

        public void CheckBirthDttm(out DateTime? birthDttm)
        {
            birthDttm = null;
            if (BirthDate != null)
            {
                if (CheckBuddhist)
                {
                    int year = BirthDate.Value.Year;
                    if (year > 2900)
                    {
                        birthDttm = BirthDate.Value.AddYears(-543);
                    }
                    else if (year > 2400)
                    {
                        birthDttm = BirthDate.Value.AddYears(-543);
                    }

                }
                else
                {
                    int year = BirthDate.Value.Year;
                    if (year > 2400)
                    {
                        birthDttm = BirthDate.Value.AddYears(-543);
                    }
                }
            }

        }
        public void AssingDataFromSkip(PatientInformationModel patientData)
        {
            FirstName = patientData.FirstName;
            LastName = patientData.LastName;
            NickName = patientData.NickName;
            NatinonalID = patientData.NationalID;
            SelectedGender = GenderSource.FirstOrDefault(p => p.Key == patientData.SEXXXUID);
        }

        #endregion


    }
}
