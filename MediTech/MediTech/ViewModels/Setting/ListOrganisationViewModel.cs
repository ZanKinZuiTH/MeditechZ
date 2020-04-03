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
    public class ListOrganisationViewModel : MediTechViewModelBase
    {

        #region Properties

        public List<HealthOrganisationModel> Organisations { get; set; }
        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set {Set(ref _SelectOrganisation , value); }
        }
            

        #endregion

        #region Command

        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddOrganisation)); }
        }


        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(EditOrganisation)); }
        }



        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteOrganisation)); }
        }




        #endregion

        #region Method

        public ListOrganisationViewModel()
        {
            Organisations = DataService.MasterData.GetHealthOrganisation();
        }

        private void AddOrganisation()
        {
            ManageOrganisation pageManageOrganisation = new ManageOrganisation();
            ChangeViewPermission(pageManageOrganisation);
        }

        private void EditOrganisation()
        {
            if (SelectOrganisation != null)
            {
                ManageOrganisation pageManageOrganisation = new ManageOrganisation();
                if (pageManageOrganisation.DataContext is ManageOrganisationViewModel)
                {
                    (pageManageOrganisation.DataContext as ManageOrganisationViewModel).AssignModelData(SelectOrganisation);
                }
                ChangeViewPermission(pageManageOrganisation);
            }

        }

        private void DeleteOrganisation()
        {
            if (SelectOrganisation != null)
            {
                try
                {
                    DialogResult result = DeleteDialog();
                    if (result == DialogResult.Yes)
                    {
                        DataService.MasterData.DeleteHealthOrganisation(SelectOrganisation.HealthOrganisationUID, AppUtil.Current.UserID);
                        SaveSuccessDialog();
                        Organisations = DataService.MasterData.GetHealthOrganisation();
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
