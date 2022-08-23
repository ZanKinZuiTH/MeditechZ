using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class DispenseReturnModel
    {
        public long PatientUID { get; set; }
        public string PrescriptionNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public double? DispensedQty { get; set; }
        public double? ReturnQty { get; set; }
        public double? PreviousReturnQty { get; set; }
        public int IMUOMUID { get; set; }
        public string UOMDesc { get; set; }

        public string BatchID { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string DispensedUser { get; set; }
        public DateTime DispensedDttm { get; set; }
        public string PrescriptionStatus { get; set; }
        public string IsContinuous { get; set; }
        public double? AvgCost { get; set; }
        public int ItemMasterUID { get; set; }
        public int StockUID { get; set; }
        public int StoreUID { get; set; }
        public long DispensedItemUID { get; set; }
        public long PatientOrderDetailUID { get; set; }
        public int PatientBillableItemUID { get; set; }
        public int PatientBilledItemUID_Is_Null { get; set; }

        public string CanReturn { get; set; }
        public bool BilledIsCannotReturn { get; set; }

        public bool IsCanReturn
        {
            get { return (BilledIsCannotReturn == false && PatientBilledItemUID_Is_Null == 0) ? true : false; }
        }
        public bool IsBilled
        {
            get { return PatientBilledItemUID_Is_Null != 0 ? true : false; }
        }

        public bool Selected { get; set; }

        public int MUser { get; set; }
    }
}
