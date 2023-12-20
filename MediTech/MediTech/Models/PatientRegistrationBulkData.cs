using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Models
{
    public class PatientRegistrationBulkData : ObservableObject
    {
        public bool HasVisitToday { get; set; }

        private string _RowColor;

        public string RowColor
        {
            get { return _RowColor; }
            set { Set(ref _RowColor, value); }
        }

        public bool IsJustRegistered { get; set; }
        public string IsInComplete { get; set; }
        public string IsDuplicate { get; set; }
        public string IsNationalIDDuplicate { get; set; }
        public string AlreadyExists { get; set; }
        private string _BN;

        public string BN
        {
            get { return _BN; }
            set { Set(ref _BN, value); }
        }

        private string _VisitID;

        public string VisitID
        {
            get { return _VisitID; }
            set { Set(ref _VisitID, value); }
        }


        public string PatientOtherID { get; set; }
        public string DateOfBirth { get; set; }
        public DateTime? BirthDttm { get; set; }
        public bool DOBComputed { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public int? SEXXXUID { get; set; }
        public int? NATNLUID { get; set; }
        public string Nationality { get; set; } 
        public int? TITLEUID { get; set; }
        public string LastName { get; set; }
        public string PreName { get; set; }
        public string FirstName { get; set; }
        public bool Register { get; set; }
        public string Registered { get; set; }
        public long PatientUID { get; set; }
        public string EmployeeID { get; set; }
        public DateTime RegistrationDTTM { get; set; }
        public string IDCard { get; set; }
        public string IDPassport { get; set; }
        public string Company { get; set; }
        public string MobilePhone { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }
        public string Group { get; set; }
        public string Program { get; set; }
        public DateTime? CheckupDttm { get; set; }
        public int No { get; set; }
        public string EmployerAddress { get; set; } 
    }
}
