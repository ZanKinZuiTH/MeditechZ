# 📋 แผนการพัฒนาฟีเจอร์การแก้ไขรายละเอียด X-ray จากหน้างาน

<div align="center">


**MediTech Hospital Information System**  
*แผนการพัฒนาฟีเจอร์การแก้ไขรายละเอียด X-ray*

---

**วันที่จัดทำ**: `2024-01-15`  
**รุ่นเอกสาร**: `v1.0`  
**สถานะ**: `Development Plan`

</div>

---

## 📋 สรุปผู้บริหาร (Executive Summary)

### 🎯 **วัตถุประสงค์**
พัฒนาฟีเจอร์การแก้ไขรายละเอียด X-ray จากหน้างานโดยตรง เพื่อให้เจ้าหน้าที่สามารถแก้ไขข้อมูลที่ผิดพลาดได้ทันที พร้อมระบบ Audit Log ที่ครบถ้วน

### ⚡ **ประโยชน์ที่คาดหวัง**
- **เพิ่มประสิทธิภาพการทำงาน**: ลดเวลาในการแก้ไขข้อมูลผิดพลาด
- **ความแม่นยำสูงขึ้น**: การแก้ไขข้อมูลแบบ Real-time
- **การติดตามที่ดี**: ระบบ Audit Log ครบถ้วน
- **ความยืดหยุ่น**: แก้ไขได้จากหน้างานโดยไม่ต้องผ่านระบบอื่น

---

## 🔍 การวิเคราะห์ระบบปัจจุบัน

### **1. โครงสร้าง Navigation Menu**

#### **1.1 ระบบเมนูหลัก**
```csharp
// MainViewModel.cs - CreateMenuByRole()
var pageViewModule = DataService.MainManage.GetPageViewModule().OrderBy(p => p.DisplayOrder).ToList();
```

#### **1.2 โครงสร้าง RIS Module**
- **RIS** → **PACS WorkList** → **Study Detail View**
- **RIS** → **Radiology Exam List** → **PACS WorkList**

### **2. โครงสร้างข้อมูลปัจจุบัน**

#### **2.1 StudiesModel Structure**
```csharp
public class StudiesModel
{
    public string StudyInstanceUID { get; set; }
    public string PatientID { get; set; }
    public string PatientName { get; set; }
    public string StudyDescription { get; set; }
    public string BodyPartsInStudy { get; set; }  // ✅ มีฟิลด์นี้แล้ว
    public string ModalitiesInStudy { get; set; }
    // ... อื่นๆ
}
```

#### **2.2 Database Schema**
```sql
-- ตาราง Studies
CREATE TABLE Studies (
    StudyInstanceUID NVARCHAR(128) PRIMARY KEY,
    BodyPartsInStudy NVARCHAR(512),  -- ✅ มีฟิลด์นี้แล้ว
    ModalitiesInStudy NVARCHAR(256),
    -- ... อื่นๆ
);
```

### **3. ระบบ Audit Log ที่มีอยู่**

#### **3.1 PatientDemographicLog Structure**
```csharp
public class PatientDemographicLogModel
{
    public long UID { get; set; }
    public long PatientUID { get; set; }
    public string TableName { get; set; }
    public string FiledName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
    public DateTime? ModifiedDttm { get; set; }
    public int Modifiedby { get; set; }
    public string ModifiedbyName { get; set; }
    // ... อื่นๆ
}
```

---

## 🎯 แนวทางการพัฒนาฟีเจอร์

### **Phase 1: Database Layer**

