using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AllocatedPatientBillableItemsPalmModel
    {
        public int PatientBillableItemUID { get; set; }
        public string SubAccountName { get; set; }
        public int SubAccountUID { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public double Amount { get; set; }
        public double Discount { get; set; }
        public double NetAmount { get; set; }
        public int PayorUID { get; set; }
        public string PayorName { get; set; }
        public int BillableItemUID { get; set; }
        public long PatientVisitPayorUID { get; set; }
        public DateTime EventOccuredDttm { get; set; }
        public int CareProviderUID { get; set; }
        public double GroupMaxCoverage { get; set; }
        public double GroupCovered { get; set; }
        public double SubGroupMaxCoverage { get; set; }
        public double SubGroupCovered { get; set; }
        public string IsModified { get; set; }
        public int GroupUID { get; set; }

        public string PackageName { get; set; }
        public string SubGroupName { get; set; }
        public string GroupName { get; set; }
        public double TotalAmount { get; set; }
        public double SubGroupTotal { get; set; }

        public double SubGroupDiscount { get; set; }

        public double GroupTotal { get; set; }
        public double GroupDiscount { get; set; }
        public int PBLCTUID { get; set; }

        public int ALLDIUID { get; set; }
        public int BSMDDUID { get; set; }
    }
}
