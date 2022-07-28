using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AllocatedPatBillableItemsSubAccountResultModel
    {
        public int? PayorUID { get; set; }
        public double? SubGroupMaxCoverage { get; set; }
        public double? GroupCovered { get; set; }
        public double? GroupMaxCoverage { get; set; }
        public int? AccountUID { get; set; }
        public string CareProvider { get; set; }
        public int? CareProviderUID { get; set; }
        public long? PatientVisitPayorUID { get; set; }
        public string PayorName { get; set; }
        public string PackageName { get; set; }
        public double? NetAmount { get; set; }
        public double? Discount { get; set; }
        public double? Amount { get; set; }
        public double? Quantity { get; set; }
        public int? SubAccountUID { get; set; }
        public string SubAccountName { get; set; }
        public char IsModified { get; set; }
        public ObservableCollection<AllocatedPatBillableItemsResultModel> AllocatedPatBillableItems { get; set; }
        public double? SubGroupCovered { get; set; }

        public string AccountName { get; set; }

        public string IsPackage { get; set; }
    }
}
