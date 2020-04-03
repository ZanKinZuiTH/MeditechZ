using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientRequestReportModel
    {
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AgeString { get; set; }
        public string Gender { get; set; }
        public Nullable<int> SEXXXUID { get; set; }
        public System.Nullable<DateTime> BirthDttm { get; set; }
        public string PatientID { get; set; }
        public DateTime RequestedDttm { get; set; }
        public string RequestItemName { get; set; }

        public string PreparedBy { get; set; }
        public System.Nullable<DateTime> PreparedDttm { get; set; }
        public string ProcessingNote { get; set; }
        public string RDUStaff { get; set; }
        public string RDUNote { get; set; }
        public System.Nullable<int> RIMTYPUID { get; set; }
        public string ImageType { get; set; }
        public string Modality { get; set; }
        public string AccessionNumber { get; set; }
        public string RequestNumber { get; set; }
        public long RequestUID { get; set; }
        public long RequestDetailUID { get; set; }
        public ResultRadiologyModel Result { get; set; }
        public List<Invenstigation> Invenstigation { get; set; }
        public List<PreviousResult> PreviousResult { get; set; }
        public string Comments { get; set; }
        public List<ResultRadiologyHistoryModel> ResultHistory { get; set; }
    }

    public class Invenstigation
    {
        public long RequestDetailUID { get; set; }
        public DateTime RequestedDttm { get; set; }
        public string AccessionNumber { get; set; }
        public string TestName { get; set; }
        public string Radiologist { get; set; }

    }

    public class PreviousResult
    {
        public System.Nullable<DateTime> RequestedDttm { get; set; }
        public System.Nullable<DateTime> ResultEnteredDttm { get; set; }
        public string TestName { get; set; }
        public string Modality { get; set; }
        public long ResultUID { get; set; }
        public string ResultStatus { get; set; }
        public string AccessionNumber { get; set; }
    }
}
