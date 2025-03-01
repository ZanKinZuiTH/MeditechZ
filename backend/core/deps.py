from typing import Generator

from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from jose import jwt
from pydantic import ValidationError
from sqlalchemy.orm import Session

from backend.core.config import settings
from backend.core.security import ALGORITHM
from backend.db.session import SessionLocal
from backend.models.user import User
from backend.schemas.token import TokenPayload

reusable_oauth2 = OAuth2PasswordBearer(
    tokenUrl=f"{settings.API_V1_STR}/auth/login/access-token"
)

def get_db() -> Generator:
    """
    ฟังก์ชันสำหรับเชื่อมต่อกับฐานข้อมูล
    """
    try:
        db = SessionLocal()
        yield db
    finally:
        db.close()

def get_current_user(
    db: Session = Depends(get_db), token: str = Depends(reusable_oauth2)
) -> User:
    """
    ฟังก์ชันสำหรับดึงข้อมูลผู้ใช้ปัจจุบันจาก token
    """
    try:
        payload = jwt.decode(
            token, settings.SECRET_KEY, algorithms=[ALGORITHM]
        )
        token_data = TokenPayload(**payload)
    except (jwt.JWTError, ValidationError):
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="ไม่สามารถตรวจสอบข้อมูลได้",
        )
    user = db.query(User).filter(User.id == token_data.sub).first()
    if not user:
        raise HTTPException(status_code=status.HTTP_404_NOT_FOUND, detail="ไม่พบผู้ใช้นี้")
    return user

def get_current_active_user(
    current_user: User = Depends(get_current_user),
) -> User:
    """
    ฟังก์ชันสำหรับตรวจสอบว่าผู้ใช้ปัจจุบันเปิดใช้งานอยู่หรือไม่
    """
    if not current_user.is_active:
        raise HTTPException(status_code=status.HTTP_400_BAD_REQUEST, detail="บัญชีผู้ใช้ไม่ได้เปิดใช้งาน")
    return current_user

def get_current_active_superuser(
    current_user: User = Depends(get_current_user),
) -> User:
    """
    ฟังก์ชันสำหรับตรวจสอบว่าผู้ใช้ปัจจุบันเป็น superuser หรือไม่
    """
    if not current_user.is_superuser:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN, detail="ไม่มีสิทธิ์เพียงพอ"
        )
    return current_user 