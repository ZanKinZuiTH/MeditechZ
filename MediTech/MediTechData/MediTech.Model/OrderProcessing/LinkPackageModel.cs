using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class LinkPackageModel
    {
        public long UID { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public DateTime StartDttm { get; set; }
        public DateTime EndDttm { get; set; }
        public double Quantity { get; set; }
        public double NetAmount { get; set; }
        public string OrderLocation { get; set; }
        public string OrderToLocation { get; set; }
        public string OrderRaisedBy { get; set; }
        public string OrderCategory { get; set; }
        public string OrderSubCategory { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string UseKey { get; set; }
        public long BillPackateUID { get; set; }
        public long PatientPackageUID { get; set; }
        public int pCount1 { get; set; }
        public int pCount2 { get; set; }
        public int ParentUID { get; set; }
        public string PaymentStatus { get; set; }

        public bool IsSelected { get; set; }
    }
}
