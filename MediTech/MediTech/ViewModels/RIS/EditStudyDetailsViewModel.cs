using GalaSoft.MvvmLight.Command;
using MediTech.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MediTech.ViewModels
{
    public class EditStudyDetailsViewModel : MediTechViewModelBase
    {
        #region Properties

        public StudiesModel SelectedStudy { get; set; }
        public StudiesModel OriginalStudy { get; private set; }

        private string _BodyPartsInStudy;
        public string BodyPartsInStudy
        {
            get { return _BodyPartsInStudy; }
            set { Set(ref _BodyPartsInStudy, value); DebounceChanges(); }
        }

        private string _StudyDescription;
        public string StudyDescription
        {
            get { return _StudyDescription; }
            set { Set(ref _StudyDescription, value); DebounceChanges(); }
        }

        private string _ModalitiesInStudy;
        public string ModalitiesInStudy
        {
            get { return _ModalitiesInStudy; }
            set { Set(ref _ModalitiesInStudy, value); DebounceChanges(); }
        }

        private string _PatientComments;
        public string PatientComments
        {
            get { return _PatientComments; }
            set { Set(ref _PatientComments, value); DebounceChanges(); }
        }

        private List<StudyAuditLogEntry> _AuditHistory;
        public List<StudyAuditLogEntry> AuditHistory
        {
            get { return _AuditHistory; }
            set { Set(ref _AuditHistory, value); }
        }

        private bool _HasChanges;
        public bool HasChanges
        {
            get { return _HasChanges; }
            set { Set(ref _HasChanges, value); }
        }

        private string _ChangeSummary;
        public string ChangeSummary
        {
            get { return _ChangeSummary; }
            set { Set(ref _ChangeSummary, value); }
        }

        public bool IsAuthorized { get; private set; }

        #endregion

        #region Commands

        private RelayCommand _SaveChangesCommand;
        public RelayCommand SaveChangesCommand
        {
            get { return _SaveChangesCommand ?? (_SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges)); }
        }

        private RelayCommand _CancelChangesCommand;
        public RelayCommand CancelChangesCommand
        {
            get { return _CancelChangesCommand ?? (_CancelChangesCommand = new RelayCommand(CancelChanges)); }
        }

        private RelayCommand _LoadAuditHistoryCommand;
        public RelayCommand LoadAuditHistoryCommand
        {
            get { return _LoadAuditHistoryCommand ?? (_LoadAuditHistoryCommand = new RelayCommand(LoadAuditHistory)); }
        }

        #endregion

        #region Lifecycle

        public override void OnLoaded()
        {
            base.OnLoaded();
            Initialize(SelectedStudy);
            LoadAuditHistory();
        }

        public void Initialize(StudiesModel study)
        {
            if (study == null) return;
            OriginalStudy = CloneStudy(study);
            SelectedStudy = study;

            BodyPartsInStudy = study.BodyPartsInStudy;
            StudyDescription = study.StudyDescription;
            ModalitiesInStudy = study.ModalitiesInStudy;
            PatientComments = study.PatientComments;

            IsAuthorized = (AppUtil.Current.IsAdminRadiologist == true) || (AppUtil.Current.IsRadiologist == true) || (AppUtil.Current.IsRDUStaff == true) || (AppUtil.Current.IsAdmin == true);
            RaisePropertyChanged(() => IsAuthorized);
            CheckForChanges();
        }

        #endregion

        #region Methods

        private bool CanSaveChanges()
        {
            return IsAuthorized && HasChanges && ValidateInputs(silent: true);
        }

        private bool ValidateInputs(bool silent = false)
        {
            if (string.IsNullOrWhiteSpace(ModalitiesInStudy))
            {
                if (!silent) WarningDialog("กรุณาระบุ Modality");
                return false;
            }
            else
            {
                var allowedModalities = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase)
                { "CR","DX","CT","ES","MR","MG","OT","RF","US" };
                if (!allowedModalities.Contains(ModalitiesInStudy))
                {
                    if (!silent) WarningDialog("ค่า Modality ไม่ถูกต้อง (อนุญาตเฉพาะ CR, DX, CT, ES, MR, MG, OT, RF, US)");
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(StudyDescription) && StudyDescription.Length > 256)
            {
                if (!silent) WarningDialog("รายละเอียดการตรวจยาวเกินกำหนด (สูงสุด 256 ตัวอักษร)");
                return false;
            }
            if (!string.IsNullOrEmpty(BodyPartsInStudy) && BodyPartsInStudy.Length > 512)
            {
                if (!silent) WarningDialog("อวัยวะที่ตรวจยาวเกินกำหนด (สูงสุด 512 ตัวอักษร)");
                return false;
            }
            if (!string.IsNullOrEmpty(ModalitiesInStudy) && ModalitiesInStudy.Length > 256)
            {
                if (!silent) WarningDialog("Modality ยาวเกินกำหนด (สูงสุด 256 ตัวอักษร)");
                return false;
            }
            if (!string.IsNullOrEmpty(PatientComments) && PatientComments.Length > 4000)
            {
                if (!silent) WarningDialog("หมายเหตุผู้ป่วยยาวเกินกำหนด (สูงสุด 4000 ตัวอักษร)");
                return false;
            }
            return true;
        }

        public void CheckForChanges()
        {
            if (OriginalStudy == null)
            {
                HasChanges = false;
                ChangeSummary = string.Empty;
                SaveChangesCommand.RaiseCanExecuteChanged();
                return;
            }

            var diffs = GetChanges();
            HasChanges = diffs.Count > 0;
            if (HasChanges)
            {
                var sb = new StringBuilder();
                sb.AppendLine("สรุปการเปลี่ยนแปลง:");
                foreach (var c in diffs)
                {
                    sb.AppendLine(string.Format("- {0}: '{1}' → '{2}'", TranslateField(c.FieldName), Safe(c.OldValue), Safe(c.NewValue)));
                }
                ChangeSummary = sb.ToString();
            }
            else
            {
                ChangeSummary = string.Empty;
            }
            SaveChangesCommand.RaiseCanExecuteChanged();
        }

        private DispatcherTimer _debounceTimer;
        private void DebounceChanges()
        {
            if (_debounceTimer == null)
            {
                _debounceTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromMilliseconds(200)
                };
                _debounceTimer.Tick += (s, e) =>
                {
                    _debounceTimer.Stop();
                    CheckForChanges();
                };
            }
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private static string Safe(string v)
        {
            return v ?? "";
        }

        private string TranslateField(string field)
        {
            switch (field)
            {
                case nameof(BodyPartsInStudy): return "อวัยวะที่ตรวจ";
                case nameof(StudyDescription): return "รายละเอียดการตรวจ";
                case nameof(ModalitiesInStudy): return "โมดาลิตี้";
                case nameof(PatientComments): return "หมายเหตุผู้ป่วย";
                default: return field;
            }
        }

        public List<StudyDetailChange> GetChanges()
        {
            var changes = new List<StudyDetailChange>();
            if (BodyPartsInStudy != OriginalStudy.BodyPartsInStudy)
                changes.Add(new StudyDetailChange { FieldName = nameof(BodyPartsInStudy), OldValue = OriginalStudy.BodyPartsInStudy, NewValue = BodyPartsInStudy });
            if (StudyDescription != OriginalStudy.StudyDescription)
                changes.Add(new StudyDetailChange { FieldName = nameof(StudyDescription), OldValue = OriginalStudy.StudyDescription, NewValue = StudyDescription });
            if (ModalitiesInStudy != OriginalStudy.ModalitiesInStudy)
                changes.Add(new StudyDetailChange { FieldName = nameof(ModalitiesInStudy), OldValue = OriginalStudy.ModalitiesInStudy, NewValue = ModalitiesInStudy });
            if (PatientComments != OriginalStudy.PatientComments)
                changes.Add(new StudyDetailChange { FieldName = nameof(PatientComments), OldValue = OriginalStudy.PatientComments, NewValue = PatientComments });
            return changes;
        }

        public async void SaveChanges()
        {
            try
            {
                if (!ValidateInputs()) return;
                if (!HasChanges)
                {
                    WarningDialog("ไม่มีการเปลี่ยนแปลง");
                    return;
                }

                var confirm = ConfirmDialog("ยืนยันบันทึกการแก้ไขรายละเอียดการตรวจ?", "ยืนยันการบันทึก");
                if (!confirm) return;

                var request = new UpdateStudyDetailsRequest
                {
                    StudyInstanceUID = SelectedStudy.StudyInstanceUID,
                    BodyPartsInStudy = BodyPartsInStudy,
                    StudyDescription = StudyDescription,
                    ModalitiesInStudy = ModalitiesInStudy,
                    PatientComments = PatientComments,
                    Changes = GetChanges(),
                    ModifiedBy = AppUtil.Current.UserID,
                    ModifiedByName = AppUtil.Current.UserName,
                    RoleUID = AppUtil.Current.RoleUID,
                    RoleName = AppUtil.Current.RoleName,
                    ActionType = "Edit",
                    OrganisationUID = AppUtil.Current.OwnerOrganisationUID
                };

                bool ok = await Task.Run(() => DataService.PACS.UpdateStudyDetailsWithAudit(request));
                if (ok)
                {
                    // sync back to SelectedStudy
                    SelectedStudy.BodyPartsInStudy = BodyPartsInStudy;
                    SelectedStudy.StudyDescription = StudyDescription;
                    SelectedStudy.ModalitiesInStudy = ModalitiesInStudy;
                    SelectedStudy.PatientComments = PatientComments;
                    OriginalStudy = CloneStudy(SelectedStudy);
                    CheckForChanges();
                    SaveSuccessDialog();
                    CloseViewDialog(ActionDialog.Save);
                }
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }
        }

        public void CancelChanges()
        {
            CloseViewDialog(ActionDialog.Cancel);
        }

        public void LoadAuditHistory()
        {
            try
            {
                if (SelectedStudy == null) return;
                AuditHistory = DataService.PACS.GetStudyAuditHistory(SelectedStudy.StudyInstanceUID) ?? new List<StudyAuditLogEntry>();
            }
            catch (Exception ex)
            {
                ErrorDialog(ex.Message);
            }
        }

        public StudiesModel CloneStudy(StudiesModel study)
        {
            return new StudiesModel
            {
                StudyInstanceUID = study.StudyInstanceUID,
                BodyPartsInStudy = study.BodyPartsInStudy,
                StudyDescription = study.StudyDescription,
                ModalitiesInStudy = study.ModalitiesInStudy,
                PatientComments = study.PatientComments
            };
        }

        #endregion
    }
}


