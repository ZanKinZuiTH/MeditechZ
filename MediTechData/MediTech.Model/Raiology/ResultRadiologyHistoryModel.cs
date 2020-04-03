using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ResultRadiologyHistoryModel
    {
        public virtual long ResultRadiologyHistoryUID { get; set; }
        public virtual long ResultUID { get; set; }
        public virtual int ResultVersion { get; set; }
        public System.Nullable<DateTime> CWhen { get; set; }
        public virtual string ResultPlainText { get; set; }

        public virtual string ResultValue { get; set; }
    }
}
