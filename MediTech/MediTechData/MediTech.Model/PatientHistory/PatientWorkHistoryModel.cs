using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientWorkHistoryModel
    {
        public int PatientWorkHistoryUID { get; set; }
        public long PatientUID { get; set; }
        public string CompanyName { get; set; }
        public string Business { get; set; }
        public string Description { get; set; }
        public string Riskfactor { get; set; }
        public string Timeperiod { get; set; }
        public string Equipment { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
