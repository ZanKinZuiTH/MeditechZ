using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class EnterOccuVisionTestResultViewModel : MediTechViewModelBase
    {
        #region Properties
        private string _RequestItemName;

        public string RequestItemName
        {
            get { return _RequestItemName; }
            set { Set(ref _RequestItemName, value); }
        }


        private ObservableCollection<ResultComponentModel> _ResultComponentItems;

        public ObservableCollection<ResultComponentModel> ResultComponentItems
        {
            get { return _ResultComponentItems; }
            set { Set(ref _ResultComponentItems, value); }
        }


        public string OrderStatus { get; set; }

        private RequestListModel RequestModel;
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


        private RelayCommand _CloseCommand;

        public RelayCommand CloseCommand
        {
            get
            {
                return _CloseCommand
                    ?? (_CloseCommand = new RelayCommand(Close));
            }
        }

        #endregion

        #region Method

        public override void OnLoaded()
        {
            base.OnLoaded();
            (this.View as EnterOccuVisionTestResult).patientBanner.SetPatientBanner(RequestModel);
        }

        public void AssignModel(RequestListModel request)
        {
            this.RequestModel = request;
            RequestItemName = this.RequestModel.RequestItemName;
            var dataList = DataService.Checkup.GetResultItemByRequestDetailUID(request.RequestDetailUID);
            if (dataList != null)
            {
                ResultComponentItems = new ObservableCollection<ResultComponentModel>(dataList);
                foreach (var item in ResultComponentItems)
                {
                    if (!string.IsNullOrEmpty(item.AutoValue))
                        item.AutoValueList = item.AutoValue.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                            .Select(p => p.Trim()).Where(p => !string.IsNullOrEmpty(p)).ToList();
                }

                foreach (var item in ResultComponentItems)
                {
                    if (item.ResultUID != null && !string.IsNullOrEmpty(item.ResultValue))
                    {
                        string[] values = item.ResultValue.Split(',');
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (item.AutoValueList != null && item.AutoValueList.Any(p => p == values[i]))
                            {
                                if (item.CheckDataList == null)
                                    item.CheckDataList = new List<object>();

                                item.CheckDataList.Add(values[i]);
                            }
                        }
                    }
                }
            }
        }

        private void Save()
        {
            try
            {
                RequestDetailItemModel reviewRequestDetail = new RequestDetailItemModel();
                reviewRequestDetail.RequestUID = RequestModel.RequestUID;
                reviewRequestDetail.RequestDetailUID = RequestModel.RequestDetailUID;
                reviewRequestDetail.PatientUID = RequestModel.PatientUID;
                reviewRequestDetail.PatientVisitUID = RequestModel.PatientVisitUID;
                reviewRequestDetail.RequestItemCode = RequestModel.RequestItemCode;
                reviewRequestDetail.RequestItemName = RequestModel.RequestItemName;

                foreach (var item in ResultComponentItems)
                {
                    string resultValue = string.Empty;
                    if (item.CheckDataList != null)
                    {
                        foreach (var phyExam in item.CheckDataList)
                        {
                            resultValue += string.IsNullOrEmpty(resultValue) ? phyExam.ToString() : "," + phyExam.ToString();
                        }

                        item.ResultValue = resultValue;
                    }
                }

                reviewRequestDetail.ResultComponents = ResultComponentItems;
                DataService.Checkup.SaveOccmedExamination(reviewRequestDetail, AppUtil.Current.UserID);
                OrderStatus = "Reviewed";
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        private void Close()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void CalculateOccuVisionResult()
        {
            try
            {
                bool? binocular_normal, far_vision_both_normal, far_vision_right_normal, farvision_left_normal, stereo_depth_normal
                    , color_discrimination_normal, far_vertical_phoria_normal
                    , far_lateral_phoria_normal, near_vision_both_normal, near_vision_right_normal, near_vision_left_normal, near_lateral_photia_normal, visual_field_normal = null;

                var job = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS1");
                var far = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS2");
                var near = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS3");
                var demonstration_slide = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS4");
                var botheyes_far = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS5");
                var righteye_far = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS6");
                var lefteye_far = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS7");
                var stereo_depth = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS8");
                var color = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS9");
                var color_blindness = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS18");
                var vertical = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS10");
                var lateral_far = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS11");
                var botheyes_near = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS12");
                var righteye_near = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS13");
                var lefteye_near = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS14");
                var lateral_near = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS15");
                var perime_score_right = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS16");
                var perime_score_left = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS17");
                var result_eyes_far = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS19");
                var result_eyes_near = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS20");
                var result_eyes_3d = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS21");
                var result_eyes_color = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS22");
                var result_eyes_muscle = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS23");
                var result_eyes_perimeter = ResultComponentItems.FirstOrDefault(p => p.ResultItemCode == "TIMUS24");

                if (string.IsNullOrEmpty(job.ToString()) || job.ToString().Contains("สำนักงาน"))
                {
                    binocular_normal = demonstration_slide.ToString().ToUpper() == "PASS" ? true : demonstration_slide.ToString().ToUpper() == "FAIL" ? false : true;
                    far_vision_both_normal = (botheyes_far == null || botheyes_far.ToString().Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_far.ToString() == "มองไม่เห็น" ? false : Convert.ToInt16(botheyes_far) > 8 ? true : (bool?)null;
                    far_vision_right_normal = (righteye_far == null || righteye_far.ToString().Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_far.ToString() == "มองไม่เห็น" ? false : Convert.ToInt16(righteye_far) > 7 ? true : (bool?)null;
                    farvision_left_normal = (lefteye_far == null || lefteye_far.ToString().Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_far.ToString() == "มองไม่เห็น" ? false : Convert.ToInt16(lefteye_far) > 7 ? true : (bool?)null;
                    stereo_depth_normal = (stereo_depth == null || stereo_depth.ToString().Trim() == "ไม่ได้ตรวจ") ? (bool?)null : stereo_depth.ToString() == "มองไม่เห็น" ? false : Convert.ToInt16(stereo_depth) > 1 ? true : (bool?)null;
                    color_discrimination_normal = (color == null || color.ToString().Trim() == "ไม่ได้ตรวจ") ? (bool?)null : color.ToString() == "มองไม่เห็น" ? false : color.ToString().Contains("X") ? true : (bool?)null;

                }
                else if (string.IsNullOrEmpty(job.ToString()) || job.ToString().Contains("สำนักงาน"))
                {

                }
                else if (string.IsNullOrEmpty(job.ToString()) || job.ToString().Contains("ตรวจสอบ"))
                {

                }
                else if (string.IsNullOrEmpty(job.ToString()) || job.ToString().Contains("ขับพาหนะ"))
                {

                }
                else if (string.IsNullOrEmpty(job.ToString()) || job.ToString().Contains("ฝ่ายผลิต"))
                {

                }
                else if (string.IsNullOrEmpty(job.ToString()) || job.ToString().Contains("แรงงานทั่วไป"))
                {

                }
                else if (string.IsNullOrEmpty(job.ToString()) || job.ToString().Contains("วิศวกรรม"))
                {

                }

            }
            catch (Exception)
            {
            }

        }

        #endregion
    }
}
