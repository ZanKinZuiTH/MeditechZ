using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.Model
{
    public class RequestDetailSpecimenModel : RequestDetailLabModel, INotifyPropertyChanged
    {
        public long? RequestDetailSpecimenUID { get; set; }
        private int? _SpecimenUID;

        public int? SpecimenUID
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

        private string _SpecimenCode;

        public string SpecimenCode
        {
            get { return _SpecimenCode; }
            set { _SpecimenCode = value; OnPropertyRaised("SpecimenCode"); }
        }

        private int _SPMTPUID;

        public int SPMTPUID
        {
            get { return _SPMTPUID; }
            set { _SPMTPUID = value; OnPropertyRaised("SPMTPUID"); }
        }

        private string _SpecimenType;

        public string SpecimenType
        {
            get { return _SpecimenType; }
            set { _SpecimenType = value; OnPropertyRaised("SpecimenType"); }
        }

        public int ContainerUID { get; set; }

        private Nullable<double> _VolumeCollected;

        public Nullable<double> VolumeCollected
        {
            get { return _VolumeCollected; }
            set { _VolumeCollected = value; OnPropertyRaised("VolumeCollected"); }
        }

        private Nullable<int> _VOUNTUID;

        public Nullable<int> VOUNTUID
        {
            get { return _VOUNTUID; }
            set { _VOUNTUID = value; OnPropertyRaised("VOUNTUID"); }
        }

        private string _VolumnUnit;

        public string VolumnUnit
        {
            get { return _VolumnUnit; }
            set { _VolumnUnit = value; OnPropertyRaised("VolumnUnit"); }
        }

        public Nullable<System.DateTime> CollectionDttm { get; set; }

        private Nullable<int> _COLSTUID;

        public Nullable<int> COLSTUID
        {
            get { return _COLSTUID; }
            set { _COLSTUID = value; OnPropertyRaised("COLSTUID"); }
        }

        private string _CollectionSite;

        public string CollectionSite
        {
            get { return _CollectionSite; }
            set { _CollectionSite = value; OnPropertyRaised("CollectionSite"); }
        }
        private Nullable<int> _CLROUUID;

        public Nullable<int> CLROUUID
        {
            get { return _CLROUUID; }
            set { _CLROUUID = value; OnPropertyRaised("CLROUUID"); }
        }

        private string _CollectionRoute;

        public string CollectionRoute
        {
            get { return _CollectionRoute; }
            set { _CollectionRoute = value; OnPropertyRaised("CollectionRoute"); }
        }

        private Nullable<int> _COLMDUID;

        public Nullable<int> COLMDUID
        {
            get { return _COLMDUID; }
            set { _COLMDUID = value; OnPropertyRaised("COLMDUID"); }
        }

        private string _CollectionMethod;

        public string CollectionMethod
        {
            get { return _CollectionMethod; }
            set { _CollectionMethod = value; OnPropertyRaised("CollectionMethod"); }
        }

        public Nullable<int> CollectedBy { get; set; }
        public string CollectedByName { get; set; }

        private string _StorageInstuction;

        public string StorageInstuction
        {
            get { return _StorageInstuction; }
            set { _StorageInstuction = value; OnPropertyRaised("StorageInstuction"); }
        }

        private string _HandlingInstruction;

        public string HandlingInstruction
        {
            get { return _HandlingInstruction; }
            set { _HandlingInstruction = value; OnPropertyRaised("HandlingInstruction"); }
        }

        private Nullable<int> _SPSTSUID;

        public Nullable<int> SPSTSUID
        {
            get { return _SPSTSUID; }
            set { _SPSTSUID = value; OnPropertyRaised("SPSTSUID"); }
        }

        private string _SpecimenStatus;

        public string SpecimenStatus
        {
            get { return _SpecimenStatus; }
            set { _SpecimenStatus = value; OnPropertyRaised("SpecimenStatus"); }
        }

        private string _Suffix;

        public string Suffix
        {
            get { return _Suffix; }
            set { _Suffix = value; OnPropertyRaised("Suffix"); }
        }

        public string ReviewComments { get; set; }
        public string SpecimenNumber { get; set; }
        public Nullable<int> ReviewedBy { get; set; }

        public string ReviewedByName { get; set; }
        public int CUser { get; set; }
        public System.DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public System.DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }

        private List<RequestItemSpecimenModel> _RequestItemSpecimens;

        public List<RequestItemSpecimenModel> RequestItemSpecimens
        {
            get { return _RequestItemSpecimens; }
            set { _RequestItemSpecimens = value; OnPropertyRaised("RequestItemSpecimens"); }
        }

        private RequestItemSpecimenModel _SelectRequestItemSpecimen;

        public RequestItemSpecimenModel SelectRequestItemSpecimen
        {
            get { return _SelectRequestItemSpecimen; }
            set
            {
                _SelectRequestItemSpecimen = value;
                OnPropertyRaised("SelectRequestItemSpecimen");
                if (SelectRequestItemSpecimen != null)
                {
                    SpecimenUID = SelectRequestItemSpecimen.SpecimenUID;
                    SpecimenName = SelectRequestItemSpecimen.SpecimenName;
                    VolumeCollected = SelectRequestItemSpecimen.VolumeCollected;
                    VolumnUnit = SelectRequestItemSpecimen.VolumeUnit;
                    SpecimenType = SelectRequestItemSpecimen.SpecimenType;
                    CollectionRoute = SelectRequestItemSpecimen.CollectionRoute;
                    CollectionMethod = SelectRequestItemSpecimen.CollectionMethod;
                    CollectionSite = SelectRequestItemSpecimen.CollectionSite;
                    StorageInstuction = SelectRequestItemSpecimen.StorageInstuction;
                    HandlingInstruction = SelectRequestItemSpecimen.HandlingInstruction;
                    Suffix = SelectRequestItemSpecimen.Suffix;
                }
            }
        }

        private bool _Selected;

        public bool Selected
        {
            get { return _Selected; }
            set { _Selected = value; OnPropertyRaised("Selected"); }
        }

        private bool _EnableSelect;

        public bool EnableSelect
        {
            get { return _EnableSelect; }
            set { _EnableSelect = value; OnPropertyRaised("EnableSelect"); }
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
