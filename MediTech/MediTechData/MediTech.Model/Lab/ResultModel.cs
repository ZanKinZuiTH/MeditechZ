using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ResultModel
    {
        public long ResultUID { get; set; }
        public long RequestDetailUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public Nullable<System.DateTime> ResultEnteredDttm { get; set; }
        public Nullable<int> ResultEnteredUserUID { get; set; }
        public string ResultEnteredUser { get; set; }
        public int ORDSTUID { get; set; }
        public string OrderStatus { get; set; }
        public string ResultNumber { get; set; }
        public string LabNumber { get; set; }
        public Nullable<int> RABSTSUID { get; set; }
        public Nullable<bool> IsCaseStudy { get; set; }
        public string Comments { get; set; }
        public string RequestItemName { get; set; }
        public string RequestItemCode { get; set; }
        public int RequestItemUID { get; set; }
        public Nullable<int> ResultedByUID { get; set; }
        public Nullable<int> RadiologistUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        public List<ResultComponentModel> ResultComponents { get; set; }
    }
}
