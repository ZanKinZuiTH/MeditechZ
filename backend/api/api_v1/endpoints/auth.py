from datetime import timedelta
from typing import Any

from fastapi import APIRouter, Depends, HTTPException, status
from fastapi.security import OAuth2PasswordRequestForm
from sqlalchemy.orm import Session

from backend.core.config import settings
from backend.core.deps import get_db
from backend.core.security import create_access_token
from backend.schemas.token import Token
from backend.schemas.user import User
from backend.services.user import user_service

router = APIRouter()

@router.post("/login/access-token", response_model=Token)
def login_access_token(
    db: Session = Depends(get_db),
    form_data: OAuth2PasswordRequestForm = Depends(),
) -> Any:
    """
    OAuth2 compatible token login, รับ access token สำหรับการเข้าสู่ระบบในอนาคต
    """
    user = user_service.authenticate(
        db, email=form_data.username, password=form_data.password
    )
    if not user:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="อีเมลหรือรหัสผ่านไม่ถูกต้อง",
        )
    elif not user_service.is_active(user):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="บัญชีผู้ใช้ไม่ได้เปิดใช้งาน",
        )
    access_token_expires = timedelta(minutes=settings.ACCESS_TOKEN_EXPIRE_MINUTES)
    return {
        "access_token": create_access_token(
            subject=user.id, expires_delta=access_token_expires
        ),
        "token_type": "bearer",
    }

@router.post("/login/test-token", response_model=User)
def test_token(current_user: User = Depends(user_service.get_current_user)) -> Any:
    """
    ทดสอบ access token
    """
    return current_user

@router.post("/reset-password/{email}")
def reset_password(email: str, db: Session = Depends(get_db)) -> Any:
    """
    รีเซ็ตรหัสผ่าน
    """
    user = user_service.get_by_email(db, email=email)
    if not user:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบผู้ใช้ที่มีอีเมลนี้",
        )
    password_reset_token = user_service.generate_password_reset_token(email=email)
    # ส่งอีเมลพร้อม token สำหรับรีเซ็ตรหัสผ่าน (ในระบบจริงจะต้องส่งอีเมล)
    return {"msg": "รีเซ็ตรหัสผ่านสำเร็จ กรุณาตรวจสอบอีเมลของคุณ"}

@router.post("/reset-password-confirm/{token}")
def reset_password_confirm(
    token: str, new_password: str, db: Session = Depends(get_db)
) -> Any:
    """
    ยืนยันการรีเซ็ตรหัสผ่าน
    """
    email = user_service.verify_password_reset_token(token)
    if not email:
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="Token ไม่ถูกต้องหรือหมดอายุ",
        )
    user = user_service.get_by_email(db, email=email)
    if not user:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="ไม่พบผู้ใช้ที่มีอีเมลนี้",
        )
    elif not user_service.is_active(user):
        raise HTTPException(
            status_code=status.HTTP_400_BAD_REQUEST,
            detail="บัญชีผู้ใช้ไม่ได้เปิดใช้งาน",
        )
    user_service.update_password(db, user=user, password=new_password)
    return {"msg": "รหัสผ่านถูกเปลี่ยนเรียบร้อยแล้ว"} 