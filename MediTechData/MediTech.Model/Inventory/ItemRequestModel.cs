using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemRequestModel
    {
        public int ItemRequestUID { get; set; }
        public int OrganisationUID { get; set; }
        public string Organisation { get; set; }
        public int StoreUID { get; set; }
        public string Store { get; set; }
        public int RequestOnOrganistaionUID { get; set; }
        public string RequestOnOrganisation { get; set; }
        public int RequestOnStoreUID { get; set; }
        public string RequestOnStore { get; set; }
        public Nullable<int> RQSTSUID { get; set; }
        public string RequestStatus { get; set; }
        public int RequestedBy { get; set; }
        public DateTime RequestedDttm { get; set; }
        public string RequestedByName { get; set; }
        public string PreferredVendorName { get; set; }
        public Nullable<int> PreferredVendorUID { get; set; }
        public Nullable<int> IRPRIUID { get; set; }
        public string Priority { get; set; }
        public string ItemRequestID { get; set; }
        public string ItemIssueID { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string CancelReason { get; set; }
        public List<ItemRequestDetailModel> ItemRequestDetail { get; set; }
    }
}
