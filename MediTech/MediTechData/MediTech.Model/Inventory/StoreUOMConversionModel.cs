using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class StoreUOMConversionModel
    {
        public int StoreUOMConversionUID { get; set; }
        public int BaseUOMUID { get; set; }
        public string BaseUnit { get; set; }
        public int ConversionUOMUID { get; set; }
        public string ConversionUnit { get; set; }
        public Nullable<double> ConversionValue { get; set; }
        public string StatusFlag { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
    }
}
