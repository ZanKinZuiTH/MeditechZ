using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Model.Report;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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

        private List<InsuranceCompanyModel> _InsuranceCompanyDetails;

        public List<InsuranceCompanyModel> InsuranceCompanyDetails
        {
            get { return _InsuranceCompanyDetails; }
            set { Set(ref _InsuranceCompanyDetails, value); }
        }

        private DateTime? _DateFrom;

        public DateTime? DateFrom
        {
            get { return _DateFrom; }
            set { Set(ref _DateFrom, value); }
        }

        private DateTime? _DateTo;

        public DateTime? DateTo
        {
            get { return _DateTo; }
            set { Set(ref _DateTo, value); }
        }

        private InsuranceCompanyModel _SelectInsuranceCompanyDetail;

        public InsuranceCompanyModel SelectInsuranceCompanyDetail
        {
            get { return _SelectInsuranceCompanyDetail; }
            set
            {
                Set(ref _SelectInsuranceCompanyDetail, value);
                if (_SelectInsuranceCompanyDetail != null)
                {
                    CheckupJobContactList = DataService.Checkup.GetCheckupJobContactByPayorDetailUID(_SelectInsuranceCompanyDetail.InsuranceCompanyUID);
                    SelectCheckupJobContact = CheckupJobContactList.OrderByDescending(p => p.StartDttm).FirstOrDefault();
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

        private List<CheckupSummaryModel> _OccMedSummeryData;

        public List<CheckupSummaryModel> OccMedSummeryData
        {
            get { return _OccMedSummeryData; }
            set { Set(ref _OccMedSummeryData, value); }
        }

        private int? _StartRow = 1;

        public int? StartRow
        {
            get { return _StartRow; }
            set { _StartRow = value; }
        }

        private int? _EndRow = 1000;

        public int? EndRow
        {
            get { return _EndRow; }
            set { _EndRow = value; }
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

        private RelayCommand _PreviewCheckupSummaryCommand;

        public RelayCommand PreviewCheckupSummaryCommand
        {
            get
            {
                return _PreviewCheckupSummaryCommand
                    ?? (_PreviewCheckupSummaryCommand = new RelayCommand(PreviewCheckupSummary));
            }
        }


        private RelayCommand _PreviewOccMedSummaryCommand;

        public RelayCommand PreviewOccMedSummaryCommand
        {
            get
            {
                return _PreviewOccMedSummaryCommand
                    ?? (_PreviewOccMedSummaryCommand = new RelayCommand(PreviewOccMedSummary));
            }
        }

        private RelayCommand _PreviewCheckupGroupCommand;

        public RelayCommand PreviewCheckupGroupCommand
        {
            get
            {
                return _PreviewCheckupGroupCommand
                    ?? (_PreviewCheckupGroupCommand = new RelayCommand(PreviewCheckupGroup));
            }
        }



        #endregion

        #region Method

        public CheckupSummaryViewModel()
        {
            InsuranceCompanyDetails = DataService.Billing.GetInsuranceCompanyAll();

            //#if DEBUG
            //            SelectPayorDetail = PayorDetails.FirstOrDefault(p => p.PayorDetailUID == 1229);
            //            SelectCheckupJobContact = CheckupJobContactList.FirstOrDefault(p => p.CheckupJobContactUID == 1);
            //#endif
        }


        void LoadData()
        {
            string branchName = string.Empty;
            string gprstUIDs = string.Empty;

            if (SelectCheckupJobContact == null)
            {
                WarningDialog("กรุณาเลือก Job");
                return;
            }

            if (SelectBranch != null)
            {
                branchName = SelectBranch.Display;
            }

            foreach (var item in CheckupJobTasks)
            {
                if (item.IsSelected)
                {
                    gprstUIDs += string.IsNullOrEmpty(gprstUIDs) ? item.GPRSTUID.ToString() : "," + item.GPRSTUID;
                }

            }
            CheckupCompanyModel branchData = new CheckupCompanyModel();
            branchData.CheckupJobUID = SelectCheckupJobContact.CheckupJobContactUID;
            branchData.GPRSTUIDs = gprstUIDs;
            branchData.CompanyName = branchName;
            branchData.DateFrom = DateFrom;
            branchData.DateTo = DateTo;
            var dataSummeryData = DataService.Reports.CheckupSummary(branchData);
            if (dataSummeryData != null && dataSummeryData.Count > 0)
            {
                CheckupSummayData = dataSummeryData.Where(p => p.GPRSTUID != 3200 && p.GPRSTUID != 3201 && p.GPRSTUID != 3208).ToList();
                OccMedSummeryData = dataSummeryData.Where(p => p.GPRSTUID == 3200 || p.GPRSTUID == 3201 || p.GPRSTUID == 3208).ToList();
            }
            else
            {
                CheckupSummayData = null;
                OccMedSummeryData = null;
            }

        }

        void PreviewOccMedSummary()
        {
            if (SelectCheckupJobContact != null)
            {
                string gprstUIDs = string.Empty;
                foreach (var item in CheckupJobTasks)
                {
                    if (item.IsSelected && (item.GPRSTUID == 3200 || item.GPRSTUID == 3201 || item.GPRSTUID == 3208))
                    {
                        gprstUIDs += string.IsNullOrEmpty(gprstUIDs) ? item.GPRSTUID.ToString() : "," + item.GPRSTUID;
                    }

                }
                string title = string.Empty;
                if (SelectBranch != null)
                {
                    title = SelectBranch.Display;
                }
                else
                {
                    title = SelectCheckupJobContact.CompanyName;
                }



                Reports.Statistic.Checkup.CheckupOccMedCheckList rpt = new Reports.Statistic.Checkup.CheckupOccMedCheckList();
                rpt.Parameters["Title"].Value = title;
                rpt.Parameters["CheckupJobUID"].Value = SelectCheckupJobContact.CheckupJobContactUID;
                rpt.Parameters["CompanyName"].Value = SelectBranch != null ? SelectBranch.Display : null;
                rpt.Parameters["GPRSTUIDs"].Value = gprstUIDs;
                rpt.Parameters["Year"].Value = (SelectCheckupJobContact.StartDttm.Year + 543);
                rpt.Parameters["DateFrom"].Value = DateFrom;
                rpt.Parameters["DateTo"].Value = DateTo;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                printTool.ShowPreviewDialog();
            }
        }
        void PreviewCheckupSummary()
        {
            if (SelectCheckupJobContact != null)
            {
                string gprstUIDs = string.Empty;
                foreach (var item in CheckupJobTasks)
                {
                    if (item.IsSelected && (item.GPRSTUID != 3200 && item.GPRSTUID != 3201 && item.GPRSTUID != 3208))
                    {
                        gprstUIDs += string.IsNullOrEmpty(gprstUIDs) ? item.GPRSTUID.ToString() : "," + item.GPRSTUID;
                    }

                }
                string title = string.Empty;
                if (SelectBranch != null)
                {
                    title = SelectBranch.Display;
                }
                else
                {
                    title = SelectCheckupJobContact.CompanyName;
                }



                Reports.Statistic.Checkup.CheckupGroupCheckList rpt = new Reports.Statistic.Checkup.CheckupGroupCheckList();
                rpt.Parameters["Title"].Value = title;
                rpt.Parameters["CheckupJobUID"].Value = SelectCheckupJobContact.CheckupJobContactUID;
                rpt.Parameters["CompanyName"].Value = SelectBranch != null ? SelectBranch.Display : null;
                rpt.Parameters["GPRSTUIDs"].Value = gprstUIDs;
                rpt.Parameters["Year"].Value = (SelectCheckupJobContact.StartDttm.Year + 543);
                rpt.Parameters["DateFrom"].Value = DateFrom;
                rpt.Parameters["DateTo"].Value = DateTo;
                rpt.RequestParameters = false;
                rpt.ShowPrintMarginsWarning = false;
                ReportPrintTool printTool = new ReportPrintTool(rpt);
                printTool.ShowPreviewDialog();
            }
        }
        void PreviewCheckupGroup()
        {
            if (SelectCheckupJobContact != null)
            {
                string companyName = (SelectBranch != null && !string.IsNullOrEmpty(SelectBranch.Display)) ? SelectBranch.Display : SelectCheckupJobContact.CompanyName;
                foreach (var item in CheckupJobTasks)
                {
                    if (item.IsSelected)
                    {
                        CheckupCompanyModel branchData = new CheckupCompanyModel();
                        branchData.CheckupJobUID = SelectCheckupJobContact.CheckupJobContactUID;
                        branchData.GPRSTUID = item.GPRSTUID;
                        branchData.CompanyName = SelectBranch != null ? SelectBranch.Display : null;
                        branchData.StartRow = StartRow;
                        branchData.EndRow = EndRow;
                        branchData.DateFrom = DateFrom;
                        branchData.DateTo = DateTo;
                        List<PatientResultCheckupModel> resultData = DataService
                            .Checkup.GetCheckupGroupResultByJob(branchData);

                        var patientData = resultData.GroupBy(p => new
                        {
                            p.RowNumber,
                            p.PatientID,
                            p.EmployeeID,
                            p.Title,
                            p.FirstName,
                            p.LastName,
                            p.Department,
                            p.Age,
                            p.Gender,
                            p.Conclusion,
                            p.CheckupResultStatus,
                            p.StartDttm
                        }).Select(g => new
                        {
                            RowNumber = g.FirstOrDefault().RowNumber,
                            PatientID = g.FirstOrDefault().PatientID,
                            EmployeeID = g.FirstOrDefault().EmployeeID,
                            Title = g.FirstOrDefault().Title,
                            FirstName = g.FirstOrDefault().FirstName,
                            LastName = g.FirstOrDefault().LastName,
                            Department = g.FirstOrDefault().Department,
                            Age = g.FirstOrDefault().Age,
                            Gender = g.FirstOrDefault().Gender,
                            Conclusion = g.FirstOrDefault().Conclusion,
                            CheckupResultStatus = g.FirstOrDefault().CheckupResultStatus,
                            StartDttm = g.FirstOrDefault().StartDttm
                        });

                        List<CheckupGroupReportModel> reportDataSource = new List<CheckupGroupReportModel>();
                        int i = 1;
                        foreach (var patient in patientData)
                        {
                            CheckupGroupReportModel newObject = new CheckupGroupReportModel();
                            newObject.No = patient.RowNumber;
                            newObject.EmployeeID = patient.EmployeeID;
                            newObject.PatientID = patient.PatientID;
                            newObject.Title = patient.Title;
                            newObject.FirstName = patient.FirstName;
                            newObject.LastName = patient.LastName;
                            newObject.Fullname = patient.Title + ' ' + patient.FirstName +' '+ patient.LastName;
                            newObject.Age = patient.Age;
                            newObject.Conclusion = patient.Conclusion;
                            newObject.ResultStatus = patient.CheckupResultStatus;
                            newObject.Gender = patient.Gender;
                            newObject.StartDate = patient.StartDttm;
                            if (patient.Gender == "ชาย (Male)")
                            {
                                newObject.GenderCode = "M";
                            }
                            if (patient.Gender == "หญิง (Female)")
                            {
                                newObject.GenderCode = "F";
                            }
                            reportDataSource.Add(newObject);
                        }

                        foreach (var result in resultData)
                        {
                            var rowData = reportDataSource.Where(p => p.PatientID == result.PatientID
                            && p.FirstName == result.FirstName).FirstOrDefault();
                            if (rowData != null)
                            {
                                PropertyInfo properties = null;
                                if (item.GPRSTUID == 3177 || item.GPRSTUID == 3178)
                                {
                                    if (result.ResultItemName != null)
                                        properties = rowData.GetType().GetProperty(result.ResultItemName);
                                }
                                else
                                {
                                    if (result.ResultItemCode != null)
                                        properties = rowData.GetType().GetProperty(result.ResultItemCode);
                                }
                                if (properties != null)
                                {
                                    string value = !string.IsNullOrEmpty(result.IsAbnormal) ? result.ResultValue + " " + result.IsAbnormal : result.ResultValue;

                                    properties.SetValue(rowData, value);
                                }

                            }
                        }

                        var myReport = Activator.CreateInstance(Type.GetType("MediTech.Reports.Statistic.Checkup." + item.ReportTemplate));
                        Reports.Statistic.Checkup.CheckupGroupBase report = (Reports.Statistic.Checkup.CheckupGroupBase)myReport;
                        report.lbTitle.Text = companyName + Environment.NewLine + item.GroupResultName;
                        report.DataSource = reportDataSource;
                        ReportPrintTool printTool = new ReportPrintTool(report);
                        report.ShowPrintMarginsWarning = false;
                        printTool.ShowPreviewDialog();

                        item.IsSelected = false;
                    }

                }
            }

        }

        #endregion
    }
}
