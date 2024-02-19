using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientResultComponentModel : PatientVisitModel
    {
        public int No { get; set; }
        public long? ResultUID { get; set; }
        public string RequestItemType { get; set; }
        public string Catagory { get; set; }
        public string RequestItemName { get; set; }
        public string RequestItemCode { get; set; }
        public string ResultItemCode { get; set; }
        public string ResultItemName { get; set; }
        public string PrintAs { get; set; }
        public string ResultItemComments { get; set; }
        public string ReferenceRange { get; set; }
        public string ResultValue { get; set; }
        public string ResultValueRange { get; set; }
        public string ResultUnit { get; set; }
        public string IsAbnormal { get; set; }
        public int? Year { get; set; }
        public int? SortBy { get; set; }
        public int? SubSortBy { get; set; }
        public string ResultValue1 { get; set; }
        public string ResultValue2 { get; set; }
        public string ResultValue3 { get; set; }
    }
}
