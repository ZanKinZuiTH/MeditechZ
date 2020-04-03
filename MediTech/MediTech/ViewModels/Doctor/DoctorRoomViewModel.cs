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

namespace MediTech.ViewModels
{
    public class DoctorRoomViewModel : MediTechViewModelBase
    {

        #region Properties

        private bool _IsEnableControl = false;

        public bool IsEnableControl
        {
            get { return _IsEnableControl; }
            set { Set(ref _IsEnableControl, value); }
        }

        public List<CareproviderModel> Doctors { get; set; }
        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { _SelectDoctor = value; }
        }

        private DateTime _VisitDate;

        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set { _VisitDate = value; }
        }


        public ObservableCollection<LookupReferenceValueModel> VisitStatus { get; set; }
        private LookupReferenceValueModel _SelectVisitStatus;

        public LookupReferenceValueModel SelectVisitStatus
        {
            get { return _SelectVisitStatus; }
            set { Set(ref _SelectVisitStatus, value); }
        }

        private List<object> _SelectVisitStatusList;

        public List<object> SelectVisitStatusList
        {
            get { return _SelectVisitStatusList; }
            set { Set(ref _SelectVisitStatusList, value); }
        }

        private List<PatientVisitModel> _PatientVisits;

        public List<PatientVisitModel> PatientVisits
        {
            get { return _PatientVisits; }
            set { Set(ref _PatientVisits, value); }
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

        private PatientVisitModel _SelectVisitMedical;

        public PatientVisitModel SelectVisitMedical
        {
            get { return _SelectVisitMedical; }
            set
            {
                Set(ref _SelectVisitMedical, value);
                if (_SelectVisitMedical != null)
                {
                    IsEnableControl = true;
                    GetLastVitalSign();
                }
                else
                {
                    IsEnableControl = false;
                    ClearLastVistalSign();
                }

            }
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
                if(_SelectedPateintSearch != null)
                {
                    RefershPatient();
                    SearchPatientCriteria = string.Empty;
                }
            }
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
            set { Set(ref _SelectOrganisation, value); }
        }

        #endregion


        #region PatientVitalSign

        private bool _IsExpandVitalSign = false;

        public bool IsExpandVitalSign
        {
            get { return _IsExpandVitalSign; }
            set { Set(ref _IsExpandVitalSign, value); }
        }

        private string _Temprature = "N/A";

        public string Temprature
        {
            get { return _Temprature; }
            set { Set(ref _Temprature, value); }
        }

        private string _RespiratoryRate = "N/A";

        public string RespiratoryRate
        {
            get { return _RespiratoryRate; }
            set { Set(ref _RespiratoryRate, value); }
        }

        private string _Pulse = "N/A";

        public string Pulse
        {
            get { return _Pulse; }
            set { Set(ref _Pulse, value); }
        }

        private string _BPsys = "N/A";

        public string BPsys
        {
            get { return _BPsys; }
            set { Set(ref _BPsys, value); }
        }

        private string _BPDio = "N/A";

        public string BPDio
        {
            get { return _BPDio; }
            set { Set(ref _BPDio, value); }
        }


        #endregion


        #endregion

        #region Command


        private RelayCommand _RefershPatientCommand;

        public RelayCommand RefershPatientCommand
        {
            get { return _RefershPatientCommand ?? (_RefershPatientCommand = new RelayCommand(RefershPatient)); }
        }


        private RelayCommand _RefershDataCommand;

        public RelayCommand RefershDataCommand
        {
            get { return _RefershDataCommand ?? (_RefershDataCommand = new RelayCommand(RefershData)); }
        }

        private RelayCommand _VisitMedicalCommand;

        public RelayCommand VisitMedicalCommand
        {
            get { return _VisitMedicalCommand ?? (_VisitMedicalCommand = new RelayCommand(VisitMedical)); }
        }

        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }


        #region VitalSign

        private RelayCommand _VitalSignCommand;

        public RelayCommand VitalSignCommand
        {
            get { return _VitalSignCommand ?? (_VitalSignCommand = new RelayCommand(VitalSignPopUp)); }
        }


        #endregion

        #region Allergy
        private RelayCommand _AllergyCommand;

        public RelayCommand AllergyCommand
        {
            get { return _AllergyCommand ?? (_AllergyCommand = new RelayCommand(AllergyPopUp)); }
        }


        #endregion

        #region AppointMent
        private RelayCommand _AppointmentCommand;

        public RelayCommand AppointmentCommand
        {
            get { return _AppointmentCommand ?? (_AppointmentCommand = new RelayCommand(AppointmentPopUp)); }
        }


        #endregion

        private RelayCommand _MedicalDischageCommand;

        public RelayCommand MedicalDischageCommand
        {
            get { return _MedicalDischageCommand ?? (_MedicalDischageCommand = new RelayCommand(MedicalDischage)); }
        }

        private RelayCommand _PastMedicalCommand;

        public RelayCommand PastMedicalCommand
        {
            get { return _PastMedicalCommand ?? (_PastMedicalCommand = new RelayCommand(PastMedicalHistory)); }
        }

        #endregion

        #region Method

        int REGST = 417;
        int CHKOUT = 418;
        int SNDDOC = 419;
        int FINDIS = 421;
        public DoctorRoomViewModel()
        {
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            var refData = DataService.Technical.GetReferenceValueList("VISTS,DIAGTYP");
            VisitStatus = new ObservableCollection<LookupReferenceValueModel>(refData.Where(p => p.DomainCode == "VISTS"));

            SelectVisitStatus = VisitStatus.FirstOrDefault(p => p.ValueCode == "SNDDOC");
            VisitDate = DateTime.Now;

            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

        }

        public override void OnLoaded()
        {
            SearchPatientVisit();
            NavPanelExpand(false);
            RibbonExpand(false);
        }

        private void SearchPatientVisit()
        {
            string statusList = string.Empty;
            if (SelectVisitStatusList != null)
            {
                foreach (object item in SelectVisitStatusList)
                {
                    if (statusList == "")
                    {
                        statusList = item.ToString();
                    }
                    else
                    {
                        statusList += "," + item.ToString();
                    }
                }
            }

            int? careproviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
            string patientID = (SelectedPateintSearch != null && SearchPatientCriteria != "") ? SelectedPateintSearch.PatientID : "";
            int? ownerOrganisationUID = (SelectOrganisation != null && SelectOrganisation.HealthOrganisationUID != 0) ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            PatientVisits = DataService.PatientIdentity.SearchPatientVisit(patientID, "", "", careproviderUID, statusList, VisitDate, null, null, ownerOrganisationUID,null);
        }

        private void VisitMedical()
        {
            SelectVisitMedical = SelectPatientVisit;
            SelectPatientVisit = null;
        }
        private void RefershPatient()
        {
            SearchPatientVisit();
        }

        private void RefershData()
        {
            if (SelectVisitMedical != null)
            {
                var view = (this.View as DoctorRoom);
                var summeryViewModel = (view.summeryView.DataContext as SummeryViewViewModel);
                summeryViewModel.LoadLabResult();
                summeryViewModel.LoadRaiologyResult();
                GetLastVitalSign();
            }
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null);
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        #region VitalSign

        private void ClearLastVistalSign()
        {
            IsExpandVitalSign = false;
            Temprature = "N/A";
            Pulse = "N/A";
            RespiratoryRate = "N/A";
            BPsys = "N/A";
            BPDio = "N/A";
        }

        private void GetLastVitalSign()
        {
            if (SelectVisitMedical != null)
            {
                List<PatientVitalSignModel> vitalSign = DataService.PatientHistory.GetPatientVitalSingByVisitUID(SelectVisitMedical.PatientVisitUID);
                if (vitalSign != null && vitalSign.Count > 0)
                {
                    IsExpandVitalSign = true;
                    var lastVitalSign = vitalSign.OrderByDescending(p => p.CWhen).FirstOrDefault();
                    if (lastVitalSign != null)
                    {
                        Temprature = !string.IsNullOrEmpty(lastVitalSign.Temprature.ToString()) ? lastVitalSign.Temprature.ToString() : "N/A";
                        Pulse = !string.IsNullOrEmpty(lastVitalSign.Pulse.ToString()) ? lastVitalSign.Pulse.ToString() : "N/A";
                        RespiratoryRate = !string.IsNullOrEmpty(lastVitalSign.RespiratoryRate.ToString()) ? lastVitalSign.RespiratoryRate.ToString() : "N/A";
                        BPsys = !string.IsNullOrEmpty(lastVitalSign.BPSys.ToString()) ? lastVitalSign.BPSys.ToString() : "N/A";
                        BPDio = !string.IsNullOrEmpty(lastVitalSign.BPDio.ToString()) ? lastVitalSign.BPDio.ToString() : "N/A";
                    }
                }
                else
                {
                    Temprature = "N/A";
                    Pulse = "N/A";
                    RespiratoryRate = "N/A";
                    BPsys = "N/A";
                    BPDio = "N/A";
                }
            }


        }

        private void VitalSignPopUp()
        {
            if (SelectVisitMedical != null)
            {
                PatientVitalSign pageview = new PatientVitalSign();
                (pageview.DataContext as PatientVitalSignViewModel).AssingPatientVisit(SelectVisitMedical);
                PatientVitalSignViewModel result = (PatientVitalSignViewModel)LaunchViewDialog(pageview, "PTVAT", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    GetLastVitalSign();
                    if (this.View != null)
                    {
                        if (this.View is DoctorRoom)
                        {
                            (this.View as DoctorRoom).PatientBanner.SetPatientBanner(SelectVisitMedical.PatientUID, SelectVisitMedical.PatientVisitUID);
                        }
                    }
                }
            }
        }


        #endregion

        #region PatientAllergy

        public void AllergyPopUp()
        {
            if (SelectVisitMedical != null)
            {
                PatientAllergy pageview = new PatientAllergy();
                (pageview.DataContext as PatientAllergyViewModel).AssingPatientVisit(SelectVisitMedical);
                PatientAllergyViewModel result = (PatientAllergyViewModel)LaunchViewDialog(pageview, "LIARGY", false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    if (this.View != null)
                    {
                        if (this.View is DoctorRoom)
                        {
                            (this.View as DoctorRoom).PatientBanner.SetPatientBanner(SelectVisitMedical.PatientUID, SelectVisitMedical.PatientVisitUID);
                        }
                    }
                }
            }
        }

        #endregion

        #region Appointment

        private void AppointmentPopUp()
        {
            if (SelectVisitMedical != null)
            {
                ManageAppointment pageview = new ManageAppointment();
                (pageview.DataContext as ManageAppointmentViewModel).AssingPatientVisit(SelectVisitMedical);
                ManageAppointmentViewModel result = (ManageAppointmentViewModel)LaunchViewDialog(pageview, "MAPOME", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                }
            }
        }

        #endregion

        private void MedicalDischage()
        {
            try
            {
                if (SelectVisitMedical != null)
                {
                    if (SelectVisitMedical.VISTSUID == CHKOUT || SelectVisitMedical.VISTSUID == FINDIS)
                    {
                        WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                        return;
                    }
                    else
                    {
                       var patientVisitCurrent =  DataService.PatientIdentity.GetPatientVisitByUID(SelectVisitMedical.PatientVisitUID);
                        if (patientVisitCurrent!= null && patientVisitCurrent.VISTSUID == CHKOUT || patientVisitCurrent.VISTSUID == FINDIS)
                        {
                            WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน โปรดตรวจสอบ หรือ Refersh ข้อมูล");
                            return;
                        }
                    }
                    DialogResult result = QuestionDialog("ต้องการจบการรักษา");
                    if (result == DialogResult.Yes)
                    {
                        DataService.PatientIdentity.ChangeVisitStatus(SelectVisitMedical.PatientVisitUID, 418, SelectVisitMedical.CareProviderUID, null, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        SelectVisitMedical = null;
                        //OnLoaded();
                        SearchPatientVisit();
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void PastMedicalHistory()
        {
            if (SelectVisitMedical != null)
            {
                PastMedicalHistory pageview = new PastMedicalHistory();
                (pageview.DataContext as PastMedicalHistoryViewModel).AssingPatientVisit(SelectVisitMedical);
                PastMedicalHistoryViewModel result = (PastMedicalHistoryViewModel)LaunchViewDialog(pageview, "PAMEDHIS", false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    PatientDiagnosisViewModel resultDiag = null;
                    if (result.IsCheckDiagnosis)
                    {
                        PatientDiagnosis pageDiag = new PatientDiagnosis();
                        PatientDiagnosisViewModel viewModelDiag = (pageDiag.DataContext as PatientDiagnosisViewModel);
                        viewModelDiag.AssingPatientVisit(SelectVisitMedical);
                        List<PatientProblemModel> listReDiagnosis = result.ListDiagnosis;
                        if (viewModelDiag.PatientProblemList != null && viewModelDiag.PatientProblemList.Count > 0)
                        {
                            WarningDialog("มีการลงข้อมูลวินิจฉัยคนไข้แล้วไม่สามารถทำการ Remed ได้");
                        }
                        else
                        {
                            foreach (var item in listReDiagnosis)
                            {
                                PatientProblemModel patProblem = new PatientProblemModel();
                                patProblem.PatientUID = SelectVisitMedical.PatientUID;
                                patProblem.PatientVisitUID = SelectVisitMedical.PatientVisitUID;
                                patProblem.ProblemUID = item.ProblemUID;
                                patProblem.ProblemCode = item.ProblemCode;
                                patProblem.ProblemName = item.ProblemName;
                                patProblem.ProblemDescription = item.ProblemDescription;
                                patProblem.DIAGTYPUID = item.DIAGTYPUID;
                                patProblem.DiagnosisType = item.DiagnosisType;
                                patProblem.IsUnderline = item.IsUnderline;
                                patProblem.SEVRTUID = item.SEVRTUID;
                                patProblem.CERNTUID = item.CERNTUID;
                                patProblem.PBMTYUID = item.PBMTYUID;
                                patProblem.BDLOCUID = item.BDLOCUID;
                                patProblem.Severity = item.Severity;
                                patProblem.Certanity = item.Certanity;
                                patProblem.ProblemType = item.ProblemType;
                                patProblem.BodyLocation = item.BodyLocation;
                                patProblem.RecordedDttm = DateTime.Now;
                                patProblem.RecordedName = AppUtil.Current.UserName;
                                viewModelDiag.PatientProblemList.Add(patProblem);
                                pageDiag.grdPatientProblem.RefreshData();
                                viewModelDiag.ClearControl();
                            }
                            resultDiag = (PatientDiagnosisViewModel)LaunchViewDialog(pageDiag, "PATDIAG", false);
                        }


                        if (resultDiag != null && resultDiag.ResultDialog == ActionDialog.Save)
                        {
                            if (this.View is DoctorRoom)
                            {
                                ((this.View as DoctorRoom).summeryView.DataContext as SummeryViewViewModel).LoadDianosis();
                            }

                        }
                    }

                    if (result.IsCheckDrug)
                    {
                        if (SelectVisitMedical.VISTSUID == CHKOUT || SelectVisitMedical.VISTSUID == FINDIS)
                        {
                            WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                            return;
                        }
                        List<PatientOrderDetailModel> listDrugProfile = result.ListDrugProfile;
                        PatientOrderEntry pageOrder = new PatientOrderEntry();
                        PatientOrderEntryViewModel viewModelPatientOrder = (pageOrder.DataContext as PatientOrderEntryViewModel);

                        viewModelPatientOrder.AssingPatientVisit(SelectVisitMedical);

                        var healthOrganisations = GetHealthOrganisationRoleMedical();
                        var SelectHealthOrganisation = healthOrganisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
                        int? ownerUID;
                        if (SelectHealthOrganisation != null)
                            ownerUID = SelectHealthOrganisation.HealthOrganisationUID;
                        else
                            ownerUID = SelectVisitMedical.OwnerOrganisationUID;

                        foreach (var itemDrug in listDrugProfile)
                        {
                            PatientOrderDetailModel newOrder = new PatientOrderDetailModel();
                            BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(itemDrug.BillableItemUID);

                            List<PatientOrderAlertModel> listOrderAlert = DataService.OrderProcessing.CriteriaOrderAlert(SelectVisitMedical.PatientUID, billItem);
                            if (listOrderAlert != null && listOrderAlert.Count > 0)
                            {
                                OrderAlertViewModel viewModel = (OrderAlertViewModel)ShowModalDialogUsingViewModel(new OrderAlert(), new OrderAlertViewModel(listOrderAlert), true);
                                if (viewModel.ResultDialog != ActionDialog.Save)
                                {
                                    continue;
                                }
                                newOrder.PatientOrderAlert = viewModel.OrderAlerts;
                            }

                            var billItemPrice = GetBillableItemPrice(billItem.BillableItemDetails, ownerUID ?? 0);

                            if (billItemPrice == null)
                            {
                                WarningDialog("รายการ " + billItem.ItemName + " นี้ยังไม่ได้กำหนดราคาสำหรับขาย โปรดตรวจสอบ");
                                return;
                            }

                            if (billItemPrice == null)
                            {
                                WarningDialog("รายการ " + billItem.ItemName + " นี้ยังไม่ได้กำหนดราคาสำหรับขาย โปรดตรวจสอบ");
                                return;
                            }

                            if (billItem.BillingServiceMetaData == "Drug" || billItem.BillingServiceMetaData == "Medical Supplies")
                            {
                                ItemMasterModel itemMaster = DataService.Inventory.GetItemMasterByUID(billItem.ItemUID.Value);
                                List<StockModel> stores = new List<StockModel>();
                                stores = DataService.Inventory.GetStockRemainByItemMasterUID(itemMaster.ItemMasterUID, ownerUID ?? 0);

                                if (stores == null || stores.Count <= 0)
                                {
                                    WarningDialog("ไม่มี " + billItem.ItemName + " ในคลัง โปรดตรวจสอบ");
                                    continue;
                                }
                                else
                                {
                                    if (itemDrug.Quantity > stores.FirstOrDefault().Quantity)
                                    {
                                        if (itemMaster.CanDispenseWithOutStock != "Y")
                                        {
                                            WarningDialog("มี " + billItem.ItemName + " ในคลังไม่พอสำหรับจ่ายยา โปรดตรวจสอบ");
                                            continue;

                                        }
                                        else if (itemMaster.CanDispenseWithOutStock == "Y")
                                        {
                                            DialogResult resultDiaglog = QuestionDialog("มี" + billItem.ItemName + "ในคลังไม่พอ คุณต้องการดำเนินการต่อหรือไม่ ?");
                                            if (resultDiaglog == DialogResult.No || resultDiaglog == DialogResult.Cancel)
                                            {
                                                continue;
                                            }
                                        }
                                    }
                                }

                                if (itemDrug.Quantity <= 0)
                                {
                                    WarningDialog("ไม่อนุญาติให้คีย์ + " + billItem.ItemName + " จำนวน < 0");
                                    continue;
                                }

                                if (itemMaster.MinSalesQty != null && itemDrug.Quantity < itemMaster.MinSalesQty)
                                {
                                    WarningDialog("คีย์จำนวน " + billItem.ItemName + " ที่ใช้น้อยกว่าจำนวนขั้นต่ำที่คีย์ได้ โปรดตรวจสอบ");
                                    continue;
                                }


                                newOrder.IsStock = itemMaster.IsStock;
                                newOrder.StoreUID = stores.FirstOrDefault().StoreUID;
                                newOrder.DFORMUID = itemMaster.FORMMUID;
                                newOrder.PDSTSUID = itemMaster.PDSTSUID;
                                newOrder.QNUOMUID = itemMaster.BaseUOM;

                            }

                            newOrder.BillableItemUID = billItem.BillableItemUID;
                            newOrder.ItemName = billItem.ItemName;
                            newOrder.BSMDDUID = billItem.BSMDDUID;
                            newOrder.ItemUID = billItem.ItemUID;
                            newOrder.ItemCode = billItem.Code;
                            newOrder.BillingService = billItem.BillingServiceMetaData;
                            newOrder.DoctorFee = billItem.DoctorFee;
                            newOrder.UnitPrice = billItemPrice.Price;
                            newOrder.DisplayPrice = billItemPrice.Price;

                            newOrder.FRQNCUID = itemDrug.FRQNCUID;
                            newOrder.Quantity = itemDrug.Quantity;
                            newOrder.Dosage = itemDrug.Dosage;
                            newOrder.IsPriceOverwrite = "N";
                            newOrder.StartDttm = DateTime.Now;

                            newOrder.NetAmount = ((billItemPrice.Price) * itemDrug.Quantity) + (billItem.DoctorFee ?? 0);
                            newOrder.OwnerOrganisationUID = ownerUID ?? 0;


                            viewModelPatientOrder.PatientOrders.Add(newOrder);
                            pageOrder.grdOrderDetail.RefreshData();
                        }
                        PatientOrderEntryViewModel resultOrder = (PatientOrderEntryViewModel)LaunchViewDialog(pageOrder, "ORDITM", false, true);
                        if (resultOrder != null && resultOrder.ResultDialog == ActionDialog.Save)
                        {
                            if (this.View is DoctorRoom)
                            {
                                ((this.View as DoctorRoom).summeryView.DataContext as SummeryViewViewModel).LoadPatientOrder();
                            }
                        }
                    }
                }
            }
        }

        public BillableItemDetailModel GetBillableItemPrice(List<BillableItemDetailModel> billItmDetail, int ownerOrganisationUID)
        {
            BillableItemDetailModel selectBillItemDetail = null;

            if (billItmDetail.Count(p => p.OwnerOrganisationUID == ownerOrganisationUID) > 0)
            {
                selectBillItemDetail = billItmDetail
                    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == ownerOrganisationUID
                    && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value.Date <= DateTime.Now.Date))
                    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date >= DateTime.Now.Date))
                    );
            }
            else
            {
                selectBillItemDetail = billItmDetail
    .FirstOrDefault(p => p.StatusFlag == "A" && p.OwnerOrganisationUID == 0
    && (p.ActiveFrom == null || (p.ActiveFrom.HasValue && p.ActiveFrom.Value.Date <= DateTime.Now.Date))
    && (p.ActiveTo == null || (p.ActiveTo.HasValue && p.ActiveTo.Value.Date >= DateTime.Now.Date))
    );
            }

            return selectBillItemDetail;
        }

        #endregion
    }
}
