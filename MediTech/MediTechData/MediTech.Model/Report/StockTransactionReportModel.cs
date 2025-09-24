using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class StockTransactionReportModel
    {
        public long RowNumber
        {
            get;
            set;
        }

        public string IssuedOrganisation
        {
            get;
            set;
        }

        public string IssuedStore
        {
            get;
            set;
        }

        public string RequestOrganisation
        {
            get;
            set;
        }

        public string RequestStore
        {
            get;
            set;
        }

        public string IssueID
        {
            get;
            set;
        }

        public string RequestID
        {
            get;
            set;
        }

        public string ReceiveID
        {
            get;
            set;
        }

        public string DisposeID { get; set; }

        public string ReceivedOrganisation
        {
            get;
            set;
        }

        public string ReceivedStore
        {
            get;
            set;
        }

        public string IssueBy
        {
            get;
            set;
        }

        public string RequestBy
        {
            get;
            set;
        }

        public string ReceivedBy
        {
            get;
            set;
        }

        public string ItemCode
        {
            get;
            set;
        }

        public string ItemName
        {
            get;
            set;
        }

        public double Itemcost
        {
            get;
            set;
        }

        public double Price
        {
            get;
            set;
        }

        public string BatchID
        {
            get;
            set;
        }

        public double Quantity
        {
            get;
            set;
        }

        public double CurrentQuantity
        {
            get;
            set;
        }

        public double MainCurrentQuantity
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        public DateTime? ExpiryDttm
        {
            get;
            set;
        }

        public DateTime? IssuedDttm
        {
            get;
            set;
        }

        public DateTime? RequestedDttm
        {
            get;
            set;
        }

        public DateTime? ReceivedDttm
        {
            get;
            set;
        }

        public double NetCost
        {
            get;
            set;
        }

        public string Comments
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }
    }
}
