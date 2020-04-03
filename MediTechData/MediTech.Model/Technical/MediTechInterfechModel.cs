using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class MediTechInterfechModel
    {
        public int MediTechInterfechUID { get; set; }
        public int MediTechInterfechDetailUID { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }

        public int? ParentUID { get; set; }
        public string ValueCode { get; set; }
        public string ValueDescription { get; set; }
        public string Value { get; set; }
    }
}
