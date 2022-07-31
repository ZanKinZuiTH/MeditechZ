using DevExpress.Xpf.Core;
using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight.Command;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.Reports.Operating.Radiology;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class RDUReviewResultViewModel : MediTechViewModelBase
    {
        #region Properties



        private RequestListModel _PatientRequest;

        public RequestListModel PatientRequest
        {
            get { return _PatientRequest; }
            set { Set(ref _PatientRequest, value); }
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

        private bool _IsStop = false;

        public bool IsStop
        {
            get { return _IsStop; }
            set { _IsStop = value; }
        }

        private string _OrderStatus;

        public string OrderStatus
        {
            get { return _OrderStatus; }
            set { _OrderStatus = value; }
        }

        private string _DoctorName;

        public string DoctorName
        {
            get { return _DoctorName; }
            set { _DoctorName = value; }
        }


        private string _ResultedStatus;

        public string ResultedStatus
        {
            get { return _ResultedStatus; }
            set { _ResultedStatus = value; }
        }


        #endregion
        #region Command

        private RelayCommand _OPENPACSViewCommand;

        /// <summary>
        /// Gets the OPENPACSViewCommand.
        /// </summary>
        public RelayCommand OPENPACSViewCommand
        {
            get
            {
                return _OPENPACSViewCommand
                    ?? (_OPENPACSViewCommand = new RelayCommand(OPENPACSView));
            }
        }

        private RelayCommand _ATKNegativeCommand;

        /// <summary>
        /// Gets the NegativeCommand.
        /// </summary>
        public RelayCommand ATKNegativeCommand
        {
            get
            {
                return _ATKNegativeCommand
                    ?? (_ATKNegativeCommand = new RelayCommand(ExecuteATKNegative));
            }
        }

        private RelayCommand _NegativeCommand;

        /// <summary>
        /// Gets the NegativeCommand.
        /// </summary>
        public RelayCommand NegativeCommand
        {
            get
            {
                return _NegativeCommand
                    ?? (_NegativeCommand = new RelayCommand(ExecuteNegative));
            }
        }

        private RelayCommand _PositiveCommand;

        /// <summary>
        /// Gets the PositiveCommand.
        /// </summary>
        public RelayCommand PositiveCommand
        {
            get
            {
                return _PositiveCommand
                    ?? (_PositiveCommand = new RelayCommand(ExecutePositive));
            }
        }

        private RelayCommand _StopCommand;

        /// <summary>
        /// Gets the CancelCommand.
        /// </summary>
        public RelayCommand StopCommand
        {
            get
            {
                return _StopCommand
                    ?? (_StopCommand = new RelayCommand(Stop));
            }
        }

        private RelayCommand _ViewOldResultCommand;

        /// <summary>
        /// Gets the ViewOldResultCommand.
        /// </summary>
        public RelayCommand ViewOldResultCommand
        {
            get
            {
                return _ViewOldResultCommand
                    ?? (_ViewOldResultCommand = new RelayCommand(ViewOldResult));
            }
        }

        private RelayCommand _OpenPastPACSCommand;

        /// <summary>
        /// Gets the OpenPastPACSCommand.
        /// </summary>
        public RelayCommand OpenPastPACSCommand
        {
            get
            {
                return _OpenPastPACSCommand
                    ?? (_OpenPastPACSCommand = new RelayCommand(OpenPastPACS));
            }
        }

        #endregion

        #region Method

        int reviewStatus = 2863;
        int completedStatus = 2845;
        int normalStatus = 2883;
        public override void OnLoaded()
        {
            (this.View as RDUReviewResult).PatientBanner.SetPatientBanner(PatientRequest.PatientUID, PatientRequest.PatientVisitUID);
            OPENPACSView();
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
                    DXWindow owner = (DXWindow)(this.View as RDUReviewResult).Parent;
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
                        DXWindow owner = (DXWindow)(this.View as RDUReviewResult).Parent;
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
                        DXWindow owner = (DXWindow)(this.View as RDUReviewResult).Parent;
                        LaunchViewShow(pacs, owner, "PACS", false, true);
                    }
                }
            }

        }

        private void ExecuteATKNegative()
        {
            try
            {
                if (PatientRequest != null)
                {
                    int userUID = AppUtil.Current.UserID;
                    DateTime now = DateTime.Now;
                    ResultRadiologyModel resultModel = new ResultRadiologyModel();
                    resultModel.ResultUID = PatientRequest.ResultUID;

                    resultModel.RequestDetailUID = PatientRequest.RequestDetailUID;
                    resultModel.ResultEnteredDttm = now;
                    resultModel.RABSTSUID = normalStatus;

                    if (AppUtil.Current.IsRadiologist ?? false)
                    {
                        resultModel.RadiologistUID = userUID;
                        resultModel.ORDSTUID = reviewStatus;
                        OrderStatus = "Reviewed";
                        ResultedStatus = "Normal";
                        DoctorName = AppUtil.Current.UserName;
                    }
                    else
                    {
                        resultModel.Comments = "RDU Review";
                        resultModel.ResultedByUID = userUID;
                        resultModel.RadiologistUID = PatientRequest.RadiologistUID;
                        resultModel.ORDSTUID = completedStatus;
                        OrderStatus = "Completed";
                    }


                    resultModel.PatientUID = PatientRequest.PatientUID;
                    resultModel.PatientVisitUID = PatientRequest.PatientVisitUID;
                    #region ResultValue
                    resultModel.Value = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
	<head>
		<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /><title>
		</title>
		<style type=""text/css"">
			.cs95E872D0{text-align:left;text-indent:0pt;margin:0pt 0pt 0pt 0pt}
			.cs28999509{color:#000000;background-color:transparent;font-family:'Cordia New';font-size:18pt;font-weight:bold;font-style:normal;}
		</style>
	</head>
	<body>
		<p class=""cs95E872D0""><span class=""cs28999509"">History of ATK positive.</span></p><p class=""cs95E872D0""><span class=""cs28999509"">&nbsp; &nbsp;- Normal heart size.</span></p><p class=""cs95E872D0""><span class=""cs28999509"">&nbsp; &nbsp;- No lung infiltration or mass.</span></p><p class=""cs95E872D0""><span class=""cs28999509"">&nbsp; &nbsp;- No pleural effusion.</span></p>
    </body>
</html>";
                    resultModel.PlainText = @"History of ATK positive.
   - Normal heart size.
   - No lung infiltration or mass.
   - No pleural effusion.";
                    #endregion
                    DataService.Radiology.SaveReviewResult(resultModel, userUID);

                    //AutoClosingMessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Sucess", 1000);

                    CloseViewDialog(ActionDialog.Save);
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void ExecuteNegative()
        {
            try
            {
                if (PatientRequest != null)
                {
                    int userUID = AppUtil.Current.UserID;
                    DateTime now = DateTime.Now;
                    ResultRadiologyModel resultModel = new ResultRadiologyModel();
                    resultModel.ResultUID = PatientRequest.ResultUID;

                    resultModel.RequestDetailUID = PatientRequest.RequestDetailUID;
                    resultModel.ResultEnteredDttm = now;
                    resultModel.RABSTSUID = normalStatus;

                    if (AppUtil.Current.IsRadiologist ?? false)
                    {
                        resultModel.RadiologistUID = userUID;
                        resultModel.ORDSTUID = reviewStatus;
                        OrderStatus = "Reviewed";
                        ResultedStatus = "Normal";
                        DoctorName = AppUtil.Current.UserName;
                    }
                    else
                    {
                        resultModel.Comments = "RDU Review";
                        resultModel.ResultedByUID = userUID;
                        resultModel.RadiologistUID = PatientRequest.RadiologistUID;
                        resultModel.ORDSTUID = completedStatus;
                        OrderStatus = "Completed";
                    }
       

                    resultModel.PatientUID = PatientRequest.PatientUID;
                    resultModel.PatientVisitUID = PatientRequest.PatientVisitUID;
                    #region ResultValue
                    resultModel.Value = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
	<head>
		<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" /><title>
		</title>
		<style type=""text/css"">
			.cs95E872D0{text-align:left;text-indent:0pt;margin:0pt 0pt 0pt 0pt}
			.cs28999509{color:#000000;background-color:transparent;font-family:'Cordia New';font-size:18pt;font-weight:bold;font-style:normal;}
		</style>
	</head>
	<body>
		<p class=""cs95E872D0""><span class=""cs28999509"">A case of check up.</span></p><p class=""cs95E872D0""><span class=""cs28999509"">&nbsp; &nbsp;No active pulmonary lesion.</span></p><p class=""cs95E872D0""><span class=""cs28999509"">&nbsp; &nbsp;No pleural effusion.</span></p><p class=""cs95E872D0""><span class=""cs28999509"">&nbsp; &nbsp;Normal heart size.</span></p><p class=""cs95E872D0""><span class=""cs28999509"">&nbsp; &nbsp;Intact bony thorax.</span></p><p class=""cs95E872D0""><span class=""cs28999509"">IMPRESSION:</span></p><p class=""cs95E872D0""><span class=""cs28999509"">&nbsp; &nbsp;Negative study</span></p></body>
</html>";
                    resultModel.PlainText = @"A case of check up.
   No active pulmonary lesion.
   No pleural effusion.
   Normal heart size.
   Intact bony thorax.
IMPRESSION:
   Negative study";
                    #endregion
                    DataService.Radiology.SaveReviewResult(resultModel, userUID);

                    //AutoClosingMessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Sucess", 1000);

                    CloseViewDialog(ActionDialog.Save);
                }
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }

        private void ExecutePositive()
        {
            try
            {
                if (PatientRequest != null)
                {
                    if (AppUtil.Current.IsRadiologist ?? false)
                    {
                        ReviewRISResult review = new ReviewRISResult();
                        (review.DataContext as ReviewRISResultViewModel).FromRDUReview = true;
                        (review.DataContext as ReviewRISResultViewModel).AssignModel(PatientRequest.PatientUID, PatientRequest.PatientVisitUID, PatientRequest.RequestUID, PatientRequest.RequestDetailUID);
                        ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)LaunchViewDialog(review, "RESTREV", false, true);

                        if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                        {
                            OrderStatus = reviewViewModel.ResultOrderStatus;
                            DoctorName = reviewViewModel.DoctorName;
                            ResultedStatus = reviewViewModel.ResultedStatus;
                            CloseViewDialog(ActionDialog.Save);
                        }

                    }
                    else
                    {
                        RDUPositivePopup rduPositPopup = new RDUPositivePopup();
                        RDUPositivePopupViewModel result = (RDUPositivePopupViewModel)LaunchViewDialogNonPermiss(rduPositPopup, true);
                        if (result != null && result.ResultDialog == ActionDialog.Save)
                        {
                            PatientRequest.Comments = "RDU Review";

                            PatientRequest.RDUResultedDttm = DateTime.Now;
                            PatientRequest.RadiologistUID = PatientRequest.RadiologistUID;
                            PatientRequest.RDUNote = result.Comments;
                            PatientRequest.RABSTSUID = 2882;
                            PatientRequest.RDUStaffUID = AppUtil.Current.UserID;

                            //PatientRequest.ExecuteByUID = AppUtil.Current.UserID;
                            //PatientRequest.PreparedDttm = DateTime.Now;
                            DataService.Radiology.AssignRadiologistForMassResult(PatientRequest, AppUtil.Current.UserID);

                            //AutoClosingMessageBox.Show("บันทึกข้อมูลเรียบร้อย", "Sucess", 1000);
                            OrderStatus = "Completed"; //Executed
                            CloseViewDialog(ActionDialog.Save);
                        }

                    }

                }

            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }

        }
        private void Stop()
        {
            IsStop = true;
            CloseViewDialog(ActionDialog.Cancel);
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
                DXWindow owner = (DXWindow)(this.View as RDUReviewResult).Parent;
                LaunchViewShow(pacs, owner, "PACS", false, true);
            }

        }

        public void AssignModel(RequestListModel PatientRequest)
        {
            this.PatientRequest = PatientRequest;
            PreviousResult = DataService.Radiology.GetPreviousResult(PatientRequest.PatientUID, PatientRequest.RequestDetailUID);
        }
        #endregion
    }
}
