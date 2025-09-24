using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PurchaseOrderPaymentModel
    {
        public int PurchaseOrderPaymentUID { get; set; }
        public double PurchaseOrderUID { get; set; }
        public double Amount { get; set; }
        public int PAYMDUID { get; set; }
        public string PaymentType { get; set; }
        public int CURNCUID { get; set; }
        public string CurrencyType { get; set; }
        public Nullable<System.DateTime> PaidDttm { get; set; }
        public Nullable<System.DateTime> ExpiryDttm { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
