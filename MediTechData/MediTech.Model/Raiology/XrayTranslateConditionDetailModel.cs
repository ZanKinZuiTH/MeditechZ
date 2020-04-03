using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class XrayTranslateConditionDetailModel
    {
        public int XrayTranslateConditionDetailUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> XrayTranslateConditionUID { get; set; }
        public Nullable<int> CUser { get; set; }
        public Nullable<System.DateTime> CWhen { get; set; }
        public Nullable<int> MUser { get; set; }
        public Nullable<System.DateTime> MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
