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

# สร้าง logger
logger = logging.getLogger("meditech")
logger.setLevel(logging.INFO)

# สร้าง application
app = FastAPI(
    title=settings.PROJECT_NAME,
    description=settings.PROJECT_DESCRIPTION,
    version=settings.PROJECT_VERSION,
    openapi_url=f"{settings.API_V1_STR}/openapi.json",
    docs_url=f"{settings.API_V1_STR}/docs",
    redoc_url=f"{settings.API_V1_STR}/redoc",
)

# ตั้งค่า CORS
app.add_middleware(
    CORSMiddleware,
    allow_origins=settings.BACKEND_CORS_ORIGINS,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# เพิ่ม router
app.include_router(api_router, prefix=settings.API_V1_STR)

# สร้าง endpoint สำหรับ health check
@app.get("/health", tags=["Health"])
async def health_check():
    return {"status": "ok", "version": settings.PROJECT_VERSION}

# สร้าง tables ใน database (ถ้ายังไม่มี)
@app.on_event("startup")
async def startup_event():
    logger.info("Starting up MediTech API...")
    try:
        # สร้าง tables ใน database (ถ้ายังไม่มี)
        Base.metadata.create_all(bind=engine)
        logger.info("Database tables created successfully")
    except Exception as e:
        logger.error(f"Error creating database tables: {e}")
        raise e

@app.on_event("shutdown")
async def shutdown_event():
    logger.info("Shutting down MediTech API...")

# รัน application ถ้าเรียกโดยตรง
if __name__ == "__main__":
    uvicorn.run("main:app", host="0.0.0.0", port=8000, reload=True) 