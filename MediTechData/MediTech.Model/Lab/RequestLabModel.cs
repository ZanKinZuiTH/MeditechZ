using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RequestLabModel : INotifyPropertyChanged
    {
        public int No { get; set; }
        public long PatientUID { get; set; }
        public long PatientVisitUID { get; set; }
        public long RequestUID { get; set; }
        public string LabNumber { get; set; }
        public string PriorityStatus { get; set; }
        public string PatientID { get; set; }
        public string FirstName { get; set; }
        public string PreName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PatientName { get; set; }
        public string VisitID { get; set; }
        public string PatientAge { get; set; }
        public string Gender { get; set; }
        public int SEXXXUID { get; set; }
        public double? Height { get; set; }
        public string PatientAddress { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateString
        {
            get
            {
                return BirthDate != null ? BirthDate.Value.ToString("dd/MM/yyyy") : "";
            }
        }
        public DateTime RequestedDttm { get; set; }
        public DateTime ArrivedDttm { get; set; }
        public string PayorName { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string OrganisationName { get; set; }
        public string OrderStatus { get; set; }
        public string Comments { get; set; }
        public string ReviewedBy { get; set; }
        public int ORDSTUID { get; set; }
        public int VISTSUID { get; set; }
        public string VisitStatus { get; set; }
        public List<RequestDetailItemModel> RequestDetailLabs { get; set; }


        private bool _Selected;

        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyRaised("Selected"); }
        }

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
