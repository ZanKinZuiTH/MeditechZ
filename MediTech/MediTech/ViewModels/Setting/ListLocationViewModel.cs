using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MediTech.DataService;
using MediTech.Model;
using MediTech.Models;
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
    public class ListLocationViewModel : MediTechViewModelBase
    {
        #region properties
        public List<LocationModel> Locations { get; set; }

        private LocationModel _SelectLocations;

        public LocationModel SelectLocations
        {
            get { return _SelectLocations; }
            set { Set(ref _SelectLocations, value); }
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

        #region method
        public ListLocationViewModel()
        {
            Locations = DataService.Technical.GetLocation();
        }

        private void Add()
        {
            ManageLocation manageLocation = new ManageLocation();
            ChangeViewPermission(manageLocation);
        }
        private void Edit()
        {
            if (SelectLocations != null)
            {
                ManageLocation pageManage = new ManageLocation();
                if (pageManage.DataContext is ManageLocationViewModel)
                {
                    (pageManage.DataContext as ManageLocationViewModel).AssignModelData(SelectLocations);
                }
                ChangeViewPermission(pageManage);
            }
        }

        private void Delete()
        {
            if (SelectLocations != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Technical.DeleteLocation(SelectLocations.LocationUID, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        Locations = DataService.Technical.GetLocation();
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
