using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.Helpers;
using MediTech.Helpers.RDNIDWRAPPER;
using MediTech.Model;
using MediTech.Views;
using ShareLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class EmergencyRegisterViewModel : MediTechViewModelBase
    {
        #region Properties
        public LookupReferenceValueModel EncouterER { get; set; }

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
        public List<LookupReferenceValueModel> NationalSource { get; set; }

        private LookupReferenceValueModel _SelectedNational;
        public LookupReferenceValueModel SelectedNational
        {
            get { return _SelectedNational; }
            set { Set(ref _SelectedNational, value); }
        }

        private bool _CalculateBrithDate;

        public bool CalculateBrithDate
        {
            get { return _CalculateBrithDate; }
            set { Set(ref _CalculateBrithDate, value); }
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

        private string _PassportNo;
        public string PassportNo
        {
            get { return _PassportNo; }
            set { Set(ref _PassportNo, value); }
        }

        private string _NatinonalID;
        public string NatinonalID
        {
            get { return _NatinonalID; }
            set { Set(ref _NatinonalID, value); }
        }

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

        public List<CareproviderModel> CareproviderSource { get; set; }
        private CareproviderModel _SelectedCareprovider;

        public CareproviderModel SelectedCareprovider
        {
            get { return _SelectedCareprovider; }
            set { Set(ref _SelectedCareprovider, value); }
        }

        #region EmergencyVisit

        private List<LookupReferenceValueModel> _EmergencyType;
        public List<LookupReferenceValueModel> EmergencyType
        {
            get { return _EmergencyType; }
            set { Set(ref _EmergencyType, value); }
        }
        private LookupReferenceValueModel _SelectedEmergencyType;
        public LookupReferenceValueModel SelectedEmergencyType
        {
            get { return _SelectedEmergencyType; }
            set { Set(ref _SelectedEmergencyType, value); }
        }

        public List<LookupReferenceValueModel> ArrivalSource { get; set; }
        private LookupReferenceValueModel _SelectedArrival;

        public LookupReferenceValueModel SelectedArrival
        {
            get { return _SelectedArrival; }
            set { Set(ref _SelectedArrival, value); }
        }

        public List<LookupReferenceValueModel> EscoutTypeSource { get; set; }
        private LookupReferenceValueModel _SelectedTypeEscoutl;

        public LookupReferenceValueModel SelectedTypeEscoutl
        {
            get { return _SelectedTypeEscoutl; }
            set { Set(ref _SelectedTypeEscoutl, value); }
        }

        public List<LookupReferenceValueModel> ReferralSource { get; set; }
        private LookupReferenceValueModel _SelectedReferral;

        public LookupReferenceValueModel SelectedReferral
        {
            get { return _SelectedReferral; }
            set { Set(ref _SelectedReferral, value); }
        }

        private string _VehicleNumber;
        public string VehicleNumber
        {
            get { return _VehicleNumber; }
            set { Set(ref _VehicleNumber, value); }
        }

        private DateTime _EmergencyStartDate;

        public DateTime EmergencyStartDate
        {
            get { return _EmergencyStartDate; }
            set { Set(ref _EmergencyStartDate, value); }
        }

        private DateTime _EmergencyStartTime;
        public DateTime EmergencyStartTime
        {
            get { return _EmergencyStartTime; }
            set { Set(ref _EmergencyStartTime, value); }
        }

        private string _ReasonDetail;
        public string ReasonDetail
        {
            get { return _ReasonDetail; }
            set { Set(ref _ReasonDetail, value); }
        }

        private string _EmergencyExamDetail;
        public string EmergencyExamDetail
        {
            get { return _EmergencyExamDetail; }
            set { Set(ref _EmergencyExamDetail, value); }
        }

        #endregion

        #region ProblemSearch
        private string _ProblemCode;
        public string ProblemCode
        {
            get { return _ProblemCode; }
            set { Set(ref _ProblemCode, value); }
        }

        private string _ProblemName;
        public string ProblemName
        {
            get { return _ProblemName; }
            set { Set(ref _ProblemName, value); }
        }

        private string _SearchProblemCriteria;
        public string SearchProblemCriteria
        {
            get { return _SearchProblemCriteria; }
            set
            {
                Set(ref _SearchProblemCriteria, value);
                ProblemSearchSource = null;
            }
        }

        private List<ProblemModel> _ProblemSearchSource;
        public List<ProblemModel> ProblemSearchSource
        {
            get { return _ProblemSearchSource; }
            set { Set(ref _ProblemSearchSource, value); }
        }

        private ProblemModel _SelectedProblemSearch;

        public ProblemModel SelectedProblemSearch
        {
            get { return _SelectedProblemSearch; }
            set
            {
                _SelectedProblemSearch = value;
                if (_SelectedProblemSearch != null)
                {
                    ProblemCode = SelectedProblemSearch.Code;
                    ProblemName = SelectedProblemSearch.Name;
                    //AssingModel(SelectedPateintSearch);
                }
            }
        }
        private bool _IsManageEmergency;
        public bool IsManageEmergency
        {
            get { return _IsManageEmergency; }
            set { Set(ref _IsManageEmergency, value); }
        }

        private bool _IsWardView;
        public bool IsWardView
        {
            get { return _IsWardView; }
            set { Set(ref _IsWardView, value); }
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

        #region Varible

        List<ReferenceRelationShipModel> referenceRealationShipTitle;
        public PatientInformationModel patientModel;
        public PatientAEAdmissionModel patientERModel;
        #endregion

        #region Address
        public bool SuppressZipCodeEvent { get; set; }

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

        #endregion

        private List<LocationModel> _Location;
        public List<LocationModel> Location
        {
            get { return _Location; }
            set { Set(ref _Location, value); }
        }
        private LocationModel _SelectedLocation;
        public LocationModel SelectedLocation
        {
            get { return _SelectedLocation; }
            set { Set(ref _SelectedLocation, value); }
        }

        private List<LookupReferenceValueModel> _Triage;
        public List<LookupReferenceValueModel> Triage
        {
            get { return _Triage; }
            set { Set(ref _Triage, value); }
        }
        private LookupReferenceValueModel _SelectedTriage;
        public LookupReferenceValueModel SelectedTriage
        {
            get { return _SelectedTriage; }
            set { Set(ref _SelectedTriage, value); }
        }

        private List<LocationModel> _Bed;
        public List<LocationModel> Bed
        {
            get { return _Bed; }
            set { Set(ref _Bed, value); }
        }
        private LocationModel _SelectedBed;
        public LocationModel SelectedBed
        {
            get { return _SelectedBed; }
            set { Set(ref _SelectedBed, value); }
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
                    //if (SelectOrganisation.HealthOrganisationUID == 5)
                    //{
                    //    SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.ValueCode == "MBCHK");
                    //}

                    //else
                    //{
                    //    SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.ValueCode == "DCPAT");
                    //}
                }
            }
        }
        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _CancleEmergencyCommand;
        public RelayCommand CancleEmergencyCommand
        {
            get { return _CancleEmergencyCommand ?? (_CancleEmergencyCommand = new RelayCommand(Cancel)); }
        }

        private RelayCommand _ClearProblemCommand;
        public RelayCommand ClearProblemCommand
        {
            get { return _ClearProblemCommand ?? (_ClearProblemCommand = new RelayCommand(ClearProblem)); }
        }

        private RelayCommand _PatientSearchProblemCommand;
        public RelayCommand PatientSearchProblemCommand
        {
            get { return _PatientSearchProblemCommand ?? (_PatientSearchProblemCommand = new RelayCommand(ProblemSearch)); }
        }

        private RelayCommand _ManageEmergencyCommand;

        public RelayCommand ManageEmergencyCommand
        {
            get { return _ManageEmergencyCommand ?? (_ManageEmergencyCommand = new RelayCommand(ManageEmergencyVisit)); }
        }

        private RelayCommand _SaveEmergencyCommand;

        public RelayCommand SaveEmergencyCommand
        {
            get { return _SaveEmergencyCommand ?? (_SaveEmergencyCommand = new RelayCommand(SaveEmergencyVisit)); }
        }

        private RelayCommand _ToWardViewCommand;

        public RelayCommand ToWardViewCommand
        {
            get { return _ToWardViewCommand ?? (_ToWardViewCommand = new RelayCommand(ToWardView)); }
        }

        #endregion

        #region Method
        public EmergencyRegisterViewModel()
        {
            DateTime now = DateTime.Now;
            EmergencyStartDate = now.Date;
            EmergencyStartTime = now;

            List<LookupReferenceValueModel> dataLookupSource = DataService.Technical.GetReferenceValueList("SEXXX,TITLE,BLOOD,NATNL,ARRMD,ESCTP,RELTN,EMGTP,EMGCD,LOTYP,ENTYP");
            GenderSource = dataLookupSource.Where(p => p.DomainCode == "SEXXX").ToList();
            TitleSource = dataLookupSource.Where(p => p.DomainCode == "TITLE").ToList();
            BloodGroupSource = dataLookupSource.Where(p => p.DomainCode == "BLOOD").ToList();
            NationalSource = dataLookupSource.Where(p => p.DomainCode == "NATNL").OrderBy(p => p.DisplayOrder).ToList();
            referenceRealationShipTitle = DataService.Technical.GetReferenceRealationShip("TITLE", "SEXXX");

            ArrivalSource = dataLookupSource.Where(p => p.DomainCode == "ARRMD").ToList();
            EscoutTypeSource = dataLookupSource.Where(p => p.DomainCode == "ESCTP").ToList();
            ReferralSource = dataLookupSource.Where(p => p.DomainCode == "RELTN").ToList();
            EmergencyType = dataLookupSource.Where(p => p.DomainCode == "EMGTP").ToList();
            
            ProvinceSource = DataService.Technical.GetProvince();
            Triage = dataLookupSource.Where(p => p.DomainCode == "EMGCD").ToList();
            //Location = DataService.Technical.GetLocationByType(3160);
            //SelectedLocation = DataService.Technical.GetLocationByTypeUID(3160).FirstOrDefault();
            var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Location = org.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            SelectedLocation = Location.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);

            EncouterER = dataLookupSource.Where(p => p.DomainCode == "ENTYP").FirstOrDefault(p => p.ValueCode == "AEPAT");
            //Bed = DataService.Technical.GetLocationByLocationUID(SelectedLocation.LocationUID).ToList();
            var bed = DataService.Technical.GetLocationByTypeUID(3160).FirstOrDefault();
            Bed = DataService.PatientIdentity.GetBedLocation(bed.LocationUID, EncouterER.Key).Where(p => p.BedIsUse == "N").ToList();
            

            CareproviderSource = DataService.UserManage.GetCareproviderDoctor();

        }

        public void SaveEmergencyVisit()
        {

            try
            {

                if (ValidatePatientData())
                {
                    return;
                }
                PatientInformationModel resultPatient = new PatientInformationModel();

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

                        resultPatient.PatientUID = patientModel.PatientUID;
                    }
                    else
                    {
                        PatientInformationModel patientInfo = AssingPropertiesToModel();
                        resultPatient = DataService.PatientIdentity.RegisterPatientEmergency(patientInfo, AppUtil.Current.UserID, AppUtil.Current.OwnerOrganisationUID);

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

                    PatientInformationModel patAlready = DataService.PatientIdentity.CheckDupicatePatient(FirstName, LastName, birthDttm.Value, SelectedGender.Key.Value);
                    if (patAlready != null)
                    {
                        MessageBoxResult dialogResult = System.Windows.MessageBox.Show("มีผู้ป่วยนี้ มี ชื่อ นามสกุล เพศ วันเดือนปีเกิด ซ้ำในะระบบ" + " \r\n คุณต้องการลงทะเบียนต่อหรือไม่ ? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (dialogResult == MessageBoxResult.No)
                        {
                            return;
                        }
                    }

                    PatientInformationModel patientInfo = AssingPropertiesToModel();
                    resultPatient = DataService.PatientIdentity.RegisterPatientEmergency(patientInfo, AppUtil.Current.UserID, AppUtil.Current.OwnerOrganisationUID);

                }

                #endregion

                if (resultPatient == null)
                {
                    ErrorDialog("ไม่สามารถบันทึกข้อมูลคนไข้ได้ ติดต่อ Admin");
                    return;
                }

                PatientVisitModel visitInfo = new PatientVisitModel();
                visitInfo.StartDttm = DateTime.Parse(EmergencyStartDate.ToString("dd/MM/yyyy") + " " + EmergencyStartTime.ToString("HH:mm"));
                visitInfo.PatientUID = resultPatient.PatientUID;
                visitInfo.PatientID = resultPatient.PatientID;
                visitInfo.VISTYUID = DataService.Technical.GetReferenceValueByCode("VISTY", "EMR").Key; ; //visit type EMR
                
                visitInfo.VISTSUID = 417; //Registered
                visitInfo.PRITYUID = 442; //Emergency
                visitInfo.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID; //รอเปลี่ยนใช้ของคลินิกไปก่อน
                visitInfo.ENTYPUID = DataService.Technical.GetReferenceValueByCode("ENTYP", "AEPAT").Key;
                visitInfo.LocationUID = AppUtil.Current.LocationUID;
                visitInfo.IDPassport = PassportNo;
                if (SelectedBed != null)
                {
                    visitInfo.BedUID = SelectedBed.LocationUID;
                }

                PatientAEAdmissionModel aeAdmission = AssingVisitERToModel();

                visitInfo.AEAdmission = aeAdmission;
                if(resultPatient.PatientAddressUID != null)
                {
                    visitInfo.AEAdmission.ADTYPUID = Convert.ToInt32(resultPatient.PatientAddressUID);
                }

                PatientVisitModel returnData = DataService.PatientIdentity.SaveERPatientVisit(visitInfo, AppUtil.Current.UserID);
                if (string.IsNullOrEmpty(returnData.VisitID))
                {
                    ErrorDialog("ไม่สามารถบันทึกข้อมูล Visit คนไข้ได้ ติดต่อ Admin");
                    return;
                }

                SaveSuccessDialog("HN : " + returnData.PatientID + "\r\nEmergency Admitted Successfully");

                var patientVisitPayors = DataService.PatientIdentity.GetPatientVisitPayorByVisitUID(returnData.PatientVisitUID);
                if (patientVisitPayors.Count == 0)
                {
                    MessageBoxResult resultDiaglog = QuestionDialog("ยังไม่มี Payor Visit ต้องการ Modify Payor Visit หรือไม่");

                    if (resultDiaglog == MessageBoxResult.Yes)
                    {
                        ModifyVisitPayor pageview = new ModifyVisitPayor();
                        (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(returnData);
                        ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            SaveSuccessDialog();
                        }
                        //continue;
                    }
                }
                if ( IsWardView == true)
                {
                    EmergencyBedStatus emergencyWard = new EmergencyBedStatus();
                    ChangeViewPermission(emergencyWard);
                }
                else
                {
                    EmergencyList emergencyList = new EmergencyList();
                    ChangeViewPermission(emergencyList);
                }
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }

        public void ManageEmergencyVisit()
        {
            try
            {
                
                PatientAEAdmissionModel aeAdmission = AssingVisitERToModel();
                aeAdmission.PatientID = patientERModel.PatientID;
                aeAdmission.PatientUid = patientERModel.PatientUid;
                aeAdmission.PatientVisitUid = patientERModel.PatientVisitUid;
                aeAdmission.Uid = patientERModel.Uid;
                if(SelectedCareprovider != null)
                {
                    aeAdmission.CareproviderUID = SelectedCareprovider.CareproviderUID;
                }
                aeAdmission.BedUID = SelectedBed.LocationUID;
                
                aeAdmission = DataService.PatientIdentity.ManageEmergencyAE(aeAdmission, AppUtil.Current.UserID);

                //SaveSuccessDialog("บันทึกข้อมูลเรียบร้อย");
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }

        public PatientAEAdmissionModel AssingVisitERToModel()
        {
            PatientAEAdmissionModel visitErModel = new PatientAEAdmissionModel();

            if (SelectedArrival != null)
            {
                visitErModel.ARRMDUID = SelectedArrival.Key ?? 0;
            }
            if (SelectedTypeEscoutl != null)
            {
                visitErModel.ESCTPUID = SelectedTypeEscoutl.Key;
            }
            if (SelectedReferral != null)
            {
                visitErModel.RELTNUID = SelectedReferral.Key;
            }
            if (SelectedEmergencyType != null)
            {
                visitErModel.EMGTPUID = SelectedEmergencyType.Key;
            }
            if (SelectedTriage != null)
            {
                visitErModel.EMGCDUID = SelectedTriage.Key;
            }
            if(SelectedProblemSearch != null)
            {
                visitErModel.ProblemUID = SelectedProblemSearch.ProblemUID;
            }
            else if(patientERModel != null)
            {
                visitErModel.ProblemUID = patientERModel.ProblemUID;
            }

            visitErModel.LocationUid = SelectedLocation.LocationUID;
            visitErModel.InjuryReason = ReasonDetail;
            visitErModel.EmergencyExamDetail = EmergencyExamDetail;
            visitErModel.VehicleNumber = VehicleNumber;
            visitErModel.EventOccuredDttm = DateTime.Parse(EmergencyStartDate.ToString("dd/MM/yyyy") + " " + EmergencyStartTime.ToString("HH:mm"));
            visitErModel.PhoneNumber = SeconePhone;
            visitErModel.MobileNumber = MobilePhone;

            visitErModel.Line1 = Line1;
            visitErModel.Line2 = Line2;
            visitErModel.Line3 = Line3;

            if (SelectedProvince != null)
                visitErModel.ProvinceUID = SelectedProvince.Key;

            if (SelectedAmphur != null)
                visitErModel.AmphurUID = SelectedAmphur.Key;

            if (SelectedDistrict != null)
                visitErModel.DistrictUID = SelectedDistrict.Key;

            visitErModel.ZipCode = ZipCode;

            return visitErModel;
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

            return patientModel;
        }

        public void ProblemSearch()
        
        {
            if (SearchProblemCriteria.Length >= 3)
            {
                List<ProblemModel> searchProblem = DataService.PatientDiagnosis.SearchProblem(SearchProblemCriteria);
                ProblemSearchSource = searchProblem;
            }
            else
            {
                ProblemSearchSource = null;
            }
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatientEmergency(patientID, firstName, "", lastName, "", null, null, "", null, "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
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

        public void AssingModel(PatientInformationModel patientData = null, PatientAEAdmissionModel erVisit = null, string bedCode = "")
        {
            if(bedCode != "")
            {
                SelectedBed = Bed.FirstOrDefault(p => p.Name == bedCode);
                IsWardView = true;
            }

            if(erVisit != null)
            {
                IsManageEmergency = true;
                patientERModel = erVisit;
                AssingModelToPropertiesFromManage();
            }
            else
            {
                patientModel = patientData ?? new PatientInformationModel();
                AssingModelToProperties();
            }

            //patientModel = patientData;
            //AssingModelToProperties();

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

            //if (bedCode != 0)
            //{
            //    SelectedBed = Bed.FirstOrDefault(p => p.Key == bedCode);
            //}

            Email = patientModel.Email;
            IDLine = patientModel.IDLine;
            SeconePhone = patientModel.SecondPhone;
            MobilePhone = patientModel.MobilePhone;

            
        }

        public void AssingModelToPropertiesFromManage()
        {
            PatientID = patientERModel.PatientID;
            SelectedTitle = TitleSource.FirstOrDefault(p => p.Key == patientERModel.TITLEUID);
            SelectedGender = GenderSource.FirstOrDefault(p => p.Key == patientERModel.SEXXXUID);
            FirstName = patientERModel.FirstName;
            LastName = patientERModel.LastName;
            NickName = patientERModel.NickName;

            BirthDate = patientERModel.BirthDttm;
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

            //OtherID = patientModel.EmployeeID;
            CalculateBrithDate = patientERModel.DOBComputed;
            //NatinonalID = patientERModel.NationalID;
            SelectedBloodGroup = BloodGroupSource.FirstOrDefault(p => p.Key == patientERModel.BLOODUID);
            SelectedNational = NationalSource.FirstOrDefault(p => p.Key == patientERModel.NATNLUID);
            //PassportNo = patientModel.IDPassport;

            Line1 = patientERModel.Line1;
            Line2 = patientERModel.Line2;
            Line3 = patientERModel.Line3;

            if (ProvinceSource != null)
                SelectedProvince = ProvinceSource.FirstOrDefault(p => p.Key == patientERModel.ProvinceUID);

            if (AmphurSource != null)
                SelectedAmphur = AmphurSource.FirstOrDefault(p => p.Key == patientERModel.AmphurUID);

            if (DistrictSource != null)
                SelectedDistrict = DistrictSource.FirstOrDefault(p => p.Key == patientERModel.DistrictUID);

            SuppressZipCodeEvent = true;
            ZipCode = patientERModel.ZipCode;

            if (patientERModel.BedUID != null)
            {

                List<LocationModel> data = DataService.PatientIdentity.GetBedLocation(SelectedLocation.LocationUID,EncouterER.Key).Where(p => p.BedIsUse == "N").ToList();
                LocationModel BedUse = DataService.Technical.GetLocationByUID(patientERModel.BedUID ?? 0);
                data.Add(BedUse);
                Bed = data.OrderByDescending(p => p.Name).ToList();

                SelectedBed = Bed.FirstOrDefault(p => p.LocationUID == patientERModel.BedUID);
            }

            if(patientERModel.CareproviderUID != null)
            {
                SelectedCareprovider = CareproviderSource.FirstOrDefault(p => p.CareproviderUID == patientERModel.CareproviderUID);
            }

            //Email = patientERModel.Email;
            //IDLine = patientERModel.IDLine;
            SeconePhone = patientERModel.PhoneNumber;
            MobilePhone = patientERModel.MobileNumber;

            if (patientERModel != null)
            {
                SelectedArrival = ArrivalSource.FirstOrDefault(p => p.Key == patientERModel.ARRMDUID);
                SelectedTypeEscoutl = EscoutTypeSource.FirstOrDefault(p => p.Key == patientERModel.ESCTPUID);
                SelectedReferral = ReferralSource.FirstOrDefault(p => p.Key == patientERModel.RELTNUID);
                VehicleNumber = patientERModel.VehicleNumber;
                EmergencyStartDate = patientERModel.EventOccuredDttm ?? DateTime.Now.Date;
                EmergencyStartTime = patientERModel.EventOccuredDttm ?? DateTime.Now;
                SelectedEmergencyType = EmergencyType.FirstOrDefault(p => p.Key == patientERModel.EMGTPUID);
                SelectedTriage = Triage.FirstOrDefault(p => p.Key == patientERModel.EMGCDUID);
                ReasonDetail = patientERModel.InjuryReason;
                EmergencyExamDetail = patientERModel.EmergencyExamDetail;

                Line1 = patientERModel.Line1;
                Line2 = patientERModel.Line2;
                Line3 = patientERModel.Line3;
                if(patientERModel.ProvinceUID != null) 
                {
                    SelectedProvince = ProvinceSource.FirstOrDefault(p => p.Key == patientERModel.ProvinceUID);
                }
                if (patientERModel.AmphurUID != null)
                {
                    SelectedAmphur = AmphurSource.FirstOrDefault(p => p.Key == patientERModel.AmphurUID);
                }
                if (patientERModel.DistrictUID != null)
                {
                    SelectedDistrict = DistrictSource.FirstOrDefault(p => p.Key == patientERModel.DistrictUID);
                }
                
                MobilePhone = patientERModel.PhoneNumber;
                SeconePhone = patientERModel.MobileNumber;

                if (patientERModel.ProblemUID != null)
                {
                    ProblemModel problem = DataService.PatientDiagnosis.GetProblemByUID(patientERModel.ProblemUID ?? 0);
                    
                    ProblemCode = problem.Code;
                    ProblemName = problem.Name;
                }
            }
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

        public void ClearProblem()
        {
            ProblemCode = null;
            ProblemName = null;
            SelectedProblemSearch = null;
        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void ToWardView()
        {
            EmergencyBedStatus emergencyWard = new EmergencyBedStatus();
            ChangeViewPermission(emergencyWard);
        }

        #endregion
    }
}
