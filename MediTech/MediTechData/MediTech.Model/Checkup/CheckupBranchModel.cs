using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupCompanyModel
    {
        public int CheckupJobUID { get; set; }
        public string GPRSTUIDs { get; set; }
        public string CompanyName { get; set; }
        public int GPRSTUID { get; set; }
        public int? StartRow { get; set; }
        public int? EndRow { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
