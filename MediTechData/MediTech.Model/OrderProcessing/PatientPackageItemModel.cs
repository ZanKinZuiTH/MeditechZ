using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientPackageItemModel
    {
        public long PatientPackageItemUID { get; set; }
        public long PatientPackageUID { get; set; }
        public int BSMDDUID { get; set; }
        public int BillableItemUID { get; set; }
        public string ItemName { get; set; }
        public double Amount { get; set; }
        public Nullable<int> PatientBillUID { get; set; }
        public Nullable<double> Discount { get; set; }
        public Nullable<double> ItemMultiplier { get; set; }
        public Nullable<double> ActualAmount { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public byte[] TIMESTAMP { get; set; }
    }
}
