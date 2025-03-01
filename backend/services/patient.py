from typing import Any, Dict, List, Optional, Union

from sqlalchemy.orm import Session
from sqlalchemy import or_

from backend.models.patient import Patient
from backend.schemas.patient import PatientCreate, PatientUpdate
from backend.services.base import BaseService


class PatientService(BaseService[Patient, PatientCreate, PatientUpdate]):
    def get_by_id_card_number(self, db: Session, *, id_card_number: str) -> Optional[Patient]:
        return db.query(Patient).filter(Patient.id_card_number == id_card_number).first()
    
    def get_multi(
        self, db: Session, *, skip: int = 0, limit: int = 100, search: Optional[str] = None
    ) -> List[Patient]:
        query = db.query(Patient)
        if search:
            search_term = f"%{search}%"
            query = query.filter(
                or_(
                    Patient.first_name.ilike(search_term),
                    Patient.last_name.ilike(search_term),
                    Patient.id_card_number.ilike(search_term)
                )
            )
        return query.offset(skip).limit(limit).all()
    
    def create(self, db: Session, *, obj_in: PatientCreate, created_by_id: int) -> Patient:
        # ตรวจสอบว่ามีผู้ป่วยที่มีเลขบัตรประชาชนนี้แล้วหรือไม่
        existing_patient = self.get_by_id_card_number(db, id_card_number=obj_in.id_card_number)
        if existing_patient:
            # ถ้ามีแล้ว ให้อัปเดตข้อมูลแทน
            return self.update(db, db_obj=existing_patient, obj_in=obj_in)
        
        # ถ้ายังไม่มี ให้สร้างใหม่
        return super().create(db, obj_in=obj_in)
    
    def update(
        self, db: Session, *, db_obj: Patient, obj_in: Union[PatientUpdate, Dict[str, Any]], updated_by_id: Optional[int] = None
    ) -> Patient:
        return super().update(db, db_obj=db_obj, obj_in=obj_in)
    
    def remove(self, db: Session, *, id: int, deleted_by_id: int) -> Patient:
        # Soft delete โดยการตั้งค่า is_active เป็น False
        obj = db.query(Patient).get(id)
        obj.is_active = False
        db.add(obj)
        db.commit()
        db.refresh(obj)
        return obj


patient_service = PatientService(Patient) 