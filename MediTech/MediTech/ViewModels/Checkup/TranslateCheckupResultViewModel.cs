using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using MediTech.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using System.Data;
using System.Windows.Forms;

namespace MediTech.ViewModels
{
    public class TranslateCheckupResultViewModel : MediTechViewModelBase
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
                    CheckupJobTasks = new ObservableCollection<CheckupJobTaskModel>(DataService.Checkup.GetCheckupJobTaskByJobUID(SelectCheckupJobContact.CheckupJobContactUID));
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


        private ObservableCollection<Column> _ColumnsResultItems;

        public ObservableCollection<Column> ColumnsResultItems
        {
            get { return _ColumnsResultItems; }
            set { Set(ref _ColumnsResultItems, value); }
        }
        #endregion

        #region Command

        private RelayCommand _TranslateSpecificCommmand;

        public RelayCommand TranslateSpecificCommmand
        {
            get
            {
                return _TranslateSpecificCommmand
                    ?? (_TranslateSpecificCommmand = new RelayCommand(TranslateSpecific));
            }
        }


        private RelayCommand _TranslateAllCommand;

        public RelayCommand TranslateAllCommand
        {
            get
            {
                return _TranslateAllCommand
                    ?? (_TranslateAllCommand = new RelayCommand(TranslateAll));
            }
        }



        private RelayCommand _LoadDataCommand;

        public RelayCommand LoadDataCommand
        {
            get
            {
                return _LoadDataCommand
                    ?? (_LoadDataCommand = new RelayCommand(LoadData));
            }
        }

        private RelayCommand _ExportDataCommand;

        public RelayCommand ExportDataCommand
        {
            get
            {
                return _ExportDataCommand
                    ?? (_ExportDataCommand = new RelayCommand(ExportData));
            }
        }

        #endregion

        #region Method
        public TranslateCheckupResultViewModel()
        {
            PayorDetails = DataService.MasterData.GetPayorDetail();
        }

