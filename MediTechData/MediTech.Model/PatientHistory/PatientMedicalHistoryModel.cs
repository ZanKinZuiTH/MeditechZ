using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientMedicalHistoryModel
    {
        public int PatientMedicalHistoryUID { get; set; }
        public long PatientUID { get; set; }
        public string LungDisease { get; set; }
        public string Epilepsy { get; set; }
        public string Unconscious { get; set; }
        public string Hypertension { get; set; }
        public string Anemia { get; set; }
        public string ChronicDisease { get; set; }
        public string SurgicalDetail { get; set; }
        public string ImmunizationDetail { get; set; }
        public string Familyhistory { get; set; }
        public string LongTemMedication { get; set; }
        public string AllergyDescription { get; set; }
        public string Smoke { get; set; }
        public string SmokePeriodYear { get; set; }
        public string SmokePeriodMonth { get; set; }
        public string BFQuitSmoke { get; set; }
        public string Alcohol { get; set; }
        public string AlcohoPeriodYear { get; set; }
        public string AlcohoPeriodMonth { get; set; }
        public string Narcotic { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        public List<PastMedicalHistoryModel> PastMedicalHistorys { get; set; }
    }
}
