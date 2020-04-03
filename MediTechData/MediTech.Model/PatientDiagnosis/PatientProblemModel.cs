using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientProblemModel
    {
        public int? BDLOCUID { get; set; }
        public string BodyLocation { get; set; }
        public int? CERNTUID { get; set; }
        public string Certanity { get; set; }
        public string ClosureComments { get; set; }
        public DateTime? ClosureDttm { get; set; }
        public int? CUser { get; set; }
        public DateTime? CWhen { get; set; }
        public int? DIAGTYPUID { get; set; }
        public string DiagnosisType { get; set; }
        public string IsPrimary { get; set; }
        public string IsUnderline { get; set; }
        public int? MUser { get; set; }
        public DateTime? MWhen { get; set; }
        public DateTime? OnsetDttm { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public int? PBMTYUID { get; set; }
        public string ProblemType { get; set; }
        public string ProblemCode { get; set; }
        public string ProblemDescription { get; set; }
        public string ProblemName { get; set; }
        public int ProblemUID { get; set; }
        public int? RecordedBy { get; set; }
        public string RecordedName { get; set; }
        public DateTime? RecordedDttm { get; set; }
        public int? SEVRTUID { get; set; }
        public string Severity { get; set; }
        public string StatusFlag { get; set; }
        public byte[] TIMESTAMP { get; set; }
        public long PatientProblemUID { get; set; }
    }
}
