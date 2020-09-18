using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RequestResultLinkModel
    {
        public int RequestResultLinkUID { get; set; }
        public int ResultItemUID { get; set; }
        public int RequestItemUID { get; set; }
        public Nullable<int> PrintOrder { get; set; }
        public string ResultItemName { get; set; }
        public string ResultItemCode { get; set; }
        public string ResultValueType { get; set; }
        public string IsMandatory { get; set; }
        public string ExcludeFrmPrint { get; set; }
        public string Unit { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
