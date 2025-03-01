FROM python:3.10-slim

WORKDIR /app

# ติดตั้ง dependencies สำหรับ PostgreSQL และ Python packages
RUN apt-get update && apt-get install -y \
    build-essential \
    libpq-dev \
    && rm -rf /var/lib/apt/lists/*

# คัดลอกไฟล์ requirements
COPY requirements.txt .
COPY requirements-dev.txt .

# ติดตั้ง Python dependencies
RUN pip install --no-cache-dir -r requirements.txt
RUN pip install --no-cache-dir -r requirements-dev.txt

# คัดลอกโค้ด
COPY . .

# สร้างโฟลเดอร์สำหรับเก็บข้อมูล
RUN mkdir -p /app/storage
RUN mkdir -p /app/logs

# เปิดพอร์ต
EXPOSE 8000

# รัน FastAPI server
CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"] 