#### **1.1 สร้างตาราง Audit Log สำหรับ PACS**
```sql
-- ตาราง PACSStudyAuditLog
CREATE TABLE PACSStudyAuditLog (
    UID BIGINT IDENTITY(1,1) PRIMARY KEY,
    StudyInstanceUID NVARCHAR(128) NOT NULL,
    TableName NVARCHAR(100) NOT NULL,
    FieldName NVARCHAR(100) NOT NULL,
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    ModifiedDttm DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedBy INT NOT NULL,
    ModifiedByName NVARCHAR(256),
    OwnerOrganisationUID INT NOT NULL,
    OwnerOrganisationName NVARCHAR(256),
    ActionType NVARCHAR(50), -- 'UPDATE', 'INSERT', 'DELETE'
    IPAddress NVARCHAR(50),
    UserAgent NVARCHAR(500),
    CUser INT NOT NULL,
    CWhen DATETIME NOT NULL DEFAULT GETDATE(),
    MUser INT NOT NULL,
    MWhen DATETIME NOT NULL DEFAULT GETDATE(),
    StatusFlag CHAR(1) NOT NULL DEFAULT 'A'
);

-- Index สำหรับ Performance
CREATE INDEX IX_PACSStudyAuditLog_StudyInstanceUID ON PACSStudyAuditLog(StudyInstanceUID);
CREATE INDEX IX_PACSStudyAuditLog_ModifiedDttm ON PACSStudyAuditLog(ModifiedDttm);
CREATE INDEX IX_PACSStudyAuditLog_ModifiedBy ON PACSStudyAuditLog(ModifiedBy);
```

#### **1.2 สร้าง Stored Procedure สำหรับ Audit Log**
```sql
-- SP_InsertPACSStudyAuditLog
CREATE PROCEDURE SP_InsertPACSStudyAuditLog
    @StudyInstanceUID NVARCHAR(128),
    @TableName NVARCHAR(100),
    @FieldName NVARCHAR(100),
    @OldValue NVARCHAR(MAX),
    @NewValue NVARCHAR(MAX),
    @ModifiedBy INT,
    @ModifiedByName NVARCHAR(256),
    @OwnerOrganisationUID INT,
    @OwnerOrganisationName NVARCHAR(256),
    @ActionType NVARCHAR(50),
    @IPAddress NVARCHAR(50),
    @UserAgent NVARCHAR(500)
AS
BEGIN
    INSERT INTO PACSStudyAuditLog (
        StudyInstanceUID, TableName, FieldName, OldValue, NewValue,
        ModifiedDttm, ModifiedBy, ModifiedByName, OwnerOrganisationUID,
        OwnerOrganisationName, ActionType, IPAddress, UserAgent,
        CUser, CWhen, MUser, MWhen, StatusFlag
    ) VALUES (
        @StudyInstanceUID, @TableName, @FieldName, @OldValue, @NewValue,
        GETDATE(), @ModifiedBy, @ModifiedByName, @OwnerOrganisationUID,
        @OwnerOrganisationName, @ActionType, @IPAddress, @UserAgent,
        @ModifiedBy, GETDATE(), @ModifiedBy, GETDATE(), 'A'
    );
END
```

### **Phase 2: Data Service Layer**

#### **2.1 สร้าง PACSStudyAuditLogModel**
```csharp
// MediTech.Model/PACS/PACSStudyAuditLogModel.cs
namespace MediTech.Model
{
    public class PACSStudyAuditLogModel
    {
        public long UID { get; set; }
        public string StudyInstanceUID { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ModifiedDttm { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public int OwnerOrganisationUID { get; set; }
        public string OwnerOrganisationName { get; set; }
        public string ActionType { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public int CUser { get; set; }
        public DateTime CWhen { get; set; }
        public int MUser { get; set; }
        public DateTime MWhen { get; set; }
        public string StatusFlag { get; set; }
    }
}
```

