import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { formatThaiDate } from '../../../lib/utils';
import useMedicalDocumentsApi, { HealthCheckupBookCreate, HealthCheckupBook } from '../hooks/useMedicalDocumentsApi';

interface HealthCheckupBookFormProps {
  isEdit: boolean;
}

const HealthCheckupBookForm: React.FC<HealthCheckupBookFormProps> = ({ isEdit }) => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { 
    getHealthCheckupBookById, 
    createHealthCheckupBook, 
    updateHealthCheckupBook,
    loading,
    error: apiError
  } = useMedicalDocumentsApi();

  const [formData, setFormData] = useState<HealthCheckupBookCreate>({
    patient_id: 0,
    doctor_id: 0,
    checkup_date: new Date().toISOString().split('T')[0],
    checkup_type: 'annual',
    vital_signs: {
      temperature: '',
      pulse: '',
      respiratory_rate: '',
      blood_pressure: '',
      weight: '',
      height: '',
      bmi: ''
    },
    physical_exam: {},
    lab_results: {},
    radiology_results: {},
    ekg_results: {},
    spirometry_results: {},
    audiometry_results: {},
    vision_test_results: {},
    conclusion: '',
    recommendations: '',
    doctor_license_no: ''
  });

  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);
  
  // สำหรับเก็บข้อมูลในรูปแบบข้อความ (string) เพื่อแสดงในฟอร์ม
  const [physicalExamText, setPhysicalExamText] = useState<string>('');
  const [labResultsText, setLabResultsText] = useState<string>('');
  const [radiologyResultsText, setRadiologyResultsText] = useState<string>('');
  const [ekgResultsText, setEkgResultsText] = useState<string>('');
  const [spirometryResultsText, setSpirometryResultsText] = useState<string>('');
  const [audiometryResultsText, setAudiometryResultsText] = useState<string>('');
  const [visionTestResultsText, setVisionTestResultsText] = useState<string>('');

  useEffect(() => {
    if (isEdit && id) {
      const fetchHealthCheckupBook = async () => {
        try {
          const data = await getHealthCheckupBookById(parseInt(id));
          setFormData({
            patient_id: data.patient_id,
            doctor_id: data.doctor_id,
            visit_id: data.visit_id,
            checkup_date: data.checkup_date,
            checkup_type: data.checkup_type,
            vital_signs: data.vital_signs || {
              temperature: '',
              pulse: '',
              respiratory_rate: '',
              blood_pressure: '',
              weight: '',
              height: '',
              bmi: ''
            },
            physical_exam: data.physical_exam || {},
            lab_results: data.lab_results || {},
            radiology_results: data.radiology_results || {},
            ekg_results: data.ekg_results || {},
            spirometry_results: data.spirometry_results || {},
            audiometry_results: data.audiometry_results || {},
            vision_test_results: data.vision_test_results || {},
            conclusion: data.conclusion || '',
            recommendations: data.recommendations || '',
            doctor_license_no: data.doctor_license_no || '',
            doctor_signature: data.doctor_signature || ''
          });
          
          // แปลงข้อมูลจาก Record<string, any> เป็น string เพื่อแสดงในฟอร์ม
          setPhysicalExamText(typeof data.physical_exam === 'string' ? data.physical_exam : JSON.stringify(data.physical_exam || {}));
          setLabResultsText(typeof data.lab_results === 'string' ? data.lab_results : JSON.stringify(data.lab_results || {}));
          setRadiologyResultsText(typeof data.radiology_results === 'string' ? data.radiology_results : JSON.stringify(data.radiology_results || {}));
          setEkgResultsText(typeof data.ekg_results === 'string' ? data.ekg_results : JSON.stringify(data.ekg_results || {}));
          setSpirometryResultsText(typeof data.spirometry_results === 'string' ? data.spirometry_results : JSON.stringify(data.spirometry_results || {}));
          setAudiometryResultsText(typeof data.audiometry_results === 'string' ? data.audiometry_results : JSON.stringify(data.audiometry_results || {}));
          setVisionTestResultsText(typeof data.vision_test_results === 'string' ? data.vision_test_results : JSON.stringify(data.vision_test_results || {}));
        } catch (err) {
          console.error('เกิดข้อผิดพลาดในการดึงข้อมูลสมุดตรวจสุขภาพ:', err);
          setError('ไม่สามารถโหลดข้อมูลสมุดตรวจสุขภาพได้ โปรดลองอีกครั้งในภายหลัง');
        }
      };

      fetchHealthCheckupBook();
    }
  }, [isEdit, id, getHealthCheckupBookById]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleNestedInputChange = (category: string, field: string, value: string) => {
    if (category === 'vital_signs') {
      setFormData({
        ...formData,
        vital_signs: {
          ...formData.vital_signs,
          [field]: value
        }
      });
    }
  };
  
  // อัพเดทข้อมูลในรูปแบบข้อความและแปลงเป็น Record<string, any> เมื่อส่งฟอร์ม
  const handleTextAreaChange = (e: React.ChangeEvent<HTMLTextAreaElement>, setter: React.Dispatch<React.SetStateAction<string>>) => {
    const { value } = e.target;
    setter(value);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);

    try {
      // แปลงข้อความเป็น Record<string, any> ก่อนส่งข้อมูล
      const submissionData = {
        ...formData,
        physical_exam: { text: physicalExamText },
        lab_results: { text: labResultsText },
        radiology_results: { text: radiologyResultsText },
        ekg_results: { text: ekgResultsText },
        spirometry_results: { text: spirometryResultsText },
        audiometry_results: { text: audiometryResultsText },
        vision_test_results: { text: visionTestResultsText }
      };

      if (isEdit && id) {
        await updateHealthCheckupBook(parseInt(id), submissionData);
        setSuccess(true);
        setTimeout(() => {
          navigate(`/medical-documents/checkup-books/${id}`);
        }, 1500);
      } else {
        const newBook = await createHealthCheckupBook(submissionData);
        setSuccess(true);
        setTimeout(() => {
          navigate(`/medical-documents/checkup-books/${newBook.id}`);
        }, 1500);
      }
    } catch (err) {
      console.error('เกิดข้อผิดพลาดในการบันทึกข้อมูล:', err);
      setError('ไม่สามารถบันทึกข้อมูลได้ โปรดตรวจสอบข้อมูลและลองอีกครั้ง');
    }
  };

  if (loading && isEdit) {
    return (
      <div className="flex justify-center p-8">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="bg-white rounded-lg shadow-sm p-6">
        <h1 className="text-2xl font-bold mb-2">
          {isEdit ? 'แก้ไขสมุดตรวจสุขภาพ' : 'สร้างสมุดตรวจสุขภาพใหม่'}
        </h1>
        <p className="text-gray-600 mb-6">
          {isEdit ? 'แก้ไขข้อมูลในสมุดตรวจสุขภาพ' : 'กรอกข้อมูลเพื่อสร้างสมุดตรวจสุขภาพใหม่'}
        </p>

        {error && (
          <div className="alert alert-error mb-6">
            <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            <span>{error}</span>
          </div>
        )}

        {apiError && (
          <div className="alert alert-error mb-6">
            <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            <span>{apiError}</span>
          </div>
        )}

        {success && (
          <div className="alert alert-success mb-6">
            <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            <span>{isEdit ? 'บันทึกการแก้ไขเรียบร้อยแล้ว' : 'สร้างสมุดตรวจสุขภาพเรียบร้อยแล้ว'}</span>
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">รหัสผู้ป่วย</span>
              </label>
              <input
                type="number"
                name="patient_id"
                className="input input-bordered"
                value={formData.patient_id}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">รหัสแพทย์</span>
              </label>
              <input
                type="number"
                name="doctor_id"
                className="input input-bordered"
                value={formData.doctor_id}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">รหัสการเข้ารับบริการ (ถ้ามี)</span>
              </label>
              <input
                type="number"
                name="visit_id"
                className="input input-bordered"
                value={formData.visit_id || ''}
                onChange={handleInputChange}
              />
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">วันที่ตรวจ</span>
              </label>
              <input
                type="date"
                name="checkup_date"
                className="input input-bordered"
                value={formData.checkup_date}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ประเภทการตรวจ</span>
              </label>
              <select
                name="checkup_type"
                className="select select-bordered"
                value={formData.checkup_type}
                onChange={handleInputChange}
                required
              >
                <option value="annual">ตรวจสุขภาพประจำปี</option>
                <option value="pre_employment">ตรวจสุขภาพก่อนเข้าทำงาน</option>
                <option value="insurance">ตรวจสุขภาพเพื่อทำประกัน</option>
                <option value="driver_license">ตรวจสุขภาพเพื่อทำใบขับขี่</option>
                <option value="other">อื่นๆ</option>
              </select>
            </div>
          </div>

          <div className="divider">สัญญาณชีพ</div>
          
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            <div className="form-control">
              <label className="label">
                <span className="label-text">อุณหภูมิ (°C)</span>
              </label>
              <input
                type="text"
                className="input input-bordered"
                value={formData.vital_signs?.temperature || ''}
                onChange={(e) => handleNestedInputChange('vital_signs', 'temperature', e.target.value)}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ชีพจร (ครั้ง/นาที)</span>
              </label>
              <input
                type="text"
                className="input input-bordered"
                value={formData.vital_signs?.pulse || ''}
                onChange={(e) => handleNestedInputChange('vital_signs', 'pulse', e.target.value)}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">อัตราการหายใจ (ครั้ง/นาที)</span>
              </label>
              <input
                type="text"
                className="input input-bordered"
                value={formData.vital_signs?.respiratory_rate || ''}
                onChange={(e) => handleNestedInputChange('vital_signs', 'respiratory_rate', e.target.value)}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ความดันโลหิต (mmHg)</span>
              </label>
              <input
                type="text"
                className="input input-bordered"
                value={formData.vital_signs?.blood_pressure || ''}
                onChange={(e) => handleNestedInputChange('vital_signs', 'blood_pressure', e.target.value)}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">น้ำหนัก (กก.)</span>
              </label>
              <input
                type="text"
                className="input input-bordered"
                value={formData.vital_signs?.weight || ''}
                onChange={(e) => handleNestedInputChange('vital_signs', 'weight', e.target.value)}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ส่วนสูง (ซม.)</span>
              </label>
              <input
                type="text"
                className="input input-bordered"
                value={formData.vital_signs?.height || ''}
                onChange={(e) => handleNestedInputChange('vital_signs', 'height', e.target.value)}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ดัชนีมวลกาย (BMI)</span>
              </label>
              <input
                type="text"
                className="input input-bordered"
                value={formData.vital_signs?.bmi || ''}
                onChange={(e) => handleNestedInputChange('vital_signs', 'bmi', e.target.value)}
              />
            </div>
          </div>

          <div className="divider">การตรวจร่างกาย</div>
          
          <div className="form-control">
            <label className="label">
              <span className="label-text">บันทึกการตรวจร่างกาย</span>
            </label>
            <textarea
              className="textarea textarea-bordered h-24"
              value={physicalExamText}
              onChange={(e) => handleTextAreaChange(e, setPhysicalExamText)}
            ></textarea>
          </div>

          <div className="divider">ผลการตรวจทางห้องปฏิบัติการและการตรวจพิเศษ</div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">ผลการตรวจทางห้องปฏิบัติการ</span>
              </label>
              <textarea
                className="textarea textarea-bordered h-24"
                value={labResultsText}
                onChange={(e) => handleTextAreaChange(e, setLabResultsText)}
              ></textarea>
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ผลการตรวจทางรังสี</span>
              </label>
              <textarea
                className="textarea textarea-bordered h-24"
                value={radiologyResultsText}
                onChange={(e) => handleTextAreaChange(e, setRadiologyResultsText)}
              ></textarea>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">ผลการตรวจคลื่นไฟฟ้าหัวใจ (EKG)</span>
              </label>
              <textarea
                className="textarea textarea-bordered h-24"
                value={ekgResultsText}
                onChange={(e) => handleTextAreaChange(e, setEkgResultsText)}
              ></textarea>
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ผลการตรวจสมรรถภาพปอด</span>
              </label>
              <textarea
                className="textarea textarea-bordered h-24"
                value={spirometryResultsText}
                onChange={(e) => handleTextAreaChange(e, setSpirometryResultsText)}
              ></textarea>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">ผลการตรวจการได้ยิน</span>
              </label>
              <textarea
                className="textarea textarea-bordered h-24"
                value={audiometryResultsText}
                onChange={(e) => handleTextAreaChange(e, setAudiometryResultsText)}
              ></textarea>
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ผลการตรวจสายตา</span>
              </label>
              <textarea
                className="textarea textarea-bordered h-24"
                value={visionTestResultsText}
                onChange={(e) => handleTextAreaChange(e, setVisionTestResultsText)}
              ></textarea>
            </div>
          </div>

          <div className="divider">ข้อสรุปและคำแนะนำ</div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">ข้อสรุป</span>
              </label>
              <textarea
                name="conclusion"
                className="textarea textarea-bordered h-24"
                value={formData.conclusion || ''}
                onChange={handleInputChange}
              ></textarea>
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">คำแนะนำ</span>
              </label>
              <textarea
                name="recommendations"
                className="textarea textarea-bordered h-24"
                value={formData.recommendations || ''}
                onChange={handleInputChange}
              ></textarea>
            </div>
          </div>

          <div className="divider">ข้อมูลแพทย์ผู้ตรวจ</div>
          
          <div className="form-control">
            <label className="label">
              <span className="label-text">เลขที่ใบอนุญาตประกอบวิชาชีพเวชกรรม</span>
            </label>
            <input
              type="text"
              name="doctor_license_no"
              className="input input-bordered"
              value={formData.doctor_license_no || ''}
              onChange={handleInputChange}
            />
          </div>

          <div className="form-control">
            <label className="label">
              <span className="label-text">ลายเซ็นแพทย์ (URL)</span>
            </label>
            <input
              type="text"
              name="doctor_signature"
              className="input input-bordered"
              value={formData.doctor_signature || ''}
              onChange={handleInputChange}
              placeholder="ใส่ URL ของรูปภาพลายเซ็น"
            />
          </div>

          <div className="flex justify-end space-x-4 mt-8">
            <button
              type="button"
              className="btn btn-outline"
              onClick={() => navigate('/medical-documents')}
            >
              ยกเลิก
            </button>
            <button
              type="submit"
              className="btn btn-primary"
              disabled={loading}
            >
              {loading ? (
                <>
                  <span className="loading loading-spinner"></span>
                  กำลังบันทึก...
                </>
              ) : (
                'บันทึก'
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default HealthCheckupBookForm;