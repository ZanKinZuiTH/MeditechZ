using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemMasterModel
    {
        public int ItemMasterUID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ITMTYPUID { get; set; }
        public string ItemType { get; set; }
        public Nullable<System.DateTime> ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public Nullable<double> ItemCost { get; set; }
        public Nullable<int> DrugGenaricUID { get; set; }
        public Nullable<int> FRQNCUID { get; set; }
        public string GenaricName { get; set; }
        public string DispenseEnglish { get; set; }
        public string DispenseLocal { get; set; }
        public string OrderInstruction { get; set; }
        public Nullable<int> BaseUOM { get; set; }
        public Nullable<int> PDSTSUID { get; set; }
        public Nullable<int> FORMMUID { get; set; }
        public Nullable<int> ROUTEUID { get; set; }
        public string Route { get; set; }
        public string BaseUnit { get; set; }
        public Nullable<int> SalesUOM { get; set; } 
        public string SalesUnit { get; set; }
        public Nullable<int> PrescriptionUOM { get; set; }
        public string PrescriptionUnit { get; set; }
        public double? VATPercentage { get; set; }
        public string IsBatchIDMandatory { get; set; }
        public Nullable<int> ManufacturerByUID { get; set; }
        public string CanDispenseWithOutStock { get; set; }
        public string IsStock { get; set; }
        public string IsNarcotic { get; set; }
        public Nullable<int> NRCTPUID { get; set; }
        public string NarcoticType { get; set; }
        public string Comments { get; set; }
        public double? MinSalesQty { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string BatchID { get; set; }
        public Nullable<double> DoseQuantity { get; set; }
        public double BatchQty { get; set; }
        public double StockQty { get; set; }
        public int StockUID { get; set; }
        public DateTime? ExpiryDttm { get; set; }
        public int IMUOMUID { get; set; }
        public string SerialNumber { get; set; }

        public double? UnitPrice { get; set; }
        public List<ItemUOMConversionModel> ItemUOMConversions { get; set; }

        public List<ItemVendorDetailModel> ItemVendorDetails { get; set; }
    }
}
