using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupSummeryResultModel
    {
        public int CheckupSummeryResultUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long GPRSTUID { get; set; }
        public int RABSTSUID { get; set; }
        public string Description { get; set; }
        public string Recommend { get; set; }
        public string SummeryResult { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
    }
}
