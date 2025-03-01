from typing import Optional, List
from datetime import datetime, time
from pydantic import BaseModel

from backend.models.appointment import AppointmentStatus


# คุณสมบัติร่วมกันของ Appointment
class AppointmentBase(BaseModel):
    patient_id: Optional[int] = None
    doctor_id: Optional[int] = None
    appointment_datetime: Optional[datetime] = None
    end_datetime: Optional[datetime] = None
    status: Optional[str] = None
    reason: Optional[str] = None
    notes: Optional[str] = None


# คุณสมบัติที่ใช้ในการสร้าง Appointment ใหม่
class AppointmentCreate(AppointmentBase):
    patient_id: int
    doctor_id: int
    appointment_datetime: datetime
    end_datetime: datetime
    reason: str


# คุณสมบัติที่ใช้ในการอัปเดต Appointment
class AppointmentUpdate(AppointmentBase):
    pass


# คุณสมบัติที่ใช้ในการแสดงข้อมูล Appointment (จาก DB)
class AppointmentInDBBase(AppointmentBase):
    id: Optional[int] = None
    created_at: Optional[datetime] = None
    updated_at: Optional[datetime] = None

    class Config:
        orm_mode = True


# คุณสมบัติที่ใช้ในการส่งข้อมูล Appointment กลับไปยังผู้ใช้
class Appointment(AppointmentInDBBase):
    pass


# คุณสมบัติที่ใช้ในการเก็บข้อมูล Appointment ใน DB
class AppointmentInDB(AppointmentInDBBase):
    pass


# คุณสมบัติสำหรับการตรวจสอบช่วงเวลาว่าง
class TimeSlotCheck(BaseModel):
    doctor_id: int
    appointment_date: datetime
    start_time: time
    end_time: time 