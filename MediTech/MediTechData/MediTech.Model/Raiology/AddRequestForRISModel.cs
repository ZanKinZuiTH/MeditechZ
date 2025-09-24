using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class AddRequestForRISModel
    {
        public long PatientUID { get; set; }
        public long PateintVisitUID { get; set; }
        public int PayorDetailUID { get; set; }
        public int PayorAgreementUID { get; set; }
        public int PriorityUID { get; set; }
        public string Comments { get; set; }
        public System.Nullable<int> RadiologistUID { get; set; }
        public List<RequestItemModel> RequestItems {get; set;}
    }
}
