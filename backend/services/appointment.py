from typing import Any, Dict, List, Optional, Union
from datetime import date, datetime, time

from sqlalchemy.orm import Session
from sqlalchemy import and_, or_, between

from backend.models.appointment import Appointment, AppointmentStatus
from backend.schemas.appointment import AppointmentCreate, AppointmentUpdate
from backend.services.base import BaseService


class AppointmentService(BaseService[Appointment, AppointmentCreate, AppointmentUpdate]):
    def get_multi(
        self, db: Session, *, skip: int = 0, limit: int = 100, 
        patient_id: Optional[int] = None, doctor_id: Optional[int] = None,
        start_date: Optional[date] = None, end_date: Optional[date] = None,
        status: Optional[str] = None
    ) -> List[Appointment]:
        query = db.query(Appointment)
        
        if patient_id:
            query = query.filter(Appointment.patient_id == patient_id)
        
        if doctor_id:
            query = query.filter(Appointment.doctor_id == doctor_id)
        
        if start_date:
            start_datetime = datetime.combine(start_date, time.min)
            query = query.filter(Appointment.appointment_datetime >= start_datetime)
        
        if end_date:
            end_datetime = datetime.combine(end_date, time.max)
            query = query.filter(Appointment.appointment_datetime <= end_datetime)
        
        if status:
            query = query.filter(Appointment.status == status)
        
        return query.order_by(Appointment.appointment_datetime).offset(skip).limit(limit).all()
    
    def is_time_slot_available(
        self, db: Session, *, doctor_id: int, appointment_date: date, 
        start_time: time, end_time: time, exclude_appointment_id: Optional[int] = None
    ) -> bool:
        # แปลงวันที่และเวลาเป็น datetime
        start_datetime = datetime.combine(appointment_date, start_time)
        end_datetime = datetime.combine(appointment_date, end_time)
        
        # สร้าง query เพื่อตรวจสอบว่ามีการนัดหมายที่ทับซ้อนกันหรือไม่
        query = db.query(Appointment).filter(
            Appointment.doctor_id == doctor_id,
            Appointment.status != AppointmentStatus.CANCELLED,
            or_(
                # กรณีที่เวลาเริ่มต้นอยู่ในช่วงเวลาของการนัดหมายที่มีอยู่แล้ว
                and_(
                    Appointment.appointment_datetime <= start_datetime,
                    Appointment.end_datetime > start_datetime
                ),
                # กรณีที่เวลาสิ้นสุดอยู่ในช่วงเวลาของการนัดหมายที่มีอยู่แล้ว
                and_(
                    Appointment.appointment_datetime < end_datetime,
                    Appointment.end_datetime >= end_datetime
                ),
                # กรณีที่ช่วงเวลาครอบคลุมการนัดหมายที่มีอยู่แล้ว
                and_(
                    Appointment.appointment_datetime >= start_datetime,
                    Appointment.end_datetime <= end_datetime
                )
            )
        )
        
        # ถ้ามีการระบุ exclude_appointment_id ให้ไม่นับการนัดหมายนั้น
        if exclude_appointment_id:
            query = query.filter(Appointment.id != exclude_appointment_id)
        
        # ถ้าไม่มีการนัดหมายที่ทับซ้อนกัน จะคืนค่า True
        return query.count() == 0
    
    def create(self, db: Session, *, obj_in: AppointmentCreate, created_by_id: int) -> Appointment:
        return super().create(db, obj_in=obj_in)
    
    def update(
        self, db: Session, *, db_obj: Appointment, obj_in: Union[AppointmentUpdate, Dict[str, Any]], updated_by_id: Optional[int] = None
    ) -> Appointment:
        return super().update(db, db_obj=db_obj, obj_in=obj_in)
    
    def cancel(self, db: Session, *, id: int, cancelled_by_id: int) -> Appointment:
        obj = db.query(Appointment).get(id)
        obj.status = AppointmentStatus.CANCELLED
        db.add(obj)
        db.commit()
        db.refresh(obj)
        return obj


appointment_service = AppointmentService(Appointment) 