from typing import Optional, List
from datetime import datetime, date
from pydantic import BaseModel, EmailStr


# คุณสมบัติร่วมกันของ Patient
class PatientBase(BaseModel):
    first_name: Optional[str] = None
    last_name: Optional[str] = None
    id_card_number: Optional[str] = None
    date_of_birth: Optional[date] = None
    gender: Optional[str] = None
    blood_type: Optional[str] = None
    address: Optional[str] = None
    phone_number: Optional[str] = None
    email: Optional[EmailStr] = None
    emergency_contact_name: Optional[str] = None
    emergency_contact_phone: Optional[str] = None
    allergies: Optional[str] = None
    chronic_diseases: Optional[str] = None
    is_active: Optional[bool] = True


# คุณสมบัติที่ใช้ในการสร้าง Patient ใหม่
class PatientCreate(PatientBase):
    first_name: str
    last_name: str
    id_card_number: str
    date_of_birth: date
    gender: str


# คุณสมบัติที่ใช้ในการอัปเดต Patient
class PatientUpdate(PatientBase):
    pass


# คุณสมบัติที่ใช้ในการแสดงข้อมูล Patient (จาก DB)
class PatientInDBBase(PatientBase):
    id: Optional[int] = None
    created_at: Optional[datetime] = None
    updated_at: Optional[datetime] = None

    class Config:
        orm_mode = True


# คุณสมบัติที่ใช้ในการส่งข้อมูล Patient กลับไปยังผู้ใช้
class Patient(PatientInDBBase):
    pass


# คุณสมบัติที่ใช้ในการเก็บข้อมูล Patient ใน DB
class PatientInDB(PatientInDBBase):
    pass 