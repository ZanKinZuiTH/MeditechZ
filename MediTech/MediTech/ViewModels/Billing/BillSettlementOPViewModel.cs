using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using MediTech.Views.Billing;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class BillSettlementOPViewModel : MediTechViewModelBase
    {
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
                if (_SelectedPateintSearch != null)
                {
                    PatientVisitModel visitInfoNonClose = DataService.PatientIdentity.GetLatestPatientVisitNonClose(_SelectedPateintSearch.PatientUID);
                    if (visitInfoNonClose != null)
                    {
                        SelectPatientVisit = visitInfoNonClose;
                    }
                }
            }
        }

        #endregion

        private Visibility _IsPatientSearchEnabled = Visibility.Visible;

        public Visibility IsPatientSearchEnabled
        {
            get { return _IsPatientSearchEnabled; }
            set { Set(ref _IsPatientSearchEnabled, value); }
        }

        private PatientVisitModel _SelectPatientVisit;

        public PatientVisitModel SelectPatientVisit
        {
            get { return _SelectPatientVisit; }
            set
            {
                Set(ref _SelectPatientVisit, value);
                if (_SelectPatientVisit != null)
                {
                    DateFrom = _SelectPatientVisit.StartDttm;
                }
            }

        }

        private List<PatientVisitPayorModel> _PatientVisitPayors;

        public List<PatientVisitPayorModel> PatientVisitPayors
        {
            get { return _PatientVisitPayors; }
            set { Set(ref _PatientVisitPayors, value); }
        }


        private PatientVisitPayorModel _SelectPatientVisitPayor;

        public PatientVisitPayorModel SelectPatientVisitPayor
        {
            get { return _SelectPatientVisitPayor; }
            set { Set(ref _SelectPatientVisitPayor, value); }
        }

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

        private bool _IsExpanded;

        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                Set(ref _IsExpanded, value);
                CollpaseExpand();
            }
        }

        private ObservableCollection<AllocatedPatBillableItemsResultModel> _AllocatedPatientBillableItems;

        public ObservableCollection<AllocatedPatBillableItemsResultModel> AllocatedPatientBillableItems
        {
            get { return _AllocatedPatientBillableItems; }
            set
            {
                Set(ref _AllocatedPatientBillableItems, value);
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

        private RelayCommand _AutoAllocateCommand;

        public RelayCommand AutoAllocateCommand
        {
            get { return _AutoAllocateCommand ?? (_AutoAllocateCommand = new RelayCommand(AutoAllocate)); }
        }

        private RelayCommand _ClearCommand;

        public RelayCommand ClearCommand
        {
            get { return _ClearCommand ?? (_ClearCommand = new RelayCommand(Clear)); }
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

        private RelayCommand _GeneratebillCommand;
        public RelayCommand GeneratebillCommand
        {
            get { return _GeneratebillCommand ?? (_GeneratebillCommand = new RelayCommand(Generatebill)); }
        }

        #endregion

        #region Method

        public BillSettlementOPViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            if (SelectPatientVisit != null)
            {
                GetVisitPayors(SelectPatientVisit.PatientVisitUID);
            }
            Search();
        }

        private void Search()
        {
            if (SelectPatientVisit != null)
            {
                Reload();
            }

        }

        private void AutoAllocate()
        {
            CallAllocation("F", allocatedVisitPayorUID: -1);
            if (SelectPatientVisitPayor != null)
            {
                CallAllocation("Y", SelectPatientVisitPayor.PatientVisitPayorUID, SelectPatientVisitPayor.PayorAgreementUID);
            }
            else
            {
                foreach (var visitPayor in PatientVisitPayors)
                {
                    CallAllocation("Y", visitPayor.PatientVisitPayorUID, visitPayor.PayorAgreementUID);
                }
            }
            Reload();
        }

        private void Clear()
        {
            CallAllocation("F", allocatedVisitPayorUID: -1);
            Reload();
        }

        public void callAllocatedMergeReceipt(long lAllocatedPatBillableItemUID)
        {
            AllocatedPatBillableItemsResultModel selectedAllocatedPatBillableItem;

            if (lAllocatedPatBillableItemUID > 0 && AllocatedPatientBillableItems != null && AllocatedPatientBillableItems.Count() > 0
&& AllocatedPatientBillableItems.Where(p => p.PatientBillableItemUID == lAllocatedPatBillableItemUID) != null
&& AllocatedPatientBillableItems.Where(p => p.PatientBillableItemUID == lAllocatedPatBillableItemUID).Count() > 0)
            {

                selectedAllocatedPatBillableItem = AllocatedPatientBillableItems.FirstOrDefault(p => p.PatientBillableItemUID == lAllocatedPatBillableItemUID);
                if (selectedAllocatedPatBillableItem != null && string.IsNullOrEmpty(selectedAllocatedPatBillableItem.PayorName))
                {
                    WarningDialog("ไม่มี Payor สำหรับการ Allocate");
                    return;
                }
                MergeBillRecipetPopup mergeBillRecipet = new MergeBillRecipetPopup();
                ((mergeBillRecipet.DataContext as MergeBillRecipetPopupViewModel)).AssignMergeRecipet(SelectPatientVisit.PatientUID, SelectPatientVisit.PatientVisitUID, selectedAllocatedPatBillableItem, AllocatedPatientBillableItems.ToList(), PatientVisitPayors, DateFrom ?? SelectPatientVisit.StartDttm.Value, DateTo ?? DateTime.Now);
            MergeBillRecipetPopupViewModel modelResult = (MergeBillRecipetPopupViewModel)LaunchViewDialogNonPermiss(mergeBillRecipet, true);
                if (modelResult.ResultDialog == ActionDialog.Save)
                {
                    Reload();
                }
            }
        }

        public void CallAllocatedBillableItem(long lAllocatedPatBillableItemUID)
        {
            AllocatedPatBillableItemsResultModel selectedAllocatedPatBillableItem;
            List<AllocatedPatBillableItemsResultModel> oSubGroupPatBillableItems;
            List<AllocatedPatBillableItemsResultModel> oGroupPatBillableItems;
            List<AllocatedPatBillableItemsResultModel> oBillableItems;

            if (lAllocatedPatBillableItemUID > 0 && AllocatedPatientBillableItems != null && AllocatedPatientBillableItems.Count() > 0
    && AllocatedPatientBillableItems.Where(p => p.PatientBillableItemUID == lAllocatedPatBillableItemUID) != null
    && AllocatedPatientBillableItems.Where(p => p.PatientBillableItemUID == lAllocatedPatBillableItemUID).Count() > 0)
            {

                selectedAllocatedPatBillableItem = AllocatedPatientBillableItems.FirstOrDefault(p => p.PatientBillableItemUID == lAllocatedPatBillableItemUID);
                if (selectedAllocatedPatBillableItem != null &&  string.IsNullOrEmpty(selectedAllocatedPatBillableItem.PayorName))
                {
                    WarningDialog("ไม่มี Payor สำหรับการ Allocate");
                    return;
                }


                oSubGroupPatBillableItems = new List<AllocatedPatBillableItemsResultModel>(AllocatedPatientBillableItems.Where(p => p.SubAccountUID == selectedAllocatedPatBillableItem.SubAccountUID && p.GroupUID == selectedAllocatedPatBillableItem.GroupUID
                    && p.PatientVisitPayorUID == selectedAllocatedPatBillableItem.PatientVisitPayorUID));
                oGroupPatBillableItems = new List<AllocatedPatBillableItemsResultModel>(AllocatedPatientBillableItems.Where(p => p.GroupUID == selectedAllocatedPatBillableItem.GroupUID
                    && p.PatientVisitPayorUID == selectedAllocatedPatBillableItem.PatientVisitPayorUID));
                oBillableItems = new List<AllocatedPatBillableItemsResultModel>(AllocatedPatientBillableItems.Where(p => p.BillableItemUID == selectedAllocatedPatBillableItem.BillableItemUID
                    && p.PatientVisitPayorUID == selectedAllocatedPatBillableItem.PatientVisitPayorUID));


                AllocateBillPopup allocatePop = new AllocateBillPopup();
                (allocatePop.DataContext as AllocateBillPopupViewModel).AssignAllocatedBillableItem(selectedAllocatedPatBillableItem, oSubGroupPatBillableItems, oGroupPatBillableItems
                    , oBillableItems, PatientVisitPayors, SelectPatientVisit.PatientUID, SelectPatientVisit.PatientVisitUID, DateFrom ?? SelectPatientVisit.StartDttm.Value, DateTo ?? DateTime.Now);
                AllocateBillPopupViewModel modelResult = (AllocateBillPopupViewModel)LaunchViewDialogNonPermiss(allocatePop, true);
                if (modelResult.ResultDialog == ActionDialog.Save)
                {
                    Reload();
                }

            }
        }

        private void CallAllocation(string cAllocationType, long? patientVisitPayorUID = null, int? payorAgreementUID = null, int? allocatedVisitPayorUID = null, int? patientBillableItemUID = null,
            int? groupUID = null, string canKeepDiscount = null)
        {
            AllocatePatientBillableItemModel allocateModel = new AllocatePatientBillableItemModel();
            allocateModel.PatientUID = SelectPatientVisit.PatientUID;
            allocateModel.PatientVisitUID = SelectPatientVisit.PatientVisitUID;
            allocateModel.IsAutoAllocate = cAllocationType;
            allocateModel.PatientVisitPayorUID = patientVisitPayorUID;
            allocateModel.PayorAgreementUID = payorAgreementUID;
            allocateModel.UserUID = AppUtil.Current.UserID;
            allocateModel.AllocatedVisitPayorUID = allocatedVisitPayorUID;
            allocateModel.PatientBillableItemUID = patientBillableItemUID;
            allocateModel.GroupUID = groupUID;
            allocateModel.CanKeepDiscount = canKeepDiscount;
            allocateModel.StartDate = DateFrom ?? SelectPatientVisit.StartDttm.Value;
            allocateModel.EndDate = DateTo ?? DateTime.Now;
            DataService.Billing.AllocatePatientBillableItem(allocateModel);
        }

        private void Reload()
        {
            var allocatedBillableItems = (DataService.Billing.GetAllocatedPatBillableItemsPalm(SelectPatientVisit.PatientUID, SelectPatientVisit.PatientVisitUID, null, null
                , null, null, DateFrom ?? DateTime.Now, DateTo ?? DateTime.Now
                ));
            AllocatedPatientBillableItems = new ObservableCollection<AllocatedPatBillableItemsResultModel>(allocatedBillableItems.OrderByDescending(p => p.PatientBillableItemUID));
            CollpaseExpand();
        }
        
        private void GetVisitPayors(long patientVisitUID)
        {
            PatientVisitPayors = DataService.PatientIdentity.GetPatientVisitPayorByVisitUID(patientVisitUID);
        }

        public void AssingPatientVisit(PatientVisitModel patientVisit)
        {
            SelectPatientVisit = patientVisit;
            IsPatientSearchEnabled = Visibility.Collapsed;
            GetVisitPayors(SelectPatientVisit.PatientVisitUID);
        }

        private void CollpaseExpand()
        {
            if (IsExpanded)
                ((BillSettlementOP)View).Expand();
            else
                ((BillSettlementOP)View).Collapse();
        }


        private void CreateOrder()
        {
            if (SelectPatientVisit != null)
            {
                PatientOrderEntry pageview = new PatientOrderEntry();
                (pageview.DataContext as PatientOrderEntryViewModel).AssingPatientVisit(SelectPatientVisit);
                PatientOrderEntryViewModel result = (PatientOrderEntryViewModel)LaunchViewDialog(pageview, "ORDITM", false, true);
            }
        }


        private void ModifyVisitPayor()
        {
            if (SelectPatientVisit != null)
            {
                var patientVisit = DataService.PatientIdentity.GetPatientVisitByUID(SelectPatientVisit.PatientVisitUID);
                ModifyVisitPayor pageview = new ModifyVisitPayor();
                (pageview.DataContext as ModifyVisitPayorViewModel).AssingPatientVisit(SelectPatientVisit);
                ModifyVisitPayorViewModel result = (ModifyVisitPayorViewModel)LaunchViewDialog(pageview, "MODPAY", true);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    GetVisitPayors(SelectPatientVisit.PatientVisitUID);
                }
            }
        }

        private void Generatebill()
        {
            if (SelectPatientVisit != null)
            {
                GenerateBill pageview = new GenerateBill();
                var viewModel = (pageview.DataContext as GenerateBillViewModel);
                viewModel.AssingGenerateBill(SelectPatientVisit);
                GenerateBillViewModel result = (GenerateBillViewModel)LaunchViewDialogNonPermiss(pageview, false);
                if (result != null && result.ResultDialog == ActionDialog.Save)
                {
                    ResultDialog = ActionDialog.Save;
                }
            }
        }


        public void PatientSearch()
        {
            string patientID = string.Empty;
            string firstName = string.Empty;
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
                List<PatientInformationModel> searchResult = DataService.PatientIdentity.SearchPatient(patientID, firstName, "", lastName, "", null, null, "", null, "");
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
