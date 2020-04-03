using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemVendorDetailModel
    {
        public int ItemVendorDetailUID { get; set; }
        public int ItemMasterUID { get; set; }
        public int VendorDetailUID { get; set; }
        public double ItemAmount { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
