from fastapi import APIRouter, Depends, HTTPException, UploadFile, File, Form, status
from fastapi.responses import JSONResponse
from typing import Optional, List, Dict, Any
from sqlalchemy.orm import Session
import os
import tempfile
import json
import logging
from datetime import datetime

# ในการใช้งานจริงควรมีการ import โมดูลสำหรับการประมวลผลภาพและ AI
# import cv2
# import numpy as np
# import pytesseract
# from PIL import Image

from db.session import get_db
from core.security import get_current_active_user
from models.user import User
from schemas.document_import import (
    DocumentImportResponse,
    DocumentSaveRequest,
    DocumentSaveResponse,
    ExtractedField,
    ExtractedData
)

router = APIRouter()
logger = logging.getLogger(__name__)

# ประเภทของเอกสารที่รองรับ
DOCUMENT_TYPES = {
    "general_medical_record": "เวชระเบียนทั่วไป",
    "lab_result": "ผลตรวจทางห้องปฏิบัติการ",
    "prescription": "ใบสั่งยา",
    "medical_certificate": "ใบรับรองแพทย์",
    "health_checkup": "ผลตรวจสุขภาพ",
    "custom": "กำหนดเอง"
}

# เทมเพลตเอกสารที่มีอยู่ในระบบ
DOCUMENT_TEMPLATES = [
    {
        "id": "template-001",
        "name": "แบบฟอร์มตรวจสุขภาพมาตรฐาน",
        "type": "health_checkup"
    },
    {
        "id": "template-002",
        "name": "ใบรับรองแพทย์ทั่วไป",
        "type": "medical_certificate"
    },
    {
        "id": "template-003",
        "name": "ผลตรวจทางห้องปฏิบัติการ",
        "type": "lab_result"
    },
    {
        "id": "template-004",
        "name": "ใบสั่งยา",
        "type": "prescription"
    }
]

@router.get("/templates", response_model=List[Dict[str, Any]])
async def get_document_templates(
    current_user: User = Depends(get_current_active_user)
):
    """
    ดึงรายการเทมเพลตเอกสารที่มีอยู่ในระบบ
    """
    return DOCUMENT_TEMPLATES

