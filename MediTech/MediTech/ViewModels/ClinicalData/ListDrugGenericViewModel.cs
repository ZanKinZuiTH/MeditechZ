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
    public class ListDrugGenericViewModel : MediTechViewModelBase
    {          
        #region Properties

        public List<DrugGenericModel> DrugGenerics { get; set; }

        private DrugGenericModel  _SelectDrugGeneric;

        public DrugGenericModel SelectDrugGeneric
        {
            get { return _SelectDrugGeneric; }
            set { Set(ref _SelectDrugGeneric, value); }
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

        public ListDrugGenericViewModel()
        {
            DrugGenerics = DataService.Pharmacy.GetDrugGeneric();
        }

        private void Add()
        {
            ManageDrugGeneric pageManage = new ManageDrugGeneric();
            ChangeViewPermission(pageManage);
        }

        private void Edit()
        {
            if (SelectDrugGeneric != null)
            {
                ManageDrugGeneric pageManage = new ManageDrugGeneric();
                (pageManage.DataContext as ManageDrugGenericViewModel).AssingModel(SelectDrugGeneric);
                ChangeViewPermission(pageManage);
            }

        }

        private void Delete()
        {
            if (SelectDrugGeneric != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Pharmacy.DeleteDrugGeneric(SelectDrugGeneric.DrugGenericUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        DrugGenerics.Remove(SelectDrugGeneric);
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
