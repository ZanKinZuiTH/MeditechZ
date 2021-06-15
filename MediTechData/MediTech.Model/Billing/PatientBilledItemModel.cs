using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientBilledItemModel
    {
        public long PatientBilledItemUID { get; set; }
        public long PatientBillUID { get; set; }
        public int BillableItemUID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double ItemMutiplier { get; set; }

        public string Unit { get; set; }
        public Nullable<double> Amount { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> NetAmount { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public int? OrderSetUID { get; set; }
        public double? OrderSetAmount { get; set; }
        public int BSMDDUID { get; set; }
        public string BillingService { get; set; }

        public string BillingGroup { get; set; }
        public int BillingGroupUID { get; set; }
        public string BillinsgSubGroup { get; set; }
        public int BillingSubGroupUID { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public long? IdentifyingUID { get; set; }

    }
}
