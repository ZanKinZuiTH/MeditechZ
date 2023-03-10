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
    public class ListDrugFrequencyViewModel : MediTechViewModelBase
    {
        #region Properties

        public List<FrequencyDefinitionModel> DrugFrequencys { get; set; }

        private FrequencyDefinitionModel  _SelectDrugFrequency;

        public FrequencyDefinitionModel SelectDrugFrequency
        {
            get { return _SelectDrugFrequency; }
            set { Set(ref _SelectDrugFrequency, value); }
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

        private RelayCommand _ExportToXLSXCommand;
        public RelayCommand ExportToXLSXCommand
        {
            get { return _ExportToXLSXCommand ?? (_ExportToXLSXCommand = new RelayCommand(ExportToXLSX)); }
        }

        #endregion

        #region Method

        public ListDrugFrequencyViewModel()
        {
            DrugFrequencys = DataService.Pharmacy.GetDrugFrequency();
        }

        private void Add()
        {
            ManageDrugFrequency pageManage = new ManageDrugFrequency();
            ChangeViewPermission(pageManage);
        }

        private void Edit()
        {
            if (SelectDrugFrequency != null)
            {
                ManageDrugFrequency pageManage = new ManageDrugFrequency();
                (pageManage.DataContext as ManageDrugFrequencyViewModel).AssingModel(SelectDrugFrequency);
                ChangeViewPermission(pageManage);
            }

        }

        private void ExportToXLSX()
        {
            try
            {
                if (DrugFrequencys != null)
                {
                    string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                    if (fileName != "")
                    {
                        ListDrugFrequency view = (ListDrugFrequency)this.View;
                        view.gvDrugFrequency.ExportToXlsx(fileName);
                        OpenFile(fileName);
                    }
                }
            }
            catch (Exception er)
            {
                ErrorDialog(er.Message);
            }
        }

        private void Delete()
        {
            if (SelectDrugFrequency != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Pharmacy.DeleteDrugGeneric(SelectDrugFrequency.FrequencyUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        DrugFrequencys.Remove(SelectDrugFrequency);
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
