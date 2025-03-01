import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { formatThaiDate } from '../../../lib/utils';
import useMedicalDocumentsApi, { HealthCheckupBook } from '../hooks/useMedicalDocumentsApi';

const HealthCheckupBookDetail: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { getHealthCheckupBookById, loading, error: apiError } = useMedicalDocumentsApi();
  
  const [book, setBook] = useState<HealthCheckupBook | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (id) {
      const fetchHealthCheckupBook = async () => {
        try {
          const data = await getHealthCheckupBookById(parseInt(id));
          setBook(data);
        } catch (err) {
          console.error('เกิดข้อผิดพลาดในการดึงข้อมูลสมุดตรวจสุขภาพ:', err);
          setError('ไม่สามารถโหลดข้อมูลสมุดตรวจสุขภาพได้ โปรดลองอีกครั้งในภายหลัง');
        }
      };

      fetchHealthCheckupBook();
    }
  }, [id, getHealthCheckupBookById]);

  const renderCheckupType = (type: string) => {
    switch (type) {
      case 'annual':
        return 'ตรวจสุขภาพประจำปี';
      case 'pre_employment':
        return 'ตรวจสุขภาพก่อนเข้าทำงาน';
      case 'insurance':
        return 'ตรวจสุขภาพเพื่อทำประกัน';
      case 'driver_license':
        return 'ตรวจสุขภาพเพื่อทำใบขับขี่';
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

  if (!book) {
    return (
      <div className="p-4">
        <div className="alert alert-warning mb-4">
          <svg xmlns="http://www.w3.org/2000/svg" className="stroke-current shrink-0 h-6 w-6" fill="none" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" /></svg>
          <span>ไม่พบข้อมูลสมุดตรวจสุขภาพ</span>
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
          <h1 className="text-2xl font-bold">รายละเอียดสมุดตรวจสุขภาพ</h1>
          <div className="flex space-x-2">
            <button
              className="btn btn-outline"
              onClick={() => navigate('/medical-documents')}
            >
              กลับ
            </button>
            <button
              className="btn btn-warning"
              onClick={() => navigate(`/medical-documents/checkup-books/${id}/edit`)}
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
              <p><span className="font-semibold">รหัสผู้ป่วย:</span> {book.patient_id}</p>
              <p><span className="font-semibold">รหัสแพทย์:</span> {book.doctor_id}</p>
              {book.visit_id && <p><span className="font-semibold">รหัสการเข้ารับบริการ:</span> {book.visit_id}</p>}
              <p><span className="font-semibold">วันที่ตรวจ:</span> {formatThaiDate(new Date(book.checkup_date))}</p>
              <p><span className="font-semibold">ประเภทการตรวจ:</span> {renderCheckupType(book.checkup_type)}</p>
            </div>
          </div>

          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">สัญญาณชีพ</h2>
              <div className="divider my-1"></div>
              {book.vital_signs && Object.keys(book.vital_signs).length > 0 ? (
                <div className="grid grid-cols-2 gap-2">
                  {book.vital_signs.temperature && (
                    <p><span className="font-semibold">อุณหภูมิ:</span> {book.vital_signs.temperature} °C</p>
                  )}
                  {book.vital_signs.pulse && (
                    <p><span className="font-semibold">ชีพจร:</span> {book.vital_signs.pulse} ครั้ง/นาที</p>
                  )}
                  {book.vital_signs.respiratory_rate && (
                    <p><span className="font-semibold">อัตราการหายใจ:</span> {book.vital_signs.respiratory_rate} ครั้ง/นาที</p>
                  )}
                  {book.vital_signs.blood_pressure && (
                    <p><span className="font-semibold">ความดันโลหิต:</span> {book.vital_signs.blood_pressure} mmHg</p>
                  )}
                  {book.vital_signs.weight && (
                    <p><span className="font-semibold">น้ำหนัก:</span> {book.vital_signs.weight} กก.</p>
                  )}
                  {book.vital_signs.height && (
                    <p><span className="font-semibold">ส่วนสูง:</span> {book.vital_signs.height} ซม.</p>
                  )}
                  {book.vital_signs.bmi && (
                    <p><span className="font-semibold">ดัชนีมวลกาย (BMI):</span> {book.vital_signs.bmi}</p>
                  )}
                </div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลสัญญาณชีพ</p>
              )}
            </div>
          </div>
        </div>

        <div className="card bg-base-100 shadow-sm mb-6">
          <div className="card-body">
            <h2 className="card-title">การตรวจร่างกาย</h2>
            <div className="divider my-1"></div>
            {book.physical_exam ? (
              <div className="whitespace-pre-line">{book.physical_exam}</div>
            ) : (
              <p className="text-gray-500">ไม่มีข้อมูลการตรวจร่างกาย</p>
            )}
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ผลการตรวจทางห้องปฏิบัติการ</h2>
              <div className="divider my-1"></div>
              {book.lab_results ? (
                <div className="whitespace-pre-line">{book.lab_results}</div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลผลการตรวจทางห้องปฏิบัติการ</p>
              )}
            </div>
          </div>

          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ผลการตรวจทางรังสี</h2>
              <div className="divider my-1"></div>
              {book.radiology_results ? (
                <div className="whitespace-pre-line">{book.radiology_results}</div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลผลการตรวจทางรังสี</p>
              )}
            </div>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ผลการตรวจคลื่นไฟฟ้าหัวใจ (EKG)</h2>
              <div className="divider my-1"></div>
              {book.ekg_results ? (
                <div className="whitespace-pre-line">{book.ekg_results}</div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลผลการตรวจคลื่นไฟฟ้าหัวใจ</p>
              )}
            </div>
          </div>

          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ผลการตรวจสมรรถภาพปอด</h2>
              <div className="divider my-1"></div>
              {book.spirometry_results ? (
                <div className="whitespace-pre-line">{book.spirometry_results}</div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลผลการตรวจสมรรถภาพปอด</p>
              )}
            </div>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ผลการตรวจการได้ยิน</h2>
              <div className="divider my-1"></div>
              {book.audiometry_results ? (
                <div className="whitespace-pre-line">{book.audiometry_results}</div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลผลการตรวจการได้ยิน</p>
              )}
            </div>
          </div>

          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ผลการตรวจสายตา</h2>
              <div className="divider my-1"></div>
              {book.vision_test_results ? (
                <div className="whitespace-pre-line">{book.vision_test_results}</div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลผลการตรวจสายตา</p>
              )}
            </div>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 mb-6">
          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">ข้อสรุป</h2>
              <div className="divider my-1"></div>
              {book.conclusion ? (
                <div className="whitespace-pre-line">{book.conclusion}</div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลข้อสรุป</p>
              )}
            </div>
          </div>

          <div className="card bg-base-100 shadow-sm">
            <div className="card-body">
              <h2 className="card-title">คำแนะนำ</h2>
              <div className="divider my-1"></div>
              {book.recommendations ? (
                <div className="whitespace-pre-line">{book.recommendations}</div>
              ) : (
                <p className="text-gray-500">ไม่มีข้อมูลคำแนะนำ</p>
              )}
            </div>
          </div>
        </div>

        <div className="card bg-base-100 shadow-sm mb-6">
          <div className="card-body">
            <h2 className="card-title">ข้อมูลแพทย์ผู้ตรวจ</h2>
            <div className="divider my-1"></div>
            <p><span className="font-semibold">เลขที่ใบอนุญาตประกอบวิชาชีพเวชกรรม:</span> {book.doctor_license_no || 'ไม่ระบุ'}</p>
            {book.doctor_signature && (
              <div className="mt-2">
                <p className="font-semibold">ลายเซ็นแพทย์:</p>
                <img 
                  src={book.doctor_signature} 
                  alt="ลายเซ็นแพทย์" 
                  className="max-w-xs mt-2 border border-gray-200 p-2"
                />
              </div>
            )}
          </div>
        </div>

        <div className="card bg-base-100 shadow-sm mb-6">
          <div className="card-body">
            <h2 className="card-title">ข้อมูลเพิ่มเติม</h2>
            <div className="divider my-1"></div>
            <p><span className="font-semibold">วันที่สร้าง:</span> {formatThaiDate(new Date(book.created_at))}</p>
            {book.updated_at && (
              <p><span className="font-semibold">วันที่แก้ไขล่าสุด:</span> {formatThaiDate(new Date(book.updated_at))}</p>
            )}
            <p><span className="font-semibold">สร้างโดย:</span> รหัสผู้ใช้ {book.created_by_id}</p>
            {book.updated_by_id && (
              <p><span className="font-semibold">แก้ไขล่าสุดโดย:</span> รหัสผู้ใช้ {book.updated_by_id}</p>
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
            onClick={() => navigate(`/medical-documents/checkup-books/${id}/edit`)}
          >
            แก้ไขสมุดตรวจสุขภาพ
          </button>
        </div>
      </div>
    </div>
  );
};

export default HealthCheckupBookDetail; 