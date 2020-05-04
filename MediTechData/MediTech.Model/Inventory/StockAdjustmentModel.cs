using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class StockAdjustmentModel
    {
        public int StockAdjustmentUID { get; set; }
        public int StoreUID { get; set; }
        public string StoreName { get; set; }
        public int StockUID { get; set; }
        public int ItemMasterUID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BatchID { get; set; }
        public string NewBatchID { get; set; }
        public double ActualQuantity { get; set; }
        public int ActualUOM { get; set; }
        public double QuantityAdjusted { get; set; }
        public double AdjustedQuantity { get; set; }
        public int AdjustedUOM { get; set; }
        public string AdjustedUnit { get; set; }
        public int StockAdjustmentID { get; set; }
        public string Comments { get; set; }
        public Nullable<double> ItemCost { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string Place { get; set; }
        public int? PLACEUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
    }
}
