using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class RevenuePerDayModel
    {
        public Int64 RowNumber { get; set; }
        public DateTime BillGeneratedDttm { get; set; }
        public string VisitType { get; set; }

        public int? VisitCount { get; set; }
        public double? NetAmount { get; set; }
        public string PaymentMethod { get; set; }
        public double BeforeRevenue { get; set; }

        public int OwnerOrganisationUID { get; set; }
        public string HealthOrganisationName { get; set; }
    }
}
