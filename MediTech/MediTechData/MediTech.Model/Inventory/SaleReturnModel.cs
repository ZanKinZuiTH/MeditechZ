using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class SaleReturnModel
    {
        public int SaleReturnUID { get; set; }
        public int SaleUID { get; set; }
        public string ReturnID { get; set; }
        public string IsCancelDispense { get; set; }
        public int ReturnedBy { get; set; }
        public string ReturnedByName { get; set; }
        public DateTime ReturnDttm { get; set; }
        public string Comments { get; set; }
        public long PatientUID { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public long PatientVisitUID { get; set; }
        public int StoreUID { get; set; }
        public string StoreName { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
