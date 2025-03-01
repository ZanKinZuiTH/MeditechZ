from typing import Any, List, Optional
from datetime import date
from fastapi import APIRouter, Depends, HTTPException, Query, status
from sqlalchemy.orm import Session

from backend.core.deps import get_current_active_user, get_db
from backend.schemas.medical_record import MedicalRecord, MedicalRecordCreate, MedicalRecordUpdate
from backend.schemas.user import User
from backend.services.medical_record import medical_record_service

router = APIRouter()

@router.get("/", response_model=List[MedicalRecord])
def read_medical_records(
    db: Session = Depends(get_db),
    skip: int = 0,
    limit: int = 100,
    patient_id: Optional[int] = Query(None, description="กรองตาม ID ผู้ป่วย"),
    doctor_id: Optional[int] = Query(None, description="กรองตาม ID แพทย์"),
    start_date: Optional[date] = Query(None, description="กรองตามวันที่เริ่มต้น"),
    end_date: Optional[date] = Query(None, description="กรองตามวันที่สิ้นสุด"),
    diagnosis: Optional[str] = Query(None, description="ค้นหาจากการวินิจฉัย"),
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลประวัติการรักษาทั้งหมด
    """
    medical_records = medical_record_service.get_multi(
        db, 
        skip=skip, 
        limit=limit, 
        patient_id=patient_id,
        doctor_id=doctor_id,
        start_date=start_date,
        end_date=end_date,
        diagnosis=diagnosis
    )
    return medical_records

@router.post("/", response_model=MedicalRecord)
def create_medical_record(
    *,
    db: Session = Depends(get_db),
    medical_record_in: MedicalRecordCreate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    สร้างประวัติการรักษาใหม่
    """
    medical_record = medical_record_service.create(db, obj_in=medical_record_in, created_by_id=current_user.id)
    return medical_record

@router.get("/{medical_record_id}", response_model=MedicalRecord)
def read_medical_record(
    *,
    db: Session = Depends(get_db),
    medical_record_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลประวัติการรักษาตาม ID
    """
    medical_record = medical_record_service.get(db, id=medical_record_id)
    if not medical_record:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบประวัติการรักษานี้",
        )
    return medical_record

@router.put("/{medical_record_id}", response_model=MedicalRecord)
def update_medical_record(
    *,
    db: Session = Depends(get_db),
    medical_record_id: int,
    medical_record_in: MedicalRecordUpdate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    อัปเดตข้อมูลประวัติการรักษา
    """
    medical_record = medical_record_service.get(db, id=medical_record_id)
    if not medical_record:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบประวัติการรักษานี้",
        )
    medical_record = medical_record_service.update(db, db_obj=medical_record, obj_in=medical_record_in, updated_by_id=current_user.id)
    return medical_record

@router.get("/patient/{patient_id}", response_model=List[MedicalRecord])
def read_patient_medical_records(
    *,
    db: Session = Depends(get_db),
    patient_id: int,
    skip: int = 0,
    limit: int = 100,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลประวัติการรักษาทั้งหมดของผู้ป่วย
    """
    medical_records = medical_record_service.get_by_patient(db, patient_id=patient_id, skip=skip, limit=limit)
    return medical_records

@router.get("/latest/{patient_id}", response_model=MedicalRecord)
def read_latest_medical_record(
    *,
    db: Session = Depends(get_db),
    patient_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลประวัติการรักษาล่าสุดของผู้ป่วย
    """
    medical_record = medical_record_service.get_latest_by_patient(db, patient_id=patient_id)
    if not medical_record:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบประวัติการรักษาของผู้ป่วยนี้",
        )
    return medical_record