from typing import Any, Dict, List, Optional, Union
from datetime import date, datetime

from sqlalchemy.orm import Session
from sqlalchemy import and_, or_, desc

from backend.models.medical_record import MedicalRecord
from backend.schemas.medical_record import MedicalRecordCreate, MedicalRecordUpdate
from backend.services.base import BaseService


class MedicalRecordService(BaseService[MedicalRecord, MedicalRecordCreate, MedicalRecordUpdate]):
    def get_multi(
        self, db: Session, *, skip: int = 0, limit: int = 100, 
        patient_id: Optional[int] = None, doctor_id: Optional[int] = None,
        start_date: Optional[date] = None, end_date: Optional[date] = None,
        diagnosis: Optional[str] = None
    ) -> List[MedicalRecord]:
        query = db.query(MedicalRecord)
        
        if patient_id:
            query = query.filter(MedicalRecord.patient_id == patient_id)
        
        if doctor_id:
            query = query.filter(MedicalRecord.doctor_id == doctor_id)
        
        if start_date:
            start_datetime = datetime.combine(start_date, datetime.min.time())
            query = query.filter(MedicalRecord.record_date >= start_datetime)
        
        if end_date:
            end_datetime = datetime.combine(end_date, datetime.max.time())
            query = query.filter(MedicalRecord.record_date <= end_datetime)
        
        if diagnosis:
            query = query.filter(MedicalRecord.diagnosis.ilike(f"%{diagnosis}%"))
        
        return query.order_by(desc(MedicalRecord.record_date)).offset(skip).limit(limit).all()
    
    def get_by_patient(
        self, db: Session, *, patient_id: int, skip: int = 0, limit: int = 100
    ) -> List[MedicalRecord]:
        return (
            db.query(MedicalRecord)
            .filter(MedicalRecord.patient_id == patient_id)
            .order_by(desc(MedicalRecord.record_date))
            .offset(skip)
            .limit(limit)
            .all()
        )
    
    def get_latest_by_patient(self, db: Session, *, patient_id: int) -> Optional[MedicalRecord]:
        return (
            db.query(MedicalRecord)
            .filter(MedicalRecord.patient_id == patient_id)
            .order_by(desc(MedicalRecord.record_date))
            .first()
        )
    
    def create(self, db: Session, *, obj_in: MedicalRecordCreate, created_by_id: int) -> MedicalRecord:
        return super().create(db, obj_in=obj_in)
    
    def update(
        self, db: Session, *, db_obj: MedicalRecord, obj_in: Union[MedicalRecordUpdate, Dict[str, Any]], updated_by_id: Optional[int] = None
    ) -> MedicalRecord:
        return super().update(db, db_obj=db_obj, obj_in=obj_in)


medical_record_service = MedicalRecordService(MedicalRecord) 