#### **2.2 เพิ่มเมธอดใน PACS DataService**
```csharp
// MediTech.DataService/PACS/PACSService.cs
public class PACSService
{
    // ... existing methods ...

    /// <summary>
    /// อัพเดทรายละเอียด Study พร้อม Audit Log
    /// </summary>
    public bool UpdateStudyDetailsWithAudit(StudiesModel study, List<StudyFieldChange> changes, int userId)
    {
        using (var transaction = context.Database.BeginTransaction())
        {
            try
            {
                // 1. อัพเดทข้อมูล Study
                var existingStudy = context.Studies.Find(study.StudyInstanceUID);
                if (existingStudy == null) return false;

                // 2. บันทึก Audit Log สำหรับแต่ละฟิลด์ที่เปลี่ยนแปลง
                foreach (var change in changes)
                {
                    var auditLog = new PACSStudyAuditLog
                    {
                        StudyInstanceUID = study.StudyInstanceUID,
                        TableName = "Studies",
                        FieldName = change.FieldName,
                        OldValue = change.OldValue,
                        NewValue = change.NewValue,
                        ModifiedBy = userId,
                        ModifiedByName = GetUserName(userId),
                        OwnerOrganisationUID = AppUtil.Current.OwnerOrganisationUID,
                        OwnerOrganisationName = AppUtil.Current.OwnerOrganisationName,
                        ActionType = "UPDATE",
                        IPAddress = GetClientIPAddress(),
                        UserAgent = GetUserAgent(),
                        CUser = userId,
                        CWhen = DateTime.Now,
                        MUser = userId,
                        MWhen = DateTime.Now,
                        StatusFlag = "A"
                    };
                    context.PACSStudyAuditLogs.Add(auditLog);
                }

                // 3. อัพเดทข้อมูล Study
                existingStudy.BodyPartsInStudy = study.BodyPartsInStudy;
                existingStudy.StudyDescription = study.StudyDescription;
                existingStudy.ModalitiesInStudy = study.ModalitiesInStudy;
                // ... อื่นๆ ตาม changes

                existingStudy.MWhen = DateTime.Now;
                existingStudy.MUser = userId;

                context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception($"เกิดข้อผิดพลาดในการอัพเดทข้อมูล: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// ดึงประวัติการเปลี่ยนแปลงข้อมูล Study
    /// </summary>
    public List<PACSStudyAuditLogModel> GetStudyAuditHistory(string studyInstanceUID)
    {
        return context.PACSStudyAuditLogs
            .Where(x => x.StudyInstanceUID == studyInstanceUID && x.StatusFlag == "A")
            .OrderByDescending(x => x.ModifiedDttm)
            .Select(x => new PACSStudyAuditLogModel
            {
                UID = x.UID,
                StudyInstanceUID = x.StudyInstanceUID,
                TableName = x.TableName,
                FieldName = x.FieldName,
                OldValue = x.OldValue,
                NewValue = x.NewValue,
                ModifiedDttm = x.ModifiedDttm,
                ModifiedBy = x.ModifiedBy,
                ModifiedByName = x.ModifiedByName,
                OwnerOrganisationUID = x.OwnerOrganisationUID,
                OwnerOrganisationName = x.OwnerOrganisationName,
                ActionType = x.ActionType,
                IPAddress = x.IPAddress,
                UserAgent = x.UserAgent,
                CUser = x.CUser,
                CWhen = x.CWhen,
                MUser = x.MUser,
                MWhen = x.MWhen,
                StatusFlag = x.StatusFlag
            }).ToList();
    }
}

// Helper Class สำหรับเก็บข้อมูลการเปลี่ยนแปลง
public class StudyFieldChange
{
    public string FieldName { get; set; }
    public string OldValue { get; set; }
    public string NewValue { get; set; }
}
```

### **Phase 3: ViewModel Layer**

