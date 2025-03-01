#!/usr/bin/env python
# -*- coding: utf-8 -*-

"""
สคริปต์สำหรับการสร้างฐานข้อมูลเริ่มต้นสำหรับโปรเจค MediTech_New
ผู้เขียน: เมษา AI
วันที่: 1 มีนาคม 2025
"""

import os
import sys
import logging
from pathlib import Path
import psycopg2
from psycopg2 import sql
from dotenv import load_dotenv

# ตั้งค่า logging
logging.basicConfig(
    level=logging.INFO,
    format="%(asctime)s - %(name)s - %(levelname)s - %(message)s",
    handlers=[logging.StreamHandler()],
)
logger = logging.getLogger("setup_db")

# โหลดค่าจากไฟล์ .env
def load_env_vars():
    """โหลดค่าจากไฟล์ .env"""
    # หาตำแหน่งของไฟล์ .env
    backend_dir = Path(__file__).resolve().parent.parent / "backend"
    env_file = backend_dir / ".env"
    
    if not env_file.exists():
        logger.error(f"ไม่พบไฟล์ .env ที่ {env_file}")
        return False
    
    load_dotenv(env_file)
    return True

# ฟังก์ชันสำหรับเชื่อมต่อกับ PostgreSQL
def connect_to_postgres(db_name=None):
    """เชื่อมต่อกับ PostgreSQL"""
    try:
        conn = psycopg2.connect(
            host=os.getenv("POSTGRES_SERVER", "localhost"),
            user=os.getenv("POSTGRES_USER", "postgres"),
            password=os.getenv("POSTGRES_PASSWORD", "postgres"),
            dbname=db_name if db_name else "postgres",
        )
        conn.autocommit = True
        return conn
    except Exception as e:
        logger.error(f"ไม่สามารถเชื่อมต่อกับ PostgreSQL ได้: {e}")
        return None

# ฟังก์ชันสำหรับสร้างฐานข้อมูล
def create_database():
    """สร้างฐานข้อมูล"""
    db_name = os.getenv("POSTGRES_DB", "meditech")
    
    # เชื่อมต่อกับ PostgreSQL
    conn = connect_to_postgres()
    if not conn:
        return False
    
    try:
        # ตรวจสอบว่าฐานข้อมูลมีอยู่แล้วหรือไม่
        cursor = conn.cursor()
        cursor.execute("SELECT 1 FROM pg_database WHERE datname = %s", (db_name,))
        exists = cursor.fetchone()
        
        if not exists:
            # สร้างฐานข้อมูล
            cursor.execute(sql.SQL("CREATE DATABASE {}").format(sql.Identifier(db_name)))
            logger.info(f"สร้างฐานข้อมูล {db_name} สำเร็จ")
        else:
            logger.info(f"ฐานข้อมูล {db_name} มีอยู่แล้ว")
        
        cursor.close()
        conn.close()
        return True
    except Exception as e:
        logger.error(f"เกิดข้อผิดพลาดในการสร้างฐานข้อมูล: {e}")
        if conn:
            conn.close()
        return False

