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
    public class ListStoresViewModel : MediTechViewModelBase
    {

        #region Properties


        private List<StoreModel> _Stores;

        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }
        public StoreModel SelectStore { get; set; }

        #endregion

        #region Command

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

        #endregion

        #region Method

        public ListStoresViewModel()
        {
            Stores = DataService.Inventory.GetStore();
        }
        private void Add()
        {
            ManageStore pageMange = new ManageStore();
            ChangeViewPermission(pageMange);
        }

        private void Edit()
        {
            if (SelectStore != null)
            {
                ManageStore pageMange = new ManageStore();
                (pageMange.DataContext as ManageStoreViewModel).AssingModel(SelectStore);
                ChangeViewPermission(pageMange);
            }
        }

        private void Delete()
        {
            if (SelectStore != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Inventory.DeleteStore(SelectStore.StoreUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        Stores.Remove(SelectStore);
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
