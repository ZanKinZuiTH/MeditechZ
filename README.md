# MeditechZ
ระบบสารสนเทศทางการแพทย์สำหรับโรงพยาบาลและคลินิก

## ภาพรวมโครงการ
โครงการ MeditechZ เป็นการพัฒนาต่อยอดจากระบบ MediTech เดิม โดยใช้เทคโนโลยีสมัยใหม่ด้วย Python เป็นหลัก เพื่อปรับปรุงประสิทธิภาพและเพิ่มความสามารถของระบบสารสนเทศทางการแพทย์

## โครงสร้างโปรเจค

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
├── frontend/               # ส่วน Frontend ของระบบ (React/Vue)
│   ├── src/                # Source code
│   ├── public/             # Public assets
│   ├── components/         # Reusable components
│   ├── pages/              # Page components
│   ├── styles/             # CSS/SCSS files
│   ├── utils/              # Utility functions
│   ├── hooks/              # Custom hooks
│   └── context/            # Context providers
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

## โมดูลหลักของระบบ

### 1. การจัดการผู้ป่วย (Patient Management)
- ลงทะเบียนผู้ป่วยใหม่
- ค้นหาและแก้ไขข้อมูลผู้ป่วย
- ประวัติการรักษา
- การนัดหมาย

### 2. การจัดการเวชระเบียน (Medical Records)
- บันทึกการตรวจรักษา
- ประวัติการรักษา
- ผลการตรวจทางห้องปฏิบัติการ
- การสั่งยาและการจ่ายยา

### 3. ระบบนัดหมาย (Appointment System)
- การนัดหมายผู้ป่วย
- การแจ้งเตือนการนัดหมาย
- การจัดการตารางแพทย์

### 4. ระบบการเงิน (Financial System)
- การออกใบเสร็จ
- การเรียกเก็บเงิน
- รายงานทางการเงิน

### 5. ระบบเอกสารทางการแพทย์ (Medical Documents)
- การจัดการเอกสารทางการแพทย์ทั่วไป
- ใบรับรองแพทย์
- สมุดตรวจสุขภาพ
- การพิมพ์เอกสารและรายงาน

### 6. ระบบวินิจฉัยด้วย AI (AI Diagnosis)
- การวินิจฉัยโรคเบื้องต้นจากอาการ
- การวิเคราะห์ภาพทางการแพทย์
- การพยากรณ์ความเสี่ยงของโรค

## เทคโนโลยีที่ใช้

### Backend
- **FastAPI**: Web framework สำหรับสร้าง API
- **SQLAlchemy**: ORM สำหรับจัดการฐานข้อมูล
- **Pydantic**: Data validation
- **Alembic**: Database migrations
- **JWT**: Authentication

### Frontend
- **React/Vue**: JavaScript framework
- **TypeScript**: Type-safe JavaScript
- **Tailwind CSS**: Utility-first CSS framework
- **Axios**: HTTP client

### Database
- **PostgreSQL**: ฐานข้อมูลหลัก
- **Redis**: Caching

### AI/ML
- **PyTorch/TensorFlow**: สำหรับโมเดล AI
- **Hugging Face Transformers**: สำหรับ NLP
- **scikit-learn**: สำหรับ ML algorithms

### Testing
- **Pytest**: Unit testing
- **Playwright**: E2E testing

### DevOps
- **Docker**: Containerization
- **GitHub Actions**: CI/CD

## การติดตั้งและการใช้งาน

### ความต้องการของระบบ
- Python 3.9+
- Node.js 16+
- PostgreSQL 13+
- Docker (แนะนำ)

### การติดตั้ง
1. โคลนโปรเจค
   ```
   git clone https://github.com/yourusername/MeditechZ.git
   cd MeditechZ
   ```

2. ติดตั้ง Backend
   ```
   cd backend
   python -m venv venv
   source venv/bin/activate  # หรือ venv\Scripts\activate บน Windows
   pip install -r requirements.txt
   ```

3. ติดตั้ง Frontend
   ```
   cd frontend
   npm install
   ```

4. ตั้งค่าฐานข้อมูล
   ```
   cd backend
   alembic upgrade head
   ```

5. รันระบบ
   ```
   # Terminal 1 (Backend)
   cd backend
   uvicorn main:app --reload

   # Terminal 2 (Frontend)
   cd frontend
   npm run dev
   ```

## การพัฒนาและการมีส่วนร่วม
โปรดอ่านเอกสาร [CONTRIBUTING.md](CONTRIBUTING.md) สำหรับรายละเอียดเกี่ยวกับกระบวนการส่ง pull requests

## ลิขสิทธิ์
โปรเจคนี้อยู่ภายใต้ลิขสิทธิ์ [MIT License](LICENSE) 