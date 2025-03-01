from typing import Dict, Any, Optional
from sqlalchemy import Column, Integer, String, DateTime, ForeignKey, Text, JSON, Boolean
from sqlalchemy.orm import relationship
from datetime import datetime

from backend.db.base_class import Base


class MedicalDocument(Base):
    """
    โมเดลสำหรับเอกสารทางการแพทย์ทั่วไป
    """
    __tablename__ = "medical_documents"

    id = Column(Integer, primary_key=True, index=True)
    patient_id = Column(Integer, ForeignKey("patients.id"), nullable=False)
    doctor_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    visit_id = Column(Integer, ForeignKey("visits.id"), nullable=True)
    document_date = Column(DateTime, default=datetime.now, nullable=False)
    document_type = Column(String(50), nullable=False)  # "medical_certificate", "health_checkup_book", "other"
    document_status = Column(String(20), default="draft", nullable=False)  # "draft", "completed", "signed", "cancelled"
    notes = Column(Text, nullable=True)
    
    # ข้อมูลการสร้างและแก้ไข
    created_at = Column(DateTime, default=datetime.now, nullable=False)
    updated_at = Column(DateTime, nullable=True)
    created_by_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    updated_by_id = Column(Integer, ForeignKey("users.id"), nullable=True)
    
    # ความสัมพันธ์
    patient = relationship("Patient", back_populates="medical_documents")
    doctor = relationship("User", foreign_keys=[doctor_id], back_populates="doctor_documents")
    created_by = relationship("User", foreign_keys=[created_by_id], back_populates="created_documents")
    updated_by = relationship("User", foreign_keys=[updated_by_id], back_populates="updated_documents")
    visit = relationship("Visit", back_populates="medical_documents")


class MedicalCertificate(Base):
    """
    โมเดลสำหรับใบรับรองแพทย์
    """
    __tablename__ = "medical_certificates"

    id = Column(Integer, primary_key=True, index=True)
    patient_id = Column(Integer, ForeignKey("patients.id"), nullable=False)
    doctor_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    visit_id = Column(Integer, ForeignKey("visits.id"), nullable=True)
    certificate_date = Column(DateTime, default=datetime.now, nullable=False)
    certificate_type = Column(String(50), default="general", nullable=False)  # "general", "confined_space", "radiology", "other"
    diagnosis = Column(Text, nullable=True)
    treatment = Column(Text, nullable=True)
    rest_period_days = Column(Integer, nullable=True)
    rest_period_start = Column(DateTime, nullable=True)
    rest_period_end = Column(DateTime, nullable=True)
    doctor_license_no = Column(String(50), nullable=True)
    doctor_license_issue_date = Column(DateTime, nullable=True)
    doctor2_id = Column(Integer, ForeignKey("users.id"), nullable=True)
    doctor2_license_no = Column(String(50), nullable=True)
    doctor2_license_issue_date = Column(DateTime, nullable=True)
    vital_signs = Column(JSON, nullable=True)
    comments = Column(Text, nullable=True)
    
    # ข้อมูลการสร้างและแก้ไข
    created_at = Column(DateTime, default=datetime.now, nullable=False)
    updated_at = Column(DateTime, nullable=True)
    created_by_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    updated_by_id = Column(Integer, ForeignKey("users.id"), nullable=True)
    
    # ความสัมพันธ์
    patient = relationship("Patient", back_populates="medical_certificates")
    doctor = relationship("User", foreign_keys=[doctor_id], back_populates="doctor_certificates")
    doctor2 = relationship("User", foreign_keys=[doctor2_id], back_populates="doctor2_certificates")
    created_by = relationship("User", foreign_keys=[created_by_id], back_populates="created_certificates")
    updated_by = relationship("User", foreign_keys=[updated_by_id], back_populates="updated_certificates")
    visit = relationship("Visit", back_populates="medical_certificates")


class HealthCheckupBook(Base):
    """
    โมเดลสำหรับสมุดตรวจสุขภาพ
    """
    __tablename__ = "health_checkup_books"

    id = Column(Integer, primary_key=True, index=True)
    patient_id = Column(Integer, ForeignKey("patients.id"), nullable=False)
    doctor_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    visit_id = Column(Integer, ForeignKey("visits.id"), nullable=True)
    checkup_date = Column(DateTime, default=datetime.now, nullable=False)
    checkup_type = Column(String(50), default="general", nullable=False)  # "general", "annual", "pre_employment", "other"
    vital_signs = Column(JSON, nullable=True)
    physical_exam = Column(JSON, nullable=True)
    lab_results = Column(JSON, nullable=True)
    radiology_results = Column(JSON, nullable=True)
    ekg_results = Column(JSON, nullable=True)
    spirometry_results = Column(JSON, nullable=True)
    audiometry_results = Column(JSON, nullable=True)
    vision_test_results = Column(JSON, nullable=True)
    conclusion = Column(Text, nullable=True)
    recommendations = Column(Text, nullable=True)
    doctor_license_no = Column(String(50), nullable=True)
    doctor_signature = Column(String(255), nullable=True)
    
    # ข้อมูลการสร้างและแก้ไข
    created_at = Column(DateTime, default=datetime.now, nullable=False)
    updated_at = Column(DateTime, nullable=True)
    created_by_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    updated_by_id = Column(Integer, ForeignKey("users.id"), nullable=True)
    
    # ความสัมพันธ์
    patient = relationship("Patient", back_populates="health_checkup_books")
    doctor = relationship("User", foreign_keys=[doctor_id], back_populates="doctor_checkup_books")
    created_by = relationship("User", foreign_keys=[created_by_id], back_populates="created_checkup_books")
    updated_by = relationship("User", foreign_keys=[updated_by_id], back_populates="updated_checkup_books")
    visit = relationship("Visit", back_populates="health_checkup_books") 