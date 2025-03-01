import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import useMedicalDocumentsApi, { MedicalCertificate, MedicalCertificateSearchParams } from '../hooks/useMedicalDocumentsApi';

interface MedicalCertificateListProps {
  patientId?: number;
  doctorId?: number;
  visitId?: number;
}

const MedicalCertificateList: React.FC<MedicalCertificateListProps> = ({ patientId, doctorId, visitId }) => {
  const { 
    loading, 
    error, 
    getMedicalCertificates, 
    deleteMedicalCertificate 
  } = useMedicalDocumentsApi();
  
  const [certificates, setCertificates] = useState<MedicalCertificate[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [certificateType, setCertificateType] = useState<string>('');
  const [startDate, setStartDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [certificateToDelete, setCertificateToDelete] = useState<number | null>(null);
  const [showFilters, setShowFilters] = useState(false);

  // ฟังก์ชันสำหรับโหลดข้อมูลใบรับรองแพทย์
  const loadCertificates = async () => {
    try {
      const params: MedicalCertificateSearchParams = {};
      
      if (patientId) params.patient_id = patientId;
      if (doctorId) params.doctor_id = doctorId;
      if (visitId) params.visit_id = visitId;
      if (certificateType) params.certificate_type = certificateType;
      if (startDate) params.start_date = startDate;
      if (endDate) params.end_date = endDate;
      
      const data = await getMedicalCertificates(params);
      setCertificates(data);
    } catch (err) {
      console.error('เกิดข้อผิดพลาดในการโหลดข้อมูลใบรับรองแพทย์:', err);
    }
  };

  // โหลดข้อมูลเมื่อคอมโพเนนต์ถูกโหลด
  useEffect(() => {
    loadCertificates();
  }, [patientId, doctorId, visitId, certificateType, startDate, endDate]);

  // ฟังก์ชันสำหรับเปิดไดอะล็อกยืนยันการลบ
  const handleOpenDeleteDialog = (id: number) => {
    setCertificateToDelete(id);
    setDeleteDialogOpen(true);
  };

  // ฟังก์ชันสำหรับปิดไดอะล็อกยืนยันการลบ
  const handleCloseDeleteDialog = () => {
    setDeleteDialogOpen(false);
    setCertificateToDelete(null);
  };

  // ฟังก์ชันสำหรับลบใบรับรองแพทย์
  const handleDeleteCertificate = async () => {
    if (certificateToDelete) {
      try {
        await deleteMedicalCertificate(certificateToDelete);
        setDeleteDialogOpen(false);
        setCertificateToDelete(null);
        loadCertificates();
      } catch (err) {
        console.error('เกิดข้อผิดพลาดในการลบใบรับรองแพทย์:', err);
      }
    }
  };

  // ฟังก์ชันสำหรับเปลี่ยนประเภทใบรับรองแพทย์
  const handleCertificateTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setCertificateType(event.target.value);
  };

  // ฟังก์ชันสำหรับรีเซ็ตตัวกรอง
  const handleResetFilters = () => {
    setCertificateType('');
    setStartDate('');
    setEndDate('');
  };

  // กรองใบรับรองแพทย์ตามคำค้นหา
  const filteredCertificates = certificates.filter(cert => 
    cert.certificate_type.toLowerCase().includes(searchTerm.toLowerCase()) ||
    cert.diagnosis?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    cert.treatment?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    cert.comments?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // ฟังก์ชันสำหรับแสดงประเภทใบรับรองแพทย์
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

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-xl font-semibold">รายการใบรับรองแพทย์</h2>
        <div>
          <button 
            className="btn btn-outline mr-2"
            onClick={() => setShowFilters(!showFilters)}
          >
            {showFilters ? 'ซ่อนตัวกรอง' : 'แสดงตัวกรอง'}
          </button>
          <Link 
            to="/medical-documents/certificates/create"
            className="btn btn-primary"
          >
            สร้างใบรับรองแพทย์ใหม่
          </Link>
        </div>
      </div>

      {showFilters && (
        <div className="bg-base-200 p-4 rounded-lg mb-4">
          <h3 className="text-lg font-medium mb-2">ตัวกรอง</h3>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            <div>
              <label className="label">ประเภทใบรับรองแพทย์</label>
              <select 
                className="select select-bordered w-full"
                value={certificateType}
                onChange={handleCertificateTypeChange}
              >
                <option value="">ทั้งหมด</option>
                <option value="sick_leave">ใบรับรองแพทย์ลาป่วย</option>
                <option value="health_certificate">ใบรับรองแพทย์ตรวจสุขภาพ</option>
                <option value="disability_certificate">ใบรับรองความพิการ</option>
                <option value="fitness_certificate">ใบรับรองความสมบูรณ์ทางร่างกาย</option>
                <option value="other">อื่นๆ</option>
              </select>
            </div>
            <div>
              <label className="label">วันที่เริ่มต้น</label>
              <input 
                type="date" 
                className="input input-bordered w-full"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
              />
            </div>
            <div>
              <label className="label">วันที่สิ้นสุด</label>
              <input 
                type="date" 
                className="input input-bordered w-full"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
              />
            </div>
            <div className="flex items-end">
              <button 
                className="btn btn-outline w-full"
                onClick={handleResetFilters}
              >
                รีเซ็ต
              </button>
            </div>
          </div>
        </div>
      )}

      <div className="mb-4">
        <input 
          type="text" 
          placeholder="ค้นหาใบรับรองแพทย์..." 
          className="input input-bordered w-full"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      {loading ? (
        <div className="text-center py-4">กำลังโหลดข้อมูล...</div>
      ) : error ? (
        <div className="alert alert-error">เกิดข้อผิดพลาด: {error}</div>
      ) : filteredCertificates.length === 0 ? (
        <div className="text-center py-4">ไม่พบใบรับรองแพทย์</div>
      ) : (
        <div className="overflow-x-auto">
          <table className="table w-full">
            <thead>
              <tr>
                <th>ประเภท</th>
                <th>วันที่</th>
                <th>การวินิจฉัย</th>
                <th>ระยะเวลาพักฟื้น</th>
                <th>การดำเนินการ</th>
              </tr>
            </thead>
            <tbody>
              {filteredCertificates.map((certificate) => (
                <tr key={certificate.id}>
                  <td>{renderCertificateType(certificate.certificate_type)}</td>
                  <td>{new Date(certificate.certificate_date).toLocaleDateString('th-TH')}</td>
                  <td>{certificate.diagnosis || '-'}</td>
                  <td>
                    {certificate.rest_period_days 
                      ? `${certificate.rest_period_days} วัน` 
                      : '-'}
                  </td>
                  <td>
                    <div className="flex space-x-2">
                      <Link 
                        to={`/medical-documents/certificates/${certificate.id}`}
                        className="btn btn-sm btn-info"
                      >
                        ดู
                      </Link>
                      <Link 
                        to={`/medical-documents/certificates/${certificate.id}/edit`}
                        className="btn btn-sm btn-warning"
                      >
                        แก้ไข
                      </Link>
                      <button 
                        className="btn btn-sm btn-error"
                        onClick={() => handleOpenDeleteDialog(certificate.id)}
                      >
                        ลบ
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* ไดอะล็อกยืนยันการลบ */}
      {deleteDialogOpen && (
        <div className="modal modal-open">
          <div className="modal-box">
            <h3 className="font-bold text-lg">ยืนยันการลบใบรับรองแพทย์</h3>
            <p className="py-4">คุณต้องการลบใบรับรองแพทย์นี้ใช่หรือไม่? การดำเนินการนี้ไม่สามารถเรียกคืนได้</p>
            <div className="modal-action">
              <button className="btn" onClick={handleCloseDeleteDialog}>ยกเลิก</button>
              <button className="btn btn-error" onClick={handleDeleteCertificate}>ลบ</button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default MedicalCertificateList; 