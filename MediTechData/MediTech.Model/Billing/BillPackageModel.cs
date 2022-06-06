using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BillPackageModel
    {
        public int BillPackageUID { get; set;}
        public string PackageName { get; set; }
        public string Description { get; set; }
        public int NoofDays { get; set; }
        public double TotalAmount { get; set; }
        public int CURNCUID { get; set; }
        public int PBLCTUID { get; set; }
        public double? MaxValue { get; set; }
        public double? MinValue { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime? ActiveTo { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int? OrderCategoryUID { get; set; }
        public int? OrderSubCategoryUID { get; set; }
        public string Code { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
