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
    public class CreateDispenseReturnViewModel : MediTechViewModelBase
    {
        #region Varibale

        int BLINP = 423;
        int FINDIS = 421;

        #endregion

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
                if (SelectedPateintSearch != null)
                {
                    SearchPatientVisit();
                }
            }
        }

        #endregion

        private List<DispenseReturnModel> _DispenseItems;
        public List<DispenseReturnModel> DispenseItems
        {
            get { return _DispenseItems; }
            set { Set(ref _DispenseItems, value); }
        }

        private DispenseReturnModel _SelectDispenseItem;
        public DispenseReturnModel SelectDispenseItem
        {
            get { return _SelectDispenseItem; }
            set { Set(ref _SelectDispenseItem, value); }
        }

        private List<PatientVisitModel> _PatientVisitLists;

        public List<PatientVisitModel> PatientVisitLists
        {
            get { return _PatientVisitLists; }
            set { Set(ref _PatientVisitLists, value); }
        }

        private PatientVisitModel _SelectedPatientVisit;

        public PatientVisitModel SelectedPatientVisit
        {
            get { return _SelectedPatientVisit; }
            set
            {
                Set(ref _SelectedPatientVisit, value);
                if (_SelectedPatientVisit != null)
                {
                    ReturnType = "Dispensed Return (คืนยา คืนเงิน)";
                    IsCanSave = true;
                    if (_SelectedPatientVisit.VISTSUID == BLINP)
                    {
                        WarningDialog("ผู้ป่วยอยู่ในสถานะ Billing in Progress ไม่สามารถ คืนยา ได้");
                        IsCanSave = false;
                    }
                    else if(_SelectedPatientVisit.VISTSUID == FINDIS)
                    {
                        WarningDialog("ผู้ป่วยอยู่ในสถานะ Financial Discharge การคืนยาไม่มีผลต่อค่าใช้จ่าย");
                        ReturnType = "Sale Return (คืนยา แต่ ไม่คืนเงิน)";
                    }
                }
            }
        }

        private DateTime _SelectDispenseDate;

        public DateTime SelectDispenseDate
        {
            get { return _SelectDispenseDate; }
            set { Set(ref _SelectDispenseDate, value); }
        }


        private List<StoreModel> _Stores;
        public List<StoreModel> Stores
        {
            get { return _Stores; }
            set { Set(ref _Stores, value); }
        }

        private StoreModel _SelectStore;
        public StoreModel SelectStore
        {
            get { return _SelectStore; }
            set { Set(ref _SelectStore, value); }
        }

        private string _PrescriptionNumber;

        public string PrescriptionNumber
        {
            get { return _PrescriptionNumber; }
            set { Set(ref _PrescriptionNumber, value); }
        }


        private string _ItemName;

        public string ItemName
        {
            get { return _ItemName; }
            set { Set(ref _ItemName, value); }
        }


        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { Set(ref _Comments, value); }
        }

        private bool? _SelectedAll;

        public bool? SelectedAll
        {
            get { return _SelectedAll; }
            set { Set(ref _SelectedAll, value);
                if (DispenseItems != null && DispenseItems.Count > 0)
                {
                    var dispenseCanReturn = DispenseItems.Where(p => p.IsCanReturn);
                    foreach (var item in dispenseCanReturn)
                    {
                        item.Selected = _SelectedAll ?? false;
                    }
                    (this.View as CreateDispenseReturn).grdItemReturn.RefreshData();
                }
            }
        }


        private bool _IsCanSave;

        public bool IsCanSave
        {
            get { return _IsCanSave; }
            set { Set(ref _IsCanSave, value); }
        }

        private string _ReturnType;

        public string ReturnType
        {
            get { return _ReturnType; }
            set { Set(ref _ReturnType, value); }
        }

        #endregion

        #region Command

        private RelayCommand _PatientSearchCommand;

        /// <summary>
        /// Gets the PatientSearchCommand.
        /// </summary>
        public RelayCommand PatientSearchCommand
        {
            get
            {
                return _PatientSearchCommand
                    ?? (_PatientSearchCommand = new RelayCommand(PatientSearch));
            }
        }


        private RelayCommand _SearchDispenseCommand;
        public RelayCommand SearchDispenseCommand
        {
            get { return _SearchDispenseCommand ?? (_SearchDispenseCommand = new RelayCommand(SearchDispense)); }
        }

        private RelayCommand _ClearCommand;
        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
        }

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _CloseCommand;
        public RelayCommand CloseCommand
        {
            get { return _CloseCommand ?? (_CloseCommand = new RelayCommand(Close)); }
        }
        #endregion

        #region Method
        public CreateDispenseReturnViewModel()
        {


        }

        private void SearchDispense()
        {
            long? patientVisitUID = SelectedPatientVisit != null ? SelectedPatientVisit.PatientVisitUID : (long?)null;
            int? storeUID = SelectStore != null ? SelectStore.StoreUID : (int?)null;
            DispenseItems = null;
            if (patientVisitUID != null)
            {
                DispenseItems = DataService.Inventory.SearchListDispensedItemForReturn(patientVisitUID.Value, storeUID, PrescriptionNumber, ItemName);
            }

        }

        private void Clear()
        {
            SelectedPateintSearch = null;
            SearchPatientCriteria = string.Empty;
            PatientVisitLists = null;
            Stores = null;
            ItemName = null;
            PrescriptionNumber = null;
            ReturnType = string.Empty;

        }
        private void Save()
        {
            try
            {
                if (DispenseItems?.Count(p => p.Selected) == 0)
                {
                    WarningDialog("กรุณาเลือกรายการที่จะคืน");
                    return;
                }

                foreach (var item in DispenseItems)
                {
                    if (item.Selected && (item.ReturnQty ?? 0) == 0)
                    {
                        WarningDialog("กรุณาใส่จำนวนที่ต้องการคืน");
                        return;
                    }
                }

                var dispenseReturn = DispenseItems.Where(p => p.Selected);
                string returnType = SelectedPatientVisit.VISTSUID == FINDIS ? "Sale Return" : "Dispensed Return";
                foreach (var item in dispenseReturn)
                {
                    item.MUser = AppUtil.Current.UserID;
                }
                DataService.Inventory.DispensedReturn(dispenseReturn.ToList(), returnType);

                SaveSuccessDialog();

                DispenseReturns dispense = new DispenseReturns();
                ChangeViewPermission(dispense);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        private void Close()
        {
            DispenseReturns dispense = new DispenseReturns();
            ChangeViewPermission(dispense);
        }




        void SearchPatientVisit()
        {
            long patientUID = 0;
            Stores = null;
            if (!string.IsNullOrEmpty(SearchPatientCriteria))
            {
                if (SelectedPateintSearch != null)
                {
                    patientUID = SelectedPateintSearch.PatientUID;
                }
            }

            PatientVisitLists = DataService.PatientIdentity.GetPatientVisitDispensed(patientUID);


            if (PatientVisitLists == null && PatientVisitLists.Count <= 0 && patientUID != 0)
            {
                WarningDialog("ไม่มีรายการ Dispense ใน HN : " + SelectedPateintSearch.PatientID);
            }

            if (PatientVisitLists != null && PatientVisitLists.Count > 0)
            {

                foreach (var visit in PatientVisitLists)
                {
                    visit.Comments = visit.VisitID + "  " + visit.StartDttm?.ToString("dd/MM/yyyy HH:mm");
                }
                SelectedPatientVisit = PatientVisitLists.FirstOrDefault();
                Stores = DataService.Inventory.GetStoreDispensedByVisitUID(SelectedPatientVisit.PatientVisitUID);
                SelectStore = Stores.FirstOrDefault();


            }

            SearchDispense();
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "", "");
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
