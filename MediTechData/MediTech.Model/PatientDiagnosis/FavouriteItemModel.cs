using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class FavouriteItemModel
    {
        public int FavouriteItemUID { get; set; }
        public int ProblemUID { get; set; }
        public string ProblemCode { get; set; }
        public string ProblemName { get; set; }
        public string ProblemDescription { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
