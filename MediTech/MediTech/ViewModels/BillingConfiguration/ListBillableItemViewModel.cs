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
    public class ListBillableItemViewModel : MediTechViewModelBase
    {
        #region Properties

        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public List<BillingSubGroupModel> AllSubGroup { get; set; }
        public List<LookupReferenceValueModel> BillItemTypes { get; set; }
        public LookupReferenceValueModel SelectBillItemType { get; set; }

        public List<HealthOrganisationModel> Organisations { get; set; }
        public HealthOrganisationModel SelectOrganisation { get; set; }


        private List<BillingGroupModel> _BillingGroup;

        public List<BillingGroupModel> BillingGroup
        {
            get { return _BillingGroup; }
            set { Set(ref _BillingGroup, value); }
        }

        private BillingGroupModel _SelectBillingGroup;

        public BillingGroupModel SelectBillingGroup
        {
            get { return _SelectBillingGroup; }
            set
            {
                Set(ref _SelectBillingGroup, value);
                if (_SelectBillingGroup != null)
                {
                    BillingSubGroup = AllSubGroup
                        .Where(p => p.BillingGroupUID == SelectBillingGroup.BillingGroupUID).ToList();
                }
                else
                {
                    BillingSubGroup = AllSubGroup;
                }
            }
        }


        private List<BillingSubGroupModel> _BillingSubGroup;

        public List<BillingSubGroupModel> BillingSubGroup
        {
            get { return _BillingSubGroup; }
            set { Set(ref _BillingSubGroup, value); }
        }

        private BillingSubGroupModel _SelectBillingSubGroup;

        public BillingSubGroupModel SelectBillingSubGroup
        {
            get { return _SelectBillingSubGroup; }
            set { Set(ref _SelectBillingSubGroup, value); }
        }

        private List<BillableItemModel> _BillableItems;

        public List<BillableItemModel> BillableItems
        {
            get { return _BillableItems; }
            set { Set(ref _BillableItems ,value); }
        }


        public BillableItemModel SelectBillableItem { get; set; }

        #endregion

        #region Command

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(Add)); }
        }

        private RelayCommand _EditCommand;
        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(Edit)); }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(Delete)); }
        }

        #endregion

        #region Method

        public ListBillableItemViewModel()
        {

            BillItemTypes = DataService.Technical.GetReferenceValueMany("BSMDD");
            if (BillItemTypes != null)
            {
                BillItemTypes.Add(new LookupReferenceValueModel { Display = "All", Key = 0 });

                BillItemTypes = BillItemTypes.OrderBy(p => p.Key).ToList();
            }
            Organisations = GetHealthOrganisationRole();
            BillingGroup = DataService.MasterData.GetBillingGroup();
            AllSubGroup = DataService.MasterData.GetBillingSubGroup();
            BillingSubGroup = AllSubGroup;

            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);
            if (SelectOrganisation == null)
            {
                SelectOrganisation = Organisations.FirstOrDefault();
            }
        }

        public override void OnLoaded()
        {
            Search();
        }

        private void Search()
        {
            int? ownerOrganisationUID = (SelectOrganisation != null && SelectOrganisation.HealthOrganisationUID != 0) ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            BillableItems = DataService.MasterData.SearchBillableItem(ItemCode, ItemName
                , (SelectBillItemType != null && SelectBillItemType.Key != 0) ? SelectBillItemType.Key : (int?)null
                , (SelectBillingGroup != null && SelectBillingGroup.BillingGroupUID != 0) ? SelectBillingGroup.BillingGroupUID : (int?)null
                , (SelectBillingSubGroup != null && SelectBillingSubGroup.BillingSubGroupUID != 0) ? SelectBillingSubGroup.BillingSubGroupUID : (int?)null
                ,ownerOrganisationUID);
        }
        private void Add()
        {
            ManageBillableItem pageManage = new ManageBillableItem();
            ChangeViewPermission(pageManage);
        }

        private void Edit()
        {
            if (SelectBillableItem != null)
            {
                ManageBillableItem pageManage = new ManageBillableItem();
                (pageManage.DataContext as ManageBillableItemViewModel).AssingModel(SelectBillableItem);
                ChangeViewPermission(pageManage);
            }

        }

        private void Delete()
        {
            if (SelectBillableItem != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.MasterData.DeleteBillableItem(SelectBillableItem.BillableItemUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        BillableItems.Remove(SelectBillableItem);
                        OnUpdateEvent();
                    }
                }
                catch (Exception er)
                {

                    ErrorDialog(er.Message);
                }

            }
        }

        #endregion
    }
}
