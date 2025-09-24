using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class SessionWithdrawnDetailModel
    {
        public int SessionWithdrawnUID { get; set; }

        public int SessionDefinitionUID { get; set; }
        public int WTHRSUID { get; set; }
        public string WithDrawReason { get; set; }
        public System.DateTime StartDttm { get; set; }
        public System.DateTime EndDttm { get; set; }
        public string Comments { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
