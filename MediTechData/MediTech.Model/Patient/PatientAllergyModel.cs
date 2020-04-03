using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientAllergyModel
    {
        public long PatientAllergyUID { get; set; }
        public long PatientUID { get; set; }
        public Nullable<long> PatientVisitUID { get; set; }
        public Nullable<int> ALRCLUID { get; set; }
        public string AllergyClass { get; set; }
        public string AllergyDescription { get; set; }

        public Nullable<int> IdentifyingUID { get; set; }
        public string IdentifyingType { get; set; }
        public string AllergicTo { get; set; }
        public Nullable<int> SEVRTUID { get; set; }
        public string Severity { get; set; }
        public Nullable<int> CERNTUID { get; set; }
        public string Certanity { get; set; }
        public string RecordByName { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
