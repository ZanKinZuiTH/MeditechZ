from typing import Any, List, Optional
from fastapi import APIRouter, Depends, HTTPException, Query, status
from sqlalchemy.orm import Session

from backend.core.deps import get_current_active_user, get_db
from backend.schemas.patient import Patient, PatientCreate, PatientUpdate
from backend.schemas.user import User
from backend.services.patient import patient_service

router = APIRouter()

@router.get("/", response_model=List[Patient])
def read_patients(
    db: Session = Depends(get_db),
    skip: int = 0,
    limit: int = 100,
    search: Optional[str] = Query(None, description="ค้นหาจากชื่อหรือนามสกุล"),
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลผู้ป่วยทั้งหมด
    """
    patients = patient_service.get_multi(db, skip=skip, limit=limit, search=search)
    return patients

@router.post("/", response_model=Patient)
def create_patient(
    *,
    db: Session = Depends(get_db),
    patient_in: PatientCreate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    สร้างผู้ป่วยใหม่
    """
    patient = patient_service.create(db, obj_in=patient_in, created_by_id=current_user.id)
    return patient

@router.get("/{patient_id}", response_model=Patient)
def read_patient(
    *,
    db: Session = Depends(get_db),
    patient_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลผู้ป่วยตาม ID
    """
    patient = patient_service.get(db, id=patient_id)
    if not patient:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบผู้ป่วยนี้",
        )
    return patient

@router.put("/{patient_id}", response_model=Patient)
def update_patient(
    *,
    db: Session = Depends(get_db),
    patient_id: int,
    patient_in: PatientUpdate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    อัปเดตข้อมูลผู้ป่วย
    """
    patient = patient_service.get(db, id=patient_id)
    if not patient:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบผู้ป่วยนี้",
        )
    patient = patient_service.update(db, db_obj=patient, obj_in=patient_in, updated_by_id=current_user.id)
    return patient

@router.delete("/{patient_id}", response_model=Patient)
def delete_patient(
    *,
    db: Session = Depends(get_db),
    patient_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ลบผู้ป่วย (soft delete)
    """
    patient = patient_service.get(db, id=patient_id)
    if not patient:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบผู้ป่วยนี้",
        )
    patient = patient_service.remove(db, id=patient_id, deleted_by_id=current_user.id)
    return patient 