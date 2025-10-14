# MediTech HIS — ฟีเจอร์แก้ไขรายละเอียดการตรวจ X‑Ray (WPF + PACS/RIS)

ระบบแก้ไขรายละเอียดการตรวจ X‑Ray แบบ Production‑Grade พร้อม Audit เต็มรูปแบบ, ควบคุมสิทธิ์ตามบทบาท, UX ไทย‑first และสอดคล้องกับสถาปัตยกรรมเดิมของ MediTech

### ไฮไลท์
- แก้ไขฟิลด์สำคัญ: BodyPartsInStudy, StudyDescription, ModalitiesInStudy, PatientComments (ขยายเพิ่มได้)
- ตรวจจับการเปลี่ยนแปลงแบบเรียลไทม์ พร้อมสรุปความต่าง (diff) ภาษาไทยก่อนบันทึก
- บันทึก Audit ครบถ้วน (who/when/what/where/why) ด้วยตารางเฉพาะ + Stored Procedure และดัชนีสำหรับประสิทธิภาพ
- ควบคุมสิทธิ์ตามบทบาท (AdminRadiologist, Radiologist, RDUStaff, Admin) ทั้งฝั่งไคลเอนต์และเซิร์ฟเวอร์
- มาตรฐาน Bodypart (ออปชัน) ผ่านตาราง Mapping + Feature Flag
- ไทย‑first UX: ป้ายกำกับ/ข้อความเตือน/ยืนยันทั้งหมดเป็นภาษาไทย และสอดคล้องสไตล์ DevExpress เดิม

## โครงสร้างและสถาปัตยกรรม (สรุป)
- Desktop: WPF (.NET, MVVM via MvvmLight), DevExpress
- Web API: ASP.NET WebApi 2 (PACSWebApi)
- Database: SQL Server (สคีมา `dicom`)
- การไหลงาน: RIS → PACS WorkList → เปิดหน้าต่าง “แก้ไขรายละเอียด” → บันทึก → รีเฟรชรายการ + Audit

## ส่วนที่เพิ่ม/แก้ไข (โค้ดหลัก)
- Desktop (WPF)
  - View: `MediTech/Views/RIS/EditStudyDetails.xaml`
  - ViewModel: `MediTech/ViewModels/RIS/EditStudyDetailsViewModel.cs`
  - ปุ่ม/คำสั่งใน WorkList: `MediTech/Views/RIS/PACSWorkList.xaml`, `PACSWorkListViewModel.cs`
  - ผูก ViewModel: `MediTech/ViewModels/ViewModelLocator.cs`
  - ทรัพยากรตัวแปลง: `MediTech/App.xaml` (BooleanToVisibilityConverter)
- DataService
  - `MediTechData/MediTech.DataService/DataService/PACSService.cs` (Update + Audit History + Audit Report)
- WebApi (PACSWebApi)
  - Controller: `WebApi/PACSWebApi/Controllers/PACSController.cs`
    - POST `Api/PACS/UpdateStudyDetailsWithAudit`
    - GET `Api/PACS/GetStudyAuditHistory?studyInstanceUID=...`
    - GET `Api/PACS/GetAuditReport?from=...&to=...&userId?=...`
- Model (แชร์ระหว่างไคลเอนต์/เซิร์ฟเวอร์)
  - `MediTechData/MediTech.Model/PACS/StudyAuditModels.cs` (Request/Change/AuditEntry)

## ฐานข้อมูลและสคริปต์ (Migration)
- Audit
  - ตาราง + ดัชนี + SP: `Documentation/SQL/PACSStudyAuditLog.sql`
- Bodypart Standardization (ออปชัน)
  - ตาราง Mapping + ดัชนี + seed: `Documentation/SQL/BodypartMapping.sql`

รันสคริปต์ตามลำดับ: AuditLog → BodypartMapping (ถ้าจะเปิดใช้)

