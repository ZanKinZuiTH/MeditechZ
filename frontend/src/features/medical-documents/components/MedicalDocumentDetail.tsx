import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { formatThaiDate } from '../../../lib/utils';
import useMedicalDocumentsApi, { MedicalDocument } from '../hooks/useMedicalDocumentsApi';

const MedicalDocumentDetail: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { getMedicalDocumentById, loading, error: apiError } = useMedicalDocumentsApi();
  
  const [document, setDocument] = useState<MedicalDocument | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      const fetchMedicalDocument = async () => {
        try {
          const data = await getMedicalDocumentById(parseInt(id));
          setDocument(data);
        } catch (err) {
          console.error('เกิดข้อผิดพลาดในการดึงข้อมูลเอกสารทางการแพทย์:', err);
          setError('ไม่สามารถโหลดข้อมูลเอกสารทางการแพทย์ได้ โปรดลองอีกครั้งในภายหลัง');
        }
      };

      fetchMedicalDocument();
    }
  }, [id, getMedicalDocumentById]);

  const renderDocumentType = (type: string) => {
    switch (type) {
      case 'prescription':
        return 'ใบสั่งยา';
      case 'lab_result':
        return 'ผลการตรวจทางห้องปฏิบัติการ';
      case 'radiology_result':
        return 'ผลการตรวจทางรังสี';
      case 'discharge_summary':
        return 'สรุปการจำหน่ายผู้ป่วย';
      case 'referral':
        return 'ใบส่งตัว';
      case 'other':
        return 'อื่นๆ';
      default:
        return type;
    }
  };

  const renderDocumentStatus = (status: string) => {
    switch (status) {
      case 'draft':
        return 'ฉบับร่าง';
      case 'active':
        return 'ใช้งาน';
      case 'inactive':
        return 'ไม่ใช้งาน';
      case 'cancelled':
        return 'ยกเลิก';
      default:
        return status;
    }
  };

  if (loading) {
    return (
      <div className="flex justify-center p-8">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
      </div>
    );
  }

  if (error || apiError) {
    return (
      <div className="p-4">
        <div className="alert alert-error mb-4">
          <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
          <span>{error || apiError}</span>
        </div>
        <button 
          className="btn btn-primary"
          onClick={() => navigate('/medical-documents')}
        >
          กลับไปหน้าเอกสารทางการแพทย์
        </button>
      </div>
    );
  }

  if (!document) {
    return (
      <div className="p-4">
        <div className="alert alert-warning mb-4">
          <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" /></svg>
          <span>ไม่พบข้อมูลเอกสารทางการแพทย์</span>
        </div>
        <button 
          className="btn btn-primary"
          onClick={() => navigate('/medical-documents')}
        >
          กลับไปหน้าเอกสารทางการแพทย์
        </button>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="bg-white rounded-lg shadow-sm p-6">
        <div className="flex justify-between items-center mb-6">
          <h1 className="text-2xl font-bold">รายละเอียดเอกสารทางการแพทย์</h1>
          <div className="flex space-x-2">
            <button
              className="btn btn-outline"
              onClick={() => navigate('/medical-documents')}
            >
              กลับ
            </button>
            <button
              className="btn btn-warning"
              onClick={() => navigate(`/medical-documents/${id}/edit`)}
            >
              แก้ไข
            </button>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ข้อมูลทั่วไป</h2>
              <div className="divider my-1"></div>
              <p><span className="font-semibold">รหัสเอกสาร:</span> {document.id}</p>
              <p><span className="font-semibold">รหัสผู้ป่วย:</span> {document.patient_id}</p>
              <p><span className="font-semibold">รหัสแพทย์:</span> {document.doctor_id}</p>
              {document.visit_id && <p><span className="font-semibold">รหัสการเข้ารับบริการ:</span> {document.visit_id}</p>}
              <p><span className="font-semibold">วันที่เอกสาร:</span> {formatThaiDate(new Date(document.document_date))}</p>
              <p><span className="font-semibold">ประเภทเอกสาร:</span> {renderDocumentType(document.document_type)}</p>
              <p><span className="font-semibold">สถานะเอกสาร:</span> {renderDocumentStatus(document.document_status)}</p>
            </div>
          </div>

          {document.notes && (
            <div className="card bg-base-100 shadow-sm">
              <div className="card-body">
                <h2 className="card-title">หมายเหตุ</h2>
                <div className="divider my-1"></div>
                <p className="whitespace-pre-line">{document.notes}</p>
              </div>
            </div>
          )}
        </div>

        <div className="card bg-base-100 shadow-sm mb-6">
          <div className="card-body">
            <h2 className="card-title">ข้อมูลเพิ่มเติม</h2>
            <div className="divider my-1"></div>
            <p><span className="font-semibold">วันที่สร้าง:</span> {formatThaiDate(new Date(document.created_at))}</p>
            {document.updated_at && (
              <p><span className="font-semibold">วันที่แก้ไขล่าสุด:</span> {formatThaiDate(new Date(document.updated_at))}</p>
            )}
            <p><span className="font-semibold">สร้างโดย:</span> รหัสผู้ใช้ {document.created_by_id}</p>
            {document.updated_by_id && (
              <p><span className="font-semibold">แก้ไขล่าสุดโดย:</span> รหัสผู้ใช้ {document.updated_by_id}</p>
            )}
          </div>
        </div>

        <div className="flex justify-end space-x-4 mt-8">
          <button
            className="btn btn-outline"
            onClick={() => navigate('/medical-documents')}
          >
            กลับไปหน้าเอกสารทางการแพทย์
          </button>
          <button
            className="btn btn-warning"
            onClick={() => navigate(`/medical-documents/${id}/edit`)}
          >
            แก้ไขเอกสารทางการแพทย์
          </button>
        </div>
      </div>
    </div>
  );
};

export default MedicalDocumentDetail; 