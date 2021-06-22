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
using System.Windows.Forms;
using DevExpress.XtraReports.UI;

namespace MediTech.ViewModels
{
    public class PatientVisitMassViewModel : MediTechViewModelBase
    {
        #region Properties
        private string _BN;

        public string BN
        {
            get { return _BN; }
            set { Set(ref _BN, value); }
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

        public List<CareproviderModel> Doctors { get; set; }
        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { _SelectDoctor = value; }
        }


        public ObservableCollection<LookupReferenceValueModel> VisitStatus { get; set; }

        private List<object> _SelectVisitStatusList;

        public List<object> SelectVisitStatusList
        {
            get { return _SelectVisitStatusList ?? (_SelectVisitStatusList = new List<object>()); }
            set { Set(ref _SelectVisitStatusList, value); }
        }


        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
                if (IsEditDate)
                {
                    SelectDateType = null;
                }

            }
        }


        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                Set(ref _DateTo, value);
                if (IsEditDate)
                {
                    SelectDateType = null;
                }
            }
        }

        public bool IsEditDate { get; set; }
        public List<LookupItemModel> DateTypes { get; set; }
        private LookupItemModel _SelectDateType;

        public LookupItemModel SelectDateType
        {
            get { return _SelectDateType; }
            set
            {
                Set(ref _SelectDateType, value);

                if (SelectDateType != null)
                {
                    if (SelectDateType.Key == 1)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Now;
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
                    else if (SelectDateType.Key == 2)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Now.AddDays(-7);
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
                    else if (SelectDateType.Key == 3)
                    {
                        IsEditDate = false;
                        DateFrom = DateTime.Parse("01/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year);
                        DateTo = DateTime.Now;
                        IsEditDate = true;
                    }
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


        private List<PayorDetailModel> _PayorDetails;

        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        private PayorDetailModel _SelectPayorDetail;

        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set { Set(ref _SelectPayorDetail, value); }
        }



        private bool _EnableSearchItem = false;

        public bool EnableSearchItem
        {
            get { return _EnableSearchItem; ; }
            set { Set(ref _EnableSearchItem, value); }
        }

        private string _SearchOrderCriteria;

        public string SearchOrderCriteria
        {
            get { return _SearchOrderCriteria; }
            set
            {
                Set(ref _SearchOrderCriteria, value);
                if (!string.IsNullOrEmpty(_SearchOrderCriteria) && _SearchOrderCriteria.Length >= 3)
                {
                    int ownerOrganisationUID = 0;
                    if (SelectHealthOrganisation != null)
                    {
                        ownerOrganisationUID = SelectHealthOrganisation.HealthOrganisationUID;
                    }
                    var dataOrderIitems = DataService.OrderProcessing.SearchOrderItem(_SearchOrderCriteria, ownerOrganisationUID);
                    OrderItems = dataOrderIitems.Where(p => p.TypeOrder != "Drug" && p.TypeOrder != "Medical Supplies" && p.TypeOrder != "Supply").ToList();

                }
                else
                {
                    OrderItems = null;
                }
            }
        }


        private List<SearchOrderItem> _OrderItems;

        public List<SearchOrderItem> OrderItems
        {
            get { return _OrderItems; }
            set { Set(ref _OrderItems, value); }
        }

        private SearchOrderItem _SelectOrderItem;

        public SearchOrderItem SelectOrderItem
        {
            get { return _SelectOrderItem; }
            set
            {
                _SelectOrderItem = value;
                if (_SelectOrderItem != null)
                {
                    ApplyOrderItem(_SelectOrderItem);
                }
            }
        }


        private List<HealthOrganisationModel> _HealthOrganisations;

        public List<HealthOrganisationModel> HealthOrganisations
        {
            get { return _HealthOrganisations; }
            set { Set(ref _HealthOrganisations, value); }
        }

        private HealthOrganisationModel _SelectHealthOrganisation;

        public HealthOrganisationModel SelectHealthOrganisation
        {
            get { return _SelectHealthOrganisation; }
            set
            {
                Set(ref _SelectHealthOrganisation, value);
                if (SelectHealthOrganisation == null)
                {
                    EnableSearchItem = false;
                }
                else
                {
                    EnableSearchItem = true;
                }
            }
        }


        public List<CareproviderModel> Careproviders { get; set; }
        private CareproviderModel _SelectCareprovider;

        public CareproviderModel SelectCareprovider
        {
            get { return _SelectCareprovider; }
            set { _SelectCareprovider = value; }
        }


        private ObservableCollection<PatientOrderDetailModel> _PatientOrders;

        public ObservableCollection<PatientOrderDetailModel> PatientOrders
        {
            get { return _PatientOrders ?? (_PatientOrders = new ObservableCollection<PatientOrderDetailModel>()); }
            set { Set(ref _PatientOrders, value); }
        }

        private PatientOrderDetailModel _SelectPatientOrder;

        public PatientOrderDetailModel SelectPatientOrder
        {
            get { return _SelectPatientOrder; }
            set { _SelectPatientOrder = value; }
        }


        private DateTime _SaveVisitStatusTime;

        public DateTime SaveVisitStatusTime
        {
            get { return _SaveVisitStatusTime; }
            set { Set(ref _SaveVisitStatusTime, value); }
        }

        private CareproviderModel _SelectDoctorSaveVisitStatus;

        public CareproviderModel SelectDoctorSaveVisitStatus
        {
            get { return _SelectDoctorSaveVisitStatus; }
            set { Set(ref _SelectDoctorSaveVisitStatus, value); }
        }

        private List<LookupReferenceValueModel> _SaveVisitStatusList;

        public List<LookupReferenceValueModel> SaveVisitStatusList
        {
            get { return _SaveVisitStatusList; }
            set { _SaveVisitStatusList = value; }
        }

        private LookupReferenceValueModel _SelectSaveVisitStatusList;

        public LookupReferenceValueModel SelectSaveVisitStatusList
        {
            get { return _SelectSaveVisitStatusList; }
            set
            {
                Set(ref _SelectSaveVisitStatusList, value);
                if (SelectSaveVisitStatusList.Key == SNDDOC)
                {
                    VisitStatusDoctorVisibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    VisitStatusDoctorVisibility = System.Windows.Visibility.Collapsed;
                }
            }
        }


        private ObservableCollection<PatientVisitModel> __PatientVisits;

        public ObservableCollection<PatientVisitModel> PatientVisits
        {
            get { return __PatientVisits; }
            set { Set(ref __PatientVisits, value); }
        }

        private Visibility _VisitStatusDoctorVisibility = Visibility.Collapsed;

        public Visibility VisitStatusDoctorVisibility
        {
            get { return _VisitStatusDoctorVisibility; }
            set { Set(ref _VisitStatusDoctorVisibility, value); }
        }

        private LookupItemModel _SelectPrinter;
        public LookupItemModel SelectPrinter
        {
            get { return _SelectPrinter; }
            set { Set(ref _SelectPrinter, value); }
        }

        private List<LookupItemModel> _PrinterLists;
        public List<LookupItemModel> PrinterLists
        {
            get { return _PrinterLists; }
            set { Set(ref _PrinterLists, value); }
        }

        private List<ReportsModel> _ListPatientReports;

        public List<ReportsModel> ListPatientReports
        {
            get { return _ListPatientReports; }
            set { Set(ref _ListPatientReports, value); }
        }

        private ReportsModel _SelectReport;

        public ReportsModel SelectReport
        {
            get { return _SelectReport; }
            set { Set(ref _SelectReport, value); }
        }

        private bool _SelectAll;

        public bool SelectAll
        {
            get { return _SelectAll; }
            set
            {
                if (_SelectAll != value)
                {
                    _SelectAll = value;
                    if (SelectAll == true)
                    {
                        if (PatientVisits != null && PatientVisits.Count > 0)
                        {
                            foreach (var patientVisit in PatientVisits)
                            {
                                patientVisit.Select = true;
                            }
                        }
                    }
                    else if (SelectAll == false)
                    {
                        if (PatientVisits != null && PatientVisits.Count > 0)
                        {
                            foreach (var patientVisit in PatientVisits)
                            {
                                patientVisit.Select = false;
                            }
                        }
                    }
                    PatientVisitMass view = (PatientVisitMass)this.View;
                    view.PatientGrid.RefreshData();
                }
                Set(ref _SelectAll, value);
            }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPatientVisit)); }
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(ClearControl)); }
        }

        private RelayCommand _ClearOrderCommand;
        public RelayCommand ClearOrderCommand
        {
            get
            {
                return _ClearOrderCommand ?? (_ClearOrderCommand = new RelayCommand(ClearOrder));
            }
        }




        private RelayCommand _SaveOrderCommand;
        public RelayCommand SaveOrderCommand
        {
            get
            {
                return _SaveOrderCommand ?? (_SaveOrderCommand = new RelayCommand(SaveOrder));
            }
        }

        private RelayCommand _SaveVisitStatusCommand;
        public RelayCommand SaveVisitStatusCommand
        {
            get
            {
                return _SaveVisitStatusCommand ?? (_SaveVisitStatusCommand = new RelayCommand(SaveVisitStatus));
            }
        }

        private RelayCommand _DeleteOrderCommand;

        public RelayCommand DeleteOrderCommand
        {
            get
            {
                return _DeleteOrderCommand
                    ?? (_DeleteOrderCommand = new RelayCommand(DeleteOrder));
            }
        }


        private RelayCommand _PrintAutoCommand;

        /// <summary>
        /// Gets the EnterResultCommand.
        /// </summary>
        public RelayCommand PrintAutoCommand
        {
            get
            {
                return _PrintAutoCommand
                    ?? (_PrintAutoCommand = new RelayCommand(PrintAuto));
            }
        }
        #endregion

        #region Method

        int REGST = 417;
        int CHKOUT = 418;
        int SNDDOC = 419;
        int FINDIS = 421;
        int CANCEL = 410;

        public PatientVisitMassViewModel()
        {
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            var refVisitStatus = DataService.Technical.GetReferenceValueMany("VISTS");
            VisitStatus = new ObservableCollection<LookupReferenceValueModel>(refVisitStatus);

            foreach (var item in VisitStatus)
            {
                if (item.Key == REGST || item.Key == CHKOUT)
                {
                    SelectVisitStatusList.Add(item.Key);
                }

            }

            DateTypes = new List<LookupItemModel>();
            DateTypes.Add(new LookupItemModel { Key = 1, Display = "วันนี้" });
            DateTypes.Add(new LookupItemModel { Key = 2, Display = "อาทิตย์ล่าสุด" });
            DateTypes.Add(new LookupItemModel { Key = 3, Display = "เดือนนี้" });
            SelectDateType = DateTypes.FirstOrDefault();

            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            PayorDetails = DataService.MasterData.GetPayorDetail();


            Careproviders = DataService.UserManage.GetCareproviderAll();
            SelectCareprovider = Careproviders.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);

            HealthOrganisations = Organisations;
            SelectHealthOrganisation = HealthOrganisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);


            SaveVisitStatusList = refVisitStatus;
            SaveVisitStatusTime = DateTime.Now;
            GetPrinters();
            GetReports();

        }

        private void GetPrinters()
        {
            PrinterLists = new List<LookupItemModel>();
            int i = 1;
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                LookupItemModel lookupData = new LookupItemModel();
                lookupData.Key = i;
                lookupData.Display = printer;
                PrinterLists.Add(lookupData);
                i++;
            }
        }
        private void GetReports()
        {
            var tempReports = ConstantData.GetPatientReports();
            ListPatientReports = tempReports;
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
            int? ownerOrganisationUID = (SelectOrganisation != null && SelectOrganisation.HealthOrganisationUID != 0) ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            int? payorDetailUID = (SelectPayorDetail != null && SelectPayorDetail.PayorDetailUID != 0) ? SelectPayorDetail.PayorDetailUID : (int?)null;
            PatientVisits = new ObservableCollection<PatientVisitModel>(DataService.PatientIdentity.SearchPatientVisit(BN, FirstName, LastName, careproviderUID, statusList, DateFrom, DateTo, null, ownerOrganisationUID, payorDetailUID, null));

        }

        public void SaveOrder()
        {
            try
            {
                if (PatientVisits != null)
                {
                    if (PatientOrders != null && PatientOrders.Count > 0)
                    {
                        PatientVisitMass view = (PatientVisitMass)this.View;
                        int upperlimit = 0;
                        int loopCounter = 0;
                        foreach (var currentData in PatientVisits)
                        {
                            if (currentData.Select == true)
                            {
                                upperlimit++;
                            }
                        }

                        view.SetProgressBarLimits(0, upperlimit);

                        foreach (var patientVisit in PatientVisits)
                        {
                            if (patientVisit.Select)
                            {


                                if (patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL)
                                {
                                    WarningDialog("ผู้ป่วย " + patientVisit.PatientName + "ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                                    patientVisit.Select = false;
                                    OnUpdateEvent();
                                    continue;
                                }
                                int userUID = AppUtil.Current.UserID;
                                int owerOrganisationUID = 0;
                                List<PatientOrderDetailModel> saveOrders = new List<PatientOrderDetailModel>();

                                foreach (var patientOrder in PatientOrders)
                                {
                                    BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(patientOrder.BillableItemUID);
                                    var listOrderAlert = DataService.OrderProcessing.CriteriaOrderAlert(patientVisit.PatientUID, billItem);
                                    if (listOrderAlert != null && listOrderAlert.Count > 0)
                                    {
                                        WarningDialog("ผู้ป่วย " + patientVisit.PatientName + " รายการ " + billItem.ItemName + " นี้มีแจ้งเตือนการคีย์");
                                        OrderAlert viewOrderAlert = new OrderAlert();
                                        OrderAlertViewModel viewModel = (OrderAlertViewModel)ShowModalDialogUsingViewModel(viewOrderAlert, new OrderAlertViewModel(listOrderAlert), true);
                                        if (viewModel.ResultDialog != ActionDialog.Save)
                                        {
                                            patientVisit.Select = false;
                                            OnUpdateEvent();
                                            continue;
                                        }
                                        patientOrder.PatientOrderAlert = viewModel.OrderAlerts;
                                    }

                                    saveOrders.Add(patientOrder);
                                }


                                if (SelectHealthOrganisation == null)
                                {
                                    owerOrganisationUID = SelectHealthOrganisation.HealthOrganisationUID;
                                }
                                else
                                {
                                    owerOrganisationUID = patientVisit.OwnerOrganisationUID ?? userUID;
                                }
                                string orderNumber = DataService.OrderProcessing.CreateOrder(patientVisit.PatientUID, patientVisit.PatientVisitUID, userUID, owerOrganisationUID, saveOrders);
                                patientVisit.Select = false;
                                loopCounter = loopCounter + 1;
                                view.SetProgressBarValue(loopCounter);

                            }
                        }

                        view.SetProgressBarValue(upperlimit);
                        SaveSuccessDialog();
                        view.PatientGrid.RefreshData();
                        PatientOrders = null;
                        view.progressBar1.Value = 0;
                    }
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void SaveVisitStatus()
        {
            try
            {
                if (PatientVisits != null)
                {
                    if (SelectSaveVisitStatusList != null)
                    {
                        PatientVisitMass view = (PatientVisitMass)this.View;
                        int upperlimit = 0;
                        int loopCounter = 0;
                        foreach (var currentData in PatientVisits)
                        {
                            if (currentData.Select == true)
                            {
                                upperlimit++;
                            }
                        }

                        view.SetProgressBarLimits(0, upperlimit);

                        int selectVisitStatus = SelectSaveVisitStatusList.Key;
                        int? CareProviderUID = null;
                        DateTime arriveTime = SaveVisitStatusTime;
                        if (selectVisitStatus == SNDDOC)
                        {
                            if (SelectDoctorSaveVisitStatus == null)
                            {
                                WarningDialog("กรุณาเลือกแพทย์");
                                return;
                            }

                            CareProviderUID = SelectDoctorSaveVisitStatus.CareproviderUID;
                        }
                        foreach (var patientVisit in PatientVisits)
                        {
                            if (patientVisit.Select)
                            {
                                if (selectVisitStatus != FINDIS)
                                {
                                    if (selectVisitStatus == SNDDOC && patientVisit.VISTSUID != REGST)
                                    {
                                        WarningDialog("ผู้ป่วย " + patientVisit.PatientName + "ไม่สามารถดำเนินการ Send To Doctor ได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                                        patientVisit.Select = false;
                                        OnUpdateEvent();
                                        continue;
                                    }
                                    if (selectVisitStatus == CHKOUT && (patientVisit.VISTSUID == CHKOUT || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL))
                                    {
                                        WarningDialog("ผู้ป่วย " + patientVisit.PatientName + "ไม่สามารถดำเนินการ ปิด Medical Discharge ได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                                        return;
                                    }

                                    if (selectVisitStatus == REGST && patientVisit.VISTSUID != CHKOUT)
                                    {
                                        WarningDialog("ผู้ป่วย " + patientVisit.PatientName + "ไม่สามารถดำเนินการ Registerได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                                    }

                                    if (selectVisitStatus == CANCEL && patientVisit.VISTSUID == FINDIS)
                                    {
                                        WarningDialog("ผู้ป่วย " + patientVisit.PatientName + "ไม่สามารถดำเนินการ Cancel ได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
                                    }
                                    if (selectVisitStatus != SNDDOC)
                                    {
                                        CareProviderUID = patientVisit.CareProviderUID;
                                    }
                                    DataService.PatientIdentity.ChangeVisitStatus(patientVisit.PatientVisitUID, selectVisitStatus, CareProviderUID, arriveTime, AppUtil.Current.UserID);
                                }
                                else
                                {
                                    List<PatientOrderDetailModel> orderAll = new List<PatientOrderDetailModel>();
                                    List<PatientOrderDetailModel> DrugOrderList = new List<PatientOrderDetailModel>();
                                    orderAll = DataService.OrderProcessing.GetOrderAllByVisitUID(patientVisit.PatientVisitUID);
                                    orderAll = orderAll.Where(p => p.ORDSTUID != 2848).ToList();
                                    ObservableCollection<PatientBilledItemModel> OrderAllList = new ObservableCollection<PatientBilledItemModel>(orderAll.Select(p => new PatientBilledItemModel
                                    {
                                        BillableItemUID = p.BillableItemUID,
                                        PatientOrderDetailUID = p.PatientOrderDetailUID,
                                        ItemName = p.ItemName,
                                        Amount = p.NetAmount,
                                        Discount = p.Discount ?? 0,
                                        NetAmount = p.NetAmount,
                                        ItemMutiplier = p.Quantity ?? 1,
                                        BSMDDUID = p.BSMDDUID,
                                        IdentifyingUID = p.IdentifyingUID,
                                        BillingService = p.BillingService
                                    }));

                                    var TempDrugList = orderAll.Where(p => p.IdentifyingType.ToUpper() == "PRESCRIPTIONITEM").ToList();
                                    foreach (var drugItem in TempDrugList)
                                    {
                                        List<PatientOrderDetailModel> drugDetails = DataService.Pharmacy.GetDrugStoreDispense(drugItem.IdentifyingUID ?? 0);
                                        foreach (var item in drugDetails)
                                        {
                                            item.LocalInstructionText = drugItem.LocalInstructionText;
                                            item.UnitPrice = drugItem.UnitPrice;
                                            item.BillingService = drugItem.BillingService;
                                        }
                                        DrugOrderList.AddRange(drugDetails);
                                    }

                                    if (DrugOrderList != null)
                                    {
                                        foreach (var drugItem in DrugOrderList)
                                        {
                                            if (drugItem.StockUID == null || drugItem.StockUID == 0)
                                            {
                                                WarningDialog("รายการ " + drugItem.ItemName + " ไม่มีในคลังสินค้า โปรดตรวจสอบ หรือ ติดต่อ Admin");
                                                return;
                                            }
                                        }
                                    }

                                    Double TotalAmount = OrderAllList.Sum(p => p.NetAmount) ?? 0;

                                    PatientBillModel patbill = new PatientBillModel();
                                    patbill.PatientBilledItems = new List<PatientBilledItemModel>();


                                    //patbill.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                                    patbill.OwnerOrganisationUID = patientVisit.OwnerOrganisationUID ?? AppUtil.Current.OwnerOrganisationUID;
                                    patbill.CUser = AppUtil.Current.UserID;
                                    patbill.MUser = AppUtil.Current.UserID;
                                    patbill.PaidAmount = TotalAmount;
                                    patbill.Comments = "From Visit Mass";
                                    patbill.PatientName = patientVisit.PatientName;
                                    patbill.PatientUID = patientVisit.PatientUID;
                                    patbill.PatientVisitUID = patientVisit.PatientVisitUID;
                                    patbill.PatientID = patientVisit.PatientID;
                                    patbill.VisitID = patientVisit.VisitID;
                                    patbill.VisitDttm = patientVisit.StartDttm.Value;
                                    patbill.TotalAmount = TotalAmount;
                                    patbill.NetAmount = TotalAmount;

                                    patbill.PatientBilledItems.AddRange(OrderAllList);


                                    PatientBillModel patBillResult = DataService.Billing.GeneratePatientBill(patbill);
                                }

                                patientVisit.Select = false;
                                loopCounter = loopCounter + 1;
                                view.SetProgressBarValue(loopCounter);
                            }
                        }

                        view.SetProgressBarValue(upperlimit);
                        view.PatientGrid.RefreshData();
                        SaveSuccessDialog();
                        SearchPatientVisit();
                        view.progressBar1.Value = 0;
                    }
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void DeleteOrder()
        {
            if (SelectPatientOrder != null)
            {
                PatientOrders.Remove(SelectPatientOrder);
            }
        }

        private void ClearControl()
        {
            BN = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            SelectDoctor = null;
            DateFrom = DateTime.Today;
            DateTo = null;

            foreach (var item in VisitStatus)
            {
                if (item.Key == REGST || item.Key == CHKOUT)
                {
                    SelectVisitStatusList.Add(item.Key);
                }

            }

            SelectDateType = DateTypes.FirstOrDefault();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            SelectPayorDetail = null;
        }

        public void ClearOrder()
        {
            if (PatientOrders != null && PatientOrders.Count > 0)
            {
                MessageBoxResult result = QuestionDialog("คุณต้องการ Clear Order ทั้งหมดใช้หรือไม่ ?");
                if (result == MessageBoxResult.Yes)
                {
                    PatientOrders = null;
                }
            }


        }

        void ApplyOrderItem(SearchOrderItem orderItem)
        {
            if (SelectHealthOrganisation == null)
            {
                WarningDialog("กรุณาเลือก Order จาก");
                return;
            }

            if (orderItem.TypeOrder == "OrderSet")
            {
                OrderSetModel orderSet = DataService.MasterData.GetOrderSetByUID(orderItem.BillableItemUID);
                int ownerUID = SelectHealthOrganisation.HealthOrganisationUID;
                if (orderSet.OrderSetBillableItems != null)
                {
                    var OrderSetBillItmActive = orderSet.OrderSetBillableItems
                        .Where(p => (p.ActiveFrom == null || p.ActiveFrom.Value.Date <= DateTime.Now.Date)
                        && (p.ActiveTo == null || p.ActiveTo.Value.Date >= DateTime.Now.Date));

                    if (OrderSetBillItmActive.Any(p => p.BillingServiceMetaData == "Drug" || p.BillingServiceMetaData == "Medical Supplies" || p.BillingServiceMetaData == "Supply"))
                    {
                        WarningDialog("ไม่สามารถคีย์ OrderSet นี่ในการจัดการ Visit แบบกลุ่มได้ เนื่องจาก OrderSet นี้มีการผูกกลับคลังสินค้า กรุณาตรวจสอบ");
                        return;
                    }

                    foreach (var item in OrderSetBillItmActive)
                    {

                        PatientOrderDetailModel newOrder = new PatientOrderDetailModel();
                        BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(item.BillableItemUID);

                        if (billItem.BillingServiceMetaData != "Drug" && billItem.BillingServiceMetaData != "Medical Supplies" && billItem.BillingServiceMetaData != "Supply")
                        {
                            var orderAlready = PatientOrders.FirstOrDefault(p => p.BillableItemUID == billItem.BillableItemUID);
                            if (orderAlready != null)
                            {
                                WarningDialog("รายการ " + billItem.ItemName + " นี้มีอยู่แล้ว โปรดตรวจสอบ");
                                continue;
                            }


                            //if (billItem.BillingServiceMetaData == "Drug" || billItem.BillingServiceMetaData == "Medical Supplies")
                            //{
                            //    ItemMasterModel itemMaster = DataService.Inventory.GetItemMasterByUID(billItem.ItemUID.Value);
                            //    List<StockModel> stores = new List<StockModel>();
                            //    stores = DataService.Inventory.GetStockRemainByItemMasterUID(itemMaster.ItemMasterUID, ownerUID ?? 0);

                            //    if (stores == null || stores.Count <= 0)
                            //    {
                            //        WarningDialog("ไม่มี " + billItem.ItemName + " ในคลัง โปรดตรวจสอบ");
                            //        continue;
                            //    }
                            //    else
                            //    {
                            //        if (item.Quantity > stores.FirstOrDefault().Quantity)
                            //        {
                            //            if (itemMaster.CanDispenseWithOutStock != "Y")
                            //            {
                            //                WarningDialog("มี " + billItem.ItemName + " ในคลังไม่พอสำหรับจ่ายยา โปรดตรวจสอบ");
                            //                continue;

                            //            }
                            //            else if (itemMaster.CanDispenseWithOutStock == "Y")
                            //            {
                            //                DialogResult result = QuestionDialog("มี" + billItem.ItemName + "ในคลังไม่พอ คุณต้องการดำเนินการต่อหรือไม่ ?");
                            //                if (result == DialogResult.No || result == DialogResult.Cancel)
                            //                {
                            //                    continue;
                            //                }
                            //            }
                            //        }
                            //    }

                            //    if (item.Quantity <= 0)
                            //    {
                            //        WarningDialog("ไม่อนุญาติให้คีย์ + " + billItem.ItemName + " จำนวน < 0");
                            //        continue;
                            //    }

                            //    if (itemMaster.MinSalesQty != null && item.Quantity < itemMaster.MinSalesQty)
                            //    {
                            //        WarningDialog("คีย์จำนวน " + billItem.ItemName + " ที่ใช้น้อยกว่าจำนวนขั้นต่ำที่คีย์ได้ โปรดตรวจสอบ");
                            //        continue;
                            //    }


                            //    newOrder.IsStock = itemMaster.IsStock;
                            //    newOrder.StoreUID = stores.FirstOrDefault().StoreUID;
                            //    newOrder.DFORMUID = itemMaster.FORMMUID;
                            //    newOrder.PDSTSUID = itemMaster.PDSTSUID;
                            //    newOrder.QNUOMUID = itemMaster.BaseUOM;

                            //}

                            newOrder.OrderSetUID = item.OrderSetUID;
                            newOrder.OrderSetBillableItemUID = item.OrderSetBillableItemUID;
                            newOrder.BillableItemUID = billItem.BillableItemUID;
                            newOrder.ItemName = billItem.ItemName;
                            newOrder.BSMDDUID = billItem.BSMDDUID;
                            newOrder.ItemUID = billItem.ItemUID;
                            newOrder.ItemCode = billItem.Code;
                            newOrder.BillingService = billItem.BillingServiceMetaData;
                            newOrder.DoctorFee = item.DoctorFee;
                            newOrder.UnitPrice = item.Price;
                            newOrder.DisplayPrice = item.Price;

                            newOrder.FRQNCUID = item.FRQNCUID;
                            newOrder.Quantity = item.Quantity;
                            newOrder.Dosage = item.DoseQty;
                            newOrder.Comments = item.ProcessingNotes;
                            newOrder.IsPriceOverwrite = "N";
                            newOrder.StartDttm = DateTime.Now;

                            newOrder.NetAmount = ((item.Price) * item.Quantity);
                            newOrder.OwnerOrganisationUID = ownerUID;


                            PatientOrders.Add(newOrder);
                            OnUpdateEvent();
                        }

                    }
                }
            }
            else
            {
                PopUpOrder(orderItem.BillableItemUID);
            }
        }


        void PopUpOrder(int billableItemUID)
        {
            var orderAlready = PatientOrders.FirstOrDefault(p => p.BillableItemUID == billableItemUID);
            if (orderAlready != null)
            {
                WarningDialog("รายการ " + orderAlready.ItemName + " นี้มีอยู่แล้ว โปรดตรวจสอบ");
                return;
            }
            BillableItemModel billItem = DataService.MasterData.GetBillableItemByUID(billableItemUID);

            int ownerUID = SelectHealthOrganisation.HealthOrganisationUID;

            var billItemPrice = GetBillableItemPrice(billItem.BillableItemDetails, ownerUID);

            if (billItemPrice == null)
            {
                WarningDialog("รายการ " + billItem.ItemName + " นี้ยังไม่ได้กำหนดราคาสำหรับขาย โปรดตรวจสอบ");
                return;
            }

            billItem.Price = billItemPrice.Price;
            billItem.CURNCUID = billItemPrice.CURNCUID;

            if (billItem != null)
            {
                switch (billItem.BillingServiceMetaData)
                {
                    case "Lab Test":
                    case "Radiology":
                    case "Mobile Checkup":
                    case "Order Item":
                        {
                            OrderWithOutStockItem ordRe = new OrderWithOutStockItem(billItem, ownerUID);
                            OrderWithOutStockItemViewModel resultRe = (OrderWithOutStockItemViewModel)LaunchViewDialog(ordRe, "ORDLAB", true);
                            if (resultRe != null && resultRe.ResultDialog == ActionDialog.Save)
                            {

                                PatientOrders.Add(resultRe.PatientOrderDetail);
                                OnUpdateEvent();
                            }
                            break;
                        }
                    case "Medical Supplies":
                        //OrderMedicalItem ordMed = new OrderMedicalItem(billItem, ownerUID);
                        //OrderMedicalItemViewModel resultMed = (OrderMedicalItemViewModel)LaunchViewDialog(ordMed, "ORDMED", true);
                        //if (resultMed != null && resultMed.ResultDialog == ActionDialog.Save)
                        //{

                        //    PatientOrders.Add(resultMed.PatientOrderDetail);
                        //    OnUpdateEvent();
                        //}
                        break;
                    case "Drug":
                        //OrderDrugItem ordDrug = new OrderDrugItem(billItem, ownerUID);
                        //OrderDrugItemViewModel resultDrug = (OrderDrugItemViewModel)LaunchViewDialog(ordDrug, "ORDDRG", true);
                        //if (resultDrug != null && resultDrug.ResultDialog == ActionDialog.Save)
                        //{
                        //    PatientOrders.Add(resultDrug.PatientOrderDetail);
                        //    OnUpdateEvent();
                        //}
                        break;
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

            return selectBillItemDetail;
        }

        private void PrintAuto()
        {
            try
            {
                if (SelectPrinter == null)
                {
                    WarningDialog("กรุณาเลือก Printer");
                    return;
                }
                if (SelectReport == null)
                {
                    WarningDialog("กรุณาเลือก เอกสาร");
                    return;
                }
                if (PatientVisits != null)
                {
                    PatientVisitMass view = (PatientVisitMass)this.View;
                    int upperlimit = 0;
                    int loopCounter = 0;
                    foreach (var currentData in PatientVisits)
                    {
                        if (currentData.Select == true)
                        {
                            upperlimit++;
                        }
                    }
                    view.SetProgressBarLimits(0, upperlimit);
                    foreach (var patientVisit in PatientVisits)
                    {
                        if (patientVisit.Select)
                        {

                            var myReport = Activator.CreateInstance(Type.GetType(SelectReport.NamespaceName));
                            XtraReport report = (XtraReport)myReport;
                            ReportPrintTool printTool = new ReportPrintTool(report);
                            if (SelectReport.Name == "ใบรับรองแพทย์โควิดนอกสถานที่")
                                printTool.PrintingSystem.StartPrint += PrintingSystem_StartPrint;

                            if (SelectReport.Name == "ปริ้น Sticker" || SelectReport.Name == "ปริ้น Sticker Large")
                            {
                                report.Parameters["OrganisationUID"].Value = patientVisit.OwnerOrganisationUID;
                                report.Parameters["HN"].Value = patientVisit.PatientID;
                                report.Parameters["PatientName"].Value = patientVisit.PatientName;
                                report.Parameters["Age"].Value = patientVisit.Age;
                                report.Parameters["BirthDate"].Value = patientVisit.BirthDttm != null ? patientVisit.BirthDttm.Value.ToString("dd/MM/yyyy") : null;
                                report.Parameters["Payor"].Value = patientVisit.PayorName;
                                report.RequestParameters = false;
                                report.ShowPrintMarginsWarning = false;
                                printTool.Print(SelectPrinter.Display);
                            }
                            else
                            {
                                report.Parameters["OrganisationUID"].Value = patientVisit.OwnerOrganisationUID;
                                report.Parameters["PatientUID"].Value = patientVisit.PatientUID;
                                report.Parameters["PatientVisitUID"].Value = patientVisit.PatientVisitUID;
                                report.RequestParameters = false;
                                report.ShowPrintMarginsWarning = false;
                                printTool.Print(SelectPrinter.Display);
                            }

                            patientVisit.Select = false;
                            loopCounter = loopCounter + 1;
                            view.SetProgressBarValue(loopCounter);
                        }
                    }
                    view.SetProgressBarValue(upperlimit);
                    view.PatientGrid.RefreshData();
                    view.progressBar1.Value = 0;
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }


        }

        private void PrintingSystem_StartPrint(object sender, DevExpress.XtraPrinting.PrintDocumentEventArgs e)
        {
            e.PrintDocument.PrinterSettings.FromPage = 2;
            e.PrintDocument.PrinterSettings.ToPage = 2;
        }

        #endregion
    }
}
