using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupGroupResultModel
    {
        public int CheckupGroupResultUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string ItemNameResult { get; set; }
        public long GPRSTUID { get; set; }
        public int RABSTSUID { get; set; }
        public string Conclusion { get; set; }
        public string GroupCode { get; set; }
        public string GroupResult { get; set; }
        public string ResultStatus { get; set; }
        public int DisplayOrder { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
    }
}
