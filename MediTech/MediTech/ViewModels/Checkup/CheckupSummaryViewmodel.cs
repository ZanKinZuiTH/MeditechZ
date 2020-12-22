using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediTech.ViewModels
{
    public class CheckupSummaryViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _HeaderText = "ตารางแสดงผลการตรวจสูขภาพประจำปี";

        public string HeaderText
        {
            get { return _HeaderText; }
            set { Set(ref _HeaderText, value); }
        }

        private string _CompanyName;

        public string CompanyName
        {
            get { return _CompanyName; }
            set { Set(ref _CompanyName, value); }
        }

        private string _BranchName;

        public string BranchName
        {
            get { return _BranchName; }
            set { Set(ref _BranchName, value); }
        }

        private Visibility _VisibilityBranch = Visibility.Collapsed;

        public Visibility VisibilityBranch
        {
            get { return _VisibilityBranch; }
            set { Set(ref _VisibilityBranch, value); }
        }


        private bool _SurpassSelectAll = false;
        public bool SurpassSelectAll
        {
            get { return _SurpassSelectAll; }
            set { _SurpassSelectAll = value; }
        }


        private bool? _IsSelectedAll = false;

        public bool? IsSelectedAll
        {
            get { return _IsSelectedAll; }
            set
            {
                Set(ref _IsSelectedAll, value);
                if (!SurpassSelectAll)
                {
                    foreach (var jobTask in CheckupJobTasks)
                    {
                        jobTask.IsSelected = IsSelectedAll ?? false;
                    }
                    OnUpdateEvent();
                }
                SurpassSelectAll = false;
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
            set
            {
                Set(ref _SelectPayorDetail, value);
                if (_SelectPayorDetail != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectPayorDetail.PayorDetailUID);
                }
            }
        }

        private List<CheckupJobContactModel> _CheckupJobContactList;

        public List<CheckupJobContactModel> CheckupJobContactList
        {
            get { return _CheckupJobContactList; }
            set { Set(ref _CheckupJobContactList, value); }
        }

        private CheckupJobContactModel _SelectCheckupJobContact;

        public CheckupJobContactModel SelectCheckupJobContact
        {
            get { return _SelectCheckupJobContact; }
            set
            {
                Set(ref _SelectCheckupJobContact, value);
                if (SelectCheckupJobContact != null)
                {
                    BranchList = DataService.Checkup.GetCompanyBranchByCheckJob(SelectCheckupJobContact.CheckupJobContactUID);
                    CheckupJobTasks = new ObservableCollection<CheckupJobTaskModel>(DataService.Checkup.GetCheckupJobTaskByJobUID(SelectCheckupJobContact.CheckupJobContactUID));
                    HeaderText += " " + (SelectCheckupJobContact.StartDttm.Year + 543);
                    CompanyName = _SelectCheckupJobContact.CompanyName;

                    IsSelectedAll = true;
                }
                else
                {
                    BranchList = null;
                    CheckupJobTasks = null;
                }
            }
        }


        private ObservableCollection<CheckupJobTaskModel> _CheckupJobTasks;

        public ObservableCollection<CheckupJobTaskModel> CheckupJobTasks
        {
            get { return _CheckupJobTasks; }
            set { Set(ref _CheckupJobTasks, value); }
        }

        private CheckupJobTaskModel _SelectCheckupJobTask;

        public CheckupJobTaskModel SelectCheckupJobTask
        {
            get { return _SelectCheckupJobTask; }
            set
            {
                Set(ref _SelectCheckupJobTask, value);
            }
        }

        private List<LookupReferenceValueModel> _BranchList;

        public List<LookupReferenceValueModel> BranchList
        {
            get { return _BranchList; }
            set { Set(ref _BranchList, value); }
        }

        private LookupReferenceValueModel _SelectBranch;

        public LookupReferenceValueModel SelectBranch
        {
            get { return _SelectBranch; }
            set
            {
                Set(ref _SelectBranch, value);
                if (SelectBranch != null)
                {
                    BranchName = "สาขา " + SelectBranch.Display;
                    VisibilityBranch = Visibility.Visible;
                }
                else
                {
                    BranchName = "";
                    VisibilityBranch = Visibility.Collapsed;
                }
            }
        }

        private List<CheckupSummaryModel> _CheckupSummayData;

        public List<CheckupSummaryModel> CheckupSummayData
        {
            get { return _CheckupSummayData; }
            set { Set(ref _CheckupSummayData, value); }
        }


        #endregion

        #region Command

        private RelayCommand _LoadDataCommand;

        public RelayCommand LoadDataCommand
        {
            get
            {
                return _LoadDataCommand
                    ?? (_LoadDataCommand = new RelayCommand(LoadData));
            }
        }

        #endregion

        #region Method

        public CheckupSummaryViewModel()
        {
            PayorDetails = DataService.MasterData.GetPayorDetail();

#if DEBUG
            SelectPayorDetail = PayorDetails.FirstOrDefault(p => p.PayorDetailUID == 1229);
            SelectCheckupJobTask = CheckupJobContactList.FirstOrDefault(p => p.CheckupJobContactUID == 1);
#endif
        }


        void LoadData()
        {
            string branchName = string.Empty;
            string gprstUIs = string.Empty;

            if (SelectCheckupJobContact == null)
            {
                WarningDialog("กรุณาเลือก Job");
                return;
            }

            if (SelectBranch != null)
            {
                BranchName = SelectBranch.Display;
            }

            foreach (var item in CheckupJobTasks)
            {
                gprstUIs += string.IsNullOrEmpty(gprstUIs) ? item.GPRSTUID.ToString() : "," + item.GPRSTUID;
            }

            CheckupSummayData = DataService.Reports.CheckupSummary(SelectCheckupJobContact.CheckupJobContactUID, gprstUIs, branchName);
        }
        #endregion
    }
}
