using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using MediTech.Model;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using MediTech.DataService;
using MediTech.Views;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ListOrderSetViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _ItemCode;

        public string ItemCode
        {
            get { return _ItemCode; }
            set { Set(ref _ItemCode , value); }
        }

        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }

        private HealthOrganisationModel _SelectOrganisation;

        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set { _SelectOrganisation = value; }
        }

        private List<OrderSetModel> _OrderSets;

        public List<OrderSetModel> OrderSets
        {
            get { return _OrderSets; }
            set { Set(ref _OrderSets, value); }
        }

        private OrderSetModel _SelectOrderSet;

        public OrderSetModel SelectOrderSet
        {
            get { return _SelectOrderSet; }
            set { Set(ref _SelectOrderSet, value); }
        }

        
        #endregion

        #region Command

        private RelayCommand _SearchCommand;

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(SearchOrderSet));
            }
        }


        private RelayCommand _AddCommand;

        /// <summary>
        /// Gets the AddCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(ExecuteAddCommand));
            }
        }

        private RelayCommand _EditCommand;

        /// <summary>
        /// Gets the EditCommand.
        /// </summary>
        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(ExecuteEditCommand));
            }
        }


        private RelayCommand _CopyCommand;

        /// <summary>
        /// Gets the EditCommand.
        /// </summary>
        public RelayCommand CopyCommand
        {
            get
            {
                return _CopyCommand
                    ?? (_CopyCommand = new RelayCommand(ExecuteCopyCommand));
            }
        }

        private RelayCommand _DeleteCommand;

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(ExecuteDeleteCommand));
            }
        }



        #endregion

        #region Method

        public ListOrderSetViewModel()
        {
            SearchOrderSet();
        }
        private void SearchOrderSet()
        {
            OrderSets = DataService.MasterData.SearchOrderSet(ItemCode, ItemName);
        }

        private void ExecuteAddCommand()
        {
            ManageOrderSet pageManage = new ManageOrderSet();
            ChangeViewPermission(pageManage);
        }

        private void ExecuteEditCommand()
        {
            if (SelectOrderSet != null)
            {
                ManageOrderSet pageManage = new ManageOrderSet();
                (pageManage.DataContext as ManageOrderSetViewModel).AssingModel(SelectOrderSet);
                ChangeViewPermission(pageManage);
            }
        }

        private void ExecuteCopyCommand()
        {
            if (SelectOrderSet != null)
            {
                ManageOrderSet pageManage = new ManageOrderSet();
                (pageManage.DataContext as ManageOrderSetViewModel).CopyOrderSet(SelectOrderSet);
                ChangeViewPermission(pageManage);
            }
        }

        private void ExecuteDeleteCommand()
        {
            if (SelectOrderSet != null)
            {
                try
                {
                    DialogResult result = DeleteDialog();
                    if (result == DialogResult.Yes)
                    {
                        DataService.MasterData.DeleteOrderSet(SelectOrderSet.OrderSetUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        OrderSets.Remove(SelectOrderSet);
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
