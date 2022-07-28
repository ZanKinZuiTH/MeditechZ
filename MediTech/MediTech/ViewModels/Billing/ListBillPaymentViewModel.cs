using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;

namespace MediTech.ViewModels
{
    public class ListBillPaymentViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<LookupReferenceValueModel> _PaymentModes;

        public List<LookupReferenceValueModel> PaymentModes
        {
            get { return _PaymentModes; }
            set { Set(ref _PaymentModes, value); }
        }

        private LookupReferenceValueModel _SelectedPaymentMode;

        public LookupReferenceValueModel SelectedPaymentMode
        {
            get { return _SelectedPaymentMode; }
            set
            {
                Set(ref _SelectedPaymentMode, value);
                if (SelectedPaymentMode != null)
                {
                    if (SelectedPaymentMode.ValueCode == "CASHH")
                    {
                        ValidityDateVisible = Visibility.Hidden;
                        BankNameVisibility = Visibility.Hidden;
                        IsEnableCardNumber = Visibility.Hidden;
                        IsEnableCardType = Visibility.Hidden;
                    }
                    else if (SelectedPaymentMode.ValueCode == "CHEQU")
                    {
                        ExpiryDateLabel = "Cheque Date";
                        ValidityDateVisible = Visibility.Visible;
                        BankNameVisibility = Visibility.Visible;
                        IsEnableCardNumber = Visibility.Visible;
                        IsEnableCardType = Visibility.Hidden;
                    }
                    else if (SelectedPaymentMode.ValueCode == "CHEQUCASH")
                    {
                        ExpiryDateLabel = "Valid upto";
                        ValidityDateVisible = Visibility.Visible;
                        BankNameVisibility = Visibility.Visible;
                        IsEnableCardNumber = Visibility.Visible;
                        IsEnableCardType = Visibility.Hidden;
                    }
                    else if (SelectedPaymentMode.ValueCode == "ONTRANS")
                    {
                        ExpiryDateLabel = "Transfer Date";
                        ValidityDateVisible = Visibility.Visible;
                        BankNameVisibility = Visibility.Visible;
                        IsEnableCardNumber = Visibility.Visible;
                        IsEnableCardType = Visibility.Hidden;
                    }
                    else if (SelectedPaymentMode.ValueCode == "COUPN")
                    {
                        ExpiryDateLabel = "Valid upto";
                        ValidityDateVisible = Visibility.Visible;
                        BankNameVisibility = Visibility.Hidden;
                        IsEnableCardNumber = Visibility.Visible;
                        IsEnableCardType = Visibility.Hidden;
                    }
                    else if (SelectedPaymentMode.ValueCode == "CARDD")
                    {
                        ExpiryDateLabel = "Valid upto";
                        ValidityDateVisible = Visibility.Visible;
                        BankNameVisibility = Visibility.Visible;
                        IsEnableCardNumber = Visibility.Collapsed;
                        IsEnableCardType = Visibility.Visible;
                    }
                    else if (SelectedPaymentMode.ValueCode == "DEBITD")
                    {
                        ExpiryDateLabel = "Valid upto";
                        ValidityDateVisible = Visibility.Visible;
                        BankNameVisibility = Visibility.Visible;
                        IsEnableCardNumber = Visibility.Visible;
                        IsEnableCardType = Visibility.Hidden;
                    }
                }
            }
        }

        private double? _Amount;

        public double? Amount
        {
            get { return _Amount; }
            set { Set(ref _Amount, value); }
        }

        private List<LookupReferenceValueModel> _Currencys;

        public List<LookupReferenceValueModel> Currencys
        {
            get { return _Currencys; }
            set { Set(ref _Currencys, value); }
        }

        private LookupReferenceValueModel _SelectedCurrency;

        public LookupReferenceValueModel SelectedCurrency
        {
            get { return _SelectedCurrency; }
            set { Set(ref _SelectedCurrency, value); }
        }


        private int? _selectedPaymentCURNCUID;
        public int? SelectedPaymentCURNCUID
        {
            get
            {
                return _selectedPaymentCURNCUID;
            }
            set
            {
                if (_selectedPaymentCURNCUID != value)
                {
                    Set(ref _selectedPaymentCURNCUID, value);

                }
            }
        }

        private DateTime _PaidDttm;

        public DateTime PaidDttm
        {
            get { return _PaidDttm; }
            set { Set(ref _PaidDttm, value); }
        }


        private string _ExpiryDateLabel;

        public string ExpiryDateLabel
        {
            get { return _ExpiryDateLabel; }
            set { Set(ref _ExpiryDateLabel, value); }
        }

        private DateTime? _CardExpiryDttm;

        public DateTime? CardExpiryDttm
        {
            get { return _CardExpiryDttm; }
            set { Set(ref _CardExpiryDttm, value); }
        }


        private Visibility _ValidityDateVisible;

