using GalaSoft.MvvmLight.Command;
using System;
using MediTech.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediTech.Views;

namespace MediTech.ViewModels
{
    public class ManageLocationViewModel : MediTechViewModelBase
    {
        #region Properties
        LocationModel model;

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        private string _PhoneNo;
        public string PhoneNo
        {
            get { return _PhoneNo; }
            set { Set(ref _PhoneNo, value); }
        }

        private List<LookupReferenceValueModel> _LocationType;
        public List<LookupReferenceValueModel> LocationType
        {
            get { return _LocationType; }
            set { Set(ref _LocationType, value); }
        }

        private LookupReferenceValueModel _SelectLocationType;
        public LookupReferenceValueModel SelectLocationType
        {
            get { return _SelectLocationType; }
            set { Set(ref _SelectLocationType, value); }
        }

        private List<HealthOrganisationModel> _HealthOrganisation;
        public List<HealthOrganisationModel> HealthOrganisation
        {
            get { return _HealthOrganisation; }
            set { Set(ref _HealthOrganisation, value); }
        }

        private HealthOrganisationModel _SelectHealthOrganisation;
        public HealthOrganisationModel SelectHealthOrganisation
        {
            get { return _SelectHealthOrganisation; }
            set { Set(ref _SelectHealthOrganisation, value); }
        }

        private List<LocationModel> _ParentLocation;
        public List<LocationModel> ParentLocation
        {
            get { return _ParentLocation; }
            set { Set(ref _ParentLocation, value); }
        }

        private LocationModel _SelectParentLocationn;
        public LocationModel SelectParentLocationn
        {
            get { return _SelectParentLocationn; }
            set { Set(ref _SelectParentLocationn, value); }
        }

        private DateTime? _ActiveFrom;
        public DateTime? ActiveFrom
        {
            get { return _ActiveFrom; }
            set { Set(ref _ActiveFrom, value); }
        }

        private DateTime? _ActiveTo;
        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }

        private string _DisplayOrder;
        public string DisplayOrder
        {
            get { return _DisplayOrder; }
            set { Set(ref _DisplayOrder, value); }
        }

        private List<LookupReferenceValueModel> _EmergencyZone;
        public List<LookupReferenceValueModel> EmergencyZone
        {
            get { return _EmergencyZone; }
            set { Set(ref _EmergencyZone, value); }
        }

        private LookupReferenceValueModel _SelectEmergencyZonee;
        public LookupReferenceValueModel SelectEmergencyZone
        {
            get { return _SelectEmergencyZonee; }
            set { Set(ref _SelectEmergencyZonee, value); }
        }

        private bool _IsTemporaryBed;
        public bool IsTemporaryBed
        {
            get { return _IsTemporaryBed; }
            set { Set(ref _IsTemporaryBed, value); }
        }

        private bool _IsRegisterAllow;
        public bool IsRegisterAllow
        {
            get { return _IsRegisterAllow; }
            set { Set(ref _IsRegisterAllow, value); }
        }

        private bool _IsCanOrder;
        public bool IsCanOrder
        {
            get { return _IsCanOrder; }
            set { Set(ref _IsCanOrder, value); }
        }

        private bool _IsEmergency;
        public bool IsEmergency
        {
            get { return _IsEmergency; }
            set { Set(ref _IsEmergency, value); }
        }


        #endregion

        #region Command

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(SaveLocation)); }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }
        #endregion
        public ManageLocationViewModel()
        {
            LocationType = DataService.Technical.GetReferenceValueList("LOTYP");
            EmergencyZone = DataService.Technical.GetReferenceValueList("EMGCD");
            HealthOrganisation = DataService.MasterData.GetHealthOrganisation();
            ParentLocation = DataService.Technical.GetLocation();
        }

        #region Method
        private void SaveLocation()
        {
            try
            {
                if (String.IsNullOrEmpty(Name))
                {
                    WarningDialog("กรุณาใส่ชื่อ");
                    return;
                }
                if (string.IsNullOrEmpty(Description))
                {
                    WarningDialog("กรุณาใส่รายละเอียด");
                    return;
                }

                if (SelectLocationType == null)
                {
                    WarningDialog("กรุณาเลือก LocationType");
                    return;
                }
                if (!String.IsNullOrEmpty(DisplayOrder))
                {
                    var isNum = IsDigitsOnly(DisplayOrder);

                    if (isNum == false)
                    {
                        WarningDialog("กรุณาใส่ DisplayOrder เป็นตัวเลขเท่านั้น");
                        return;
                    }
                }

                AssingPropertiesToModel();
                DataService.Technical.ManageLocation(model, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListLocation pageList = new ListLocation();
                ChangeViewPermission(pageList);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        public void AssignModelData(LocationModel modelData)
        {
            model = modelData;
            AssingModelToProperties();
        }
        public void AssingModelToProperties()
        {
            Name = model.Name;
            Description = model.Description;
            PhoneNo = model.PhoneNumber;
            ActiveFrom = model.ActiveFrom;
            ActiveTo = model.ActiveTo;
            SelectLocationType = LocationType.Find(p => p.Key == model.LOTYPUID);
            IsCanOrder = model.IsCanOrder  == "Y" ? true : false; 
            IsEmergency = model.IsEmergency == "Y" ? true : false;
            IsRegisterAllow = model.IsRegistrationAllowed == "Y" ? true : false;
            IsTemporaryBed = model.IsTemporaryBed == "Y" ? true : false;
            if (model.EMRZONUID != null)
            {
                SelectEmergencyZone = EmergencyZone.Find(p => p.Key == model.EMRZONUID);
            }
            DisplayOrder = model.DisplayOrder.ToString();
            if(model.ParentLocationUID != null)
            {
                SelectParentLocationn = ParentLocation.Find(p => p.LocationUID == model.ParentLocationUID);
            }
            if (model.OwnerOrganisationUID != null)
            {
                SelectHealthOrganisation = HealthOrganisation.Find(p => p.HealthOrganisationUID == model.OwnerOrganisationUID);
            }
        }

        public void AssingPropertiesToModel()
        {
            if (model == null)
            {
                model = new LocationModel();
            }
            model.Name = Name;
            model.Description = Description;
            model.PhoneNumber = PhoneNo;
            model.ActiveFrom = ActiveFrom;
            model.ActiveTo = ActiveTo;
            model.LOTYPUID = SelectLocationType.Key ?? 0;
           
            model.IsCanOrder = IsCanOrder == true ?"Y" : "N";
            model.IsEmergency = IsEmergency == true ? "Y" : "N";
            model.IsRegistrationAllowed = IsRegisterAllow == true ? "Y" : "N";
            model.IsTemporaryBed = IsTemporaryBed == true ? "Y" : "N";
            
            model.EMRZONUID = SelectEmergencyZone != null ? SelectEmergencyZone.Key : (int?)null;
            model.OwnerOrganisationUID = SelectHealthOrganisation!= null ? SelectHealthOrganisation.HealthOrganisationUID :(int ?)null;
            model.ParentLocationUID = SelectParentLocationn != null ? SelectParentLocationn.LocationUID : (int?)null;

            if (!String.IsNullOrEmpty(DisplayOrder))
            {
                int val = 0;
                Int32.TryParse(DisplayOrder, out val);
                model.DisplayOrder = val;
            }
            else
            {
                model.DisplayOrder = null;
            }
        } 

        private void Cancel()
        {
            ListLocation pageListLocation = new ListLocation();
            ChangeViewPermission(pageListLocation);
        }

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }
        #endregion
    }
}
