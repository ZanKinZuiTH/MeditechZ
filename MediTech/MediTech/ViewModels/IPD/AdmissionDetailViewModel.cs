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
                    var insurancePlan = DataService.Billing.GetInsurancePlans(_SelectInsuranceCompany.InsuranceCompanyUID);
                }
            }
        }


        private List<HealthOrganisationModel> _Organisations;

        public List<HealthOrganisationModel> Organisations
        {
            get { return _Organisations; }
            set { Set(ref _Organisations, value); }
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


        private DateTime? _ExpectedAdmission;
        public DateTime? ExpectedAdmission
        {
            get { return _ExpectedAdmission; }
            set
            {
                Set(ref _ExpectedAdmission, value);

            }
        }



        private string _LenghtofDay;
        public string LenghtofDay
        {
            get { return _LenghtofDay; }
            set
            {
                Set(ref _LenghtofDay, value);

                double OutVal;
                double.TryParse(LenghtofDay, out OutVal);
                DischargeDate = ExpectedAdmission?.AddDays(OutVal);

            }
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




        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }


        private PatientVisitModel _PatientMed;
        public PatientVisitModel PatientMed
        {
            get { return _PatientMed; }
            set { Set(ref _PatientMed, value); }
        }


        private List<LookupReferenceValueModel> _BillingCatagory;

        public List<LookupReferenceValueModel> BillingCatagory
        {
            get { return _BillingCatagory; }
            set { Set(ref _BillingCatagory, value); }
        }

        private LookupReferenceValueModel _SelectBillCatagory;

        public LookupReferenceValueModel SelectBillCatagory
        {
            get { return _SelectBillCatagory; }
            set { Set(ref _SelectBillCatagory, value); }
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
                    if (PatientVisit != null)
                    {
                        SearchPatientVisit();
                    }
                    
                }
            }
        }

        private List<CheckupJobContactModel> _CheckupJobSource;

        public List<CheckupJobContactModel> CheckupJobSource
        {
            get { return _CheckupJobSource; }
            set { Set(ref _CheckupJobSource, value); }
        }

        private List<PayorAgreementModel> _PayorAgreementSource;

        public List<PayorAgreementModel> PayorAgreementSource
        {
            get { return _PayorAgreementSource; }
            set { Set(ref _PayorAgreementSource, value); }
        }





        public List<LocationModel> Bed { get; set; }


        private LocationModel _SelectBed;

        public LocationModel SelectBed
        {
            get { return _SelectBed; }
            set { _SelectBed = value; }
        }


        private List<LocationModel> _ward;
        public List<LocationModel> Ward
        {
            get { return _ward; }
            set { _ward = value; }
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
            set { _SelectLocation = value; }
        }





        public List<CareproviderModel> Doctors { get; set; }


        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { _SelectDoctor = value; }
        }



        private PayorAgreementModel _SelectedPayorAgreement;

        public PayorAgreementModel SelectedPayorAgreement
        {
            get { return _SelectedPayorAgreement; }
            set { Set(ref _SelectedPayorAgreement, value); }
        }

        private CheckupJobContactModel _SelectedCheckupJob;

        public CheckupJobContactModel SelectedCheckupJob
        {
            get { return _SelectedCheckupJob; }
            set { Set(ref _SelectedCheckupJob, value); }
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


        private PatientVisitModel _PatientVisit;

        public PatientVisitModel PatientVisit
        {
            get { return _PatientVisit; }
            set { Set(ref _PatientVisit, value); }
        }

        #endregion


        #endregion

        #region command

        private RelayCommand _SaveAdmitCommand;

        public RelayCommand SaveAdmitCommand
        {
            get { return _SaveAdmitCommand ?? (_SaveAdmitCommand = new RelayCommand(SaveAdmit)); }
        }

        #endregion

        #region method

    

        public AdmissionDetailViewModel()
        {
            DateTime now = DateTime.Now;
            var locationAll = DataService.IPDService.GetBedALL();
            BillingCatagory = DataService.Technical.GetReferenceValueMany("TARIFF");
            BedCatagory = DataService.Technical.GetReferenceValueMany("BEDCAT");
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            var test = DataService.MasterData.GetHealthOrganisation();
            Location = locationAll;
            Ward = locationAll.Where(w=>w.LOTYPUID == 3152).ToList();
          
            StartDate = now.Date;
            StartTime = now;
        }

        private void SearchPatientVisit()
        {
            long UIDPatient;
            if (SearchPatientCriteria != "" && SelectedPateintSearch != null)
            {
                UIDPatient = SelectedPateintSearch.PatientUID;
                var patientInfo = DataService.PatientIdentity.GetPatientByUID(UIDPatient);
                (this.View as AdmissionDetail).patientBanner.SetPatientBanner(patientInfo.PatientUID,0);
            }


        }


        private void ByPatientVisit(long UIDPatient)
        {
            //if (UIDPatient != null)
            //{
            //    var patientInfo = DataService.PatientIdentity.GetPatientByUID(UIDPatient);
            //    (this.View as AdmissionDetail).patientBanner.SetPatientBanner(patientInfo.PatientUID, 0);
            //}


        }



        void Close()
        {
            try
            {
                this.CloseViewDialog(ActionDialog.Cancel);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }





        void SavePatientVisit()
        {
           
            PatientVisitModel visitInfo = new PatientVisitModel();
            visitInfo.StartDttm = DateTime.Parse(StartDate.ToString("dd/MM/yyyy") + " " + StartTime.ToString("HH:mm"));
            visitInfo.PatientUID = SelectedPateintSearch.PatientUID;
            visitInfo.VISTYUID = 430;
            visitInfo.VISTSUID = 417;
            //visitInfo.BookingUID = Booking.BookingUID; //Appointment
            //visitInfo.PRITYUID = SelectedPriority.Key;
            visitInfo.PRITYUID = 144;
            visitInfo.Comments = "test";
            visitInfo.OwnerOrganisationUID = 17;
            visitInfo.CheckupJobUID = SelectedCheckupJob != null ? SelectedCheckupJob.CheckupJobContactUID : (int?)null;
            if (SelectDoctor!= null)
                visitInfo.CareProviderUID = SelectDoctor.CareproviderUID;
            visitInfo.PatientVisitPayors = null;
            PatientVisitModel returnData = DataService.PatientIdentity.SavePatientVisit(visitInfo, AppUtil.Current.UserID);
            if (string.IsNullOrEmpty(returnData.VisitID))
            {
                ErrorDialog("ไม่สามารถบันทึกข้อมูล Visit คนไข้ได้ ติดต่อ Admin");
                return;
            }
            //else
            //{
            //    DataService.PatientIdentity.ManagePatientInsuranceDetail(PatientVisitPayorList.ToList());
            //    if (Booking != null)
            //    {
            //        DataService.PatientIdentity.UpdateBookingArrive(Booking.BookingUID, AppUtil.Current.UserID);
            //    }
            //}

            var parent = ((System.Windows.Controls.UserControl)this.View).Parent;
            if (parent != null && parent is System.Windows.Window)
            {
                CloseViewDialog(ActionDialog.Save);
            }
        }


        //void SaveAdmit()
        //{
        //    try
        //    {

        //        //SavePatientVisit();
        //       // DataService.PatientIdentity.SavePatientVisit(, AppUtil.Current.UserID);
        //        CloseViewDialog(ActionDialog.Save);
        //        WardView pageview = new WardView();
        //        (pageview.DataContext as WardViewModel).Eventlog();
        //        WardViewModel result = (WardViewModel)LaunchViewDialogNonPermiss(pageview, false);


        //    }
        //    catch (Exception ex)
        //    {

        //        ErrorDialog(ex.Message);
        //    }

        //}

        public void SaveAdmit()
        {

            try
            {


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
                    WarningDialog("กรุณา เลือกแพทย์");
                    return;
                }

                PatientVisitModel visitInfo = new PatientVisitModel();
                visitInfo.StartDttm = DateTime.Parse(StartDate.ToString("dd/MM/yyyy") + " " + StartTime.ToString("HH:mm"));
                visitInfo.PatientUID = SelectedPateintSearch.PatientUID;
                visitInfo.VISTYUID = DataService.Technical.GetReferenceValueByCode("VISTY", "IPD").Key; ; //visit type IPD
                visitInfo.VISTSUID = 417; //Registered
                //visitInfo.BookingUID = Booking.BookingUID; //Appointment
                //visitInfo.PRITYUID = SelectedPriority.Key;
                visitInfo.PRITYUID = 1122; // เลขอาไรหว่า ใส่ 1122 ไปก่อน
                visitInfo.Comments = "test iPD insert";
                visitInfo.OwnerOrganisationUID = 17; //รอเปลี่ยนใช้ของคลินิกไปก่อน
                visitInfo.ENTYPUID = DataService.Technical.GetReferenceValueByCode("ENTYP", "INPAT").Key;
                visitInfo.LocationUID = SelectWard.LocationUID;
                visitInfo.BedUID = SelectedListBed.LocationUID;
                PatientAEAdmissionModel aeAdmission = AssingVisitIPDToModel();
                visitInfo.AEAdmission = aeAdmission;
                visitInfo.CheckupJobUID = SelectedCheckupJob != null ? SelectedCheckupJob.CheckupJobContactUID : (int?)null;
                if (SelectDoctor != null)
                    visitInfo.CareProviderUID = SelectDoctor.CareproviderUID;
                visitInfo.PatientVisitPayors = null;
                PatientVisitModel returnData = DataService.PatientIdentity.SaveIPDPatientVisit(visitInfo, AppUtil.Current.UserID);

                SaveSuccessDialog("BN : " + returnData.VisitID);

                if (SelectWard != null)
                {
                    WardView pageto = new WardView();
                    ChangeViewPermission(pageto);
                    CloseViewDialog(ActionDialog.Save);
                }
                else
                {
                    WardView pageto = new WardView();
                    ChangeViewPermission(pageto);
                }
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }





        public PatientAEAdmissionModel AssingVisitIPDToModel()
        {
            PatientAEAdmissionModel visitErModel = new PatientAEAdmissionModel();

            visitErModel.LocationUid = SelectWard.LocationUID;
            visitErModel.CareproviderUID = SelectDoctor.CareproviderUID;

          //  visitErModel.InjuryReason = ReasonDetail;
          //  visitErModel.EmergencyExamDetail = EmergencyExamDetail;
           // visitErModel.VehicleNumber = VehicleNumber;
            visitErModel.EventOccuredDttm = DateTime.Parse(StartDate.ToString("dd/MM/yyyy") + " " + StartTime.ToString("HH:mm"));
           // visitErModel.PhoneNumber = SeconePhone;
           // visitErModel.MobileNumber = MobilePhone;
            //if (SelectedProvince != null)
            //    visitErModel.ProvinceUID = SelectedProvince.Key;

            return visitErModel;
        }


        public void TestC()
        {


        }

        public void ConfirmFromRequestAdmission(IPBookingModel datarequest)
        {
           
            if (datarequest.LocationUID != null)
            {
                if (datarequest.BedUID != null)
                {

                }
                Ward = DataService.Technical.GetLocation().Where(p => p.LocationUID == datarequest.LocationUID).ToList();
                ListWard = DataService.Technical.GetLocation().Where(p => p.LocationUID == datarequest.BedUID).ToList();
                SelectWard = ListWard.FirstOrDefault(p=> p.LocationUID == datarequest.LocationUID);
                //EncounterType = DataService.Technical.GetReferenceValueMany("ENTYP");
                //SelectEncounterType = EncounterType.FirstOrDefault(p => p.ValueCode == "INPAT");

            }
            var visitInfo = DataService.PatientIdentity.GetPatientVisitByUID(datarequest.PatientVisitUID ?? 0);
            SelectedPateintSearch = DataService.PatientIdentity.GetPatientByUID(datarequest.PatientUID);
            PatientVisit = visitInfo;
      
        
        }


        public void sendVisit(PatientVisitModel datavisit)
        {

           // ListWard = DataService.PatientIdentity.GetBedLocation((inforequest.BedUID ?? 0), null).Where(p => p.BedIsUse == "N").ToList();
            var visitInfo = DataService.PatientIdentity.GetPatientVisitByUID(datavisit.PatientVisitUID);
            PatientVisit = visitInfo;

        }

        public void SendbedWard(BedStatusModel resivebed)
        {
            //List<LocationModel> res = new List<LocationModel>();
            //Ward.Add(resivebed);

            int idlocation = (resivebed.ParentLocationUID ?? 0);
            ListWard = DataService.PatientIdentity.GetBedLocation(idlocation, null).Where(p => p.BedIsUse == "N" && p.LocationUID == resivebed.LocationUID).ToList();
            SelectedListBed = DataService.PatientIdentity.GetBedLocation(idlocation, null).Where(p => p.BedIsUse == "N" && p.LocationUID == resivebed.LocationUID).FirstOrDefault();
            //SelectWard = resivebed;

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