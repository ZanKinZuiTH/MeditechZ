using System;
using System.Collections.Generic;

namespace MediTech.Model.Report
{

    public class PatientBookingModel
    {

        public Nullable<int> CareProviderUID { get; set; }
        public string Comments { get; set; }
        public string Gender { get; set; }
        public long No { get; set; }
        public string PatientID { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string DOB { get; set; }
        public long PatientVisitUID { get; set; }
        public string VisitID { get; set; }
        public DateTime VisitDate { get; set; }
        public string Doctor { get; set; }
        public string CUserAppointment { get; set; }
        public int BKSTSUID { get; set; }
        public Nullable<int> PATMSGUID { get; set; }
        public string PatientReminderMessage { get; set; }
        public System.DateTime AppointmentDttm { get; set; }
        public System.DateTime CWhenAppointmentDttm { get; set; }
        public string DrugAllergy { get; set; }
        public List<DetailBookingModel> DetailBook{get;set;}
        public string strVisitData { get; set; }

    }
    public class DetailBookingModel
    {
        public string Detail { get; set; }
    }



}