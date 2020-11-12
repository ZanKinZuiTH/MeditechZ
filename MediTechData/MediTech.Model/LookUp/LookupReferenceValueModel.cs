using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class LookupReferenceValueModel : LookupItemModel
    {
        public string ValueCode { get; set; }
        public string DomainCode { get; set; }

        public int DisplayOrder { get; set; }
        public double? NumericValue { get; set; }
        public string AlternateName { get; set; }
    }
}
