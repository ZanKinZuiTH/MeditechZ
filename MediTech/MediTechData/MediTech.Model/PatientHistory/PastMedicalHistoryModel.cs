using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PastMedicalHistoryModel
    {
        public int PastMedicalHistoryUID { get; set; }
        public int PatientMedicalHistoryUID { get; set; }
        public Nullable<System.DateTime> MedicalDttm { get; set; }
        public string MedicalName { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
