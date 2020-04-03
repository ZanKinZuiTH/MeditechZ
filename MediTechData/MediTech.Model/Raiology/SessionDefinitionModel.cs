using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class SessionDefinitionModel
    {
        public int SessionDefinitionUID { get; set; }
        public int CareproviderUID { get; set; }
        public string CareproviderName { get; set; }
        public System.DateTime StartDttm { get; set; }
        public System.DateTime EndDttm { get; set; }
        public System.DateTime SessionStartDttm { get; set; }
        public System.DateTime SessionEndDttm { get; set; }
        public bool Day1 { get; set; }
        public bool Day2 { get; set; }
        public bool Day3 { get; set; }
        public bool Day4 { get; set; }
        public bool Day5 { get; set; }
        public bool Day6 { get; set; }
        public bool Day7 { get; set; }
        public int CUser { get; set; }
        public int RequestNumber { get; set; }
        public int RequestBulkNumber { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
