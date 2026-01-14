# MediTech (HIS/PACS) — ระบบใช้งานจริง (Solo End‑to‑End)

> โปรเจกต์นี้ตั้งใจทำเพื่อ “พรีเซนต์บน GitHub” แบบ **ไม่เปิดเผยข้อมูลอ่อนไหว** (ข้อมูลผู้ป่วย, URL ภายใน, IP, credentials)


---


## Tech stack (สรุปแบบอ่านเร็ว)

- **Client**: WPF (.NET Framework 4.8), MVVM, DevExpress
- **Backend**: ASP.NET Web API (.NET Framework)
- **Data**: Entity Framework + Microsoft SQL Server
- **Reporting**: DevExpress/XtraReports

---

## ภาพรวมใน 30 วินาที

- **Solo owner 100% (ตั้งแต่เริ่ม → go‑live → ดูแลต่อเนื่อง)**: เก็บ requirement หน้างาน, ออกแบบระบบ/DB, พัฒนา, ทดสอบ, ส่งมอบ, แก้ปัญหา production, ทำเอกสาร และพัฒนาเพิ่มตามการใช้งานจริง
- **Production use**: ใช้งานจริงต่อเนื่องในงานหน้างาน (โรงพยาบาล/คลินิก/หน่วยออกตรวจ) เน้น workflow จริง + ความถูกต้องของข้อมูล + รายงานจำนวนมาก
- **สถาปัตยกรรมหลัก**: WPF (MVVM) → DataService → Web API → EF/SQL Server → Reports

---

## จุดเด่นเชิงวิศวกรรม (Why this matters)

- **Workflows หน้างานจริง**: ออกแบบ UX ให้สั้น/เร็ว/ลดความผิดพลาด (ลดการพิมพ์ซ้ำ ลดจุดหลุดของข้อมูล)
- **Data integrity เป็นเรื่องใหญ่**: คุมข้อมูลตั้งแต่ UI → API → DB (ตัวอย่าง: pregnancy flags มีทั้ง logic กันซ้ำ + DB constraint)
- **Report‑heavy system**: รองรับการพิมพ์/สรุปผลหลายรูปแบบ (ฟอร์ม/ขนาด/ภาษา) พร้อมเงื่อนไขธุรกิจจริง
- **Operational readiness**: มีเอกสารสรุป/รายงานเทคนิค/คู่มือทดสอบ ช่วยให้ส่งมอบและตรวจรับได้เร็ว

---

## Impact (ใส่ “ตัวเลขจริง” ได้ทันที)

> ปล. ตอนอัปขึ้น GitHub แนะนำใส่เป็นช่วง/ประมาณการเพื่อไม่เปิดเผยข้อมูลอ่อนไหว

- **ลดความผิดพลาดจาก workflow**: จาก ___ → ___ (เช่น ลดเคสแก้ไขย้อนหลัง/ลดการพิมพ์ซ้ำ)
- **ลดเวลาในขั้นตอนสำคัญ**: จาก ___ นาที/เคส → ___ นาที/เคส
- **ความครอบคลุมรายงาน**: ___ รูปแบบ/ฟอร์ม, ___ โมดูลหลัก
- **Uptime/การดูแลหลัง go‑live**: แก้ incident หน้างาน + ปรับปรุงต่อเนื่องจาก feedback

---

## สถาปัตยกรรม (ภาพรวม)

```mermaid
flowchart LR
  A[WPF Desktop (MVVM/DevExpress)\nViews • ViewModels • Reports] --> B[DataService (Client SDK)]
  B --> C[ASP.NET Web API]
  C --> D[Entity Framework]
  D --> E[(Microsoft SQL Server)]
  A --> F[Report Templates\n(XtraReports)]
  F --> E
```

---

## สถานะงานปัจจุบัน (ติดตามง่าย / อัปเดตได้เลย)

> ใช้เป็น checklist สำหรับทีม/HR/ผู้รีวิวให้เห็น “กำลังทำอะไรอยู่” ใน repo แบบอ่านครั้งเดียวรู้เรื่อง

- [x] ฟีเจอร์: **สถานะสงสัยตั้งครรภ์** (DB → UI → Report → API) + บังคับเลือกได้แค่ 1 สถานะ
- [x] DB Script: เพิ่มคอลัมน์ + CHECK constraint ป้องกันซ้ำซ้อน (`DatabaseScripts/`)
- [ ] เพิ่มภาพประกอบ (screenshots/flow) เพื่อเล่า workflow ให้ชัดขึ้น
- [ ] เพิ่มชุด Test Case เชิง regression (กรณีข้อมูลเก่าที่เป็น NULL/การแก้ไขย้อนหลัง/รายงาน)
- [ ] จัดทำ Release checklist (DB scripts / build artifacts / smoke test)

