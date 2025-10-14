-- Idempotent creation of PACSStudyAuditLog and SP_InsertPACSStudyAuditLog
IF NOT EXISTS (SELECT 1 FROM sys.schemas WHERE name = N'dicom')
    PRINT 'Schema dicom assumed to exist.';
GO

IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dicom].[PACSStudyAuditLog]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dicom].[PACSStudyAuditLog]
    (
        [AuditId] BIGINT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PACSStudyAuditLog PRIMARY KEY,
        [StudyInstanceUID] NVARCHAR(128) NOT NULL,
        [ModifiedDttm] DATETIME2(3) NOT NULL CONSTRAINT DF_PACSStudyAuditLog_ModifiedDttm DEFAULT (SYSUTCDATETIME()),
        [ModifiedBy] INT NOT NULL,
        [ModifiedByName] NVARCHAR(256) NULL,
        [ActionType] NVARCHAR(64) NOT NULL,
        [FieldName] NVARCHAR(128) NOT NULL,
        [OldValue] NVARCHAR(4000) NULL,
        [NewValue] NVARCHAR(4000) NULL,
        [IPAddress] NVARCHAR(64) NULL,
        [UserAgent] NVARCHAR(256) NULL,
        [OrganisationUID] INT NULL
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_PACSStudyAuditLog_Study_Modified' AND object_id = OBJECT_ID(N'[dicom].[PACSStudyAuditLog]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_PACSStudyAuditLog_Study_Modified]
    ON [dicom].[PACSStudyAuditLog] ([StudyInstanceUID], [ModifiedDttm] DESC)
    INCLUDE ([FieldName], [ModifiedBy]);
END
GO

IF EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dicom].[SP_InsertPACSStudyAuditLog]') AND type in (N'P'))
BEGIN
    DROP PROCEDURE [dicom].[SP_InsertPACSStudyAuditLog];
END
GO

CREATE PROCEDURE [dicom].[SP_InsertPACSStudyAuditLog]
    @StudyInstanceUID NVARCHAR(128),
    @ModifiedBy INT,
    @ModifiedByName NVARCHAR(256) = NULL,
    @ActionType NVARCHAR(64),
    @FieldName NVARCHAR(128),
    @OldValue NVARCHAR(4000) = NULL,
    @NewValue NVARCHAR(4000) = NULL,
    @IPAddress NVARCHAR(64) = NULL,
    @UserAgent NVARCHAR(256) = NULL,
    @OrganisationUID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO [dicom].[PACSStudyAuditLog]
    (
        StudyInstanceUID, ModifiedBy, ModifiedByName, ActionType, FieldName, OldValue, NewValue, IPAddress, UserAgent, OrganisationUID
    )
    VALUES
    (
        @StudyInstanceUID, @ModifiedBy, @ModifiedByName, @ActionType, @FieldName, @OldValue, @NewValue, @IPAddress, @UserAgent, @OrganisationUID
    );
END
GO


