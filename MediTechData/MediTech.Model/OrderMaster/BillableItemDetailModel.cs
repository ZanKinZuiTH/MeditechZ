using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class BillableItemDetailModel
    {
        public int BillableItemDetailUID { get; set; }
        public int BillableItemUID { get; set; }
        public int OwnerOrganisationUID { get; set; }

        public string OwnerOrganisationName { get; set; }

        public Nullable<DateTime> ActiveFrom { get; set; }
        public Nullable<DateTime> ActiveTo { get; set; }
        public double Price { get; set; }
        public int CURNCUID { get; set; }
        public String Unit { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
