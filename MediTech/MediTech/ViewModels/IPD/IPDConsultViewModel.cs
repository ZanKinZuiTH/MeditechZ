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
   public class IPDConsultViewModel : MediTechViewModelBase
    {

        #region Properties

        #region Varible
        public PatientVisitModel patientVisitModel;
        public IPDConsultModel consultDetail { get; set; }

        public List<IPDConsultModel> cousultDelete;

        #endregion

        private PatientVisitModel _SelectPatientVisit;
        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set { Set(ref _SelectPatientVisit, value); }
        }


        private List<IPDConsultModel> _deletedConsultlist;


        private DateTime? _StartDateConsult;
        public DateTime? StartDateConsult
        {
            get { return _StartDateConsult; }
            set { Set(ref _StartDateConsult, value); }
        }

        private DateTime? _StartTimeConsult;
        public DateTime? StartTimeConsult
        {
            get { return _StartTimeConsult; }
            set { Set(ref _StartTimeConsult, value); }
        }




        private DateTime? _StampDate;
        public DateTime? StampDate
        {
            get { return _StampDate; }
            set { Set(ref _StampDate, value); }
        }

        private DateTime? _StampTime;
        public DateTime? StampTime
        {
            get { return _StampTime; }
            set { Set(ref _StampTime, value); }
        }


        private DateTime? _EndDateConsult;
        public DateTime? EndDateConsult
        {
            get { return _EndDateConsult; }
            set { Set(ref _EndDateConsult, value); }
        }

        private DateTime? _EndTimeConsult;
        public DateTime? EndTimeConsult
        {
            get { return _EndTimeConsult; }
            set { Set(ref _EndTimeConsult, value); }
        }

        private string _Comments;
        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private ObservableCollection<IPDConsultModel> _IPDConsultList;
        public ObservableCollection<IPDConsultModel> IPDConsultList
        {
            get { return _IPDConsultList; }
            set { Set(ref _IPDConsultList, value); }


        }

        private IPDConsultModel _SelectPatientConsult;
        public IPDConsultModel SelectPatientConsult
        {
            get { return _SelectPatientConsult; }
            //set { Set(ref _SelectIPDConsultList, value); }
            set {
                _SelectPatientConsult = value;
                if (_SelectPatientConsult != null)
                {
                    SelectConsultStatus = cmbConsultStatus.FirstOrDefault(p => p.Key == _SelectPatientConsult.CONSTSUID);
                    SelectDoctor = Doctors.FirstOrDefault(p => p.CareproviderUID == _SelectPatientConsult.CareProviderUID);


                }

              

            }
        }






        private DateTime _SendConsultDate;
        public DateTime SendConsultDate
        {
            get { return _SendConsultDate; }
            set { Set(ref _SendConsultDate, value); }
        }

        private DateTime? _SendConsultTime;
        public DateTime? SendConsultTime
        { 
            get { return _SendConsultTime; }
            set { Set(ref _SendConsultTime, value); }
        }



        private List<LookupReferenceValueModel> _cmbConsultType;

        public List<LookupReferenceValueModel> cmbConsultType
        {
            get { return _cmbConsultType; }
            set { Set(ref _cmbConsultType, value); }
        }


        private LookupReferenceValueModel _SelectCousultType;
        public LookupReferenceValueModel SelectConsultType
        {
            get { return _SelectCousultType; }
            set { Set(ref _SelectCousultType, value); }
        }


        private List<LookupReferenceValueModel> _cmbConsultStatus;

        public List<LookupReferenceValueModel> cmbConsultStatus
        {
            get { return _cmbConsultStatus; }
            set { Set(ref _cmbConsultStatus, value); }
        }


        private LookupReferenceValueModel _SelectCousultStatus;
        public LookupReferenceValueModel SelectConsultStatus
        {
            get { return _SelectCousultStatus; }
            set { Set(ref _SelectCousultStatus, value); }
        }


        public List<CareproviderModel> Doctors { get; set; }
        private CareproviderModel _SelectDoctor;

        public CareproviderModel SelectDoctor
        {
            get { return _SelectDoctor; }
            set { _SelectDoctor = value; }
        }

    


        #endregion

        #region Command



        private RelayCommand _AddCommand;

        public RelayCommand AddCommand
        {
            get { return _AddCommand ?? (_AddCommand = new RelayCommand(Add)); }
        }


        //private RelayCommand _UpdateConsultCommand;

        //public RelayCommand UpdateConsultCommand
        //{
        //    get { return _AddCommand ?? (_AddCommand = new RelayCommand(UpdateVisitConsult)); }
        //}


        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }


        #endregion

        #region Method



        public IPDConsultViewModel()
        {
            //BookingStatus = DataService.Technical.GetReferenceValueByCode("VISTS", "REGST");
            //var org = GetLocatioinRole(AppUtil.Current.OwnerOrganisationUID);
            Doctors = DataService.UserManage.GetCareproviderDoctor();
            cmbConsultType = DataService.Technical.GetReferenceValueMany("CONTYP");
            cmbConsultStatus = DataService.Technical.GetReferenceValueMany("CONSTAT");
            StartDateConsult = DateTime.Now;
            StartTimeConsult = DateTime.Now;
            EndDateConsult = DateTime.Now;
            EndTimeConsult = DateTime.Now;

            // var requestData = DataService.PatientIdentity.GetAppointmentRequestbyUID(patientUID, patientVisitUID, bkstsUID);
           //IPDConsultList = new ObservableCollection<IPDConsultModel>();
        }


        void LoadPatientVisitConsult()
        {
            var patientvisitconsult = DataService.InPatientService.GetPatientVisitConsult(SelectPatientConsult.PatientVisitUID??0);
            //var patientVisitPayors = DataService.PatientIdentity.GetPatientVisitPayorByVisitUID(PatientVisit.PatientVisitUID);
            //PatientInsuranceDetails = patientInsuranceDetails;
            //PatientVisitPayorList = new ObservableCollection<PatientVisitPayorModel>(patientVisitPayors);
            //_deletedVisitPayorList = new List<PatientVisitPayorModel>();
        }



        public void AssignData(PatientVisitModel model)
        {

           
            SelectPatientVisit = model;
            StartDateConsult = DateTime.Today;
            EndDateConsult = DateTime.Today;
           

        }

        private void Add()
        {
            if (IPDConsultList == null)
                IPDConsultList = new ObservableCollection<IPDConsultModel>();

            if (SelectDoctor == null)
            {
                WarningDialog("กรุณาเลือก แพทย์ที่ต้องการ consult");
                return;
            }
            if (SelectConsultStatus == null)
            {
                WarningDialog("กรุณาเลือก Status Consult");
                return;
            }

            if (SelectConsultType == null)
            {
                WarningDialog("กรุณาเลือก Type Consult");
                return;
            }

            if (StartDateConsult == null)
            {
                WarningDialog("กรุณาเลือกเลือกวันและช่วงเวลาที่ต้องการ consult");
                return;
            }
            if (StartTimeConsult == null)
            {
                WarningDialog("กรุณาเลือกเลือกวันและช่วงเวลาที่ต้องการ consult");
                return;
            }
            if (EndDateConsult == null)
            {
                WarningDialog("กรุณาเลือกเลือกวันและช่วงเวลาที่ต้องการ consult");
                return;
            }

            if (EndTimeConsult == null)
            {
                WarningDialog("กรุณาเลือกเลือกวันและช่วงเวลาที่ต้องการ consult");
                return;
            }

            AssignToGrid();

            if (consultDetail != null)
            {
                IPDConsultList.Add(consultDetail);
            }
        }




        void UpdateVisitPayor()
        {
            if (SelectPatientConsult != null)
            {

                SelectPatientConsult.CareProviderUID = SelectDoctor.CareproviderUID;
                SelectPatientConsult.CareProviderName = SelectDoctor.FullName;
                SelectPatientConsult.CONSTSUID = SelectConsultStatus.Key;
                SelectPatientConsult.CONTYPUID = SelectConsultType.Key;
                SelectPatientConsult.Note = Comments;
                SelectPatientConsult.StartConsultDate = DateTime.Parse(StartDateConsult.Value.ToString("dd/MM/yyyy") + " " + StartTimeConsult.Value.ToString("HH:mm"));
                SelectPatientConsult.EndConsultDate = DateTime.Parse(EndDateConsult.Value.ToString("dd/MM/yyyy") + " " + EndTimeConsult.Value.ToString("HH:mm"));
                SelectPatientConsult.StatusFlag = "A";
                SelectPatientConsult.CUser = AppUtil.Current.UserID;
                SelectPatientConsult.MUser = AppUtil.Current.UserID;
                SelectPatientConsult.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
               // ClearControl();
               // SelectedPatientVisitPayor = null;
            }
        }




        void DeleteVisitPayor()
        {
            if (SelectPatientConsult != null)
            {
                SelectPatientConsult.StatusFlag = "D";
                _deletedConsultlist.Add(SelectPatientConsult);
                IPDConsultList.Remove(SelectPatientConsult);

            }
        }



        private void Save()
        {
            try
            {
                if (IPDConsultList == null || IPDConsultList.Count <= 0)
                {
                    WarningDialog("กรุณาใส่ข้อมูล Consult");
                }


                var listconsultdata = IPDConsultList.ToList();
                //if (_deletedVisitPayorList != null)
                //PateintVisitPayorDatas.AddRange(_deletedVisitPayorList);
                DataService.InPatientService.InsertPatientConsult(listconsultdata, AppUtil.Current.UserID);
                //DataService.InPatientService.InsertPatientConsult(listconsultdata, AppUtil.Current.UserID);
              

               // DataService.PatientIdentity.ManagePatientInsuranceDetail(PateintVisitPayorDatas, AppUtil.Current.UserID);
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }




        private void AssignToGrid()
        {
           consultDetail = new IPDConsultModel();
           
 
            consultDetail.StartConsultDate = DateTime.Parse(StartDateConsult.Value.ToString("dd/MM/yyyy") + " " + StartTimeConsult.Value.ToString("HH:mm"));
            consultDetail.EndConsultDate = DateTime.Parse(EndDateConsult.Value.ToString("dd/MM/yyyy") + " " + EndTimeConsult.Value.ToString("HH:mm"));
            consultDetail.CONSTSUID = SelectConsultStatus.Key;
            consultDetail.CONTYPUID = SelectConsultType.Key;
            consultDetail.ConultStatusStr = SelectConsultStatus.Display;
            consultDetail.ConultTypeStr = SelectConsultType.Display;
            consultDetail.Note = Comments;

            consultDetail.PatientUID = SelectPatientVisit.PatientUID;
            consultDetail.VISTSUID = SelectPatientVisit.VISTSUID;
            consultDetail.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
            consultDetail.PatientName = SelectPatientVisit.PatientName;
            consultDetail.PatientID = SelectPatientVisit.PatientID;
            consultDetail.VisitID = SelectPatientVisit.VisitID;

            consultDetail.CareProviderUID = SelectDoctor != null ? SelectDoctor.CareproviderUID : (int?)null;
             consultDetail.CareProviderName = SelectDoctor != null ? Doctors.FirstOrDefault(p => p.CareproviderUID == SelectDoctor.CareproviderUID).FullName : null;
            consultDetail.CUser = AppUtil.Current.UserID;
              consultDetail.MUser = AppUtil.Current.UserID;
              consultDetail.StatusFlag = "A";
              consultDetail.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;

        }
        #endregion
    }
}
