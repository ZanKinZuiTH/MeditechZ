using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ResultItemModel
    {
        public int ResultItemUID { get; set; }
        public string Description { get; set; }
        public string DisplyName { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> EffectiveFrom { get; set; }
        public Nullable<System.DateTime> EffectiveTo { get; set; }
        public Nullable<int> UnitofMeasure { get; set; }
        public string UOM{ get; set; }
        public Nullable<int> RVTYPUID { get; set; }
        public string ResultType { get; set; }
        public string IsCumulative { get; set; }
        public Nullable<int> GPRSTUID { get; set; }
        public string AutoValue { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        public string Comments { get; set; }   
        public List<ResultItemRangeModel> ResultItemRanges { get; set; }

    }
}
