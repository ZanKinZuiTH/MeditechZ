using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AllocateSplitItemModel
    {

        public double amount { get; set; }
        public double discount { get; set; }
        public double netAmount { get; set; }
        public int userUID { get; set; }
        public string isSplit { get; set; }
        public int groupUID { get; set; }
        public int subGroupUID { get; set; }
        public long currentVisitPayorUID { get; set; }
        public string canKeepDiscount { get; set; }
        public double discountDecimal { get; set; }
        public double amountDecimal { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
}
