using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class CheckupSummaryViewModel : MediTechViewModelBase
    {
        #region Properties
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
            set { Set(ref _SelectBranch, value); }
        }

        #endregion

        #region Command

        #endregion

        #region Method

        public CheckupSummaryViewModel()
        {
            PayorDetails = DataService.MasterData.GetPayorDetail();

        }
        #endregion
    }
}
