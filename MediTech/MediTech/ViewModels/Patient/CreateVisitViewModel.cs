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
using System.Collections.ObjectModel;

namespace MediTech.ViewModels
{
    public class CreateVisitViewModel : MediTechViewModelBase
    {
        #region Properties

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
                    Locations = DataService.MasterData.GetLocationIsRegister(SelectOrganisation.HealthOrganisationUID);
                    SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == AppUtil.Current.LocationUID);
                    if (SelectOrganisation.HealthOrganisationUID == 5)
                    {
                        SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.ValueCode == "MBCHK");
                    }
                }
            }
        }

        private List<LocationModel> _Locations;

        public List<LocationModel> Locations
        {
            get { return _Locations; }
            set { Set(ref _Locations, value); }
        }

        private LocationModel _SelectLocation;

        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set
            {
                Set(ref _SelectLocation, value);
            }
        }


        private List<LookupReferenceValueModel> _VisitTypeSource;

        public List<LookupReferenceValueModel> VisitTypeSource
        {
            get { return _VisitTypeSource; }
            set
            {
                Set(ref _VisitTypeSource, value);
            }
        }


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

                }
            }
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


        private List<LookupReferenceValueModel> _PrioritySource;

        public List<LookupReferenceValueModel> PrioritySource
        {
            get { return _PrioritySource; }
            set
            {
                Set(ref _PrioritySource, value);
            }
        }


        private LookupReferenceValueModel _SelectedPriority;

        public LookupReferenceValueModel SelectedPriority
        {
            get { return _SelectedPriority; }
            set
            {
                Set(ref _SelectedPriority, value);
            }
        }

        private List<LookupReferenceValueModel> _PayorTypes;

        public List<LookupReferenceValueModel> PayorTypes
        {
            get { return _PayorTypes; }
            set
            {
                Set(ref _PayorTypes, value);
            }
        }


        private LookupReferenceValueModel _SelectedPayorType;

        public LookupReferenceValueModel SelectedPayorType
        {
            get { return _SelectedPayorType; }
            set
            {
                Set(ref _SelectedPayorType, value);
            }
        }

        private List<InsuranceCompanyModel> _InsuranceCompanys;

        public List<InsuranceCompanyModel> InsuranceCompanys
        {
            get { return _InsuranceCompanys; }
            set { Set(ref _InsuranceCompanys, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompany;

        public InsuranceCompanyModel SelectInsuranceCompany
        {
            get { return _SelectInsuranceCompany; }
            set
            {
                Set(ref _SelectInsuranceCompany, value);
                if (_SelectInsuranceCompany != null)
                {
                    InsurancePlans = DataService.Billing.GetInsurancePlans(_SelectInsuranceCompany.InsuranceCompanyUID);
                }
            }
        }

        private List<InsurancePlanModel> _InsurancePlans;

        public List<InsurancePlanModel> InsurancePlans
        {
            get { return _InsurancePlans; }
            set { Set(ref _InsurancePlans, value); }
        }

        private InsurancePlanModel _SelectInsurancePlan;

        public InsurancePlanModel SelectInsurancePlan
        {
            get { return _SelectInsurancePlan; }
            set
            {
                Set(ref _SelectInsurancePlan, value);
                if (_SelectInsurancePlan != null)
                {
                    OPDCoverPerDay = _SelectInsurancePlan.OPDCoverPerDay;
                    FixedCopayAmount = _SelectInsurancePlan.FixedCopayAmount;
                    ClaimPercentage = _SelectInsurancePlan.ClaimPercentage;
                }
            }
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

        private ObservableCollection<PatientVisitPayorModel> _PatientVisitPayorList;

        public ObservableCollection<PatientVisitPayorModel> PatientVisitPayorList
        {
            get { return _PatientVisitPayorList; }
            set { Set(ref _PatientVisitPayorList, value); }
        }

        private PatientVisitPayorModel _SelectedPatientVisitPayor;

        public PatientVisitPayorModel SelectedPatientVisitPayor
        {
            get { return _SelectedPatientVisitPayor; }
            set { _SelectedPatientVisitPayor = value; }
        }


        public List<CareproviderModel> CareproviderSource { get; set; }
        private CareproviderModel _SelectedCareprovider;

        public CareproviderModel SelectedCareprovider
        {
            get { return _SelectedCareprovider; }
            set { Set(ref _SelectedCareprovider, value); }
        }


        private string _CommentDoctor;

        public string CommentDoctor
        {
            get { return _CommentDoctor; }
            set { Set(ref _CommentDoctor, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private double? _OPDCoverPerDay;

        public double? OPDCoverPerDay
        {
            get { return _OPDCoverPerDay; }
            set { Set(ref _OPDCoverPerDay, value); }
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


        private DateTime? _ActiveForm;

        public DateTime? ActiveFrom
        {
            get { return _ActiveForm; }
            set { Set(ref _ActiveForm, value); }
        }

        private DateTime? _ActiveTo;

        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }

        private Visibility _VisibiltyCheckupCompany = Visibility.Collapsed;

        public Visibility VisibiltyCheckupCompany
        {
            get { return _VisibiltyCheckupCompany; }
            set { Set(ref _VisibiltyCheckupCompany, value); }
        }


        private Visibility _VisibilityCancelVisit;

        public Visibility VisibilityCancelVisit
        {
            get { return _VisibilityCancelVisit; }
            set { Set(ref _VisibilityCancelVisit, value); }
        }

        private Visibility _VisibilityCancel;

        public Visibility VisibilityCancel
        {
            get { return _VisibilityCancel; }
            set { Set(ref _VisibilityCancel, value); }
        }

        private bool _IsUpdateVisit = false;

        public bool IsUpdateVisit
        {
            get { return _IsUpdateVisit; }
            set { Set(ref _IsUpdateVisit, value); }
        }


        private PatientInformationModel _Patient;

        public PatientInformationModel Patient
        {
            get { return _Patient; }
            set { Set(ref _Patient, value); }
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
                    SelectedCareprovider = CareproviderSource.FirstOrDefault(p => p.CareproviderUID == Booking.CareProviderUID);
                }
            }
        }

        private bool _UseReadCard = false;

        public bool UseReadCard
        {
            get { return _UseReadCard; }
            set { _UseReadCard = value; }
        }
        #endregion

        #region Command
        private RelayCommand _AddVisitPayorCommand;

        public RelayCommand AddVisitPayorCommand
        {
            get { return _AddVisitPayorCommand ?? (_AddVisitPayorCommand = new RelayCommand(AddVisitPayor)); }
        }

        private RelayCommand _UpdateVisitPayorCommand;

        public RelayCommand UpdateVisitPayorCommand
        {
            get { return _UpdateVisitPayorCommand ?? (_UpdateVisitPayorCommand = new RelayCommand(UpdateVisitPayor)); }
        }

        private RelayCommand _DeleteVisitPayorCommand;

        public RelayCommand DeleteVisitPayorCommand
        {
            get { return _DeleteVisitPayorCommand ?? (_DeleteVisitPayorCommand = new RelayCommand(DeleteVisitPayor)); }
        }



        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SavePatientVisit)); }
        }

        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }
        #endregion

        #region Method


        public override void OnLoaded()
        {
            base.OnLoaded();
            DateTime now = DateTime.Now;

            (this.View as CreateVisit).banner.SetPatientBanner(Patient.PatientUID, 0);

            List<LookupReferenceValueModel> dataLookupSource = DataService.Technical.GetReferenceValueList("VISTY,RQPRT,PAYRTP");
            Organisations = GetHealthOrganisationMedical();
            VisitTypeSource = dataLookupSource.Where(p => p.DomainCode == "VISTY").ToList();
            PrioritySource = dataLookupSource.Where(P => P.DomainCode == "RQPRT").ToList();
            PayorTypes = dataLookupSource.Where(P => P.DomainCode == "PAYRTP").ToList();
            CareproviderSource = DataService.UserManage.GetCareproviderDoctor();
            InsuranceCompanys = DataService.Billing.GetInsuranceCompanies();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            SelectedVisitType = VisitTypeSource.FirstOrDefault();
            SelectedPriority = PrioritySource.FirstOrDefault(p => p.Key == 440);

            StartDate = now.Date;
            StartTime = now;

            LoadPatientVisitPayors();

            if (PatientVisitPayorList != null && PatientVisitPayorList.Count() > 0 && PayorTypes != null)
                SelectedPayorType = (from p in PayorTypes where (!(from q in PatientVisitPayorList select q.PAYRTPUID).Contains(p.Key)) select p).FirstOrDefault();
            else
                SelectedPayorType = PayorTypes.FirstOrDefault(p => p.ValueCode == "PRIMARY");

            if (IsUpdateVisit == false)
            {
                #region CheckVisitDuplicate

                List<PatientVisitModel> visitData = DataService.PatientIdentity.GetPatientVisitByPatientUID(Patient.PatientUID);
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


                #endregion
            }
        }



        void LoadPatientVisitPayors()
        {
            PatientVisitPayorList = new ObservableCollection<PatientVisitPayorModel>();
            var patientInsuranceDetail = DataService.PatientIdentity.GetPatientInsuranceDetail(Patient.PatientUID);
            if (patientInsuranceDetail != null && patientInsuranceDetail.Count > 0)
            {
                foreach (var aIns in patientInsuranceDetail)
                {
                    if (aIns.EndDttm == null || aIns.EndDttm >= DateTime.Today)
                    {
                        PatientVisitPayorModel aPayor = new PatientVisitPayorModel();
                        aPayor.PatientUID = Patient.PatientUID;
                        aPayor.PatientVisitUID = 0;
                        aPayor.PayorDetailUID = aIns.PayorDetailUID ?? 0;
                        aPayor.PayorAgreementUID = aIns.PayorAgreementUID ?? 0;
                        var agreement = GetAgreement(aIns.PayorAgreementUID ?? 0);
                        if (agreement != null)
                        {
                            aPayor.AgreementName = agreement.Name;
                            aPayor.PolicyMasterUID = agreement.PolicyMasterUID;
                            aPayor.PolicyName = agreement.PolicyName;
                        };
                        aPayor.InsuranceCompanyUID = aIns.InsuranceCompanyUID;
                        aPayor.InsuranceName = GetInsuranceComapnyName(aIns.InsuranceCompanyUID);
                        aPayor.PAYRTPUID = aIns.PAYRTPUID;
                        aPayor.EligibileAmount = aIns.EligibleAmount;
                        aPayor.ActiveFrom = aIns.StartDttm;
                        aPayor.ActiveTo = aIns.EndDttm;
                        aPayor.ClaimPercentage = aIns.ClaimPercentage;
                        if (aPayor.ClaimPercentage == 0)
                            aPayor.ClaimPercentage = null;
                        aPayor.FixedCopayAmount = aIns.FixedCopayAmount;
                        if (aPayor.FixedCopayAmount == 0)
                            aPayor.FixedCopayAmount = null;
                        aPayor.PayorName = GetPayorDetailName(aIns.PayorDetailUID.Value);
                        PatientVisitPayorList.Add(aPayor);
                    }
                }
            }
            else
            {
                var billconfiguration = DataService.Billing.GetBillConFiguration(AppUtil.Current.OwnerOrganisationUID);
                if (billconfiguration != null)
                {
                    if (billconfiguration.InsuranceCompanyUID != null && billconfiguration.InsuranceCompanyUID > 0)
                    {
                        PatientVisitPayorModel defaultPatientVisitPayor = new PatientVisitPayorModel();
                        defaultPatientVisitPayor.PatientUID = Patient.PatientUID;
                        defaultPatientVisitPayor.ActiveFrom = DateTime.Now;
                        defaultPatientVisitPayor.PAYRTPUID = PayorTypes.FirstOrDefault(p => p.ValueCode == "PRIMARY").Key;
                        defaultPatientVisitPayor.PayorType = PayorTypes.FirstOrDefault(p => p.ValueCode == "PRIMARY").Display;
                        defaultPatientVisitPayor.InsuranceCompanyUID = billconfiguration.InsuranceCompanyUID;
                        defaultPatientVisitPayor.InsuranceName = GetInsuranceComapnyName(billconfiguration.InsuranceCompanyUID ?? 0);
                        defaultPatientVisitPayor.PayorAgreementUID = billconfiguration.PayorAgreementUID ?? 0;
                        var agreement = GetAgreement(billconfiguration.PayorAgreementUID ?? 0);
                        if (agreement != null)
                        {
                            defaultPatientVisitPayor.AgreementName = agreement.Name;
                            defaultPatientVisitPayor.PolicyMasterUID = agreement.PolicyMasterUID;
                            defaultPatientVisitPayor.PolicyName = agreement.PolicyName;
                        }

                        defaultPatientVisitPayor.ClaimPercentage = 0;
                        defaultPatientVisitPayor.FixedCopayAmount = 0;
                        defaultPatientVisitPayor.PayorDetailUID = (billconfiguration.PayorUID ?? 0);
                        defaultPatientVisitPayor.PayorName = GetPayorDetailName(billconfiguration.PayorUID ?? 0);

                        PatientVisitPayorList.Add(defaultPatientVisitPayor);
                    }
                }
            }
        }

        void AddVisitPayor()
        {
            if (PatientVisitPayorList == null)
                PatientVisitPayorList = new ObservableCollection<PatientVisitPayorModel>();

            if (SelectInsuranceCompany == null || SelectInsuranceCompany.InsuranceCompanyUID == 0)
            {
                WarningDialog("กรุณาเลือก Payor");
                return;
            }
            if (SelectInsurancePlan == null || SelectInsurancePlan.PayorAgreementUID == 0)
            {
                WarningDialog("กรุณาเลือก Agreement");
                return;
            }

            if (ClaimPercentage != null && ClaimPercentage > 100)
            {
                WarningDialog("ClaimPercentage ไม่ถูกต้อง");
                return;
            }
            if (PatientVisitPayorList != null && PatientVisitPayorList.Count() > 0)
            {
                if (PatientVisitPayorList.Where(i => i.PAYRTPUID == SelectedPayorType.Key) != null && PatientVisitPayorList.Where(i => i.PAYRTPUID == SelectedPayorType.Key).Count() > 0)
                {
                    WarningDialog("PayorType ซ้ำ กรุณาตรวจสอบ");
                    return;
                }
            }

            if (CheckDataPresentInList(new PatientVisitPayorModel { PayorAgreementUID = SelectInsurancePlan.PayorAgreementUID, PayorDetailUID = SelectInsurancePlan.PayorDetailUID }))
            {
                WarningDialog("Payor ซ้ำ กรุณาตรวจสอบ");
                return;
            }

            PatientVisitPayorModel newPatientVisitPayor = new PatientVisitPayorModel();
            newPatientVisitPayor.PatientUID = Patient.PatientUID;
            newPatientVisitPayor.PAYRTPUID = SelectedPayorType.Key;
            newPatientVisitPayor.PayorType = SelectedPayorType.Display;
            newPatientVisitPayor.InsuranceCompanyUID = SelectInsuranceCompany.InsuranceCompanyUID;
            newPatientVisitPayor.InsuranceName = SelectInsuranceCompany.CompanyName;
            newPatientVisitPayor.PayorAgreementUID = SelectInsurancePlan.PayorAgreementUID;
            newPatientVisitPayor.AgreementName = SelectInsurancePlan.PayorAgreementName;
            newPatientVisitPayor.PayorDetailUID = SelectInsurancePlan.PayorDetailUID;
            newPatientVisitPayor.PayorName = GetPayorDetailName(SelectInsurancePlan.PayorDetailUID);
            var agreement = GetAgreement(newPatientVisitPayor.PayorAgreementUID);
            if (agreement != null)
            {
                newPatientVisitPayor.PolicyMasterUID = agreement.PolicyMasterUID;
                newPatientVisitPayor.PolicyName = agreement.PolicyName;
            }
            newPatientVisitPayor.Comment = Comments;
            newPatientVisitPayor.ClaimPercentage = ClaimPercentage;
            newPatientVisitPayor.FixedCopayAmount = FixedCopayAmount;
            newPatientVisitPayor.EligibileAmount = OPDCoverPerDay;

            PatientVisitPayorList.Add(newPatientVisitPayor);
            ClearControl();
        }
        void UpdateVisitPayor()
        {
            if (SelectedPatientVisitPayor != null)
            {
                if (SelectInsuranceCompany == null || SelectInsuranceCompany.InsuranceCompanyUID == 0)
                {
                    WarningDialog("กรุณาเลือก Payor");
                    return;
                }
                if (SelectInsurancePlan == null || SelectInsurancePlan.PayorAgreementUID == 0)
                {
                    WarningDialog("กรุณาเลือก Agreement");
                    return;
                }

                if (ClaimPercentage != null && ClaimPercentage > 100)
                {
                    WarningDialog("ClaimPercentage ไม่ถูกต้อง");
                    return;
                }
                if (PatientVisitPayorList != null && PatientVisitPayorList.Count() > 0)
                {
                    if (PatientVisitPayorList.Where(i => i.PAYRTPUID == SelectedPayorType.Key) != null && PatientVisitPayorList.Where(i => i.PAYRTPUID == SelectedPayorType.Key).Count() > 0)
                    {
                        WarningDialog("PayorType ซ้ำ กรุณาตรวจสอบ");
                        return;
                    }
                }

                if (CheckDataPresentInList(new PatientVisitPayorModel { PayorAgreementUID = SelectInsurancePlan.PayorAgreementUID, PayorDetailUID = SelectInsurancePlan.PayorDetailUID }))
                {
                    WarningDialog("Payor ซ้ำ กรุณาตรวจสอบ");
                    return;
                }

                SelectedPatientVisitPayor.PAYRTPUID = SelectedPayorType.Key;
                SelectedPatientVisitPayor.PayorType = SelectedPayorType.Display;
                SelectedPatientVisitPayor.InsuranceCompanyUID = SelectInsuranceCompany.InsuranceCompanyUID;
                SelectedPatientVisitPayor.InsuranceName = SelectInsuranceCompany.CompanyName;
                SelectedPatientVisitPayor.PayorAgreementUID = SelectInsurancePlan.PayorAgreementUID;
                SelectedPatientVisitPayor.AgreementName = SelectInsurancePlan.PayorAgreementName;
                SelectedPatientVisitPayor.PayorDetailUID = SelectInsurancePlan.PayorDetailUID;
                SelectedPatientVisitPayor.PayorName = GetPayorDetailName(SelectInsurancePlan.PayorDetailUID);
                var agreement = GetAgreement(SelectedPatientVisitPayor.PayorAgreementUID);
                if (agreement != null)
                {
                    SelectedPatientVisitPayor.PolicyMasterUID = agreement.PolicyMasterUID;
                    SelectedPatientVisitPayor.PolicyName = agreement.PolicyName;
                }
                SelectedPatientVisitPayor.Comment = Comments;
                SelectedPatientVisitPayor.ClaimPercentage = ClaimPercentage;
                SelectedPatientVisitPayor.FixedCopayAmount = FixedCopayAmount;
                SelectedPatientVisitPayor.EligibileAmount = OPDCoverPerDay;
                ClearControl();
            }
        }

        void DeleteVisitPayor()
        {
            if (SelectedPatientVisitPayor != null)
            {

                PatientVisitPayorList.Remove(SelectedPatientVisitPayor);
                ClearControl();
            }
        }

        void ClearControl()
        {
            if (PatientVisitPayorList != null && PatientVisitPayorList.Count() > 0 && PayorTypes != null)
                SelectedPayorType = (from p in PayorTypes where (!(from q in PatientVisitPayorList select q.PAYRTPUID).Contains(p.Key)) select p).FirstOrDefault();
            else
                SelectedPayorType = PayorTypes.FirstOrDefault(p => p.ValueCode == "PRIMARY");

            SelectInsuranceCompany = null;
            SelectInsurancePlan = null;
            Comments = null;
            OPDCoverPerDay = null;
            FixedCopayAmount = null;
            ClaimPercentage = null;
            ActiveFrom = DateTime.Now;
            ActiveTo = null;
            InsurancePlans = new List<InsurancePlanModel>();
        }

        void SavePatientVisit()
        {
            if (ValidateVisitData())
            {
                return;
            }
            PatientVisitModel visitInfo = new PatientVisitModel();
            visitInfo.StartDttm = DateTime.Parse(StartDate.ToString("dd/MM/yyyy") + " " + StartTime.ToString("HH:mm"));
            visitInfo.PatientUID = Patient.PatientUID;
            visitInfo.VISTYUID = SelectedVisitType.Key;
            visitInfo.VISTSUID = 417;

            if (UseReadCard && Booking==null && Patient.PatientUID != 0)
            {
                var Bookings = DataService.PatientIdentity.SearchBookingNotExistsVisit(DateTime.Now, DateTime.Now, null, Patient.PatientUID, 2944, null, AppUtil.Current.OwnerOrganisationUID);
                if (Bookings != null && Bookings.Count > 0)
                {
                    string reminderMessage = Bookings.FirstOrDefault().PatientReminderMessage;
                    MessageBoxResult result = QuestionDialog("ผู้ป่วยมีนัด " + reminderMessage + " วันนี้ คุณต้องการดึงนัดมาลงทะเบียน หรือไม่?");
                    if (result == MessageBoxResult.Yes)
                    {
                        Booking = Bookings.FirstOrDefault();
                    }
                }
            }

            if (Booking != null)
                visitInfo.BookingUID = Booking.BookingUID; //Appointment

            visitInfo.PRITYUID = SelectedPriority.Key;
            visitInfo.Comments = CommentDoctor;
            visitInfo.OwnerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
            visitInfo.CheckupJobUID = SelectedCheckupJob != null ? SelectedCheckupJob.CheckupJobContactUID : (int?)null;
            if (SelectedCareprovider != null)
                visitInfo.CareProviderUID = SelectedCareprovider.CareproviderUID;
            visitInfo.PatientVisitPayors = PatientVisitPayorList.ToList();
            PatientVisitModel returnData = DataService.PatientIdentity.SavePatientVisit(visitInfo, AppUtil.Current.UserID);
            if (string.IsNullOrEmpty(returnData.VisitID))
            {
                ErrorDialog("ไม่สามารถบันทึกข้อมูล Visit คนไข้ได้ ติดต่อ Admin");
                return;
            }
            else
            {
                DataService.PatientIdentity.ManagePatientInsuranceDetail(PatientVisitPayorList.ToList());
                if (Booking != null)
                {
                    DataService.PatientIdentity.UpdateBookingArrive(Booking.BookingUID, AppUtil.Current.UserID);
                }
            }

            var parent = ((System.Windows.Controls.UserControl)this.View).Parent;
            if (parent != null && parent is System.Windows.Window)
            {
                CloseViewDialog(ActionDialog.Save);
            }
        }

        string GetInsuranceComapnyName(int insuranceComapnyUID)
        {
            string name = string.Empty;

            var insuranceCompany = DataService.Billing.GetInsuranceCompanyByUID(insuranceComapnyUID);
            if (insuranceCompany != null)
            {
                name = insuranceCompany.CompanyName;
            }

            return name;
        }

        private bool CheckDataPresentInList(PatientVisitPayorModel inputPayor)
        {
            bool ret = false;
            if (PatientVisitPayorList != null && PatientVisitPayorList.Count > 0)
            {
                foreach (PatientVisitPayorModel payor in PatientVisitPayorList)
                {
                    if (payor.PayorDetailUID == inputPayor.PayorDetailUID
                        && payor.PayorAgreementUID == inputPayor.PayorAgreementUID)
                    {
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }

        string GetPayorDetailName(int payorDetailUID)
        {
            string name = string.Empty;

            var payorDetail = DataService.Billing.GetPayorDetailByUID(payorDetailUID);
            if (payorDetail != null)
            {
                name = payorDetail.PayorName;
            }

            return name;
        }

        PayorAgreementModel GetAgreement(int agreeementUID)
        {

            var payorAgreement = DataService.Billing.GetPayorAgreementByUID(agreeementUID);
            return payorAgreement;

        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public bool ValidateVisitData()
        {
            if (SelectOrganisation == null)
            {
                WarningDialog("กรุณาเลือก สถานประกอบการ");
                return true;
            }
            if (SelectedVisitType == null)
            {
                WarningDialog("กรุณาเลือก ประเภท Visit");
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

            if (SelectedPatientVisitPayor == null)
            {
                WarningDialog("กรุณาใส่ข้อมูล Payor");
                return true;
            }

            return false;
        }
        #endregion
    }
}
