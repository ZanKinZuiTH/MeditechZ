using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AdjustablePackageItemModel
    {
        public int BillableItemUID { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public string OrderNumber { get; set; }
        public string OrderFromLocation { get; set; }
        public string OrderToLocation { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderRasiedBy { get; set; }
        public long PatientOrderUID { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public string OrderStatus { get; set; }
        public string Comments { get; set; }
        public int BillPackageUID { get; set; }
        public int AltBillableItemUID { get; set; }
    }
}
