using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RequestListModel : INotifyPropertyChanged
    {
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long RequestUID { get; set; }
        public long RequestDetailUID { get; set; }
        public long ResultUID { get; set; }
        public string PatientID { get; set; }
        public string VisitID { get; set; }
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public int SEXXXUID { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateString
        {
            get
            {
                return BirthDate != null ? BirthDate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public string PatientAge { get; set; }
        public string PatientAddress { get; set; }
        public string RequestNumber { get; set; }
        public string AccessionNumber { get; set; }
        public System.Nullable<DateTime> RequestedDttm { get; set; }
        public string RequestedDttmString
        {
            get
            {
                return RequestedDttm != null ? RequestedDttm.Value.ToString("dd/MM/yyyy HH:mm") : "";
            }
        }
        public System.Nullable<DateTime> ArriveTime { get; set; }
        public string ArriveTimeString
        {
            get
            {
                return ArriveTime != null ? ArriveTime.Value.ToString("dd/MM/yyyy HH:mm") : "";
            }
        }
        public System.Nullable<DateTime> PreparedDttm { get; set; }
        public string PreparedDttmString
        {
            get
            {
                return PreparedDttm != null ? PreparedDttm.Value.ToString("dd/MM/yyyy HH:mm") : "";
            }
        }
        public string ProcessingNote { get; set; }
        public string RDUNote { get; set; }
        public int? RABSTSUID { get; set; }
        public System.Nullable<DateTime> ResultedDttm { get; set; }
        public string OrderStatus { get; set; }
        public string PriorityStatus { get; set; }
        public string RDUResultStatus { get; set; }
        public string ResultStatus { get; set; }
        public string RequestUserName { get; set; }
        public string RequestItemCode { get; set; }
        public string RequestItemName { get; set; }
        public int? PRTGPUID { get; set; }
        public string PrintGroup { get; set; }
        public string Modality { get; set; }
        public System.Nullable<int> RadiologistUID { get; set; }
        public System.Nullable<int> ExecuteByUID { get; set; }
        public System.Nullable<int> RefNo { get; set; }
        public string DoctorName { get; set; }
        public string ExecuteBy { get; set; }
        public string LocationName { get; set; }
        public string OrganisationName { get; set; }
        public string PayorName { get; set; }
        public string Comments { get; set; }
        public string AssignedBy { get; set; }
        public int? AssignedByUID { get; set; }
        public DateTime? AssignedDttm { get; set; }
        public string AssignedTo { get; set; }
        public int? AssignedToUID { get; set; }
        public DateTime? RDUResultedDttm { get; set; }
        public string RDUStaff { get; set; }
        public int? RDUStaffUID { get; set; }

        private bool _IsSelected;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;
                OnPropertyRaised("IsSelected");
            }
        }

        public int RowHandle { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyRaised(string propertyname)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
            }
        }
    }
}
