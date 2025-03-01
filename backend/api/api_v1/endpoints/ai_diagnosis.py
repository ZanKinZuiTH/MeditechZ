"""
AI Diagnosis API Endpoints

ไฟล์นี้ประกอบด้วย API endpoints สำหรับการวินิจฉัยโรคด้วย AI
ใช้สำหรับเรียกใช้โมเดล AI เพื่อวิเคราะห์ข้อมูลทางการแพทย์

Author: ทีมพัฒนา MeditechZ
Version: 1.0.0
Date: March 1, 2025
"""

from fastapi import APIRouter, Depends, HTTPException, status
from typing import List, Dict, Any, Optional
from pydantic import BaseModel, Field
import logging

from backend.core.deps import get_current_user
from backend.models.user import User
from backend.schemas.user import UserOut

# ในสถานการณ์จริง จะต้อง import โมเดล AI
# from ai_models.inference.medical_diagnosis import MedicalDiagnosisAI

# สร้าง router
router = APIRouter()

# สร้าง logger
logger = logging.getLogger("meditech.api.ai_diagnosis")

# สร้าง schemas สำหรับ request และ response

class SymptomRequest(BaseModel):
    """
    Schema สำหรับข้อมูลอาการที่ส่งมาจาก client
    """
    symptoms: List[str] = Field(..., description="รายการอาการของผู้ป่วย")
    patient_id: Optional[int] = Field(None, description="รหัสผู้ป่วย (ถ้ามี)")
    patient_data: Optional[Dict[str, Any]] = Field(None, description="ข้อมูลผู้ป่วยเพิ่มเติม")

class DiagnosisResponse(BaseModel):
    """
    Schema สำหรับผลการวินิจฉัยที่ส่งกลับไปยัง client
    """
    disease: str = Field(..., description="ชื่อโรคที่วินิจฉัย")
    confidence: float = Field(..., description="ความมั่นใจในการวินิจฉัย (0-1)")
    description: str = Field(..., description="คำอธิบายโรค")
    recommendations: List[str] = Field(..., description="คำแนะนำเบื้องต้น")
    differential_diagnoses: List[Dict[str, Any]] = Field(..., description="การวินิจฉัยแยกโรคอื่นๆ ที่เป็นไปได้")

class DiagnosisHistoryResponse(BaseModel):
    """
    Schema สำหรับประวัติการวินิจฉัยที่ส่งกลับไปยัง client
    """
    id: int = Field(..., description="รหัสการวินิจฉัย")
    patient_id: Optional[int] = Field(None, description="รหัสผู้ป่วย (ถ้ามี)")
    symptoms: List[str] = Field(..., description="รายการอาการของผู้ป่วย")
    diagnosis: DiagnosisResponse = Field(..., description="ผลการวินิจฉัย")
    created_at: str = Field(..., description="วันเวลาที่วินิจฉัย")
    created_by: UserOut = Field(..., description="ผู้ที่ทำการวินิจฉัย")

# สร้าง endpoints

