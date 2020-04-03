using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class StockOnHandModel
    {
        public int OrganisationUID { get; set; }
        public string OrganisationName { get; set; }
        public int StockUID { get; set; }
        public int StoreUID { get; set; }
        public string StoreName { get; set; }
        public int ItemMasterUID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public int IMUOMUID { get; set; }
        public string Unit { get; set; }
        public string BatchID { get; set; }
        public DateTime? ExpiryDttm { get; set; }
        public string IsExpiry { get; set; }
        public string VendorName { get; set; }

    }
}
