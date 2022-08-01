using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediTech.Model;
using MediTech.Views;
using MediTech.Models;
using System.Collections.ObjectModel;

namespace MediTech.ViewModels
{
    public class ListQueryBillsOPViewModel : MediTechViewModelBase
    {

        int CHKOUT = 418;
        int BLINP = 423;

        #region Properties

        #region PatientSearch

        private string _SearchPatientCriteria;

        public string SearchPatientCriteria
        {
            get { return _SearchPatientCriteria; }
            set
            {
                Set(ref _SearchPatientCriteria, value);
                PatientsSearchSource = null;
            }
        }


        private List<PatientInformationModel> _PatientsSearchSource;

        public List<PatientInformationModel> PatientsSearchSource
        {
            get { return _PatientsSearchSource; }
            set { Set(ref _PatientsSearchSource, value); }
        }

        private PatientInformationModel _SelectedPateintSearch;

        public PatientInformationModel SelectedPateintSearch
        {
            get { return _SelectedPateintSearch; }
            set
            {
                _SelectedPateintSearch = value;
            }
        }




        private ObservableCollection<UnbilledPatientsResult> _UnBilledPatientLists;
        public ObservableCollection<UnbilledPatientsResult> UnBilledPatientLists
        {
            get { return _UnBilledPatientLists; }
            set { Set(ref _UnBilledPatientLists, value); }
        }

        private UnbilledPatientsResult _SelectUnBilledPatient;

        public UnbilledPatientsResult SelectUnBilledPatient
        {
            get { return _SelectUnBilledPatient; }
            set { Set(ref _SelectUnBilledPatient, value); }
        }

        #endregion

        private DateTime? _DateFrom;
        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set
            {
                Set(ref _DateFrom, value);
            }
        }

        private DateTime? _DateTo;
        public DateTime? DateTo
        {
            get { return _DateTo; }
            set
            {
                Set(ref _DateTo, value);
            }
        }


        #endregion

        #region Command


        private RelayCommand _PatientSearchCommand;

        public RelayCommand PatientSearchCommand
        {
            get { return _PatientSearchCommand ?? (_PatientSearchCommand = new RelayCommand(PatientSearch)); }
        }