#### **3.1 สร้าง EditStudyDetailsViewModel**
```csharp
// MediTech.ViewModels/RIS/EditStudyDetailsViewModel.cs
namespace MediTech.ViewModels
{
    public class EditStudyDetailsViewModel : MediTechViewModelBase
    {
        #region Properties

        private StudiesModel _SelectedStudy;
        public StudiesModel SelectedStudy
        {
            get { return _SelectedStudy; }
            set { Set(ref _SelectedStudy, value); }
        }

        private StudiesModel _OriginalStudy;
        public StudiesModel OriginalStudy
        {
            get { return _OriginalStudy; }
            set { Set(ref _OriginalStudy, value); }
        }

        private List<PACSStudyAuditLogModel> _AuditHistory;
        public List<PACSStudyAuditLogModel> AuditHistory
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

        #endregion

        #region Commands

        private RelayCommand _SaveChangesCommand;
        public RelayCommand SaveChangesCommand
        {
            get
            {
                return _SaveChangesCommand
                    ?? (_SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges));
            }
        }

        private RelayCommand _CancelChangesCommand;
        public RelayCommand CancelChangesCommand
        {
            get
            {
                return _CancelChangesCommand
                    ?? (_CancelChangesCommand = new RelayCommand(CancelChanges));
            }
        }

        private RelayCommand _LoadAuditHistoryCommand;
        public RelayCommand LoadAuditHistoryCommand
        {
            get
            {
                return _LoadAuditHistoryCommand
                    ?? (_LoadAuditHistoryCommand = new RelayCommand(LoadAuditHistory));
            }
        }

        #endregion

        #region Methods

        public EditStudyDetailsViewModel()
        {
            // Initialize
        }

        public void Initialize(StudiesModel study)
        {
            SelectedStudy = study;
            OriginalStudy = CloneStudy(study);
            LoadAuditHistory();
            CheckForChanges();
        }

        private void CheckForChanges()
        {
            if (SelectedStudy == null || OriginalStudy == null)
            {
                HasChanges = false;
                return;
            }

            var changes = new List<string>();

            if (SelectedStudy.BodyPartsInStudy != OriginalStudy.BodyPartsInStudy)
                changes.Add($"Body Parts: '{OriginalStudy.BodyPartsInStudy}' → '{SelectedStudy.BodyPartsInStudy}'");

            if (SelectedStudy.StudyDescription != OriginalStudy.StudyDescription)
                changes.Add($"Study Description: '{OriginalStudy.StudyDescription}' → '{SelectedStudy.StudyDescription}'");

            if (SelectedStudy.ModalitiesInStudy != OriginalStudy.ModalitiesInStudy)
                changes.Add($"Modalities: '{OriginalStudy.ModalitiesInStudy}' → '{SelectedStudy.ModalitiesInStudy}'");

            HasChanges = changes.Count > 0;
            ChangeSummary = string.Join("\n", changes);
        }

        private bool CanSaveChanges()
        {
            return HasChanges && SelectedStudy != null;
        }

        private void SaveChanges()
        {
            try
            {
                var changes = GetChanges();
                
                if (changes.Count == 0)
                {
                    ShowMessage("ไม่มีการเปลี่ยนแปลงข้อมูล", MessageType.Info);
                    return;
                }

                // ยืนยันการบันทึก
                var result = ShowConfirmDialog(
                    "ยืนยันการบันทึกการเปลี่ยนแปลง",
                    $"คุณต้องการบันทึกการเปลี่ยนแปลงต่อไปนี้หรือไม่?\n\n{ChangeSummary}",
                    MessageType.Question);

                if (result == DialogResult.Yes)
                {
                    var success = DataService.PACS.UpdateStudyDetailsWithAudit(SelectedStudy, changes, AppUtil.Current.UserID);
                    
                    if (success)
                    {
                        ShowMessage("บันทึกการเปลี่ยนแปลงเรียบร้อยแล้ว", MessageType.Success);
                        OriginalStudy = CloneStudy(SelectedStudy);
                        LoadAuditHistory();
                        CheckForChanges();
                    }
                    else
                    {
                        ShowMessage("เกิดข้อผิดพลาดในการบันทึกข้อมูล", MessageType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"เกิดข้อผิดพลาด: {ex.Message}", MessageType.Error);
            }
        }

        private void CancelChanges()
        {
            if (HasChanges)
            {
                var result = ShowConfirmDialog(
                    "ยืนยันการยกเลิก",
                    "คุณต้องการยกเลิกการเปลี่ยนแปลงทั้งหมดหรือไม่?",
                    MessageType.Question);

                if (result == DialogResult.Yes)
                {
                    SelectedStudy = CloneStudy(OriginalStudy);
                    CheckForChanges();
                }
            }
        }

        private void LoadAuditHistory()
        {
            if (SelectedStudy != null)
            {
                AuditHistory = DataService.PACS.GetStudyAuditHistory(SelectedStudy.StudyInstanceUID);
            }
        }

        private List<StudyFieldChange> GetChanges()
        {
            var changes = new List<StudyFieldChange>();

            if (SelectedStudy.BodyPartsInStudy != OriginalStudy.BodyPartsInStudy)
            {
                changes.Add(new StudyFieldChange
                {
                    FieldName = "BodyPartsInStudy",
                    OldValue = OriginalStudy.BodyPartsInStudy,
                    NewValue = SelectedStudy.BodyPartsInStudy
                });
            }

            if (SelectedStudy.StudyDescription != OriginalStudy.StudyDescription)
            {
                changes.Add(new StudyFieldChange
                {
                    FieldName = "StudyDescription",
                    OldValue = OriginalStudy.StudyDescription,
                    NewValue = SelectedStudy.StudyDescription
                });
            }

            if (SelectedStudy.ModalitiesInStudy != OriginalStudy.ModalitiesInStudy)
            {
                changes.Add(new StudyFieldChange
                {
                    FieldName = "ModalitiesInStudy",
                    OldValue = OriginalStudy.ModalitiesInStudy,
                    NewValue = SelectedStudy.ModalitiesInStudy
                });
            }

            return changes;
        }

        private StudiesModel CloneStudy(StudiesModel study)
        {
            return new StudiesModel
            {
                StudyInstanceUID = study.StudyInstanceUID,
                PatientID = study.PatientID,
                PatientName = study.PatientName,
                StudyDescription = study.StudyDescription,
                BodyPartsInStudy = study.BodyPartsInStudy,
                ModalitiesInStudy = study.ModalitiesInStudy,
                StudyDate = study.StudyDate,
                StudyTime = study.StudyTime,
                AccessionNumber = study.AccessionNumber,
                InstitutionName = study.InstitutionName,
                PatientBirthDate = study.PatientBirthDate,
                PatientSex = study.PatientSex,
                PatientComments = study.PatientComments,
                NumberOfStudyRelatedSeries = study.NumberOfStudyRelatedSeries,
                NumberOfStudyRelatedInstances = study.NumberOfStudyRelatedInstances,
                SpecificCharacterSet = study.SpecificCharacterSet,
                PatientAge = study.PatientAge,
                Edited = study.Edited,
                ReportStatus = study.ReportStatus
            };
        }

        #endregion
    }
}
```

