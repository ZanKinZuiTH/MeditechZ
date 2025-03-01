import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { formatThaiDate } from '../../../lib/utils';
import useMedicalDocumentsApi, { MedicalCertificate } from '../hooks/useMedicalDocumentsApi';

const MedicalCertificateDetail: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { getMedicalCertificateById, loading, error: apiError } = useMedicalDocumentsApi();
  
  const [certificate, setCertificate] = useState<MedicalCertificate | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      const fetchMedicalCertificate = async () => {
        try {
          const data = await getMedicalCertificateById(parseInt(id));
          setCertificate(data);
        } catch (err) {
          console.error('เกิดข้อผิดพลาดในการดึงข้อมูลใบรับรองแพทย์:', err);
          setError('ไม่สามารถโหลดข้อมูลใบรับรองแพทย์ได้ โปรดลองอีกครั้งในภายหลัง');
        }
      };

      fetchMedicalCertificate();
    }
  }, [id, getMedicalCertificateById]);

  const renderCertificateType = (type: string) => {
    switch (type) {
      case 'sick_leave':
        return 'ใบรับรองแพทย์ลาป่วย';
      case 'health_certificate':
        return 'ใบรับรองแพทย์ตรวจสุขภาพ';
      case 'disability_certificate':
        return 'ใบรับรองความพิการ';
      case 'fitness_certificate':
        return 'ใบรับรองความสมบูรณ์ทางร่างกาย';
      case 'other':
        return 'อื่นๆ';
      default:
        return type;
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

  if (!certificate) {
    return (
      <div className="p-4">
        <div className="alert alert-warning mb-4">
          <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" /></svg>
          <span>ไม่พบข้อมูลใบรับรองแพทย์</span>
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
          <h1 className="text-2xl font-bold">รายละเอียดใบรับรองแพทย์</h1>
          <div className="flex space-x-2">
            <button
              className="btn btn-outline"
              onClick={() => navigate('/medical-documents')}
            >
              กลับ
            </button>
            <button
              className="btn btn-warning"
              onClick={() => navigate(`/medical-documents/certificates/${id}/edit`)}
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
              <p><span className="font-semibold">รหัสผู้ป่วย:</span> {certificate.patient_id}</p>
              <p><span className="font-semibold">รหัสแพทย์:</span> {certificate.doctor_id}</p>
              {certificate.visit_id && <p><span className="font-semibold">รหัสการเข้ารับบริการ:</span> {certificate.visit_id}</p>}
              <p><span className="font-semibold">วันที่ออกใบรับรอง:</span> {formatThaiDate(new Date(certificate.certificate_date))}</p>
              <p><span className="font-semibold">ประเภทใบรับรอง:</span> {renderCertificateType(certificate.certificate_type)}</p>
            </div>
          </div>

          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ข้อมูลการวินิจฉัยและการรักษา</h2>
              <div className="divider my-1"></div>
              {certificate.diagnosis && <p><span className="font-semibold">การวินิจฉัย:</span> {certificate.diagnosis}</p>}
              {certificate.treatment && <p><span className="font-semibold">การรักษา:</span> {certificate.treatment}</p>}
              {certificate.rest_period_days !== undefined && <p><span className="font-semibold">จำนวนวันพักฟื้น:</span> {certificate.rest_period_days} วัน</p>}
              {certificate.rest_period_start && <p><span className="font-semibold">วันที่เริ่มพักฟื้น:</span> {formatThaiDate(new Date(certificate.rest_period_start))}</p>}
              {certificate.rest_period_end && <p><span className="font-semibold">วันที่สิ้นสุดการพักฟื้น:</span> {formatThaiDate(new Date(certificate.rest_period_end))}</p>}
            </div>
          </div>
        </div>

        {certificate.vital_signs && Object.keys(certificate.vital_signs).length > 0 && (
          <div className="card bg-base-100 shadow-sm mb-6">
            <div className="card-body">
              <h2 className="card-title">สัญญาณชีพ</h2>
              <div className="divider my-1"></div>
              <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                {Object.entries(certificate.vital_signs).map(([key, value]) => (
                  value && value.toString().trim() !== '' ? (
                    <div key={key} className="mb-2">
                      <span className="font-semibold">{key}: </span>
                      <span>{value.toString()}</span>
                    </div>
                  ) : null
                ))}
              </div>
            </div>
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ข้อมูลแพทย์ผู้ออกใบรับรอง</h2>
              <div className="divider my-1"></div>
              {certificate.doctor_license_no && <p><span className="font-semibold">เลขที่ใบอนุญาตประกอบวิชาชีพเวชกรรม:</span> {certificate.doctor_license_no}</p>}
              {certificate.doctor_license_issue_date && <p><span className="font-semibold">วันที่ออกใบอนุญาต:</span> {formatThaiDate(new Date(certificate.doctor_license_issue_date))}</p>}
            </div>
          </div>

          {certificate.doctor2_id && (
            <div className="card bg-base-100 shadow-sm">
              <div className="card-body">
                <h2 className="card-title">ข้อมูลแพทย์ผู้ร่วมออกใบรับรอง</h2>
                <div className="divider my-1"></div>
                <p><span className="font-semibold">รหัสแพทย์:</span> {certificate.doctor2_id}</p>
                {certificate.doctor2_license_no && <p><span className="font-semibold">เลขที่ใบอนุญาตประกอบวิชาชีพเวชกรรม:</span> {certificate.doctor2_license_no}</p>}
                {certificate.doctor2_license_issue_date && <p><span className="font-semibold">วันที่ออกใบอนุญาต:</span> {formatThaiDate(new Date(certificate.doctor2_license_issue_date))}</p>}
              </div>
            </div>
          )}
        </div>

        {certificate.comments && (
          <div className="card bg-base-100 shadow-sm mb-6">
            <div className="card-body">
              <h2 className="card-title">หมายเหตุ</h2>
              <div className="divider my-1"></div>
              <p className="whitespace-pre-line">{certificate.comments}</p>
            </div>
          </div>
        )}

        <div className="card bg-base-100 shadow-sm mb-6">
          <div className="card-body">
            <h2 className="card-title">ข้อมูลเพิ่มเติม</h2>
            <div className="divider my-1"></div>
            <p><span className="font-semibold">วันที่สร้าง:</span> {formatThaiDate(new Date(certificate.created_at))}</p>
            {certificate.updated_at && (
              <p><span className="font-semibold">วันที่แก้ไขล่าสุด:</span> {formatThaiDate(new Date(certificate.updated_at))}</p>
            )}
            <p><span className="font-semibold">สร้างโดย:</span> รหัสผู้ใช้ {certificate.created_by_id}</p>
            {certificate.updated_by_id && (
              <p><span className="font-semibold">แก้ไขล่าสุดโดย:</span> รหัสผู้ใช้ {certificate.updated_by_id}</p>
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
            onClick={() => navigate(`/medical-documents/certificates/${id}/edit`)}
          >
            แก้ไขใบรับรองแพทย์
          </button>
        </div>
      </div>
    </div>
  );
};

export default MedicalCertificateDetail; 