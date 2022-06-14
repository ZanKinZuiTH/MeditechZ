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
using System.Collections.ObjectModel;

namespace MediTech.ViewModels
{
    public class ManagePatientViewModel : MediTechViewModelBase
    {
        #region Properites
        public bool SuppressZipCodeEvent { get; set; }

        private int _SelectTabIndex;
        public int SelectTabIndex
        {
            get { return _SelectTabIndex; }
            set
            {
                Set(ref _SelectTabIndex, value);
                if (SelectTabIndex == 1)
                {
                    var now = DateTime.Now;
                    PayorDetailActiveFrom = now;
                    var data = DataService.Billing.GetInsuranceCompanies();
                    InsuranceNameSource = data;
                    PayorType = DataService.Technical.GetReferenceValueMany("PAYRTP");
                    //InsuranceNameSource = DataService.Billing.GetInsurancePlansGroupPayorCompany();

                    //PayorDetailSource = DataService.Billing.GetPayorDetail();
                    //PolicySource = DataService.Billing.GetPolicyMasterAll();

                    if (patientModel.PatientUID != 0)
                    {
                        var PatientInsurance = DataService.PatientIdentity.GetPatientInsuranceDetail(patientModel.PatientUID);
                        PatientInsuranceDetail = new ObservableCollection<PatientInsuranceDetailModel>(PatientInsurance);
                    }
                    if (SelectTabIndex == 2)
                    {
                        if (patientModel.PatientUID != 0)
                        {
                            PatientModificateLog = DataService.PatientIdentity.GetPatientDemographicLogByUID(patientModel.PatientUID);
                        }
                    }
                }
            }
        }

        private BookingModel _Booking;
        public BookingModel Booking
        {
            get { return _Booking; }
            set
            {
                _Booking = value;
                if (_Booking != null)
                {
                    //SelectedCareprovider = CareproviderSource.FirstOrDefault(p => p.CareproviderUID == Booking.CareProviderUID);
                }
            }
        }

        private bool _UseReadCard = false;
        public bool UseReadCard
        {
            get { return _UseReadCard; }
            set { _UseReadCard = value; }
        }