### **Phase 4: View Layer**

#### **4.1 สร้าง EditStudyDetails.xaml**
```xml
<!-- MediTech.Views/RIS/EditStudyDetails.xaml -->
<UserControl x:Class="MediTech.Views.EditStudyDetails"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:dxlc="http://schemas.devexpress.com/winfx/2008/xaml/layoutcontrol"
             xmlns:dxe="http://schemas.devexpress.com/winfx/2008/xaml/editors"
             xmlns:dx="http://schemas.devexpress.com/winfx/2008/xaml/core"
             xmlns:dxg="http://schemas.devexpress.com/winfx/2008/xaml/grid"
             DataContext="{Binding Path=EditStudyDetailsViewModel,Source={StaticResource Locator}}"
             d:DesignHeight="600" d:DesignWidth="800">

    <Grid>
        <dxlc:LayoutControl Orientation="Vertical" View="Group">
            
            <!-- Header Section -->
            <dxlc:LayoutGroup Header="ข้อมูล Study" Orientation="Vertical">
                <dxlc:LayoutGroup Orientation="Horizontal">
                    <dxlc:LayoutItem Label="Study Instance UID" Width="200">
                        <TextBlock Text="{Binding SelectedStudy.StudyInstanceUID}" 
                                   FontWeight="Bold" Foreground="Blue"/>
                    </dxlc:LayoutItem>
                    <dxlc:LayoutItem Label="Patient ID" Width="150">
                        <TextBlock Text="{Binding SelectedStudy.PatientID}"/>
                    </dxlc:LayoutItem>
                    <dxlc:LayoutItem Label="Patient Name" Width="200">
                        <TextBlock Text="{Binding SelectedStudy.PatientName}"/>
                    </dxlc:LayoutItem>
                </dxlc:LayoutGroup>
                
                <dxlc:LayoutGroup Orientation="Horizontal">
                    <dxlc:LayoutItem Label="Study Date" Width="150">
                        <TextBlock Text="{Binding SelectedStudy.StudyDate, StringFormat=dd/MM/yyyy}"/>
                    </dxlc:LayoutItem>
                    <dxlc:LayoutItem Label="Study Time" Width="150">
                        <TextBlock Text="{Binding SelectedStudy.StudyTime}"/>
                    </dxlc:LayoutItem>
                    <dxlc:LayoutItem Label="Accession Number" Width="150">
                        <TextBlock Text="{Binding SelectedStudy.AccessionNumber}"/>
                    </dxlc:LayoutItem>
                </dxlc:LayoutGroup>
            </dxlc:LayoutGroup>

            <!-- Editable Fields Section -->
            <dxlc:LayoutGroup Header="แก้ไขรายละเอียด" Orientation="Vertical">
                <dxlc:LayoutItem Label="Body Parts in Study" Required="True">
                    <dxe:TextEdit Text="{Binding SelectedStudy.BodyPartsInStudy, UpdateSourceTrigger=PropertyChanged}"
                                  MaxLength="512"
                                  Watermark="กรุณากรอกส่วนของร่างกายที่ตรวจ"/>
                </dxlc:LayoutItem>
                
                <dxlc:LayoutItem Label="Study Description" Required="True">
                    <dxe:TextEdit Text="{Binding SelectedStudy.StudyDescription, UpdateSourceTrigger=PropertyChanged}"
                                  MaxLength="256"
                                  Watermark="กรุณากรอกคำอธิบายการตรวจ"/>
                </dxlc:LayoutItem>
                
                <dxlc:LayoutItem Label="Modalities in Study">
                    <dxe:TextEdit Text="{Binding SelectedStudy.ModalitiesInStudy, UpdateSourceTrigger=PropertyChanged}"
                                  MaxLength="256"
                                  Watermark="กรุณากรอกประเภทการตรวจ"/>
                </dxlc:LayoutItem>
                
                <dxlc:LayoutItem Label="Patient Comments">
                    <dxe:TextEdit Text="{Binding SelectedStudy.PatientComments, UpdateSourceTrigger=PropertyChanged}"
                                  MaxLength="4000"
                                  AcceptsReturn="True"
                                  Height="80"
                                  Watermark="หมายเหตุเพิ่มเติม"/>
                </dxlc:LayoutItem>
            </dxlc:LayoutGroup>

            <!-- Change Summary Section -->
            <dxlc:LayoutGroup Header="สรุปการเปลี่ยนแปลง" Orientation="Vertical" 
                              Visibility="{Binding HasChanges, Converter={StaticResource BooleanToVisibilityConverter}}">
                <dxlc:LayoutItem>
                    <dxe:TextEdit Text="{Binding ChangeSummary}"
                                  ReadOnly="True"
                                  AcceptsReturn="True"
                                  Height="100"
                                  Background="LightYellow"
                                  Foreground="DarkBlue"/>
                </dxlc:LayoutItem>
            </dxlc:LayoutGroup>

            <!-- Action Buttons -->
            <dxlc:LayoutGroup Orientation="Horizontal" HorizontalAlignment="Right">
                <dx:SimpleButton Command="{Binding LoadAuditHistoryCommand}"
                                 Content="ดูประวัติการเปลี่ยนแปลง"
                                 Width="150" Height="30"
                                 Margin="5"/>
                <dx:SimpleButton Command="{Binding CancelChangesCommand}"
                                 Content="ยกเลิก"
                                 Width="80" Height="30"
                                 Margin="5"
                                 IsEnabled="{Binding HasChanges}"/>
                <dx:SimpleButton Command="{Binding SaveChangesCommand}"
                                 Content="บันทึกการเปลี่ยนแปลง"
                                 Width="150" Height="30"
                                 Margin="5"
                                 IsEnabled="{Binding HasChanges}"
                                 Background="LightGreen"/>
            </dxlc:LayoutGroup>

            <!-- Audit History Section -->
            <dxlc:LayoutGroup Header="ประวัติการเปลี่ยนแปลง" Orientation="Vertical">
                <dxg:GridControl ItemsSource="{Binding AuditHistory}" Height="200">
                    <dxg:GridControl.Columns>
                        <dxg:GridColumn Header="วันที่/เวลา" FieldName="ModifiedDttm" 
                                        DisplayFormat="dd/MM/yyyy HH:mm" Width="120"/>
                        <dxg:GridColumn Header="ผู้แก้ไข" FieldName="ModifiedByName" Width="150"/>
                        <dxg:GridColumn Header="ฟิลด์" FieldName="FieldName" Width="150"/>
                        <dxg:GridColumn Header="ค่าเดิม" FieldName="OldValue" Width="200"/>
                        <dxg:GridColumn Header="ค่าใหม่" FieldName="NewValue" Width="200"/>
                        <dxg:GridColumn Header="การดำเนินการ" FieldName="ActionType" Width="100"/>
                    </dxg:GridControl.Columns>
                    <dxg:GridControl.View>
                        <dxg:TableView ShowGroupPanel="False" 
                                       AllowPerPixelScrolling="True"/>
                    </dxg:GridControl.View>
                </dxg:GridControl>
            </dxlc:LayoutGroup>

        </dxlc:LayoutControl>
    </Grid>
</UserControl>
```

