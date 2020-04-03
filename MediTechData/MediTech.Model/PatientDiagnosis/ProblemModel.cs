using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ProblemModel
    {
        public int ProblemUID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string IsChapter { get; set; }
        public Nullable<System.DateTime> StartDttm { get; set; }
        public Nullable<System.DateTime> EndDttm { get; set; }
        public string Version { get; set; }
    }
}
