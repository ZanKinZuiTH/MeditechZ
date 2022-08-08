using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RequestItemModel
    {
        public int RequestItemUID { get; set; }
        public string Comments { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public Nullable<int> PRTGPUID { get; set; }
        public Nullable<int> TSTTPUID { get; set; }
        public string TestType { get; set; }
        public Nullable<int> RIMTYPUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string IsConfidential { get; set; }
        public List<RequestResultLinkModel> RequestResultLinks { get; set; }
        public List<RequestItemSpecimenModel> RequestItemSpecimens { get; set; }

        public List<RequestItemGroupResultModel> RequestItemGroupResults { get; set; }
    }
}
