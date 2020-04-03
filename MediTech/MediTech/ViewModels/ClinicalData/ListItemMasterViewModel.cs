using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ListItemMasterViewModel : MediTechViewModelBase
    {

        #region Properties

        public string ItemName { get; set; }
        public string ItemCode { get; set; }

        public List<LookupReferenceValueModel> ItemTypes { get; set; }
        public LookupReferenceValueModel SelectItemType { get; set; }

        private List<ItemMasterModel> _ItemMasters;

        public List<ItemMasterModel> ItemMasters
        {
            get { return _ItemMasters; }
            set { Set(ref _ItemMasters, value); }
        }

        public ItemMasterModel SelectItemMaster { get; set; }
        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(SearchItemMaster));
            }

        }

        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddItemMaster));
            }

        }

        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditItemMaster));
            }

        }

        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteItemMaster));
            }

        }

        #endregion

        #region Method

        public ListItemMasterViewModel()
        {
            ItemTypes = DataService.Technical.GetReferenceValueMany("ITMTYP");
        }


        private void SearchItemMaster()
        {
            ItemMasters = DataService.Inventory.SearchItemMaster(ItemName, ItemCode, SelectItemType != null ? SelectItemType.Key : (int?)null);
        }


        private void AddItemMaster()
        {
            ManageItemMaster pageManageItemMaster = new ManageItemMaster();
            ChangeViewPermission(pageManageItemMaster);
        }

        private void EditItemMaster()
        {
            if (SelectItemMaster != null)
            {
                ManageItemMaster pageManageItemMaster = new ManageItemMaster();
                (pageManageItemMaster.DataContext as ManageItemMasterViewModel).AssingModelData(SelectItemMaster);
                ChangeViewPermission(pageManageItemMaster);
            }

        }

        private void DeleteItemMaster()
        {
            if (SelectItemMaster != null)
            {
                var dataTransaction = DataService.Inventory.GetStockRemainByItemMasterUID(SelectItemMaster.ItemMasterUID, 0);
                if (dataTransaction != null && dataTransaction.Any(p => p.Quantity > 0))
                {
                    WarningDialog("รายการนี้มีจำนวนคงเหลืออยู่ในคลัง ไม่สามารถทำการลบได้ !");
                    return;
                }
                DialogResult result = DeleteDialog();

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        DeleteSuccessDialog();
                        DataService.Inventory.DeleteItemMaster(SelectItemMaster.ItemMasterUID, AppUtil.Current.UserID);
                        ItemMasters.Remove(SelectItemMaster);
                        OnUpdateEvent();
                    }
                    catch (Exception er)
                    {

                        ErrorDialog(er.Message);
                    }

                }
            }
        }




        #endregion
    }
}
