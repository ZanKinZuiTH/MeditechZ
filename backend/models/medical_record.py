from sqlalchemy import Boolean, Column, Integer, String, DateTime, ForeignKey, Text, Float, JSON
from sqlalchemy.orm import relationship
from datetime import datetime

from backend.db.base_class import Base


class MedicalRecord(Base):
    __tablename__ = "medical_records"

    id = Column(Integer, primary_key=True, index=True)
    patient_id = Column(Integer, ForeignKey("patients.id"), nullable=False)
    doctor_id = Column(Integer, ForeignKey("users.id"), nullable=False)
    appointment_id = Column(Integer, ForeignKey("appointments.id"))
    record_date = Column(DateTime, default=datetime.utcnow, nullable=False)
    chief_complaint = Column(Text, nullable=False)
    diagnosis = Column(Text, nullable=False)
    treatment = Column(Text)
    prescription = Column(Text)
    lab_results = Column(JSON)
    vital_signs = Column(JSON)  # เก็บข้อมูล BP, HR, RR, Temp, SpO2, etc.
    notes = Column(Text)
    follow_up_date = Column(DateTime)
    created_at = Column(DateTime, default=datetime.utcnow)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # ความสัมพันธ์กับตารางอื่น
    patient = relationship("Patient", back_populates="medical_records")
    doctor = relationship("User", back_populates="medical_records")
    appointment = relationship("Appointment", back_populates="medical_record") 