        public Visibility ValidityDateVisible
        {
            get { return _ValidityDateVisible; }
            set { Set(ref _ValidityDateVisible, value); }
        }

        private List<LookupReferenceValueModel> _BankNames;

        public List<LookupReferenceValueModel> BankNames
        {
            get { return _BankNames; }
            set { Set(ref _BankNames, value); }
        }


        private Visibility _BankNameVisibility;

        public Visibility BankNameVisibility
        {
            get { return _BankNameVisibility; }
            set { Set(ref _BankNameVisibility, value); }
        }

        private LookupReferenceValueModel _SelectedBank;

        public LookupReferenceValueModel SelectedBank
        {
            get { return _SelectedBank; }
            set { Set(ref _SelectedBank, value); }
        }

        private string _CardNumber;

        public string CardNumber
        {
            get { return _CardNumber; }
            set { Set(ref _CardNumber, value); }
        }

        private Visibility _IsEnableCardNumber;

        public Visibility IsEnableCardNumber
        {
            get { return _IsEnableCardNumber; }
            set { Set(ref _IsEnableCardNumber, value); }
        }

        private string _AuthorizationNumber;

        public string AuthorizationNumber
        {
            get { return _AuthorizationNumber; }
            set { Set(ref _AuthorizationNumber, value); }
        }

        private Visibility _IsEnableCardType;

        public Visibility IsEnableCardType
        {
            get { return _IsEnableCardType; }
            set { Set(ref _IsEnableCardType, value); }
        }

        private List<LookupReferenceValueModel> _CardTypes;

        public List<LookupReferenceValueModel> CardTypes
        {
            get { return _CardTypes; }
            set { Set(ref _CardTypes, value); }
        }

        private LookupReferenceValueModel _SelectedCardType;

        public LookupReferenceValueModel SelectedCardType
        {
            get { return _SelectedCardType; }
            set { Set(ref _SelectedCardType, value); }
        }

        private double _TotalAmount;

        public double TotalAmount
        {
            get { return _TotalAmount; }
            set { Set(ref _TotalAmount, value); }
        }

        private double _NetPayable;

        public double NetPayable
        {
            get { return _NetPayable; }
            set { Set(ref _NetPayable, value); }
        }

        private double _AmountReceived;

        public double AmountReceived
        {
            get { return _AmountReceived; }
            set { Set(ref _AmountReceived, value); }
        }



        private ObservableCollection<PatientPaymentDetailModel> _PaymentDetailsList;

        public ObservableCollection<PatientPaymentDetailModel> PaymentDetailsList
        {
            get { return _PaymentDetailsList ?? (_PaymentDetailsList = new ObservableCollection<PatientPaymentDetailModel>()); }
            set
            {
                Set(ref _PaymentDetailsList, value);
            }
        }

        private PatientPaymentDetailModel _SelectedPaymentDetail;

        public PatientPaymentDetailModel SelectedPaymentDetail
        {
            get { return _SelectedPaymentDetail; }
            set
            {
                Set(ref _SelectedPaymentDetail, value);
                if (_SelectedPaymentDetail != null)
                {
                    SelectedPaymentMode = PaymentModes.FirstOrDefault(p => p.Key == _SelectedPaymentDetail.PAYMDUID);
                    Amount = _SelectedPaymentDetail.Amount;
                    SelectedCurrency = Currencys.FirstOrDefault(p => p.Key == _SelectedPaymentDetail.CURNCUID);
                    PaidDttm = _SelectedPaymentDetail.PaidDttm;
                    CardExpiryDttm = _SelectedPaymentDetail.CardExpiryDttm;
                    SelectedBank = BankNames.FirstOrDefault(p => p.Display == _SelectedPaymentDetail.BankName);
                    CardNumber = _SelectedPaymentDetail.CardNumber;
                    SelectedCardType = CardTypes.FirstOrDefault(p => p.Key == _SelectedPaymentDetail.CRDTYUID);
                    AuthorizationNumber = _SelectedPaymentDetail.AuthorizationNumber;
                    CardNumber = _SelectedPaymentDetail.CardNumber;
                }
            }
        }


        #endregion

        #region Command

        private RelayCommand _AddPaymentItemCommand;

        public RelayCommand AddPaymentItemCommand
        {
            get { return _AddPaymentItemCommand ?? (_AddPaymentItemCommand = new RelayCommand(AddPaymentItem)); }
        }


        private RelayCommand _UpdatePaymentItemCommand;

        public RelayCommand UpdatePaymentItemCommand
        {
            get { return _UpdatePaymentItemCommand ?? (_UpdatePaymentItemCommand = new RelayCommand(UpdatePaymentItem)); }
        }


        private RelayCommand _DeletePaymentItemCommand;

        public RelayCommand DeletePaymentItemCommand
        {
            get { return _DeletePaymentItemCommand ?? (_DeletePaymentItemCommand = new RelayCommand(DeletePaymentItem)); }
        }

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