@router.post("/diagnose", response_model=DiagnosisResponse, status_code=status.HTTP_200_OK)
async def diagnose_symptoms(
    request: SymptomRequest,
    current_user: User = Depends(get_current_user)
) -> DiagnosisResponse:
    """
    วินิจฉัยโรคจากอาการและข้อมูลผู้ป่วย
    
    Args:
        request: ข้อมูลอาการและข้อมูลผู้ป่วย
        current_user: ผู้ใช้ที่กำลังใช้งาน
        
    Returns:
        DiagnosisResponse: ผลการวินิจฉัย
        
    Raises:
        HTTPException: หากมีข้อผิดพลาดในการวินิจฉัย
    """
    try:
        logger.info(f"User {current_user.username} requested diagnosis for symptoms: {request.symptoms}")
        
        # ในสถานการณ์จริง จะต้องสร้าง instance ของ MedicalDiagnosisAI และเรียกใช้
        # ai_diagnosis = MedicalDiagnosisAI()
        # diagnosis_result = ai_diagnosis.diagnose(request.symptoms, request.patient_data)
        
        # ตัวอย่างข้อมูลจำลอง
        if "ไข้" in request.symptoms and "ไอ" in request.symptoms and "เจ็บคอ" in request.symptoms:
            if "หายใจลำบาก" in request.symptoms:
                diagnosis_result = {
                    "disease": "โควิด-19",
                    "confidence": 0.85,
                    "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสโคโรนา SARS-CoV-2",
                    "recommendations": [
                        "แยกตัวจากผู้อื่นทันที",
                        "ตรวจหาเชื้อโควิด-19 ด้วยชุดตรวจ ATK หรือ RT-PCR",
                        "ติดต่อสายด่วนโควิด-19 หรือสถานพยาบาลใกล้บ้าน",
                        "ตรวจวัดระดับออกซิเจนในเลือดอย่างสม่ำเสมอ (ถ้ามีเครื่องวัด)",
                        "พักผ่อนให้เพียงพอ",
                        "ดื่มน้ำมากๆ",
                        "หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์",
                        "เช็ดตัวด้วยน้ำอุ่นเพื่อลดไข้",
                        "ดื่มน้ำอุ่นผสมน้ำผึ้งเพื่อบรรเทาอาการไอ",
                        "กลั้วคอด้วยน้ำเกลืออุ่นเพื่อบรรเทาอาการเจ็บคอ"
                    ],
                    "differential_diagnoses": [
                        {
                            "disease": "ไข้หวัดใหญ่",
                            "confidence": 0.65,
                            "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสอินฟลูเอนซา"
                        },
                        {
                            "disease": "ปอดอักเสบ",
                            "confidence": 0.45,
                            "description": "การอักเสบของเนื้อปอด มักเกิดจากการติดเชื้อ"
                        }
                    ]
                }
            else:
                diagnosis_result = {
                    "disease": "ไข้หวัดใหญ่",
                    "confidence": 0.75,
                    "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสอินฟลูเอนซา",
                    "recommendations": [
                        "ทานยาลดไข้ตามคำแนะนำของแพทย์",
                        "หลีกเลี่ยงการสัมผัสใกล้ชิดกับผู้อื่นเพื่อป้องกันการแพร่เชื้อ",
                        "สวมหน้ากากอนามัยเมื่อต้องอยู่ร่วมกับผู้อื่น",
                        "พักผ่อนให้เพียงพอ",
                        "ดื่มน้ำมากๆ",
                        "หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์",
                        "เช็ดตัวด้วยน้ำอุ่นเพื่อลดไข้",
                        "ดื่มน้ำอุ่นผสมน้ำผึ้งเพื่อบรรเทาอาการไอ",
                        "กลั้วคอด้วยน้ำเกลืออุ่นเพื่อบรรเทาอาการเจ็บคอ"
                    ],
                    "differential_diagnoses": [
                        {
                            "disease": "ไข้หวัดธรรมดา",
                            "confidence": 0.6,
                            "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสหลายชนิด"
                        },
                        {
                            "disease": "โควิด-19",
                            "confidence": 0.4,
                            "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสโคโรนา SARS-CoV-2"
                        }
                    ]
                }
        elif "ปวดหัว" in request.symptoms and "คลื่นไส้" in request.symptoms:
            diagnosis_result = {
                "disease": "ไมเกรน",
                "confidence": 0.8,
                "description": "อาการปวดศีรษะข้างเดียวหรือสองข้าง มักมีอาการคลื่นไส้ร่วมด้วย",
                "recommendations": [
                    "พักในที่เงียบและมืด",
                    "ประคบเย็นบริเวณที่ปวด",
                    "ทานยาแก้ปวดตามคำแนะนำของแพทย์",
                    "พักผ่อนให้เพียงพอ",
                    "ดื่มน้ำมากๆ",
                    "หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์"
                ],
                "differential_diagnoses": [
                    {
                        "disease": "ความดันโลหิตสูง",
                        "confidence": 0.4,
                        "description": "ภาวะที่ความดันในหลอดเลือดแดงสูงกว่าปกติ"
                    }
                ]
            }
        else:
            diagnosis_result = {
                "disease": "ไข้หวัดธรรมดา",
                "confidence": 0.5,
                "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสหลายชนิด",
                "recommendations": [
                    "ทานยาลดไข้ตามคำแนะนำของแพทย์",
                    "ใช้ยาพ่นจมูกหากมีอาการคัดจมูก",
                    "ดื่มน้ำอุ่นผสมน้ำผึ้งและมะนาวเพื่อบรรเทาอาการเจ็บคอ",
                    "พักผ่อนให้เพียงพอ",
                    "ดื่มน้ำมากๆ",
                    "หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์"
                ],
                "differential_diagnoses": []
            }
        
        # บันทึกผลการวินิจฉัยลงในฐานข้อมูล (ในสถานการณ์จริง)
        # diagnosis_history = DiagnosisHistory(
        #     patient_id=request.patient_id,
        #     symptoms=request.symptoms,
        #     diagnosis=diagnosis_result,
        #     created_by=current_user.id
        # )
        # db.add(diagnosis_history)
        # db.commit()
        
        logger.info(f"Diagnosis completed: {diagnosis_result['disease']} with confidence {diagnosis_result['confidence']}")
        
        return diagnosis_result
    except Exception as e:
        logger.error(f"Error in diagnosis: {e}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Error in diagnosis: {str(e)}"
        )

