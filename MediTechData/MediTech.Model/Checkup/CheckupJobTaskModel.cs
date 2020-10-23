using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class CheckupJobTaskModel
    {
        public int CheckupJobTaskUID { get; set; }
        public int CheckupJobContactUID { get; set; }
        public int GPRSTUID { get; set; }
        public string GroupResultName { get; set; }
        public int? DisplayOrder { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
