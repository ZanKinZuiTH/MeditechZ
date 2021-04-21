using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class GroupReceiptModel
    {
        public int GroupReceiptUID { get; set; }
        public int OrderSetUID { get; set; }
        public string ReceiptNo { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> PriceUnit { get; set; }
        public Nullable<int> OwnerOrganisation { get; set; }
        public int PayorDetailUID { get; set; }
        public string PayorName { get; set; }
        public string PayerAddress { get; set; }
        public string Seller { get; set; }
        public DateTime? StartDttm { get; set; }
        public int StaffUID { get; set; }
        public List<GroupReceiptDetailModel> GroupReceiptDetailModel { get; set; }
    }
}
