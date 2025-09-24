using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientDiagnosisModel
    {
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public long PatientVisitUID { get; set; }
        public DateTime VisitDate { get; set; }
        public List<PatientProblemModel> Problems { get; set; }
    }
}
