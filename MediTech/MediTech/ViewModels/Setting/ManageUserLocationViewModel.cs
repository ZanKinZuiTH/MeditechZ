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
    public class ManageUserLocationViewModel : MediTechViewModelBase
    {
        #region Properties

        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
                if (SelectOrganisation != null)
                {
                    var LocationDatas = DataService.MasterData.GetLocationByOrganisationUID(SelectOrganisation.HealthOrganisationUID);
                    Locations = LocationDatas?.Where(p => p.Description != "Pac_Bed").ToList();
                    var careOrgan = DataService.UserManage.GetCareProviderOrganisation(SelectOrganisation.HealthOrganisationUID);
                    if (careOrgan != null)
                    {
                        Careproviders = careOrgan.Select(p => new CareproviderModel
                        {
                            CareproviderUID = p.CareproviderUID,
                            FullName = p.CareproviderName
                        }).ToList();
                    }
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
                if (_SelectLocation != null)
                {
                    CareproviderLocation = DataService.UserManage.GetCareProviderLocation(_SelectLocation.LocationUID);
                }
            }
        }


        private DateTime _ActiveFrom;

        public DateTime ActiveFrom
        {
            get { return _ActiveFrom; }
            set { Set(ref _ActiveFrom, value); }
        }

        private DateTime? _ActiveTo;

        public DateTime? ActiveTo
        {
            get { return _ActiveTo; }
            set { Set(ref _ActiveTo, value); }
        }

        private List<CareproviderLocationModel> _CareproviderLocation;

        public List<CareproviderLocationModel> CareproviderLocation
        {
            get { return _CareproviderLocation; }
            set { Set(ref _CareproviderLocation, value); }
        }

        private CareproviderLocationModel _SelectCareproviderLocation;

        public CareproviderLocationModel SelectCareproviderLocation
        {
            get { return _SelectCareproviderLocation; }
            set { Set(ref _SelectCareproviderLocation, value); }
        }

        private List<CareproviderModel> _Careproviders;

        public List<CareproviderModel> Careproviders
        {
            get { return _Careproviders; }
            set { Set(ref _Careproviders, value); }
        }

        private CareproviderModel _SelectCareProvider;

        public CareproviderModel SelectCareProvider
        {
            get { return _SelectCareProvider; }
            set { Set(ref _SelectCareProvider, value); }
        }


        #endregion

        #region Command

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddUserLocation)); }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteUserLocation)); }
        }

        #endregion

        #region Method

        public ManageUserLocationViewModel()
        {
            Organisations = GetHealthOrganisation();
            ActiveFrom = DateTime.Today;
            SelectOrganisation = Organisations.FirstOrDefault();

        }

        void AddUserLocation()
        {
            try
            {
                if (SelectLocation == null)
                {
                    WarningDialog("กรุณาเลือกสถานที่/แผนก");
                    return;
                }
                if (SelectCareProvider == null)
                {
                    WarningDialog("กรุณาเลือกผู้ใช้งาน");
                    return;
                }
                if (ActiveFrom == null)
                {
                    WarningDialog("กรุณาเลือกวันที่เปิดใช้งาน");
                    return;
                }

                if (CareproviderLocation != null)
                {
                    if (CareproviderLocation.Any(p => p.LocationUID == SelectLocation.LocationUID && p.CareproviderUID == SelectCareProvider.CareproviderUID))
                    {
                        WarningDialog("ผู้ใช้งานนี้มีข้อมูลอยู่แล้ว");
                        return;
                    }
                }

                CareproviderLocationModel careLoc = new CareproviderLocationModel(); ;

                careLoc.CareproviderUID = SelectCareProvider.CareproviderUID;
                careLoc.LocationUID = SelectLocation.LocationUID;
                careLoc.HealthOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                careLoc.ActiveFrom = ActiveFrom;
                careLoc.ActiveTo = ActiveTo;
                DataService.UserManage.ManageCareProviderLocation(careLoc, AppUtil.Current.UserID);

                CareproviderLocation = DataService.UserManage.GetCareProviderLocation(SelectLocation.LocationUID);

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        void DeleteUserLocation()
        {
            if (SelectCareproviderLocation != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.UserManage.DeleteCareproviderLocation(SelectCareproviderLocation.CareproviderLocationUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        CareproviderLocation.Remove(SelectCareproviderLocation);
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
