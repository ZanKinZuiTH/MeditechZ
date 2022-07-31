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
        public LocationModel SelectedLocation { get; set; }

        private BedStatusModel _SelectBedData;

        public BedStatusModel SelectBedData
        {
            get { return _SelectBedData; }
            set
            {
                Set(ref _SelectBedData, value);
            }
        }
        

        private List<BedStatusModel> _Bed;

        public List<BedStatusModel> Bed
        {
            get { return _Bed; }
            set { Set(ref _Bed, value); }
        }

        public string PatientName { get; set; }
        public string PatientID { get; set; }

        #endregion

        #region Command
        private RelayCommand _NewPatientCommand;

        public RelayCommand NewPatientCommand
        {
            get { return _NewPatientCommand ?? (_NewPatientCommand = new RelayCommand(NewPatient)); }
        }

        private RelayCommand _PatientRecordsCommand;

        public RelayCommand PatientRecordsCommand
        {
            get { return _PatientRecordsCommand ?? (_PatientRecordsCommand = new RelayCommand(PatientRecords)); }
        }

        private RelayCommand _CheckoutCommand;

        public RelayCommand CheckoutCommand
        {
            get { return _CheckoutCommand ?? (_CheckoutCommand = new RelayCommand(Checkout)); }
        }

        private RelayCommand _CreateOrderCommand;

        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
        }
        
        private RelayCommand _PrintDocumentCommand;

        public RelayCommand PrintDocumentCommand
        {
            get { return _PrintDocumentCommand ?? (_PrintDocumentCommand = new RelayCommand(PrintDocument)); }
        }
        
        private RelayCommand _VitalSignCommand;

        public RelayCommand VitalSignCommand
        {
            get { return _VitalSignCommand ?? (_VitalSignCommand = new RelayCommand(VitalSign)); }
        }

        private RelayCommand _ManageAEAdmissionCommand;

        public RelayCommand ManageAEAdmissionCommand
        {
            get { return _ManageAEAdmissionCommand ?? (_ManageAEAdmissionCommand = new RelayCommand(ManageAEAdmission)); }
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
            SelectedLocation = DataService.Technical.GetLocationByTypeUID(3160).FirstOrDefault();

            Bed = DataService.PatientIdentity.GetBedByPatientVisit(SelectedLocation.LocationUID).ToList();

        }

        private void PatientRecords()
        {
            if (SelectBedData != null)
            {
                PatientVisitModel visitModel = new PatientVisitModel();
                visitModel.PatientID = SelectBedData.PatientID;
                visitModel.PatientUID = SelectBedData.PatientUID;
                visitModel.PatientVisitUID = SelectBedData.PatientVisitUID ?? 0;
                EMRView pageview = new EMRView();
                (pageview.DataContext as EMRViewViewModel).AssignPatientVisit(visitModel);
                EMRViewViewModel result = (EMRViewViewModel)LaunchViewDialog(pageview, "EMRVE", false, true);
            }
        }

        private void PrintDocument()
        {
            if (SelectBedData != null)
            {
                PatientVisitModel visitModel = new PatientVisitModel();
                visitModel.PatientID = SelectBedData.PatientID;
                visitModel.PatientUID = SelectBedData.PatientUID;
                visitModel.PatientVisitUID = SelectBedData.PatientVisitUID ?? 0;
                visitModel.OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID;
                ShowModalDialogUsingViewModel(new RunPatientReports(), new RunPatientReportsViewModel() { SelectPatientVisit = visitModel}, true);
            }
        }

        private void ManageAEAdmission()
        {

            if (SelectBedData != null)
            {
                PatientAEAdmissionModel erVisit = DataService.PatientIdentity.GetPatientAEAdmissionByUID(SelectBedData.PatientVisitUID ?? 0);

                EmergencyRegister pageview = new EmergencyRegister();
                (pageview.DataContext as EmergencyRegisterViewModel).AssingModel(new PatientInformationModel(), erVisit);
                EmergencyRegisterViewModel result = (EmergencyRegisterViewModel)LaunchViewDialog(pageview, "ERREG", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    //SearchPatientVisit();
                }
                Bed = DataService.PatientIdentity.GetBedByPatientVisit(SelectedLocation.LocationUID).ToList();
            }
        }

        private void VitalSign()
        {
            if (SelectBedData != null)
            {
                PatientVisitModel visitModel = new PatientVisitModel();
                visitModel.PatientID = SelectBedData.PatientID;
                visitModel.PatientUID = SelectBedData.PatientUID;
                visitModel.PatientVisitUID = SelectBedData.PatientVisitUID ?? 0;
                PatientVitalSign pageview = new PatientVitalSign();
                (pageview.DataContext as PatientVitalSignViewModel).AssingPatientVisit(visitModel);
                PatientVitalSignViewModel result = (PatientVitalSignViewModel)LaunchViewDialog(pageview, "PTVAT", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    //SearchPatientVisit();
                }
            }

        }

        private void CreateOrder()
        {
            if (SelectBedData != null)
            {
                PatientVisitModel visitModel = new PatientVisitModel();
                visitModel.PatientID = SelectBedData.PatientID;
                visitModel.PatientUID = SelectBedData.PatientUID;
                visitModel.PatientVisitUID = SelectBedData.PatientVisitUID ?? 0;
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(visitModel);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);
            }
        }


        private void Checkout()
        {
            if(SelectBedData != null)
            {
                PatientVisitModel visitModel = new PatientVisitModel();
                visitModel.PatientID = SelectBedData.PatientID;
                visitModel.PatientUID = SelectBedData.PatientUID;
                visitModel.PatientVisitUID = SelectBedData.PatientVisitUID ?? 0;
                visitModel.AEAdmissionUID = SelectBedData.AEAdmissionUID ?? 0;
                visitModel.LocationUID = SelectBedData.LocationUID;
                visitModel.BedUID = SelectBedData.BedUID;

                AECheckout pageview = new AECheckout();
                (pageview.DataContext as AECheckoutViewModel).AssingAEAdmission(visitModel);
                AECheckoutViewModel result = (AECheckoutViewModel)LaunchViewDialog(pageview, "AECEKOT", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                    //SearchPatientVisit();
                }

                Bed = DataService.PatientIdentity.GetBedByPatientVisit(SelectedLocation.LocationUID).ToList();
            }
        }

        public void NewPatient()
        {

            EmergencyRegister register = new EmergencyRegister();
            (register.DataContext as EmergencyRegisterViewModel).AssingModel(null, null,SelectBedData.Name);
            ChangeViewPermission(register);

            //(register.DataContext as EmergencyRegisterViewModel).OpenPage(PageRegister.Manage, new PatientInformationModel(),null,false,SelectBedData.SevereID);
            //ChangeViewPermission(register);

            //ChangeView_CloseViewDialog(SelectBedData, ActionDialog.Cancel);

            //ManageReceipt receipt = new ManageReceipt();
            //var data = DataService.Purchaseing.GetGroupReceiptByUID(SelectGroupReceipt.GroupReceiptUID);
            //(receipt.DataContext as ManageReceiptViewModel).AssignModel(data);
            //ChangeViewPermission(receipt);

            #endregion
        }
    }
}
