using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class OrderSetBillableItemModel
    {
        public int OrderSetBillableItemUID { get; set; }
        public string Code { get; set; }
        public int BillableItemUID { get; set; }
        public int OrderSetUID { get; set; }
        public string OrderCatalogName { get; set; }
        public double Quantity { get; set; }
        public Nullable<int> FRQNCUID { get; set; }
        public Nullable<double> DoseQty { get; set; }
        public int BSMDDUID { get; set; }
        public string BillingServiceMetaData { get; set; }
        public string ProcessingNotes { get; set; }
        public double? Price { get; set; }
        public double NetPrice { get; set; }
        public double? DoctorFee { get; set; }
        public DateTime? ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public List<BillableItemDetailModel> BillableItemDetails { get; set; }
    }
}
