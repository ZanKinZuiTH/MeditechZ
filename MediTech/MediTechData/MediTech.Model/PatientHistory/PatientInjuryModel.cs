using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientInjuryModel
    {
        public int PatientInjuryUID { get; set; }
        public long PatientUID { get; set; }
        public Nullable<System.DateTime> OccuredDate { get; set; }
        public string BodyLocation { get; set; }
        public string InjuryDetail { get; set; }
        public Nullable<int> INRYSEVUID { get; set; }
        public string InjuryServerity { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public Nullable<System.DateTime> CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
