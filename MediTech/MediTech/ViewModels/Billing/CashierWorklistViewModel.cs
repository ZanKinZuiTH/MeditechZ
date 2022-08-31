using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
using MediTech.Reports.Operating.Cashier;
using MediTech.Reports.Operating.Pharmacy;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class CashierWorklistViewModel : MediTechViewModelBase
    {

        #region Properties
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

        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
                if (_DateFrom != null)
                {
                    //SearchPatientVisit();
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
                if (_DateTo != null)
                {
                    //SearchPatientVisit();
                }
            }
        }

        private bool _IsSumDrug = true;

        public bool IsSumDrug
        {
            get { return _IsSumDrug; }
            set
            {
                Set(ref _IsSumDrug, value);
            }
        }

        private DateTime _FinanceDate;

        public DateTime FinanceDate
        {
            get { return _FinanceDate; }
            set { Set(ref _FinanceDate, value); }
        }

        private DateTime _FinanceTime;
        public DateTime FinanceTime
        {
            get { return _FinanceTime; }
            set { Set(ref _FinanceTime, value); }
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
                    SearchPatientVisit();
                }
            }
        }

        #endregion


        private List<string> _PrinterLists;

        public List<string> PrinterLists
        {
            get { return _PrinterLists; }
            set { Set(ref _PrinterLists, value); }
        }

        private string _SelectPrinter;

        public string SelectPrinter
        {
            get { return _SelectPrinter; }
            set { Set(ref _SelectPrinter, value); }
        }


        private List<PatientVisitModel> _PatientCloseMed;
        public List<PatientVisitModel> PatientCloseMed
        {
            get { return _PatientCloseMed; }
            set { Set(ref _PatientCloseMed, value); }
        }

        private PatientVisitModel _SelectPatientCloseMed;
        public PatientVisitModel SelectPatientCloseMed
        {
            get { return _SelectPatientCloseMed; }
            set
            {
                Set(ref _SelectPatientCloseMed, value);
                if (_SelectPatientCloseMed != null)
                {
                    GetOrderAll();
                }
                else
                {
                    TotalPrice = 0;
                    OrderAllList = null;
                    StoreOrderList = null;
                    DiscountAmount = 0;
                    TotalAmount = 0;
                    Payment = 0;
                    ReCash = 0;
                }
            }
        }


        private ObservableCollection<StoreItemList> _StoreOrderList;
        public ObservableCollection<StoreItemList> StoreOrderList
        {
            get { return _StoreOrderList; }
            set { Set(ref _StoreOrderList, value); }
        }

        private List<PatientOrderDetailModel> _SelectItemStore;

        public List<PatientOrderDetailModel> SelectItemStore
        {
            get { return _SelectItemStore ?? (SelectItemStore = new List<PatientOrderDetailModel>()); }
            set { Set(ref _SelectItemStore, value); }
        }


        private ObservableCollection<PatientBilledItemModel> _OrderAllList;
        public ObservableCollection<PatientBilledItemModel> OrderAllList
        {
            get { return _OrderAllList; }
            set { Set(ref _OrderAllList, value); }
        }

        private double _TotalPrice;
        public double TotalPrice
        {
            get { return _TotalPrice; }
            set
            {
                Set(ref _TotalPrice, value);
                //TotalAmount = CalTotalAmount(TotalPrice, DiscountAmount);
            }
        }

        private double _DiscountAmount;

        public double DiscountAmount
        {
            get { return _DiscountAmount; }
            set
            {
                Set(ref _DiscountAmount, value);
                //TotalAmount = CalTotalAmount(TotalPrice, DiscountAmount);
            }
        }

        private double _TotalAmount;

        public double TotalAmount
        {
            get { return _TotalAmount; }
            set { Set(ref _TotalAmount, value); }
        }


        private double _Payment;
        public double Payment
        {
            get { return _Payment; }
            set
            {
                Set(ref _Payment, value);
                ReCash = CalReCash(Payment, TotalAmount);

            }
        }

        private double _ReCash;
        public double ReCash
        {
            get { return _ReCash; }
            set { Set(ref _ReCash, value); }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }

        private bool _IsPrint;

        public bool IsPrint
        {
            get { return _IsPrint; }
            set { Set(ref _IsPrint, value); }
        }

        private bool _SelectAll;

        public bool SelectAll
        {
            get { return _SelectAll; }
            set
            {
                Set(ref _SelectAll, value);
                foreach (var item in StoreOrderList)
                {
                    item.IsSelected = _SelectAll;
                }
                (this.View as CashierWorklist).gDrugDetail.RefreshData();
            }
        }

        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _RefershPatientCommand;

        public RelayCommand RefershPatientCommand
        {
            get { return _RefershPatientCommand ?? (_RefershPatientCommand = new RelayCommand(RefershPatient)); }
        }

        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _ReMedicalCommand;

        public RelayCommand ReMedicalCommand
        {
            get { return _ReMedicalCommand ?? (_ReMedicalCommand = new RelayCommand(ReMedical)); }
        }

        private RelayCommand _PrintDrugStickerCommand;

        public RelayCommand PrintDrugStickerCommand
        {
            get { return _PrintDrugStickerCommand ?? (_PrintDrugStickerCommand = new RelayCommand(PrintDrugSticker)); }
        }

        private RelayCommand _CreateOrderCommand;

        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
        }



        private RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> _ChangeValueCommand;
        public RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs> ChangeValueCommand
        {
            get { return _ChangeValueCommand ?? (_ChangeValueCommand = new RelayCommand<DevExpress.Xpf.Grid.CellValueChangedEventArgs>(ChangeValue)); }
        }


        #endregion

        #region Method


        #region Varible

        List<PatientOrderDetailModel> orderAll;

        int REGST = 417;
        int CHKOUT = 418;
        int SNDDOC = 419;
        int FINDIS = 421;
        #endregion

        public CashierWorklistViewModel()
        {
            IsPrint = false;
            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            PayorDetails = DataService.Billing.GetPayorDetail();
            PrinterLists = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterLists.Add(printer);
            }


            DateFrom = DateTime.Now.Date;
            RefershPatient();
        }

        private void RefershPatient()
        {
            SearchPatientVisit();
        }

        private void Save()
        {
            if (SelectPatientCloseMed != null)
            {
                try
                {
                    if (Payment < 0)
                    {
                        WarningDialog("กรุณาใส่จำนวนเงินที่รับมา");
                        return;
                    }
                    if (ReCash < 0)
                    {
                        WarningDialog("กรุณาตรวจสอบจำนวนเงินที่รับมา");
                        return;
                    }

                    GetStoreItem();

                    if ((StoreOrderList == null || StoreOrderList.Count == 0)
                        && (OrderAllList != null && OrderAllList.Count(p => p.BillingService == "Drug"
            || p.BillingService == "Medical Supplies"
            || p.BillingService == "Supply") > 0)
                        )
                    {
                        WarningDialog("ไม่มีคลังสินค้า โปรดตรวจสอบ หรือ ติดต่อ Admin");
                        return;
                    }


                    foreach (var orderStore in OrderAllList.Where(p => p.BillingService == "Drug"
            || p.BillingService == "Medical Supplies"
            || p.BillingService == "Supply"))
                    {
                        if (!StoreOrderList.Any(p => p.BillableItemUID == orderStore.BillableItemUID))
                        {
                            WarningDialog("รายการ " + orderStore.ItemName + " ไม่มีในคลังสินค้า โปรดตรวจสอบ หรือ ติดต่อ Admin");
                            return;
                        }
                    }

                    if (StoreOrderList != null)
                    {
                        foreach (var storeItem in StoreOrderList)
                        {
                            if (storeItem.StockUID == null || storeItem.StockUID == 0)
                            {
                                WarningDialog("รายการ " + storeItem.ItemName + " ไม่มีในคลังสินค้า โปรดตรวจสอบ หรือ ติดต่อ Admin");
                                return;
                            }
                            else if (storeItem.Quantity > storeItem.BalQty)
                            {
                                WarningDialog("มี " + storeItem.ItemName + " ในคลัง ไม่พอสำหรับจ่ายยา โปรดตรวจสอบ");
                                return;
                            }
                        }
                    }

                    if (Payment == 0)
                    {
                        MessageBoxResult result = QuestionDialog("จำนวนเงินที่รับเท่ากับ 0 คุณต้องการดำเนินการต่อหรือไม่ ?");
                        if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                    }



                    PatientBillModel patbill = new PatientBillModel();
                    patbill.PatientBilledItems = new List<PatientBilledItemModel>();


                    //patbill.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                    patbill.OwnerOrganisationUID = SelectPatientCloseMed.OwnerOrganisationUID.Value;
                    patbill.CUser = AppUtil.Current.UserID;
                    patbill.MUser = AppUtil.Current.UserID;
                    patbill.PaidAmount = Payment;
                    patbill.Comments = Comments;
                    patbill.ChangeAmount = ReCash;
                    patbill.PatientName = SelectPatientCloseMed.PatientName;
                    patbill.PatientUID = SelectPatientCloseMed.PatientUID;
                    patbill.PatientVisitUID = SelectPatientCloseMed.PatientVisitUID;
                    patbill.PatientID = SelectPatientCloseMed.PatientID;
                    patbill.VisitID = SelectPatientCloseMed.VisitID;
                    patbill.VisitDttm = SelectPatientCloseMed.StartDttm.Value;
                    patbill.TotalAmount = TotalPrice;
                    patbill.DiscountAmount = DiscountAmount;
                    patbill.NetAmount = TotalAmount;

                    patbill.PatientBilledItems.AddRange(OrderAllList);


                    PatientBillModel patBillResult = DataService.Billing.OLDGeneratePatientBill(patbill);
                    SaveSuccessDialog();

                    if (IsPrint)
                    {

                        XtraReport report;
                        //var selectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == SelectPatientCloseMed.OwnerOrganisationUID);
                        if (SelectPatientCloseMed.VisitType != "Non Medical")
                        {
                            report = new PatientBill();
                        }
                        else
                        {
                            report = new PatientBill2();
                        }

                        report.RequestParameters = false;
                        report.Parameters["OrganisationUID"].Value = patBillResult.OwnerOrganisationUID;
                        report.Parameters["PatientBillUID"].Value = patBillResult.PatientBillUID;
                        ReportPrintTool printTool = new ReportPrintTool(report);
                        report.ShowPrintMarginsWarning = false;
                        printTool.ShowPreviewDialog();
                    }


                    SelectPatientCloseMed = null;
                    SearchPatientVisit();
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }
            }

        }

        private void SearchPatientVisit()
        {
            string hn = "";
            if (SearchPatientCriteria != "" && SelectedPateintSearch != null)
            {
                hn = SelectedPateintSearch.PatientID;
            }

            int? payorDetailUID = SelectPayorDetail != null ? SelectPayorDetail.PayorDetailUID : (int?)null;
            int? ownerOrganisationUID = (SelectOrganisation != null) ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            var visitData = DataService.PatientIdentity.SearchPatientMedicalDischarge(hn, "", "", null, DateFrom, DateTo, ownerOrganisationUID, payorDetailUID);
            PatientCloseMed = visitData;
        }

        private void ReMedical()
        {
            try
            {
                if (SelectPatientCloseMed != null)
                {
                    var currentPatientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientCloseMed.PatientVisitUID);
                    if (currentPatientVisit.VISTSUID != CHKOUT)
                    {
                        WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน โปรดตรวจสอบ หรือ Refersh ข้อมูล");
                        return;
                    }
                    MessageBoxResult result = QuestionDialog("คูณต้องการส่งผู้ป่วยกลับไปรักษาใช่หรือไม่ ?");
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.PatientIdentity.ChangeVisitStatus(SelectPatientCloseMed.PatientVisitUID, 417, SelectPatientCloseMed.CareProviderUID,AppUtil.Current.LocationUID, null, AppUtil.Current.UserID, null, null);
                        SaveSuccessDialog();
                        SelectPatientCloseMed = null;
                        SearchPatientVisit();
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }


        }

        public void ChangeValue(DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

            foreach (var item in OrderAllList)
            {
                double NetAmount = orderAll.FirstOrDefault(p => p.PatientOrderDetailUID == item.PatientOrderDetailUID).NetAmount ?? 0;
                if (item.Discount == null || item.Discount <= 0)
                {
                    item.NetAmount = NetAmount;
                }
                else
                {
                    item.NetAmount = NetAmount - item.Discount;
                }
            }

            TotalPrice = OrderAllList.Sum(p => p.Amount) ?? 0;
            DiscountAmount = OrderAllList.Sum(p => p.Discount) ?? 0;
            TotalAmount = Math.Round(OrderAllList.Sum(p => p.NetAmount) ?? 0,2);
            OnUpdateEvent();
        }

        public double CalReCash(double cash, double total)
        {
            double result = 0;
            if (cash != 0 && total != 0)
            {
                result = cash - total;
            }

            return result;
        }

        public double CalTotalAmount(double price, double discount)
        {
            double result = 0;
            result = price - discount;

            return result;
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, null, "");
                PatientsSearchSource = searchResult;
            }
            else
            {

                PatientsSearchSource = null;

            }

        }

        private void PrintDrugSticker()
        {
            if (SelectPrinter == null)
            {
                WarningDialog("กรุณาเลือก Printer");
                return;
            }
            var printStickers = StoreOrderList.Where(p => p.IsSelected);
            if (printStickers != null && printStickers.Count() > 0)
            {
                var groupItemStore = printStickers.GroupBy(p => new
                {
                    p.IdentifyingUID,
                    p.ItemName,
                    p.OwnerOrganisationUID
                })
                     .Select(
                     g => new
                     {
                         PrescriptionItemUID = g.FirstOrDefault().IdentifyingUID,
                         ExpiryDate = g.Max(expy => expy.ExpiryDate),
                         OrganisationUID = g.FirstOrDefault().OwnerOrganisationUID
                     });


                foreach (var item in groupItemStore)
                {
                    DrugSticker rpt = new DrugSticker();
                    ReportPrintTool printTool = new ReportPrintTool(rpt);

                    rpt.Parameters["OrganisationUID"].Value = item.OrganisationUID;
                    rpt.Parameters["PrescriptionItemUID"].Value = item.PrescriptionItemUID;
                    rpt.Parameters["ExpiryDate"].Value = item.ExpiryDate;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.Print(SelectPrinter);
                }
            }
        }

        private void CreateOrder()
        {
            if (SelectPatientCloseMed != null)
            {
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(SelectPatientCloseMed);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);

                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    GetOrderAll();
                }
            }
        }

        private void GetOrderAll()
        {
            orderAll = new List<PatientOrderDetailModel>();

            orderAll = DataService.OrderProcessing.GetOrderAllByVisitUID(SelectPatientCloseMed.PatientVisitUID);
            orderAll = orderAll.Where(p => p.ORDSTUID != 2848).ToList();
            OrderAllList = new ObservableCollection<PatientBilledItemModel>(orderAll.Select(p => new PatientBilledItemModel
            {
                BillableItemUID = p.BillableItemUID,
                PatientOrderDetailUID = p.PatientOrderDetailUID,
                ItemCode = p.ItemCode,
                ItemName = p.ItemName,
                Amount = p.NetAmount,
                Discount = p.Discount ?? 0,
                NetAmount = p.NetAmount,
                DoctorFee = p.DoctorFee,
                CareproviderUID = p.CareproviderUID,
                ItemMultiplier = p.Quantity ?? 1,
                BSMDDUID = p.BSMDDUID,
                IdentifyingUID = p.IdentifyingUID,
                BillingService = p.BillingService,
                OwnerOrganisationUID = p.OwnerOrganisationUID
            }));


            GetStoreItem();


            TotalPrice = OrderAllList.Sum(p => p.Amount) ?? 0;
            DiscountAmount = OrderAllList.Sum(p => p.Discount) ?? 0;
            TotalAmount = OrderAllList.Sum(p => p.NetAmount) ?? 0;
            Payment = 0;
            ReCash = 0;

            OnUpdateEvent();
        }

        private void GetStoreItem()
        {
            try
            {
                StoreOrderList = new ObservableCollection<StoreItemList>();
                List<PatientOrderDetailModel> drugDetails = new List<PatientOrderDetailModel>();
                var TempDrugList = orderAll.Where(p => p.BillingService == "Drug"
                || p.BillingService == "Medical Supplies"
                || p.BillingService == "Supply");

                StoreOrderList = new ObservableCollection<StoreItemList>(
    TempDrugList
    .GroupBy(p => new { p.ItemUID, p.QNUOMUID, p.StoreUID })
    .Select(
    g => new StoreItemList
    {
        Quantity = g.Sum(p => p.Quantity),
        ItemCode = g.FirstOrDefault().ItemCode,
        ItemName = g.FirstOrDefault().ItemName,
        ItemUID = g.FirstOrDefault().ItemUID,
        QNUOMUID = g.FirstOrDefault().QNUOMUID,
        StoreUID = g.FirstOrDefault().StoreUID,
        QuantityUnit = g.FirstOrDefault().QuantityUnit,
        BillableItemUID = g.FirstOrDefault().BillableItemUID,
        InstructionRoute = g.FirstOrDefault(p => !string.IsNullOrEmpty(p.InstructionRoute)) != null ? g.FirstOrDefault(p => !string.IsNullOrEmpty(p.InstructionRoute)).InstructionRoute : "",
        Dosage = g.FirstOrDefault().Dosage,
        DrugFrequency = g.FirstOrDefault(p => !string.IsNullOrEmpty(p.DrugFrequency)) != null ? g.FirstOrDefault(p => !string.IsNullOrEmpty(p.DrugFrequency)).DrugFrequency : "",
        LocalInstructionText = g.FirstOrDefault(p => !string.IsNullOrEmpty(p.LocalInstructionText)) != null ? g.FirstOrDefault(p => !string.IsNullOrEmpty(p.LocalInstructionText)).LocalInstructionText : "",
        ClinicalComments = g.FirstOrDefault(p => !string.IsNullOrEmpty(p.ClinicalComments)) != null ? g.FirstOrDefault(p => !string.IsNullOrEmpty(p.ClinicalComments)).ClinicalComments : "",
        TypeDrug = g.FirstOrDefault().TypeDrug,
        IdentifyingUID = g.FirstOrDefault().IdentifyingUID,
        BillingService = g.FirstOrDefault().BillingService,
        OwnerOrganisationUID = g.FirstOrDefault().OwnerOrganisationUID
    }));
                foreach (var stockItem in StoreOrderList)
                {
                    var storeUsed = DataService.Pharmacy.GetDrugStoreDispense(stockItem.ItemUID ?? 0, stockItem.Quantity ?? 0, stockItem.QNUOMUID ?? 0, stockItem.StoreUID ?? 0);
                    foreach (var item in storeUsed)
                    {
                        if (item.Quantity > item.BalQty)
                        {
                            item.IsWithoutStock = true;
                        }

                        if (item.ExpiryDate?.Date <= DateTime.Now.Date)
                        {
                            item.IsExpired = true;
                        }
                    }
                    stockItem.StockUID = storeUsed.FirstOrDefault() != null ? storeUsed.FirstOrDefault().StockUID : null;
                    stockItem.StoreStockItem = new ObservableCollection<PatientOrderDetailModel>(storeUsed);
                    stockItem.BalQty = storeUsed.Sum(p => p.BalQty);
                    stockItem.ExpiryDate = storeUsed.FirstOrDefault() != null ? storeUsed.FirstOrDefault().ExpiryDate : null;
                    stockItem.IsWithoutStock = storeUsed.FirstOrDefault(p => p.IsWithoutStock == true) != null ? true : false;
                }


            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }

            //StoreOrderList = new ObservableCollection<StoreItemList>();
            //List<PatientOrderDetailModel> drugDetails = new List<PatientOrderDetailModel>();
            //var TempDrugList = orderAll.Where(p => p.BillingService == "Drug"
            //|| p.BillingService == "Medical Supplies"
            //|| p.BillingService == "Supply");



            //foreach (var drugItem in TempDrugList)
            //{
            //    var storeUsed = DataService.Pharmacy.GetDrugStoreDispense(drugItem.IdentifyingUID ?? 0);
            //    foreach (var item in storeUsed)
            //    {
            //        var dupicateStock = drugDetails.FirstOrDefault(p => p.StockUID == item.StockUID);
            //        if (dupicateStock == null)
            //        {
            //            item.ItemCode = drugItem.ItemCode;
            //            item.UnitPrice = drugItem.UnitPrice;
            //            item.BillingService = drugItem.BillingService;
            //            item.BillableItemUID = drugItem.BillableItemUID;

            //            if (item.Quantity > item.BalQty)
            //            {
            //                item.IsWithoutStock = true;
            //            }

            //            if (item.ExpiryDate?.Date <= DateTime.Now.Date)
            //            {
            //                item.IsExpired = true;
            //            }

            //            drugDetails.Add(item);
            //        }
            //        else
            //        {
            //            dupicateStock.Quantity += drugItem.Quantity;
            //            if (dupicateStock.Quantity > dupicateStock.BalQty)
            //            {
            //                dupicateStock.IsWithoutStock = true;
            //            }
            //        }

            //    }
            //}

            //StoreOrderList = new ObservableCollection<StoreItemList>(
            //    drugDetails
            //    .GroupBy(p => new { p.BillableItemUID })
            //    .Select(
            //         g => new StoreItemList
            //         {
            //             Quantity = g.Sum(p => p.Quantity),
            //             BalQty = g.Sum(p => p.BalQty),
            //             StockUID = g.FirstOrDefault().StockUID,
            //             ItemCode = g.FirstOrDefault().ItemCode,
            //             ItemName = g.FirstOrDefault().ItemName,
            //             QuantityUnit = g.FirstOrDefault().QuantityUnit,
            //             BillableItemUID = g.FirstOrDefault().BillableItemUID,
            //             ExpiryDate = g.FirstOrDefault().ExpiryDate,
            //             IsWithoutStock = g.FirstOrDefault(p => p.IsWithoutStock == true) != null ? true : false,
            //             IsExpired = g.FirstOrDefault().IsExpired,
            //             InstructionRoute = g.FirstOrDefault().InstructionRoute,
            //             Dosage = g.FirstOrDefault().Dosage,
            //             DrugFrequency = g.FirstOrDefault().DrugFrequency,
            //             LocalInstructionText = g.FirstOrDefault().LocalInstructionText,
            //             ClinicalComments = g.FirstOrDefault().ClinicalComments,
            //             TypeDrug = g.FirstOrDefault().TypeDrug,
            //             IdentifyingUID = g.FirstOrDefault().IdentifyingUID,
            //             BillingService = g.FirstOrDefault().BillingService
            //         }));

            //foreach (var item in StoreOrderList)
            //{
            //    item.StoreStockItem = new ObservableCollection<PatientOrderDetailModel>();
            //    item.StoreStockItem = new ObservableCollection<PatientOrderDetailModel>(
            //        drugDetails.Where(p => p.BillableItemUID == item.BillableItemUID)
            //       .GroupBy(p => new { p.BillableItemUID, p.StoreUID, p.BatchID }).Select(
            //         g => new StoreItemList
            //         {
            //             Quantity = g.Sum(p => p.Quantity),
            //             ItemCode = g.FirstOrDefault().ItemCode,
            //             ItemName = g.FirstOrDefault().ItemName,
            //             QuantityUnit = g.FirstOrDefault().QuantityUnit,
            //             BalQty = g.Sum(p => p.BalQty),
            //             BillableItemUID = g.FirstOrDefault().BillableItemUID,
            //             StockUID = g.FirstOrDefault().StockUID,
            //             StoreUID = g.FirstOrDefault().StoreUID,
            //             ExpiryDate = g.FirstOrDefault().ExpiryDate,
            //             BatchID = g.FirstOrDefault().BatchID,
            //             IsWithoutStock = g.FirstOrDefault().IsWithoutStock,
            //             IsExpired = g.FirstOrDefault().IsExpired,
            //             StoreName = g.FirstOrDefault().StoreName,
            //             InstructionRoute = g.FirstOrDefault().InstructionRoute,
            //             Dosage = g.FirstOrDefault().Dosage,
            //             DrugFrequency = g.FirstOrDefault().DrugFrequency,
            //             LocalInstructionText = g.FirstOrDefault().LocalInstructionText,
            //             ClinicalComments = g.FirstOrDefault().ClinicalComments,
            //             TypeDrug = g.FirstOrDefault().TypeDrug,
            //             IdentifyingUID = g.FirstOrDefault().IdentifyingUID,
            //             BillingService = g.FirstOrDefault().BillingService
            //         }));
            //}

        }
        #endregion
    }
}
