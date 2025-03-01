from typing import Any, List, Optional
from datetime import date
from fastapi import APIRouter, Depends, HTTPException, Query, status
from sqlalchemy.orm import Session

from backend.core.deps import get_current_active_user, get_db
from backend.schemas.medical_document import (
    MedicalDocument, MedicalDocumentCreate, MedicalDocumentUpdate,
    MedicalCertificate, MedicalCertificateCreate, MedicalCertificateUpdate,
    HealthCheckupBook, HealthCheckupBookCreate, HealthCheckupBookUpdate
)
from backend.schemas.user import User
from backend.services.medical_document import (
    medical_document_service,
    medical_certificate_service,
    health_checkup_book_service
)

router = APIRouter()

# ============= Medical Document Endpoints =============

@router.get("/", response_model=List[MedicalDocument])
def read_medical_documents(
    db: Session = Depends(get_db),
    skip: int = 0,
    limit: int = 100,
    patient_id: Optional[int] = Query(None, description="กรองตาม ID ผู้ป่วย"),
    doctor_id: Optional[int] = Query(None, description="กรองตาม ID แพทย์"),
    visit_id: Optional[int] = Query(None, description="กรองตาม ID การเข้ารับบริการ"),
    document_type: Optional[str] = Query(None, description="กรองตามประเภทเอกสาร"),
    start_date: Optional[date] = Query(None, description="กรองตามวันที่เริ่มต้น"),
    end_date: Optional[date] = Query(None, description="กรองตามวันที่สิ้นสุด"),
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลเอกสารทางการแพทย์ทั้งหมด
    """
    if patient_id:
        medical_documents = medical_document_service.get_by_patient(db, patient_id=patient_id, skip=skip, limit=limit)
    elif doctor_id:
        medical_documents = medical_document_service.get_by_doctor(db, doctor_id=doctor_id, skip=skip, limit=limit)
    elif visit_id:
        medical_documents = medical_document_service.get_by_visit(db, visit_id=visit_id, skip=skip, limit=limit)
    elif document_type:
        medical_documents = medical_document_service.get_by_type(db, document_type=document_type, skip=skip, limit=limit)
    elif start_date and end_date:
        medical_documents = medical_document_service.get_by_date_range(db, start_date=start_date, end_date=end_date, skip=skip, limit=limit)
    else:
        medical_documents = medical_document_service.get_multi(db, skip=skip, limit=limit)
    
    return medical_documents

@router.post("/", response_model=MedicalDocument)
def create_medical_document(
    *,
    db: Session = Depends(get_db),
    medical_document_in: MedicalDocumentCreate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    สร้างเอกสารทางการแพทย์ใหม่
    """
    medical_document = medical_document_service.create(db, obj_in=medical_document_in, created_by_id=current_user.id)
    return medical_document

@router.get("/{document_id}", response_model=MedicalDocument)
def read_medical_document(
    *,
    db: Session = Depends(get_db),
    document_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลเอกสารทางการแพทย์ตาม ID
    """
    medical_document = medical_document_service.get(db, id=document_id)
    if not medical_document:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบเอกสารทางการแพทย์นี้",
        )
    return medical_document

@router.put("/{document_id}", response_model=MedicalDocument)
def update_medical_document(
    *,
    db: Session = Depends(get_db),
    document_id: int,
    medical_document_in: MedicalDocumentUpdate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    อัปเดตข้อมูลเอกสารทางการแพทย์
    """
    medical_document = medical_document_service.get(db, id=document_id)
    if not medical_document:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบเอกสารทางการแพทย์นี้",
        )
    medical_document = medical_document_service.update(db, db_obj=medical_document, obj_in=medical_document_in, updated_by_id=current_user.id)
    return medical_document

@router.delete("/{document_id}", response_model=MedicalDocument)
def delete_medical_document(
    *,
    db: Session = Depends(get_db),
    document_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ลบเอกสารทางการแพทย์
    """
    medical_document = medical_document_service.get(db, id=document_id)
    if not medical_document:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบเอกสารทางการแพทย์นี้",
        )
    medical_document = medical_document_service.remove(db, id=document_id)
    return medical_document

# ============= Medical Certificate Endpoints =============

@router.get("/certificates/", response_model=List[MedicalCertificate])
def read_medical_certificates(
    db: Session = Depends(get_db),
    skip: int = 0,
    limit: int = 100,
    patient_id: Optional[int] = Query(None, description="กรองตาม ID ผู้ป่วย"),
    doctor_id: Optional[int] = Query(None, description="กรองตาม ID แพทย์"),
    visit_id: Optional[int] = Query(None, description="กรองตาม ID การเข้ารับบริการ"),
    certificate_type: Optional[str] = Query(None, description="กรองตามประเภทใบรับรองแพทย์"),
    start_date: Optional[date] = Query(None, description="กรองตามวันที่เริ่มต้น"),
    end_date: Optional[date] = Query(None, description="กรองตามวันที่สิ้นสุด"),
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลใบรับรองแพทย์ทั้งหมด
    """
    if patient_id:
        certificates = medical_certificate_service.get_by_patient(db, patient_id=patient_id, skip=skip, limit=limit)
    elif doctor_id:
        certificates = medical_certificate_service.get_by_doctor(db, doctor_id=doctor_id, skip=skip, limit=limit)
    elif visit_id:
        certificates = medical_certificate_service.get_by_visit(db, visit_id=visit_id, skip=skip, limit=limit)
    elif certificate_type:
        certificates = medical_certificate_service.get_by_type(db, certificate_type=certificate_type, skip=skip, limit=limit)
    elif start_date and end_date:
        certificates = medical_certificate_service.get_by_date_range(db, start_date=start_date, end_date=end_date, skip=skip, limit=limit)
    else:
        certificates = medical_certificate_service.get_multi(db, skip=skip, limit=limit)
    
    return certificates

@router.post("/certificates/", response_model=MedicalCertificate)
def create_medical_certificate(
    *,
    db: Session = Depends(get_db),
    certificate_in: MedicalCertificateCreate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    สร้างใบรับรองแพทย์ใหม่
    """
    certificate = medical_certificate_service.create(db, obj_in=certificate_in, created_by_id=current_user.id)
    return certificate

@router.get("/certificates/{certificate_id}", response_model=MedicalCertificate)
def read_medical_certificate(
    *,
    db: Session = Depends(get_db),
    certificate_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลใบรับรองแพทย์ตาม ID
    """
    certificate = medical_certificate_service.get(db, id=certificate_id)
    if not certificate:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบใบรับรองแพทย์นี้",
        )
    return certificate

@router.put("/certificates/{certificate_id}", response_model=MedicalCertificate)
def update_medical_certificate(
    *,
    db: Session = Depends(get_db),
    certificate_id: int,
    certificate_in: MedicalCertificateUpdate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    อัปเดตข้อมูลใบรับรองแพทย์
    """
    certificate = medical_certificate_service.get(db, id=certificate_id)
    if not certificate:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบใบรับรองแพทย์นี้",
        )
    certificate = medical_certificate_service.update(db, db_obj=certificate, obj_in=certificate_in, updated_by_id=current_user.id)
    return certificate

@router.delete("/certificates/{certificate_id}", response_model=MedicalCertificate)
def delete_medical_certificate(
    *,
    db: Session = Depends(get_db),
    certificate_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ลบใบรับรองแพทย์
    """
    certificate = medical_certificate_service.get(db, id=certificate_id)
    if not certificate:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบใบรับรองแพทย์นี้",
        )
    certificate = medical_certificate_service.remove(db, id=certificate_id)
    return certificate

# ============= Health Checkup Book Endpoints =============

@router.get("/checkup-books/", response_model=List[HealthCheckupBook])
def read_health_checkup_books(
    db: Session = Depends(get_db),
    skip: int = 0,
    limit: int = 100,
    patient_id: Optional[int] = Query(None, description="กรองตาม ID ผู้ป่วย"),
    doctor_id: Optional[int] = Query(None, description="กรองตาม ID แพทย์"),
    visit_id: Optional[int] = Query(None, description="กรองตาม ID การเข้ารับบริการ"),
    checkup_type: Optional[str] = Query(None, description="กรองตามประเภทการตรวจสุขภาพ"),
    start_date: Optional[date] = Query(None, description="กรองตามวันที่เริ่มต้น"),
    end_date: Optional[date] = Query(None, description="กรองตามวันที่สิ้นสุด"),
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลสมุดตรวจสุขภาพทั้งหมด
    """
    if patient_id:
        checkup_books = health_checkup_book_service.get_by_patient(db, patient_id=patient_id, skip=skip, limit=limit)
    elif doctor_id:
        checkup_books = health_checkup_book_service.get_by_doctor(db, doctor_id=doctor_id, skip=skip, limit=limit)
    elif visit_id:
        checkup_books = health_checkup_book_service.get_by_visit(db, visit_id=visit_id, skip=skip, limit=limit)
    elif checkup_type:
        checkup_books = health_checkup_book_service.get_by_type(db, checkup_type=checkup_type, skip=skip, limit=limit)
    elif start_date and end_date:
        checkup_books = health_checkup_book_service.get_by_date_range(db, start_date=start_date, end_date=end_date, skip=skip, limit=limit)
    else:
        checkup_books = health_checkup_book_service.get_multi(db, skip=skip, limit=limit)
    
    return checkup_books

@router.post("/checkup-books/", response_model=HealthCheckupBook)
def create_health_checkup_book(
    *,
    db: Session = Depends(get_db),
    checkup_book_in: HealthCheckupBookCreate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    สร้างสมุดตรวจสุขภาพใหม่
    """
    checkup_book = health_checkup_book_service.create(db, obj_in=checkup_book_in, created_by_id=current_user.id)
    return checkup_book

@router.get("/checkup-books/{checkup_book_id}", response_model=HealthCheckupBook)
def read_health_checkup_book(
    *,
    db: Session = Depends(get_db),
    checkup_book_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ดึงข้อมูลสมุดตรวจสุขภาพตาม ID
    """
    checkup_book = health_checkup_book_service.get(db, id=checkup_book_id)
    if not checkup_book:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบสมุดตรวจสุขภาพนี้",
        )
    return checkup_book

@router.put("/checkup-books/{checkup_book_id}", response_model=HealthCheckupBook)
def update_health_checkup_book(
    *,
    db: Session = Depends(get_db),
    checkup_book_id: int,
    checkup_book_in: HealthCheckupBookUpdate,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    อัปเดตข้อมูลสมุดตรวจสุขภาพ
    """
    checkup_book = health_checkup_book_service.get(db, id=checkup_book_id)
    if not checkup_book:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบสมุดตรวจสุขภาพนี้",
        )
    checkup_book = health_checkup_book_service.update(db, db_obj=checkup_book, obj_in=checkup_book_in, updated_by_id=current_user.id)
    return checkup_book

@router.delete("/checkup-books/{checkup_book_id}", response_model=HealthCheckupBook)
def delete_health_checkup_book(
    *,
    db: Session = Depends(get_db),
    checkup_book_id: int,
    current_user: User = Depends(get_current_active_user),
) -> Any:
    """
    ลบสมุดตรวจสุขภาพ
    """
    checkup_book = health_checkup_book_service.get(db, id=checkup_book_id)
    if not checkup_book:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบสมุดตรวจสุขภาพนี้",
        )
    checkup_book = health_checkup_book_service.remove(db, id=checkup_book_id)
    return checkup_book 