@router.post("/process", response_model=DocumentImportResponse)
async def process_document(
    file: UploadFile = File(...),
    template_id: Optional[str] = Form(None),
    current_user: User = Depends(get_current_active_user),
    db: Session = Depends(get_db)
):
    """
    ประมวลผลเอกสารด้วย AI และสกัดข้อมูล
    """
    # ตรวจสอบประเภทไฟล์
    if not file.content_type.startswith("image/"):
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="รองรับเฉพาะไฟล์ภาพเท่านั้น (JPEG, PNG, etc.)"
        )
    
    try:
        # บันทึกไฟล์ชั่วคราว
        with tempfile.NamedTemporaryFile(delete=False, suffix=os.path.splitext(file.filename)[1]) as temp_file:
            temp_file_path = temp_file.name
            content = await file.read()
            temp_file.write(content)
        
        # ในการใช้งานจริง ควรมีการประมวลผลภาพและใช้ AI ในการสกัดข้อมูล
        # image = cv2.imread(temp_file_path)
        # ประมวลผลภาพและสกัดข้อมูล
        # ...
        
        # จำลองข้อมูลที่สกัดได้
        extracted_data = {
            "document_type": "health_checkup",
            "recognized_template": "แบบฟอร์มตรวจสุขภาพมาตรฐาน",
            "confidence": 0.89,
            "fields": {
                "patient_name": {
                    "value": "นายสมชาย ใจดี",
                    "confidence": 0.95
                },
                "patient_id": {
                    "value": "1234567890123",
                    "confidence": 0.98
                },
                "examination_date": {
                    "value": "2025-03-01",
                    "confidence": 0.92
                },
                "doctor_name": {
                    "value": "นพ.รักษา สุขภาพดี",
                    "confidence": 0.87
                }
            },
            "table_data": [
                {"test": "ความดันโลหิต", "result": "120/80 mmHg", "normal_range": "90-120/60-80 mmHg", "status": "ปกติ"},
                {"test": "น้ำตาลในเลือด", "result": "95 mg/dL", "normal_range": "70-100 mg/dL", "status": "ปกติ"},
                {"test": "คอเลสเตอรอล", "result": "210 mg/dL", "normal_range": "<200 mg/dL", "status": "สูงกว่าปกติ"}
            ],
            "raw_text": "ผลการตรวจสุขภาพ\nชื่อ-นามสกุล: นายสมชาย ใจดี\nเลขประจำตัวประชาชน: 1234567890123\nวันที่ตรวจ: 2025-03-01\nแพทย์ผู้ตรวจ: นพ.รักษา สุขภาพดี\n\nผลการตรวจ:\n1. ความดันโลหิต: 120/80 mmHg (ปกติ)\n2. น้ำตาลในเลือด: 95 mg/dL (ปกติ)\n3. คอเลสเตอรอล: 210 mg/dL (สูงกว่าปกติ)"
        }
        
        # บันทึกประวัติการนำเข้าเอกสาร
        # ในการใช้งานจริง ควรมีการบันทึกข้อมูลลงในฐานข้อมูล
        # document_import_history = DocumentImportHistory(
        #     user_id=current_user.id,
        #     filename=file.filename,
        #     document_type=extracted_data["document_type"],
        #     processed_at=datetime.now(),
        #     status="processed"
        # )
        # db.add(document_import_history)
        # db.commit()
        
        # ลบไฟล์ชั่วคราว
        os.unlink(temp_file_path)
        
        return {
            "success": True,
            "message": "ประมวลผลเอกสารสำเร็จ",
            "data": extracted_data
        }
    
    except Exception as e:
        logger.error(f"เกิดข้อผิดพลาดในการประมวลผลเอกสาร: {str(e)}")
        # ลบไฟล์ชั่วคราวหากยังมีอยู่
        if 'temp_file_path' in locals() and os.path.exists(temp_file_path):
            os.unlink(temp_file_path)
        
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"เกิดข้อผิดพลาดในการประมวลผลเอกสาร: {str(e)}"
        )

@router.post("/save", response_model=DocumentSaveResponse)
async def save_document(
    request: DocumentSaveRequest,
    current_user: User = Depends(get_current_active_user),
    db: Session = Depends(get_db)
):
    """
    บันทึกข้อมูลที่สกัดได้จากเอกสารเข้าสู่ระบบ
    """
    try:
        # ตรวจสอบประเภทเอกสาร
        if request.document_type not in DOCUMENT_TYPES:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail=f"ประเภทเอกสารไม่ถูกต้อง: {request.document_type}"
            )
        
        # ในการใช้งานจริง ควรมีการบันทึกข้อมูลลงในฐานข้อมูลตามประเภทเอกสาร
        # if request.document_type == "health_checkup":
        #     # บันทึกข้อมูลผลตรวจสุขภาพ
        #     health_checkup = HealthCheckupBook(
        #         patient_id=request.data.fields.get("patient_id", {}).get("value"),
        #         examination_date=request.data.fields.get("examination_date", {}).get("value"),
        #         doctor_id=current_user.id,
        #         # ...
        #     )
        #     db.add(health_checkup)
        #     db.commit()
        # elif request.document_type == "medical_certificate":
        #     # บันทึกข้อมูลใบรับรองแพทย์
        #     # ...
        
        # จำลองการบันทึกข้อมูลสำเร็จ
        return {
            "success": True,
            "message": f"บันทึกข้อมูล{DOCUMENT_TYPES[request.document_type]}สำเร็จ",
            "document_id": "doc-" + datetime.now().strftime("%Y%m%d%H%M%S")
        }
    
    except HTTPException:
        raise
    
    except Exception as e:
        logger.error(f"เกิดข้อผิดพลาดในการบันทึกข้อมูล: {str(e)}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"เกิดข้อผิดพลาดในการบันทึกข้อมูล: {str(e)}"
        ) 