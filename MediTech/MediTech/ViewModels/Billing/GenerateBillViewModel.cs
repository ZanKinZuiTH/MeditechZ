using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
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

namespace MediTech.ViewModels
{
    public class GenerateBillViewModel : MediTechViewModelBase
    {
        #region Properties

        int INVOC = 2899;
        int RECEI = 2900;
        int defaultCURNC = 2811;
        int CASHH = 2933;
        private int _currentIndex = 0;


        private PatientVisitModel _SelectPateintVisit;

        public PatientVisitModel SelectPateintVisit
        {
            get { return _SelectPateintVisit; }
            set { Set(ref _SelectPateintVisit, value); }
        }

        private List<PatientVisitPayorModel> _PatientVisitPayors;

        public List<PatientVisitPayorModel> PatientVisitPayors
        {
            get { return _PatientVisitPayors; }
            set { Set(ref _PatientVisitPayors, value); }
        }


        private PatientVisitPayorModel _SelectPatientVisitPayor;

        public PatientVisitPayorModel SelectPatientVisitPayor
        {
            get { return _SelectPatientVisitPayor; }
            set
            {
                Set(ref _SelectPatientVisitPayor, value);
                if (_SelectPatientVisitPayor != null)
                {
                    Amount = null;
                    DiscountAmount = null;
                    NetAmount = null;
                    PayorAmount = null;
                    PaymentAmount = null;
                    PaidAmount = null;
                    BalanceAmount = null;

                    var patBillableItems = DataService.Billing.GetPatientBillableItemsAccount(SelectPateintVisit.PatientUID, SelectPateintVisit.PatientVisitUID, null, _SelectPatientVisitPayor.PatientVisitPayorUID
                        , SelectPateintVisit.StartDttm.Value, DateTime.Now, null, null, null);
                    PatientBillableItemsAccounts = new ObservableCollection<AllocatedPatBillableItemsAccountResultModel>(patBillableItems);

                    if (PatientBillableItemsAccounts != null && PatientBillableItemsAccounts.Count > 0)
                    {
                        Amount = PatientBillableItemsAccounts.Sum(p => p.Amount ?? 0);
                        DiscountAmount = PatientBillableItemsAccounts.Sum(p => p.Discount ?? 0);
                        NetAmount = PatientBillableItemsAccounts.Sum(p => p.NetAmount ?? 0);

                        calculateBalance();
                    }

                    if (_SelectPatientVisitPayor.PBTYPUID == INVOC)
                    {
                        PayorAmount = NetAmount;
                        IsEnablePayment = Visibility.Collapsed;
                    }
                    else
                    {
                        PayorAmount = null;
                        IsEnablePayment = Visibility.Visible;
                    }

                }
            }
        }

        private ObservableCollection<AllocatedPatBillableItemsAccountResultModel> _PatientBillableItemsAccounts;

        public ObservableCollection<AllocatedPatBillableItemsAccountResultModel> PatientBillableItemsAccounts
        {
            get { return _PatientBillableItemsAccounts; }
            set { Set(ref _PatientBillableItemsAccounts, value); }
        }

        private Visibility _IsEnablePayment;

        public Visibility IsEnablePayment
        {
            get { return _IsEnablePayment; }
            set { Set(ref _IsEnablePayment, value); }
        }

        private Visibility _IsEnablePaymentOnly;

        public Visibility IsEnablePaymentOnly
        {
            get { return _IsEnablePaymentOnly; }
            set { Set(ref _IsEnablePaymentOnly, value); }
        }


        private double? _Amount;

        public double? Amount
        {
            get { return _Amount; }
            set { Set(ref _Amount, value); }
        }

        private double? _DiscountAmount;

        public double? DiscountAmount
        {
            get { return _DiscountAmount; }
            set { Set(ref _DiscountAmount, value); }
        }

        private double? _NetAmount;

        public double? NetAmount
        {
            get { return _NetAmount; }
            set { Set(ref _NetAmount, value); }
        }

        private double? _PayorAmount;

        public double? PayorAmount
        {
            get { return _PayorAmount; }
            set { Set(ref _PayorAmount, value); }
        }

        private double? _PaymentAmount;

        public double? PaymentAmount
        {
            get { return _PaymentAmount; }
            set
            {
                Set(ref _PaymentAmount, value);
                if (PaymentAmount != null)
                {
                    IsEnablePaymentOnly = Visibility.Hidden;

                }
                else
                {
                    IsEnablePaymentOnly = Visibility.Visible;
                }
                calculateBalance();
            }
        }

        private double? _PaidAmount;

        public double? PaidAmount
        {
            get { return _PaidAmount; }
            set { Set(ref _PaidAmount, value); }
        }

        private double? _BalanceAmountt;

        public double? BalanceAmount
        {
            get { return _BalanceAmountt; }
            set { Set(ref _BalanceAmountt, value); }
        }

        private List<PatientPaymentDetailModel> _PaymentDetailsList;

