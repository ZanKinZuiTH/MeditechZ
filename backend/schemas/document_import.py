from pydantic import BaseModel, Field
from typing import Dict, List, Optional, Any, Union
from datetime import datetime

class ExtractedField(BaseModel):
    """
    ข้อมูลที่สกัดได้จากเอกสาร
    """
    value: str
    confidence: float = Field(..., ge=0.0, le=1.0, description="ระดับความมั่นใจในการสกัดข้อมูล (0.0-1.0)")

class ExtractedData(BaseModel):
    """
    ข้อมูลทั้งหมดที่สกัดได้จากเอกสาร
    """
    document_type: str = Field(..., description="ประเภทของเอกสาร")
    recognized_template: Optional[str] = Field(None, description="เทมเพลตเอกสารที่ตรวจพบ")
    confidence: float = Field(..., ge=0.0, le=1.0, description="ระดับความมั่นใจโดยรวม (0.0-1.0)")
    fields: Dict[str, ExtractedField] = Field(..., description="ข้อมูลที่สกัดได้จากเอกสาร")
    table_data: Optional[List[Dict[str, str]]] = Field(None, description="ข้อมูลตารางที่สกัดได้")
    raw_text: Optional[str] = Field(None, description="ข้อความดิบที่สกัดได้จากเอกสาร")

class DocumentImportResponse(BaseModel):
    """
    การตอบกลับจากการประมวลผลเอกสาร
    """
    success: bool
    message: str
    data: Optional[Dict[str, Any]] = None

class DocumentSaveRequest(BaseModel):
    """
    คำขอสำหรับบันทึกข้อมูลที่สกัดได้
    """
    document_type: str = Field(..., description="ประเภทของเอกสาร")
    data: Dict[str, Any] = Field(..., description="ข้อมูลที่สกัดได้จากเอกสาร")
    metadata: Optional[Dict[str, Any]] = Field(None, description="ข้อมูลเพิ่มเติม")

class DocumentSaveResponse(BaseModel):
    """
    การตอบกลับจากการบันทึกข้อมูล
    """
    success: bool
    message: str
    document_id: Optional[str] = None

class DocumentTemplate(BaseModel):
    """
    เทมเพลตเอกสาร
    """
    id: str
    name: str
    type: str
    description: Optional[str] = None
    created_at: Optional[datetime] = None
    updated_at: Optional[datetime] = None

class DocumentImportHistory(BaseModel):
    """
    ประวัติการนำเข้าเอกสาร
    """
    id: str
    user_id: str
    filename: str
    document_type: str
    processed_at: datetime
    status: str
    document_id: Optional[str] = None
    error_message: Optional[str] = None 