using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ResultItemRangeModel
    {
        public int ResultItemRangeUID { get; set; }
        public int ResultItemUID { get; set; }
        public int LABRAMUID { get; set; }
        public string LabRangeMaster { get; set; }
        public int? SEXXXUID { get; set; }
        public string Gender { get; set; }
        public double? Low { get; set; }
        public double? High { get; set; }
        public string DisplayValue { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