# ฟังก์ชันสำหรับสร้างตาราง
def create_tables():
    """สร้างตารางในฐานข้อมูล"""
    db_name = os.getenv("POSTGRES_DB", "meditech")
    
    # เชื่อมต่อกับฐานข้อมูล
    conn = connect_to_postgres(db_name)
    if not conn:
        return False
    
    try:
        cursor = conn.cursor()
        
        # สร้างตาราง users
        cursor.execute("""
        CREATE TABLE IF NOT EXISTS users (
            id SERIAL PRIMARY KEY,
            email VARCHAR(255) UNIQUE NOT NULL,
            hashed_password VARCHAR(255) NOT NULL,
            full_name VARCHAR(255),
            is_active BOOLEAN DEFAULT TRUE,
            is_superuser BOOLEAN DEFAULT FALSE,
            created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
        )
        """)
        
        # สร้างตาราง patients
        cursor.execute("""
        CREATE TABLE IF NOT EXISTS patients (
            id SERIAL PRIMARY KEY,
            first_name VARCHAR(255) NOT NULL,
            last_name VARCHAR(255) NOT NULL,
            date_of_birth DATE,
            gender VARCHAR(10),
            blood_type VARCHAR(5),
            address TEXT,
            phone VARCHAR(20),
            email VARCHAR(255),
            emergency_contact VARCHAR(255),
            emergency_phone VARCHAR(20),
            medical_history TEXT,
            allergies TEXT,
            created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
            created_by_id INTEGER REFERENCES users(id),
            updated_by_id INTEGER REFERENCES users(id),
            deleted_at TIMESTAMP WITH TIME ZONE,
            deleted_by_id INTEGER REFERENCES users(id)
        )
        """)
        
        # สร้างตาราง appointments
        cursor.execute("""
        CREATE TABLE IF NOT EXISTS appointments (
            id SERIAL PRIMARY KEY,
            patient_id INTEGER REFERENCES patients(id) NOT NULL,
            doctor_id INTEGER REFERENCES users(id) NOT NULL,
            appointment_date DATE NOT NULL,
            start_time TIME NOT NULL,
            end_time TIME NOT NULL,
            status VARCHAR(20) DEFAULT 'scheduled',
            reason TEXT,
            notes TEXT,
            created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
            created_by_id INTEGER REFERENCES users(id),
            updated_by_id INTEGER REFERENCES users(id),
            cancelled_at TIMESTAMP WITH TIME ZONE,
            cancelled_by_id INTEGER REFERENCES users(id)
        )
        """)
        
        # สร้างตาราง medical_records
        cursor.execute("""
        CREATE TABLE IF NOT EXISTS medical_records (
            id SERIAL PRIMARY KEY,
            patient_id INTEGER REFERENCES patients(id) NOT NULL,
            doctor_id INTEGER REFERENCES users(id) NOT NULL,
            appointment_id INTEGER REFERENCES appointments(id),
            record_date DATE NOT NULL,
            symptoms TEXT,
            diagnosis TEXT,
            treatment TEXT,
            prescription TEXT,
            notes TEXT,
            follow_up_date DATE,
            created_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
            created_by_id INTEGER REFERENCES users(id),
            updated_by_id INTEGER REFERENCES users(id)
        )
        """)
        
        logger.info("สร้างตารางทั้งหมดสำเร็จ")
        
        cursor.close()
        conn.close()
        return True
    except Exception as e:
        logger.error(f"เกิดข้อผิดพลาดในการสร้างตาราง: {e}")
        if conn:
            conn.close()
        return False

# ฟังก์ชันสำหรับเพิ่มข้อมูลเริ่มต้น
def insert_initial_data():
    """เพิ่มข้อมูลเริ่มต้น"""
    from passlib.context import CryptContext
    
    pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")
    
    db_name = os.getenv("POSTGRES_DB", "meditech")
    
    # เชื่อมต่อกับฐานข้อมูล
    conn = connect_to_postgres(db_name)
    if not conn:
        return False
    
    try:
        cursor = conn.cursor()
        
        # ตรวจสอบว่ามี superuser อยู่แล้วหรือไม่
        cursor.execute("SELECT 1 FROM users WHERE is_superuser = TRUE")
        exists = cursor.fetchone()
        
        if not exists:
            # เพิ่ม superuser
            superuser_email = os.getenv("FIRST_SUPERUSER", "admin@meditech.com")
            superuser_password = os.getenv("FIRST_SUPERUSER_PASSWORD", "admin")
            hashed_password = pwd_context.hash(superuser_password)
            
            cursor.execute("""
            INSERT INTO users (email, hashed_password, full_name, is_active, is_superuser)
            VALUES (%s, %s, %s, %s, %s)
            """, (superuser_email, hashed_password, "Admin User", True, True))
            
            logger.info(f"เพิ่ม superuser {superuser_email} สำเร็จ")
        else:
            logger.info("มี superuser อยู่แล้ว")
        
        cursor.close()
        conn.close()
        return True
    except Exception as e:
        logger.error(f"เกิดข้อผิดพลาดในการเพิ่มข้อมูลเริ่มต้น: {e}")
        if conn:
            conn.close()
        return False

# ฟังก์ชันหลัก
def main():
    """ฟังก์ชันหลัก"""
    logger.info("เริ่มต้นการสร้างฐานข้อมูล...")
    
    # โหลดค่าจากไฟล์ .env
    if not load_env_vars():
        logger.error("ไม่สามารถโหลดค่าจากไฟล์ .env ได้")
        return 1
    
    # สร้างฐานข้อมูล
    if not create_database():
        logger.error("ไม่สามารถสร้างฐานข้อมูลได้")
        return 1
    
    # สร้างตาราง
    if not create_tables():
        logger.error("ไม่สามารถสร้างตารางได้")
        return 1
    
    # เพิ่มข้อมูลเริ่มต้น
    if not insert_initial_data():
        logger.error("ไม่สามารถเพิ่มข้อมูลเริ่มต้นได้")
        return 1
    
    logger.info("สร้างฐานข้อมูลสำเร็จ")
    return 0

if __name__ == "__main__":
    sys.exit(main()) 