        private string _PassportNo;
        public string PassportNo
        {
            get { return _PassportNo; }
            set { Set(ref _PassportNo, value); }
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

        #region PayorDetail
        private ObservableCollection<PatientInsuranceDetailModel> _PatientInsuranceDetail;
        public ObservableCollection<PatientInsuranceDetailModel> PatientInsuranceDetail
        {
            get { return _PatientInsuranceDetail; }
            set { Set(ref _PatientInsuranceDetail, value); }
        }

        private PatientInsuranceDetailModel _SelectPatientInsuranceDetail;
        public PatientInsuranceDetailModel SelectPatientInsuranceDetail
        {
            get { return _SelectPatientInsuranceDetail; }
            set { Set(ref _SelectPatientInsuranceDetail, value); 
            if (SelectPatientInsuranceDetail != null)
                {
                    AssignSelectToPropotiesPayor();
                }
            }
        }

        private List<InsuranceCompanyModel> _InsuranceNameSource;
        public List<InsuranceCompanyModel> InsuranceNameSource
        {
            get { return _InsuranceNameSource; }
            set { Set(ref _InsuranceNameSource, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceName;
        public InsuranceCompanyModel SelectInsuranceName
        {
            get { return _SelectInsuranceName; }
            set { Set(ref _SelectInsuranceName, value); 
            if (SelectInsuranceName != null)
                {
                    //InsuranceCompanySource = DataService.Billing.SearchInsurancePlaneByINCO(SelectInsuranceName.InsuranceCompanyUID);

                    var insurancePlan = DataService.Billing.GetInsurancePlans(SelectInsuranceName.InsuranceCompanyUID);
                    InsuranceCompanySource = insurancePlan;
                    //AgreementSource = InsuranceCompanySource.Where(p => p.InsuranceCompanyUID == SelectInsuranceName.InsuranceCompanyUID ).ToList();
                    AgreementSource = insurancePlan.Where(p => p.StatusFlag == "A" && p.InsuranceCompanyUID == SelectInsuranceName.InsuranceCompanyUID).ToList();
                    
                    if(AgreementSource.Count > 0)
                    {
                        SelectAgreement = AgreementSource.First();
                    }
                }
            }
        }
        private List<InsurancePlanModel> _InsuranceCompanySource;
        public List<InsurancePlanModel> InsuranceCompanySource
        {
            get { return _InsuranceCompanySource; }
            set { Set(ref _InsuranceCompanySource, value); }
        }

        private List<InsurancePlanModel> _PayorDetailSource;
        public List<InsurancePlanModel> PayorDetailSource
        {
            get { return _PayorDetailSource; }
            set { Set(ref _PayorDetailSource, value); }
        }

        private InsurancePlanModel _SelectPayorDetail;
        public InsurancePlanModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set { Set(ref _SelectPayorDetail, value); 
                if(SelectPayorDetail != null)
                {
                    //PolicySource = InsuranceCompanySource.Where(p => p.PolicyMasterUID == SelectPayorDetail.PolicyMasterUID && p.StatusFlag == "A").ToList();
                    //SelectPolicy = PolicySource.First();

                    //AgreementSource = DataService.Billing.GetAgreementByInsuranceUID(SelectPayorDetail.InsuranceCompanyUID ?? 0);
                    //SelectAgreement = AgreementSource.FirstOrDefault();
                }
            }
        }

        private List<InsurancePlanModel> _AgreementSource;
        public List<InsurancePlanModel> AgreementSource
        {
            get { return _AgreementSource; }
            set { Set(ref _AgreementSource, value); }
        }

        private InsurancePlanModel _SelectAgreement;
        public InsurancePlanModel SelectAgreement
        {
            get { return _SelectAgreement; }
            set
            {
                Set(ref _SelectAgreement, value);
                if (SelectAgreement != null)
                {
                    PayorDetailSource = InsuranceCompanySource.Where(p => p.PayorAgreementUID == SelectAgreement.PayorAgreementUID && p.StatusFlag == "A").ToList();
                    SelectPayorDetail = PayorDetailSource != null ? PayorDetailSource.First() : null;

                    if(SelectAgreement.PolicyMasterUID != 0)
                    {
                        PolicySource = InsuranceCompanySource.Where(p => p.PolicyMasterUID == SelectAgreement.PolicyMasterUID && p.StatusFlag == "A").ToList();
                        SelectPolicy = PolicySource != null ? PolicySource.First() : null;
                    }

                    FixedCopayAmount = SelectAgreement.FixedCopayAmount;
                    ClaimPercentage = SelectAgreement.ClaimPercentage;
                }
                else
                {
                    PayorDetailSource = null;
                    SelectPayorDetail = null;
                }
            }
        }

        private List<InsurancePlanModel> _PolicySource;
        public List<InsurancePlanModel> PolicySource
        {
            get { return _PolicySource; }
            set { Set(ref _PolicySource, value); }
        }

        private InsurancePlanModel _SelectPolicy;
        public InsurancePlanModel SelectPolicy
        {
            get { return _SelectPolicy; }
            set { Set(ref _SelectPolicy, value); }
        }

        private DateTime? _PayorDetailActiveFrom;
        public DateTime? PayorDetailActiveFrom
        {
            get { return _PayorDetailActiveFrom; }
            set { Set(ref _PayorDetailActiveFrom, value); }
        }

        private DateTime? _PayorDetailActiveTo;
        public DateTime? PayorDetailActiveTo
        {
            get { return _PayorDetailActiveTo; }
            set { Set(ref _PayorDetailActiveTo, value); }
        }
       
        private List<LookupReferenceValueModel> _PayorType;
        public List<LookupReferenceValueModel> PayorType
        {
            get { return _PayorType; }
            set { Set(ref _PayorType, value); }
        }

        private LookupReferenceValueModel _SelectPayorType;
        public LookupReferenceValueModel SelectPayorType
        {
            get { return _SelectPayorType; }
            set { Set(ref _SelectPayorType, value); }
        }

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { Set(ref _Comment, value); }
        }

        private double? _EligibleAmount;
        public double? EligibleAmount
        {
            get { return _EligibleAmount; }
            set { Set(ref _EligibleAmount, value); }
        }

        private double? _ClaimPercentage;
        public double? ClaimPercentage
        {
            get { return _ClaimPercentage; }
            set { Set(ref _ClaimPercentage, value); }
        }

        private double? _FixedCopayAmount;
        public double? FixedCopayAmount
        {
            get { return _FixedCopayAmount; }
            set { Set(ref _FixedCopayAmount, value); }
        }

        #endregion

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
                    AmphurSource = DataService.Technical.GetAmphurByPronvince(_SelectedProvince.Key.Value);
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
                    DistrictSource = DataService.Technical.GetDistrictByAmphur(_SelectedAmphur.Key.Value);
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

        #region Patient Modificate log

        private List<PatientDemographicLogModel> _PatientModificateLog;
        public List<PatientDemographicLogModel> PatientModificateLog
        {
            get { return _PatientModificateLog; }
            set { Set(ref _PatientModificateLog, value); }
        }
        
        #endregion

        #endregion

        #region Varible

        List<ReferenceRelationShipModel> referenceRealationShipTitle;
        public PatientInformationModel patientModel;
        public PatientInsuranceDetailModel patientInsuranceDetail;
        public List<PatientInsuranceDetailModel> patientInsuranceDetailDelete;
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



        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        #region Payor Detail Command
        private RelayCommand _AddPayorDetailCommand;
        public RelayCommand AddPayorDetailCommand
        {
            get { return _AddPayorDetailCommand ?? (_AddPayorDetailCommand = new RelayCommand(AddPayorDetail)); }
        }
       
        private RelayCommand _EditPayorDetailCommand;
        public RelayCommand EditPayorDetailCommand
        {
            get { return _EditPayorDetailCommand ?? (_EditPayorDetailCommand = new RelayCommand(EditPayorDetail)); }
        }

        private RelayCommand _DeletePayorDetailCommand;
        public RelayCommand DeletePayorDetailCommand
        {
            get { return _DeletePayorDetailCommand ?? (_DeletePayorDetailCommand = new RelayCommand(DeletePayorDetail)); }
        }

        private RelayCommand _ClearPayorDetailCommand;
        public RelayCommand ClearPayorDetailCommand
        {
            get { return _ClearPayorDetailCommand ?? (_ClearPayorDetailCommand = new RelayCommand(ClearPayorDetail)); }
        }

        private RelayCommand _SavePayorDetailCommand;
        public RelayCommand SavePayorDetailCommand
        {
            get { return _SavePayorDetailCommand ?? (_SavePayorDetailCommand = new RelayCommand(SavePayorDetail)); }
        }

        private RelayCommand _CancelPayorDetailCommand;
        public RelayCommand CancelPayorDetailCommand
        {
            get { return _CancelPayorDetailCommand ?? (_CancelPayorDetailCommand = new RelayCommand(CancelPayorDetail)); }
        }

        #endregion

        #endregion

        #region Method
        public ManagePatientViewModel()
        {
            DateTime now = DateTime.Now;
            List<LookupReferenceValueModel> dataLookupSource = DataService.Technical.GetReferenceValueList("SEXXX,TITLE,BLOOD,MARRY,RELGN,NATNL,RQPRT,OCCUP,SPOKL,VIPTP");
            GenderSource = dataLookupSource.Where(p => p.DomainCode == "SEXXX").ToList();
            TitleSource = dataLookupSource.Where(p => p.DomainCode == "TITLE").ToList();
            BloodGroupSource = dataLookupSource.Where(p => p.DomainCode == "BLOOD").ToList();
            MarryStatusSource = dataLookupSource.Where(p => p.DomainCode == "MARRY").ToList();
            OccupationSource = dataLookupSource.Where(p => p.DomainCode == "OCCUP").ToList();
            RegionSource = dataLookupSource.Where(p => p.DomainCode == "RELGN").OrderBy(p => p.DisplayOrder).ToList();
            NationalSource = dataLookupSource.Where(p => p.DomainCode == "NATNL").OrderBy(p => p.DisplayOrder).ToList();
            PreferredLanguageSource = dataLookupSource.Where(p => p.DomainCode == "SPOKL").OrderBy(p => p.DisplayOrder).ToList();
            VIPTypeSources = dataLookupSource.Where(P => P.DomainCode == "VIPTP").OrderBy(p => p.DisplayOrder).ToList();
            //PayorDetailSource = DataService.MasterData.GetPayorDetail();
            referenceRealationShipTitle = DataService.Technical.GetReferenceRealationShip("TITLE", "SEXXX");

            ProvinceSource = DataService.Technical.GetProvince();
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "");
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
                PatientInformationModel patientInfo = GeneratePatientID();
                if(patientInfo != null)
                {
                    RegisterPatient register = new RegisterPatient();
                    ChangeViewPermission(register);
                }

            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }

        public PatientInformationModel GeneratePatientID()
        {
            try
            {
                PatientInformationModel resultPatient;
                if (ValidatePatientData())
                {
                    return null;
                }
                #region CheckPatientDuplicate

                if (patientModel == null)
                {
                    DateTime? birthDttm;
                    CheckBirthDttm(out birthDttm);
                    if (birthDttm == null)
                    {
                        birthDttm = BirthDate != null ? BirthDate.Value : (DateTime?)null;
                    }

                    PatientInformationModel patAlready = DataService.PatientIdentity.CheckDupicatePatient(FirstName, LastName, birthDttm.Value, SelectedGender.Key.Value);
                    if (patAlready != null)
                    {
                        MessageBoxResult dialogResult = System.Windows.MessageBox.Show("มีผู้ป่วยนี้ มี ชื่อ นามสกุล เพศ วันเดือนปีเกิด ซ้ำในะระบบ" + " \r\n คุณต้องการลงทะเบียนต่อหรือไม่ ? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dialogResult == MessageBoxResult.No)
                        {
                            return null;
                        }
                    }
                }
                PatientInformationModel patientInfo = AssingPropertiesToModel();
                resultPatient = DataService.PatientIdentity.RegisterPatient(patientInfo, AppUtil.Current.UserID, AppUtil.Current.OwnerOrganisationUID);

                if (resultPatient == null)
                {
                    ErrorDialog("ไม่สามารถบันทึกข้อมูลคนไข้ได้ ติดต่อ Admin");
                    return null;
                }
                #endregion

                if(String.IsNullOrEmpty(patientInfo.PatientID))
                {
                    SaveSuccessDialog("HN : " + resultPatient.PatientID);
                }

                return resultPatient;
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
                return null;
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
            PassportNo = string.Empty;
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
            SelectedOccupation = null;
            CommentDoctor = string.Empty;
            IsVIP = false;

        }

        public void AssingModel(PatientInformationModel patientData)
        {
            SelectTabIndex = 0;
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
            PassportNo = patientModel.IDPassport;
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
            patientModel.IDPassport = PassportNo;

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
            MobilePhone = patientData.MobilePhone;
            PassportNo = patientData.IDPassport;
        }

        #region Payor Detail Method

        private void AddPayorDetail()
        {
            if(SelectInsuranceName == null)
            {
                WarningDialog("กรุณาเลือก Insurance Company");
                return;
            }

            if (SelectAgreement == null)
            {
                WarningDialog("กรุณาเลือก Agreement");
                return;
            }

            if (SelectPayorDetail == null)
            {
                WarningDialog("กรุณาเลือก Payor");
                return;
            }

            bool isDuplicate = CheckDuplicate(SelectInsuranceName.InsuranceCompanyUID, SelectAgreement.PayorAgreementUID);

            if (isDuplicate == true)
            {
                WarningDialog(SelectInsuranceName.CompanyName + " Agreement "+ SelectAgreement.PayorAgreementName + " มีรายการแล้ว กรุณาตรวจสอบรายการที่ต้องการเพิ่มอีกครั้ง");
                return;
            }

            AssignSelectToPayorGrid();

            if(patientInsuranceDetail != null)
            {
              
                PatientInsuranceDetail.Add(patientInsuranceDetail);
            }

            ClearPayorDetail();
        }

        private void EditPayorDetail()
        {
            if(SelectPatientInsuranceDetail != null)
            {
                if (SelectInsuranceName == null)
                {
                    WarningDialog("กรุณาเลือก Insurance Company");
                    return;
                }
                //var index = PatientInsuranceDetail.IndexOf(SelectPatientInsuranceDetail);
                var index = PatientInsuranceDetail.Where(p => p.PayorAgreementUID == SelectPatientInsuranceDetail.PayorAgreementUID && p.InsuranceCompanyUID == SelectPatientInsuranceDetail.InsuranceCompanyUID);
                var item = SelectPatientInsuranceDetail;
                item.InsuranceCompanyName = SelectInsuranceName.CompanyName;
                item.InsuranceCompanyUID = SelectInsuranceName.InsuranceCompanyUID;

                item.EligibleAmount = EligibleAmount;
                item.PayorAgreementName = SelectAgreement.PayorAgreementName;
                item.PayorAgreementUID = SelectAgreement != null ? SelectAgreement.PayorAgreementUID : (int?)null;
                item.PayorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;
                item.PayorName = SelectPayorDetail.PayorName;
                item.PolicyMasterUID = SelectPolicy != null ? SelectPolicy.PolicyMasterUID : (int?)null;
                item.PolicyName = SelectPolicy != null ? SelectPolicy.PolicyName : null;

                item.FixedCopayAmount = FixedCopayAmount;
                item.ClaimPercentage = ClaimPercentage;

                item.PAYRTPUID = SelectPayorType != null ? SelectPayorType.Key : (int?)null;
                item.Type = SelectPayorType != null ? SelectPayorType.Display : null;

                item.StartDttm = PayorDetailActiveFrom;
                item.EndDttm = PayorDetailActiveTo;
                item.Comments = Comment;

                PatientInsuranceDetail.Remove(index.FirstOrDefault());
                PatientInsuranceDetail.Add(item);

                ClearPayorDetail();
            }
        }

        private void DeletePayorDetail()
        {
            if (SelectPatientInsuranceDetail != null)
            {
                if(SelectPatientInsuranceDetail.PatientInsuranceDetailUID != 0)
                {
                    PatientInsuranceDetailModel item = new PatientInsuranceDetailModel();
                    item = SelectPatientInsuranceDetail;
                    item.StatusFlag = "D";
                    item.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                    item.CUser = AppUtil.Current.UserID;

                    if (patientInsuranceDetailDelete == null)
                        patientInsuranceDetailDelete = new List<PatientInsuranceDetailModel>();

                    patientInsuranceDetailDelete.Add(item);
                }

                PatientInsuranceDetail.Remove(SelectPatientInsuranceDetail);
                ClearPayorDetail();
            }
        }

        private void ClearPayorDetail()
        {
            SelectInsuranceName = null;
            SelectPayorDetail = null;
            SelectAgreement = null;
            SelectPolicy = null;
            SelectPayorType = null;
            EligibleAmount = null;
            ClaimPercentage = null;
            FixedCopayAmount = null;
            Comment = null;
            PayorDetailActiveFrom = DateTime.Now;
            PayorDetailActiveTo = null;
        }
        private void AssignSelectToPropotiesPayor()
        {
            
            if (SelectPatientInsuranceDetail != null)
            {
                SelectInsuranceName = InsuranceNameSource.FirstOrDefault(p => p.InsuranceCompanyUID == SelectPatientInsuranceDetail.InsuranceCompanyUID);
                SelectAgreement = AgreementSource.FirstOrDefault(p => p.PayorAgreementUID == SelectPatientInsuranceDetail.PayorAgreementUID);
                SelectPayorType = PayorType.FirstOrDefault(p => p.Key == SelectPatientInsuranceDetail.PAYRTPUID);
                FixedCopayAmount = SelectPatientInsuranceDetail.FixedCopayAmount;
                ClaimPercentage = SelectPatientInsuranceDetail.ClaimPercentage;
                Comment = SelectPatientInsuranceDetail.Comments;
                EligibleAmount = SelectPatientInsuranceDetail.EligibleAmount;
                PayorDetailActiveFrom = SelectPatientInsuranceDetail.StartDttm;
                PayorDetailActiveTo = SelectPatientInsuranceDetail.EndDttm;

               
            }
        }
        private void AssignSelectToPayorGrid()
        {
            patientInsuranceDetail = new PatientInsuranceDetailModel();

            patientInsuranceDetail.PatientUID = patientModel.PatientUID;

            patientInsuranceDetail.InsuranceCompanyName = SelectInsuranceName.CompanyName;
            patientInsuranceDetail.InsuranceCompanyUID = SelectInsuranceName.InsuranceCompanyUID;

            patientInsuranceDetail.EligibleAmount = EligibleAmount;
            patientInsuranceDetail.PayorAgreementName = SelectAgreement.PayorAgreementName;
            patientInsuranceDetail.PayorAgreementUID = SelectAgreement != null ? SelectAgreement.PayorAgreementUID : (int?)null;
            patientInsuranceDetail.PayorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;
            patientInsuranceDetail.PayorName =  SelectPayorDetail.PayorName ;
            patientInsuranceDetail.PolicyMasterUID = SelectPolicy != null ? SelectPolicy.PolicyMasterUID : (int?)null;
            patientInsuranceDetail.PolicyName = SelectPolicy != null ? SelectPolicy.PolicyName : null ;

            patientInsuranceDetail.FixedCopayAmount = FixedCopayAmount;
            patientInsuranceDetail.ClaimPercentage = ClaimPercentage;

            patientInsuranceDetail.PAYRTPUID = SelectPayorType != null ? SelectPayorType.Key : (int?)null ;
            patientInsuranceDetail.Type = SelectPayorType != null ? SelectPayorType.Display : null;

            patientInsuranceDetail.StartDttm = PayorDetailActiveFrom;
            patientInsuranceDetail.EndDttm = PayorDetailActiveTo;
            patientInsuranceDetail.Comments = Comment;

            patientInsuranceDetail.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
        }

        private List<PatientInsuranceDetailModel> AssignPayorToModel()
        {
            List<PatientInsuranceDetailModel> returnData = new List<PatientInsuranceDetailModel>();

            if (PatientInsuranceDetail != null)
            {
                foreach(var item in PatientInsuranceDetail)
                {
                    //PatientInsuranceDetailModel data = new PatientInsuranceDetailModel();
                    //data = item;
                    item.StatusFlag = "A";
                    item.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                    returnData.Add(item);
                }
            }
            return returnData;
        }

        private void SavePayorDetail()
        {
            if(PatientInsuranceDetail.Count > 0 || patientInsuranceDetailDelete.Count > 0)
            {
                var data = AssignPayorToModel();
                if (patientInsuranceDetailDelete != null)
                {
                    data.AddRange(patientInsuranceDetailDelete);
                }

                DataService.PatientIdentity.ManagePatientInsurance(data, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ClearPayorDetail();
            }
           
        }

        private void CancelPayorDetail()
        {

        }

        private bool CheckDuplicate(int insuranceCompanyUID, int agreementUID)
        {
            bool Duplicate;
            var data = PatientInsuranceDetail.Where(p => p.InsuranceCompanyUID == insuranceCompanyUID && p.PayorAgreementUID == agreementUID).FirstOrDefault();
            
            Duplicate = data != null ? true : false;
            return Duplicate;
        }
        #endregion

        #endregion


    }
}
