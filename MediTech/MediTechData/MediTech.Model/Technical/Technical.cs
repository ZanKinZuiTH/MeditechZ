using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class ReferenceDomainModel
    {
        public int UID { get; set; }
        public string DomainCode { get; set; }
        public string Description { get; set; }
        public System.DateTime ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public bool IsSortByDescription { get; set; }
        public string DLFlag { get; set; }

        public List<ReferencevalueModel> ReferenceValues { get; set; }
    }
    public class ReferencevalueModel
    {
        public int ReferenceValueUID { get; set; }
        public string Description { get; set; }
        public string ValueCode { get; set; }
        public int ReferenceDomainUID { get; set; }
        public Nullable<int> DisplayOrder { get; set; }
        public string DomainCode { get; set; }
        public System.Nullable<DateTime> ActiveFrom { get; set; }
        public System.Nullable<DateTime> ActiveTo { get; set; }
        public Nullable<int> SpecialityUID { get; set; }
        public Nullable<int> LANUGUID { get; set; }
        public string DLFlag { get; set; }
        public Nullable<double> NumericValue { get; set; }
        public string AlternateName { get; set; }

        public string IsUpdate { get; set; }
    }
    public class ReferenceRelationShipModel
    {
        public int ReferenceRelationShipUID { get; set; }
        public int SourceReferenceValueUID { get; set; }
        public int TargetReferenceValueUID { get; set; }
        public int SourceReferenceDomainUID { get; set; }
        public string SourceReferenceDomainCode { get; set; }
        public int TargetReferenceDomainUID { get; set; }
        public string TargetReferenceDomainCode { get; set; }
        public System.DateTime ActiveFrom { get; set; }
        public Nullable<System.DateTime> ActiveTo { get; set; }
        public int MUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int CUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }


}
