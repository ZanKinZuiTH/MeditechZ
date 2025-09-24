using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class CheckupJobOrderModel :PatientVisitModel
    {
        public string OrderSetName { get; set; }
        public string ItemName { get; set; }
        public double? NetAmount { get; set; }
    }
}
