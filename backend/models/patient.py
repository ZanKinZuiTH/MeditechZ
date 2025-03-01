from sqlalchemy import Boolean, Column, Integer, String, DateTime, Date, ForeignKey, Text
from sqlalchemy.orm import relationship
from datetime import datetime

from backend.db.base_class import Base


class Patient(Base):
    __tablename__ = "patients"

    id = Column(Integer, primary_key=True, index=True)
    first_name = Column(String, index=True, nullable=False)
    last_name = Column(String, index=True, nullable=False)
    id_card_number = Column(String, unique=True, index=True, nullable=False)
    date_of_birth = Column(Date, nullable=False)
    gender = Column(String, nullable=False)
    blood_type = Column(String)
    address = Column(Text)
    phone_number = Column(String)
    email = Column(String)
    emergency_contact_name = Column(String)
    emergency_contact_phone = Column(String)
    allergies = Column(Text)
    chronic_diseases = Column(Text)
    is_active = Column(Boolean(), default=True)
    created_at = Column(DateTime, default=datetime.utcnow)
    updated_at = Column(DateTime, default=datetime.utcnow, onupdate=datetime.utcnow)
    
    # ความสัมพันธ์กับตารางอื่น
    appointments = relationship("Appointment", back_populates="patient")
    medical_records = relationship("MedicalRecord", back_populates="patient") 