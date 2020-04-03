using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BillableItemModel
    {
        public int BillableItemUID { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public string IsShareDoctor { get; set; }
        public System.Nullable<double> DoctorFee { get; set; }
        public double TotalCost { get; set; }
        public double Price { get; set; }
        public double Profit { get; set; }
        public string Comments { get; set; }
        public Nullable<int> ItemUID { get; set; }
        public int CURNCUID { get; set; }
        public int BSMDDUID { get; set; }
        public string BillingServiceMetaData { get; set; }
        public int? BillingGroupUID { get; set; }
        public int? BillingSubGroupUID { get; set; }
        public System.Nullable<DateTime> ActiveFrom { get; set; }
        public System.Nullable<DateTime> ActiveTo { get; set; }

        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public List<BillableItemDetailModel> BillableItemDetails { get; set; }
    }
}
