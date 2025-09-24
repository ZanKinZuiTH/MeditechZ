using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class StockSummaryModel
    {
        public DateTime StockDate { get; set; }
        public double BringForward { get; set; }
        public double StockReceive { get; set; }
        public double GoodReceive { get; set; }
        public double AdjustStockIn { get; set; }
        public double BalanceStockStart { get; set; }
        public double TransferStockIn { get; set; }
        public double TransferStockOut { get; set; }
        public double StockUsed { get; set; }
        public double AdjustStockOut { get; set; }
        public double DisposeStock { get; set; }
        public double ConsumptionStock { get; set; }
        public double IssueStock { get; set; }
        public double SalesReturn { get; set; }
        public double CancelDispensed { get; set; }
        public double BalanceStockEnd { get; set; }
    }
}
