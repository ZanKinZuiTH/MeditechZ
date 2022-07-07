using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AllocatePatientBillableItem
    {

        public long patientUID { get; set; }

        public long patientVisitUID { get; set; }

        public int ownerOrganisationUID { get; set; }
        public int userUID { get; set; }

        public string isAutoAllocate { get; set; }

        public int? groupUID { get; set; }

        public int? subGroupUID { get; set; }

        public long? patientVisitPayorUID { get; set; }

        public int? payorAgreementUID { get; set; }

        public int? allocatedVisitPayorUID { get; set; }

        public int? patientBillableItemUID { get; set; }

        public string canKeepDiscount { get; set; }


        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }
    }
}