#### **4.2 เพิ่มปุ่ม Edit ใน PACSWorkList.xaml**
```xml
<!-- เพิ่มใน PACSWorkList.xaml ในส่วน Action Buttons -->
<dxlc:LayoutGroup Orientation="Horizontal" HorizontalAlignment="Right">
    <dx:SimpleButton Command="{Binding EditStudyDetailsCommand}"
                     Content="แก้ไขรายละเอียด"
                     Width="120" Height="25"
                     Margin="5"
                     Background="LightBlue"/>
    <dx:SimpleButton Command="{Binding ViewerOnCDCommand}" 
                     Content="Viewer On CD" Width="100" 
                     Height="25" HorizontalAlignment="Right" />
    <dx:SimpleButton Command="{Binding ClearBufferCommand}" 
                     Content="Clear Buffer" Width="100"
                     Height="25" HorizontalAlignment="Right" />
</dxlc:LayoutGroup>
```

### **Phase 5: Integration**

#### **5.1 เพิ่ม Command ใน PACSWorkListViewModel**
```csharp
// เพิ่มใน PACSWorkListViewModel.cs
private RelayCommand _EditStudyDetailsCommand;
public RelayCommand EditStudyDetailsCommand
{
    get
    {
        return _EditStudyDetailsCommand
            ?? (_EditStudyDetailsCommand = new RelayCommand(EditStudyDetails));
    }
}

private void EditStudyDetails()
{
    if (SelectStudies == null)
    {
        ShowMessage("กรุณาเลือก Study ที่ต้องการแก้ไข", MessageType.Warning);
        return;
    }

    try
    {
        // เปิดหน้าต่างแก้ไขรายละเอียด
        var editViewModel = ServiceLocator.Current.GetInstance<EditStudyDetailsViewModel>();
        editViewModel.Initialize(SelectStudies);
        
        // เปิดหน้าต่างแก้ไข
        var editWindow = new EditStudyDetailsWindow();
        editWindow.DataContext = editViewModel;
        
        var result = editWindow.ShowDialog();
        
        if (result == true)
        {
            // รีเฟรชข้อมูลหลังจากแก้ไข
            Search();
        }
    }
    catch (Exception ex)
    {
        ShowMessage($"เกิดข้อผิดพลาดในการเปิดหน้าต่างแก้ไข: {ex.Message}", MessageType.Error);
    }
}
```

