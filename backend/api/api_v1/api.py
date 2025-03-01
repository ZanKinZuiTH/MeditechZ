from fastapi import APIRouter

from backend.api.api_v1.endpoints import users, auth, patients, appointments, medical_records

api_router = APIRouter()

# เพิ่ม router สำหรับแต่ละ endpoint
api_router.include_router(auth.router, prefix="/auth", tags=["Authentication"])
api_router.include_router(users.router, prefix="/users", tags=["Users"])
api_router.include_router(patients.router, prefix="/patients", tags=["Patients"])
api_router.include_router(appointments.router, prefix="/appointments", tags=["Appointments"])
api_router.include_router(medical_records.router, prefix="/medical-records", tags=["Medical Records"]) 