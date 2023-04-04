using DevExpress.Charts.Native;
using DevExpress.Xpf.Core;
using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.Reports.Operating.Radiology;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class CheckRISResultViewModel : MediTechViewModelBase
    {
        #region Properties

        private RequestListModel _PatientRequest;
        public RequestListModel PatientRequest
        {
            get { return _PatientRequest; }
            set { Set(ref _PatientRequest, value); }
        }

        private bool _IsStop = false;
        public bool IsStop
        {
            get { return _IsStop; }
            set { _IsStop = value; }
        }

        private string _ThaiResult;
        public string ThaiResult
        {
            get { return _ThaiResult; }
            set { Set(ref _ThaiResult, value); }
        }

        private PatientResultRadiology _DataResultReport;
        public PatientResultRadiology DataResultReport
        {
            get { return _DataResultReport; }
            set { Set(ref _DataResultReport, value); }
        }

        private List<PreviousResult> _PreviousResult;
        public List<PreviousResult> PreviousResult
        {
            get { return _PreviousResult; }
            set { Set(ref _PreviousResult, value); }
        }

        private PreviousResult _SelectPreviousResult;
        public PreviousResult SelectPreviousResult
        {
            get { return _SelectPreviousResult; }
            set { _SelectPreviousResult = value; }
        }

        public string ResultOrderStatus { get; set; }

        public string DoctorName { get; set; }

        public string ResultedStatus { get; set; }

        #endregion

        #region Command

        private RelayCommand _OPENPACSViewCommand;
        public RelayCommand OPENPACSViewCommand
        {
            get
            {
                return _OPENPACSViewCommand
                    ?? (_OPENPACSViewCommand = new RelayCommand(OPENPACSView));
            }
        }

        private RelayCommand _OpenPastPACSCommand;
        public RelayCommand OpenPastPACSCommand
        {
            get
            {
                return _OpenPastPACSCommand
                    ?? (_OpenPastPACSCommand = new RelayCommand(OpenPastPACS));
            }
        }

        private RelayCommand _StopCommand;
        public RelayCommand StopCommand
        {
            get
            {
                return _StopCommand ?? (_StopCommand = new RelayCommand(Stop));
            }
        }

        private RelayCommand _OKCommand;
        public RelayCommand OKCommand
        {
            get
            {
                return _OKCommand ?? (_OKCommand = new RelayCommand(OK));
            }
        }

        private RelayCommand _EditCommand;
        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand ?? (_EditCommand = new RelayCommand(Edit));
            }
        }

        private RelayCommand _ViewOldResultCommand;
        public RelayCommand ViewOldResultCommand
        {
            get
            {
                return _ViewOldResultCommand ?? (_ViewOldResultCommand = new RelayCommand(ViewOldResult));
            }
        }
        #endregion

        #region Method

        public override void OnLoaded()
        {
            (this.View as CheckRISResult).PatientBanner.SetPatientBanner(PatientRequest.PatientUID, PatientRequest.PatientVisitUID);
        }
        private void OPENPACSView()
        {
            if (PatientRequest != null)
            {
                string modlity;
                if (PatientRequest.Modality == "MG")
                {
                    modlity = "'MG','US'";
                }
                else if (PatientRequest.Modality == "DX")
                {
                    modlity = "'DX','CR'";
                }
                else
                {
                    modlity = "'" + PatientRequest.Modality + "'";
                }

                DateTime? dateFrom = PatientRequest.PreparedDttm != null ? PatientRequest.PreparedDttm : PatientRequest.RequestedDttm;
                DateTime? dateTo = PatientRequest.PreparedDttm != null ? PatientRequest.PreparedDttm : PatientRequest.RequestedDttm;

                var StudiesList = DataService.PACS.SearchPACSWorkList(dateFrom, dateTo,
      modlity, null, PatientRequest.PatientID, null, null, null, null, null);

                if (StudiesList != null && StudiesList.Count == 1)
                {
                    string url = PACSHelper.GetPACSViewerStudyUrl(StudiesList.FirstOrDefault().StudyInstanceUID);
                    PACSHelper.OpenPACSViewer(url);
                }
                else if (StudiesList != null && StudiesList.Count > 1)
                {
                    PACSWorkList pacs = new PACSWorkList();
                    PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                    pacsViewModel.PatientID = PatientRequest.PatientID;
                    pacsViewModel.DateFrom = dateFrom;
                    pacsViewModel.DateTo = dateTo;
                    pacsViewModel.IsCheckedPeriod = true;
                    pacsViewModel.Modality = PatientRequest.Modality;
                    pacsViewModel.StudiesList = StudiesList;
                    DXWindow owner = (DXWindow)(this.View as CheckRISResult).Parent;
                    LaunchViewShow(pacs, owner, "PACS", false, true);
                }
                else
                {

                    dateFrom = dateTo?.AddDays(-15);

                    StudiesList = DataService.PACS.SearchPACSWorkList(dateFrom, dateTo,
                        modlity, null, PatientRequest.PatientID, null, null, null, null, null);
                    if (StudiesList != null && StudiesList.Count == 1)
                    {
                        string url = PACSHelper.GetPACSViewerStudyUrl(StudiesList.FirstOrDefault().StudyInstanceUID);
                        PACSHelper.OpenPACSViewer(url);
                    }
                    else if (StudiesList != null && StudiesList.Count > 1)
                    {
                        PACSWorkList pacs = new PACSWorkList();
                        PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                        pacsViewModel.PatientID = PatientRequest.PatientID;
                        pacsViewModel.DateFrom = dateFrom;
                        pacsViewModel.DateTo = dateTo;
                        pacsViewModel.IsCheckedPeriod = true;
                        pacsViewModel.Modality = PatientRequest.Modality;
                        pacsViewModel.StudiesList = StudiesList;
                        DXWindow owner = (DXWindow)(this.View as CheckRISResult).Parent;
                        LaunchViewShow(pacs, owner, "PACS", false, true);
                    }
                    else
                    {
                        dateFrom = null;
                        dateTo = null;
                        StudiesList = DataService.PACS.SearchPACSWorkList(dateFrom, dateTo,
                            modlity, null, PatientRequest.PatientID, null, null, null, null, null);

                        PACSWorkList pacs = new PACSWorkList();
                        PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                        pacsViewModel.PatientID = PatientRequest.PatientID;
                        pacsViewModel.DateFrom = dateFrom;
                        pacsViewModel.DateTo = dateTo;
                        pacsViewModel.IsCheckedPeriod = true;
                        pacsViewModel.Modality = PatientRequest.Modality;
                        pacsViewModel.StudiesList = StudiesList;
                        pacsViewModel.IsOpenFromExam = true;
                        DXWindow owner = (DXWindow)(this.View as CheckRISResult).Parent;
                        LaunchViewShow(pacs, owner, "PACS", false, true);
                    }
                }
            }

        }

        private void Stop()
        {
            IsStop = true;
            CloseViewDialog(ActionDialog.Cancel);
        }

        private void OK()
        {
            try
            {
                if (PatientRequest != null)
                {
                    CloseViewDialog(ActionDialog.Save);
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }


        private void Edit()
        {
             if (PatientRequest != null)
                {
                    ReviewRISResult review = new ReviewRISResult();
                    (review.DataContext as ReviewRISResultViewModel).AssignModel(PatientRequest.PatientUID
                        , PatientRequest.PatientVisitUID, PatientRequest.RequestUID, PatientRequest.RequestDetailUID);
                    ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)LaunchViewDialog(review, "RESTREV", false, true);

                    if (reviewViewModel == null)
                    {
                        return;
                    }

                    if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                    {
                        ResultOrderStatus = reviewViewModel.ResultOrderStatus;
                        DoctorName = reviewViewModel.DoctorName;
                        ResultedStatus = reviewViewModel.ResultedStatus;

                        AssignModel(PatientRequest);
                        return;
                    }
                
            }
        }
        private void ViewOldResult()
        {
            try
            {
                if (SelectPreviousResult != null)
                {
                    ImagingReport rpt = new ImagingReport();
                    ReportPrintTool printTool = new ReportPrintTool(rpt);

                    rpt.Parameters["ResultUID"].Value = SelectPreviousResult.ResultUID;
                    rpt.RequestParameters = false;
                    rpt.ShowPrintMarginsWarning = false;
                    printTool.ShowPreviewDialog();
                }
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void OpenPastPACS()
        {
            if (SelectPreviousResult != null)
            {
                PACSWorkList pacs = new PACSWorkList();
                PACSWorkListViewModel pacsViewModel = (pacs.DataContext as PACSWorkListViewModel);
                pacsViewModel.PatientID = PatientRequest.PatientID;
                pacsViewModel.IsCheckedPeriod = true;
                pacsViewModel.DateFrom = null;
                pacsViewModel.DateTo = SelectPreviousResult.PreparedDttm != null ? SelectPreviousResult.PreparedDttm : SelectPreviousResult.RequestedDttm;
                pacsViewModel.Modality = SelectPreviousResult.Modality;
                pacsViewModel.IsOpenFromExam = true;
                DXWindow owner = (DXWindow)(this.View as CheckRISResult).Parent;
                LaunchViewShow(pacs, owner, "PACS", false, true);
            }

        }

        public void AssignModel(RequestListModel PatientRequest)
        {
            this.PatientRequest = PatientRequest;
            PreviousResult = DataService.Radiology.GetPreviousResult(PatientRequest.PatientUID, PatientRequest.RequestDetailUID);

            DataResultReport = DataService.Reports.GetPatientResultRadiology(PatientRequest.ResultUID);
            
            List<XrayTranslateMappingModel> dtResultMapping = DataService.Radiology.GetXrayTranslateMapping();
            List<string> listNoMapResult = new List<string>();
            ResultRadiologyModel result = DataService.Radiology.GetResultRadiologyByResultUID(PatientRequest.ResultUID);
            string thairesult = TranslateResult.TranslateResultXray(result.PlainText, PatientRequest.ResultStatus, PatientRequest.RequestItemName, " // ", dtResultMapping, ref listNoMapResult);

            ThaiResult = thairesult;
        }
        #endregion
    }
}
