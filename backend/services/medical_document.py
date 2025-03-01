from typing import Any, Dict, Generic, List, Optional, Type, TypeVar, Union
from datetime import datetime, date
from fastapi.encoders import jsonable_encoder
from pydantic import BaseModel
from sqlalchemy.orm import Session

from backend.db.base_class import Base
from backend.models.medical_document import MedicalDocument, MedicalCertificate, HealthCheckupBook
from backend.schemas.medical_document import (
    MedicalDocumentCreate, MedicalDocumentUpdate,
    MedicalCertificateCreate, MedicalCertificateUpdate,
    HealthCheckupBookCreate, HealthCheckupBookUpdate
)

ModelType = TypeVar("ModelType", bound=Base)
CreateSchemaType = TypeVar("CreateSchemaType", bound=BaseModel)
UpdateSchemaType = TypeVar("UpdateSchemaType", bound=BaseModel)


class BaseService(Generic[ModelType, CreateSchemaType, UpdateSchemaType]):
    def __init__(self, model: Type[ModelType]):
        """
        บริการ CRUD พื้นฐานสำหรับโมเดล
        """
        self.model = model

    def get(self, db: Session, id: Any) -> Optional[ModelType]:
        return db.query(self.model).filter(self.model.id == id).first()

    def get_multi(
        self, db: Session, *, skip: int = 0, limit: int = 100
    ) -> List[ModelType]:
        return db.query(self.model).offset(skip).limit(limit).all()

    def create(self, db: Session, *, obj_in: CreateSchemaType, created_by_id: int) -> ModelType:
        obj_in_data = jsonable_encoder(obj_in)
        db_obj = self.model(**obj_in_data, created_by_id=created_by_id)
        db.add(db_obj)
        db.commit()
        db.refresh(db_obj)
        return db_obj

    def update(
        self,
        db: Session,
        *,
        db_obj: ModelType,
        obj_in: Union[UpdateSchemaType, Dict[str, Any]],
        updated_by_id: int
    ) -> ModelType:
        obj_data = jsonable_encoder(db_obj)
        if isinstance(obj_in, dict):
            update_data = obj_in
        else:
            update_data = obj_in.dict(exclude_unset=True)
        for field in obj_data:
            if field in update_data:
                setattr(db_obj, field, update_data[field])
        
        db_obj.updated_at = datetime.now()
        db_obj.updated_by_id = updated_by_id
        
        db.add(db_obj)
        db.commit()
        db.refresh(db_obj)
        return db_obj

    def remove(self, db: Session, *, id: int) -> ModelType:
        obj = db.query(self.model).get(id)
        db.delete(obj)
        db.commit()
        return obj


class MedicalDocumentService(BaseService[MedicalDocument, MedicalDocumentCreate, MedicalDocumentUpdate]):
    def get_by_patient(
        self, db: Session, *, patient_id: int, skip: int = 0, limit: int = 100
    ) -> List[MedicalDocument]:
        return (
            db.query(self.model)
            .filter(self.model.patient_id == patient_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_doctor(
        self, db: Session, *, doctor_id: int, skip: int = 0, limit: int = 100
    ) -> List[MedicalDocument]:
        return (
            db.query(self.model)
            .filter(self.model.doctor_id == doctor_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_visit(
        self, db: Session, *, visit_id: int, skip: int = 0, limit: int = 100
    ) -> List[MedicalDocument]:
        return (
            db.query(self.model)
            .filter(self.model.visit_id == visit_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_type(
        self, db: Session, *, document_type: str, skip: int = 0, limit: int = 100
    ) -> List[MedicalDocument]:
        return (
            db.query(self.model)
            .filter(self.model.document_type == document_type)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_date_range(
        self, db: Session, *, start_date: date, end_date: date, skip: int = 0, limit: int = 100
    ) -> List[MedicalDocument]:
        return (
            db.query(self.model)
            .filter(self.model.document_date >= start_date)
            .filter(self.model.document_date <= end_date)
            .offset(skip)
            .limit(limit)
            .all()
        )


class MedicalCertificateService(BaseService[MedicalCertificate, MedicalCertificateCreate, MedicalCertificateUpdate]):
    def get_by_patient(
        self, db: Session, *, patient_id: int, skip: int = 0, limit: int = 100
    ) -> List[MedicalCertificate]:
        return (
            db.query(self.model)
            .filter(self.model.patient_id == patient_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_doctor(
        self, db: Session, *, doctor_id: int, skip: int = 0, limit: int = 100
    ) -> List[MedicalCertificate]:
        return (
            db.query(self.model)
            .filter(self.model.doctor_id == doctor_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_visit(
        self, db: Session, *, visit_id: int, skip: int = 0, limit: int = 100
    ) -> List[MedicalCertificate]:
        return (
            db.query(self.model)
            .filter(self.model.visit_id == visit_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_type(
        self, db: Session, *, certificate_type: str, skip: int = 0, limit: int = 100
    ) -> List[MedicalCertificate]:
        return (
            db.query(self.model)
            .filter(self.model.certificate_type == certificate_type)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_date_range(
        self, db: Session, *, start_date: date, end_date: date, skip: int = 0, limit: int = 100
    ) -> List[MedicalCertificate]:
        return (
            db.query(self.model)
            .filter(self.model.certificate_date >= start_date)
            .filter(self.model.certificate_date <= end_date)
            .offset(skip)
            .limit(limit)
            .all()
        )


class HealthCheckupBookService(BaseService[HealthCheckupBook, HealthCheckupBookCreate, HealthCheckupBookUpdate]):
    def get_by_patient(
        self, db: Session, *, patient_id: int, skip: int = 0, limit: int = 100
    ) -> List[HealthCheckupBook]:
        return (
            db.query(self.model)
            .filter(self.model.patient_id == patient_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_doctor(
        self, db: Session, *, doctor_id: int, skip: int = 0, limit: int = 100
    ) -> List[HealthCheckupBook]:
        return (
            db.query(self.model)
            .filter(self.model.doctor_id == doctor_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_visit(
        self, db: Session, *, visit_id: int, skip: int = 0, limit: int = 100
    ) -> List[HealthCheckupBook]:
        return (
            db.query(self.model)
            .filter(self.model.visit_id == visit_id)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_type(
        self, db: Session, *, checkup_type: str, skip: int = 0, limit: int = 100
    ) -> List[HealthCheckupBook]:
        return (
            db.query(self.model)
            .filter(self.model.checkup_type == checkup_type)
            .offset(skip)
            .limit(limit)
            .all()
        )

    def get_by_date_range(
        self, db: Session, *, start_date: date, end_date: date, skip: int = 0, limit: int = 100
    ) -> List[HealthCheckupBook]:
        return (
            db.query(self.model)
            .filter(self.model.checkup_date >= start_date)
            .filter(self.model.checkup_date <= end_date)
            .offset(skip)
            .limit(limit)
            .all()
        )


medical_document_service = MedicalDocumentService(MedicalDocument)
medical_certificate_service = MedicalCertificateService(MedicalCertificate)
health_checkup_book_service = HealthCheckupBookService(HealthCheckupBook) 