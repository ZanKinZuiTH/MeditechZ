using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RequestItemSpecimenModel : INotifyPropertyChanged
    {
        public int RequestItemSpecimenUID { get; set; }
        private int _SpecimenUID;

        public int SpecimenUID
        {
            get { return _SpecimenUID; }
            set { _SpecimenUID = value; OnPropertyRaised("SpecimenUID"); }
        }

        private string _SpecimenName;

        public string SpecimenName
        {
            get { return _SpecimenName; }
            set { _SpecimenName = value; OnPropertyRaised("SpecimenName"); }
        }
        public int RequestItemUID { get; set; }
        public string SpecimenType { get; set; }
        public double? VolumeCollected { get; set; }
        public string VolumeUnit { get; set; }
        public string CollectionSite { get; set; }

        public string CollectionRoute { get; set; }
        public string CollectionMethod { get; set; }
        public string IsDFT { get; set; }
        public string CollectionInterval { get; set; }
        public string StorageInstuction { get; set; }
        public string HandlingInstruction { get; set; }
        public string IsDefault { get; set; }

        public string Suffix { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

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