@router.get("/history", response_model=List[DiagnosisHistoryResponse], status_code=status.HTTP_200_OK)
async def get_diagnosis_history(
    patient_id: Optional[int] = None,
    current_user: User = Depends(get_current_user)
) -> List[DiagnosisHistoryResponse]:
    """
    ดึงประวัติการวินิจฉัยของผู้ป่วย
    
    Args:
        patient_id: รหัสผู้ป่วย (ถ้าไม่ระบุ จะดึงประวัติทั้งหมด)
        current_user: ผู้ใช้ที่กำลังใช้งาน
        
    Returns:
        List[DiagnosisHistoryResponse]: รายการประวัติการวินิจฉัย
        
    Raises:
        HTTPException: หากมีข้อผิดพลาดในการดึงข้อมูล
    """
    try:
        logger.info(f"User {current_user.username} requested diagnosis history for patient_id: {patient_id}")
        
        # ในสถานการณ์จริง จะต้องดึงข้อมูลจากฐานข้อมูล
        # if patient_id:
        #     history = db.query(DiagnosisHistory).filter(DiagnosisHistory.patient_id == patient_id).all()
        # else:
        #     history = db.query(DiagnosisHistory).all()
        
        # ตัวอย่างข้อมูลจำลอง
        history = [
            {
                "id": 1,
                "patient_id": 123 if patient_id else 123,
                "symptoms": ["ไข้", "ไอ", "เจ็บคอ", "หายใจลำบาก"],
                "diagnosis": {
                    "disease": "โควิด-19",
                    "confidence": 0.85,
                    "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสโคโรนา SARS-CoV-2",
                    "recommendations": [
                        "แยกตัวจากผู้อื่นทันที",
                        "ตรวจหาเชื้อโควิด-19 ด้วยชุดตรวจ ATK หรือ RT-PCR",
                        "ติดต่อสายด่วนโควิด-19 หรือสถานพยาบาลใกล้บ้าน",
                        "ตรวจวัดระดับออกซิเจนในเลือดอย่างสม่ำเสมอ (ถ้ามีเครื่องวัด)"
                    ],
                    "differential_diagnoses": [
                        {
                            "disease": "ไข้หวัดใหญ่",
                            "confidence": 0.65,
                            "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสอินฟลูเอนซา"
                        }
                    ]
                },
                "created_at": "2025-03-01T10:30:00",
                "created_by": {
                    "id": 1,
                    "username": "doctor1",
                    "email": "doctor1@meditech.com",
                    "full_name": "นพ.สมชาย ใจดี"
                }
            },
            {
                "id": 2,
                "patient_id": 456 if not patient_id else patient_id,
                "symptoms": ["ปวดหัว", "คลื่นไส้", "แสงจ้า"],
                "diagnosis": {
                    "disease": "ไมเกรน",
                    "confidence": 0.8,
                    "description": "อาการปวดศีรษะข้างเดียวหรือสองข้าง มักมีอาการคลื่นไส้ร่วมด้วย",
                    "recommendations": [
                        "พักในที่เงียบและมืด",
                        "ประคบเย็นบริเวณที่ปวด",
                        "ทานยาแก้ปวดตามคำแนะนำของแพทย์"
                    ],
                    "differential_diagnoses": [
                        {
                            "disease": "ความดันโลหิตสูง",
                            "confidence": 0.4,
                            "description": "ภาวะที่ความดันในหลอดเลือดแดงสูงกว่าปกติ"
                        }
                    ]
                },
                "created_at": "2025-03-01T11:15:00",
                "created_by": {
                    "id": 2,
                    "username": "doctor2",
                    "email": "doctor2@meditech.com",
                    "full_name": "พญ.สมศรี มีสุข"
                }
            }
        ]
        
        # กรองข้อมูลตาม patient_id ถ้ามีการระบุ
        if patient_id:
            history = [h for h in history if h["patient_id"] == patient_id]
        
        logger.info(f"Retrieved {len(history)} diagnosis history records")
        
        return history
    except Exception as e:
        logger.error(f"Error retrieving diagnosis history: {e}")
        raise HTTPException(
            status_code=status.HTTP_500_INTERNAL_SERVER_ERROR,
            detail=f"Error retrieving diagnosis history: {str(e)}"
        ) 