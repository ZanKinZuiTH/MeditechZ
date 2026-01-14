# MediTech (HIS/PACS) — Solo End-to-End Case Study

> TH first, EN below.  
> **Safety note:** This document intentionally avoids sensitive information (patient data, internal URLs, IPs, credentials).

---

## สารบัญ (TH)

- [1) ภาพรวม](#1-ภาพรวม)
- [2) สถานะการใช้งานจริง](#2-สถานะการใช้งานจริง)
- [3) สิ่งที่ผมทำในฐานะ Owner (Solo)](#3-สิ่งที่ผมทำในฐานะ-owner-solo)
- [4) สถาปัตยกรรมระบบ](#4-สถาปัตยกรรมระบบ)
- [5) จุดเด่นทางเทคนิค](#5-จุดเด่นทางเทคนิค)
- [6) ฟีเจอร์เด่น (Selected Highlights)](#6-ฟีเจอร์เด่น-selected-highlights)
- [7) Reliability / Operations](#7-reliability--operations)
- [8) หลักฐานและเอกสารประกอบใน repo](#8-หลักฐานและเอกสารประกอบใน-repo)
- [9) โครงสร้าง repo (สำหรับผู้อ่านโค้ด)](#9-โครงสร้าง-repo-สำหรับผู้อ่านโค้ด)
- [10) ช่องทางติดต่อ](#10-ช่องทางติดต่อ)

---

## 1) ภาพรวม

**MediTech** คือระบบด้าน Healthcare ที่ครอบคลุมงาน **HIS + RIS/PACS + Checkup/Reports** โดยโฟกัสการใช้งานจริงใน workflow หน้างาน (โรงพยาบาล/คลินิก/หน่วยออกตรวจ Mobile)

- **บทบาทของผม**: Owner / Architect / Developer / Maintainer (ทำคนเดียว end-to-end)
- **แนวทาง**: พัฒนาแบบ “Feature end-to-end” (DB → Model → UI → Reports → API) พร้อมเอกสารประกอบการตรวจรับ/ทดสอบ

---

## 2) สถานะการใช้งานจริง

- **Production use**: โรงพยาบาลหลัก + คลินิกย่อย + หน่วยออกตรวจ Mobile
- **การใช้งาน**: มีการใช้งานต่อเนื่องจำนวนมากตลอดวัน
- **ตัวเลขที่ควรใส่เพิ่ม (ถ้ามี)**:
  - จำนวนสาขา: [METRIC?]
  - จำนวนผู้ใช้งาน/แผนก: [METRIC?]
  - จำนวนรายงาน/ฟอร์มสำคัญ: [METRIC?]
  - จำนวนโมดูลหลัก/หน้าจอหลัก: [METRIC?]

---

## 3) สิ่งที่ผมทำในฐานะ Owner (Solo)

- ออกแบบสถาปัตยกรรมและโครงสร้างโปรเจกต์ (Desktop/Service/API/DB)
- พัฒนา Desktop Client (WPF/MVVM) และ UI ที่เหมาะกับ workflow หน้างาน
- พัฒนา Web API และ data layer (EF/DB) เพื่อให้ระบบอื่น/โมดูลอื่นเรียกใช้งานได้
- สร้าง/ดูแล Reports (report-heavy) และ logic เฉพาะทางด้านการแพทย์
- จัดทำเอกสารประกอบการส่งมอบ (Executive summary / รายงานสรุป / คู่มือทดสอบ)
- ดูแลระบบ production และสนับสนุนผู้ใช้งานจริง (Windows Server + Microsoft SQL Server)

---

## 4) สถาปัตยกรรมระบบ

ภาพรวมสถาปัตยกรรม (สรุป):

```mermaid
flowchart LR
  A[WPF Desktop Client\n(MVVM/DevExpress)] --> B[DataService Layer\n(HTTP Client)]
  B --> C[ASP.NET Web API\n(MediTechWebApi / PACSWebApi)]
  C --> D[Entity Framework 6 (EDMX)]
  D --> E[Microsoft SQL Server]
  A --> R[Reports\n(Report-heavy)]
  C --> R
```

**หมายเหตุ**  
- งานเกี่ยวกับ DICOM: ผมดูแลการใช้งานหน้างาน (ถ่าย/อ่าน/ลงข้อมูล)  
- การติดตั้ง/เซ็ตอัพ DICOM เริ่มต้นมีการจ้างผู้ให้บริการ/บริษัทภายนอก (vendor) เป็นผู้ดำเนินการ

---

## 5) จุดเด่นทางเทคนิค

- **Layered architecture** ชัดเจน: Client → Service → API → DB
- **MVVM บน WPF** ช่วยให้ UI/logic แยกบทบาท และพัฒนาหน้าจอจำนวนมากได้
- **Report-heavy delivery** รองรับงานเอกสารและรูปแบบรายงานหลากหลาย (checkup/ใบรับรอง/เอกสารเฉพาะทาง)
- **Healthcare context**: รองรับ workflow ที่มีข้อจำกัดและความละเอียดสูง (ข้อมูลสำคัญ, การตรวจสอบย้อนกลับ, การส่งมอบแบบไม่กระทบหน้างาน)

---

## 6) ฟีเจอร์เด่น (Selected Highlights)

> ทุกฟีเจอร์ด้านล่างเป็นตัวอย่าง “งาน cross-layer” ที่ทำให้ระบบมีความซับซ้อนสูง และเป็นจุดเด่นของโปรเจกต์นี้

### 6.1 ระบบบันทึกสถานะการตั้งครรภ์ (Suspected Pregnancy Status)

- เพิ่มความสามารถบันทึกสถานะ **“สงสัยตั้งครรภ์ / ตั้งครรภ์”** ในข้อมูล Vital Sign
- ครอบคลุมการพัฒนา **DB → EF Model → DTO/Model → ViewModel/UI → Translation logic → Reports → Web API**
- เพิ่มคู่มือทดสอบสำหรับผู้ใช้งานหน้างาน (UAT) และเอกสารสรุปผลการส่งมอบ

เอกสารอ้างอิง:
- `Executive_Summary_ระบบสงสัยการตั้งครรภ์.md`
- `รายงานสรุประบบสงสัยการตั้งครรภ์.md`
- `คู่มือทดสอบระบบ_สงสัยการตั้งครรภ์.md`

### 6.2 การยกระดับคุณภาพข้อมูล PACS (Bodypart Issue Analysis)

- วิเคราะห์ปัญหาข้อมูล Bodypart ใน PACS WorkList และแนวทางแก้ไขหลายระดับ (DB/Application/UI)
- เน้น validation/standardization/audit แนวคิดเพื่อให้ข้อมูลค้นหา/รายงานสอดคล้อง

เอกสารอ้างอิง:
- `PACS_Bodypart_Issue_Analysis_Report.md`

### 6.3 ฟีเจอร์แก้ไขรายละเอียด X-ray หน้างาน (X-ray Detail Edit)

- วางแผนฟีเจอร์เพื่อแก้ pain point หน้างาน: แก้ข้อมูลผิดพลาด, มี audit log, และกำหนด role-based

เอกสารอ้างอิง:
- `PACS_Feature_Executive_Summary.md`
- `PACS_Xray_Detail_Edit_Feature_Development_Plan.md`

---

## 7) Reliability / Operations

- ระบบใช้งานบน **Windows Server** และฐานข้อมูล **Microsoft SQL Server**
- แนวทางที่ควรใส่เพิ่มให้ครบ (ถ้าต้องการนำเสนอเชิง production):
  - Backup/Restore: [METRIC?]
  - Monitoring/Alerting: [METRIC?]
  - Deployment/Rollback approach: [METRIC?]
  - Incident response (SLA/วิธีรับมือ): [METRIC?]

---

## 8) หลักฐานและเอกสารประกอบใน repo

- สรุปผู้บริหาร/ภาพรวมฟีเจอร์: `Executive_Summary_ระบบสงสัยการตั้งครรภ์.md`, `PACS_Feature_Executive_Summary.md`
- รายงานสรุปการเปลี่ยนแปลง: `รายงานสรุประบบสงสัยการตั้งครรภ์.md`
- คู่มือทดสอบ/ตรวจรับ: `คู่มือทดสอบระบบ_สงสัยการตั้งครรภ์.md`
- แผนและการวิเคราะห์เชิงเทคนิค: `PACS_Xray_Detail_Edit_Feature_Development_Plan.md`, `PACS_Bodypart_Issue_Analysis_Report.md`

---

## 9) โครงสร้าง repo (สำหรับผู้อ่านโค้ด)

- `MediTech/` — WPF Desktop Client (Views/ViewModels/Reports)
- `WebApi/` — ASP.NET Web API (MediTechWebApi + PACSWebApi)
- `MediTechData/` — DataBase/Model/DataService/ShareLibrary
- `MDWorkFlow/` — Dashboard/Tooling ติดตามงาน (HTML/JS)

---

## 10) ช่องทางติดต่อ

- **Name**: Sorawit Pannngam (สรวิศ ปานงาม)
- **Email**: zankinzui@gmail.com
- **Phone**: 063-4217907
- **GitHub**: (omitted)

---
---

# MediTech (HIS/PACS) — Solo End-to-End Case Study (EN)

## Overview

**MediTech** is a Healthcare system covering **HIS + RIS/PACS + Checkup/Reports**, designed for real-world hospital workflows (main hospital, clinic branches, and mobile units).

- **My role**: Owner / Architect / Developer / Maintainer (solo end-to-end)
- **Approach**: deliver features end-to-end across layers (DB → Models → UI → Reports → API), backed by documentation and test guides

## Production Usage

- Running in production across **main hospital + clinic branches + mobile operations**
- Heavy continuous usage throughout the day
- Metrics to add (recommended):
  - Sites/branches: [METRIC?]
  - Active users/departments: [METRIC?]
  - Major reports/forms: [METRIC?]

## Architecture

```mermaid
flowchart LR
  A[WPF Desktop Client\n(MVVM/DevExpress)] --> B[DataService Layer\n(HTTP Client)]
  B --> C[ASP.NET Web API\n(MediTechWebApi / PACSWebApi)]
  C --> D[Entity Framework 6 (EDMX)]
  D --> E[Microsoft SQL Server]
  A --> R[Reports\n(Report-heavy)]
  C --> R
```

**DICOM note**: I support day-to-day DICOM operations on-site (capture/read/register). Initial DICOM installation/setup was handled by an external vendor.

## Selected Highlights

- **Suspected/Confirmed Pregnancy Status** (cross-layer: DB → UI → Reports → API)  
  - `Executive_Summary_ระบบสงสัยการตั้งครรภ์.md`  
  - `รายงานสรุประบบสงสัยการตั้งครรภ์.md`  
  - `คู่มือทดสอบระบบ_สงสัยการตั้งครรภ์.md`
- **PACS data quality (Bodypart issue analysis)**  
  - `PACS_Bodypart_Issue_Analysis_Report.md`
- **X-ray detail edit (plan/proposal)**  
  - `PACS_Feature_Executive_Summary.md`, `PACS_Xray_Detail_Edit_Feature_Development_Plan.md`

## Contact

- Email: zankinzui@gmail.com
- Phone: 063-4217907
- GitHub: (omitted)
