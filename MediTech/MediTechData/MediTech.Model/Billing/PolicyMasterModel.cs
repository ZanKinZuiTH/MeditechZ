using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PolicyMasterModel
    {
        public int PolicyMasterUID { get; set; }
        public string Code { get; set; }
        public string PolicyName { get; set; }
        public string Description { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public Nullable<int> AGTYPUID { get; set; }
        public string AgreementType { get; set; }
        public int OwnweOwnerOrganisationUID { get; set; }
        public List<BillingGroupModel> OrderGroup { get; set; }
        public List<BillingSubGroupModel> OrderSubGroup { get; set; }
        public List<BillableItemModel> OrderItem { get; set; }

        public List<OrderCategoryModel> OrderSetGroup { get; set; }
        public List<OrderSubCategoryModel> OrderSetSubGroup { get; set; }
        public List<OrderSetModel> OrderSetItem { get; set; }

        public List<OrderCategoryModel> PackageGroup { get; set; }
        public List<OrderSubCategoryModel> PackageSubGroup { get; set; }
        public List<BillPackageModel> Package { get; set; }
    }
}

