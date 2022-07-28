using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;

namespace MediTech.ViewModels
{
    public class BillingPaymentModePopUpViewModel : MediTechViewModelBase
    {
        int defaultCURNC = 2811;
        #region Properties
        //
        #region IsCashChecked
        private bool _IsCashCheked;
        public bool IsCashChecked
        {
            get
            {
                return _IsCashCheked;
            }
            set
            {
                if (_IsCashCheked != value)
                {
                    Set(ref _IsCashCheked, value);
                    if (IsCashChecked)
                    {
                        IsCreditChecked = false;
                        IsCardChecked = false;
                        IsOLTransferChecked = false;
                        IsCouponChecked = false;
                        Update();
                    }
                }
            }
        }
        #endregion
        //
        #region IsCreditChecked
        private bool _isCreditChecked;
        public bool IsCreditChecked
        {
            get
            {
                return _isCreditChecked;
            }
            set
            {
                if (_isCreditChecked != value)
                {
                    Set(ref _isCreditChecked, value);
                    if (IsCreditChecked)
                    {
                        IsCardChecked = false;
                        IsCashChecked = false;
                        IsOLTransferChecked = false;
                        IsCouponChecked = false;
                        Update();
                    }
                }
            }
        }
        #endregion
        //
        #region IsCardChecked
        private bool _isCardChecked;
        public bool IsCardChecked
        {
            get
            {
                return _isCardChecked;
            }
            set
            {
                if (_isCardChecked != value)
                {
                    Set(ref _isCardChecked, value);
                    if (IsCardChecked)
                    {
                        IsCreditChecked = false;
                        IsCashChecked = false;
                        IsOLTransferChecked = false;
                        IsCouponChecked = false;
                        Update();
                    }
                }
            }
        }
        #endregion       
        //
        #region IsOLTransferChecked
        private bool _isOLTransferCheked;
        public bool IsOLTransferChecked
        {
            get
            {
                return _isOLTransferCheked;
            }
            set
            {
                if (_isOLTransferCheked != value)
                {
                    Set(ref _isOLTransferCheked, value);
                    if (_isOLTransferCheked)
                    {
                        IsCreditChecked = false;
                        IsCardChecked = false;
                        IsCashChecked = false;
                        IsCouponChecked = false;
                        Update();
                    }
                }
            }
        }
        #endregion

        #region IsCouponChecked
        private bool _isCouponChecked;
        public bool IsCouponChecked
        {
            get
            {
                return _isCouponChecked;
            }
            set
            {
                if (_isCouponChecked != value)
                {
                    Set(ref _isCouponChecked, value);
                    if (IsCouponChecked)
                    {
                        IsCardChecked = false;
                        IsCashChecked = false;
                        IsCreditChecked = false;
                        IsOLTransferChecked = false;
                        Update();
                    }
                }
            }
        }
        #endregion


        private List<LookupReferenceValueModel> _BankNames;

        public List<LookupReferenceValueModel> BankNames
        {
            get { return _BankNames; }
            set { Set(ref _BankNames, value); }
        }


        private bool _IsBankNameEnabled;

        public bool IsBankNameEnabled
        {
            get { return _IsBankNameEnabled; }
            set { Set(ref _IsBankNameEnabled, value); }
        }

        private string _BankName;

        public string BankName
        {
            get { return _BankName; }
            set { Set(ref _BankName, value); }
        }


        private string _CardNumber;

        public string CardNumber
        {
            get { return _CardNumber; }
            set { Set(ref _CardNumber, value); }
        }

        private bool _IsNumberEnabled;

