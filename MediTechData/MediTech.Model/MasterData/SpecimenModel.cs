using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class SpecimenModel
    {
        public int SpecimenUID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Nullable<int> SPMTPUID { get; set; }
        public string SpecimenType { get; set; }
        public Nullable<bool> IsVolumeCollectionReqd { get; set; }
        public Nullable<double> VolumeCollected { get; set; }
        public Nullable<int> VOUNTUID { get; set; }
        public string VolumnUnit { get; set; }
        public Nullable<System.DateTime> ExpiryDttm { get; set; }
        public Nullable<int> COLSTUID { get; set; }
        public string CollectionSite { get; set; }
        public Nullable<int> CLROUUID { get; set; }
        public string CollectionRoute { get; set; }
        public Nullable<int> COLMDUID { get; set; }
        public string CollectionMethod { get; set; }
        public string StorageInstuction { get; set; }
        public string HandlingInstruction { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
