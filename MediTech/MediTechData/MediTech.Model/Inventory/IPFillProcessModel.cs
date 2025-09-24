using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class IPFillProcessModel
    {
        public long IPFillProcessUID { get; set; }
        public string FillID { get; set; }
        public DateTime FillDttm { get; set; }
        public int? FillByUserUID { get; set; }
        public string FillByUser { get; set; }
        public int? FillForDays { get; set; }
        public DateTime? ExcludePriorHour { get; set; }
        public int? WardUID { get; set; }
        public string WardName { get; set; }
        public int? StoreUID { get; set; }
        public string StoreName { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string OwnerOrganisationName { get; set; }
        public string Comment { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
        public int FillForDay { get; set; }
        public List<PatientOrderStandingModel> StandingModels { get; set; }
    }
}
