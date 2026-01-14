using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class PatientVitalSignModel
    {
        public int PatientVitalSignUID { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<double> Height { get; set; }
        public Nullable<double> RespiratoryRate { get; set; }
        public Nullable<double> Temprature { get; set; }
        public Nullable<double> Pulse { get; set; }
        public Nullable<System.DateTime> RecordedDttm { get; set; }
        public string RecordedBy { get; set; }
        public Nullable<double> BMIValue { get; set; }
        public Nullable<double> BSAValue { get; set; }
        public Nullable<double> BPSys { get; set; }
        public Nullable<double> BPDio { get; set; }
        public Nullable<double> OxygenSat { get; set; }
        public Nullable<double> WaistCircumference { get; set; }
        public Nullable<double> HipCircumference { get; set; }
        public Nullable<double> WHRValue { get; set; }
        // Mutually exclusive flags:
        // - If IsPregnant becomes true => force IsSuspectedPregnant to false
        // - If IsSuspectedPregnant becomes true => force IsPregnant to false
        private bool? _isPregnant;
        public bool? IsPregnant
        {
            get { return _isPregnant; }
            set
            {
                _isPregnant = value;
                if ((_isPregnant ?? false) == true)
                {
                    _isSuspectedPregnant = false;
                }
            }
        }

        private bool? _isSuspectedPregnant;
        public bool? IsSuspectedPregnant
        {
            get { return _isSuspectedPregnant; }
            set
            {
                _isSuspectedPregnant = value;
                if ((_isSuspectedPregnant ?? false) == true)
                {
                    _isPregnant = false;
                }
            }
        }
        public string Comments { get; set; }
        public string Skin { get; set; }
        public string MentalHealth { get; set; }
        public System.DateTime CWhen { get; set; }
        public int CUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public int MUser { get; set; }
        public string StatusFlag { get; set; }
    }
}