---

## ไฮไลต์ฟีเจอร์ (อ่านแล้ว “เห็นภาพ” ทันที)

### 1) ระบบบันทึกสถานะการตั้งครรภ์ (สงสัย/ยืนยัน)
- **เป้าหมาย**: ลดความเสี่ยงทางการแพทย์และความผิดพลาดใน workflow (โดยเฉพาะงานที่เกี่ยวกับ X‑ray/การแปลผล/รายงาน)
- **ครอบคลุมครบวงจร**: Database + UI + Business Logic + Reports + Web API
- **เอกสารประกอบ**:
  - Executive summary: `Documentation/feature-suspected-pregnancy/Executive_Summary_ระบบสงสัยการตั้งครรภ์.md`
  - รายงานสรุป: `Documentation/feature-suspected-pregnancy/รายงานสรุประบบสงสัยการตั้งครรภ์.md`
  - คู่มือทดสอบ: `Documentation/feature-suspected-pregnancy/คู่มือทดสอบระบบ_สงสัยการตั้งครรภ์.md`
  - DB Script: `DatabaseScripts/2026-01-14_AddPregnancyFlags_PatientVitalSign.sql`

<details>
<summary><b>Workflow แบบย่อ (กดดู)</b></summary>

- **บันทึกสถานะใน Vital Sign / Physical Exam** → ส่งผ่าน `DataService` → `WebApi` → บันทึกลง `PatientVitalSign`
- **แปลผล/รายงาน**: ใช้ค่าเดียวกันในการแสดงผลแทน BMI และสร้าง Comment เพื่อความปลอดภัยใน workflow
- **Data safety**: UI + API คุมไม่ให้เลือกซ้ำ, DB มี CHECK constraint กันข้อมูลเสีย

</details>

### 2) PACS / คุณภาพข้อมูล / งานวิเคราะห์ปัญหาหน้างาน
- Report: `Documentation/pacs/PACS_Bodypart_Issue_Analysis_Report.md`
- Plan/Proposal: `Documentation/pacs/PACS_Xray_Detail_Edit_Feature_Development_Plan.md`, `Documentation/pacs/PACS_Feature_Executive_Summary.md`

---

## Demo (แนะนำใส่รูป/คลิปสั้นเพื่อให้ “ขายงาน” ได้ที่สุด)

> ใส่ภาพได้โดยสร้างโฟลเดอร์ `Documentation/media/` แล้ววางไฟล์รูป เช่น `overview.png`, `workflow.png`

- Screenshot 1: หน้าจอ Vital Sign (checkbox สงสัย/ตั้งครรภ์)
- Screenshot 2: หน้าจอ Physical Exam (บันทึก + แปลผล)
- Screenshot 3: ตัวอย่างรายงานที่แสดงผล (แทนค่า BMI/Comment)

---

## โครงสร้าง repo (หาไฟล์ให้เจอใน 10 วินาที)

- `MediTech/` — WPF Desktop Client (MVVM/DevExpress), `Views/`, `ViewModels/`, `Reports/`
- `WebApi/` — ASP.NET Web API (`MediTechWebApi`, `PACSWebApi`)
- `MediTechData/` — `DataBase/` (EDMX/EF), `Model/`, `DataService/`, `ShareLibrary/`
- `MDWorkFlow/` — Dashboard/Tooling ติดตามงาน (HTML/JS)
- `DatabaseScripts/` — สคริปต์ DB สำหรับ deploy/ปรับ schema
- `Documentation/` — เอกสารทั้งหมด (จัดหมวดแล้ว)

---

## เอกสารหลัก (เริ่มอ่านจากตรงนี้)

- Case study/ภาพรวม: `Documentation/meditech-case-study.md`
- AI prompts/แนวทางเอกสาร: `Documentation/ai-prompts/`

---

## หมายเหตุด้านการ Build (สำหรับผู้รีวิว)

- โปรเจกต์นี้มีทั้ง WPF/COM reference และ WebApplication targets  
  **แนะนำให้ build ผ่าน Visual Studio ที่ลง workload ครบ** (มากกว่า `dotnet msbuild` บนเครื่องเปล่า)

<details>
<summary><b>Build prerequisites (กดดู)</b></summary>

- Visual Studio (แนะนำ 2019/2022) + workload:
  - .NET desktop development
  - ASP.NET and web development
- .NET Framework Developer Pack **4.8**
- ถ้าใช้ DevExpress: ต้องมี assemblies/license ตาม environment ขององค์กร

</details>
