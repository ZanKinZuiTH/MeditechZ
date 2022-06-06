using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BillPackageDetailModel
    {
        public int BillPackageDetailUID { get; set; }
        public int BillPackageUID { get; set; }
        public int BSMDDUID { get; set; }
        public string Comments { get; set; }
        public int CURNCUID { get; set; }
        public double Amount { get; set; }
        public int? BillableItemUID { get; set; }
        public int? Quantity { get; set; }
        public int? ItemUID { get; set; }
        public int? OrderCategoryUID { get; set; }
        public int? OrderSubCategoryUID { get; set; }
        public double? DoctorShare { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
