using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class GRNDetailModel
    {
        public int GRNDetailUID { get; set; }
        public int VendorDetailUID { get; set; }
        public string VendorName { get; set; }
        public string GRNNumber { get; set; }
        public int RecievedOrganisationUID { get; set; }
        public string RecievedOrganisationName { get; set; }
        public int RecievedStoreUID { get; set; }
        public string StoreName { get; set; }
        public Nullable<System.DateTime> RecievedDttm { get; set; }
        public string Comments { get; set; }
        public double Discount { get; set; }
        public double NetAmount { get; set; }
        public double OtherCharges { get; set; }
        public string InvoiceNo { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<int> GRNSTSUID { get; set; }
        public string GRNStatus { get; set; }
        public string CancelReason { get; set; }
        public Nullable<int> GRNTYPUID { get; set; }
        public string GRNType { get; set; }
        public string PurchaseOrderID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public List<GRNItemListModel> GRNItemLists { get; set; }
    }
}