        public List<PatientPaymentDetailModel> PaymentDetailsList
        {
            get { return _PaymentDetailsList ?? (_PaymentDetailsList = new List<PatientPaymentDetailModel>()); }
            set
            {
                Set(ref _PaymentDetailsList, value);
            }
        }

        private List<LookupReferenceValueModel> _BillCatagory;

        public List<LookupReferenceValueModel> BillCatagory
        {
            get { return _BillCatagory; }
            set { _BillCatagory = value; }
        }

        private List<LookupReferenceValueModel> _EncounterTypes;

        public List<LookupReferenceValueModel> EncounterTypes
        {
            get { return _EncounterTypes; }
            set { _EncounterTypes = value; }
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

        private RelayCommand _CallPaymentModeCommand;

        public RelayCommand CallPaymentModeCommand
        {
            get { return _CallPaymentModeCommand ?? (_CallPaymentModeCommand = new RelayCommand(CallPaymentMode)); }
        }

        private RelayCommand _CreditOnlyCommand;

        public RelayCommand CreditOnlyCommand
        {
            get { return _CreditOnlyCommand ?? (_CreditOnlyCommand = new RelayCommand(CreditOnly)); }
        }

        private RelayCommand _CashOnlyCommand;

        public RelayCommand CashOnlyCommand
        {
            get { return _CashOnlyCommand ?? (_CashOnlyCommand = new RelayCommand(CashOnly)); }
        }
        #endregion

        #region Method

        public GenerateBillViewModel()
        {
            IsPrint = true;
            var refValList = DataService.Technical.GetReferenceValueList("BLCAT,ENTYP");
            BillCatagory = refValList.Where(p => p.DomainCode == "BLCAT").ToList();
            EncounterTypes = refValList.Where(p => p.DomainCode == "ENTYP").ToList();
        }
        public void AssingGenerateBill(PatientVisitModel patientVisit)
        {
            this.SelectPateintVisit = patientVisit;
            PatientVisitPayors = DataService.PatientIdentity.GetPatientVisitPayorByVisitUID(SelectPateintVisit.PatientVisitUID);
        }

        public List<AllocatedPatBillableItemsSubAccountResultModel> LoadPatientBillableItemsSubGroups(int accountUID, long patientVisitPayorUID,string IsPackage)
        {
            return DataService.Billing.GetPatientBillableItemsSubAccount(SelectPateintVisit.PatientUID, SelectPateintVisit.PatientVisitUID, null, patientVisitPayorUID, SelectPateintVisit.StartDttm.Value, DateTime.Now, IsPackage, accountUID, null, null);
        }

        public List<AllocatedPatBillableItemsResultModel> LoadPatientBillableItems(long patientVisitPayorUID,int? accountUID, int? subAccountUID, int? careproviderUID, string IsPackage)
        {
            return DataService.Billing.GetPatientBillableItemsBySA(SelectPateintVisit.PatientUID, SelectPateintVisit.PatientVisitUID, null, careproviderUID, null, patientVisitPayorUID, SelectPateintVisit.StartDttm.Value, DateTime.Now, IsPackage, accountUID, subAccountUID, null);
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            CallNextPayor();
        }

        public void CallPaymentMode()
        {
            ListBillPayment pageview = new ListBillPayment();
            var viewModel = (pageview.DataContext as ListBillPaymentViewModel);
            viewModel.TotalAmount = NetAmount ?? 0;
            viewModel.PaymentDetailsList = new ObservableCollection<PatientPaymentDetailModel>(PaymentDetailsList);
            ListBillPaymentViewModel result = (ListBillPaymentViewModel)LaunchViewDialogNonPermiss(pageview, true);
            if (result.ResultDialog == ActionDialog.Save)
            {
                PaymentAmount = null;
                PaymentDetailsList = result.PaymentDetailsList.ToList();
                if (PaymentDetailsList != null && PaymentDetailsList.Count > 0)
                {
                    IsEnablePaymentOnly = Visibility.Hidden;
                }
                else
                {
                    IsEnablePaymentOnly = Visibility.Visible;
                }
                calculateBalance();
            }
        }

        private void CallNextPayor()
        {
            int maxIndex = 0;
            if (PatientVisitPayors != null && PatientVisitPayors.Count() > 0)
            {
                maxIndex = PatientVisitPayors.Count() - 1;
                if (SelectPatientVisitPayor == null || SelectPatientVisitPayor.PayorDetailUID == 0)
                {
                    SelectPatientVisitPayor = PatientVisitPayors.FirstOrDefault();
                }
                else
                {
                    if (PatientVisitPayors.Contains(SelectPatientVisitPayor))
                    {
                        _currentIndex = PatientVisitPayors.IndexOf(SelectPatientVisitPayor);
                        _currentIndex++;
                        if (_currentIndex <= maxIndex)
                            SelectPatientVisitPayor = PatientVisitPayors[_currentIndex];
                        else
                        {
                            _currentIndex = 0;
                            SelectPatientVisitPayor = PatientVisitPayors[_currentIndex];
                        }
                    }
                }
            }
        }
        void CreditOnly()
        {
            BillingPaymentModePopUp pageview = new BillingPaymentModePopUp();
            var viewModel = (pageview.DataContext as BillingPaymentModePopUpViewModel);
            viewModel.IsCardChecked = true;
            BillingPaymentModePopUpViewModel result = (BillingPaymentModePopUpViewModel)LaunchViewDialogNonPermiss(pageview, true);
            if (result.ResultDialog == ActionDialog.Save)
            {
                PaymentDetailsList = new List<PatientPaymentDetailModel>();
                PaymentDetailsList.Add(result.SelectedPaymentDetail);
                PaymentAmount = null;
                Save();
            }
        }
        void CashOnly()
        {
            PaymentDetailsList = new List<PatientPaymentDetailModel>();
            PaymentDetailsList.Add(new PatientPaymentDetailModel { Amount = NetAmount.Value, PaidDttm = DateTime.Now, CURNCUID = defaultCURNC });
            PaymentAmount = null;
            Save();
        }

        void calculateBalance()
        {
            PaidAmount = (PaymentAmount ?? 0) + PaymentDetailsList?.Sum(p => p.Amount);
            BalanceAmount = NetAmount - (PaidAmount ?? 0);
        }

        void Save()
        {
            try
            {
                if (PatientBillableItemsAccounts == null || PatientBillableItemsAccounts.Count <= 0)
                {
                    return;
                }
                if (BalanceAmount < 0)
                {
                    WarningDialog("ยอดไม่ถูกต้อง กรุณาตรวจสอบ");
                    return;
                }

                if (PaymentAmount != null && PaymentAmount > 0)
                {
                    var cahsPayment = PaymentDetailsList.FirstOrDefault(p => p.PAYMDUID == CASHH);
                    if (cahsPayment != null)
                    {
                        cahsPayment.Amount = cahsPayment.Amount + (PaymentAmount ?? 0);
                    }
                    else
                    {
                        PaymentDetailsList.Add(new PatientPaymentDetailModel { Amount = PaymentAmount.Value,PaidDttm = DateTime.Now,CURNCUID = defaultCURNC});
                    }
                }


                GeneratePatientBillModel generateBill = new GeneratePatientBillModel();
                generateBill.PatientUID = SelectPateintVisit.PatientUID;
                generateBill.PatientVisitUID = SelectPateintVisit.PatientVisitUID;
                generateBill.BillGenerateDttm = DateTime.Now;
                generateBill.TotalAmount = Amount;
                generateBill.DiscountAmount = DiscountAmount;
                generateBill.NetAmount = NetAmount;
                generateBill.PBTYPUID = SelectPatientVisitPayor.PBTYPUID;
                generateBill.BLTYPUID = SelectPatientVisitPayor.BLTYPUID;
                generateBill.PatientVisitPayorUID = SelectPatientVisitPayor.PatientVisitPayorUID;
                generateBill.PayorDetailUID = SelectPatientVisitPayor.PayorDetailUID;
                generateBill.PayorAgreementUID = SelectPatientVisitPayor.PayorAgreementUID;
                generateBill.UserUID = AppUtil.Current.UserID;
                generateBill.DateFrom = SelectPateintVisit.StartDttm.Value;
                generateBill.DateTo = DateTime.Now;
                generateBill.Comments = Comments;

                if (SelectPateintVisit.ENTYPUID == EncounterTypes.FirstOrDefault(p => p.ValueCode == "INPAT").Key)
                {
                    generateBill.BLCATUID = BillCatagory.FirstOrDefault(p => p.ValueCode == "IPBILL").Key;
                }
                else
                {
                    generateBill.BLCATUID = BillCatagory.FirstOrDefault(p => p.ValueCode == "OPBILL").Key;
                }

                generateBill.PatientBillableItemsAccounts = PatientBillableItemsAccounts.ToList();
                generateBill.PatientPaymentDetails = PaymentDetailsList;

                PatientBillModel patBillResult = DataService.Billing.GeneratePatientBill(generateBill);

                string isBillComplete = DataService.Billing.GetCompleteBill(SelectPateintVisit.PatientVisitUID);
                if (isBillComplete == "Y")
                {
                    int FINDIS = 421;
                    DataService.PatientIdentity.ChangeVisitStatus(SelectPateintVisit.PatientVisitUID, FINDIS, SelectPateintVisit.CareProviderUID, SelectPateintVisit.LocationUID, DateTime.Now, AppUtil.Current.UserID, null, null);
                }


                if (IsPrint)
                {

                    XtraReport report;
                    //var selectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == SelectPatientCloseMed.OwnerOrganisationUID);
                    if (SelectPateintVisit.VisitType != "Non Medical")
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

                PaymentDetailsList.Clear();
                PatientBillableItemsAccounts.Clear();
                ResultDialog = ActionDialog.Save;
                CallNextPayor();
                //CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }



        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }



        #endregion
    }
}
