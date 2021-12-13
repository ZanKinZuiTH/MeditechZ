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
                    if (item.ResultItemCode == "TIMUS9" || item.ResultItemCode == "TIMUS16" || item.ResultItemCode == "TIMUS17")
                    {
                        if (item.CheckDataList != null)
                        {
                            foreach (var phyExam in item.CheckDataList)
                            {
                                resultValue += string.IsNullOrEmpty(resultValue) ? phyExam.ToString() : "," + phyExam.ToString();
                            }

                            item.ResultValue = resultValue;
                        }
                        else
                        {
                            item.ResultValue = string.Empty;
                        }
                    }

                }

                reviewRequestDetail.ResultComponents = new ObservableCollection<ResultComponentModel>(ResultComponentItems.Where(p => !string.IsNullOrEmpty(p.ResultValue)));

                if (reviewRequestDetail.ResultComponents != null && reviewRequestDetail.ResultComponents.Count > 0)
                {
                    DataService.Checkup.SaveOccmedExamination(reviewRequestDetail, AppUtil.Current.UserID);
                    OrderStatus = "Reviewed";
                }

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
                bool? binocular_normal = null, far_vision_both_normal = null, far_vision_right_normal = null, far_vision_left_normal = null, stereo_depth_normal = null
                    , color_discrimination_normal = null, far_vertical_phoria_normal = null, far_lateral_phoria_normal = null, near_vision_both_normal = null
                    , near_vision_right_normal = null, near_vision_left_normal = null, near_lateral_photia_normal = null, perime_score_both_normal = null;

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





                if (job.ResultValue == null || job.ResultValue.Contains("สำนักงาน") || job.ResultValue.Contains("ไม่ระบุอาชีพ"))
                {
                    if (demonstration_slide != null && demonstration_slide.ResultValue != null)
                        binocular_normal = demonstration_slide.ResultValue.ToUpper() == "PASS" ? true : demonstration_slide.ResultValue.ToUpper() == "FAIL" ? false : true;
                    if (botheyes_far != null && botheyes_far.ResultValue != null)
                        far_vision_both_normal = (botheyes_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_far.ResultValue) == true && Convert.ToInt16(botheyes_far.ResultValue) >= 8 ? true : false;
                    if (righteye_far != null && righteye_far.ResultValue != null)
                        far_vision_right_normal = (righteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_far.ResultValue) == true && Convert.ToInt16(righteye_far.ResultValue) >= 7 ? true : false;
                    if (lefteye_far != null && lefteye_far.ResultValue != null)
                        far_vision_left_normal = (lefteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_far.ResultValue) == true && Convert.ToInt16(lefteye_far.ResultValue) >= 7 ? true : false;
                    if (stereo_depth != null && stereo_depth.ResultValue != null)
                        stereo_depth_normal = (stereo_depth.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : stereo_depth.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(stereo_depth.ResultValue) == true && Convert.ToInt16(stereo_depth.ResultValue) >= 1 ? true : false;
                    if (color != null && color.CheckDataList != null)
                        color_discrimination_normal = (color.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ")) ? (bool?)null : color.CheckDataList.Any(p => p.ToString() == "มองไม่เห็น") ? false : color.CheckDataList.Any(p => p.ToString() == "X") ? true : false;
                    if (vertical != null && vertical.ResultValue != null)
                        far_vertical_phoria_normal = (vertical.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : vertical.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(vertical.ResultValue) == true && Convert.ToInt16(vertical.ResultValue) >= 3 && Convert.ToInt16(vertical.ResultValue) <= 5 ? true : false;
                    if (lateral_far != null && lateral_far.ResultValue != null)
                        far_lateral_phoria_normal = (lateral_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_far.ResultValue) == true && Convert.ToInt16(lateral_far.ResultValue) >= 4 && Convert.ToInt16(lateral_far.ResultValue) <= 13 ? true : false;
                    if (botheyes_near != null && botheyes_near.ResultValue != null)
                        near_vision_both_normal = (botheyes_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_near.ResultValue) == true && Convert.ToInt16(botheyes_near.ResultValue) >= 9 ? true : false;
                    if (righteye_near != null && righteye_near.ResultValue != null)
                        near_vision_right_normal = (righteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_near.ResultValue) == true &&  Convert.ToInt16(righteye_near.ResultValue) >= 8 ? true : false;
                    if (lefteye_near != null && lefteye_near.ResultValue != null)
                        near_vision_left_normal = (lefteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_near.ResultValue) == true &&  Convert.ToInt16(lefteye_near.ResultValue) >= 8 ? true : false;
                    if (lateral_near != null && lateral_near.ResultValue != null)
                        near_lateral_photia_normal = (lateral_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_near.ResultValue) == true &&  Convert.ToInt16(lateral_near.ResultValue) >= 4 && Convert.ToInt16(lateral_near.ResultValue) <= 13 ? true : false;

                    bool? right_perimis_normal = null;
                    bool? left_perimis_normal = null;
                    if (perime_score_right != null && perime_score_right.CheckDataList != null)
                        right_perimis_normal = perime_score_right.CheckDataList.Any(p=> p.ToString()== "ไม่ได้ตรวจ") ? (bool?)null : perime_score_right.CheckDataList.Count() == 4 ? true : false;
                    if (perime_score_left != null && perime_score_left.CheckDataList != null)
                        left_perimis_normal = perime_score_left.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_left.CheckDataList.Count() == 4 ? true : false;
                    perime_score_both_normal = (right_perimis_normal == null || left_perimis_normal == null) ? (bool?)null : (right_perimis_normal.Value && left_perimis_normal.Value) ? true : false;
                    if (right_perimis_normal != null && left_perimis_normal != null)
                    {
                        if (right_perimis_normal.Value && left_perimis_normal.Value)
                        {
                            perime_score_both_normal = true;
                        }
                        else
                        {
                            perime_score_both_normal = false;
                        }
                    }
                    else
                    {
                        perime_score_both_normal = null;
                    }


                }
                else if (job != null && job.ResultValue.Contains("ตรวจสอบ"))
                {
                    if (demonstration_slide != null && demonstration_slide.ResultValue != null)
                        binocular_normal = demonstration_slide.ResultValue.ToUpper() == "PASS" ? true : demonstration_slide.ResultValue.ToUpper() == "FAIL" ? false : true;
                    if (botheyes_far != null && botheyes_far.ResultValue != null)
                        far_vision_both_normal = (botheyes_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_far.ResultValue) == true && Convert.ToInt16(botheyes_far.ResultValue) >= 7 ? true : false;
                    if (righteye_far != null && righteye_far.ResultValue != null)
                        far_vision_right_normal = (righteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_far.ResultValue) == true && Convert.ToInt16(righteye_far.ResultValue) >= 6 ? true : false;
                    if (lefteye_far != null && lefteye_far.ResultValue != null)
                        far_vision_left_normal = (lefteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_far.ResultValue) == true && Convert.ToInt16(lefteye_far.ResultValue) >= 6 ? true : false;
                    if (stereo_depth != null && stereo_depth.ResultValue != null)
                        stereo_depth_normal = (stereo_depth.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : stereo_depth.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(stereo_depth.ResultValue) == true && Convert.ToInt16(stereo_depth.ResultValue) >= 1 ? true : false;
                    if (color != null && color.CheckDataList != null)
                        color_discrimination_normal = (color.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ")) ? (bool?)null : color.CheckDataList.Any(p => p.ToString() == "มองไม่เห็น") ? false : color.CheckDataList.Any(p => p.ToString() == "X") ? true : false;
                    if (vertical != null && vertical.ResultValue != null)
                        far_vertical_phoria_normal = (vertical.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : vertical.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(vertical.ResultValue) == true && Convert.ToInt16(vertical.ResultValue) >= 3 && Convert.ToInt16(vertical.ResultValue) <= 5 ? true : false;
                    if (lateral_far != null && lateral_far.ResultValue != null)
                        far_lateral_phoria_normal = (lateral_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_far.ResultValue) == true && Convert.ToInt16(lateral_far.ResultValue) >= 4 && Convert.ToInt16(lateral_far.ResultValue) <= 13 ? true : false;
                    if (botheyes_near != null && botheyes_near.ResultValue != null)
                        near_vision_both_normal = (botheyes_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_near.ResultValue) == true && Convert.ToInt16(botheyes_near.ResultValue) >= 9 ? true : false;
                    if (righteye_near != null && righteye_near.ResultValue != null)
                        near_vision_right_normal = (righteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_near.ResultValue) == true && Convert.ToInt16(righteye_near.ResultValue) >= 8 ? true : false;
                    if (lefteye_near != null && lefteye_near.ResultValue != null)
                        near_vision_left_normal = (lefteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_near.ResultValue) == true && Convert.ToInt16(lefteye_near.ResultValue) >= 8 ? true : false;
                    if (lateral_near != null && lateral_near.ResultValue != null)
                        near_lateral_photia_normal = (lateral_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_near.ResultValue) == true && Convert.ToInt16(lateral_near.ResultValue) >= 4 && Convert.ToInt16(lateral_near.ResultValue) <= 13 ? true : false;

                    bool? right_perimis_normal = null;
                    bool? left_perimis_normal = null;
                    if (perime_score_right != null && perime_score_right.CheckDataList != null)
                        right_perimis_normal = perime_score_right.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_right.CheckDataList.Count() == 4 ? true : false;
                    if (perime_score_left != null && perime_score_left.CheckDataList != null)
                        left_perimis_normal = perime_score_left.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_left.CheckDataList.Count() == 4 ? true : false;
                    perime_score_both_normal = (right_perimis_normal == null || left_perimis_normal == null) ? (bool?)null : (right_perimis_normal.Value && left_perimis_normal.Value) ? true : false;
                    if (right_perimis_normal != null && left_perimis_normal != null)
                    {
                        if (right_perimis_normal.Value && left_perimis_normal.Value)
                        {
                            perime_score_both_normal = true;
                        }
                        else
                        {
                            perime_score_both_normal = false;
                        }
                    }
                    else
                    {
                        perime_score_both_normal = null;
                    }
                }
                else if (job != null && job.ResultValue.Contains("ขับพาหนะ"))
                {
                    if (demonstration_slide != null && demonstration_slide.ResultValue != null)
                        binocular_normal = demonstration_slide.ResultValue.ToUpper() == "PASS" ? true : demonstration_slide.ResultValue.ToUpper() == "FAIL" ? false : true;
                    if (botheyes_far != null && botheyes_far.ResultValue != null)
                        far_vision_both_normal = (botheyes_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_far.ResultValue) == true && Convert.ToInt16(botheyes_far.ResultValue) >= 9 ? true : false;
                    if (righteye_far != null && righteye_far.ResultValue != null)
                        far_vision_right_normal = (righteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_far.ResultValue) == true && Convert.ToInt16(righteye_far.ResultValue) >= 8 ? true : false;
                    if (lefteye_far != null && lefteye_far.ResultValue != null)
                        far_vision_left_normal = (lefteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_far.ResultValue) == true && Convert.ToInt16(lefteye_far.ResultValue) >= 8 ? true : false;
                    if (stereo_depth != null && stereo_depth.ResultValue != null)
                        stereo_depth_normal = (stereo_depth.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : stereo_depth.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(stereo_depth.ResultValue) == true && Convert.ToInt16(stereo_depth.ResultValue) >= 6 ? true : false;
                    if (color != null && color.CheckDataList != null)
                        color_discrimination_normal = (color.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ")) ? (bool?)null : color.CheckDataList.Any(p => p.ToString() == "มองไม่เห็น") ? false : color.CheckDataList.Any(p => p.ToString() == "X") ? true : false;
                    if (vertical != null && vertical.ResultValue != null)
                        far_vertical_phoria_normal = (vertical.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : vertical.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(vertical.ResultValue) == true && Convert.ToInt16(vertical.ResultValue) >= 3 && Convert.ToInt16(vertical.ResultValue) <= 5 ? true : false;
                    if (lateral_far != null && lateral_far.ResultValue != null)
                        far_lateral_phoria_normal = (lateral_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_far.ResultValue) == true && Convert.ToInt16(lateral_far.ResultValue) >= 4 && Convert.ToInt16(lateral_far.ResultValue) <= 13 ? true : false;
                    if (botheyes_near != null && botheyes_near.ResultValue != null)
                        near_vision_both_normal = (botheyes_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_near.ResultValue) == true && Convert.ToInt16(botheyes_near.ResultValue) >= 7 ? true : false;
                    if (righteye_near != null && righteye_near.ResultValue != null)
                        near_vision_right_normal = (righteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_near.ResultValue) == true && Convert.ToInt16(righteye_near.ResultValue) >= 6 ? true : false;
                    if (lefteye_near != null && lefteye_near.ResultValue != null)
                        near_vision_left_normal = (lefteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_near.ResultValue) == true && Convert.ToInt16(lefteye_near.ResultValue) > 6 ? true : false;
                    if (lateral_near != null && lateral_near.ResultValue != null)
                        near_lateral_photia_normal = (lateral_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_near.ResultValue) == true && Convert.ToInt16(lateral_near.ResultValue) >= 4 && Convert.ToInt16(lateral_near.ResultValue) <= 13 ? true : false;

                    bool? right_perimis_normal = null;
                    bool? left_perimis_normal = null;
                    if (perime_score_right != null && perime_score_right.CheckDataList != null)
                        right_perimis_normal = perime_score_right.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_right.CheckDataList.Count() == 4 ? true : false;
                    if (perime_score_left != null && perime_score_left.CheckDataList != null)
                        left_perimis_normal = perime_score_left.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_left.CheckDataList.Count() == 4 ? true : false;
                    perime_score_both_normal = (right_perimis_normal == null || left_perimis_normal == null) ? (bool?)null : (right_perimis_normal.Value && left_perimis_normal.Value) ? true : false;
                    if (right_perimis_normal != null && left_perimis_normal != null)
                    {
                        if (right_perimis_normal.Value && left_perimis_normal.Value)
                        {
                            perime_score_both_normal = true;
                        }
                        else
                        {
                            perime_score_both_normal = false;
                        }
                    }
                    else
                    {
                        perime_score_both_normal = null;
                    }
                }
                else if (job != null && job.ResultValue.Contains("ฝ่ายผลิต"))
                {
                    if (demonstration_slide != null && demonstration_slide.ResultValue != null)
                        binocular_normal = demonstration_slide.ResultValue.ToUpper() == "PASS" ? true : demonstration_slide.ResultValue.ToUpper() == "FAIL" ? false : true;
                    if (botheyes_far != null && botheyes_far.ResultValue != null)
                        far_vision_both_normal = (botheyes_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_far.ResultValue) == true && Convert.ToInt16(botheyes_far.ResultValue) >= 8 ? true : false;
                    if (righteye_far != null && righteye_far.ResultValue != null)
                        far_vision_right_normal = (righteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_far.ResultValue) == true && Convert.ToInt16(righteye_far.ResultValue) >= 7 ? true : false;
                    if (lefteye_far != null && lefteye_far.ResultValue != null)
                        far_vision_left_normal = (lefteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_far.ResultValue) == true && Convert.ToInt16(lefteye_far.ResultValue) >= 7 ? true : false;
                    if (stereo_depth != null && stereo_depth.ResultValue != null)
                        stereo_depth_normal = (stereo_depth.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : stereo_depth.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(stereo_depth.ResultValue) == true && Convert.ToInt16(stereo_depth.ResultValue) >= 5 ? true : false;
                    if (color != null && color.CheckDataList != null)
                        color_discrimination_normal = (color.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ")) ? (bool?)null : color.CheckDataList.Any(p => p.ToString() == "มองไม่เห็น") ? false : color.CheckDataList.Any(p => p.ToString() == "X") ? true : false;
                    if (vertical != null && vertical.ResultValue != null)
                        far_vertical_phoria_normal = (vertical.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : vertical.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(vertical.ResultValue) == true && Convert.ToInt16(vertical.ResultValue) >= 3 && Convert.ToInt16(vertical.ResultValue) <= 5 ? true : false;
                    if (lateral_far != null && lateral_far.ResultValue != null)
                        far_lateral_phoria_normal = (lateral_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_far.ResultValue) == true && Convert.ToInt16(lateral_far.ResultValue) >= 4 && Convert.ToInt16(lateral_far.ResultValue) <= 13 ? true : false;
                    if (botheyes_near != null && botheyes_near.ResultValue != null)
                        near_vision_both_normal = (botheyes_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_near.ResultValue) == true && Convert.ToInt16(botheyes_near.ResultValue) >= 8 ? true : false;
                    if (righteye_near != null && righteye_near.ResultValue != null)
                        near_vision_right_normal = (righteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_near.ResultValue) == true && Convert.ToInt16(righteye_near.ResultValue) >= 7 ? true : false;
                    if (lefteye_near != null && lefteye_near.ResultValue != null)
                        near_vision_left_normal = (lefteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_near.ResultValue) == true && Convert.ToInt16(lefteye_near.ResultValue) >= 7 ? true : false;
                    if (lateral_near != null && lateral_near.ResultValue != null)
                        near_lateral_photia_normal = (lateral_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_near.ResultValue) == true && Convert.ToInt16(lateral_near.ResultValue) >= 4 && Convert.ToInt16(lateral_near.ResultValue) <= 13 ? true : false;

                    bool? right_perimis_normal = null;
                    bool? left_perimis_normal = null;
                    if (perime_score_right != null && perime_score_right.CheckDataList != null)
                        right_perimis_normal = perime_score_right.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_right.CheckDataList.Count() == 4 ? true : false;
                    if (perime_score_left != null && perime_score_left.CheckDataList != null)
                        left_perimis_normal = perime_score_left.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_left.CheckDataList.Count() == 4 ? true : false;
                    perime_score_both_normal = (right_perimis_normal == null || left_perimis_normal == null) ? (bool?)null : (right_perimis_normal.Value && left_perimis_normal.Value) ? true : false;
                    if (right_perimis_normal != null && left_perimis_normal != null)
                    {
                        if (right_perimis_normal.Value && left_perimis_normal.Value)
                        {
                            perime_score_both_normal = true;
                        }
                        else
                        {
                            perime_score_both_normal = false;
                        }
                    }
                    else
                    {
                        perime_score_both_normal = null;
                    }
                }
                else if (job != null && job.ResultValue.Contains("แรงงานทั่วไป"))
                {
                    if (demonstration_slide != null && demonstration_slide.ResultValue != null)
                        binocular_normal = demonstration_slide.ResultValue.ToUpper() == "PASS" ? true : demonstration_slide.ResultValue.ToUpper() == "FAIL" ? false : true;
                    if (botheyes_far != null && botheyes_far.ResultValue != null)
                        far_vision_both_normal = (botheyes_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_far.ResultValue) == true && Convert.ToInt16(botheyes_far.ResultValue) >= 8 ? true : false;
                    if (righteye_far != null && righteye_far.ResultValue != null)
                        far_vision_right_normal = (righteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_far.ResultValue) == true && Convert.ToInt16(righteye_far.ResultValue) >= 7 ? true : false;
                    if (lefteye_far != null && lefteye_far.ResultValue != null)
                        far_vision_left_normal = (lefteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_far.ResultValue) == true && Convert.ToInt16(lefteye_far.ResultValue) >= 7 ? true : false;
                    if (stereo_depth != null && stereo_depth.ResultValue != null)
                        stereo_depth_normal = (stereo_depth.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : stereo_depth.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(stereo_depth.ResultValue) == true && Convert.ToInt16(stereo_depth.ResultValue) >= 1 ? true : false;
                    if (color != null && color.CheckDataList != null)
                        color_discrimination_normal = (color.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ")) ? (bool?)null : color.CheckDataList.Any(p => p.ToString() == "มองไม่เห็น") ? false : color.CheckDataList.Any() ? true : false;
                    if (vertical != null && vertical.ResultValue != null)
                        far_vertical_phoria_normal = (vertical.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : vertical.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(vertical.ResultValue) == true && Convert.ToInt16(vertical.ResultValue) >= 2  ? true : false;
                    if (lateral_far != null && lateral_far.ResultValue != null)
                        far_lateral_phoria_normal = (lateral_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_far.ResultValue) == true && Convert.ToInt16(lateral_far.ResultValue) >= 1 && Convert.ToInt16(lateral_far.ResultValue) <= 15 ? true : false;
                    if (botheyes_near != null && botheyes_near.ResultValue != null)
                        near_vision_both_normal = (botheyes_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_near.ResultValue) == true && Convert.ToInt16(botheyes_near.ResultValue) >= 7 ? true : false;
                    if (righteye_near != null && righteye_near.ResultValue != null)
                        near_vision_right_normal = (righteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_near.ResultValue) == true && Convert.ToInt16(righteye_near.ResultValue) >= 6 ? true : false;
                    if (lefteye_near != null && lefteye_near.ResultValue != null)
                        near_vision_left_normal = (lefteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_near.ResultValue) == true && Convert.ToInt16(lefteye_near.ResultValue) >= 6 ? true : false;
                    if (lateral_near != null && lateral_near.ResultValue != null)
                        near_lateral_photia_normal = (lateral_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_near.ResultValue) == true && Convert.ToInt16(lateral_near.ResultValue) >= 1 && Convert.ToInt16(lateral_near.ResultValue) <= 15 ? true : false;

                    bool? right_perimis_normal = null;
                    bool? left_perimis_normal = null;
                    if (perime_score_right != null && perime_score_right.CheckDataList != null)
                        right_perimis_normal = perime_score_right.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_right.CheckDataList.Count() == 4 ? true : false;
                    if (perime_score_left != null && perime_score_left.CheckDataList != null)
                        left_perimis_normal = perime_score_left.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_left.CheckDataList.Count() == 4 ? true : false;
                    perime_score_both_normal = (right_perimis_normal == null || left_perimis_normal == null) ? (bool?)null : (right_perimis_normal.Value && left_perimis_normal.Value) ? true : false;
                    if (right_perimis_normal != null && left_perimis_normal != null)
                    {
                        if (right_perimis_normal.Value && left_perimis_normal.Value)
                        {
                            perime_score_both_normal = true;
                        }
                        else
                        {
                            perime_score_both_normal = false;
                        }
                    }
                    else
                    {
                        perime_score_both_normal = null;
                    }
                }
                else if (job != null && job.ResultValue.Contains("วิศวกรรม"))
                {
                    if (demonstration_slide != null && demonstration_slide.ResultValue != null)
                        binocular_normal = demonstration_slide.ResultValue.ToUpper() == "PASS" ? true : demonstration_slide.ResultValue.ToUpper() == "FAIL" ? false : true;
                    if (botheyes_far != null && botheyes_far.ResultValue != null)
                        far_vision_both_normal = (botheyes_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_far.ResultValue) == true && Convert.ToInt16(botheyes_far.ResultValue) >= 8 ? true : false;
                    if (righteye_far != null && righteye_far.ResultValue != null)
                        far_vision_right_normal = (righteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_far.ResultValue) == true && Convert.ToInt16(righteye_far.ResultValue) >= 7 ? true : false;
                    if (lefteye_far != null && lefteye_far.ResultValue != null)
                        far_vision_left_normal = (lefteye_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_far.ResultValue) == true && Convert.ToInt16(lefteye_far.ResultValue) >= 7 ? true : false;
                    if (stereo_depth != null && stereo_depth.ResultValue != null)
                        stereo_depth_normal = (stereo_depth.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : stereo_depth.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(stereo_depth.ResultValue) == true && Convert.ToInt16(stereo_depth.ResultValue) >= 5 ? true : false;
                    if (color != null && color.CheckDataList != null)
                        color_discrimination_normal = (color.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ")) ? (bool?)null : color.CheckDataList.Any(p => p.ToString() == "มองไม่เห็น") ? false : color.CheckDataList.Any(p => p.ToString() == "X") ? true : false;
                    if (vertical != null && vertical.ResultValue != null)
                        far_vertical_phoria_normal = (vertical.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : vertical.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(vertical.ResultValue) == true && Convert.ToInt16(vertical.ResultValue) >= 3 && Convert.ToInt16(vertical.ResultValue) <= 5 ? true : false;
                    if (lateral_far != null && lateral_far.ResultValue != null)
                        far_lateral_phoria_normal = (lateral_far.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_far.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_far.ResultValue) == true && Convert.ToInt16(lateral_far.ResultValue) >= 4 && Convert.ToInt16(lateral_far.ResultValue) <= 13 ? true : false;
                    if (botheyes_near != null && botheyes_near.ResultValue != null)
                        near_vision_both_normal = (botheyes_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : botheyes_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(botheyes_near.ResultValue) == true && Convert.ToInt16(botheyes_near.ResultValue) >= 9 ? true : false;
                    if (righteye_near != null && righteye_near.ResultValue != null)
                        near_vision_right_normal = (righteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : righteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(righteye_near.ResultValue) == true && Convert.ToInt16(righteye_near.ResultValue) >= 8 ? true : false;
                    if (lefteye_near != null && lefteye_near.ResultValue != null)
                        near_vision_left_normal = (lefteye_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lefteye_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lefteye_near.ResultValue) == true && Convert.ToInt16(lefteye_near.ResultValue) >= 8 ? true : false;
                    if (lateral_near != null && lateral_near.ResultValue != null)
                        near_lateral_photia_normal = (lateral_near.ResultValue.Trim() == "ไม่ได้ตรวจ") ? (bool?)null : lateral_near.ResultValue == "มองไม่เห็น" ? false : ShareLibrary.CheckValidate.IsNumber(lateral_near.ResultValue) == true && Convert.ToInt16(lateral_near.ResultValue) >= 4 && Convert.ToInt16(lateral_near.ResultValue) <= 13 ? true : false;

                    bool? right_perimis_normal = null;
                    bool? left_perimis_normal = null;
                    if (perime_score_right != null && perime_score_right.CheckDataList != null)
                        right_perimis_normal = perime_score_right.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_right.CheckDataList.Count() == 4 ? true : false;
                    if (perime_score_left != null && perime_score_left.CheckDataList != null)
                        left_perimis_normal = perime_score_left.CheckDataList.Any(p => p.ToString() == "ไม่ได้ตรวจ") ? (bool?)null : perime_score_left.CheckDataList.Count() == 4 ? true : false;
                    perime_score_both_normal = (right_perimis_normal == null || left_perimis_normal == null) ? (bool?)null : (right_perimis_normal.Value && left_perimis_normal.Value) ? true : false;
                    if (right_perimis_normal != null && left_perimis_normal != null)
                    {
                        if (right_perimis_normal.Value && left_perimis_normal.Value)
                        {
                            perime_score_both_normal = true;
                        }
                        else
                        {
                            perime_score_both_normal = false;
                        }
                    }
                    else
                    {
                        perime_score_both_normal = null;
                    }
                }

                //มองระยะไกล
                if (far_vision_both_normal == true && far_vision_right_normal == true && far_vision_left_normal == true)
                {
                    result_eyes_far.ResultValue = "ปกติ";
                }
                else if (far_vision_both_normal == false || far_vision_right_normal == false || far_vision_left_normal == false)
                {
                    result_eyes_far.ResultValue = "ผิดปกติ";
                }
                else if(far_vision_both_normal == true || far_vision_right_normal == true || far_vision_left_normal == true)
                {
                    result_eyes_far.ResultValue = "ปกติ";
                }
                else if(far_vision_both_normal == null && far_vision_right_normal == null && far_vision_left_normal == null)
                {
                    result_eyes_far.ResultValue = "";
                }

                //มองระยะใกล้
                if (near_vision_both_normal == true && near_vision_right_normal == true && near_vision_left_normal == true)
                {
                    result_eyes_near.ResultValue = "ปกติ";
                }
                else if (near_vision_both_normal == false || near_vision_right_normal == false || near_vision_left_normal == false)
                {
                    result_eyes_near.ResultValue = "ผิดปกติ";
                }
                else if (near_vision_both_normal == true || near_vision_right_normal == true || near_vision_left_normal == true)
                {
                    result_eyes_near.ResultValue = "ปกติ";
                }
                else if (near_vision_both_normal == null && near_vision_right_normal == null && near_vision_left_normal == null)
                {
                    result_eyes_near.ResultValue = "";
                }

                //มองภาพ 3 มิติ
                if (stereo_depth_normal == true)
                {
                    result_eyes_3d.ResultValue = "ปกติ";
                }
                else if (stereo_depth_normal == false)
                {
                    result_eyes_3d.ResultValue = "ผิดปกติ";
                }
                else if (stereo_depth_normal == null)
                {
                    result_eyes_3d.ResultValue = "";
                }


                //การแยกสี
                if (color_discrimination_normal == true)
                {
                    result_eyes_color.ResultValue = "ปกติ";
                }
                else if (color_discrimination_normal == false)
                {
                    result_eyes_color.ResultValue = "ผิดปกติ";
                }
                else if (color_discrimination_normal == null)
                {
                    result_eyes_color.ResultValue = "";
                }

                if (color_blindness != null && !string.IsNullOrEmpty(color_blindness.ResultValue))
                {
                    if (color_blindness.ResultValue == "Normal")
                    {
                        result_eyes_color.ResultValue = "ปกติ";
                    }
                    else
                    {
                        result_eyes_color.ResultValue = "ผิดปกติ";
                    }
                }

                //ความสมดุลย์กล้ามเนื้อตา
                if (far_vertical_phoria_normal == true && far_lateral_phoria_normal == true && near_lateral_photia_normal == true)
                {
                    result_eyes_muscle.ResultValue = "ปกติ";
                }
                else if (far_vertical_phoria_normal == false || far_lateral_phoria_normal == false || near_lateral_photia_normal == false)
                {
                    result_eyes_muscle.ResultValue = "ผิดปกติ";
                }
                else if (far_vertical_phoria_normal == true || far_lateral_phoria_normal == true || near_lateral_photia_normal == true)
                {
                    result_eyes_muscle.ResultValue = "ปกติ";
                }
                else if (far_vertical_phoria_normal == null && far_lateral_phoria_normal == null && near_lateral_photia_normal == null)
                {
                    result_eyes_muscle.ResultValue = "";
                }

                //ลานสายตา
                if (perime_score_both_normal == true)
                {
                    result_eyes_perimeter.ResultValue = "ปกติ";
                }
                else if (perime_score_both_normal == false)
                {
                    result_eyes_perimeter.ResultValue = "ผิดปกติ";
                }
                else if (perime_score_both_normal == null)
                {
                    result_eyes_perimeter.ResultValue = "";
                }

            }
            catch (Exception er)
            {
            }

        }

        #endregion
    }
}
