"""
MeditechZ Backend Main Application

ไฟล์นี้เป็นจุดเริ่มต้นของ Backend API สำหรับระบบ MeditechZ
ทำหน้าที่กำหนดค่าและเริ่มต้น FastAPI application รวมถึงการตั้งค่า middleware, router และ event handlers

Author: ทีมพัฒนา MeditechZ
Version: 1.0.0
Date: March 1, 2025
"""

from fastapi import FastAPI, Depends
from fastapi.middleware.cors import CORSMiddleware
from fastapi.responses import JSONResponse
from fastapi.staticfiles import StaticFiles
import uvicorn
import logging
from typing import List

from backend.core.config import settings
from backend.api.api_v1.api import api_router
from backend.db.session import engine, SessionLocal
from backend.db.base import Base

# สร้าง logger สำหรับบันทึกข้อมูลการทำงานของระบบ
logger = logging.getLogger("meditech")
logger.setLevel(logging.INFO)

# สร้าง FastAPI application พร้อมกำหนดค่าพื้นฐาน
# - title: ชื่อของ API
# - description: คำอธิบายของ API
# - version: เวอร์ชันของ API
# - openapi_url: URL สำหรับ OpenAPI schema
# - docs_url: URL สำหรับ Swagger UI
# - redoc_url: URL สำหรับ ReDoc UI
app = FastAPI(
    title=settings.PROJECT_NAME,
    description=settings.PROJECT_DESCRIPTION,
    version=settings.PROJECT_VERSION,
    openapi_url=f"{settings.API_V1_STR}/openapi.json",
    docs_url=f"{settings.API_V1_STR}/docs",
    redoc_url=f"{settings.API_V1_STR}/redoc",
)

# ตั้งค่า CORS (Cross-Origin Resource Sharing) เพื่อให้ frontend สามารถเรียกใช้ API ได้
# - allow_origins: domains ที่อนุญาตให้เรียกใช้ API
# - allow_credentials: อนุญาตให้ส่ง credentials (cookies, authorization headers)
# - allow_methods: HTTP methods ที่อนุญาต
# - allow_headers: HTTP headers ที่อนุญาต
app.add_middleware(
    CORSMiddleware,
    allow_origins=settings.BACKEND_CORS_ORIGINS,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# เพิ่ม router สำหรับ API endpoints
# prefix คือ path prefix สำหรับทุก endpoints ใน router
app.include_router(api_router, prefix=settings.API_V1_STR)

# สร้าง endpoint สำหรับตรวจสอบสถานะของ API (health check)
# ใช้สำหรับการตรวจสอบว่า API ยังทำงานอยู่หรือไม่
@app.get("/health", tags=["Health"])
async def health_check():
    """
    ตรวจสอบสถานะของ API
    
    Returns:
        dict: สถานะและเวอร์ชันของ API
    """
    return {"status": "ok", "version": settings.PROJECT_VERSION}

# Event handler ที่จะทำงานเมื่อ application เริ่มต้น
# ใช้สำหรับการเตรียมทรัพยากรที่จำเป็น เช่น การสร้าง database tables
@app.on_event("startup")
async def startup_event():
    """
    Event handler ที่ทำงานเมื่อ application เริ่มต้น
    
    - สร้าง database tables (ถ้ายังไม่มี)
    - เตรียมทรัพยากรอื่นๆ ที่จำเป็น
    
    Raises:
        Exception: หากมีข้อผิดพลาดในการสร้าง database tables
    """
    logger.info("Starting up MediTech API...")
    try:
        # สร้าง tables ใน database (ถ้ายังไม่มี)
        Base.metadata.create_all(bind=engine)
        logger.info("Database tables created successfully")
    except Exception as e:
        logger.error(f"Error creating database tables: {e}")
        raise e

# Event handler ที่จะทำงานเมื่อ application ปิดตัวลง
# ใช้สำหรับการทำความสะอาดทรัพยากร เช่น การปิด connections
@app.on_event("shutdown")
async def shutdown_event():
    """
    Event handler ที่ทำงานเมื่อ application ปิดตัวลง
    
    - ทำความสะอาดทรัพยากร
    - ปิด connections
    """
    logger.info("Shutting down MediTech API...")

# รัน application ถ้าเรียกโดยตรง (ไม่ได้ import)
if __name__ == "__main__":
    # รัน uvicorn server พร้อมกำหนดค่า
    # - host: IP address ที่จะรัน server
    # - port: port ที่จะรัน server
    # - reload: ให้ server restart เมื่อมีการเปลี่ยนแปลงโค้ด (สำหรับ development)
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True) 