using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ListStoreUOMConversionViewModel : MediTechViewModelBase
    {

        #region Properties


        private List<StoreUOMConversionModel> _StoreUOMConversion;

        public List<StoreUOMConversionModel> StoreUOMConversion
        {
            get { return _StoreUOMConversion; }
            set { Set(ref _StoreUOMConversion, value); }
        }
        public StoreUOMConversionModel SelectStoreUOMConversion { get; set; }

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

        public ListStoreUOMConversionViewModel()
        {
            StoreUOMConversion = DataService.Inventory.GetStoreUOMConversion();
        }
        private void Add()
        {
            ManageStoreUOMConversion pageMange = new ManageStoreUOMConversion();
            ChangeViewPermission(pageMange);
        }

        private void Edit()
        {
            if (SelectStoreUOMConversion != null)
            {
                ManageStoreUOMConversion pageMange = new ManageStoreUOMConversion();
                (pageMange.DataContext as ManageStoreUOMConversionViewModel).AssingModel(SelectStoreUOMConversion);
                ChangeViewPermission(pageMange);
            }
        }

        private void Delete()
        {
            if (SelectStoreUOMConversion != null)
            {
                try
                {
                    DialogResult result = DeleteDialog();
                    if (result == DialogResult.Yes)
                    {
                        DataService.Inventory.DeleteStoreUOMConversion(SelectStoreUOMConversion.StoreUOMConversionUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        StoreUOMConversion.Remove(SelectStoreUOMConversion);
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