#### **5.2 เพิ่มใน ViewModelLocator**
```csharp
// เพิ่มใน ViewModelLocator.cs
public EditStudyDetailsViewModel EditStudyDetailsViewModel
{
    get
    {
        if (!SimpleIoc.Default.ContainsCreated<EditStudyDetailsViewModel>())
            SimpleIoc.Default.Register<EditStudyDetailsViewModel>();

        return ServiceLocator.Current.GetInstance<EditStudyDetailsViewModel>();
    }
}
```

---

## 📊 แผนการดำเนินงาน (Implementation Timeline)

### **Week 1-2: Database & Data Service**
- [ ] สร้างตาราง PACSStudyAuditLog
- [ ] สร้าง Stored Procedures
- [ ] สร้าง PACSStudyAuditLogModel
- [ ] เพิ่มเมธอดใน PACSService
- [ ] ทดสอบ Data Service Layer

### **Week 3-4: ViewModel & Business Logic**
- [ ] สร้าง EditStudyDetailsViewModel
- [ ] เพิ่ม Commands และ Methods
- [ ] ทดสอบ Business Logic
- [ ] Integration Testing

### **Week 5-6: User Interface**
- [ ] สร้าง EditStudyDetails.xaml
- [ ] เพิ่มปุ่ม Edit ใน PACSWorkList
- [ ] ปรับปรุง UI/UX
- [ ] ทดสอบ User Interface

### **Week 7-8: Integration & Testing**
- [ ] Integration Testing
- [ ] User Acceptance Testing
- [ ] Performance Testing
- [ ] Security Testing
- [ ] Documentation

---

## 🔒 ระบบความปลอดภัย

### **1. การควบคุมสิทธิ์**
```csharp
// ตรวจสอบสิทธิ์ก่อนแก้ไข
private bool CanEditStudyDetails()
{
    // ตรวจสอบสิทธิ์ตาม Role
    var userRoles = AppUtil.Current.UserRoles;
    
    // เฉพาะ Admin Radiologist, Radiologist, RDU Staff เท่านั้น
    return userRoles.Any(r => r.Contains("AdminRadiologist") || 
                              r.Contains("Radiologist") || 
                              r.Contains("RDUStaff"));
}
```

