using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CCHPIModel
    {
        public long CCHPIUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public Nullable<int> CCHPIMasterUID { get; set; }
        public string Complaint { get; set; }
        public string DisplayComplaint { get; set; }
        public Nullable<int> Period { get; set; }
        public Nullable<int> AGUOMUID { get; set; }
        public string DateUnit { get; set; }
        public string Presentillness { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string RecordBy { get; set; }
        public string StatusFlag { get; set; }
    }
}
