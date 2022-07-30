using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ProgressNoteModel
    {
        public int ProgressNoteUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string Note { get; set; }
        public DateTime? RecordedDttm { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public bool IsWellness { get; set; }
        public string RecordBy { get; set; }
        public string StatusFlag { get; set; }
    }
}
