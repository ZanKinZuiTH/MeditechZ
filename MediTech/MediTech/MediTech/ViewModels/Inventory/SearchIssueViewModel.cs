using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public  class SearchIssueViewModel : MediTechViewModelBase
    {
        #region Properties
        public List<LocationModel> LocationFormData { get; set; }
        public List<LocationModel> LocationToData { get; set; }

        private bool _IsEnableEdit = false;

        public bool IsEnableEdit
        {
            get { return _IsEnableEdit; }
            set { Set(ref _IsEnableEdit, value); }
        }


        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string IssueNo { get; set; }

        public List<LookupReferenceValueModel> IssueStatus { get; set; }

        public List<HealthOrganisationModel> OrganisationsFrom { get; set; }
        private HealthOrganisationModel _SelectOrganisationFrom;

        public HealthOrganisationModel SelectOrganisationFrom
        {
            get { return _SelectOrganisationFrom; }
            set
            {
                Set(ref _SelectOrganisationFrom, value);
                if (_SelectOrganisationFrom != null)
                {
                    LocationFormData = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisationFrom.HealthOrganisationUID);

                    LocationFrom = LocationFormData;

                    if (SelectLocationTo != null)
                        LocationFrom = LocationFormData.Where(p => p.LocationUID != SelectLocationTo.LocationUID).ToList();
                }
            }
        }

        private List<LocationModel> _LocationFrom;

        public List<LocationModel> LocationFrom
        {
            get { return _LocationFrom; }
            set { Set(ref _LocationFrom, value); }
        }

        private LocationModel _SelectLocationFrom;

        public LocationModel SelectLocationFrom
        {
            get { return _SelectLocationFrom; }
            set
            {
                Set(ref _SelectLocationFrom, value);
                if (_SelectLocationFrom != null)
                {
                    LocationTo = LocationToData.Where(p => p.LocationUID != SelectLocationFrom.LocationUID).ToList();
                }
            }
        }

        public List<HealthOrganisationModel> OrganisationsTo { get; set; }
        private HealthOrganisationModel _SelectOrganisationTo;

        public HealthOrganisationModel SelectOrganisationTo
        {
            get { return _SelectOrganisationTo; }
            set
            {
                Set(ref _SelectOrganisationTo, value);
                if (_SelectOrganisationTo != null)
                {
                    LocationToData = DataService.MasterData.GetLocationByOrganisationUID(_SelectOrganisationTo.HealthOrganisationUID);
                    LocationTo = LocationToData;
                    if (SelectLocationFrom != null)
                        LocationTo = LocationToData.Where(p => p.LocationUID != SelectLocationFrom.LocationUID).ToList();
                }
            }
        }

        private List<LocationModel> _LocationTo;

        public List<LocationModel> LocationTo
        {
            get { return _LocationTo; }
            set { Set(ref _LocationTo, value); }
        }

        private LocationModel _SelectLocationTo;

        public LocationModel SelectLocationTo
        {
            get { return _SelectLocationTo; }
            set
            {
                Set(ref _SelectLocationTo, value);
                if (_SelectLocationTo != null)
                {
                    LocationFrom = LocationFormData.Where(p => p.LocationUID != SelectLocationTo.LocationUID).ToList();
                }
            }
        }

        private List<ItemIssueModel> _ItemIssues;

        public List<ItemIssueModel> ItemIssues
        {
            get { return _ItemIssues; }
            set { Set(ref _ItemIssues, value); }
        }

        private ItemIssueModel _SelectItemIssues;

        public ItemIssueModel SelectItemIssues
        {
            get { return _SelectItemIssues; }
            set
            {
                Set(ref _SelectItemIssues, value);
                if (_SelectItemIssues != null)
                {
                    ItemIssueDetails = DataService.Inventory.GetItemIssueDetailByItemIssueUID(SelectItemIssues.ItemIssueUID);
                    if (_SelectItemIssues.ISUSTUID == 2914 || _SelectItemIssues.ISUSTUID == 2916)
                    {
                        IsEnableEdit = false;
                    }
                    else
                    {
                        IsEnableEdit = true;
                    }
                }
            }
        }

        private List<ItemIssueDetailModel> _ItemIssueDetails;

        public List<ItemIssueDetailModel> ItemIssueDetails
        {
            get { return _ItemIssueDetails; }
            set { Set(ref _ItemIssueDetails, value); }
        }

        private ItemIssueDetailModel _SelectItemIssueDetail;

        public ItemIssueDetailModel SelectItemIssueDetail
        {
            get { return _SelectItemIssueDetail; }
            set { Set(ref _SelectItemIssueDetail, value); }
        }

        private LookupReferenceValueModel _SelectIssueStatus;

        public LookupReferenceValueModel SelectIssueStatus
        {
            get { return _SelectIssueStatus; }
            set { Set(ref _SelectIssueStatus, value); }
        }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }

        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get
            {
                return _ClearCommand
                    ?? (_ClearCommand = new RelayCommand(Clear));
            }

        }


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
            }

        }


        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Canecl));
            }

        }



        #endregion

        #region Method

        public SearchIssueViewModel()
        {
            var Organis = GetHealthOrganisationIsStock();
            IssueStatus = DataService.Technical.GetReferenceValueMany("ISUST");
            OrganisationsFrom = Organis;
            OrganisationsTo = GetHealthOrganisationIsRoleStock();

            if (IssueStatus != null)
            {
                SelectIssueStatus = IssueStatus.FirstOrDefault(p => p.ValueCode == "ISSED");
            }
            //DateFrom = DateTime.Now;
        }

        public override void OnLoaded()
        {
            Search();
        }
        private void Save()
        {
            CloseViewDialog(ActionDialog.Save);
        }

        private void Canecl()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        private void Search()
        {
            ItemIssues = null;
            ItemIssueDetails = null;
            int? organisationUIDFrom = SelectOrganisationFrom != null ? SelectOrganisationFrom.HealthOrganisationUID : (int?)null;
            int? locationFromUID = SelectLocationFrom != null ? SelectLocationFrom.LocationUID : (int?)null;
            int? organisationUIDTo = SelectOrganisationTo != null ? SelectOrganisationTo.HealthOrganisationUID : (int?)null;
            int? locationToUID = SelectLocationTo != null ? SelectLocationTo.LocationUID : (int?)null;
            int? issueStatus = SelectIssueStatus != null ? SelectIssueStatus.Key : (int?)null;
            ItemIssues = DataService.Inventory.SearchItemIssue(DateFrom, DateTo, IssueNo, 2917, issueStatus, organisationUIDFrom, locationFromUID, organisationUIDTo, locationToUID);
        }


        private void Clear()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SelectOrganisationFrom = null;
            SelectOrganisationTo = null;
            IssueNo = string.Empty ;
        }


        #endregion
    }
}
