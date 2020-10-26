using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class ManageCheckupJobViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _JobNumber;

        public string JobNumber
        {
            get { return _JobNumber; }
            set { Set(ref _JobNumber, value); }
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
                if (SelectPayorDetail != null)
                {
                    CompanyName = SelectPayorDetail.PayorName;
                }
            }
        }

        private string _CompanyName;

        public string CompanyName
        {
            get { return _CompanyName; }
            set { Set(ref _CompanyName, value); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }

        private string _Location;

        public string Location
        {
            get { return _Location; }
            set { Set(ref _Location, value); }
        }

        private string _ContactPerson;

        public string ContactPerson
        {
            get { return _ContactPerson; }
            set { Set(ref _ContactPerson, value); }
        }

        private string _ContactPhone;

        public string ContactPhone
        {
            get { return _ContactPhone; }
            set { Set(ref _ContactPhone, value); }
        }

        private string _ContactEmail;

        public string ContactEmail
        {
            get { return _ContactEmail; }
            set { Set(ref _ContactEmail, value); }
        }


        private List<LookupItemModel> _CheckupService;

        public List<LookupItemModel> CheckupService
        {
            get { return _CheckupService; }
            set { Set(ref _CheckupService, value); }
        }

        private LookupItemModel _SelectCheckupService;

        public LookupItemModel SelectCheckupService
        {
            get { return _SelectCheckupService; }
            set { _SelectCheckupService = value; }
        }

        private int _VisitCount;

        public int VisitCount
        {
            get { return _VisitCount; }
            set { Set(ref _VisitCount, value); }
        }

        private DateTime? _StartDttm;

        public DateTime? StartDttm
        {
            get { return _StartDttm; }
            set { Set(ref _StartDttm, value); }
        }

        private DateTime? _EndDttm;

        public DateTime? EndDttm
        {
            get { return _EndDttm; }
            set { Set(ref _EndDttm, value); }
        }

        private DateTime? _CollectDttm;

        public DateTime? CollectDttm
        {
            get { return _CollectDttm; }
            set { Set(ref _CollectDttm, value); }
        }

        private List<LookupReferenceValueModel> _GroupResults;

        public List<LookupReferenceValueModel> GroupResults
        {
            get { return _GroupResults; }
            set { Set(ref _GroupResults, value); }
        }

        private LookupReferenceValueModel _SelectGroupResult;

        public LookupReferenceValueModel SelectGroupResult
        {
            get { return _SelectGroupResult; }
            set { Set(ref _SelectGroupResult, value); }
        }

        private List<CheckupJobTaskModel> _CheckupJobTask;

        public List<CheckupJobTaskModel> CheckupJobTask
        {
            get { return _CheckupJobTask; }
            set { Set(ref _CheckupJobTask, value); }
        }

        private CheckupJobTaskModel _SelectCheckupJobTask;

        public CheckupJobTaskModel SelectCheckupJobTask
        {
            get { return _SelectCheckupJobTask; }
            set { Set(ref _SelectCheckupJobTask, value); }
        }
        #endregion

        #region Command

        private RelayCommand _SaveCommand;

        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(Save));
            }
        }


        private RelayCommand _CancelCommand;

        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
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

        CheckupJobContactModel modelCheckupJobContact;

        public ManageCheckupJobViewModel()
        {
            PayorDetails = DataService.MasterData.GetPayorDetail();
            GroupResults = DataService.Technical.GetReferenceValueMany("GPRST");
            CheckupService = new List<LookupItemModel>();
            List<LookupItemModel> checkupServices = new List<LookupItemModel>();
            checkupServices.Add(new LookupItemModel { Key = 1, Display = "ตรวจสุขภาพประจำปี" });
            checkupServices.Add(new LookupItemModel { Key = 2, Display = "ตรวจอับอากาศ" });
            checkupServices.Add(new LookupItemModel { Key = 3, Display = "ตรวจตามปัจจัยเสี่ยง" });
            CheckupService.AddRange(checkupServices);
        }
        public void AssingModel(int checkupJobContactUID)
        {
            var modelData = DataService.Checkup.GetCheckupJobContactByUID(checkupJobContactUID);
            modelCheckupJobContact = modelData;
            AssignModelToProperties();
        }


        public void Save()
        {
            if (SelectPayorDetail == null)
            {
                WarningDialog("กรุณาเลือก Payor");
                return;
            }

            if (String.IsNullOrEmpty(CompanyName))
            {
                WarningDialog("กรุณาใส่ชื่อ บริษัท");
                return;
            }

            if (VisitCount == 0)
            {
                WarningDialog("กรุณาระบุจำนวนผู้ตรวจ");
                return;
            }

            if (StartDttm == null)
            {
                WarningDialog("กรุณาระบุวันที่ตรวจ");
                return;
            }

            if (EndDttm == null)
            {
                WarningDialog("กรุณาระบุวันที่ส่งเล่ม");
                return;
            }
        }

        public void Cancel()
        {
            ListCheckupJob page = new ListCheckupJob();
            ChangeViewPermission(page);
        }


        public void AssignModelToProperties()
        {
            JobNumber = modelCheckupJobContact.JobNumber;
            SelectPayorDetail = PayorDetails.FirstOrDefault(p => p.PayorDetailUID == modelCheckupJobContact.PayorDetailUID);
            CompanyName = modelCheckupJobContact.CompanyName;
            Description = modelCheckupJobContact.Description;
            Location = modelCheckupJobContact.Location;
            ContactPerson = modelCheckupJobContact.ContactPerson;
            ContactPhone = modelCheckupJobContact.ContactPhone;
            ContactEmail = modelCheckupJobContact.ContactEmail;
            SelectCheckupService = CheckupService.FirstOrDefault(p => p.Display == modelCheckupJobContact.ServiceName);
            VisitCount = modelCheckupJobContact.VisitCount;
            StartDttm = modelCheckupJobContact.StartDttm;
            EndDttm = modelCheckupJobContact.EndDttm;
            CollectDttm = modelCheckupJobContact.CollectDttm;
            CheckupJobTask = modelCheckupJobContact.CheckupJobTasks;

            if (CheckupJobTask != null && CheckupJobTask.Count > 0)
            {
                foreach (var item in CheckupJobTask)
                {
                    var dataAlredy = GroupResults.FirstOrDefault(p => p.Key == item.GPRSTUID);
                    if (dataAlredy != null)
                    {
                        GroupResults.Remove(dataAlredy);
                    }
                    RefershGrid();
                }
            }
        }

        public void AddGroupResult()
        {
            if (SelectGroupResult != null)
            {
                if (CheckupJobTask == null)
                    CheckupJobTask = new List<CheckupJobTaskModel>();

                CheckupJobTaskModel newCheckupJobTask = new CheckupJobTaskModel();
                newCheckupJobTask.GPRSTUID = SelectGroupResult.Key;
                newCheckupJobTask.GroupResultName = SelectGroupResult.Display;
                newCheckupJobTask.DisplayOrder = (CheckupJobTask.Max(p => p.DisplayOrder) ?? 0) + 1;
                CheckupJobTask.Add(newCheckupJobTask);

                GroupResults.Remove(SelectGroupResult);

                RefershGrid();
            }


        }

        public void RemoveGroupResult()
        {
            if (SelectCheckupJobTask != null)
            {
                LookupReferenceValueModel newLookupRef = new LookupReferenceValueModel();
                newLookupRef.Key = SelectCheckupJobTask.GPRSTUID;
                newLookupRef.Display = SelectCheckupJobTask.GroupResultName;

                GroupResults.Add(newLookupRef);
                CheckupJobTask.Remove(SelectCheckupJobTask);
                RefershGrid();

            }
        }

        void RefershGrid()
        {
            ManageCheckupJob view = (this.View as ManageCheckupJob);
            view.grdJobTask.RefreshData();
            view.grdGroupResult.RefreshData();
        }
        #endregion


    }
}
