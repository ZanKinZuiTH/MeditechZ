using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RequestItemGroupResultModel
    {
        public int RequestItemGroupResultUID { get; set; }
        public int RequestItemUID { get; set; }
        public int GPRSTUID { get; set; }
        public string GroupResultName { get; set; }
        public Nullable<int> PrintOrder { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int Muser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
