using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientVisitModel : PatientInformationModel, INotifyPropertyChanged
    {
        public long PatientVisitUID { get; set; }
        public Nullable<int> CareProviderUID { get; set; }
        public string CareProviderName { get; set; }
        public Nullable<int> ENTYPUID { get; set; }
        public Nullable<int> ENSTAUID { get; set; }

        private Nullable<int> _VISTSUID;

        public Nullable<int> VISTSUID
        {
            get { return _VISTSUID; }
            set { _VISTSUID = value; OnPropertyRaised("VISTSUID"); }
        }
        public Nullable<int> CheckupJobUID { get; set; }
        public string CompanyName { get; set; }
        public string EmployerAddress { get; set; }
        public string Program { get; set; }
        public string VisitType { get; set; }

        private string _VisitStatus;

        public string VisitStatus
        {
            get { return _VisitStatus; }
            set { _VisitStatus = value; OnPropertyRaised("VisitStatus"); }
        }

        public Nullable<int> VISTYUID { get; set; }
        public Nullable<int> PRITYUID { get; set; }
        public Nullable<int> SpecialityUID { get; set; }
        public Nullable<System.DateTime> StartDttm { get; set; }
        public Nullable<System.DateTime> EndDttm { get; set; }
        public Nullable<System.DateTime> ArrivedDttm { get; set; }
        public Nullable<System.DateTime> DischargeDttm { get; set; }
        public Nullable<System.DateTime> AdmisstionDttm { get; set; }
        public string DischargedUser { get; set; }
        public String LocationName { get; set; }
        public String EncounterType { get; set; }

        public String PrimaryDiagnosis { get; set; }
        public string VisitID { get; set; }
        public string IsBillFinalized { get; set; }
        public string Comments { get; set; }
        public int? RefNo { get; set; }
        public int? LocationUID { get; set; }
        public int? BookingUID { get; set; }
        public int? BedUID { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string CreateBy { get; set; }
        public string StatusFlag { get; set; }
        public string OwnerOrganisation { get; set; }
        public long PatientVisitPayorUID { get; set; }
        public int PayorDetailUID { get; set; }
        public string PayorName { get; set; }
        public string PolicyName { get; set; }
        public string PayorAgreementName { get; set; }
        public double? EligibileAmount { get; set; }
        public int PayorAgreementUID { get; set; }
        public string WellnessResult { get; set; }
        public bool IsWellnessResult { get; set; }
        public bool IsDataInconsistency { get; set; }
        public int WellnessResultUID { get; set; }
        public string OnBLIFE { get; set; }
        public int RowHandle { get; set; }
        public long RowNumber { get; set; }
        public bool Select { get; set; }
        public string IsReAdmisstion { get; set; }
        public string IsAllocated { get; set; }
        public long PatientBillUID { get; set; }
        public List<PatientVisitPayorModel> PatientVisitPayors { get; set; }
        public PatientVisitPayorModel PatientVisitPayorsAdmit { get; set; }
        public PatientAEAdmissionModel AEAdmission { get; set; }
        public long? AEAdmissionUID { get; set; }
        public long? AdmissionEventUID { get; set; }
        public string BedName { get; set; }
        public List<CareproviderModel> SecondCareprovider { get; set; }
        public AdmissionEventModel AdmissionEvent { get; set; }
        public string ICD10 { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
