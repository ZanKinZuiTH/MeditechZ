using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientMergeModel
    {
        public int PatientMergeUID { get; set; }
        public Nullable<int> MergedByUID { get; set; }
        public string MergedBy { get; set; }
        public Nullable<System.DateTime> MergedDttm { get; set; }
        public long MajorPatientUID { get; set; }
        public string MajorPatientID { get; set; }
        public string MajorPatientName { get; set; }
        public long MinorPatientUID { get; set; }
        public string MinorPatientID { get; set; }
        public string MinorPatientName { get; set; }
        public string IsUnMerge { get; set; }
        public Nullable<int> MRGTPUID { get; set; }
        public string MergeType { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
