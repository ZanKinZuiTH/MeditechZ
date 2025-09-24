using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class PatientRiskBookModel : PatientWellnessModel
    {
        public PatientMedicalHistoryModel MedicalHistory { get; set; }
        public List<PatientAddressModel> PatientAddresses { get; set; }
        public List<PatientWorkHistoryModel> WorkHistorys { get; set; }
        public List<PatientInjuryModel> InjuryDetails { get; set; }
    }
}
