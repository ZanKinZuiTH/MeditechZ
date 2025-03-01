from typing import Optional, List, Dict, Any
from datetime import datetime
from pydantic import BaseModel


# คุณสมบัติร่วมกันของ MedicalRecord
class MedicalRecordBase(BaseModel):
    patient_id: Optional[int] = None
    doctor_id: Optional[int] = None
    appointment_id: Optional[int] = None
    record_date: Optional[datetime] = None
    chief_complaint: Optional[str] = None
    diagnosis: Optional[str] = None
    treatment: Optional[str] = None
    prescription: Optional[str] = None
    lab_results: Optional[Dict[str, Any]] = None
    vital_signs: Optional[Dict[str, Any]] = None
    notes: Optional[str] = None
    follow_up_date: Optional[datetime] = None


# คุณสมบัติที่ใช้ในการสร้าง MedicalRecord ใหม่
class MedicalRecordCreate(MedicalRecordBase):
    patient_id: int
    doctor_id: int
    chief_complaint: str
    diagnosis: str


# คุณสมบัติที่ใช้ในการอัปเดต MedicalRecord
class MedicalRecordUpdate(MedicalRecordBase):
    pass


# คุณสมบัติที่ใช้ในการแสดงข้อมูล MedicalRecord (จาก DB)
class MedicalRecordInDBBase(MedicalRecordBase):
    id: Optional[int] = None
    created_at: Optional[datetime] = None
    updated_at: Optional[datetime] = None

    class Config:
        orm_mode = True


# คุณสมบัติที่ใช้ในการส่งข้อมูล MedicalRecord กลับไปยังผู้ใช้
class MedicalRecord(MedicalRecordInDBBase):
    pass


# คุณสมบัติที่ใช้ในการเก็บข้อมูล MedicalRecord ใน DB
class MedicalRecordInDB(MedicalRecordInDBBase):
    pass 