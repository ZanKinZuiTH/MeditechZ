using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ItemReceiveModel
    {
        public int ItemRecieveUID { get; set; }
        public int OrganisationUID { get; set; }
        public string Organisation { get; set; }
        public string LocationName { get; set; }
        public int LocationUID { get; set; }
        public int StoreUID { get; set; }
        public string Store { get; set; }
        public int IssuedByStoreUID { get; set; }
        public int IssuedByLocationUID { get; set; }
        public int IssuedByOrganisationUID { get; set; }
        public string IssuedByStore { get; set; }
        public string IssuedByLocation { get; set; }
        public string IssuedByOrganisation { get; set; }
        public int? ItemIssueUID { get; set; }
        public string ItemIssueID { get; set; }
        public Nullable<int> RCSTSUID { get; set; }
        public string ReceiveStatus { get; set; }
        public int ReceiveBy { get; set; }
        public string ReceiveByName { get; set; }
        public DateTime ReceivedDttm { get; set; }
        public string ItemReceiveID { get; set; }
        public string CancelReason { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string Comments { get; set; }
        public double NetAmount { get; set; }
        public double OtherCharges { get; set; }
        public List<ItemReceiveDetailModel> ItemReceiveDetail { get; set; }
    }
}
