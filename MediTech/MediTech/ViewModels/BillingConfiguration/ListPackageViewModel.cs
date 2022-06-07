using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ListPackageViewModel : MediTechViewModelBase
    {
        #region Properties

        private SearchOrderItem _SelectOrderItem;
        public SearchOrderItem SelectOrderItem
        {
            get { return _SelectOrderItem; }
            set
            {
                _SelectOrderItem = value;
                if (_SelectOrderItem != null)
                {
                    //ApplyOrderItem(_SelectOrderItem);
                }
            }
        }

        private List<BillPackageModel> _BillPackage;
        public List<BillPackageModel> BillPackage
        {
            get { return _BillPackage; }
            set { Set(ref _BillPackage, value); }
        }

        private BillPackageModel _SelectBillPackage;
        public BillPackageModel SelectBillPackage
        {
            get { return _SelectBillPackage; }
            set { Set(ref _SelectBillPackage, value); }
        }

        private string _PackageCode;
        public string PackageCode
        {
            get { return _PackageCode; }
            set { Set(ref _PackageCode, value); }
        }

        private string _PackageName;
        public string PackageName
        {
            get { return _PackageName; }
            set { Set(ref _PackageName, value); }
        }

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
        public ListPackageViewModel()
        {
            BillPackage = DataService.Billing.SearchBillPackage(PackageCode,PackageName);
        }

        private void Search()
        {
            BillPackage = DataService.Billing.SearchBillPackage(PackageCode, PackageName);
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
            if(SelectBillPackage != null)
            {
                ManagePackage pageManage = new ManagePackage();
                (pageManage.DataContext as ManagePackageViewModel).AssignModel(SelectBillPackage);
                ChangeViewPermission(pageManage);
            }
        }

        private void Delete()
        {
            if (SelectBillPackage != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Billing.DeleteBillPackage(SelectBillPackage.BillPackageUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        BillPackage.Remove(SelectBillPackage);
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
