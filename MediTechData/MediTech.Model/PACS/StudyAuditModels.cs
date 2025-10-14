using System;
using System.Collections.Generic;

namespace MediTech.Model
{
    public class StudyDetailChange
    {
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }

    public class StudyAuditLogEntry
    {
        public long AuditId { get; set; }
        public string StudyInstanceUID { get; set; }
        public DateTime ModifiedDttm { get; set; }
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public string ActionType { get; set; }
        public string FieldName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public int? OrganisationUID { get; set; }
    }

    public class UpdateStudyDetailsRequest
    {
        public string StudyInstanceUID { get; set; }

        // Editable fields (extensible)
        public string BodyPartsInStudy { get; set; }
        public string StudyDescription { get; set; }
        public string ModalitiesInStudy { get; set; }
        public string PatientComments { get; set; }

        // Client-provided baseline to compute server-side validation if needed
        public List<StudyDetailChange> Changes { get; set; }

        // Audit context
        public int ModifiedBy { get; set; }
        public string ModifiedByName { get; set; }
        public int? RoleUID { get; set; }
        public string RoleName { get; set; }
        public string ActionType { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public int? OrganisationUID { get; set; }
    }
}


