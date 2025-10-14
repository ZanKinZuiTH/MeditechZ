# MediTech HIS — ฟีเจอร์แก้ไขรายละเอียดการตรวจ X‑Ray (WPF + PACS/RIS)

ระบบแก้ไขรายละเอียดการตรวจ X‑Ray แบบ Production‑Grade พร้อม Audit เต็มรูปแบบ, ควบคุมสิทธิ์ตามบทบาท, UX ไทย‑first และสอดคล้องกับสถาปัตยกรรมเดิมของ MediTech

### ไฮไลท์
- แก้ไขฟิลด์สำคัญ: BodyPartsInStudy, StudyDescription, ModalitiesInStudy, PatientComments (ขยายเพิ่มได้)
- ตรวจจับการเปลี่ยนแปลงแบบเรียลไทม์ พร้อมสรุปความต่าง (diff) ภาษาไทยก่อนบันทึก
- บันทึก Audit ครบถ้วน (who/when/what/where/why) ด้วยตารางเฉพาะ + Stored Procedure และดัชนีสำหรับประสิทธิภาพ
- ควบคุมสิทธิ์ตามบทบาท (AdminRadiologist, Radiologist, RDUStaff, Admin) ทั้งฝั่งไคลเอนต์และเซิร์ฟเวอร์
- มาตรฐาน Bodypart (ออปชัน) ผ่านตาราง Mapping + Feature Flag
- ไทย‑first UX: ป้ายกำกับ/ข้อความเตือน/ยืนยันทั้งหมดเป็นภาษาไทย และสอดคล้องสไตล์ DevExpress เดิม

## ความคืบหน้า (Progress)
อัปเดตล่าสุด: ปรับปรุงครบ End‑to‑End พร้อมสังเกตการณ์และเอกสารภาษาไทย
- UI ไทยและประสบการณ์ใช้งาน
  - สร้างหน้าต่าง “แก้ไขรายละเอียด” (DevExpress) + สรุป diff ไทย + ปุ่มรีเฟรชประวัติ + Modality แบบคอมโบบ็อกซ์ (CR/DX/CT/ES/MR/MG/OT/RF/US)
  - Debounce ตรวจจับการเปลี่ยนแปลง ลดภาระ UI และเพิ่มความลื่นไหล
- Service/WebApi
  - Endpoint: Update + Audit History + Audit Report (with transaction)
  - Validation สองชั้น (Client/Server) ข้อความไทยครบ และ RBAC ทั้งสองฝั่ง
  - Bodypart Standardization (ออปชัน) ผ่านตาราง Mapping + Feature Flag
- Database/SQL
  - ตาราง Audit พร้อมดัชนี, Stored Procedure แบบ idempotent, สคริปต์ Mapping (ออปชัน)
- Observability/Performance
  - Structured logging + Correlation ID (วิเคราะห์เหตุขัดข้องเร็วขึ้น) และ query audit เร็วด้วยดัชนี
- เอกสารไทยครบ
  - คู่มือผู้ใช้/Runbook Admin‑Ops/แผนพัฒนา และสรุปผู้บริหาร (ลิงก์ด้านล่าง)

## ความยาก/ผลกระทบเชิงเทคนิค (Complexity & Impact)
- Cross‑Layer Change: UI (WPF/DevExpress) ↔ ViewModel (MVVM) ↔ DataService ↔ WebApi ↔ SQL — ต้องคงสัญญาข้อมูลและ validation ให้สอดคล้องสองฝั่ง
- Atomic Update+Audit: ใช้ธุรกรรมรับประกันความถูกต้องของประวัติและข้อมูลจริง (no partial write)
- RBAC สองชั้น + Validation สองชั้น: ลดความเสี่ยงจาก misuse และข้อมูลผิดรูปแบบ
- Standardization Flag: ออกแบบให้เปิด/ปิดได้แบบ config‑driven ไม่ต้องแก้โค้ด
- Observability: เพิ่ม correlation ID + structured logs ช่วยลด MTTR และรองรับการตรวจสอบย้อนหลัง
- Performance: ดัชนีประวัติ audit, UI async/debounce ป้องกัน freeze, query ไม่มี N+1

## KPI เป้าหมาย (เสนอใช้วัดทีม Dev)
- Data Correctness & Auditability
  - 100% ของการแก้ไขมี audit entry ครบ (who/when/what/where/why)
  - 0 partial update ในธุรกรรม update+audit
- Security & Compliance
  - ≥ 99% ของ request ที่ไม่มีสิทธิ์ถูกปฏิเสธ (RBAC ทั้งสองฝั่ง)
  - ไม่มี PII รั่วใน log (ตรวจด้วย code review checklist)
- Performance & UX
  - โหลดประวัติ 200 รายการ ≤ 300 ms บนสภาพแวดล้อมเป้าหมาย
  - UI save asynchronous: ไม่เกิด UI freeze ที่สังเกตได้ (≤ 100 ms block)
- Operability
  - ลด MTTR เหตุขัดข้องที่เกี่ยวกับฟีเจอร์ ≥ 30% ด้วย correlation ID
  - เอกสารผู้ใช้/Runbook ครบและผ่านการทดสอบ UAT
- Delivery
  - Test coverage (unit/VM/UI/integration) ≥ เกณฑ์ทีม (เช่น 60–80%)
  - Lead time for change ลดลงเมื่อเทียบฟีเจอร์เทียบเคียง (ระบุใน Sprint Report)

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
 - คู่มือผู้ใช้ (ไทย): [`Documentation/UserGuide_XrayEdit_TH.md`](./Documentation/UserGuide_XrayEdit_TH.md)
 - Runbook Admin/Ops (ไทย): [`Documentation/AdminOps_Runbook_TH.md`](./Documentation/AdminOps_Runbook_TH.md)

## เส้นทางทดสอบ/UAT (สั้น)
- แก้ไข Bodypart/Description/Modality/Comments → บันทึก → ตรวจ Audit ล่าสุดต้องมีรายการใหม่ (1 ฟิลด์ = 1 แถว)
- ผู้ไม่มีสิทธิ์: ปุ่มปิด/บันทึกถูก block พร้อมข้อความไทย (HTTP 403 จาก WebApi)
- เปิดใช้มาตรฐาน Bodypart (flag=true): ค่าภายใน DB ควรถูก map เป็นค่า Standard ตามตาราง

---
หากพบปัญหา โปรดแนบ: StudyInstanceUID, เวลาเกิดเหตุ, บทบาทผู้ใช้, ข้อความ Error/HTTP code และผล SQL จากการตรวจ `dicom.PACSStudyAuditLog` เพื่อช่วยวิเคราะห์อย่างรวดเร็ว
