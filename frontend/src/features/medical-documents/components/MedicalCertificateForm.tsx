import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import useMedicalDocumentsApi, { MedicalCertificateCreate, MedicalCertificate } from '../hooks/useMedicalDocumentsApi';

interface MedicalCertificateFormProps {
  isEdit: boolean;
}

const MedicalCertificateForm: React.FC<MedicalCertificateFormProps> = ({ isEdit }) => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { 
    getMedicalCertificateById, 
    createMedicalCertificate, 
    updateMedicalCertificate,
    loading,
    error: apiError
  } = useMedicalDocumentsApi();

  const [formData, setFormData] = useState<MedicalCertificateCreate>({
    patient_id: 0,
    doctor_id: 0,
    certificate_date: new Date().toISOString().split('T')[0],
    certificate_type: 'sick_leave',
    diagnosis: '',
    treatment: '',
    rest_period_days: 0,
    rest_period_start: '',
    rest_period_end: '',
    doctor_license_no: '',
    doctor_license_issue_date: '',
    vital_signs: {
      temperature: '',
      pulse: '',
      respiratory_rate: '',
      blood_pressure: ''
    },
    comments: ''
  });

  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);

  useEffect(() => {
    if (isEdit && id) {
      const fetchMedicalCertificate = async () => {
        try {
          const data = await getMedicalCertificateById(parseInt(id));
          setFormData({
            patient_id: data.patient_id,
            doctor_id: data.doctor_id,
            visit_id: data.visit_id,
            certificate_date: data.certificate_date,
            certificate_type: data.certificate_type,
            diagnosis: data.diagnosis || '',
            treatment: data.treatment || '',
            rest_period_days: data.rest_period_days || 0,
            rest_period_start: data.rest_period_start || '',
            rest_period_end: data.rest_period_end || '',
            doctor_license_no: data.doctor_license_no || '',
            doctor_license_issue_date: data.doctor_license_issue_date || '',
            doctor2_id: data.doctor2_id,
            doctor2_license_no: data.doctor2_license_no || '',
            doctor2_license_issue_date: data.doctor2_license_issue_date || '',
            vital_signs: data.vital_signs || {
              temperature: '',
              pulse: '',
              respiratory_rate: '',
              blood_pressure: ''
            },
            comments: data.comments || ''
          });
        } catch (err) {
          console.error('เกิดข้อผิดพลาดในการดึงข้อมูลใบรับรองแพทย์:', err);
          setError('ไม่สามารถโหลดข้อมูลใบรับรองแพทย์ได้ โปรดลองอีกครั้งในภายหลัง');
        }
      };

      fetchMedicalCertificate();
    }
  }, [isEdit, id, getMedicalCertificateById]);

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

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);

    try {
      if (isEdit && id) {
        await updateMedicalCertificate(parseInt(id), formData);
        setSuccess(true);
        setTimeout(() => {
          navigate(`/medical-documents/certificates/${id}`);
        }, 1500);
      } else {
        const newCertificate = await createMedicalCertificate(formData);
        setSuccess(true);
        setTimeout(() => {
          navigate(`/medical-documents/certificates/${newCertificate.id}`);
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
          {isEdit ? 'แก้ไขใบรับรองแพทย์' : 'สร้างใบรับรองแพทย์ใหม่'}
        </h1>
        <p className="text-gray-600 mb-6">
          {isEdit ? 'แก้ไขข้อมูลในใบรับรองแพทย์' : 'กรอกข้อมูลเพื่อสร้างใบรับรองแพทย์ใหม่'}
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
            <span>{isEdit ? 'บันทึกการแก้ไขเรียบร้อยแล้ว' : 'สร้างใบรับรองแพทย์เรียบร้อยแล้ว'}</span>
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
                <span className="label-text">วันที่ออกใบรับรอง</span>
              </label>
              <input
                type="date"
                name="certificate_date"
                className="input input-bordered"
                value={formData.certificate_date}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ประเภทใบรับรอง</span>
              </label>
              <select
                name="certificate_type"
                className="select select-bordered"
                value={formData.certificate_type}
                onChange={handleInputChange}
                required
              >
                <option value="sick_leave">ใบรับรองแพทย์ลาป่วย</option>
                <option value="health_certificate">ใบรับรองแพทย์ตรวจสุขภาพ</option>
                <option value="disability_certificate">ใบรับรองความพิการ</option>
                <option value="fitness_certificate">ใบรับรองความสมบูรณ์ทางร่างกาย</option>
                <option value="other">อื่นๆ</option>
              </select>
            </div>
          </div>

          <div className="divider">ข้อมูลการวินิจฉัยและการรักษา</div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">การวินิจฉัย</span>
              </label>
              <textarea
                name="diagnosis"
                className="textarea textarea-bordered h-24"
                value={formData.diagnosis || ''}
                onChange={handleInputChange}
              ></textarea>
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">การรักษา</span>
              </label>
              <textarea
                name="treatment"
                className="textarea textarea-bordered h-24"
                value={formData.treatment || ''}
                onChange={handleInputChange}
              ></textarea>
            </div>
          </div>

          <div className="divider">ข้อมูลการพักฟื้น</div>
          
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">จำนวนวันพักฟื้น</span>
              </label>
              <input
                type="number"
                name="rest_period_days"
                className="input input-bordered"
                value={formData.rest_period_days || ''}
                onChange={handleInputChange}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">วันที่เริ่มพักฟื้น</span>
              </label>
              <input
                type="date"
                name="rest_period_start"
                className="input input-bordered"
                value={formData.rest_period_start || ''}
                onChange={handleInputChange}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">วันที่สิ้นสุดการพักฟื้น</span>
              </label>
              <input
                type="date"
                name="rest_period_end"
                className="input input-bordered"
                value={formData.rest_period_end || ''}
                onChange={handleInputChange}
              />
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
          </div>

          <div className="divider">ข้อมูลแพทย์ผู้ออกใบรับรอง</div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
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
                <span className="label-text">วันที่ออกใบอนุญาต</span>
              </label>
              <input
                type="date"
                name="doctor_license_issue_date"
                className="input input-bordered"
                value={formData.doctor_license_issue_date || ''}
                onChange={handleInputChange}
              />
            </div>
          </div>

          <div className="divider">ข้อมูลแพทย์ผู้ร่วมออกใบรับรอง (ถ้ามี)</div>
          
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">รหัสแพทย์</span>
              </label>
              <input
                type="number"
                name="doctor2_id"
                className="input input-bordered"
                value={formData.doctor2_id || ''}
                onChange={handleInputChange}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">เลขที่ใบอนุญาตประกอบวิชาชีพเวชกรรม</span>
              </label>
              <input
                type="text"
                name="doctor2_license_no"
                className="input input-bordered"
                value={formData.doctor2_license_no || ''}
                onChange={handleInputChange}
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">วันที่ออกใบอนุญาต</span>
              </label>
              <input
                type="date"
                name="doctor2_license_issue_date"
                className="input input-bordered"
                value={formData.doctor2_license_issue_date || ''}
                onChange={handleInputChange}
              />
            </div>
          </div>

          <div className="form-control">
            <label className="label">
              <span className="label-text">หมายเหตุ</span>
            </label>
            <textarea
              name="comments"
              className="textarea textarea-bordered h-24"
              value={formData.comments || ''}
              onChange={handleInputChange}
            ></textarea>
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

export default MedicalCertificateForm; 