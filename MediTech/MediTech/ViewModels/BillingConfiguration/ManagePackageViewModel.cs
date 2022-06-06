using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManagePackageViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<SearchOrderItem> _OrderItems;
        public List<SearchOrderItem> OrderItems
        {
            get { return _OrderItems; }
            set { Set(ref _OrderItems, value); }
        }

        private bool _EnableSearchItem = false;
        public bool EnableSearchItem
        {
            get { return _EnableSearchItem; ; }
            set { Set(ref _EnableSearchItem, value); }
        }

        private string _SearchOrderCriteria;
        public string SearchOrderCriteria
        {
            get { return _SearchOrderCriteria; }
            set
            {
                Set(ref _SearchOrderCriteria, value);
                if (!string.IsNullOrEmpty(_SearchOrderCriteria) && _SearchOrderCriteria.Length >= 3)
                {
                    int ownerOrganisationUID = 0;
                    //if (SelectOrganisation != null)
                    //{
                    //    ownerOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                    //}
                    OrderItems = DataService.OrderProcessing.SearchOrderItem(_SearchOrderCriteria, ownerOrganisationUID);

                }
                else
                {
                    OrderItems = null;
                }
            }
        }
        #endregion

        #region Command
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
        public ManagePackageViewModel()
        {

        }
        private void Add()
        {
            ManagePackage pageManage = new ManagePackage();
            ChangeViewPermission(pageManage);
            //ManageBillableItem pageManage = new ManageBillableItem();
            //ChangeViewPermission(pageManage)
        }

        private void Edit()
        {
            //if (SelectBillPackage != null)
            //{
            //    //ManageBillableItem pageManage = new ManageBillableItem();
            //    //(pageManage.DataContext as ManageBillableItemViewModel).AssingModel(SelectBillableItem);
            //    //ChangeViewPermission(pageManage);
            //}
        }

        private void Delete()
        {
            //if (SelectBillPackage != null)
            //{
            //    try
            //    {
            //        MessageBoxResult result = DeleteDialog();
            //        if (result == MessageBoxResult.Yes)
            //        {
            //            DataService.Billing.DeleteBillPackage(SelectBillPackage.BillPackageUID, AppUtil.Current.UserID);
            //            DeleteSuccessDialog();
            //            BillPackage.Remove(SelectBillPackage);
            //            OnUpdateEvent();
            //        }
            //    }
            //    catch (Exception er)
            //    {

            //        ErrorDialog(er.Message);
            //    }
            //}
        }
        #endregion
    }
}
