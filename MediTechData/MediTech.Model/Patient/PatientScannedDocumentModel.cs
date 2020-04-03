using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientScannedDocumentModel
    {
        public int PatientScannedDocumentUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string DocumentName { get; set; }
        public byte[] ScannedDocument { get; set; }
        public System.DateTime DocUploadedDttm { get; set; }
        public int DocUploadedBy { get; set; }
        public string DocUploadedByName { get; set; }
        public string Comments { get; set; }
        public int SCDTYUID { get; set; }
        public string ScanDocumentType { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
