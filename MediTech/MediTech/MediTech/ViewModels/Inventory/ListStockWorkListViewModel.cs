using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ListStockWorkListViewModel : MediTechViewModelBase
    {

        #region Properties

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }


        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }



        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if(SelectOrganisation != null)
                {
                    Locations = DataService.MasterData.GetLocationByOrganisationUID(_SelectOrganisation.HealthOrganisationUID);
                }
            }
        }

        private List<LocationModel> _Locations;

        public List<LocationModel> Locations
        {
            get { return _Locations; }
            set { Set(ref _Locations, value); }
        }

        private LocationModel _SelectLocation;

        public LocationModel SelectLocation
        {
            get { return _SelectLocation; }
            set
            {
                Set(ref _SelectLocation, value);
            }
        }

        private List<StockWorkListModel> _StockWorkLists;

        public List<StockWorkListModel> StockWorkLists
        {
            get { return _StockWorkLists; }
            set { Set(ref _StockWorkLists, value); }
        }

        private StockWorkListModel _SelectStockWorkList;

        public StockWorkListModel SelectStockWorkList
        {
            get { return _SelectStockWorkList; }
            set
            {
                Set(ref _SelectStockWorkList, value);
                if (SelectStockWorkList != null)
                {
                    IsEnabledIssue = true;
                    IsEnabledReceive = true;
                    IsEnabledTranfer = true;
                    if (SelectStockWorkList.StatusUID != 2926)
                    {
                        IsEnabledIssue = false;
                        IsEnabledTranfer = false;
                    }


                    if (SelectStockWorkList.StatusUID != 2913)
                    {
                        IsEnabledReceive = false;
                    }
                }



            }
        }


        private bool _IsEnabledIssue;

        public bool IsEnabledIssue
        {
            get { return _IsEnabledIssue; }
            set { Set(ref _IsEnabledIssue, value); }
        }

        private bool _IsEnabledTranfer;

        public bool IsEnabledTranfer
        {
            get { return _IsEnabledTranfer; }
            set { Set(ref _IsEnabledTranfer, value); }
        }

        private bool _IsEnabledReceive;

        public bool IsEnabledReceive
        {
            get { return _IsEnabledReceive; }
            set { Set(ref _IsEnabledReceive, value); }
        }
        #endregion

        #region Command

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _ClearCommand;
        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }

        private RelayCommand _IssueCommand;
        public RelayCommand IssueCommand
        {
            get { return _IssueCommand ?? (_IssueCommand = new RelayCommand(Issue)); }
        }


        private RelayCommand _TranferCommand;
        public RelayCommand TranferCommand
        {
            get { return _TranferCommand ?? (_TranferCommand = new RelayCommand(Tranfer)); }
        }

        private RelayCommand _ReceiveCommand;
        public RelayCommand ReceiveCommand
        {
            get { return _ReceiveCommand ?? (_ReceiveCommand = new RelayCommand(Receive)); }
        }



        #endregion

        #region Method

        public ListStockWorkListViewModel()
        {
            DateFrom = DateTime.Now.Date;
            Organisations = GetHealthOrganisationIsRoleStock();

            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }

        public override void OnLoaded()
        {
            Search();
        }
        private void Clear()
        {
            DateFrom = DateTime.Now.Date;
            DateTo = null;
            if (Organisations != null)
            {
                SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            }
        }

        private void Search()
        {
            StockWorkLists = DataService.Inventory.SearchStockWorkList(DateFrom, DateTo, SelectOrganisation != null ? SelectOrganisation.HealthOrganisationUID : (int?)null
                ,SelectLocation != null ? SelectLocation.LocationUID  : (int?)null
                );
        }
        private void Issue()
        {

            ManageItemIssue view = new ManageItemIssue();
            (view.DataContext as ManageItemIssueViewModel).AssingProperitesFromRequest(SelectStockWorkList.DocumentUID);
            ChangeViewPermission(view, this.View);
        }
        private void Tranfer()
        {
            ManageItemTransfer view = new ManageItemTransfer();
            (view.DataContext as ManageItemTransferViewModel).AssingProperitesFromRequest(SelectStockWorkList.DocumentUID);
            ChangeViewPermission(view, this.View);
        }
        private void Receive()
        {
            ManageItemReceive view = new ManageItemReceive();
            (view.DataContext as ManageItemReceiveViewModel).AssingPropertiesFromIssue(SelectStockWorkList.DocumentUID);
            ChangeViewPermission(view, this.View);
        }


        #endregion

    }
}
