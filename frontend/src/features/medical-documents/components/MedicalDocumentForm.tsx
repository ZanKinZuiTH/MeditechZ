import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import useMedicalDocumentsApi, { MedicalDocumentCreate, MedicalDocument } from '../hooks/useMedicalDocumentsApi';

interface MedicalDocumentFormProps {
  isEdit: boolean;
}

const MedicalDocumentForm: React.FC<MedicalDocumentFormProps> = ({ isEdit }) => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { 
    getMedicalDocumentById, 
    createMedicalDocument, 
    updateMedicalDocument,
    loading,
    error: apiError
  } = useMedicalDocumentsApi();

  const [formData, setFormData] = useState<MedicalDocumentCreate>({
    patient_id: 0,
    doctor_id: 0,
    document_date: new Date().toISOString().split('T')[0],
    document_type: 'prescription',
    document_status: 'active',
    notes: ''
  });

  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);

  useEffect(() => {
    if (isEdit && id) {
      const fetchMedicalDocument = async () => {
        try {
          const data = await getMedicalDocumentById(parseInt(id));
          setFormData({
            patient_id: data.patient_id,
            doctor_id: data.doctor_id,
            visit_id: data.visit_id,
            document_date: data.document_date,
            document_type: data.document_type,
            document_status: data.document_status,
            notes: data.notes || ''
          });
        } catch (err) {
          console.error('เกิดข้อผิดพลาดในการดึงข้อมูลเอกสารทางการแพทย์:', err);
          setError('ไม่สามารถโหลดข้อมูลเอกสารทางการแพทย์ได้ โปรดลองอีกครั้งในภายหลัง');
        }
      };

      fetchMedicalDocument();
    }
  }, [isEdit, id, getMedicalDocumentById]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);

    try {
      if (isEdit && id) {
        await updateMedicalDocument(parseInt(id), formData);
        setSuccess(true);
        setTimeout(() => {
          navigate(`/medical-documents/${id}`);
        }, 1500);
      } else {
        const newDocument = await createMedicalDocument(formData);
        setSuccess(true);
        setTimeout(() => {
          navigate(`/medical-documents/${newDocument.id}`);
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
          {isEdit ? 'แก้ไขเอกสารทางการแพทย์' : 'สร้างเอกสารทางการแพทย์ใหม่'}
        </h1>
        <p className="text-gray-600 mb-6">
          {isEdit ? 'แก้ไขข้อมูลในเอกสารทางการแพทย์' : 'กรอกข้อมูลเพื่อสร้างเอกสารทางการแพทย์ใหม่'}
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
            <span>{isEdit ? 'บันทึกการแก้ไขเรียบร้อยแล้ว' : 'สร้างเอกสารทางการแพทย์เรียบร้อยแล้ว'}</span>
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

          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="form-control">
              <label className="label">
                <span className="label-text">วันที่เอกสาร</span>
              </label>
              <input
                type="date"
                name="document_date"
                className="input input-bordered"
                value={formData.document_date}
                onChange={handleInputChange}
                required
              />
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">ประเภทเอกสาร</span>
              </label>
              <select
                name="document_type"
                className="select select-bordered"
                value={formData.document_type}
                onChange={handleInputChange}
                required
              >
                <option value="prescription">ใบสั่งยา</option>
                <option value="lab_result">ผลการตรวจทางห้องปฏิบัติการ</option>
                <option value="radiology_result">ผลการตรวจทางรังสี</option>
                <option value="discharge_summary">สรุปการจำหน่ายผู้ป่วย</option>
                <option value="referral">ใบส่งตัว</option>
                <option value="other">อื่นๆ</option>
              </select>
            </div>
            <div className="form-control">
              <label className="label">
                <span className="label-text">สถานะเอกสาร</span>
              </label>
              <select
                name="document_status"
                className="select select-bordered"
                value={formData.document_status}
                onChange={handleInputChange}
                required
              >
                <option value="draft">ฉบับร่าง</option>
                <option value="active">ใช้งาน</option>
                <option value="inactive">ไม่ใช้งาน</option>
                <option value="cancelled">ยกเลิก</option>
              </select>
            </div>
          </div>

          <div className="form-control">
            <label className="label">
              <span className="label-text">หมายเหตุ</span>
            </label>
            <textarea
              name="notes"
              className="textarea textarea-bordered h-24"
              value={formData.notes || ''}
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

export default MedicalDocumentForm;
