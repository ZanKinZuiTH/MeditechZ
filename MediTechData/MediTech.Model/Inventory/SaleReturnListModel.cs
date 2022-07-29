using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class SaleReturnListModel
    {
        public int SaleReturnListUID { get; set; }
        public int SaleReturnUID { get; set; }
        public int StockUID { get; set; }
        public string BatchID { get; set; }
        public int ItemMasterUID { get; set; }
        public string ItemName { get; set; }
        public double Quantity { get; set; }
        public int IMUOMUID { get; set; }
        public string IMUOM { get; set; }
        public double ItemCost { get; set; }
        public string Comments { get; set; }
        public long DispensedItemUID { get; set; }
        public long PatientBilledItemUID { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
