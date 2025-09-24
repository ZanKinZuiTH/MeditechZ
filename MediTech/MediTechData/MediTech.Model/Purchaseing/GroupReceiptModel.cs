using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class GroupReceiptModel
    {
        public int GroupReceiptUID { get; set; }
        public int OrderSetUID { get; set; }
        public string ReceiptNo { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> OwnerOrganisation { get; set; }
        public int PayorDetailUID { get; set; }
        public string PayorName { get; set; }
        public string PayerAddress { get; set; }
        public string TINNo { get; set; }
        public string Seller { get; set; }
        public DateTime? StartDttm { get; set; }
        public int StaffUID { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public Nullable<double> TaxAmount { get; set; }
        public Nullable<double> NoTaxAmount { get; set; }
        public Nullable<double> BfTaxAmount { get; set; }

        public string IsInvoice { get; set; }
        public string CancelledReason { get; set; }

        public DateTime? CancelledDttm { get; set; }
        public List<GroupReceiptPatientBillModel> GroupReceiptPatientBills { get; set; }
        public List<GroupReceiptDetailModel> GroupReceiptDetails { get; set; }
    }
}
