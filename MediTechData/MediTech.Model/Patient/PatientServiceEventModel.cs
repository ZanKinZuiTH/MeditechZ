using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientServiceEventModel
    {
        public long PatientServiceEventUID { get; set; }
        public long PatientVistUID { get; set; }
        public int VISTSUID { get; set; }
        public string VisitStatus { get; set; }
        public string UserName { get; set; }
        public int LocationUID { get; set; }
        public string Location { get; set; }
        public System.DateTime EventStartDttm { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
