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
    public class AdmissionDetailViewModel : MediTechViewModelBase
    {
        #region property

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
                    PatientVisitModel visitInfoNonClose = DataService.PatientIdentity.GetLatestPatientVisitToConvert(_SelectedPateintSearch.PatientUID);

                    if (IsSearchAll != true)
                    {
                        SelectPatientVisit = visitInfoNonClose != null ? visitInfoNonClose : null;
                        IsLatestVisit = true;
                    }
                    else
                    {
                        if (visitInfoNonClose != null)
                        {
                            SelectPatientVisit = visitInfoNonClose;
                        }
                        else
                        {
                            PatientVisitModel patientVisit = new PatientVisitModel();
                            patientVisit.PatientID = SelectedPateintSearch.PatientID;
                            patientVisit.PatientUID = SelectedPateintSearch.PatientUID;

                            SelectPatientVisit = patientVisit;
                        }
                    }
                }
            }
        }

        private bool _IsSearchAll;
        public bool IsSearchAll
        {
            get { return _IsSearchAll; }
            set { _IsSearchAll = value; }
        }

        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
            }
        }

        #endregion
        
        private bool _IsRequestAdmit;
        public bool IsRequestAdmit
        {
            get { return _IsRequestAdmit; }
            set { Set(ref _IsRequestAdmit, value); }
        }

        private long _IpBookingUID;
        public long IpBookingUID
        {
            get { return _IpBookingUID; }
            set { Set(ref _IpBookingUID, value); }
        }

        private DateTime? _DischargeDate;
        public DateTime? DischargeDate
        {
            get { return _DischargeDate; }
            set
            {
                Set(ref _DischargeDate, value);
                if (DischargeDate != null)
                {
                   
                }
            }
        }

        private DateTime _ExpectedAdmission;
        public DateTime ExpectedAdmission
        {
            get { return _ExpectedAdmission; }
            set
            {
                Set(ref _ExpectedAdmission, value);

                DischargeDate = ExpectedAdmission.AddDays(LenghtofDay);
            }
        }

        private int _LenghtofDay;
        public int LenghtofDay
        {
            get { return _LenghtofDay; }
            set
            {
                Set(ref _LenghtofDay, value);

                DischargeDate = ExpectedAdmission.AddDays(LenghtofDay);

            }
        }

        private bool _IsWardView;
        public bool IsWardView
        {
            get { return _IsWardView; }
            set { Set(ref _IsWardView, value); }
        }

        private List<LocationModel> _ListWard;
        public List<LocationModel> ListWard
        {
            get { return _ListWard; }
            set { Set(ref _ListWard, value); }
        }

        private LocationModel _SelectedListBed;
        public LocationModel SelectedListBed
        {
            get { return _SelectedListBed; }
            set { Set(ref _SelectedListBed, value); }
        }

        private List<LookupReferenceValueModel> _Classification;
        public List<LookupReferenceValueModel> Classification
        {
            get { return _Classification; }
            set { Set(ref _Classification, value); }
        }

        private LookupReferenceValueModel _SelectClassification;
        public LookupReferenceValueModel SelectClassification
        {
            get { return _SelectClassification; }
            set { Set(ref _SelectClassification, value); }
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

        private bool _IsLatestVisit;
        public bool IsLatestVisit
        {
            get { return _IsLatestVisit; }
            set { _IsLatestVisit = value; }
        }

        private bool _IsReAdmission;
        public bool IsReAdmission
        {
            get { return _IsReAdmission; }
            set { _IsReAdmission = value; }
        }


        private List<LocationModel> _ward;
        public List<LocationModel> Ward
        {
            get { return _ward; }
            set { Set(ref _ward, value); }
        }

        private LocationModel _SelectWard;
        public LocationModel SelectWard
        {
            get { return _SelectWard; }
            set
            {
                Set(ref _SelectWard, value);
                if (_SelectWard != null)
                {
                    ListWard = DataService.PatientIdentity.GetBedLocation(SelectWard.LocationUID, null).Where(p => p.BedIsUse == "N").ToList();
                }
            }
        }

        public List<LocationModel> Location { get; set; }


        private LocationModel _SelectLocation;
        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set { Set(ref _SelectLocation, value); }
        }

        private List<CareproviderModel> _Doctors;
        public List<CareproviderModel> Doctors
        {
            get { return _Doctors; }
            set { Set(ref _Doctors, value); }
        }

        private CareproviderModel _SelectDoctor;
        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { Set(ref _SelectDoctor, value); }
        }

        private List<CareproviderModel> _SecondDoctors;
        public List<CareproviderModel> SecondDoctors
        {
            get { return _SecondDoctors; }
            set { Set(ref _SecondDoctors, value); }
        }

        private CareproviderModel _SelectSecondDoctors;
        public CareproviderModel SelectSecondDoctors
        {
            get { return _SelectSecondDoctors; }
            set { Set(ref _SelectSecondDoctors, value); }
        }

        private ObservableCollection<CareproviderModel> _SecondDoctorsSource;
        public ObservableCollection<CareproviderModel> SecondDoctorsSource
        {
            get { return _SecondDoctorsSource ?? (_SecondDoctorsSource = new ObservableCollection<CareproviderModel>()); }
           
            set { Set(ref _SecondDoctorsSource, value); }
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
                Set(ref _SelectedProblemSearch, value);
                if (_SelectedProblemSearch != null)
                {
                }
            }
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

        private List<SpecialityModel> _SpecialitySource;
        public List<SpecialityModel> SpecialitySource
        {
            get { return _SpecialitySource; }
            set { Set(ref _SpecialitySource, value); }
        }

        private SpecialityModel _SelectSpeciality;
        public SpecialityModel SelectSpeciality
        {
            get { return _SelectSpeciality; }
            set
            {
                Set(ref _SelectSpeciality, value);
            }
        }

        private List<BillPackageModel> _ChargablePackage;
        public List<BillPackageModel> ChargablePackage
        {
            get { return _ChargablePackage; }
            set { Set(ref _ChargablePackage, value); }
        }

        private BillPackageModel _SelectChargablePackage;
        public BillPackageModel SelectChargablePackage
        {
            get { return _SelectChargablePackage; }
            set
            {
                Set(ref _SelectChargablePackage, value);
            }
        }

        #endregion

        #region command

        private RelayCommand _PatientSearchCommand;
        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _ModifyPayorCommand;
        public RelayCommand ModifyPayorCommand
        {
            get { return _ModifyPayorCommand ?? (_ModifyPayorCommand = new RelayCommand(ModifyPayorDetail)); }
        }

        private RelayCommand _AddSecondDoctorCommand;
        public RelayCommand AddSecondDoctorCommand
        {
            get { return _AddSecondDoctorCommand ?? (_AddSecondDoctorCommand = new RelayCommand(AddSecondDoctor)); }
        }

        private RelayCommand _DeleteSecondDoctorCommand;
        public RelayCommand DeleteSecondDoctorCommand
        {
            get { return _DeleteSecondDoctorCommand ?? (_DeleteSecondDoctorCommand = new RelayCommand(DeleteSecondDoctor)); }
        }

        private RelayCommand _SaveAdmitCommand;
        public RelayCommand SaveAdmitCommand
        {
            get { return _SaveAdmitCommand ?? (_SaveAdmitCommand = new RelayCommand(SaveAdmit)); }
        }

        private RelayCommand _PatientSearchProblemCommand;
        public RelayCommand PatientSearchProblemCommand
        {
            get { return _PatientSearchProblemCommand ?? (_PatientSearchProblemCommand = new RelayCommand(ProblemSearch)); }
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(Close)); }
        }

        #endregion

        #region method
        PatientVisitModel visitModel;
        List<CareproviderModel> secondDoctor = new List<CareproviderModel>();
        public AdmissionDetailViewModel()
        {
            DateTime now = DateTime.Now;
            ExpectedAdmission = now;
            LenghtofDay = 1;

            var locationAll = DataService.IPDService.GetBedALL();
            //BillingCatagory = DataService.Technical.GetReferenceValueMany("TARIFF");
            BedCatagory = DataService.Technical.GetReferenceValueMany("BEDCAT");
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            SecondDoctors = DataService.UserManage.GetCareproviderDoctor();
            var test = DataService.MasterData.GetHealthOrganisation();
            Location = locationAll;
            Ward = locationAll.Where(w=>w.LOTYPUID == 3152).ToList();
            SpecialitySource = DataService.MasterData.GetSpecialityAll();
            var roomcharge = DataService.MasterData.GetOrderCategory().Where(p => p.Name == "ค่าห้อง").FirstOrDefault();
            var Subroomcharge = DataService.MasterData.GetOrderSubCategoryByUID(roomcharge.OrderCategoryUID).FirstOrDefault();
            ChargablePackage = DataService.Billing.SearchBillPackage("", "", roomcharge.OrderCategoryUID, Subroomcharge.OrderSubCategoryUID);
            Classification = DataService.Technical.GetReferenceValueMany("CRCLS");
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


        private void ModifyPayorDetail()
        {
            if (SelectPatientVisit != null && SelectPatientVisit.PatientVisitUID != 0)
            {
                ModifyVisitPayor pageview = new ModifyVisitPayor();
                (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(SelectPatientVisit);
                ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                }
            }
        }

        private void AddSecondDoctor()
        {
            if(SelectSecondDoctors != null)
            {
                if(SelectDoctor != null)
                {
                    if (SelectSecondDoctors.CareproviderUID == SelectDoctor.CareproviderUID)
                    {
                        WarningDialog("มีรายการแพทย์หลักแล้ว กรุณาเลือกแพทย์ท่านอื่น");
                        return;
                    }
                }

                var data = SecondDoctorsSource.FirstOrDefault(p => p.CareproviderUID == SelectSecondDoctors.CareproviderUID);
                if (data != null)
                {
                    WarningDialog("มีรายการแพทย์ท่านนี้แล้ว กรุณาเลือกแพทย์ท่านอื่น");
                    return;
                }
                SelectSecondDoctors.VISTYUID = DataService.Technical.GetReferenceValueByCode("CONTYP", "SCNDCONS").Key ?? 0;
                secondDoctor.Add(SelectSecondDoctors);
                SecondDoctorsSource = new ObservableCollection<CareproviderModel>(secondDoctor);

                SelectSecondDoctors = null;
            }
        }

        private void DeleteSecondDoctor()
        {
            if (SelectSecondDoctors != null)
            {
                SecondDoctorsSource.Remove(SelectSecondDoctors);
            }
        }

        private void Close()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void SaveAdmit()
        {
            try
            {
                if (SelectPatientVisit == null)
                {
                    WarningDialog("กรุณาเลือกคนไข้");
                    return;
                }

                //if (SelectBillCatagory == null)
                //{
                //    WarningDialog("กรุณาใส่ Billing Catagory");
                //    return;
                //}

                if (SelectWard == null)
                {
                    WarningDialog("กรุณาใส่ Ward");
                    return;
                }

                if (SelectedListBed == null)
                {
                    WarningDialog("กรุณาใส่ เตียง");
                    return;
                }
                if (SelectDoctor == null )
                {
                    WarningDialog("กรุณา เลือกแพทย์หลัก");
                    return;
                }

                AssignPropertieToModel();

                if (visitModel.PatientVisitUID != 0)
                {
                    SaveSuccessDialog("Patient OP visit will be converted to IP visit");
                }

                PatientVisitModel returnData = DataService.PatientIdentity.SaveIPDPatientVisit(visitModel, AppUtil.Current.UserID);

                if(IsRequestAdmit == true)
                {
                    int status = DataService.Technical.GetReferenceValueByCode("BKTYP", "ADMIT").Key ?? 0;
                    DataService.PatientIdentity.ChangeStatusIPBooking(IpBookingUID, status, AppUtil.Current.UserID);
                }

                SaveSuccessDialog("HN : " + returnData.PatientID + " Admitted Successfully");

                var patientVisitPayors = DataService.PatientIdentity.GetPatientVisitPayorByVisitUID(returnData.PatientVisitUID);
                if (patientVisitPayors.Count == 0)
                {
                    MessageBoxResult resultDiaglog = QuestionDialog("ยังไม่มี Payor Visit ต้องการ Modify Payor Visit หรือไม่");

                    if (resultDiaglog == MessageBoxResult.Yes)
                    {
                        SelectPatientVisit.PatientVisitUID = returnData.PatientVisitUID;
                        ModifyVisitPayor pageview = new ModifyVisitPayor();
                        (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(SelectPatientVisit);
                        ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            SaveSuccessDialog();
                        }
                        //continue;
                    }
                }
                
                if (IsRequestAdmit != true)
                {
                    WardView pageto = new WardView();
                    ChangeViewPermission(pageto);
                    CloseViewDialog(ActionDialog.Save);
                }
                else
                {
                    CloseViewDialog(ActionDialog.Save);
                }
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }

        public void AssignPropertieToModel()
        {
            if(visitModel == null)
                visitModel = new PatientVisitModel();

            visitModel.PatientUID = SelectPatientVisit.PatientUID != 0 ? SelectPatientVisit.PatientUID : SelectedPateintSearch.PatientUID;
            visitModel.PatientVisitUID = SelectPatientVisit.PatientVisitUID != 0 ? SelectPatientVisit.PatientVisitUID : 0;
            visitModel.VISTSUID = 417;
            //visitModel.PRITYUID = SelectBillCatagory.Key;
            visitModel.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            visitModel.ENTYPUID = DataService.Technical.GetReferenceValueByCode("ENTYP", "INPAT").Key;
            visitModel.LocationUID = SelectWard.LocationUID;
            visitModel.BedUID = SelectedListBed.LocationUID;
            visitModel.IsReAdmisstion = IsReAdmission == true ? "Y" : null;
            visitModel.SpecialityUID = SelectSpeciality != null ? SelectSpeciality.SpecialityUID : (int?)null;
            visitModel.CareProviderUID =  SelectDoctor.CareproviderUID;
            visitModel.ENSTAUID = DataService.Technical.GetReferenceValueByCode("ENSTA", "PEADMT").Key;

            if (SecondDoctorsSource.Count != 0)
            {
                List<CareproviderModel> secondDoctor = new List<CareproviderModel>(SecondDoctorsSource);
                visitModel.SecondCareprovider = secondDoctor;
            }

            visitModel.AdmissionEvent = new AdmissionEventModel();
            visitModel.AdmissionEvent.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
            visitModel.AdmissionEvent.CarepoviderUID =  SelectDoctor.CareproviderUID;
            visitModel.AdmissionEvent.ExpectedLengthOfStay = LenghtofDay;
            visitModel.AdmissionEvent.AdmissionDttm = ExpectedAdmission;
            visitModel.AdmissionEvent.ExpectedDischargeDttm = DischargeDate;
        }

        public void ConfirmFromRequestAdmission(IPBookingModel datarequest)
        {
            IsRequestAdmit = true;
            IpBookingUID = datarequest.IPBookingUID;
            if (datarequest.LocationUID != null)
            {
                SelectPatientVisit = DataService.PatientIdentity.GetPatientVisitByUID(datarequest.PatientVisitUID ?? 0);
                visitModel = SelectPatientVisit;
                AssignModelToProperties(datarequest);
            }
        }

        public void AssignModelToProperties(IPBookingModel iPBooking)
        {
            SelectWard = iPBooking.LocationUID != null ? Ward.FirstOrDefault(p => p.LocationUID == iPBooking.LocationUID) : null;
            LenghtofDay = iPBooking.ExpectedLengthofStay ?? 1;
            ExpectedAdmission = iPBooking.AdmissionDttm;
            DischargeDate = iPBooking.ExpectedDischargeDttm;
            SelectDoctor =  Doctors.FirstOrDefault(p => p.CareproviderUID == iPBooking.CareproviderUID);
            SelectSpeciality = iPBooking.SpecialityUID != null ? SpecialitySource.FirstOrDefault(p => p.SpecialityUID == iPBooking.SpecialityUID) : null;
            SelectedListBed = iPBooking.BedUID != null ? ListWard.FirstOrDefault(p => p.LocationUID == iPBooking.BedUID) : null;
        }

        public void SendbedWard(BedStatusModel bedModel)
        {
            SelectWard = Ward.FirstOrDefault(p => p.LocationUID == bedModel.ParentLocationUID);
            SelectedListBed = ListWard.FirstOrDefault(p => p.LocationUID == bedModel.LocationUID);
            
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