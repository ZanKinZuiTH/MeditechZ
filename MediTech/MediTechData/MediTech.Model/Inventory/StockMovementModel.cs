using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class StockMovementModel
    {
        public int OrganisationUID { get; set; }
        public string OrganisationName { get; set; }
        public int LocationUID { get; set; }
        public string LocationName { get; set; }
        public int StoreUID { get; set; }
        public string StoreName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string BatchID { get; set; }
        public DateTime StockDttm { get; set; }
        public double TotalBFQty { get; set; }
        public double BFQty { get; set; }
        public double InQty { get; set; }
        public double OutQty { get; set; }
        public double BalQty { get; set; }
        public double TotBalQty { get; set; }
        public string Unit { get; set; }
        public int IMUOMUID { get; set; }
        public string RefNo { get; set; }
        public string RefTable { get; set; }
        public Nullable<long> RefUID { get; set; }
        public Nullable<int> RefPatientUID { get; set; }
        public string Note { get; set; }

    }
}