        public bool IsNumberEnabled
        {
            get { return _IsNumberEnabled; }
            set { Set(ref _IsNumberEnabled, value); }
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

        private bool _IsCardNameEnabled;

        public bool IsCardNameEnabled
        {
            get { return _IsCardNameEnabled; }
            set { Set(ref _IsCardNameEnabled, value); }
        }

        private string _DateLabel;

        public string DateLabel
        {
            get { return _DateLabel; }
            set { Set(ref _DateLabel, value); }
        }

        private DateTime? _PaidDttm;

        public DateTime? PaidDttm
        {
            get { return _PaidDttm; }
            set { Set(ref _PaidDttm, value); }
        }


        private string _AuthorizationNumber;

        public string AuthorizationNumber
        {
            get { return _AuthorizationNumber; }
            set { Set(ref _AuthorizationNumber, value); }
        }


        private PatientPaymentDetailModel _SelectedPaymentDetail;

        public PatientPaymentDetailModel SelectedPaymentDetail
        {
            get { return _SelectedPaymentDetail ?? (_SelectedPaymentDetail = new PatientPaymentDetailModel{ PaidDttm = DateTime.Now ,CURNCUID = defaultCURNC }); }
            set { Set(ref _SelectedPaymentDetail, value); }
        }

        private List<LookupReferenceValueModel> _PaymentTypes;

        public List<LookupReferenceValueModel> PaymentTypes
        {
            get { return _PaymentTypes; }
            set { _PaymentTypes = value; }
        }

        #endregion

        #region Command

        private RelayCommand _ChangeCommand;

        public RelayCommand ChangeCommand
        {
            get { return _ChangeCommand ?? (_ChangeCommand = new RelayCommand(Change)); }
        }

        #endregion

        #region Method

        public BillingPaymentModePopUpViewModel()
        {
            var refValueList = DataService.Technical.GetReferenceValueList("PAYMD,CRDTY,BANKS");
            PaymentTypes = refValueList.Where(p => p.DomainCode == "PAYMD").ToList();
            BankNames = refValueList.Where(p => p.DomainCode == "BANKS").ToList();
            CardTypes = refValueList.Where(p => p.DomainCode == "CRDTY").ToList();
        }

        private void Change()
        {

            CloseViewDialog(ActionDialog.Save);
        }

        private void Update()
        {
            if (IsCreditChecked)
            {
                SelectedPaymentDetail.PAYMDUID = PaymentTypes.FirstOrDefault(p => p.ValueCode == "CHEQU")?.Key ?? 0;
                SelectedPaymentDetail.CRDTYUID = null;
                SelectedPaymentDetail.AuthorizationNumber = null;
                IsBankNameEnabled = true;
                IsNumberEnabled = true;
                IsCardNameEnabled = false;

                DateLabel = "Cheque Date";

            }
            else if (IsCardChecked)
            {
                SelectedPaymentDetail.PAYMDUID = PaymentTypes.FirstOrDefault(p => p.ValueCode == "CARDD")?.Key ?? 0;
                IsBankNameEnabled = true;
                IsNumberEnabled = true;
                IsCardNameEnabled = true;
                DateLabel = "Valid upto";
            }
            else if (IsOLTransferChecked)
            {
                SelectedPaymentDetail.PAYMDUID = PaymentTypes.FirstOrDefault(p => p.ValueCode == "ONTRANS")?.Key ?? 0;
                SelectedPaymentDetail.CRDTYUID = null;
                SelectedPaymentDetail.AuthorizationNumber = null;
                IsBankNameEnabled = true;
                IsNumberEnabled = true;
                IsCardNameEnabled = false;
                DateLabel = "Transfer Date";
            }
            else if (IsCashChecked)
            {
                SelectedPaymentDetail.PAYMDUID = PaymentTypes.FirstOrDefault(p => p.ValueCode == "CASHH")?.Key ?? 0;
                SelectedPaymentDetail.CRDTYUID = null;
                SelectedPaymentDetail.CardNumber = null;
                SelectedPaymentDetail.BankName = null;
                SelectedPaymentDetail.AuthorizationNumber = null;
                //((BillingPaymentModePopUp)View).BankNamAutCompTextBox.Text = null;
                IsBankNameEnabled = false;
                IsNumberEnabled = false;
                IsCardNameEnabled = false;
                DateLabel = "Cash Collected On";
            }
            else if (IsCouponChecked)
            {
                SelectedPaymentDetail.PAYMDUID = PaymentTypes.FirstOrDefault(p => p.ValueCode == "COUPN")?.Key ?? 0;
                SelectedPaymentDetail.CRDTYUID = null;
                SelectedPaymentDetail.CardNumber = null;
                SelectedPaymentDetail.BankName = null;
                SelectedPaymentDetail.AuthorizationNumber = null;
                IsBankNameEnabled = false;
                IsNumberEnabled = true;
                IsCardNameEnabled = false;
                DateLabel = "Valid upto";
            }
        }
        #endregion
    }
}
