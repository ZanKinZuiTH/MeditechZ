using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class RadiologistReportViewModel : MediTechViewModelBase
    {
        #region Properties

        private List<DoctorFeeModel> _ListDoctorFee;

        public List<DoctorFeeModel> ListDoctorFee
        {
            get { return _ListDoctorFee; }
            set { Set(ref _ListDoctorFee, value); }
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

        #endregion

        #region Method

        public RadiologistReportViewModel()
        {
            int year = DateTime.Now.Year;
            int mounth = DateTime.Now.Month;
            DateFrom = DateTime.Parse("01/" + mounth.ToString() + "/" + year);
            DateTo = DateTime.Parse(DateTime.DaysInMonth(year, mounth).ToString() + "/" + mounth.ToString() + "/" + year);
            Doctors = DataService.UserManage.GetCareproviderDoctor();


            SelectDoctor = Doctors.FirstOrDefault(p => p.CareproviderUID == AppUtil.Current.UserID);
        }

        void SearchDoctorFee()
        {
            int? radiologistUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;

            var doctorFee =  DataService.Radiology.GetRadiologistReport(DateFrom, DateTo, radiologistUID);
            foreach (var item in doctorFee)
            {
                item.ResultEnteredDate = item.ResultEnteredDate.Value.Date;
            }
            ListDoctorFee = doctorFee;
        }

        #endregion
    }
}
