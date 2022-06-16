using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
  public  class IPDMedicalDischargeViewModel : MediTechViewModelBase
    {
        #region properties
        

        public List<CareproviderModel> Doctors { get; set; }

        private CareproviderModel _SelectDoctors;
        public CareproviderModel SelectDoctors
        {
            get { return _SelectDoctors; }
            set { Set(ref _SelectDoctors, value); }
        }

        #endregion



        #region methor

        public IPDMedicalDischargeViewModel()
        {
            //ExpectedAdmission = DateTime.Now;
            //WardSource = DataService.Technical.GetLocationByTypeUID(3152);
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            // SelectAdmissionCareprovider = AdmissionCareprovider.Find(p => p.CareproviderUID == AppUtil.Current.UserID);
            //SpecialitySource = DataService.MasterData.GetSpecialityAll();
            //RequestedDoctor = DataService.UserManage.GetCareproviderDoctor();
            //LocationDepartment = DataService.Technical.GetLocationByTypeUID(3159);
            //LenghtofDay = "1";
        }

        #endregion

    }
}
