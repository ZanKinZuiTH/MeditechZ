using DevExpress.XtraRichEdit.API.Native;
using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using MediTech.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class ManageRadiologistTemplateViewModel : MediTechViewModelBase
    {
        #region Properites

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { Set(ref _Name, value); }
        }

        private string _Description;

        public string Description
        {
            get { return _Description; }
            set { Set(ref _Description, value); }
        }


        private List<LookupReferenceValueModel> _ImageTypeList;

        public List<LookupReferenceValueModel> ImageTypeList
        {
            get { return _ImageTypeList; }
            set { _ImageTypeList = value; }
        }

        private List<LookupReferenceValueModel> _Gender;

        public List<LookupReferenceValueModel> Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }

        private List<LookupReferenceValueModel> _ResultStatus;

        public List<LookupReferenceValueModel> ResultStatus
        {
            get { return _ResultStatus; }
            set { _ResultStatus = value; }
        }

        private LookupReferenceValueModel _SelectImageType;

        public LookupReferenceValueModel SelectImageType
        {
            get { return _SelectImageType; }
            set { Set(ref _SelectImageType, value); }
        }

        private LookupReferenceValueModel _SelectGender;

        public LookupReferenceValueModel SelectGender
        {
            get { return _SelectGender; }
            set { Set(ref _SelectGender, value); }
        }

        private LookupReferenceValueModel _SelectResultStatus;

        public LookupReferenceValueModel SelectResultStatus
        {
            get { return _SelectResultStatus; }
            set { Set(ref _SelectResultStatus, value); }
        }
        private bool _IsPublic;

        public bool IsPublic
        {
            get { return _IsPublic; }
            set { Set(ref _IsPublic, value); }
        }

        private bool _IsPrimary;

        public bool IsPrimary
        {
            get { return _IsPrimary; }
            set { Set(ref _IsPrimary, value); }
        }

        public Document Document { get; set; }
        #endregion

        #region Command

        private RelayCommand _SaveCommand;

        /// <summary>
        /// Gets the SaveCommand.
        /// </summary>
        public RelayCommand SaveCommand
        {
            get
            {
                return _SaveCommand
                    ?? (_SaveCommand = new RelayCommand(SaveTemplate));
            }
        }

        private RelayCommand _CancelCommand;

        /// <summary>
        /// Gets the CancelCommand.
        /// </summary>
        public RelayCommand CancelCommand
        {
            get
            {
                return _CancelCommand
                    ?? (_CancelCommand = new RelayCommand(Cancel));
            }
        }



        #endregion

        #region Method

        ResultRadiologyTemplateModel ResultTemplateModel;
        public ManageRadiologistTemplateViewModel()
        {
            var refValues = DataService.Technical.GetReferenceValueList("SEXXX,RABSTS,RIMTYP");
            ImageTypeList = refValues.Where(p => p.DomainCode == "RIMTYP").ToList();
            Gender = refValues.Where(p => p.DomainCode == "SEXXX").ToList();
            ResultStatus = refValues.Where(p => p.DomainCode == "RABSTS").ToList();
        }
        private void Cancel()
        {
            ListRadiologistTemplate listView = new ListRadiologistTemplate();
            ChangeViewPermission(listView);
        }
        private void SaveTemplate()
        {
            try
            {
                if (Name == "")
                {
                    WarningDialog("กรุณาใส่ชื่อ");
                    return;
                }
                if (SelectResultStatus == null)
                {
                    WarningDialog("กรุณาเลือกสถานะ");
                    return;
                }
                AssingProperitesToModel();
                DataService.Radiology.SaveResultRadiologyTemplate(ResultTemplateModel, AppUtil.Current.UserID);
                SaveSuccessDialog();
                ListRadiologistTemplate listView = new ListRadiologistTemplate();
                ChangeViewPermission(listView);
            }
            catch (Exception er)
            {

                ErrorDialog(er.Message);
            }
        }

        public void AssingModel(int resultTemplateUID)
        {
            ResultTemplateModel = DataService.Radiology.GetResultRaiologyTemplateByUID(resultTemplateUID);
            if (ResultTemplateModel != null)
            {
                Name = ResultTemplateModel.Name;
                Description = ResultTemplateModel.Description;
                IsPrimary = ResultTemplateModel.IsPrimary;
                IsPublic = ResultTemplateModel.IsPublic;
                SelectGender = Gender.FirstOrDefault(p => p.Key == ResultTemplateModel.SEXXXUID);
                SelectImageType = ImageTypeList.FirstOrDefault(p => p.Key == ResultTemplateModel.RIMTYPUID);
                SelectResultStatus = ResultStatus.FirstOrDefault(p => p.Key == ResultTemplateModel.RABSTSUID);
                Document.HtmlText = ResultTemplateModel.Value;
            }
        }

        private void AssingProperitesToModel()
        {
            if (ResultTemplateModel == null)
            {
                ResultTemplateModel = new ResultRadiologyTemplateModel();
            }
            string htmlText = Document.GetHtmlText(Document.Range, null);
            string plainText = Document.GetText(Document.Range);
            ResultTemplateModel.Name = Name;
            ResultTemplateModel.Description = Description;
            ResultTemplateModel.IsPrimary = IsPrimary;
            ResultTemplateModel.IsPublic = IsPublic;
            ResultTemplateModel.SEXXXUID = SelectGender != null ? SelectGender.Key : (int?)null;
            ResultTemplateModel.RIMTYPUID = SelectImageType != null ? SelectImageType.Key : (int?)null;
                
            if (SelectResultStatus != null)
                ResultTemplateModel.RABSTSUID = SelectResultStatus.Key;

            ResultTemplateModel.Value = htmlText;
            ResultTemplateModel.PlainText = plainText;
        }
        #endregion
    }
}
