using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientPaymentDetailModel : INotifyPropertyChanged
    {
        public long PatientPaymentDetailUID { get; set; }
        public Nullable<long> PatientBillUID { get; set; }
        public Nullable<long> PatientUID { get; set; }
        public Nullable<long> PatientVisitUID { get; set; }
        public int PAYMDUID { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<int> CURNCUID { get; set; }
        public string Currency { get; set; }

        private System.DateTime _PaidDttm;
        public System.DateTime PaidDttm
        {
            get
            {
                return _PaidDttm;
            }
            set
            {
                _PaidDttm = value;
                OnPropertyRaised("PaidDttm");
            }
        }

        public double Amount { get; set; }

        private Nullable<int> _CRDTYUIDd;
        public Nullable<int> CRDTYUID
        {
            get
            {
                return _CRDTYUIDd;
            }
            set
            {
                _CRDTYUIDd = value;
                OnPropertyRaised("CRDTYUID");
            }
        }
        public string CardType { get; set; }

        private string _CardNumber;
        public string CardNumber
        {
            get
            {
                return _CardNumber;
            }
            set
            {
                _CardNumber = value;
                OnPropertyRaised("CardNumber");
            }
        }
        public Nullable<System.DateTime> CardExpiryDttm { get; set; }
        public string RecieptNumber { get; set; }
        public Nullable<long> PatientEpisodePackageUID { get; set; }
        public Nullable<int> ADVTPUID { get; set; }

        private string _BankName;
        public string BankName
        {
            get
            {
                return _BankName;
            }
            set
            {
                _BankName = value;
                OnPropertyRaised("BankName");
            }
        }

        private string _AuthorizationNumber;
        public string AuthorizationNumber
        {
            get
            {
                return _AuthorizationNumber;
            }
            set
            {
                _AuthorizationNumber = value;
                OnPropertyRaised("AuthorizationNumber");
            }
        }

        public string IsRefund { get; set; }
        public string RefundReceiptNumber { get; set; }
        public Nullable<double> ForeignCurrencyAmt { get; set; }
        public Nullable<double> ConversionRate { get; set; }
        public Nullable<double> TDSAmount { get; set; }
        public Nullable<double> WriteOff { get; set; }
        public Nullable<double> ExcessAmount { get; set; }
        public Nullable<int> BLCATUID { get; set; }
        public string Comments { get; set; }
        public string IsAdvance { get; set; }
        public Nullable<double> SpecialAmount { get; set; }
        public string IsCautiousAdvance { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
