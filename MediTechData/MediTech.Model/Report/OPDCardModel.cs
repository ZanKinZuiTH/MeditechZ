using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model.Report
{
    public class OPDCardModel
    {
        public string PatientName { get; set; }
        public string PatientID { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string PatientIDCard { get; set; }
        public string VisitID { get; set; }
        public DateTime VisitDttm { get; set; }
        public string PatientAddress { get; set; }
        public string MobilePhone { get; set; }
        public string PatientAllergy { get; set; }
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string BP { get; set; }
        public string Temprature { get; set; }
        public string Pulse { get; set; }
        public string RespiratoryRate { get; set; }
        public string OxygenSat { get; set; }
        public string PI { get; set; }
        public string ChiefComplaint { get; set; }
        public string PhysicalExam { get; set; }
        public string CareproviderName { get; set; }
        public List<PatientProblemModel> PateintProblem { get; set; }
        public List<PatientOrderDetailModel> PatientDrugDetail { get; set; }

        public string OrganisationCode { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationAddress { get; set; }
    }
}
