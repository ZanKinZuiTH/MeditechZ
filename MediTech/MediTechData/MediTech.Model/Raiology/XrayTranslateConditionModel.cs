using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class XrayTranslateConditionModel
    {
        public int XrayTranslateConditionUID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> CUser { get; set; }
        public Nullable<System.DateTime> CWhen { get; set; }
        public Nullable<int> MUser { get; set; }
        public Nullable<System.DateTime> MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
