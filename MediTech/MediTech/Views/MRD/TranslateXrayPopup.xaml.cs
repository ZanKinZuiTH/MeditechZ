using DevExpress.Data.Filtering;
using GalaSoft.MvvmLight;
using MediTech.DataService;
using MediTech.Helpers;
using MediTech.Model;
using MediTech.Model.Report;
using MediTech.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediTech.Views
{
    /// <summary>
    /// Interaction logic for TranslateXrayPopup.xaml
    /// </summary>
    public partial class TranslateXrayPopup : Window
    {
        RadiologyService radiologyData;

        public PatientResultRadiology patientResultRadioloty;
        public bool IsUpdateResult { get; set; }

        public bool IsCancel { get; set; }

        public ObservableCollection<XrayMappingEdit> ListMapping;
        public List<object> TranslateConditions { get; set; }

        int typeid = 0;
        public TranslateXrayPopup(List<string> wordstranslate)
        {
            InitializeComponent();
            this.Loaded += TranslateXrayPopup_Loaded;
            btnSave.Click += btnSave_Click;
            btnStop.Click += btnStop_Click;
            btnEnterResult.Click += btnEnterResult_Click;
            btnTemplate.Click += btnTemplate_Click;
            ListMapping = new ObservableCollection<XrayMappingEdit>();
            foreach (string word in wordstranslate)
            {
                XrayMappingEdit dr = new XrayMappingEdit();

                dr.IsKeyWord = false;
                dr.EngResult = word;

                ListMapping.Add(dr);
            }
            gcTranslate.ItemsSource = ListMapping;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            IsCancel = true;
            this.Close();
        }

        private void btnTemplate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gcTranslate.SelectedItem != null)
                {
                    int userID = AppUtil.Current.UserID;
                    XrayMappingEdit selectMapping = (XrayMappingEdit)gcTranslate.SelectedItem;
                    if (string.IsNullOrEmpty(selectMapping.ThaiResult)
                        || string.IsNullOrEmpty(selectMapping.XrayTranslateConditionName)
                        || string.IsNullOrEmpty(selectMapping.XrayTranslateConditionDetailName))
                    {
                        System.Windows.Forms.MessageBox.Show("กรูณาใส่ข้อมูลให้ครบ", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    radiologyData.SaveTemplateTransalteMaster(selectMapping.XrayTranslateConditionName
                        , selectMapping.XrayTranslateConditionDetailName, selectMapping.ThaiResult, typeid, userID);

                    System.Windows.Forms.MessageBox.Show("บันทึกเรียบร้อย", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show(er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEnterResult_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReviewRISResult review = new ReviewRISResult();
                ReviewRISResultViewModel viewModel = (review.DataContext as ReviewRISResultViewModel);
                viewModel.AssignModel(patientResultRadioloty.PatientUID, patientResultRadioloty.PatientVisitUID, patientResultRadioloty.RequestUID, patientResultRadioloty.RequestDetailUID);
                ReviewRISResultViewModel reviewViewModel = (ReviewRISResultViewModel)viewModel.LaunchViewDialog(review, "RESTREV", false, true);
                if (reviewViewModel != null && reviewViewModel.ResultDialog == ActionDialog.Save)
                {
                    DataService.MediTechDataService dataService = new MediTechDataService();
                    List<string> wordnomap = new List<string>();
                    List<XrayTranslateMappingModel> dtResultMapping = dataService.Radiology.GetXrayTranslateMapping();
                    ResultRadiologyModel dataResultLast = dataService.Radiology.GetResultRadiologyByResultUID(patientResultRadioloty.ResultUID);

                    TranslateResult.TranslateResultXray(dataResultLast.PlainText, dataResultLast.ResultStatus,dataResultLast.RequestItemName," ", dtResultMapping, ref wordnomap);
                    ListMapping = new ObservableCollection<XrayMappingEdit>();

                    foreach (string word in wordnomap)
                    {
                        XrayMappingEdit dr = new XrayMappingEdit();

                        dr.IsKeyWord = false;
                        dr.EngResult = word;

                        ListMapping.Add(dr);
                    }
                    gcTranslate.ItemsSource = ListMapping;
                    txtResultStatus.Text = dataResultLast.ResultStatus;
                    patientResultRadioloty.ResultValue = dataResultLast.PlainText;
                    patientResultRadioloty.ResultStatus = dataResultLast.ResultStatus;
                    patientResultRadioloty.ResultHtml = dataResultLast.Value;
                    patientResultRadioloty.Doctor = dataResultLast.Radiologist;
                    IsUpdateResult = true;
                }
            }
            catch (Exception er)
            {

                System.Windows.Forms.MessageBox.Show(er.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gcTranslate.ItemsSource != null)
                {
                    ObservableCollection<XrayMappingEdit> listMapping = (ObservableCollection<XrayMappingEdit>)gcTranslate.ItemsSource;
                    foreach (var item in listMapping)
                    {
                        if (item.IsKeyWord == false)
                        {
                            if (string.IsNullOrEmpty(item.ThaiResult))
                            {
                                System.Windows.Forms.MessageBox.Show("กรุณาใส่ข้อมูลให้ครบ", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }

                    foreach (var item in listMapping)
                    {

                        bool isKeyword = item.IsKeyWord.ToString() != "" ? item.IsKeyWord : false;

                        XrayTranslateMappingModel mappingModel = new XrayTranslateMappingModel();
                        mappingModel.EngResult = item.EngResult;
                        mappingModel.ThaiResult = item.ThaiResult;
                        mappingModel.Type = typeid;
                        mappingModel.IsKeyword = isKeyword;
                        mappingModel.CUser = AppUtil.Current.UserID;
                        mappingModel.MUser = AppUtil.Current.UserID;
                        radiologyData.SaveXrayTranslateMapping(mappingModel);
                    }
                    this.Close();
                }
            }
            catch (Exception er)
            {
                System.Windows.Forms.MessageBox.Show(er.Message, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        void TranslateXrayPopup_Loaded(object sender, RoutedEventArgs e)
        {
            radiologyData = new RadiologyService();

            TranslateConditions = new List<object>();

            if (txtOrderName.Text.ToLower().Contains("chest"))
            {
                typeid = 1;
            }
            else if (txtOrderName.Text.ToLower().Contains("mammo") || txtOrderName.Text.ToLower().Contains("breast"))
            {
                typeid = 3;
            }
            else if (txtOrderName.Text.ToLower().Contains("ultrasound") || txtOrderName.Text.ToLower().StartsWith("us w"))
            {
                typeid = 2;
            }

            //TranslateConditions = radiologyData.GetXrayTranslateCondition(typeid);
            TranslateConditions.AddRange(radiologyData.GetXrayTranslateCondition(typeid).Select(p => p.Name));
            this.DataContext = this;
        }

    }

    public class XrayMappingEdit : ObservableObject
    {
        private RadiologyService _radiologyData;

        public RadiologyService radiologyData
        {
            get { return _radiologyData ?? (_radiologyData = new RadiologyService()); }
            set { _radiologyData = value; }
        }

        private bool _IsKeyWord;

        public bool IsKeyWord
        {
            get { return _IsKeyWord; }
            set
            {
                Set(ref _IsKeyWord, value);
                if (IsKeyWord)
                {
                    ThaiResult = "";
                }
            }
        }

        public string EngResult { get; set; }
        private int _XrayTranslateConditionUID;

        public int XrayTranslateConditionUID
        {
            get { return _XrayTranslateConditionUID; }
            set
            {
                Set(ref _XrayTranslateConditionUID, value);
            }
        }

        private string _XrayTranslateConditionName;

        public string XrayTranslateConditionName
        {
            get { return _XrayTranslateConditionName; }
            set
            {
                Set(ref _XrayTranslateConditionName, value);
                TranslateConditionsDetails = new ObservableCollection<object>();
                TranslateConditionsDetails2 = new List<XrayTranslateConditionDetailModel>();
                var dataDetail = radiologyData.GetXrayTranslateConditionDetailByConditionName(XrayTranslateConditionName);
                if (dataDetail != null)
                {
                    foreach (var item in dataDetail)
                    {
                        TranslateConditionsDetails.Add(item.Name);
                    }
                }
                TranslateConditionsDetails2 = dataDetail;
                XrayTranslateConditionDetailName = null;
            }
        }


        public int XrayTranslateConditionDetailUID { get; set; }

        private string _XrayTranslateConditionDetailName;

        public string XrayTranslateConditionDetailName
        {
            get { return _XrayTranslateConditionDetailName; }
            set
            {
                Set(ref _XrayTranslateConditionDetailName, value);
                if (_XrayTranslateConditionDetailName != null)
                {
                    var conditionDetail = TranslateConditionsDetails2.Where(p => p.Name == _XrayTranslateConditionDetailName).ToList();
                    if (conditionDetail != null && conditionDetail.Count > 0)
                    {
                        ThaiResult = conditionDetail.FirstOrDefault().Description;
                    }
                }
            }
        }


        private ObservableCollection<object> _TranslateConditionsDetails;

        public ObservableCollection<object> TranslateConditionsDetails
        {
            get
            {
                return _TranslateConditionsDetails;
            }
            set
            {
                Set(ref _TranslateConditionsDetails, value);
            }
        }

        private List<XrayTranslateConditionDetailModel> _TranslateConditionsDetails2;

        public List<XrayTranslateConditionDetailModel> TranslateConditionsDetails2
        {
            get
            {
                return _TranslateConditionsDetails2;
            }
            set
            {
                Set(ref _TranslateConditionsDetails2, value);
            }
        }

        private XrayTranslateConditionDetailModel _SelectTranslateConditionsDetails;

        public XrayTranslateConditionDetailModel SelectTranslateConditionsDetails
        {
            get
            {
                return _SelectTranslateConditionsDetails;
            }
            set
            {
                _SelectTranslateConditionsDetails = value;
                if (SelectTranslateConditionsDetails != null)
                {
                    ThaiResult = SelectTranslateConditionsDetails.Description;
                }
            }
        }

        private string _ThaiResult;

        public string ThaiResult
        {
            get { return _ThaiResult; }
            set
            {
                Set(ref _ThaiResult, value);
                if (!string.IsNullOrEmpty(ThaiResult))
                {
                    IsKeyWord = false;
                }
            }
        }

    }
}