        private RelayCommand _SearchCommand;

        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(Search)); }
        }


        private RelayCommand _LockCommand;

        public RelayCommand LockCommand
        {
            get { return _LockCommand ?? (_LockCommand = new RelayCommand(Lock)); }
        }



        private RelayCommand _UnLockCommand;

        public RelayCommand UnLockCommand
        {
            get { return _UnLockCommand ?? (_UnLockCommand = new RelayCommand(Unlock)); }
        }


        private RelayCommand _AllocateBillCommand;

        public RelayCommand AllocateBillCommand
        {
            get { return _AllocateBillCommand ?? (_AllocateBillCommand = new RelayCommand(AllocateBill)); }
        }


        private RelayCommand _CreateOrderCommand;

        public RelayCommand CreateOrderCommand
        {
            get { return _CreateOrderCommand ?? (_CreateOrderCommand = new RelayCommand(CreateOrder)); }
        }

        private RelayCommand _ModifyVisitPayorCommand;
        public RelayCommand ModifyVisitPayorCommand
        {
            get { return _ModifyVisitPayorCommand ?? (_ModifyVisitPayorCommand = new RelayCommand(ModifyVisitPayor)); }
        }

        private RelayCommand _PatientTrackingCommand;
        public RelayCommand PatientTrackingCommand
        {
            get { return _PatientTrackingCommand ?? (_PatientTrackingCommand = new RelayCommand(PatientTracking)); }
        }


        private RelayCommand _CleanCommand;

        public RelayCommand CleanCommand
        {
            get { return _CleanCommand ?? (_CleanCommand = new RelayCommand(Clean)); }
        }

        #endregion

        #region Method

        public ListQueryBillsOPViewModel()
        {
            DateFrom = DateTime.Now;
            Search();
        }


        void Search()
        {
            long? patientUID = null;
            UnBilledPatientLists = null;
            if (SelectedPateintSearch != null && SearchPatientCriteria != "")
            {
                patientUID = SelectedPateintSearch.PatientUID;
            }
            UnBilledPatientLists = new ObservableCollection<UnbilledPatientsResult>();
            var patientVisitList = DataService.Billing.SearchUnbilledPatients(patientUID, DateFrom, DateTo, AppUtil.Current.OwnerOrganisationUID, "N");
            foreach (var patVisit in patientVisitList)
            {
                UnbilledPatientsResult newPatient = new UnbilledPatientsResult();
                newPatient.PatientUID = patVisit.PatientUID;
                newPatient.PatientVisitUID = patVisit.PatientVisitUID;
                newPatient.PatientName = patVisit.PatientName;
                newPatient.PatientID = patVisit.PatientID;
                newPatient.VisitID = patVisit.VisitID;
                newPatient.OwnerOrganisationUID = patVisit.OwnerOrganisationUID;
                newPatient.StartDttm = patVisit.StartDttm;
                newPatient.VisitStatus = patVisit.VisitStatus;
                newPatient.LocationUID = patVisit.LocationUID;
                newPatient.VISTSUID = patVisit.VISTSUID;
                newPatient.ENTYPUID = patVisit.ENTYPUID;
                newPatient.LocationName = patVisit.LocationName;
                newPatient.DischargeDttm = patVisit.DischargeDttm;
                newPatient.DischargedUser = patVisit.DischargedUser;
                newPatient.Gender = patVisit.Gender;
                newPatient.BloodGroup = patVisit.BloodGroup;
                newPatient.CareProviderName = patVisit.CareProviderName;
                newPatient.PatientAddress = patVisit.PatientAddress;
                newPatient.SecondPhone = patVisit.SecondPhone;
                newPatient.MobilePhone = patVisit.MobilePhone;
                newPatient.UnlockVisibility = patVisit.VisitStatus == "Billing Inprogress" ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
                newPatient.LockVisibility = newPatient.UnlockVisibility == System.Windows.Visibility.Visible ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;


                UnBilledPatientLists.Add(newPatient);
            }
          
        }

        void Clean()
        {
            DateFrom = DateTime.Now;
            DateTo = null;
            SearchPatientCriteria = string.Empty;
            UnBilledPatientLists = null;
        }

        private void Lock()
        {
            try
            {
                if (SelectUnBilledPatient != null)
                {
                    DataService.PatientIdentity.ChangeVisitStatus(SelectUnBilledPatient.PatientVisitUID, BLINP, SelectUnBilledPatient.CareProviderUID, SelectUnBilledPatient.LocationUID, DateTime.Now, AppUtil.Current.UserID, null, null);
                    SelectUnBilledPatient.VISTSUID = BLINP;
                    SelectUnBilledPatient.VisitStatus = "Billing Inprogress";
                    SelectUnBilledPatient.UnlockVisibility = System.Windows.Visibility.Visible;
                    SelectUnBilledPatient.LockVisibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void Unlock()
        {
            try
            {
                if (SelectUnBilledPatient != null)
                {
                    DataService.PatientIdentity.ChangeVisitStatus(SelectUnBilledPatient.PatientVisitUID, CHKOUT, SelectUnBilledPatient.CareProviderUID, SelectUnBilledPatient.LocationUID, DateTime.Now, AppUtil.Current.UserID, null, null);
                    SelectUnBilledPatient.VISTSUID = CHKOUT;
                    SelectUnBilledPatient.VisitStatus = "Medical Discharge";
                    SelectUnBilledPatient.UnlockVisibility = System.Windows.Visibility.Collapsed;
                    SelectUnBilledPatient.LockVisibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        private void AllocateBill()
        {
            if (SelectUnBilledPatient != null)
            {
                if (SelectUnBilledPatient.VISTSUID == CHKOUT)
                {
                  var resultDiaglog =  QuestionDialog("สถานะยังไม่ได้ทำการ Lock Bill กด \"Yes\" เพื่อเปลี่ยนสถานะเป็น Lock แล้วดำเนินการ Allocate Bill ต่อไป");
                    if (resultDiaglog == System.Windows.MessageBoxResult.Cancel || resultDiaglog == System.Windows.MessageBoxResult.No)
                    {
                        return;
                    }
                    Lock();
                }
                BillSettlementOP pageview = new BillSettlementOP();
                (pageview.DataContext as BillSettlementOPViewModel).AssingPatientVisit(SelectUnBilledPatient);
                BillSettlementOPViewModel result = (BillSettlementOPViewModel)LaunchViewDialog(pageview, "BLSETTLEOP", false, true);
                Search();
            }
        }

        private void CreateOrder()
        {
            if (SelectUnBilledPatient != null)
            {
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(SelectUnBilledPatient);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);
            }
        }

        private void PatientTracking()
        {
            if (SelectUnBilledPatient != null)
            {
                PatientTracking pageview = new PatientTracking();
                (pageview.DataContext as PatientTrackingViewModel).AssingModel(SelectUnBilledPatient);
                PatientTrackingViewModel result = (PatientTrackingViewModel)LaunchViewDialog(pageview, "PATRCK", false);
            }

        }

        private void ModifyVisitPayor()
        {
            if (SelectUnBilledPatient != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectUnBilledPatient.PatientVisitUID);
                ModifyVisitPayor pageview = new ModifyVisitPayor();
                (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(SelectUnBilledPatient);
                ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    SaveSuccessDialog();
                }
            }
        }

        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty; ;
            string lastName = string.Empty;
            if (SearchPatientCriteria.Length >= 3)
            {
                string[] patientName = SearchPatientCriteria.Split(' ');
                if (patientName.Length >= 2)
                {
                    firstName = patientName[0];
                    lastName = patientName[1];
                }
                else
                {
                    int num = 0;
                    foreach (var ch in SearchPatientCriteria)
                    {
                        if (ShareLibrary.CheckValidate.IsNumber(ch.ToString()))
                        {
                            num++;
                        }
                    }
                    if (num >= 5)
                    {
                        patientID = SearchPatientCriteria;
                    }
                    else if (num <= 2)
                    {
                        firstName = SearchPatientCriteria;
                        lastName = "empty";
                    }

                }
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, null);
                PatientsSearchSource = searchResult;
            }
            else
            {

                PatientsSearchSource = null;

            }

        }

        #endregion
    }
}
