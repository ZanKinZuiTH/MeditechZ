using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class StockModel
    
    {
        public int StockUID { get; set; }
        public int OrganisationUID { get; set; }
        public string OrganisationName { get; set; }
        public int StoreUID { get; set; }
        public string StoreName { get; set; }
        public int ItemMasterUID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public int VenderDetailUID { get; set; }
        public string VendorName { get; set; }
        public int ManufacturerUID { get; set; }
        public string ManufacturerName { get; set; }
        public double Quantity { get; set; }
        public int IMUOMUID { get; set; }
        public string Unit { get; set; }
        public string BatchID { get; set; }
        public Nullable<double> ItemCost { get; set; }
        public string IsExpired { get; set; }
        public Nullable<System.DateTime> ExpiryDttm { get; set; }
        public string Comments { get; set; }
        public Nullable<int> DSPSTUID { get; set; }
        public string DisposeComments { get; set; }
        public double DisposeQty { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string SerialNumber { get; set; }
    }
}
