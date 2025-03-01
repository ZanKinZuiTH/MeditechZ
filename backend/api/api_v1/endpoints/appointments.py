from typing import Any, List, Optional
from datetime import date, datetime
from fastapi import APIRouter, Depends, HTTPException, Query, status
from sqlalchemy.orm import Session

from backend.core.deps import get_current_active_user, get_db
from backend.schemas.appointment import Appointment, AppointmentCreate, AppointmentUpdate
from backend.schemas.user import User
from backend.services.appointment import appointment_service

router = APIRouter()

@router.get("/", response_model=List[Appointment])
def read_appointments(
    db: Session = Depends(get_db),
    skip: int = 0,
    limit: int = 100,
    patient_id: Optional[int] = Query(None, description="กรองตาม ID ผู้ป่วย"),
    doctor_id: Optional[int] = Query(None, description="กรองตาม ID แพทย์"),
    start_date: Optional[date] = Query(None, description="กรองตามวันที่เริ่มต้น"),
    end_date: Optional[date] = Query(None, description="กรองตามวันที่สิ้นสุด"),
    status: Optional[str] = Query(None, description="กรองตามสถานะการนัดหมาย"),
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลการนัดหมายทั้งหมด
    """
    appointments = appointment_service.get_multi(
        db, 
        skip=skip, 
        limit=limit, 
        patient_id=patient_id,
        doctor_id=doctor_id,
        start_date=start_date,
        end_date=end_date,
        status=status
    )
    return appointments

@router.post("/", response_model=Appointment)
def create_appointment(
    *,
    db: Session = Depends(get_db),
    appointment_in: AppointmentCreate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    สร้างการนัดหมายใหม่
    """
    # ตรวจสอบว่าเวลานัดหมายว่างหรือไม่
    if appointment_service.is_time_slot_available(
        db, 
        doctor_id=appointment_in.doctor_id,
        appointment_date=appointment_in.appointment_date,
        start_time=appointment_in.start_time,
        end_time=appointment_in.end_time
    ) is False:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="เวลานัดหมายนี้ไม่ว่าง กรุณาเลือกเวลาอื่น",
        )
    
    appointment = appointment_service.create(db, obj_in=appointment_in, created_by_id=current_user.id)
    return appointment

@router.get("/{appointment_id}", response_model=Appointment)
def read_appointment(
    *,
    db: Session = Depends(get_db),
    appointment_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลการนัดหมายตาม ID
    """
    appointment = appointment_service.get(db, id=appointment_id)
    if not appointment:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบการนัดหมายนี้",
        )
    return appointment

@router.put("/{appointment_id}", response_model=Appointment)
def update_appointment(
    *,
    db: Session = Depends(get_db),
    appointment_id: int,
    appointment_in: AppointmentUpdate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    อัปเดตข้อมูลการนัดหมาย
    """
    appointment = appointment_service.get(db, id=appointment_id)
    if not appointment:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบการนัดหมายนี้",
        )
    
    # ตรวจสอบว่าเวลานัดหมายว่างหรือไม่ (ถ้ามีการเปลี่ยนเวลา)
    if (appointment_in.doctor_id and appointment_in.doctor_id != appointment.doctor_id) or \
       (appointment_in.appointment_date and appointment_in.appointment_date != appointment.appointment_date) or \
       (appointment_in.start_time and appointment_in.start_time != appointment.start_time) or \
       (appointment_in.end_time and appointment_in.end_time != appointment.end_time):
        
        doctor_id = appointment_in.doctor_id or appointment.doctor_id
        appointment_date = appointment_in.appointment_date or appointment.appointment_date
        start_time = appointment_in.start_time or appointment.start_time
        end_time = appointment_in.end_time or appointment.end_time
        
        if appointment_service.is_time_slot_available(
            db, 
            doctor_id=doctor_id,
            appointment_date=appointment_date,
            start_time=start_time,
            end_time=end_time,
            exclude_appointment_id=appointment_id
        ) is False:
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="เวลานัดหมายนี้ไม่ว่าง กรุณาเลือกเวลาอื่น",
            )
    
    appointment = appointment_service.update(db, db_obj=appointment, obj_in=appointment_in, updated_by_id=current_user.id)
    return appointment

@router.delete("/{appointment_id}", response_model=Appointment)
def delete_appointment(
    *,
    db: Session = Depends(get_db),
    appointment_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ยกเลิกการนัดหมาย
    """
    appointment = appointment_service.get(db, id=appointment_id)
    if not appointment:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบการนัดหมายนี้",
        )
    appointment = appointment_service.cancel(db, id=appointment_id, cancelled_by_id=current_user.id)
    return appointment 