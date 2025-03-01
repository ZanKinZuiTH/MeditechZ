from typing import Optional
from datetime import datetime
from pydantic import BaseModel, EmailStr


# คุณสมบัติร่วมกันของ User
class UserBase(BaseModel):
    email: Optional[EmailStr] = None
    is_active: Optional[bool] = True
    is_superuser: bool = False
    full_name: Optional[str] = None


# คุณสมบัติที่ใช้ในการสร้าง User ใหม่
class UserCreate(UserBase):
    email: EmailStr
    password: str


# คุณสมบัติที่ใช้ในการอัปเดต User
class UserUpdate(UserBase):
    password: Optional[str] = None


# คุณสมบัติที่ใช้ในการแสดงข้อมูล User (จาก DB)
class UserInDBBase(UserBase):
    id: Optional[int] = None
    created_at: Optional[datetime] = None
    updated_at: Optional[datetime] = None

    class Config:
        orm_mode = True


# คุณสมบัติที่ใช้ในการส่งข้อมูล User กลับไปยังผู้ใช้
class User(UserInDBBase):
    pass


# คุณสมบัติที่ใช้ในการเก็บข้อมูล User ใน DB (รวมถึง hashed_password)
class UserInDB(UserInDBBase):
    hashed_password: str 