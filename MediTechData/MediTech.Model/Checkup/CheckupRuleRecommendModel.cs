using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupRuleRecommendModel
    {
        public int CheckupRuleRecommendUID { get; set; }
        public int CheckupRuleUID { get; set; }
        public int CheckupTextMasterUID { get; set; }
        public string Recommend { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