### **2. การตรวจสอบข้อมูล**
```csharp
// Validation Rules
private bool ValidateStudyData(StudiesModel study)
{
    if (string.IsNullOrWhiteSpace(study.BodyPartsInStudy))
    {
        ShowMessage("กรุณากรอกส่วนของร่างกายที่ตรวจ", MessageType.Warning);
        return false;
    }
    
    if (string.IsNullOrWhiteSpace(study.StudyDescription))
    {
        ShowMessage("กรุณากรอกคำอธิบายการตรวจ", MessageType.Warning);
        return false;
    }
    
    return true;
}
```

---

## 📈 การติดตามและรายงาน

### **1. Audit Log Report**
```csharp
// สร้างรายงาน Audit Log
public List<PACSStudyAuditLogModel> GetAuditReport(DateTime fromDate, DateTime toDate, int? userId = null)
{
    var query = context.PACSStudyAuditLogs
        .Where(x => x.ModifiedDttm >= fromDate && x.ModifiedDttm <= toDate && x.StatusFlag == "A");
    
    if (userId.HasValue)
        query = query.Where(x => x.ModifiedBy == userId.Value);
    
    return query.OrderByDescending(x => x.ModifiedDttm).ToList();
}
```

### **2. Dashboard Metrics**
- จำนวนการแก้ไขต่อวัน
- ผู้ใช้ที่แก้ไขมากที่สุด
- ฟิลด์ที่แก้ไขบ่อยที่สุด
- จำนวนข้อผิดพลาดที่แก้ไข

---

## 💰 การประมาณการต้นทุน

### **Development Cost**
- **Database Development**: 2 สัปดาห์ × ฿50,000 = ฿100,000
- **Backend Development**: 2 สัปดาห์ × ฿60,000 = ฿120,000
- **Frontend Development**: 2 สัปดาห์ × ฿55,000 = ฿110,000
- **Testing & Integration**: 2 สัปดาห์ × ฿45,000 = ฿90,000

**รวมต้นทุนการพัฒนา**: ฿420,000

### **Maintenance Cost (ต่อปี)**
- **System Maintenance**: ฿50,000
- **Bug Fixes**: ฿30,000
- **Feature Enhancements**: ฿40,000

**รวมต้นทุนบำรุงรักษา**: ฿120,000/ปี

---

## 🎯 ผลลัพธ์ที่คาดหวัง

### **ประสิทธิภาพ**
- **ลดเวลาแก้ไขข้อมูล**: จาก 30 นาที → 5 นาที
- **เพิ่มความแม่นยำ**: ลดข้อผิดพลาด 90%
- **เพิ่มประสิทธิภาพการทำงาน**: 20%

### **ความพึงพอใจ**
- **เจ้าหน้าที่**: ใช้งานง่าย สะดวก
- **ผู้บริหาร**: ระบบติดตามครบถ้วน
- **ผู้ป่วย**: ได้รับบริการเร็วขึ้น

---

## 📝 สรุป

ฟีเจอร์การแก้ไขรายละเอียด X-ray จากหน้างานจะเป็นประโยชน์อย่างมากต่อระบบ MediTech โดยจะช่วย:

1. **เพิ่มประสิทธิภาพการทำงาน** ของเจ้าหน้าที่ X-ray
2. **ลดข้อผิดพลาด** ในการบันทึกข้อมูล
3. **เพิ่มความยืดหยุ่น** ในการแก้ไขข้อมูล
4. **ระบบ Audit Log** ที่ครบถ้วนสำหรับการติดตาม

การพัฒนาจะใช้เวลา **8 สัปดาห์** ด้วยต้นทุน **฿420,000** และจะให้ผลตอบแทนที่คุ้มค่าในระยะยาว

---

<div align="center">

**📞 ติดต่อสอบถาม**  
**Email**: support@meditech.com  
**Tel**: 02-XXX-XXXX

---

*เอกสารนี้จัดทำขึ้นเพื่อการวางแผนการพัฒนาฟีเจอร์ใหม่ของระบบ MediTech*  
*© 2024 MediTech Hospital Information System*

</div>
