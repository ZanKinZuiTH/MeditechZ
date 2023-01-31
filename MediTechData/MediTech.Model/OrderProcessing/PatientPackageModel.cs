using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientPackageModel
    {
        public long PatientPackageUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string PackageName { get; set; }
        public Nullable<int> BillPackageUID { get; set; }
        public Nullable<double> TotalAmount { get; set; }
        public System.DateTime ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public int PackageCreatedByUID { get; set; }
        public string PackageCreatedByName { get; set; }
        public System.DateTime PackageCreatedDttm { get; set; }
        public string Comments { get; set; }
        public Nullable<int> BillingGroupUID { get; set; }
        public Nullable<int> BillingSubGroupUID { get; set; }
        public string IsConsiderItem { get; set; }
        public Nullable<double> Qty { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public byte[] TIMESTAMP { get; set; }

        public string BillingGroup { get; set; }
        public string BillingSubGroup { get; set; }

        public List<PatientPackageItemModel> BillPackageDetails { get; set; }
    }
}
