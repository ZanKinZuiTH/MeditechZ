using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientMergeDetailModel
    {
        public int PatientMergeDetailUID { get; set; }
        public int PatientMergeUID { get; set; }
        public int IdentifyingUID { get; set; }
        public string IdentifyingType { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
