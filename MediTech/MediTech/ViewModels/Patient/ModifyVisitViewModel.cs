using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ModifyVisitViewModel : MediTechViewModelBase
    {
        #region Properties
        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }


        public List<LookupReferenceValueModel> PrioritySource { get; set; }
        private LookupReferenceValueModel _SelectedPriority;

        public LookupReferenceValueModel SelectedPriority
        {
            get { return _SelectedPriority; }
            set { Set(ref _SelectedPriority, value); }
        }

        private DateTime? _StartDate;

        public DateTime? StartDate
        {
            get { return _StartDate; }
            set { Set(ref _StartDate, value); }
        }

        private DateTime? _StartTime;
        public DateTime? StartTime
        {
            get { return _StartTime; }
            set { Set(ref _StartTime, value); }
        }

        //private List<LocationModel> _Locations;

        //public List<LocationModel> Locations
        //{
        //    get { return _Locations; }
        //    set { Set(ref _Locations, value); }
        //}

        //private LocationModel _SelectLocation;

        //public LocationModel SelectLocation
        //{
        //    get { return _SelectLocation; }
        //    set
        //    {
        //        Set(ref _SelectLocation, value);
        //    }
        //}
        
        private List<LookupReferenceValueModel> _VisitTypeSource;

        public List<LookupReferenceValueModel> VisitTypeSource
        {
            get { return _VisitTypeSource; }
            set { Set(ref _VisitTypeSource, value); }
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
                    VisibiltyEmployerAddress = Visibility.Collapsed;
                    VisibilityCareprovider2 = Visibility.Collapsed;
                    CareproviderLabel = "แพทย์";
                    if (SelectedVisitType.ValueCode == "MBCHK" || SelectedVisitType.ValueCode == "CHKUP" || SelectedVisitType.ValueCode == "CHKIN" || SelectedVisitType.ValueCode == "CHKIN5" || SelectedVisitType.ValueCode == "CHKIN6")
                    {
                        if (CheckupJobSource == null)
                        {
                            CheckupJobSource = DataService.Checkup.GetCheckupJobContactAllActive();
                        }
                        VisibiltyCheckupCompany = Visibility.Visible;
                    }
                    else
                    {
                        CheckupJobSource = null;
                    }
                    if (SelectedVisitType.ValueCode == "CHKIN4" || SelectedVisitType.ValueCode == "CHKIN5" || SelectedVisitType.ValueCode == "CHKIN6")
                    {
                        VisibilityCareprovider2 = Visibility.Visible;
                        CareproviderLabel = "แพทย์อาชีว";
                    }
                    if (SelectedVisitType.ValueCode == "CHKIN3")
                    {
                        VisibiltyEmployerAddress = Visibility.Visible;
                    }
                }
            }
        }


        private List<CareproviderModel> _CareproviderSource;

        public List<CareproviderModel> CareproviderSource
        {
            get { return _CareproviderSource; }
            set { Set(ref _CareproviderSource, value); }
        }

        private CareproviderModel _SelectedCareprovider;

        public CareproviderModel SelectedCareprovider
        {
            get { return _SelectedCareprovider; }
            set { Set(ref _SelectedCareprovider, value); }
        }

        private CareproviderModel _SelectedCareprovider2;

        public CareproviderModel SelectedCareprovider2
        {
            get { return _SelectedCareprovider2; }
            set { Set(ref _SelectedCareprovider2, value); }
        }

        private string _CareproviderLabel = "แพทย์";

        public string CareproviderLabel
        {
            get { return _CareproviderLabel; }
            set { Set(ref _CareproviderLabel, value); }
        }

        private string _CommentDoctor;

        public string CommentDoctor
        {
            get { return _CommentDoctor; }
            set { Set(ref _CommentDoctor, value); }
        }

        private string _Company;

        public string Company
        {
            get { return _Company; }
            set { Set(ref _Company, value); }
        }
        
        private string _EmployerAddress;

        public string EmployerAddress
        {
            get { return _EmployerAddress; }
            set { Set(ref _EmployerAddress, value); }
        }

        private bool _IsEnableFinancial;

        public bool IsEnableFinancial
        {
            get { return _IsEnableFinancial; }
            set { Set(ref _IsEnableFinancial, value); }
        }

        private Visibility _VisibilityCareprovider2 = Visibility.Collapsed;

        public Visibility VisibilityCareprovider2
        {
            get { return _VisibilityCareprovider2; }
            set { Set(ref _VisibilityCareprovider2, value); }
        }

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

        private Visibility _VisibiltyEmployerAddress = Visibility.Collapsed;

        public Visibility VisibiltyEmployerAddress
        {
            get { return _VisibiltyEmployerAddress; }
            set { Set(ref _VisibiltyEmployerAddress, value); }
        }
        #endregion

        #region Command


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }


        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        private RelayCommand _CancelVisitCommand;

        public RelayCommand CancelVisitCommand
        {
            get { return _CancelVisitCommand ?? (_CancelVisitCommand = new RelayCommand(CancelVisit)); }
        }

        #endregion

        #region Method

        public ModifyVisitViewModel()
        {
            List<LookupReferenceValueModel> dataLookupSource = DataService.Technical.GetReferenceValueList("VISTY,RQPRT");
            VisitTypeSource = dataLookupSource.Where(p => p.DomainCode == "VISTY").OrderBy(p => p.DisplayOrder).ToList();
            PrioritySource = dataLookupSource.Where(P => P.DomainCode == "RQPRT").OrderBy(p => p.DisplayOrder).ToList();
            //var LocationSource = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            //Locations = LocationSource.Where(p => p.IsRegistrationAllowed == "Y").ToList();
            CareproviderSource = DataService.UserManage.GetCareproviderDoctor();
        }
        private void Save()
        {
            try
            {
                if (SelectPatientVisit != null)
                {
                    if (ValidateVisitData())
                    {
                        return;
                    }

                    PatientVisitModel visitInfo = new PatientVisitModel();
                    visitInfo.PatientUID = SelectPatientVisit.PatientUID;
                    visitInfo.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
                    visitInfo.StartDttm = DateTime.Parse(StartDate.Value.ToString("dd/MM/yyyy") + " " + StartTime.Value.ToString("HH:mm"));
                    visitInfo.VISTYUID = SelectedVisitType.Key;
                    visitInfo.PRITYUID = SelectedPriority.Key;

                    visitInfo.CheckupJobUID = SelectedCheckupJob != null ? SelectedCheckupJob.CheckupJobContactUID : (int?)null;

                    visitInfo.Comments = CommentDoctor;
                    visitInfo.CompanyName = Company;
                    visitInfo.EmployerAddress = EmployerAddress;
                    //visitInfo.LocationUID = SelectLocation.LocationUID;
                    if (SelectedCareprovider != null)
                        visitInfo.CareProviderUID = SelectedCareprovider.CareproviderUID;

                    if (SelectedCareprovider2 != null && VisibilityCareprovider2 == Visibility.Visible)
                    {
                        visitInfo.CareProvider2UID = SelectedCareprovider2.CareproviderUID;
                    }

                    DataService.PatientIdentity.ModifyPatientVisit(visitInfo, AppUtil.Current.UserID);
                    CloseViewDialog(ActionDialog.Save);
                }

            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void CancelVisit()
        {
            try
            {
                if (SelectPatientVisit != null)
                {
                    var orderLists = DataService.OrderProcessing.GetOrderAllByVisitUID(SelectPatientVisit.PatientVisitUID);
                    if (orderLists != null)
                    {
                        var reviewOrder = orderLists.Where(p => p.OrderDetailStatus == "Reviewed");
                        var dispensedOrder = orderLists.Where(p => p.OrderDetailStatus == "Dispensed");
                        if (reviewOrder != null && reviewOrder.Count() > 0)
                        {
                            WarningDialog("มี Order สถานะ Reviewed กรุณาทำการ Cancel Order");
                            return;
                        }
                        if (dispensedOrder != null && dispensedOrder.Count() > 0)
                        {
                            WarningDialog("มี Order สถานะ Dispensed กรุณาทำการ ยกเลิกการจ่ายยา ที่หน้าใบสั่งยา");
                            return;
                        }
                    }

                    MessageBoxResult diagResult = QuestionDialog("คุณต้องการยกเลิกการละเบียนของผู้ป่วยคนนี้ใช้หรือไม่ ?");
                    if (diagResult == MessageBoxResult.Yes)
                    {
           
                        DataService.PatientIdentity.CancelVisit(SelectPatientVisit.PatientVisitUID, AppUtil.Current.UserID);
                        CloseViewDialog(ActionDialog.Save);
                    }

                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        private void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }
        public void AssingPatientVisit(PatientVisitModel visitModel)
        {
            SelectPatientVisit = visitModel;
            //SelectLocation = Locations.FirstOrDefault(p => p.LocationUID == SelectPatientVisit.LocationUID);
            SelectedVisitType = VisitTypeSource.FirstOrDefault(p => p.Key == SelectPatientVisit.VISTYUID);
            StartDate = SelectPatientVisit.StartDttm != null ? SelectPatientVisit.StartDttm.Value : (DateTime?)null;
            StartTime = SelectPatientVisit.StartDttm != null ? SelectPatientVisit.StartDttm.Value : (DateTime?)null;

            SelectedCheckupJob = CheckupJobSource?.FirstOrDefault(p => p.CheckupJobContactUID == SelectPatientVisit.CheckupJobUID);
            SelectedPriority = PrioritySource.FirstOrDefault(p => p.Key == SelectPatientVisit.PRITYUID);
            SelectedCareprovider = CareproviderSource.FirstOrDefault(p => p.CareproviderUID == SelectPatientVisit.CareProviderUID);

            if (visitModel.VISTYUID == 4867) //ใบรับรองแพทย์อับอากาศ
            {
                var careMedical = DataService.PatientIdentity.GetPatientConsultationMedicalCertificate(visitModel.PatientUID, visitModel.PatientVisitUID);
                if (careMedical != null && careMedical.Count > 0)
                {
                    SelectedCareprovider2 = CareproviderSource.FirstOrDefault(p => p.CareproviderUID == careMedical.FirstOrDefault().CareproviderUID);
                }
  
            }



            CommentDoctor = SelectPatientVisit.Comments;
            Company = SelectPatientVisit.CompanyName;
            EmployerAddress = SelectPatientVisit.EmployerAddress;

            if (SelectPatientVisit.VisitStatus == "Financial Discharge")
            {
                IsEnableFinancial = false;
            }
            else
            {
                IsEnableFinancial = true;
            }

        }

        public bool ValidateVisitData()
        {

            //if (SelectLocation == null)
            //{
            //    WarningDialog("กรุณาเลือก แผนก");
            //    return true;
            //}
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

            return false;
        }

        #endregion
    }
}
