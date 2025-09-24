using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemUOMConversionModel
    {
        public int ItemUOMConversionUID { get; set; }
        public int ItemMasterUID { get; set; }
        public int BaseUOMUID { get; set; }
        public int ConversionUOMUID { get; set; }
        public string ConversionUnit { get; set; }
        public double ConversionValue { get; set; }
        public int? NumericValue { get; set; }
        public string Ratio { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
