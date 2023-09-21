using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class AssignDoctorGPPopupViewModel : MediTechViewModelBase
    {
        #region Properties

        public List<CareproviderModel> Doctors { get; set; }
        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { _SelectDoctor = value; }
        }


        private ObservableCollection<CareproviderModel> _CareproviderLists;

        public ObservableCollection<CareproviderModel> CareproviderLists
        {
            get { return _CareproviderLists ?? new ObservableCollection<CareproviderModel>(); }
            set { _CareproviderLists = value; }
        }

        private CareproviderModel _SelectCareprovider;

        public CareproviderModel SelectCareprovider
        {
            get { return _SelectCareprovider; }
            set { _SelectCareprovider = value; }
        }

        #endregion

        #region Command

        private RelayCommand _AddDoctorCommand;

        public RelayCommand AddDoctorCommand
        {
            get
            {
                return _AddDoctorCommand
                    ?? (_AddDoctorCommand = new RelayCommand(AddDoctor));
            }
        }

        private RelayCommand _RemoveDoctorCommand;

        public RelayCommand RemoveDoctorCommand
        {
            get
            {
                return _RemoveDoctorCommand
                    ?? (_RemoveDoctorCommand = new RelayCommand(RemoveDoctor));
            }
        }


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
            }
        }



        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }

        #endregion

        #region Method

        public AssignDoctorGPPopupViewModel()
        {
            Doctors = DataService.UserManage.GetCareproviderDoctor();
        }

        void AddDoctor()
        {
            if (SelectDoctor != null)
            {
                if (CareproviderLists.FirstOrDefault(p => p.CareproviderUID == SelectCareprovider.CareproviderUID) != null)
                {
                    WarningDialog("ชื่อแพทย์ ซ้ำ");
                    return;
                }

                CareproviderLists.Add(SelectDoctor);
            }
        }

        void RemoveDoctor()
        {
            if (SelectCareprovider != null)
            {

                CareproviderLists.Remove(SelectCareprovider);
            }
        }

        void Save()
        {
            try
            {
                if (QuestionDialog("คุณต้องการบันทึกใช้หรื่อไม่") == System.Windows.MessageBoxResult.Yes)
                {
                    CloseViewDialog(ActionDialog.Save);
                }
         
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion

    }
}
