using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemAverageCostModel
    {
        public int ItemAverageCostUID { get; set; }
        public int ItemMasterUID { get; set; }
        public System.DateTime StartDttm { get; set; }
        public Nullable<System.DateTime> EndDttm { get; set; }
        public Nullable<double> InQty { get; set; }
        public double UnitCost { get; set; }
        public double BFQty { get; set; }
        public Nullable<double> BFAvgCost { get; set; }
        public double AvgCost { get; set; }
        public int IMUOMUID { get; set; }
        public string Unit { get; set; }
        public string RefNo { get; set; }
        public string RefTable { get; set; }
        public Nullable<int> RefUID { get; set; }
        public string Note { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string OwnerOrganisationName { get; set; }
    }
}
