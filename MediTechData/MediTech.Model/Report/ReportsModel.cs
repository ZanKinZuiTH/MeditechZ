using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ReportsModel
    {
        public int ReportsUID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ViewCode { get; set; }
        public string ReportGroup { get; set; }
        public string NamespaceName { get; set; }
        public bool IsChecked { get; set; }
    }
}
