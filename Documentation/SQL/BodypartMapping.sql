IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'[dicom].[BodypartMapping]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dicom].[BodypartMapping]
    (
        MappingId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BodypartMapping PRIMARY KEY,
        InputValue NVARCHAR(512) NOT NULL,
        StandardValue NVARCHAR(128) NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_BodypartMapping_IsActive DEFAULT(1),
        CreatedDttm DATETIME2(3) NOT NULL CONSTRAINT DF_BodypartMapping_CreatedDttm DEFAULT (SYSUTCDATETIME())
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_BodypartMapping_Input' AND object_id = OBJECT_ID(N'[dicom].[BodypartMapping]'))
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX IX_BodypartMapping_Input ON [dicom].[BodypartMapping](InputValue);
END
GO

-- Seed ตัวอย่าง
IF NOT EXISTS (SELECT 1 FROM dicom.dbo.BodypartMapping WHERE InputValue = N'CHEST')
    INSERT INTO dicom.dbo.BodypartMapping (InputValue, StandardValue) VALUES (N'CHEST', N'CHEST');
IF NOT EXISTS (SELECT 1 FROM dicom.dbo.BodypartMapping WHERE InputValue = N'Chest')
    INSERT INTO dicom.dbo.BodypartMapping (InputValue, StandardValue) VALUES (N'Chest', N'CHEST');
IF NOT EXISTS (SELECT 1 FROM dicom.dbo.BodypartMapping WHERE InputValue = N'CXR')
    INSERT INTO dicom.dbo.BodypartMapping (InputValue, StandardValue) VALUES (N'CXR', N'CHEST');
GO


