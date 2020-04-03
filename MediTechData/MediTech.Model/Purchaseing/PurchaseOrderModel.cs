using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PurchaseOrderModel
    {
        public int PurchaseOrderUID { get; set; }
        public string PurchaseOrderID { get; set; }

        public int VendorDetailUID { get; set; }
        public string VendorName { get; set; }
        public Nullable<System.DateTime> RequiredDttm { get; set; }
        public System.DateTime RequestedDttm { get; set; }
        public Nullable<int> DelieryToStoreUID { get; set; }
        public string StoreName { get; set; }
        public Nullable<int> HealthOrganisationUID { get; set; }
        public string HealthOrganisationName { get; set; }
        public string Comments { get; set; }
        public int POSTSUID { get; set; }
        public string POStatus { get; set; }
        public double OtherCharges { get; set; }
        public double NetAmount { get; set; }
        public double Discount { get; set; }
        public string CancelReason { get; set; }
        public string GRNNumber { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public string Approved { get; set; }
        public string ApprovalComments { get; set; }
        public Nullable<int> CUser { get; set; }
        public Nullable<System.DateTime> CWhen { get; set; }
        public Nullable<int> MUser { get; set; }
        public Nullable<System.DateTime> MWhen { get; set; }
        public string StatusFlag { get; set; }
        public List<PurchaseOrderItemListModel> PurchaseOrderItemList { get; set; }
        public List<PurchaseOrderPaymentModel> PurchaseOrderPayments { get; set; }
    }
}
