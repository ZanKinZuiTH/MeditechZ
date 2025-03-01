from typing import Optional, List, Dict, Any, Union
from datetime import datetime
from pydantic import BaseModel, Field


# คุณสมบัติร่วมกันของเอกสารทางการแพทย์
class MedicalDocumentBase(BaseModel):
    patient_id: int
    doctor_id: int
    visit_id: Optional[int] = None
    document_date: datetime = Field(default_factory=datetime.now)
    document_type: str  # "medical_certificate", "health_checkup_book", "other"
    document_status: str = "draft"  # "draft", "completed", "signed", "cancelled"
    notes: Optional[str] = None


# คุณสมบัติร่วมกันของใบรับรองแพทย์
class MedicalCertificateBase(BaseModel):
    patient_id: int
    doctor_id: int
    visit_id: Optional[int] = None
    certificate_date: datetime = Field(default_factory=datetime.now)
    certificate_type: str = "general"  # "general", "confined_space", "radiology", "other"
    diagnosis: Optional[str] = None
    treatment: Optional[str] = None
    rest_period_days: Optional[int] = None
    rest_period_start: Optional[datetime] = None
    rest_period_end: Optional[datetime] = None
    doctor_license_no: Optional[str] = None
    doctor_license_issue_date: Optional[datetime] = None
    doctor2_id: Optional[int] = None
    doctor2_license_no: Optional[str] = None
    doctor2_license_issue_date: Optional[datetime] = None
    vital_signs: Optional[Dict[str, Any]] = None
    comments: Optional[str] = None


# คุณสมบัติร่วมกันของสมุดตรวจสุขภาพ
class HealthCheckupBookBase(BaseModel):
    patient_id: int
    doctor_id: int
    visit_id: Optional[int] = None
    checkup_date: datetime = Field(default_factory=datetime.now)
    checkup_type: str = "general"  # "general", "annual", "pre_employment", "other"
    vital_signs: Optional[Dict[str, Any]] = None
    physical_exam: Optional[Dict[str, Any]] = None
    lab_results: Optional[Dict[str, Any]] = None
    radiology_results: Optional[Dict[str, Any]] = None
    ekg_results: Optional[Dict[str, Any]] = None
    spirometry_results: Optional[Dict[str, Any]] = None
    audiometry_results: Optional[Dict[str, Any]] = None
    vision_test_results: Optional[Dict[str, Any]] = None
    conclusion: Optional[str] = None
    recommendations: Optional[str] = None
    doctor_license_no: Optional[str] = None
    doctor_signature: Optional[str] = None


# คุณสมบัติที่ใช้ในการสร้างเอกสารใหม่
class MedicalDocumentCreate(MedicalDocumentBase):
    pass


class MedicalCertificateCreate(MedicalCertificateBase):
    pass


class HealthCheckupBookCreate(HealthCheckupBookBase):
    pass


# คุณสมบัติที่ใช้ในการอัปเดตเอกสาร
class MedicalDocumentUpdate(BaseModel):
    patient_id: Optional[int] = None
    doctor_id: Optional[int] = None
    visit_id: Optional[int] = None
    document_date: Optional[datetime] = None
    document_type: Optional[str] = None
    document_status: Optional[str] = None
    notes: Optional[str] = None


class MedicalCertificateUpdate(BaseModel):
    patient_id: Optional[int] = None
    doctor_id: Optional[int] = None
    visit_id: Optional[int] = None
    certificate_date: Optional[datetime] = None
    certificate_type: Optional[str] = None
    diagnosis: Optional[str] = None
    treatment: Optional[str] = None
    rest_period_days: Optional[int] = None
    rest_period_start: Optional[datetime] = None
    rest_period_end: Optional[datetime] = None
    doctor_license_no: Optional[str] = None
    doctor_license_issue_date: Optional[datetime] = None
    doctor2_id: Optional[int] = None
    doctor2_license_no: Optional[str] = None
    doctor2_license_issue_date: Optional[datetime] = None
    vital_signs: Optional[Dict[str, Any]] = None
    comments: Optional[str] = None


class HealthCheckupBookUpdate(BaseModel):
    patient_id: Optional[int] = None
    doctor_id: Optional[int] = None
    visit_id: Optional[int] = None
    checkup_date: Optional[datetime] = None
    checkup_type: Optional[str] = None
    vital_signs: Optional[Dict[str, Any]] = None
    physical_exam: Optional[Dict[str, Any]] = None
    lab_results: Optional[Dict[str, Any]] = None
    radiology_results: Optional[Dict[str, Any]] = None
    ekg_results: Optional[Dict[str, Any]] = None
    spirometry_results: Optional[Dict[str, Any]] = None
    audiometry_results: Optional[Dict[str, Any]] = None
    vision_test_results: Optional[Dict[str, Any]] = None
    conclusion: Optional[str] = None
    recommendations: Optional[str] = None
    doctor_license_no: Optional[str] = None
    doctor_signature: Optional[str] = None


# คุณสมบัติที่ใช้ในการแสดงข้อมูลเอกสาร (จาก DB)
class MedicalDocumentInDBBase(MedicalDocumentBase):
    id: int
    created_at: datetime
    updated_at: Optional[datetime] = None
    created_by_id: int
    updated_by_id: Optional[int] = None

    class Config:
        orm_mode = True


class MedicalCertificateInDBBase(MedicalCertificateBase):
    id: int
    created_at: datetime
    updated_at: Optional[datetime] = None
    created_by_id: int
    updated_by_id: Optional[int] = None

    class Config:
        orm_mode = True


class HealthCheckupBookInDBBase(HealthCheckupBookBase):
    id: int
    created_at: datetime
    updated_at: Optional[datetime] = None
    created_by_id: int
    updated_by_id: Optional[int] = None

    class Config:
        orm_mode = True


# คุณสมบัติที่ใช้ในการส่งข้อมูลเอกสารกลับไปยังผู้ใช้
class MedicalDocument(MedicalDocumentInDBBase):
    pass


class MedicalCertificate(MedicalCertificateInDBBase):
    pass


class HealthCheckupBook(HealthCheckupBookInDBBase):
    pass


# คุณสมบัติที่ใช้ในการเก็บข้อมูลเอกสารใน DB
class MedicalDocumentInDB(MedicalDocumentInDBBase):
    pass


class MedicalCertificateInDB(MedicalCertificateInDBBase):
    pass


class HealthCheckupBookInDB(HealthCheckupBookInDBBase):
    pass 