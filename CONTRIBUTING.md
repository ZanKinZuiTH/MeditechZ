# แนวทางการมีส่วนร่วมในการพัฒนา MeditechZ

ขอบคุณที่สนใจมีส่วนร่วมในการพัฒนา MeditechZ! เราเชื่อว่าการมีส่วนร่วมจากชุมชนจะช่วยให้โปรเจคนี้พัฒนาได้อย่างยั่งยืนและตอบโจทย์ความต้องการของผู้ใช้ได้ดียิ่งขึ้น

## สารบัญ

- [จรรยาบรรณ](#จรรยาบรรณ)
- [ฉันจะช่วยอะไรได้บ้าง?](#ฉันจะช่วยอะไรได้บ้าง)
- [การรายงานบัก](#การรายงานบัก)
- [การเสนอฟีเจอร์ใหม่](#การเสนอฟีเจอร์ใหม่)
- [กระบวนการพัฒนา](#กระบวนการพัฒนา)
- [แนวทางการเขียนโค้ด](#แนวทางการเขียนโค้ด)
- [การทดสอบ](#การทดสอบ)
- [การส่ง Pull Request](#การส่ง-pull-request)

## จรรยาบรรณ

โปรเจคนี้และทุกคนที่มีส่วนร่วมจะต้องปฏิบัติตามจรรยาบรรณของเรา ซึ่งสามารถอ่านได้ที่ [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md) โปรดรายงานพฤติกรรมที่ไม่เหมาะสมไปที่ [contact@meditechz.com](mailto:contact@meditechz.com)

## ฉันจะช่วยอะไรได้บ้าง?

มีหลายวิธีที่คุณสามารถมีส่วนร่วมในการพัฒนา MeditechZ:

- **รายงานบัก**: หากคุณพบปัญหาในการใช้งาน MeditechZ โปรดรายงานผ่าน [GitHub Issues](https://github.com/ZanKinZuiTH/MeditechZ/issues)
- **เสนอฟีเจอร์ใหม่**: หากคุณมีไอเดียสำหรับฟีเจอร์ใหม่ โปรดเสนอผ่าน [GitHub Issues](https://github.com/ZanKinZuiTH/MeditechZ/issues)
- **แก้ไขเอกสาร**: ช่วยปรับปรุงเอกสารให้ชัดเจนและครอบคลุมมากขึ้น
- **แก้ไขบัก**: ช่วยแก้ไขบักที่มีอยู่ใน [GitHub Issues](https://github.com/ZanKinZuiTH/MeditechZ/issues)
- **พัฒนาฟีเจอร์ใหม่**: ช่วยพัฒนาฟีเจอร์ใหม่ที่อยู่ใน roadmap หรือที่มีการเสนอไว้
- **แบ่งปันความคิดเห็น**: ให้ความคิดเห็นในประเด็นที่มีการอภิปราย

## การรายงานบัก

การรายงานบักที่ดีช่วยให้เราสามารถแก้ไขปัญหาได้อย่างรวดเร็ว โปรดปฏิบัติตามแนวทางต่อไปนี้:

1. **ตรวจสอบว่าบักยังไม่ได้รับการรายงาน**: ค้นหาใน [GitHub Issues](https://github.com/ZanKinZuiTH/MeditechZ/issues) ก่อนสร้าง issue ใหม่
2. **ใช้เทมเพลต**: เมื่อสร้าง issue ใหม่ โปรดใช้เทมเพลตที่เรากำหนดไว้
3. **ให้ข้อมูลที่จำเป็น**: ระบุขั้นตอนในการทำซ้ำบัก, พฤติกรรมที่คาดหวัง, พฤติกรรมที่เกิดขึ้นจริง, และสภาพแวดล้อมที่ใช้ (เช่น OS, เบราว์เซอร์, เวอร์ชัน)
4. **แนบภาพหน้าจอหรือวิดีโอ**: หากเป็นไปได้ แนบภาพหน้าจอหรือวิดีโอที่แสดงปัญหา
5. **แนบ log**: หากมี error log ที่เกี่ยวข้อง โปรดแนบมาด้วย

## การเสนอฟีเจอร์ใหม่

เรายินดีรับฟังไอเดียสำหรับฟีเจอร์ใหม่ โปรดปฏิบัติตามแนวทางต่อไปนี้:

1. **ตรวจสอบว่าฟีเจอร์ยังไม่ได้รับการเสนอ**: ค้นหาใน [GitHub Issues](https://github.com/ZanKinZuiTH/MeditechZ/issues) ก่อนสร้าง issue ใหม่
2. **ใช้เทมเพลต**: เมื่อสร้าง issue ใหม่ โปรดใช้เทมเพลตสำหรับการเสนอฟีเจอร์
3. **อธิบายปัญหาที่ฟีเจอร์นี้จะแก้ไข**: อธิบายว่าทำไมฟีเจอร์นี้จึงมีประโยชน์
4. **อธิบายวิธีการทำงานที่คุณคาดหวัง**: อธิบายว่าคุณคาดหวังให้ฟีเจอร์นี้ทำงานอย่างไร
5. **เสนอแนวทางการพัฒนา**: หากคุณมีแนวคิดเกี่ยวกับวิธีการพัฒนาฟีเจอร์นี้ โปรดแบ่งปัน

## กระบวนการพัฒนา

เราใช้ [GitHub Flow](https://guides.github.com/introduction/flow/) เป็นกระบวนการพัฒนา:

1. **Fork โปรเจค**: สร้าง fork ของโปรเจคไปยัง GitHub account ของคุณ
2. **Clone โปรเจค**: Clone fork ของคุณมายังเครื่องของคุณ
3. **สร้าง branch**: สร้าง branch ใหม่สำหรับการเปลี่ยนแปลงของคุณ
   ```bash
   git checkout -b feature/amazing-feature
   ```
4. **พัฒนา**: ทำการเปลี่ยนแปลงในโค้ด
5. **Commit**: Commit การเปลี่ยนแปลงของคุณ
   ```bash
   git commit -m "Add some amazing feature"
   ```
6. **Push**: Push การเปลี่ยนแปลงไปยัง fork ของคุณ
   ```bash
   git push origin feature/amazing-feature
   ```
7. **สร้าง Pull Request**: สร้าง Pull Request จาก branch ของคุณไปยัง `master` branch ของโปรเจคหลัก

## แนวทางการเขียนโค้ด

เพื่อให้โค้ดของเรามีความสอดคล้องกัน โปรดปฏิบัติตามแนวทางต่อไปนี้:

### Python (Backend)

- ปฏิบัติตาม [PEP 8](https://www.python.org/dev/peps/pep-0008/)
- ใช้ [Black](https://github.com/psf/black) สำหรับการจัดรูปแบบโค้ด
- ใช้ [isort](https://github.com/PyCQA/isort) สำหรับการจัดเรียง import
- เขียน docstring ในรูปแบบ [Google style](https://google.github.io/styleguide/pyguide.html#38-comments-and-docstrings)
- ใช้ type hints

### TypeScript/JavaScript (Frontend)

- ปฏิบัติตาม [Airbnb JavaScript Style Guide](https://github.com/airbnb/javascript)
- ใช้ [ESLint](https://eslint.org/) และ [Prettier](https://prettier.io/) สำหรับการตรวจสอบและจัดรูปแบบโค้ด
- ใช้ TypeScript interfaces สำหรับ props และ state
- ใช้ functional components และ hooks แทน class components
- จัดโครงสร้างโค้ดตาม feature-based architecture

## การทดสอบ

การทดสอบเป็นส่วนสำคัญของกระบวนการพัฒนา โปรดปฏิบัติตามแนวทางต่อไปนี้:

### Backend

- เขียน unit tests สำหรับ services และ utilities
- เขียน integration tests สำหรับ API endpoints
- ใช้ [pytest](https://docs.pytest.org/) สำหรับการทดสอบ
- รัน tests ก่อนส่ง Pull Request
  ```bash
  cd backend
  pytest
  ```

### Frontend

- เขียน unit tests สำหรับ components และ hooks
- เขียน integration tests สำหรับ pages
- ใช้ [Jest](https://jestjs.io/) และ [React Testing Library](https://testing-library.com/docs/react-testing-library/intro/) สำหรับการทดสอบ
- รัน tests ก่อนส่ง Pull Request
  ```bash
  cd frontend
  npm test
  ```

## การส่ง Pull Request

เมื่อคุณพร้อมส่ง Pull Request โปรดปฏิบัติตามแนวทางต่อไปนี้:

1. **ตรวจสอบว่าโค้ดของคุณผ่านการทดสอบทั้งหมด**
2. **อัพเดท branch ของคุณให้ทันสมัย**
   ```bash
   git checkout master
   git pull upstream master
   git checkout feature/amazing-feature
   git rebase master
   ```
3. **แก้ไข conflicts (ถ้ามี)**
4. **Push การเปลี่ยนแปลงไปยัง fork ของคุณ**
   ```bash
   git push origin feature/amazing-feature -f
   ```
5. **สร้าง Pull Request**:
   - ใช้ชื่อที่ชัดเจนและกระชับ
   - อธิบายการเปลี่ยนแปลงของคุณอย่างละเอียด
   - อ้างอิงถึง issue ที่เกี่ยวข้อง (ถ้ามี)
   - แนบภาพหน้าจอหรือวิดีโอ (ถ้าเกี่ยวข้อง)
6. **รอการ review**: ทีมพัฒนาจะ review Pull Request ของคุณและอาจขอให้คุณแก้ไขบางส่วน
7. **แก้ไขตามคำแนะนำ**: หากมีการขอให้แก้ไข โปรดแก้ไขและ push การเปลี่ยนแปลงไปยัง branch เดิม

---

ขอบคุณอีกครั้งสำหรับความสนใจในการมีส่วนร่วมพัฒนา MeditechZ! หากคุณมีคำถามหรือข้อสงสัยใดๆ โปรดติดต่อเราที่ [contact@meditechz.com](mailto:contact@meditechz.com) หรือสร้าง issue ใน [GitHub Issues](https://github.com/ZanKinZuiTH/MeditechZ/issues) 