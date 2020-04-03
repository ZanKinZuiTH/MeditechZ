using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientOrderAlertModel
    {
        public long PatientOrderAlertUID { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public string AlertType { get; set; }
        public string AlertMessage { get; set; }
        public int OverrideByUserUID { get; set; }
        public int OverrideRSNUID { get; set; }
        public string OverrideRemarks { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