        void TranslateSpecific()
        {
            try
            {
                TranslateCheckupResult view = (TranslateCheckupResult)this.View;
                List<int> GPRSTUIDs = CheckupJobTasks.Where(p => p.IsSelected).Select(p => p.GPRSTUID).ToList();
                if (GPRSTUIDs != null && GPRSTUIDs.Count > 0)
                {
                    List<PatientVisitModel> visitData = DataService.Checkup.GetVisitCheckupGroupNonTran(SelectCheckupJobContact.CheckupJobContactUID, GPRSTUIDs);
                    List<CheckupRuleModel> dataCheckupRule = DataService.Checkup.GetCheckupRuleGroupList(GPRSTUIDs);
                    view.SetProgressBarLimits(0, visitData.Count());
                    int loopCount = 0;
                    foreach (var patientVisit in visitData)
                    {
                        TranslateProcess(patientVisit, GPRSTUIDs, dataCheckupRule);

                        loopCount++;
                        view.SetProgressBarValue(loopCount);
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }
        void TranslateAll()
        {
            try
            {
                if (SelectCheckupJobContact != null)
                {
                    TranslateCheckupResult view = (TranslateCheckupResult)this.View;
                    List<int> GPRSTUIDs = CheckupJobTasks.Where(p => p.IsSelected).Select(p => p.GPRSTUID).ToList();
                    if (GPRSTUIDs != null && GPRSTUIDs.Count > 0)
                    {
                        List<PatientVisitModel> visitData = DataService.Checkup.GetVisitCheckupGroup(SelectCheckupJobContact.CheckupJobContactUID, GPRSTUIDs);
                        List<CheckupRuleModel> dataCheckupRule = DataService.Checkup.GetCheckupRuleGroupList(GPRSTUIDs);
                        view.SetProgressBarLimits(0, visitData.Count());
                        int loopCount = 0;
                        foreach (var patientVisit in visitData)
                        {
                            TranslateProcess(patientVisit, GPRSTUIDs, dataCheckupRule);

                            loopCount++;
                            view.SetProgressBarValue(loopCount);
                        }
                    }
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }

        void TranslateProcess(PatientVisitModel patientVisit, List<int> GPRSTUIDs, List<CheckupRuleModel> dataCheckupRule)
        {
            int? ageInt = !string.IsNullOrEmpty(patientVisit.Age) ? int.Parse(patientVisit.Age) : (int?)null;
            foreach (var grpstUID in GPRSTUIDs)
            {
                List<CheckupRuleModel> ruleCheckupIsCorrect = new List<CheckupRuleModel>();
                string wellNessResult = string.Empty;
                List<ResultComponentModel> resultComponent = null;
                if (grpstUID != 3177 && grpstUID != 3178)
                {
                    resultComponent = DataService.Checkup.GetResultComponent(patientVisit.PatientVisitUID, grpstUID);
                }
                else if (grpstUID == 3177)
                {

                }
                else if (grpstUID == 3178)
                {

                }

                if (resultComponent != null)
                {
                    var ruleCheckups = dataCheckupRule
.Where(p => p.GPRSTUID == grpstUID
&& (p.SEXXXUID == 3 || p.SEXXXUID == patientVisit.SEXXXUID)
&& ((p.AgeFrom == null && p.AgeTo == null) || (ageInt > p.AgeFrom && ageInt < p.AgeTo)
|| (ageInt > p.AgeFrom && p.AgeTo == null) || (p.AgeFrom == null && ageInt < p.AgeTo))
).ToList();
                    foreach (var ruleCheckup in ruleCheckups)
                    {
                        foreach (var ruleItem in ruleCheckup.CheckupRuleItem)
                        {
                            var resultItemValue = resultComponent.FirstOrDefault(p => p.ResultItemUID == ruleItem.ResultItemUID);

                            if (resultItemValue != null)
                            {
                                if (!string.IsNullOrEmpty(ruleItem.Text))
                                {
                                    if (resultItemValue.ResultValue.Trim() == ruleItem.Text.Trim())
                                    {

                                        ruleCheckupIsCorrect.Add(ruleCheckup);
                                        if (ruleItem.Operator == "Or")
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    double resultValueNumber;
                                    if (double.TryParse(resultItemValue.ResultValue, out resultValueNumber))
                                    {
                                        if ((resultValueNumber >= ruleItem.Low && resultValueNumber <= ruleItem.Hight)
                                            || (resultValueNumber >= ruleItem.Low && ruleItem.Hight == null)
                                            || (ruleItem.Low == null && resultValueNumber <= ruleItem.Hight)
                                            )
                                        {
                                            ruleCheckupIsCorrect.Add(ruleCheckup);
                                            if (ruleItem.Operator == "Or")
                                            {
                                                break;
                                            }
                                        }
                                    }

                                }
                            }

                        }

                    }



                    int RABSTSUID = 0;
                    List<CheckupRuleDescriptionModel> descriptions = new List<CheckupRuleDescriptionModel>();
                    List<CheckupRuleRecommendModel> recommands = new List<CheckupRuleRecommendModel>();
                    if (ruleCheckupIsCorrect.Any(p => p.RABSTSUID == 2882))
                    {
                        RABSTSUID = 2882;
                        foreach (var item in ruleCheckupIsCorrect.Where(p => p.RABSTSUID != 2883))
                        {
                            descriptions.AddRange(item.CheckupRuleDescription);
                            recommands.AddRange(item.CheckupRuleRecommend);
                        }

                    }
                    else
                    {
                        RABSTSUID = 2883;
                        foreach (var item in ruleCheckupIsCorrect)
                        {
                            descriptions.AddRange(item.CheckupRuleDescription);
                            recommands.AddRange(item.CheckupRuleRecommend);
                        }
                    }

                    var descriptionGroup = descriptions.GroupBy(p => new
                    {
                        p.CheckupTextMasterUID
                    })
                    .Select(g => new
                    {
                        ThaiDescription = g.FirstOrDefault().ThaiDescription,
                        EngDescription = g.FirstOrDefault().EngDescription,
                    });

                    var recommandGroup = recommands.GroupBy(p => new
                    {
                        p.CheckupTextMasterUID
                    }).Select(g => new
                    {
                        ThaiRecommend = g.FirstOrDefault().ThaiRecommend,
                        EndRecommend = g.FirstOrDefault().EndRecommend,
                    });



                    string descriptionString = string.Empty;
                    string recommandString = string.Empty;
                    foreach (var item in descriptionGroup)
                    {
                        descriptionString += string.IsNullOrEmpty(descriptionString) ? item.ThaiDescription : " " + item.ThaiDescription;
                    }

                    foreach (var item in recommandGroup)
                    {
                        recommandString += string.IsNullOrEmpty(recommandString) ? item.ThaiRecommend : " " + item.ThaiRecommend;
                    }

                }
            }

        }

        void LoadData()
        {
            try
            {

                if (SelectCheckupJobTask != null)
                {
                    TranslateCheckupResult view = (TranslateCheckupResult)this.View;
                    DataTable dtResult = new DataTable();
                    dtResult.Clear();
                    dtResult.Columns.Add("RowHandle");
                    dtResult.Columns.Add("PatientID");
                    dtResult.Columns.Add("FirstName");
                    dtResult.Columns.Add("LastName");
                    dtResult.Columns.Add("Department");
                    dtResult.Columns.Add("Age");
                    dtResult.Columns.Add("Gender");

                    ColumnsResultItems = new ObservableCollection<Column>();
                    ColumnsResultItems.Add(new Column() { Header = "No", FieldName = "RowHandle", VisibleIndex = 1 });
                    ColumnsResultItems.Add(new Column() { Header = "PatientID", FieldName = "PatientID", VisibleIndex = 2 });
                    ColumnsResultItems.Add(new Column() { Header = "FirstName", FieldName = "FirstName", VisibleIndex = 3 });
                    ColumnsResultItems.Add(new Column() { Header = "LastName", FieldName = "LastName", VisibleIndex = 4 });
                    ColumnsResultItems.Add(new Column() { Header = "Department", FieldName = "Department", VisibleIndex = 5 });
                    ColumnsResultItems.Add(new Column() { Header = "Age", FieldName = "Age", VisibleIndex = 5 });
                    ColumnsResultItems.Add(new Column() { Header = "Gender", FieldName = "Gender", VisibleIndex = 7 });
                    int visibleIndex = 8;
                    List<PatientResultCheckupModel> resultData = DataService.Checkup
                        .GetResultComponentyByGroup(SelectCheckupJobContact.CheckupJobContactUID, SelectCheckupJobTask.GPRSTUID);
                    if (resultData != null && resultData.Count > 0)
                    {
                        var resultItemList = resultData.Select(p => p.ResultItemName).Distinct();
                        foreach (var item in resultItemList)
                        {
                            ColumnsResultItems.Add(new Column() { Header = item, FieldName = item, VisibleIndex = visibleIndex++ });
                            dtResult.Columns.Add(item);
                        }

                        ColumnsResultItems.Add(new Column() { Header = "คำอธิบาย", FieldName = "CheckupDescription", VisibleIndex = ColumnsResultItems.Count() });
                        ColumnsResultItems.Add(new Column() { Header = "คำแนะนำ", FieldName = "CheckupRecommend", VisibleIndex = ColumnsResultItems.Count() + 1 });
                        ColumnsResultItems.Add(new Column() { Header = "สรุปผลการตรวจสุขภาพ", FieldName = "CheckupResultStatus", VisibleIndex = ColumnsResultItems.Count() + 2 });

                        dtResult.Columns.Add("CheckupDescription");
                        dtResult.Columns.Add("CheckupRecommend");
                        dtResult.Columns.Add("CheckupResultStatus");
                    }
                    var patientData = resultData.GroupBy(p => new
                    {
                        p.PatientID,
                        p.FirstName,
                        p.LastName,
                        p.Department,
                        p.Age,
                        p.Gender,
                        p.CheckupDescription,
                        p.CheckupRecommend,
                        p.CheckupResultStatus
                    })
                    .Select(g => new
                    {
                        PatientID = g.FirstOrDefault().PatientID,
                        FirstName = g.FirstOrDefault().FirstName,
                        LastName = g.FirstOrDefault().LastName,
                        Department = g.FirstOrDefault().Department,
                        Age = g.FirstOrDefault().Age,
                        Gender = g.FirstOrDefault().Gender,
                        CheckupDescription = g.FirstOrDefault().CheckupDescription,
                        CheckupRecommend = g.FirstOrDefault().CheckupRecommend,
                        CheckupResultStatus = g.FirstOrDefault().CheckupResultStatus
                    });
                    int i = 1;
                    foreach (var patient in patientData)
                    {
                        DataRow newRow = dtResult.NewRow();
                        newRow["RowHandle"] = i++;
                        newRow["PatientID"] = patient.PatientID;
                        newRow["FirstName"] = patient.FirstName;
                        newRow["LastName"] = patient.LastName;
                        newRow["Department"] = patient.Department;
                        newRow["Age"] = patient.Age;
                        newRow["Gender"] = patient.Gender;
                        newRow["CheckupDescription"] = patient.CheckupDescription;
                        newRow["CheckupRecommend"] = patient.CheckupRecommend;
                        newRow["CheckupResultStatus"] = patient.CheckupResultStatus;
                        dtResult.Rows.Add(newRow);
                    }
                    foreach (var result in resultData)
                    {
                        var rowData = dtResult.AsEnumerable().Where(dr => dr.Field<string>("PatientID") == result.PatientID
                        && dr.Field<string>("FirstName") == result.FirstName).FirstOrDefault();
                        if (rowData != null)
                        {
                            rowData[result.ResultItemName] = !string.IsNullOrEmpty(result.IsAbnormal) ? result.ResultValue + " " + result.IsAbnormal : result.ResultValue;
                        }
                    }
                    view.gcResultList.ItemsSource = dtResult;
                }

            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }

        }


        private void ExportData()
        {
            TranslateCheckupResult view = (TranslateCheckupResult)this.View;
            if (view.gcResultList.ItemsSource != null)
            {
                string fileName = ShowSaveFileDialog("Microsoft Excel Document", "Microsoft Excel|*.xlsx");
                if (fileName != "")
                {

                    view.gvResultList.ExportToXlsx(fileName);
                    OpenFile(fileName);
                }

            }
        }

        private string ShowSaveFileDialog(string title, string filter)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string name = System.Windows.Forms.Application.ProductName;
            int n = name.LastIndexOf(".") + 1;
            if (n > 0) name = name.Substring(n, name.Length - n);
            dlg.Title = "Export To " + title;
            dlg.FileName = name;
            dlg.Filter = filter;
            if (dlg.ShowDialog() == DialogResult.OK) return dlg.FileName;
            return "";
        }

        private void OpenFile(string fileName)
        {
            if (System.Windows.Forms.MessageBox.Show("Do you want to open this file?", "Export To...", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    process.StartInfo.FileName = fileName;
                    process.StartInfo.Verb = "Open";
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                    process.Start();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Cannot find an application on your system suitable for openning the file with exported data.", System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion
    }
}
