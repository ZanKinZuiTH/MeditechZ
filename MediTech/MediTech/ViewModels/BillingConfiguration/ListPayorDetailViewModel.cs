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
    public class ListPayorDetailViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _Code;

        public string Code
        {
            get { return _Code; }
            set { Set(ref _Code, value); }
        }

        private string _PayorName;

        public string PayorName
        {
            get { return _PayorName; }
            set { Set(ref _PayorName, value); }
        }

        private List<PayorDetailModel> _PayorDetails;

        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        private PayorDetailModel _SelectPayorDetail;

        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set { Set(ref _SelectPayorDetail, value); }
        }


        #endregion

        #region Command

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(SearchPayorDetail));
            }
        }


        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddPayorDetail));
            }
        }

        private RelayCommand _EditCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditPayorDetail));
            }
        }

        private RelayCommand _DeleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeletePayorDetail));
            }
        }

        #endregion

        #region Method

        private void SearchPayorDetail()
        {
            PayorDetails = DataService.Billing.SearchPayorDetail(Code, PayorName);
        }

        private void AddPayorDetail()
        {
            ManagePayorDetail mangaPayor = new ManagePayorDetail();
            ChangeViewPermission(mangaPayor);
        }

        private void EditPayorDetail()
        {
            if (SelectPayorDetail != null)
            {
                ManagePayorDetail pageManage = new ManagePayorDetail();
                (pageManage.DataContext as ManagePayorDetailViewModel).AssingModel(SelectPayorDetail.PayorDetailUID);
                ChangeViewPermission(pageManage);
            }
        }

        private void DeletePayorDetail()
        {
            if (SelectPayorDetail != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {
                        DataService.Billing.DeletePayorDetail(SelectPayorDetail.PayorDetailUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        PayorDetails.Remove(SelectPayorDetail);
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
