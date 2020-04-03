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
    public class ListVendorViewModel : MediTechViewModelBase
    {
        #region Properties

        public List<VendorDetailModel> VendorDetails { get; set; }
        private VendorDetailModel _SelectVendorDetail;

        public VendorDetailModel SelectVendorDetail
        {
            get { return _SelectVendorDetail; }
            set {Set(ref _SelectVendorDetail , value); }
        }
            

        #endregion

        #region Command

        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(AddVendor)); }
        }


        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get { return _EditCommand ?? (_EditCommand = new RelayCommand(EditVendor)); }
        }



        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return _DeleteCommand ?? (_DeleteCommand = new RelayCommand(DeleteVendor)); }
        }




        #endregion

        #region Method

        public ListVendorViewModel()
        {
            VendorDetails = DataService.Purchaseing.GetVendorDetail();
        }

        private void AddVendor()
        {
            ManageVendorDetail pageManageVendorDetail = new ManageVendorDetail();
            ChangeViewPermission(pageManageVendorDetail);
        }

        private void EditVendor()
        {
            if (SelectVendorDetail != null)
            {
                ManageVendorDetail pageManageVendorDetail = new ManageVendorDetail();
                if (pageManageVendorDetail.DataContext is ManageVendorDetailViewModel)
                {
                    (pageManageVendorDetail.DataContext as ManageVendorDetailViewModel).AssignModelData(SelectVendorDetail);
                }
                ChangeViewPermission(pageManageVendorDetail);
            }

        }

        private void DeleteVendor()
        {
            if (SelectVendorDetail != null)
            {
                try
                {
                    DialogResult result = DeleteDialog();
                    if (result == DialogResult.Yes)
                    {
                        DataService.Purchaseing.DeleteVendorDetail(SelectVendorDetail.VendorDetailUID, AppUtil.Current.UserID);
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
