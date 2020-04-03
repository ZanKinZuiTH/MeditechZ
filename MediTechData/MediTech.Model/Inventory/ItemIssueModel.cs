using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemIssueModel
    {
        public int ItemIssueUID { get; set; }
        public int StoreUID { get; set; }
        public string Store { get; set; }

        public int OrganisationUID { get; set; }
        public string Organisation { get; set; }
        public int RequestedByOrganisationUID { get; set; }
        public string RequestedByOrganisation { get; set; }
        public int RequestedByStoreUID { get; set; }
        public string RequestedByStore { get; set; }
        public System.DateTime ItemIssueDttm { get; set; }
        public string ItemIssueID { get; set; }
        public int? ItemRequestUID { get; set; }
        public string ItemRequestID { get; set; }

        public int? ItemReceiveUID { get; set; }
        public string ItemReceiveID { get; set; }
        public Nullable<int> ISUSTUID { get; set; }
        public string IssueStatus { get; set; }
        public Nullable<int> ISSTPUID { get; set; }
        public string IssueType { get; set; }
        public int IssueBy { get; set; }
        public string IssueByName { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string CancelReason { get; set; }
        public double OtherCharges { get; set; }
        public double NetAmount { get; set; }
        public List<ItemIssueDetailModel> ItemIssueDetail { get; set; }
    }
}
