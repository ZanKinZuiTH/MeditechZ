"""
API Router Configuration

ไฟล์นี้รวม routers ทั้งหมดของ API เวอร์ชัน 1
และกำหนดค่า prefix และ tags สำหรับแต่ละ router

Author: ทีมพัฒนา MeditechZ
Version: 1.0.0
Date: March 1, 2025
"""

from fastapi import APIRouter

from backend.api.api_v1.endpoints import (
    auth,
    users,
    patients,
    appointments,
    medical_records,
    ai_diagnosis,  # เพิ่ม import สำหรับ ai_diagnosis
    medical_documents
)

api_router = APIRouter()

# เพิ่ม routers ต่างๆ พร้อมกำหนด prefix และ tags
api_router.include_router(auth.router, prefix="/auth", tags=["Authentication"])
api_router.include_router(users.router, prefix="/users", tags=["Users"])
api_router.include_router(patients.router, prefix="/patients", tags=["Patients"])
api_router.include_router(appointments.router, prefix="/appointments", tags=["Appointments"])
api_router.include_router(medical_records.router, prefix="/medical-records", tags=["Medical Records"])
api_router.include_router(ai_diagnosis.router, prefix="/ai-diagnosis", tags=["AI Diagnosis"])  # เพิ่ม router สำหรับ ai_diagnosis 
api_router.include_router(medical_documents.router, prefix="/medical-documents", tags=["Medical Documents"]) 