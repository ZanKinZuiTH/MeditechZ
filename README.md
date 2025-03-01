# MeditechZ

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
├── ai_models/              # โมเดล AI
│   ├── training/           # Training scripts
│   └── inference/          # Inference scripts
└── utils/                  # Utility scripts
```

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
- Docker (optional)

### การติดตั้ง
1. Clone repository
```bash
git clone https://github.com/ZanKinZuiTH/MeditechZ.git
cd MeditechZ
```

2. ติดตั้ง dependencies สำหรับ backend
```bash
cd backend
python -m venv venv
source venv/bin/activate  # หรือ venv\Scripts\activate บน Windows
pip install -r requirements.txt
```

3. ติดตั้ง dependencies สำหรับ frontend
```bash
cd frontend
npm install
```

4. ตั้งค่าฐานข้อมูล
```bash
cd database
python setup_db.py
```

### การรัน
1. รัน backend
```bash
cd backend
uvicorn main:app --reload
```

2. รัน frontend
```bash
cd frontend
npm run dev
```

## การพัฒนา

### การสร้าง API ใหม่
1. สร้าง model ใน `backend/models/`
2. สร้าง schema ใน `backend/schemas/`
3. สร้าง service ใน `backend/services/`
4. สร้าง API endpoint ใน `backend/api/`

### การสร้างหน้าใหม่
1. สร้าง component ใน `frontend/components/`
2. สร้าง page ใน `frontend/pages/`
3. เพิ่ม route ใน router

## การทดสอบ
```bash
# รัน unit tests
cd tests
pytest unit/

# รัน integration tests
pytest integration/

# รัน e2e tests
pytest e2e/
```

## การ Backup โปรเจคเดิม
โปรเจค MediTech เดิมได้ถูกสำรองไว้ใน `MediTech_Backup/` ซึ่งประกอบด้วย:
- `MediTech/`: Desktop Application (WPF)
- `WebApi/`: Web API (ASP.NET)
- `MediTechData/`: Data Layer (Entity Framework)
- `MediTech_HealthCheckup/`: ระบบตรวจสุขภาพ (FastAPI, PyQt6)

## ผู้พัฒนา
- ทีมพัฒนา MediTech

## ลิขสิทธิ์
© 2025 MeditechZ. All rights reserved. 