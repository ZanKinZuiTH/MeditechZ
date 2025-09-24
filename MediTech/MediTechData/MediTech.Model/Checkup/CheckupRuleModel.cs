using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupRuleModel
    {
        public int CheckupRuleUID { get; set; }
        public string Name { get; set; }
        public Nullable<int> SEXXXUID { get; set; }
        public Nullable<int> AgeFrom { get; set; }
        public Nullable<int> AgeTo { get; set; }
        public int RABSTSUID { get; set; }
        public int GPRSTUID { get; set; }
        public string Gender { get; set; }
        public string ResultStatus { get; set; }
        public string GroupResult { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public List<CheckupRuleRecommendModel> CheckupRuleRecommend { get; set; }
        public List<CheckupRuleDescriptionModel> CheckupRuleDescription { get; set; }
        public List<CheckupRuleItemModel> CheckupRuleItem { get; set; }
    }
}
