using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AllocatePatientBillableItemModel
    {

        public long PatientUID { get; set; }

        public long PatientVisitUID { get; set; }

        public int UserUID { get; set; }

        public string IsAutoAllocate { get; set; }

        public int? GroupUID { get; set; }

        public int? SubGroupUID { get; set; }

        public long? PatientVisitPayorUID { get; set; }

        public int? PayorAgreementUID { get; set; }

        public long? AllocatedVisitPayorUID { get; set; }

        public long? PatientBillableItemUID { get; set; }

        public string CanKeepDiscount { get; set; }


        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
