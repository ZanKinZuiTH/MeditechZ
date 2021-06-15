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
    public class SearchPatientBillViewModel : MediTechViewModelBase
    {
        #region Propoties
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public string RequestNo { get; set; }

        public List<HealthOrganisationModel> Organisations { get; set; }

        private HealthOrganisationModel _SelectOrganisation;
        public HealthOrganisationModel SelectOrganisation
        {
            get { return _SelectOrganisation; }
            set
            {
                Set(ref _SelectOrganisation, value);
            }
        }

        private List<PayorDetailModel> _PayorDetails;
        public List<PayorDetailModel> PayorDetails
        {
            get { return _PayorDetails; }
            set { Set(ref _PayorDetails, value); }
        }

        private PayorDetailModel _SelectPayorDetail;
        public PayorDetailModel SelectPayorDetail
        {
            get { return _SelectPayorDetail; }
            set{Set(ref _SelectPayorDetail, value); }
        }

        private List<PatientBillModel> _PatientBillSource;
        public List<PatientBillModel> PatientBillSource
        {
            get { return _PatientBillSource; }
            set { Set(ref _PatientBillSource, value); }
        }

        private ObservableCollection<PatientBillModel> _SelectPatientBill;
        public ObservableCollection<PatientBillModel> SelectPatientBill
        {
            get
            {
                return _SelectPatientBill
                    ?? (_SelectPatientBill = new ObservableCollection<PatientBillModel>());
            }
            set { _SelectPatientBill = value; }
        }

        private ObservableCollection<GroupReceiptPatientBillModel> _PatientBillGroup;
        public ObservableCollection<GroupReceiptPatientBillModel> PatientBillGroup
        {
            get { return _PatientBillGroup; }
            set { Set(ref _PatientBillGroup, value); }
        }

        private GroupReceiptPatientBillModel _SelectPatientBillGroup;
        public GroupReceiptPatientBillModel SelectPatientBillGroup
        {
            get { return _SelectPatientBillGroup; }
            set { _SelectPatientBillGroup = value; }
        }

        private string _BillNumber;
        public string BillNumber
        {
            get { return _BillNumber; }
            set { Set(ref _BillNumber, value); }
        }
        private double ? _UnitPrice;
        public double ? UnitPrice
        {
            get { return _UnitPrice; }
            set { Set(ref _UnitPrice, value); }
        }

      

        #endregion

        #region Command

        private RelayCommand _SearchCommand;
        public RelayCommand SearchCommand
        {
            get { return _SearchCommand ?? (_SearchCommand = new RelayCommand(SearchPatientBill)); }
        }

        private RelayCommand _AddCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return _AddCommand
                    ?? (_AddCommand = new RelayCommand(AddGroupResult));
            }
        }

        private RelayCommand _CancelCommand;
        public RelayCommand CancelCommand
        {
            get { return _CancelCommand ?? (_CancelCommand = new RelayCommand(Cancel)); }
        }

        private RelayCommand _SaveCommand;
        public RelayCommand SaveCommand
        {
            get { return _SaveCommand ?? (_SaveCommand = new RelayCommand(Save)); }
        }

        private RelayCommand _RemoveCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return _RemoveCommand
                    ?? (_RemoveCommand = new RelayCommand(RemoveGroupResult));
            }
        }
        #endregion

        #region Method

        public SearchPatientBillViewModel()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now;
            PayorDetails = DataService.MasterData.GetPayorDetail();
            Organisations = GetHealthOrganisationRoleMedical();
            SelectOrganisation = Organisations.FirstOrDefault(p => p.HealthOrganisationUID == AppUtil.Current.OwnerOrganisationUID);

        }

        public void Save()
        {
            if (PatientBillGroup == null || PatientBillGroup.Count <= 0)
            {
                WarningDialog("กรุณาเลือกใบ Invoice อย่างน้อย 1 รายการ");
                return;
            }
            CloseViewDialog(ActionDialog.Save);
        }

        public void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void SearchPatientBill()
        {
            long? patientUID = null;

            int? ownerOrganisationUID = (SelectOrganisation != null && SelectOrganisation.HealthOrganisationUID != 0) ? SelectOrganisation.HealthOrganisationUID : (int?)null;
            PatientBillSource = DataService.Billing.SearchPatientBill(DateFrom, DateTo, patientUID, BillNumber, ownerOrganisationUID);
        }

        public void AddGroupResult()
        {

            if(SelectPatientBill != null)
            {
                if (PatientBillGroup == null)
                    PatientBillGroup = new ObservableCollection<GroupReceiptPatientBillModel>();

                foreach (var item in SelectPatientBill)
                {
                    if (PatientBillGroup.FirstOrDefault(p => p.PatientBillUID == item.PatientBillUID) == null)
                    {
                        GroupReceiptPatientBillModel newPatBill = new GroupReceiptPatientBillModel();
                        newPatBill.PatientBillUID = item.PatientBillUID;
                        newPatBill.PatientID = item.PatientID;
                        newPatBill.PatientName = item.PatientName;
                        newPatBill.BillNumber = item.BillNumber;
                        newPatBill.BillGeneratedDttm = item.BillGeneratedDttm;
                        newPatBill.TotalAmount = item.TotalAmount;
                        newPatBill.DiscountAmount = item.DiscountAmount;
                        newPatBill.NetAmount = item.NetAmount;
                        newPatBill.Comments = item.Comments;
                        newPatBill.PayorName = item.PayorName;
                        newPatBill.OrganisationName = item.OrganisationName;
                        newPatBill.CancelReason = item.CancelReason;
                        PatientBillGroup.Add(newPatBill);
                    }
                }

            }
        }

        public void RemoveGroupResult()
        {
            if (SelectPatientBillGroup != null)
            {
                PatientBillGroup.Remove(SelectPatientBillGroup);
            }
        }
        #endregion
    }
}
