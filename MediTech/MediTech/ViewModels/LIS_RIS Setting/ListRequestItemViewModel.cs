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
    public class ListRequestItemViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<RequestItemModel> _RequestItems;

        public List<RequestItemModel> RequestItems
        {
            get { return _RequestItems; }
            set { Set(ref _RequestItems , value); }
        }

        private RequestItemModel _SelectRequestItem;

        public RequestItemModel SelectRequestItem
        {
            get { return _SelectRequestItem; }
            set { Set(ref _SelectRequestItem, value); }
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

        public ListRequestItemViewModel()
        {
            RequestItems = DataService.MasterData.GetRequestItems();
        }


        public void Add()
        {
            ManageRequestItem manageview = new ManageRequestItem();
            ChangeViewPermission(manageview);
        }

        public void Edit()
        {
            if (SelectRequestItem != null)
            {
                ManageRequestItem manageview = new ManageRequestItem();
                (manageview.DataContext as ManageRequestItemViewModel).AssignModel(SelectRequestItem);
                ChangeViewPermission(manageview);
            }
        }

        public void Delete()
        {
            if (SelectRequestItem != null)
            {
                DialogResult result = DeleteDialog();
                if (result == DialogResult.Yes)
                {
                    try
                    {

                        DeleteSuccessDialog();
                        DataService.MasterData.DeleteRequestItem(SelectRequestItem.RequestItemUID, AppUtil.Current.UserID);
                        RequestItems.Remove(SelectRequestItem);
                        OnUpdateEvent();
                    }
                    catch (Exception er)
                    {

                        ErrorDialog(er.Message);
                    }

                }
            }
        }

        #endregion
    }
}
