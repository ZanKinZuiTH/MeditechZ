using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientVisitIDModel
    {
        public long UID { get; set; }
        public string Identifier { get; set; }
        public long PatientVisitUID { get; set; }
        public DateTime ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
