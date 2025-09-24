using DevExpress.Pdf.Native.BouncyCastle.Asn1.X509;
using DevExpress.Xpf;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Reports.Operating.Cashier;
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
    public class PatientBilledOPViewModel : MediTechViewModelBase
    {
        int? opbill;
        int? ipdbill;

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
            }
        }

        private Visibility _VisibiltyOrganisations = Visibility.Collapsed;
        public Visibility VisibiltyOrganisations
        {
            get { return _VisibiltyOrganisations; }
            set { Set(ref _VisibiltyOrganisations, value); }
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
            }
        }
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

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private string _BillNumber;

        public string BillNumber
        {
            get { return _BillNumber; }
            set { Set(ref _BillNumber, value); }
        }

        private ObservableCollection<PatientBillModel> _PatientBillSource;

        public ObservableCollection<PatientBillModel> PatientBillSource
        {
            get { return _PatientBillSource; }
            set { Set(ref _PatientBillSource, value); }
        }

        private ObservableCollection<PatientBillModel> _SelectPatientBills;

        public ObservableCollection<PatientBillModel> SelectPatientBills
        {
            get
            {
                return _SelectPatientBills
                    ?? (_SelectPatientBills = new ObservableCollection<PatientBillModel>());
            }

            set { Set(ref _SelectPatientBills, value); }
        }

        private PatientBillModel _SelectPatientBill;

        public PatientBillModel SelectPatientBill
        {
            get { return _SelectPatientBill; }
            set { _SelectPatientBill = value; }
        }

        //private List<LookupReferenceValueModel> _PaymentMethods;

        //public List<LookupReferenceValueModel> PaymentMethods
        //{
        //    get { return _PaymentMethods; }
        //    set { Set(ref _PaymentMethods, value); }
        //}

        private List<LookupReferenceValueModel> _BillingCategory;

        public List<LookupReferenceValueModel> BillingCategory
        {
            get { return _BillingCategory; }
            set { Set(ref _BillingCategory, value); }
        }


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


        private List<LookUpValue> _LanguageBills;

        public List<LookUpValue> LanguageBills
        {
            get { return _LanguageBills; }
            set { Set(ref _LanguageBills, value); }
        }

        private LookUpValue _SelectLanguageBills;

        public LookUpValue SelectLanguageBills
        {
            get { return _SelectLanguageBills; }
            set { Set(ref _SelectLanguageBills, value); }
        }

        private List<LookUpValue> _BillTypes;

        public List<LookUpValue> BillTypes
        {
            get { return _BillTypes; }
            set { Set(ref _BillTypes, value); }
        }

        private LookUpValue _SelectBillType;

        public LookUpValue SelectBillType
        {
            get { return _SelectBillType; }
            set { Set(ref _SelectBillType, value); }
        }


        private HealthOrganisationModel _SelectOrganisationLogo;

        public HealthOrganisationModel SelectOrganisationLogo
        {
            get { return _SelectOrganisationLogo; }
            set
            {
                Set(ref _SelectOrganisationLogo, value);
            }
        }


        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }

        private RelayCommand _ViewBillCommand;

        public RelayCommand ViewBillCommand
        {
            get { return _ViewBillCommand ?? (_ViewBillCommand = new RelayCommand(ViewBill)); }
        }

        private RelayCommand _ViewPaymentCommand;

        public RelayCommand ViewPaymentCommand
        {
            get { return _ViewPaymentCommand ?? (_ViewPaymentCommand = new RelayCommand(ViewPayment)); }
        }

        private RelayCommand _CancelBillCommand;

        public RelayCommand CancelBillCommand
        {
            get { return _CancelBillCommand ?? (_CancelBillCommand = new RelayCommand(CancelBill)); }
        }

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPatientBill)); }
        }


        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(ClearData)); }
        }

        private RelayCommand _ExportToExcelCommand;

        public RelayCommand ExportToExcelCommand
        {
            get { return _ExportToExcelCommand ?? (_ExportToExcelCommand = new RelayCommand(ExportToExcel)); }
        }



        private RelayCommand _CancelBillMassCommand;

        public RelayCommand CancelBillMassCommand
        {
            get { return _CancelBillMassCommand ?? (_CancelBillMassCommand = new RelayCommand(CancelBillMass)); }
        }

        private RelayCommand _PaymentMassCommand;

        public RelayCommand PaymentMassCommand
        {
            get { return _PaymentMassCommand ?? (_PaymentMassCommand = new RelayCommand(PaymentMass)); }
        }

        private RelayCommand _PrintBillMassCommand;

        public RelayCommand PrintBillMassCommand
        {
            get { return _PrintBillMassCommand ?? (_PrintBillMassCommand = new RelayCommand(PrintBillMass)); }
        }
        

        #endregion

        #region Method

        public PatientBilledOPViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            BillingCategory = DataService.Technical.GetReferenceValueMany("BLCAT");
            opbill = BillingCategory.FirstOrDefault(p => p.ValueCode == "OPBILL").Key;
            ipdbill = BillingCategory.FirstOrDefault(p => p.ValueCode == "IPBILL").Key;
            Organisations = DataService.MasterData.GetHealthOrganisation();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            SelectOrganisationLogo = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

            int? cptUID = DataService.Technical.GetReferenceValueByCode("CPTYP", "FINCPT").Key;
            var fin = DataService.UserManage.GetCareproviderByUID(AppUtil.Current.UserID);
            if (fin.CPTYPUID == cptUID)
            {
                VisibiltyOrganisations = Visibility.Visible;
            }

            PrinterLists = new List<string>();
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                PrinterLists.Add(printer);
            }
            InitDataForBillReport();
        }

        void InitDataForBillReport()
        {
            LanguageBills = new List<LookUpValue>();
            BillTypes = new List<LookUpValue>();
            LanguageBills.AddRange(new List<LookUpValue>() { new LookUpValue { Value = "TH", Description = "Thai" }, new LookUpValue { Value = "EN", Description = "English" } });
            BillTypes.AddRange(new List<LookUpValue>() { new LookUpValue { Value = 0, Description = "ใบเสร็จทั่วไป" }
            , new LookUpValue { Value = 1, Description = "ใบเสร็จสำหรับประกัน" }, new LookUpValue { Value = 2, Description = "ใบเสร็จตรวจสุขภาพ" }
            , new LookUpValue { Value = 3, Description = "ใบเสร็จสำหรับ CAH" } });
        }

        public override void OnLoaded()
        {
            SearchPatientBill();
        }

        public void SearchPatientBill()
        {
            long? patientUID = null;

            if (SelectedPateintSearch != null && SearchPatientCriteria != "")
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            int? ownerOrganisationUID = SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            PatientBillSource = new ObservableCollection<PatientBillModel>(DataService.Billing.SearchPatientBill(DateFrom, DateTo, patientUID, BillNumber, "N", ownerOrganisationUID, AppUtil.Current.UserID));
        }

        public void ViewBill()
        {
            if (SelectPatientBill != null)
            {
                try
                {
                    XtraReport report;
                    if (SelectPatientBill.VisitType != "Non Medical")
                    {
                        report = new PatientBill();
                    }
                    else
                    {
                        report = new PatientBill2();
                    }
                    report.Parameters["OrganisationUID"].Value = SelectPatientBill.OwnerOrganisationUID;
                    report.Parameters["PatientBillUID"].Value = SelectPatientBill.PatientBillUID;
                    report.RequestParameters = false;

                    ReportPrintTool printTool = new ReportPrintTool(report);
                    report.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }
            }
        }

        public void ViewPayment()
        {
            if (SelectPatientBill != null)
            {
                if (SelectPatientBill.BillType == "Invoice")
                {
                    WarningDialog("บิลนี้เป็นประเภทการชำระเงินแบบวางบิล ไม่สามารถทำการแก้ไขรูปแบบการชำระเงินได้");
                    return;
                }

                if (SelectPatientBill.IsCancel)
                {
                    ListPaymentDetails view = new ListPaymentDetails();
                    (view.DataContext as ListPaymentDetailsViewModel).PatientBillUID = SelectPatientBill.PatientBillUID;
                    LaunchViewDialogNonPermiss(view, true);
                }
                else
                {
                    ListBillPayment pageview = new ListBillPayment();
                    var viewModel = (pageview.DataContext as ListBillPaymentViewModel);
                    var paymentDetailsList = DataService.Billing.GetPatientPaymentDetailByBillUID(SelectPatientBill.PatientBillUID);
                    var paidDttm = paymentDetailsList.FirstOrDefault()?.PaidDttm ?? DateTime.Now;
                    viewModel.TotalAmount = SelectPatientBill.NetAmount ?? 0;
                    viewModel.PaymentDetailsList = new ObservableCollection<PatientPaymentDetailModel>(paymentDetailsList);
                    ListBillPaymentViewModel result = (ListBillPaymentViewModel)LaunchViewDialogNonPermiss(pageview, true);
                    if (result.ResultDialog == ActionDialog.Save)
                    {
                        foreach (var item in result.PaymentDetailsList)
                        {
                            item.PatientBillUID = SelectPatientBill.PatientBillUID;
                            item.PatientUID = SelectPatientBill.PatientUID;
                            item.PatientVisitUID = SelectPatientBill.PatientVisitUID;
                            item.PaidDttm = paidDttm;
                        }

                        List<PatientPaymentDetailModel> patientPaymentDetails = new List<PatientPaymentDetailModel>();
                        patientPaymentDetails.AddRange(result.PaymentDetailsList);
                        if (result.DeletedPaymentDataLists != null && result.DeletedPaymentDataLists.Count > 0)
                        {
                            patientPaymentDetails.AddRange(result.DeletedPaymentDataLists);
                        }

                        DataService.Billing.ManagePatientPaymentDetail(patientPaymentDetails, AppUtil.Current.UserID);
                        SearchPatientBill();
                    }
                }

            }
        }
        public void CancelBill()
        {
            if (SelectPatientBill != null)
            {
                if (SelectPatientBill.IsCancel)
                {
                    WarningDialog("รายการนี้ถูกยกเลิกไปแล้ว");
                    return;
                }
                try
                {
                    var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientBill.PatientVisitUID);
                    CancelBillPopup cancelPopup = new CancelBillPopup();
                    (cancelPopup.DataContext as CancelBillPopupViewModel).AssignPatientVisit(patientVisit);
                    CancelBillPopupViewModel result = (CancelBillPopupViewModel)LaunchViewDialog(cancelPopup, "CANBILLOP", true);
                    if (result != null && result.ResultDialog == ActionDialog.Save)
                    {
                        SearchPatientBill();
                    }

                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }
        }

        public void UpdatePatientBill(long patientBillUID, int PAYMDUID)
        {
            try
            {
                DataService.Billing.UpdatePaymentMethod(patientBillUID, PAYMDUID, AppUtil.Current.UserID);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void ClearData()
        {
            BillNumber = string.Empty;
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            SearchPatientCriteria = string.Empty;
            PatientBillSource = null;
            SelectOrganisation = null;
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "", "");
                PatientsSearchSource = searchResult;
            }
            else
            {
                PatientsSearchSource = null;
            }

        }

        private void CancelBillMass()
        {
            try
            {
                if (SelectPatientBills != null)
                {
                    var billUnCancel = SelectPatientBills.Where(p => !p.IsCancel).ToList();
                    if (billUnCancel != null && billUnCancel.Count() > 0)
                    {
                        CancelPopup cancelPO = new CancelPopup();
                        CancelPopupViewModel result = (CancelPopupViewModel)LaunchViewDialog(cancelPO, "CANISS", true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            if (string.IsNullOrEmpty(result.Comments))
                            {
                                WarningDialog("กรุณาระบุเหตุผลในการยกเลิกให้ครบถ้วน");
                                return;
                            }

                            foreach (var bill in billUnCancel)
                            {
                                bill.Comments = result.Comments;
                            }

                            DataService.Billing.CancelBillLists(billUnCancel);
                            SelectPatientBills = null;
                            SearchPatientBill();
                        }

                    }

                }
            }
            catch (Exception ex)
            {


                ErrorDialog(ex.Message);
            }
        }

        private void PaymentMass()
        {
            try
            {
                if (SelectPatientBills != null)
                {
                    var billUnCancel = SelectPatientBills.Where(p => !p.IsCancel && p.BillType != "Invoice").ToList();
                    if (billUnCancel != null && billUnCancel.Count() > 0)
                    {
                        ListBillPayment pageview = new ListBillPayment();
                        var viewModel = (pageview.DataContext as ListBillPaymentViewModel);
                        viewModel.TotalAmount = SelectPatientBill.NetAmount ?? 0;
                        ListBillPaymentViewModel result = (ListBillPaymentViewModel)LaunchViewDialogNonPermiss(pageview, true);
                        if (result.ResultDialog == ActionDialog.Save)
                        {
                            List<PatientPaymentDetailModel> PaymentDetailsList = new List<PatientPaymentDetailModel>();
                            foreach (var bill in billUnCancel)
                            {
                                foreach (var item in result.PaymentDetailsList)
                                {
                                    PatientPaymentDetailModel newPayment =new PatientPaymentDetailModel();
                                    newPayment.PatientBillUID = bill.PatientBillUID;
                                    newPayment.PatientUID = bill.PatientUID;
                                    newPayment.PatientVisitUID = bill.PatientVisitUID;
                                    newPayment.PaidDttm = bill.BillGeneratedDttm ?? DateTime.Now;

                                    newPayment.PAYMDUID = item.PAYMDUID;
                                    newPayment.PaymentMode = item.PaymentMode;
                                    newPayment.Amount = item.Amount;
                                    newPayment.CURNCUID = item.CURNCUID;
                                    newPayment.Currency = item.Currency;
                                    newPayment.PaidDttm = item.PaidDttm;
                                    newPayment.CardExpiryDttm = item.CardExpiryDttm;
                                    newPayment.BankName = item.BankName;
                                    newPayment.CardNumber = item.CardNumber;
                                    newPayment.CRDTYUID = item.CRDTYUID;
                                    newPayment.CardType = item.CardType;
                                    newPayment.AuthorizationNumber = item.AuthorizationNumber;
                                    newPayment.StatusFlag = "A";
                                    newPayment.OwnerOrganisationUID = bill.OwnerOrganisationUID;
                                    PaymentDetailsList.Add(newPayment);
                                }
                            }
         



                            DataService.Billing.ManagePatientPaymentDetailForMass(PaymentDetailsList, AppUtil.Current.UserID);
                            SearchPatientBill();
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        private void PrintBillMass()
        {
            try
            {

                if (SelectPatientBills != null)
                {
                    if (string.IsNullOrEmpty(SelectPrinter))
                    {
                        WarningDialog("กรุณาเลือก ปริ้นเตอร์");
                        return;
                    }
                    if (SelectLanguageBills == null)
                    {
                        WarningDialog("กรุณาเลือก ภาษา");
                        return;
                    }

                    if (SelectOrganisationLogo == null)
                    {
                        WarningDialog("กรุณาเลือก Logo");
                        return;
                    }


                    if (SelectBillType == null)
                    {
                        WarningDialog("กรุณาเลือกรูปแบบ รายงาน");
                        return;
                    }
                    //var billUnCancel = SelectPatientBills.Where(p => !p.IsCancel).ToList();
                    if (SelectPatientBills != null && SelectPatientBills.Count() > 0)
                    {
                        foreach (var bill in SelectPatientBills)
                        {
                            XtraReport report;
                            if (SelectPatientBill.VisitType != "Non Medical")
                            {
                                report = new PatientBill();
                            }
                            else
                            {
                                report = new PatientBill2();
                            }
                            report.Parameters["OrganisationUID"].Value = bill.OwnerOrganisationUID;
                            report.Parameters["PatientBillUID"].Value = bill.PatientBillUID;

                            report.Parameters["ReportType"].Value = SelectBillType.Value;
                            report.Parameters["LogoBillType"].Value = SelectOrganisationLogo.HealthOrganisationUID;
                            report.Parameters["LangType"].Value = SelectLanguageBills.Value;
                            report.RequestParameters = false;

                            ReportPrintTool printTool = new ReportPrintTool(report);
                            report.ShowPrintMarginsWarning = false;
                            printTool.Print();
                        }

                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        private void ExportToExcel()
        {
            try
            {
                if (PatientBillSource != null)
                {
                    string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                    if (fileName != "")
                    {
                        PatientBilledOP view = (PatientBilledOP)this.View;
                        view.gvPatBill.ExportToXlsx(fileName);
                        OpenFile(fileName);
                    }

                }
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }

        }

        #endregion

    }
}
