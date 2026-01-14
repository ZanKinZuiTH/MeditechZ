/*
  Purpose:
    - Add pregnancy flags to dbo.PatientVitalSign (idempotent)
    - Enforce mutually exclusive selection (cannot be both TRUE)

  Safe to run multiple times.
*/

SET NOCOUNT ON;

PRINT '== Ensure columns exist ==';
IF COL_LENGTH('dbo.PatientVitalSign', 'IsPregnant') IS NULL
BEGIN
    ALTER TABLE dbo.PatientVitalSign ADD IsPregnant BIT NULL;
    PRINT 'Added column dbo.PatientVitalSign.IsPregnant';
END
ELSE
BEGIN
    PRINT 'Column dbo.PatientVitalSign.IsPregnant already exists';
END

IF COL_LENGTH('dbo.PatientVitalSign', 'IsSuspectedPregnant') IS NULL
BEGIN
    ALTER TABLE dbo.PatientVitalSign ADD IsSuspectedPregnant BIT NULL;
    PRINT 'Added column dbo.PatientVitalSign.IsSuspectedPregnant';
END
ELSE
BEGIN
    PRINT 'Column dbo.PatientVitalSign.IsSuspectedPregnant already exists';
END

PRINT '== Normalize existing data (if any duplicates exist) ==';
UPDATE dbo.PatientVitalSign
SET IsSuspectedPregnant = 0
WHERE IsPregnant = 1 AND IsSuspectedPregnant = 1;

PRINT '== Add CHECK constraint to prevent duplicates ==';
IF NOT EXISTS (
    SELECT 1
    FROM sys.check_constraints cc
    WHERE cc.name = 'CK_PatientVitalSign_PregnancyFlags_NotBothTrue'
)
BEGIN
    ALTER TABLE dbo.PatientVitalSign WITH CHECK
    ADD CONSTRAINT CK_PatientVitalSign_PregnancyFlags_NotBothTrue
    CHECK (NOT (IsPregnant = 1 AND IsSuspectedPregnant = 1));

    PRINT 'Added constraint CK_PatientVitalSign_PregnancyFlags_NotBothTrue';
END
ELSE
BEGIN
    PRINT 'Constraint CK_PatientVitalSign_PregnancyFlags_NotBothTrue already exists';
END

PRINT '== Verification ==';
SELECT
    COL_LENGTH('dbo.PatientVitalSign','IsPregnant')           AS HasIsPregnant,
    COL_LENGTH('dbo.PatientVitalSign','IsSuspectedPregnant')  AS HasIsSuspectedPregnant;

SELECT TOP (20)
    UID,
    PatientUID,
    PatientVisitUID,
    IsPregnant,
    IsSuspectedPregnant,
    RecordedDttm,
    StatusFlag
FROM dbo.PatientVitalSign
WHERE (IsPregnant = 1 OR IsSuspectedPregnant = 1)
ORDER BY RecordedDttm DESC;

