# Import all the models, so that Base has them before being
# imported by Alembic
from backend.db.base_class import Base
from backend.models.user import User
from backend.models.patient import Patient
from backend.models.appointment import Appointment
from backend.models.medical_record import MedicalRecord 