## การตั้งค่า (Configuration)
- WebApi `WebApi/PACSWebApi/Web.config`
  - `EnableBodypartStandardization` (true/false) — เปิด/ปิดการ map Bodypart
  - `StructuredLogPath` — เส้นทางเก็บ log (เตรียมไดเรกทอรีให้พร้อมสิทธิ์เขียน)
  - `DICOMPath`, `PACSEntities` — ตามสภาพแวดล้อม
- Desktop (WPF) `MediTech/MediTech/MediTech/App.config`
  - `PACSAddress` — URL ของ PACSWebApi

## วิธีใช้งาน (ผู้ใช้)
1) เปิดหน้า RIS → PACS WorkList
2) เลือก Study ที่ต้องการ → คลิก “แก้ไขรายละเอียด”
3) แก้ไขฟิลด์ที่ต้องการ → ตรวจสรุปการเปลี่ยนแปลง (ภาษาไทย)
4) กดยืนยัน “บันทึก” → ระบบ update + บันทึก Audit → รีเฟรชรายการ

## ความปลอดภัยและการตรวจสอบ
- Role‑based Access ทั้งฝั่ง VM และ WebApi (AdminRadiologist, Radiologist, RDUStaff, Admin)
- Validation ไทยทั้งฝั่งไคลเอนต์/เซิร์ฟเวอร์ (ความยาว/รูปแบบตามข้อจำกัดคอลัมน์จริง)
- Audit ครบ (ผู้แก้ไข/เวลา/ฟิลด์/ค่าเดิม/ค่าใหม่/IP/UserAgent/องค์กร)

## ตัวอย่างเรียก API
- ประวัติการแก้ไขของ Study
```bash
GET /Api/PACS/GetStudyAuditHistory?studyInstanceUID=<UID>
```
- รายงาน Audit ตามช่วงเวลาและ (ออปชัน) ผู้ใช้
```bash
GET /Api/PACS/GetAuditReport?from=2025-10-01&to=2025-10-14&userId=123
```

## เกณฑ์ยอมรับ (Acceptance)
- ประสิทธิภาพ: โหลดประวัติ 200 รายการ ≤ 300 ms (ด้วยดัชนี)
- UI ไม่ค้างระหว่างบันทึก (บันทึกแบบ async ที่ฝั่ง VM)
- การอัปเดตและ Audit เป็นธุรกรรมเดียวกัน (atomic)

## เอกสารเชิงลึก (ลิงก์)
- สรุปผู้บริหาร: [`PACS_Feature_Executive_Summary.md`](./PACS_Feature_Executive_Summary.md)
- แผนพัฒนาและรายละเอียด End‑to‑End: [`PACS_Xray_Detail_Edit_Feature_Development_Plan.md`](./PACS_Xray_Detail_Edit_Feature_Development_Plan.md)
- วิเคราะห์ปัญหา Bodypart: [`PACS_Bodypart_Issue_Analysis_Report.md`](./PACS_Bodypart_Issue_Analysis_Report.md)
- SQL Scripts: [`Documentation/SQL`](./Documentation/SQL)

## เส้นทางทดสอบ/UAT (สั้น)
- แก้ไข Bodypart/Description/Modality/Comments → บันทึก → ตรวจ Audit ล่าสุดต้องมีรายการใหม่ (1 ฟิลด์ = 1 แถว)
- ผู้ไม่มีสิทธิ์: ปุ่มปิด/บันทึกถูก block พร้อมข้อความไทย (HTTP 403 จาก WebApi)
- เปิดใช้มาตรฐาน Bodypart (flag=true): ค่าภายใน DB ควรถูก map เป็นค่า Standard ตามตาราง

---
หากพบปัญหา โปรดแนบ: StudyInstanceUID, เวลาเกิดเหตุ, บทบาทผู้ใช้, ข้อความ Error/HTTP code และผล SQL จากการตรวจ `dicom.PACSStudyAuditLog` เพื่อช่วยวิเคราะห์อย่างรวดเร็ว
