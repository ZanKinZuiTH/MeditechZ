using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class DispenseReturnModel
    {
        public string PrescriptionNumber { get; set; }
        public string DrugCode { get; set; }
        public string DrugName { get; set; }
        public double? DispenseQty { get; set; }
        public double? ReturnQty { get; set; }
        public double? PreviousQty { get; set; }
        public string Unit { get; set; }
        public string IsBilled { get; set; }
        public string BatchID { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string DispenseBy { get; set; }
        public DateTime DispenseDate { get; set; }
        public string IsContinuous { get; set; }
        public double? ItemCost { get; set; }
        public double? AvgCost { get; set; }
        public int ItemMasterUID { get; set; }
        public int StockUID { get; set; }
        public int StoreUID { get; set; }
    }
}
