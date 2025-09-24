using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Icheckup
{
   public class IcehckUpdataModel
    {

        public long PatientVisitUID { get; set; }
        public string VisitID { get; set; }
        public string IsBillFinalized { get; set; }
        public string Comments { get; set; }
        public int? RefNo { get; set; }
        public int? BookingUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public string OwnerOrganisation { get; set; }
        public int PayorDetailUID { get; set; }
        public string PayorName { get; set; }
        public int PayorAgreementUID { get; set; }
        public string WellnessResult { get; set; }
        public bool IsWellnessResult { get; set; }
        public int WellnessResultUID { get; set; }
        public string OnBLIFE { get; set; }
        public int RowHandle { get; set; }
        public long RowNumber { get; set; }
        public int No { get; set; }
        public long? ResultUID { get; set; }
        public string RequestItemType { get; set; }
        public string Catagory { get; set; }
        public string RequestItemName { get; set; }
        public string RequestItemCode { get; set; }
        public string ResultItemCode { get; set; }
        public string ResultItemName { get; set; }
        public string ReferenceRange { get; set; }
        public string ResultValue { get; set; }
        public string ResultValueRange { get; set; }
        public string ResultUnit { get; set; }
        public string IsAbnormal { get; set; }
        public int? Year { get; set; }
        public int? SortBy { get; set; }
        public int? SubSortBy { get; set; }
        public string ResultValue1 { get; set; }
        public string ResultValue2 { get; set; }
        public string ResultValue3 { get; set; }
    }
}
