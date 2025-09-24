using GalaSoft.MvvmLight.Command;
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
    public class ListCheckupJobViewModel : MediTechViewModelBase
    {
        #region Properties
        private List<CheckupJobContactModel> _ListCheckupJob;

        public List<CheckupJobContactModel> ListCheckupJob
        {
            get { return _ListCheckupJob; }
            set { Set(ref _ListCheckupJob, value); }
        }

        private CheckupJobContactModel _SelectCheckupJob;

        public CheckupJobContactModel SelectCheckupJob
        {
            get { return _SelectCheckupJob; }
            set { Set(ref _SelectCheckupJob, value); }
        }

        private List<InsuranceCompanyModel> _InsuranceCompany;
        public List<InsuranceCompanyModel> InsuranceCompany
        {
            get { return _InsuranceCompany; }
            set { Set(ref _InsuranceCompany, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompany;
        public InsuranceCompanyModel SelectInsuranceCompany
        {
            get { return _SelectInsuranceCompany; }
            set
            {
                Set(ref _SelectInsuranceCompany, value);
            }
        }

        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        #endregion

        #region Command


        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddJob));
            }
        }


        private RelayCommand _EditCommand;

        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditJob));
            }
        }


        private RelayCommand _DeleteCommand;

        public RelayCommand DeleteCommand
        {
            get
            {
                return _DeleteCommand
                    ?? (_DeleteCommand = new RelayCommand(DeleteJob));
            }
        }
        
        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                return _SearchCommand
                    ?? (_SearchCommand = new RelayCommand(Search));
            }
        }

        #endregion

        #region Method

        public ListCheckupJobViewModel()
        {
            InsuranceCompany = DataService.Billing.GetInsuranceCompanyAll();
            SelectInsuranceCompany = InsuranceCompany.FirstOrDefault();
            Search();
        }

        void Search()
        {
            if(SelectInsuranceCompany == null)
            {
                WarningDialog("กรุณาเลือก Payor");
                return;
            }

            int? insuranceUID = SelectInsuranceCompany != null ? SelectInsuranceCompany.InsuranceCompanyUID : (int?) null;
            ListCheckupJob = DataService.Checkup.SearchCheckupJobContactActive(insuranceUID, DateFrom, DateTo);
        }

        void AddJob()
        {
            ManageCheckupJob pageManage = new ManageCheckupJob();
            ChangeViewPermission(pageManage);
        }

        void EditJob()
        {
            if (SelectCheckupJob != null)
            {
                ManageCheckupJob pageUserManage = new ManageCheckupJob();
                (pageUserManage.DataContext as ManageCheckupJobViewModel).AssingModel(SelectCheckupJob.CheckupJobContactUID);
                ChangeViewPermission(pageUserManage);
            }
        }

        void DeleteJob()
        {
            if (SelectCheckupJob != null)
            {
                try
                {
                    MessageBoxResult result = DeleteDialog();
                    if (result == MessageBoxResult.Yes)
                    {

                        DataService.Checkup.DeleteCheckupJobContact(SelectCheckupJob.CheckupJobContactUID, AppUtil.Current.UserID);
                        DeleteSuccessDialog();
                        ListCheckupJob.Remove(SelectCheckupJob);
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
