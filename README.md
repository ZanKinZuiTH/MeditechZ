# <img src="https://raw.githubusercontent.com/ZanKinZuiTH/MeditechZ/master/frontend/public/logo.png" alt="MeditechZ Logo" width="40" height="40" style="vertical-align: middle;"> MeditechZ

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![FastAPI](https://img.shields.io/badge/FastAPI-0.95.1-009688.svg?style=flat&logo=fastapi)](https://fastapi.tiangolo.com/)
[![React](https://img.shields.io/badge/React-18.2.0-61DAFB.svg?style=flat&logo=react)](https://reactjs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-4.9.5-3178C6.svg?style=flat&logo=typescript)](https://www.typescriptlang.org/)
[![Python](https://img.shields.io/badge/Python-3.9+-3776AB.svg?style=flat&logo=python)](https://www.python.org/)
[![TailwindCSS](https://img.shields.io/badge/TailwindCSS-3.3.2-38B2AC.svg?style=flat&logo=tailwind-css)](https://tailwindcss.com/)

<p align="center">
  <img src="https://raw.githubusercontent.com/ZanKinZuiTH/MeditechZ/master/frontend/public/dashboard-preview.png" alt="MeditechZ Dashboard" width="80%">
</p>

> **ระบบสารสนเทศทางการแพทย์ที่ทันสมัยสำหรับโรงพยาบาลและคลินิก** - ยกระดับการดูแลผู้ป่วยด้วยเทคโนโลยีล้ำสมัย

## 📋 สารบัญ

- [ภาพรวมโครงการ](#-ภาพรวมโครงการ)
- [คุณสมบัติหลัก](#-คุณสมบัติหลัก)
- [โมดูลของระบบ](#-โมดูลของระบบ)
- [เทคโนโลยีที่ใช้](#-เทคโนโลยีที่ใช้)
- [โครงสร้างโปรเจค](#-โครงสร้างโปรเจค)
- [การติดตั้งและการใช้งาน](#-การติดตั้งและการใช้งาน)
- [การพัฒนาและการมีส่วนร่วม](#-การพัฒนาและการมีส่วนร่วม)
- [ภาพหน้าจอ](#-ภาพหน้าจอ)
- [ทีมพัฒนา](#-ทีมพัฒนา)
- [ลิขสิทธิ์](#-ลิขสิทธิ์)

## 🚀 ภาพรวมโครงการ

MeditechZ เป็นระบบสารสนเทศทางการแพทย์รุ่นใหม่ที่พัฒนาต่อยอดจากระบบ MediTech เดิม โดยใช้เทคโนโลยีสมัยใหม่ด้วย Python, FastAPI, React และ TypeScript เป็นหลัก เพื่อปรับปรุงประสิทธิภาพและเพิ่มความสามารถของระบบให้ตอบโจทย์ความต้องการของสถานพยาบาลในยุคดิจิทัล

ระบบถูกออกแบบให้ใช้งานง่าย มีความยืดหยุ่นสูง และรองรับการขยายตัวในอนาคต พร้อมด้วยฟีเจอร์การวิเคราะห์ข้อมูลด้วย AI เพื่อช่วยในการวินิจฉัยและการตัดสินใจทางการแพทย์

## ✨ คุณสมบัติหลัก

- **ออกแบบตามหลัก UX/UI ที่ทันสมัย** - อินเตอร์เฟซที่ใช้งานง่าย สวยงาม และตอบสนองรวดเร็ว
- **ระบบความปลอดภัยระดับสูง** - การเข้ารหัสข้อมูล, การยืนยันตัวตนหลายขั้นตอน, และการจัดการสิทธิ์ที่ละเอียด
- **รองรับการทำงานบนอุปกรณ์ทุกประเภท** - Responsive design ที่ทำงานได้ดีทั้งบน Desktop, Tablet และ Mobile
- **การวิเคราะห์ข้อมูลด้วย AI** - ช่วยในการวินิจฉัยโรค, วิเคราะห์ภาพทางการแพทย์, และพยากรณ์ความเสี่ยง
- **การทำงานแบบ Offline-first** - สามารถทำงานได้แม้ในสภาวะที่การเชื่อมต่ออินเทอร์เน็ตไม่เสถียร
- **การบูรณาการกับระบบอื่น** - รองรับการเชื่อมต่อกับระบบ HIS, LIS, PACS และอื่นๆ ผ่าน API
- **รายงานและการวิเคราะห์ข้อมูล** - สร้างรายงานที่ครอบคลุมและมีประสิทธิภาพสำหรับการตัดสินใจ

## 📊 โมดูลของระบบ

### 1. การจัดการผู้ป่วย (Patient Management)
- **ลงทะเบียนผู้ป่วยใหม่** - บันทึกข้อมูลส่วนตัว, ประวัติการแพ้ยา, และข้อมูลประกัน
- **ค้นหาและแก้ไขข้อมูลผู้ป่วย** - ค้นหาด่วนด้วยชื่อ, เลขบัตรประชาชน, หรือรหัสผู้ป่วย
- **ประวัติการรักษา** - ดูประวัติการรักษาทั้งหมดแบบไทม์ไลน์
- **การนัดหมาย** - จัดการนัดหมายและส่งการแจ้งเตือนอัตโนมัติ

### 2. การจัดการเวชระเบียน (Medical Records)
- **บันทึกการตรวจรักษา** - บันทึกอาการ, การวินิจฉัย, และแผนการรักษา
- **ประวัติการรักษา** - ดูประวัติการรักษาทั้งหมดแบบไทม์ไลน์
- **ผลการตรวจทางห้องปฏิบัติการ** - บันทึกและดูผลตรวจทางห้องปฏิบัติการ
- **การสั่งยาและการจ่ายยา** - ระบบสั่งยาอัจฉริยะพร้อมตรวจสอบการแพ้ยาและปฏิกิริยาระหว่างยา

### 3. ระบบนัดหมาย (Appointment System)
- **การนัดหมายผู้ป่วย** - จองและจัดการนัดหมายแบบอินเทอร์แอคทีฟ
- **การแจ้งเตือนการนัดหมาย** - ส่ง SMS, อีเมล, หรือแจ้งเตือนผ่านแอพ
- **การจัดการตารางแพทย์** - จัดการตารางเวลาของแพทย์และทรัพยากรอื่นๆ

### 4. ระบบการเงิน (Financial System)
- **การออกใบเสร็จ** - ออกใบเสร็จและใบกำกับภาษีอิเล็กทรอนิกส์
- **การเรียกเก็บเงิน** - จัดการการเรียกเก็บเงินจากประกันและผู้ป่วย
- **รายงานทางการเงิน** - สร้างรายงานรายได้, ค่าใช้จ่าย, และการวิเคราะห์ทางการเงิน

### 5. ระบบเอกสารทางการแพทย์ (Medical Documents)
- **การจัดการเอกสารทางการแพทย์ทั่วไป** - สร้าง, แก้ไข, และจัดการเอกสารทางการแพทย์
- **ใบรับรองแพทย์** - ออกใบรับรองแพทย์หลากหลายรูปแบบ
- **สมุดตรวจสุขภาพ** - บันทึกและติดตามผลการตรวจสุขภาพ
- **การพิมพ์เอกสารและรายงาน** - พิมพ์เอกสารและรายงานในรูปแบบต่างๆ

### 6. ระบบวินิจฉัยด้วย AI (AI Diagnosis)
- **การวินิจฉัยโรคเบื้องต้นจากอาการ** - ใช้ AI ช่วยวินิจฉัยโรคจากอาการที่บันทึก
- **การวิเคราะห์ภาพทางการแพทย์** - วิเคราะห์ภาพ X-ray, CT, MRI ด้วย AI
- **การพยากรณ์ความเสี่ยงของโรค** - ประเมินความเสี่ยงของโรคจากข้อมูลผู้ป่วย

## 💻 เทคโนโลยีที่ใช้

<p align="center">
  <img src="https://raw.githubusercontent.com/ZanKinZuiTH/MeditechZ/master/docs/tech-stack.png" alt="Tech Stack" width="70%">
</p>

### Backend
- **FastAPI** - Web framework ที่เร็วที่สุดสำหรับสร้าง API ด้วย Python
- **SQLAlchemy** - ORM ที่ทรงพลังสำหรับจัดการฐานข้อมูล
- **Pydantic** - Data validation และ settings management
- **Alembic** - Database migrations ที่ยืดหยุ่น
- **JWT** - การยืนยันตัวตนที่ปลอดภัย

### Frontend
- **React** - JavaScript library สำหรับสร้าง UI ที่ตอบสนองรวดเร็ว
- **TypeScript** - JavaScript ที่มีการตรวจสอบประเภทข้อมูล
- **Tailwind CSS** - Utility-first CSS framework ที่ปรับแต่งได้สูง
- **DaisyUI** - คอมโพเนนต์ที่สวยงามสำหรับ Tailwind CSS
- **Axios** - HTTP client ที่ใช้งานง่าย

### Database
- **PostgreSQL** - ฐานข้อมูลเชิงสัมพันธ์ที่ทรงพลังและน่าเชื่อถือ
- **Redis** - In-memory data store สำหรับ caching และ session management

### AI/ML
- **PyTorch/TensorFlow** - Framework สำหรับสร้างและฝึกฝนโมเดล AI
- **Hugging Face Transformers** - Library สำหรับ NLP ที่ทันสมัย
- **scikit-learn** - Library สำหรับ machine learning ที่ใช้งานง่าย

### Testing
- **Pytest** - Framework สำหรับทดสอบ Python ที่ยืดหยุ่น
- **Playwright** - Library สำหรับทดสอบ end-to-end ที่ทันสมัย

### DevOps
- **Docker** - Containerization สำหรับการพัฒนาและการ deploy
- **GitHub Actions** - CI/CD ที่ทำงานร่วมกับ GitHub ได้อย่างราบรื่น

## 📁 โครงสร้างโปรเจค

```
MeditechZ/
├── backend/                # ส่วน Backend ของระบบ (FastAPI)
│   ├── api/                # API Endpoints
│   ├── core/               # Core functionality
│   ├── models/             # Data models
│   ├── schemas/            # Pydantic schemas
│   ├── services/           # Business logic
│   ├── auth/               # Authentication
│   ├── config/             # Configuration
│   └── db/                 # Database connections
├── frontend/               # ส่วน Frontend ของระบบ (React)
│   ├── src/                # Source code
│   │   ├── components/     # Reusable components
│   │   ├── features/       # Feature modules
│   │   ├── pages/          # Page components
│   │   ├── hooks/          # Custom hooks
│   │   ├── context/        # Context providers
│   │   ├── lib/            # Utility functions
│   │   └── styles/         # CSS/SCSS files
│   └── public/             # Public assets
├── docs/                   # เอกสารโครงการ
├── tests/                  # ชุดทดสอบ
│   ├── unit/               # Unit tests
│   ├── integration/        # Integration tests
│   └── e2e/                # End-to-end tests
├── scripts/                # Scripts ต่างๆ
├── database/               # ส่วนจัดการฐานข้อมูล
│   ├── migrations/         # Database migrations
│   └── scripts/            # Database scripts
├── ai_models/              # โมเดล AI สำหรับการวินิจฉัยและวิเคราะห์
│   ├── training/           # โค้ดสำหรับการเทรนโมเดล
│   ├── inference/          # โค้ดสำหรับการใช้งานโมเดล
│   └── models/             # โมเดลที่เทรนแล้ว
└── utils/                  # Utility scripts
```

## 🔧 การติดตั้งและการใช้งาน

### ความต้องการของระบบ
- Python 3.9+
- Node.js 16+
- PostgreSQL 13+
- Docker (แนะนำ)

### การติดตั้งด้วย Docker (แนะนำ)
```bash
# โคลนโปรเจค
git clone https://github.com/ZanKinZuiTH/MeditechZ.git
cd MeditechZ

# สร้างและรันคอนเทนเนอร์
docker-compose up -d
```

### การติดตั้งแบบ Manual
1. โคลนโปรเจค
   ```bash
   git clone https://github.com/ZanKinZuiTH/MeditechZ.git
   cd MeditechZ
   ```

2. ติดตั้ง Backend
   ```bash
   cd backend
   python -m venv venv
   source venv/bin/activate  # หรือ venv\Scripts\activate บน Windows
   pip install -r requirements.txt
   ```

3. ตั้งค่าไฟล์ .env
   ```bash
   cp .env.example .env
   # แก้ไขไฟล์ .env ตามการตั้งค่าของคุณ
   ```

4. ติดตั้ง Frontend
   ```bash
   cd frontend
   npm install
   ```

5. ตั้งค่าฐานข้อมูล
   ```bash
   cd backend
   alembic upgrade head
   ```

6. รันระบบ
   ```bash
   # Terminal 1 (Backend)
   cd backend
   uvicorn main:app --reload --host 0.0.0.0 --port 8000

   # Terminal 2 (Frontend)
   cd frontend
   npm run dev
   ```

7. เข้าถึงระบบ
   - Frontend: http://localhost:5173
   - API Documentation: http://localhost:8000/docs

## 🤝 การพัฒนาและการมีส่วนร่วม

เรายินดีต้อนรับการมีส่วนร่วมจากชุมชน! หากคุณสนใจที่จะมีส่วนร่วมในการพัฒนา MeditechZ โปรดอ่านเอกสาร [CONTRIBUTING.md](CONTRIBUTING.md) สำหรับรายละเอียดเกี่ยวกับกระบวนการส่ง pull requests และแนวทางการพัฒนา

### การรายงานปัญหา
หากคุณพบปัญหาหรือมีข้อเสนอแนะ โปรดสร้าง issue ใหม่ใน [GitHub Issues](https://github.com/ZanKinZuiTH/MeditechZ/issues)

### การพัฒนา
1. Fork โปรเจค
2. สร้าง branch ใหม่ (`git checkout -b feature/amazing-feature`)
3. Commit การเปลี่ยนแปลงของคุณ (`git commit -m 'Add some amazing feature'`)
4. Push ไปยัง branch (`git push origin feature/amazing-feature`)
5. เปิด Pull Request

## 📸 ภาพหน้าจอ

<p align="center">
  <img src="https://raw.githubusercontent.com/ZanKinZuiTH/MeditechZ/master/docs/screenshots/patient-management.png" alt="Patient Management" width="45%">
  <img src="https://raw.githubusercontent.com/ZanKinZuiTH/MeditechZ/master/docs/screenshots/medical-records.png" alt="Medical Records" width="45%">
</p>

<p align="center">
  <img src="https://raw.githubusercontent.com/ZanKinZuiTH/MeditechZ/master/docs/screenshots/appointment.png" alt="Appointment System" width="45%">
  <img src="https://raw.githubusercontent.com/ZanKinZuiTH/MeditechZ/master/docs/screenshots/ai-diagnosis.png" alt="AI Diagnosis" width="45%">
</p>

## 👥 ทีมพัฒนา

- **ทีมพัฒนา MeditechZ** - [GitHub Organization](https://github.com/ZanKinZuiTH)

## 📄 ลิขสิทธิ์

โปรเจคนี้อยู่ภายใต้ลิขสิทธิ์ [MIT License](LICENSE) - ดูรายละเอียดเพิ่มเติมได้ที่ไฟล์ LICENSE 