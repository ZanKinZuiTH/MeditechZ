# Runbook: Admin/Ops — X‑Ray Study Edit

## Deployment
1) สำรองฐานข้อมูล dicom
2) รันสคริปต์: `Documentation/SQL/PACSStudyAuditLog.sql`
3) (ออปชัน) รัน `Documentation/SQL/BodypartMapping.sql`
4) อัปเดต WebApi `Web.config`:
   - `EnableBodypartStandardization` (true/false)
   - `StructuredLogPath`
5) Deploy WebApi และ Desktop Client ตามขั้นตอนมาตรฐาน

## Verification
- ทดสอบแก้ไข Study หนึ่งรายการ → ตรวจ Audit เพิ่มใน `dicom.PACSStudyAuditLog`
- `GetStudyAuditHistory`/`GetAuditReport` ตอบสนองภายในเวลาที่เหมาะสม
- ตรวจไฟล์ log ในโฟลเดอร์ที่ตั้งค่าไว้

## Monitoring
- ตรวจสอบความหนาแน่นของ Audit Log, พิจารณา job archive ถ้าจำเป็น
- เฝ้าระวัง error จาก WebApi (ค้นหาใน structured log โดยใช้วันที่/Correlation ID)

## Rollback
- ปิดใช้ feature (UI), revert deploy, restore DB จาก backup
- ลบ/ปิดใช้งาน `EnableBodypartStandardization` หากพบ mapping ผิดพลาด
