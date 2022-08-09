using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RequestDetailItemModel
    {
        public long RequestDetailUID { get; set; }
        public long RequestUID { get; set; }
        public string RequestItemName { get; set; }
        public string RequestItemCode { get; set; }
        public int RequestItemUID { get; set; }
        public string OrderStatus { get; set; }
        public string ResultedEnterBy { get; set; }
        public DateTime? ResultEnteredDttm { get; set; }
        public string PriorityStatus { get; set; }
        public string Comments { get; set; }
        public long? ResultUID { get; set; }
        public long PatientUID { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public int? SEXXXUID { get; set; }
        public string RequestNumber { get; set; }
        public long PatientVisitUID { get; set; }
        public string IsConfidential { get; set; }
        public int? ResultEnterUID { get; set; }
        public  ObservableCollection<ResultComponentModel> ResultComponents { get; set; }
    }
}
