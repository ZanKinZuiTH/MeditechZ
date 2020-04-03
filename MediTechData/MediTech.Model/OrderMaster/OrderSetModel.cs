using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class OrderSetModel
    {
        public int OrderSetUID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OrdersetNameSearch { get; set; }
        public Nullable<System.DateTime> ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public Nullable<int> OwnerOrganisationUID { get; set; }
        public string OwnerOrganisationName { get; set; }
        public double Price { get; set; }
        public List<OrderSetBillableItemModel> OrderSetBillableItems { get; set; }
    }
}
