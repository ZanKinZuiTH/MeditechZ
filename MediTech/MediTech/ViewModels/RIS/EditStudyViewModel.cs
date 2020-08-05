using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediTech.ViewModels
{
    public class EditStudyViewModel : MediTechViewModelBase
    {
        #region Properties

        private string _PatientID;

        public string PatientID
        {
            get { return _PatientID; }
            set { Set(ref _PatientID, value); }
        }

        private string _PatientName;

        public string PatientName
        {
            get { return _PatientName; }
            set { Set(ref _PatientName, value); }
        }

        private DateTime? _DateofBirth;

        public DateTime? DateofBirth
        {
            get { return _DateofBirth; }
            set { Set(ref _DateofBirth, value); }
        }

        private string _AccessionNumber;

        public string AccessionNumber
        {
            get { return _AccessionNumber; }
            set { Set(ref _AccessionNumber, value); }
        }

        private DateTime? _StudyDate;

        public DateTime? StudyDate
        {
            get { return _StudyDate; }
            set { Set(ref _StudyDate, value); }
        }

        private DateTime? _StudyTime;

        public DateTime? StudyTime
        {
            get { return _StudyTime; }
            set { Set(ref _StudyTime, value); }
        }

        private List<LookupReferenceValueModel> _PatientSexs;

        public List<LookupReferenceValueModel> PatientSexs
        {
            get { return _PatientSexs; }
            set
            {
                Set(ref _PatientSexs, value);
            }
        }

        private LookupReferenceValueModel _SelectPatientSex;

        public LookupReferenceValueModel SelectPatientSex
        {
            get { return _SelectPatientSex; }
            set { Set(ref _SelectPatientSex, value); }
        }

        private List<LookupReferenceValueModel> _Modalitys;

        public List<LookupReferenceValueModel> Modalitys
        {
            get { return _Modalitys; }
            set
            {
                Set(ref _Modalitys, value);
            }
        }

        private LookupReferenceValueModel _SelectModality;

        public LookupReferenceValueModel SelectModality
        {
            get { return _SelectModality; }
            set { Set(ref _SelectModality, value); }
        }


        private StudiesModel _StudyModel;

        public StudiesModel StudyModel
        {
            get { return _StudyModel; }

            set { _StudyModel = value; }
        }

        #endregion

        #region Command

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

        private RelayCommand _EditCommand;

        /// <summary>
        /// Gets the EditStudyCommand.
        /// </summary>
        public RelayCommand EditCommand
        {
            get
            {
                return _EditCommand
                    ?? (_EditCommand = new RelayCommand(EditData));
            }
        }

        #endregion

        #region Method

        public EditStudyViewModel()
        {
            PatientSexs = new List<LookupReferenceValueModel>();
            Modalitys = new List<LookupReferenceValueModel>();
            PatientSexs.AddRange(
                new List<LookupReferenceValueModel>
                { new LookupReferenceValueModel {Key = 1,Display = "M" }
                ,new LookupReferenceValueModel {Key = 2,Display = "F" }
                ,new LookupReferenceValueModel {Key = 3,Display = "O"  } });

            Modalitys.AddRange(new List<LookupReferenceValueModel>
            { new LookupReferenceValueModel {Key = 1,Display = "CR" }
            ,new LookupReferenceValueModel {Key = 2,Display = "DX" }
                ,new LookupReferenceValueModel {Key = 3,Display = "CT" }
                ,new LookupReferenceValueModel {Key = 4,Display = "ES"  }
                ,new LookupReferenceValueModel {Key = 5,Display = "MR" }
                ,new LookupReferenceValueModel {Key = 6,Display = "MG" }
                ,new LookupReferenceValueModel {Key = 7,Display = "OT" }
                ,new LookupReferenceValueModel {Key = 8,Display = "RF"  }
            ,new LookupReferenceValueModel {Key = 9,Display = "US"  } });
        }

        public override void OnLoaded()
        {
            base.OnLoaded();
            PatientID = StudyModel.PatientID.Trim();
            PatientName = StudyModel.PatientName;

            DateTime dateofBirth;
            if (DateTime.TryParse(StudyModel.PatientBirthDate, out dateofBirth))
            {
                DateofBirth = dateofBirth;
            }
            AccessionNumber = StudyModel.AccessionNumber;
            DateTime studyDateTime;
            if (DateTime.TryParse(StudyModel.StudyDate.ToString("dd/MM/yyyy") + " " + StudyModel.StudyTime, out studyDateTime))
            {
                StudyDate = studyDateTime;
                StudyTime = studyDateTime;
            }

            SelectPatientSex = PatientSexs.FirstOrDefault(p => p.Display == StudyModel.PatientSex);
            SelectModality = Modalitys.FirstOrDefault(p => p.Display == StudyModel.ModalitiesInStudy);
        }

        void EditData()
        {
            try
            {
                if (string.IsNullOrEmpty(PatientID))
                {
                    WarningDialog("กรุณาระบุ ID");
                    return;
                }
                if (SelectModality == null)
                {
                    WarningDialog("กรุณาระบุ Modality");
                    return;
                }
                if (StudyDate == null)
                {
                    WarningDialog("กรุณาระบุ Study Date");
                    return;
                }
                if (StudyTime == null)
                {
                    WarningDialog("กรุณาระบุ Study Time");
                    return;
                }
                StudyModel.PatientID2 = PatientID.Trim();
                StudyModel.PatientName = PatientName;
                StudyModel.PatientBirthDate = DateofBirth?.ToString("yyyy/MM/dd");
                StudyModel.AccessionNumber = AccessionNumber;
                StudyModel.StudyDate = StudyDate.Value;
                StudyModel.StudyTime = StudyTime.Value.ToString("HH:mm:ss");
                StudyModel.PatientSex = SelectPatientSex?.Display;
                StudyModel.ModalitiesInStudy = SelectModality?.Display;
                DataService.PACS.EditStudy(StudyModel);
                SaveSuccessDialog();
                CloseViewDialog(ActionDialog.Save);
            }
            catch (Exception ex)
            {

                ErrorDialog(ex.Message);
            }
        }

        void Cancel()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        #endregion
    }
}
