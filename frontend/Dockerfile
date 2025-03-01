FROM node:18-alpine

WORKDIR /app

# ติดตั้ง dependencies
COPY package.json package-lock.json ./
RUN npm ci

# คัดลอกโค้ด
COPY . .

# เปิดพอร์ต
EXPOSE 3000

# รัน development server
CMD ["npm", "run", "dev", "--", "--host", "0.0.0.0"] 