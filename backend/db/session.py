from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker

from backend.core.config import settings

# สร้าง engine สำหรับเชื่อมต่อฐานข้อมูล
engine = create_engine(settings.SQLALCHEMY_DATABASE_URI, pool_pre_ping=True)

# สร้าง session factory
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)


# ฟังก์ชันสำหรับสร้าง database session
def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close() 