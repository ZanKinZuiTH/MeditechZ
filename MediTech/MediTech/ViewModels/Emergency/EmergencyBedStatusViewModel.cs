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
    public class EmergencyBedStatusViewModel : MediTechViewModelBase
    {
        #region Properties
        private List<BedStatusModel> _BedData;

        public List<BedStatusModel> BedData
        {
            get { return _BedData; }
            set { Set(ref _BedData, value); }
        }

        private List<PatientEmergencyModel> _ListPatientEmergency;

        public List<PatientEmergencyModel> ListPatientEmergency
        {
            get { return _ListPatientEmergency; }
            set { Set(ref _ListPatientEmergency, value); }
        }

        private PatientEmergencyModel _SelectListPatientEmergency;
        public PatientEmergencyModel SelectListPatientEmergency
        {
            get { return _SelectListPatientEmergency; }
            set { Set(ref _SelectListPatientEmergency, value); }
        }

        private BedStatusModel _SelectBedData;

        public BedStatusModel SelectBedData
        {
            get { return _SelectBedData; }
            set { Set(ref _SelectBedData, value); }
        }

        #endregion

        #region Command
        private RelayCommand _NewPatientCommand;

        public RelayCommand NewPatientCommand
        {
            get { return _NewPatientCommand ?? (_NewPatientCommand = new RelayCommand(NewPatient)); }
        }

        private RelayCommand _ChangeStatusCommand;

        public RelayCommand ChangeStatusCommand
        {
            get { return _ChangeStatusCommand ?? (_ChangeStatusCommand = new RelayCommand(ChangeStatus)); }
        }

        private RelayCommand _SendToCommand;

        public RelayCommand SendToCommand
        {
            get { return _SendToCommand ?? (_SendToCommand = new RelayCommand(SendDeparmentt)); }
        }

        #endregion

        #region Method
        public EmergencyBedStatusViewModel()
        {
            //Triage.Add(new LookupReferenceValueModel { Key = 1, DomainCode = "TRIAGE", ValueCode = "TRIAGE1", Display = "สีแดง(emergency/immediate)" });
            //Triage.Add(new LookupReferenceValueModel { Key = 2, DomainCode = "TRIAGE", ValueCode = "TRIAGE2", Display = "สีเหลือง(urgent)" });
            //Triage.Add(new LookupReferenceValueModel { Key = 3, DomainCode = "TRIAGE", ValueCode = "TRIAGE3", Display = "สีเขียว(delayed)" });
            //Triage.Add(new LookupReferenceValueModel { Key = 4, DomainCode = "TRIAGE", ValueCode = "TRIAGE4", Display = "สีฟ้า(expectant)" });
            //Triage.Add(new LookupReferenceValueModel { Key = 5, DomainCode = "TRIAGE", ValueCode = "TRIAGE5", Display = "สีดำ(dead)" });

            ListPatientEmergency = new List<PatientEmergencyModel> { 
                new PatientEmergencyModel { No = 1, FirstName = "Test1", LastName = "LastName1" },
                new PatientEmergencyModel { No = 2, FirstName = "Test2", LastName = "LastName2" },
            };
            BedData = new List<BedStatusModel> {
                new BedStatusModel { No = 1, SevereID = 4, Severe = "สีฟ้า(expectant)", SevereCode = "TRIAGE4" , Status = "ว่าง", IsUsed = false, IsLevel0 = true, IsLevel1 = false, IsLevel2 = false, IsLevel3 = false, },
                new BedStatusModel { No = 2, SevereID = 3,Severe = "สีเขียว(delayed)",SevereCode = "TRIAGE3" , Status = "ไม่ว่าง", IsUsed = true, IsLevel0 = false, IsLevel1 = true, IsLevel2 = false, IsLevel3 = false, FirstName = "Test1", LastName = "LastName1" , PatientName = "Kim numjoon"},
                new BedStatusModel { No = 3, SevereID = 1,Severe = "สีแดง(emergency/immediate)",SevereCode = "TRIAGE1" , Status = "ไม่ว่าง", IsUsed = true, IsLevel0 = false, IsLevel1 = false, IsLevel2 = false, IsLevel3 = true, FirstName = "Test2", LastName = "LastName2" , PatientName = "Min yoongi"}
            };
            List<BedStatusModel> DataItem = new List<BedStatusModel>();
            foreach (var item in BedData)
            {
                BedStatusModel dd = new BedStatusModel();
                if (item.Status == "ไม่ว่าง")
                {
                    dd.Status = item.Status;
                }
            }
        }

        private void ChangeStatus()
        {
            //if (SelectPatientVisit != null)
            //{
            //    var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
            //    if (patientVisit.VISTSUID == CHKOUT || patientVisit.VISTSUID == FINDIS || patientVisit.VISTSUID == CANCEL)
            //    {
            //        WarningDialog("ไม่สามารถดำเนินการได้ เนื่องจากสถานะของ Visit ปัจจุบัน");
            //        SelectPatientVisit.VISTSUID = patientVisit.VISTSUID;
            //        SelectPatientVisit.VisitStatus = patientVisit.VisitStatus;
            //        OnUpdateEvent();
            //        return;
            //    }
            //    PatientStatus sendToDoctor = new PatientStatus(SelectPatientVisit, PatientStatusType.SendToDoctor);
            //    sendToDoctor.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //    sendToDoctor.Owner = MainWindow;
            //    sendToDoctor.ShowDialog();
            //    ActionDialog result = sendToDoctor.ResultDialog;
            //    if (result == ActionDialog.Save)
            //    {
            //        SaveSuccessDialog();
            //        SearchPatientVisit();
            //    }
            //}
            string type = "Change Status";
            ChangeStatus sendToDoctor = new ChangeStatus(SelectBedData, type);
            sendToDoctor.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            sendToDoctor.Owner = MainWindow;
            sendToDoctor.ShowDialog();
            ActionDialog result = sendToDoctor.ResultDialog;
            if (result == ActionDialog.Save)
            {
                SaveSuccessDialog();
                //SearchPatientVisit();
            }
        }
        private void SendDeparmentt()
        {
            SendToDepartment toDoepartment = new SendToDepartment(SelectBedData);
            toDoepartment.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            toDoepartment.Owner = MainWindow;
            toDoepartment.ShowDialog();
            ActionDialog result = toDoepartment.ResultDialog;
            if (result == ActionDialog.Save)
            {
                SaveSuccessDialog();
                //SearchPatientVisit();
            }
        }

        public void NewPatient()
        {
            EmergencyRegister register = new EmergencyRegister();
            (register.DataContext as EmergencyRegisterViewModel).OpenPage(PageRegister.Manage, new PatientInformationModel(),null,false,SelectBedData.SevereID);
            ChangeViewPermission(register);

            //ChangeView_CloseViewDialog(SelectBedData, ActionDialog.Cancel);
        }
            #endregion
    }
}
