using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientDemographicLogModel
    {
        public long UID { get; set; }
        public long PatientUID { get; set; }
        public string TableName { get; set; }
        public string FiledName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public int CUser { get; set; }
        public DateTime? ModifiedDttm { get; set; }
        public int Modifiedby { get; set; }
        public string ModifiedbyName { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public int OwnerOrganisationUID { get; set; }

        public string OwnerOrganisationName { get; set; }
        public string StatusFlag { get; set; }
    }
}
