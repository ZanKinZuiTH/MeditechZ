using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupRuleItemModel
    {
        public int CheckupRuleItemUID { get; set; }
        public int CheckupRuleUID { get; set; }
        public int ResultItemUID { get; set; }
        public string ResultItemName { get; set; }
        public string Unit { get; set; }
        public Nullable<double> Low { get; set; }
        public Nullable<double> Hight { get; set; }
        public string Text { get; set; }
        public string IsText { get; set; }
        public string Operator { get; set; }
        public bool? NonCheckup { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
