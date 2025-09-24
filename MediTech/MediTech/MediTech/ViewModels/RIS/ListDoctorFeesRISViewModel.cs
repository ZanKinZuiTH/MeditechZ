using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class ListDoctorFeesRISViewModel : MediTechViewModelBase
    {

        #region Properties

        private ObservableCollection<DoctorFeeModel> _ListDoctorFeeNonPay;

        public ObservableCollection<DoctorFeeModel> ListDoctorFeeNonPay
        {
            get { return _ListDoctorFeeNonPay; }
            set { Set(ref _ListDoctorFeeNonPay, value); }
        }

        private ObservableCollection<DoctorFeeModel> _SelectDoctorFeeNonPay;

        public ObservableCollection<DoctorFeeModel> SelectDoctorFeeNonPay
        {
            get { return _SelectDoctorFeeNonPay ?? (_SelectDoctorFeeNonPay = new ObservableCollection<DoctorFeeModel>()); }
            set { Set(ref _SelectDoctorFeeNonPay, value); }
        }


        private ObservableCollection<DoctorFeeModel> _ListDoctorFee;

        public ObservableCollection<DoctorFeeModel> ListDoctorFee
        {
            get { return _ListDoctorFee; }
            set { Set(ref _ListDoctorFee, value); }
        }

        private ObservableCollection<DoctorFeeModel> _SelectDoctorFee;

        public ObservableCollection<DoctorFeeModel> SelectDoctorFee
        {
            get { return _SelectDoctorFee ?? (_SelectDoctorFee = new ObservableCollection<DoctorFeeModel>()); }
            set { Set(ref _SelectDoctorFee, value); }
        }

        private DateTime _DateFrom;

        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime _DateTo;

        public DateTime DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private List<CareproviderModel> _Doctors;

        public List<CareproviderModel> Doctors
        {
            get { return _Doctors; }
            set { _Doctors = value; }
        }

        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { Set(ref _SelectDoctor, value); }
        }

        private Visibility _IsVisibility;

        public Visibility IsVisibility
        {
            get { return _IsVisibility; }
            set { _IsVisibility = value; }
        }

        private bool _ShowCheckBoxColumn;

        public bool ShowCheckBoxColumn
        {
            get { return _ShowCheckBoxColumn; }
            set { _ShowCheckBoxColumn = value; }
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
                    ?? (_SearchCommand = new RelayCommand(SearchDoctorFee));
            }
        }

        private RelayCommand _AddCommand;

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddDoctorFee));
            }
        }

        private RelayCommand _RemoveCommand;

        /// <summary>
        /// Gets the SearchCommand.
        /// </summary>
        public RelayCommand RemoveCommand
        {
            get
            {
                return _RemoveCommand
                    ?? (_RemoveCommand = new RelayCommand(RemoveDoctorFee));
            }
        }



        #endregion

        #region Method

        public ListDoctorFeesRISViewModel()
        {
            int year = DateTime.Now.Year;
            int mounth = DateTime.Now.Month;
            DateFrom = DateTime.Parse("01/" + mounth.ToString() + "/" + year);
            DateTo = DateTime.Parse(DateTime.DaysInMonth(year, mounth).ToString() + "/" + mounth.ToString() + "/" + year);
            if ((AppUtil.Current.IsAdminRadread ?? false))
            {
                IsVisibility = Visibility.Visible;
                ShowCheckBoxColumn = true;
            }
            else
            {
                IsVisibility = Visibility.Hidden;
                ShowCheckBoxColumn = false;
            }

            Doctors = DataService.UserManage.GetCareproviderDoctor();

            SelectDoctor = Doctors.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
            SearchDoctorFee();
        }
        private void SearchDoctorFee()
        {

            int? radiologistUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
            ListDoctorFeeNonPay = new ObservableCollection<DoctorFeeModel>(DataService.Radiology.GetDoctorFeeNonPay(DateFrom, DateTo, radiologistUID));
            ListDoctorFee = new ObservableCollection<DoctorFeeModel>(DataService.Radiology.GetDoctorFee(DateFrom, DateTo, radiologistUID));
        }

        private void AddDoctorFee()
        {
            try
            {
                if (SelectDoctorFeeNonPay != null && SelectDoctorFeeNonPay.Count > 0)
                {
                    DataService.Radiology.AddDoctorFee(SelectDoctorFeeNonPay.ToList(), AppUtil.Current.UserID);
                    SaveSuccessDialog();
                    SearchDoctorFee();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void RemoveDoctorFee()
        {
            try
            {
                if (SelectDoctorFee != null && SelectDoctorFee.Count > 0)
                {
                    DataService.Radiology.RemoveDoctorFee(SelectDoctorFee.ToList(), AppUtil.Current.UserID);
                    SaveSuccessDialog();
                    SearchDoctorFee();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        #endregion
    }
}
