using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class SearchOrderItem
    {
        public long No { get; set; }
        public int BillableItemUID { get; set; }
        public string Code { get; set; }
        public string ItemName { get; set; }
        public string Generic { get; set; }
        public string TypeOrder { get; set; }
    }
}
