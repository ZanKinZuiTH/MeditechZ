using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class IPFillDetailModel
    {
        public long IPFillDetailUID { get; set; }
        public string PatientFillID { get; set; }
        public long IPFillProcessUID { get; set; }
        public long PatientUID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public int? WardUID { get; set; }
        public string WardName { get; set; }
        public int? BedUID { get; set; }
        public string BedName { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public int? ItemMasterUID { get; set; }
        public string ItemName { get; set; }
        public double? Quantity { get; set; }
        public int? IMUOMUID { get; set; }
        public String IMUOM { get; set; }
        public string BatchID { get; set; }
        public DateTime ExpiryDttm { get; set; }
        public int? StockUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long PatientOrderUID { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string OwnerOrganisationName { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
