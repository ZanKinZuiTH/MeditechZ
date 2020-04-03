using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{

    public partial class XrayTranslateMappingModel
    {
        public int XrayTranslateMappingUID { get; set; }
        public string EngResult { get; set; }
        public string ThaiResult { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<bool> IsKeyword { get; set; }
        public Nullable<int> CUser { get; set; }
        public Nullable<System.DateTime> CWhen { get; set; }
        public Nullable<int> MUser { get; set; }
        public Nullable<System.DateTime> MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