        #endregion

        #region Method

        public override void OnLoaded()
        {

            base.OnLoaded();
            PaidDttm = DateTime.Now;
            var refValList = DataService.Technical.GetReferenceValueList("PAYMD,CRDTY,CURNC,BANKS");
            PaymentModes = refValList.Where(p => p.DomainCode == "PAYMD" && (p.ValueCode != "DEPOSIT" && p.ValueCode != "PACKAGE" && p.ValueCode != "BILLING")).ToList();
            Currencys = refValList.Where(p => p.DomainCode == "CURNC").ToList();
            BankNames = refValList.Where(p => p.DomainCode == "BANKS").ToList();
            CardTypes = refValList.Where(p => p.DomainCode == "CRDTY").ToList();
            CalculateNetAmount();

        }

        void AddPaymentItem()
        {
            if (Amount > NetPayable)
            {
                WarningDialog("ยอดไม่ถูกต้อง โปรดตรวจสอบ");
                return;
            }
            PatientPaymentDetailModel newPayment = new PatientPaymentDetailModel();
            newPayment.PAYMDUID = SelectedPaymentMode.Key.Value;
            newPayment.PaymentMode = SelectedPaymentMode.Display;
            newPayment.Amount = Amount ?? 0;
            newPayment.CURNCUID = SelectedCurrency.Key;
            newPayment.Currency = SelectedCurrency.Display;
            newPayment.PaidDttm = PaidDttm;
            newPayment.CardExpiryDttm = ValidityDateVisible == Visibility.Visible ? CardExpiryDttm : null;
            newPayment.BankName = BankNameVisibility == Visibility.Visible ? SelectedBank?.Display : null;
            newPayment.CardNumber = (IsEnableCardNumber == Visibility.Visible || IsEnableCardType == Visibility.Visible) ? CardNumber : null;
            newPayment.CRDTYUID = IsEnableCardType == Visibility.Visible ? SelectedCardType?.Key : null;
            newPayment.CardType = IsEnableCardType == Visibility.Visible ? SelectedCardType?.Display : null;
            newPayment.AuthorizationNumber = IsEnableCardType == Visibility.Visible ? AuthorizationNumber : null;
            PaymentDetailsList.Add(newPayment);
            CalculateNetAmount();
            SelectedPaymentDetail = null;

        }

        void UpdatePaymentItem()
        {
            if (SelectedPaymentDetail != null)
            {
                var amountReceived = AmountReceived = PaymentDetailsList.Where(P => !P.Equals(SelectedPaymentDetail)).Sum(p => p.Amount);
                if ((amountReceived + Amount) > TotalAmount)
                {
                    WarningDialog("ยอดไม่ถูกต้อง โปรดตรวจสอบ");
                    return;
                }
                SelectedPaymentDetail.PAYMDUID = SelectedPaymentMode.Key.Value;
                SelectedPaymentDetail.PaymentMode = SelectedPaymentMode.Display;
                SelectedPaymentDetail.Amount = Amount ?? 0;
                SelectedPaymentDetail.CURNCUID = SelectedCurrency.Key;
                SelectedPaymentDetail.Currency = SelectedCurrency.Display;
                SelectedPaymentDetail.PaidDttm = PaidDttm;
                SelectedPaymentDetail.CardExpiryDttm = ValidityDateVisible == Visibility.Visible ? CardExpiryDttm : null;
                SelectedPaymentDetail.BankName = BankNameVisibility == Visibility.Visible ? SelectedBank?.Display : null;
                SelectedPaymentDetail.CardNumber = (IsEnableCardNumber == Visibility.Visible || IsEnableCardType == Visibility.Visible) ? CardNumber : null;
                SelectedPaymentDetail.CRDTYUID = IsEnableCardType == Visibility.Visible ? SelectedCardType?.Key : null;
                SelectedPaymentDetail.CardType = IsEnableCardType == Visibility.Visible ? SelectedCardType?.Display : null;
                SelectedPaymentDetail.AuthorizationNumber = IsEnableCardType == Visibility.Visible ? AuthorizationNumber : null;
                (this.View as ListBillPayment).grdPaymentDetails.RefreshData();
                CalculateNetAmount();
                SelectedPaymentDetail = null;
            }
        }

        void DeletePaymentItem()
        {
            if (SelectedPaymentDetail != null)
            {
                PaymentDetailsList.Remove(SelectedPaymentDetail);
                CalculateNetAmount();
            }
        }

        void Save()
        {
            try
            {

                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        void CalculateNetAmount()
        {
            AmountReceived = PaymentDetailsList.Sum(p => p.Amount);
            NetPayable = TotalAmount - AmountReceived;
            Amount = (NetPayable == 0 ? (double?)null : NetPayable);
        }
        #endregion
    }
}
