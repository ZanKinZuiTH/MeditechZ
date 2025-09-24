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
    public class ManageUserOrganisationViewModel : MediTechViewModelBase
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
                    CareproviderOrganisation = DataService.UserManage.GetCareProviderOrganisation(SelectOrganisation.HealthOrganisationUID);
                }
            }
        }

        private DateTime? _ActiveFrom;

        public DateTime? ActiveFrom
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

        private List<CareproviderOrganisationModel> _CareproviderOrganisation;

        public List<CareproviderOrganisationModel> CareproviderOrganisation
        {
            get { return _CareproviderOrganisation; }
            set { Set(ref _CareproviderOrganisation, value); }
        }

        private CareproviderOrganisationModel _SelectCareproviderOrganisation;

        public CareproviderOrganisationModel SelectCareproviderOrganisation
        {
            get { return _SelectCareproviderOrganisation; }
            set { Set(ref _SelectCareproviderOrganisation, value); }
        }

        private List<CareproviderModel> _CareProvider;

        public List<CareproviderModel> CareProvider
        {
            get { return _CareProvider; }
            set { _CareProvider = value; }
        }

        private CareproviderModel _SelectCareProvider;

        public CareproviderModel SelectCareProvider
        {
            get { return _SelectCareProvider; }
            set { _SelectCareProvider = value; }
        }


        #endregion

        #region Command

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddUserOrganisation)); }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteUserOrganisation)); }
        }

        #endregion

        #region Method

        public ManageUserOrganisationViewModel()
        {
            Organisations = GetHealthOrganisation();
            ActiveFrom = DateTime.Today;
            CareProvider = DataService.UserManage.GetCareproviderAll();
            SelectOrganisation = Organisations.FirstOrDefault();
            
        }

        void AddUserOrganisation()
        {
            try
            {
                if (SelectCareProvider == null)
                {
                    WarningDialog("กรุณาเลือกผู้ใช้งาน");
                    return;
                }

                if (CareproviderOrganisation != null)
                {
                    if (CareproviderOrganisation.Any(p => p.HealthOrganisationUID == SelectOrganisation.HealthOrganisationUID && p.CareproviderUID == SelectCareProvider.CareproviderUID))
                    {
                        WarningDialog("ผู้ใช้งานนี้มีข้อมูลอยู่แล้ว");
                        return;
                    }
                }

                CareproviderOrganisationModel careOrgan = new CareproviderOrganisationModel();;

                careOrgan.CareproviderUID = SelectCareProvider.CareproviderUID;
                careOrgan.HealthOrganisationUID = SelectOrganisation.HealthOrganisationUID;
                careOrgan.ActiveFrom = ActiveFrom;
                careOrgan.ActiveTo = ActiveTo;
                DataService.UserManage.ManageCareProviderOrganisation(careOrgan, AppUtil.Current.UserID);

                CareproviderOrganisation = DataService.UserManage.GetCareProviderOrganisation(SelectOrganisation.HealthOrganisationUID);

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        void DeleteUserOrganisation()
        {
            if (SelectCareproviderOrganisation != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.UserManage.DeleteCareproviderOrganisation(SelectCareproviderOrganisation.CareproviderOrganisationUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        CareproviderOrganisation.Remove(SelectCareproviderOrganisation);
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
