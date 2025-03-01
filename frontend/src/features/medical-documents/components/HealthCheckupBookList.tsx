import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import useMedicalDocumentsApi, { HealthCheckupBook, HealthCheckupBookSearchParams } from '../hooks/useMedicalDocumentsApi';

interface HealthCheckupBookListProps {
  patientId?: number;
  doctorId?: number;
  visitId?: number;
}

const HealthCheckupBookList: React.FC<HealthCheckupBookListProps> = ({ patientId, doctorId, visitId }) => {
  const { 
    loading, 
    error, 
    getHealthCheckupBooks, 
    deleteHealthCheckupBook 
  } = useMedicalDocumentsApi();
  
  const [books, setBooks] = useState<HealthCheckupBook[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [checkupType, setCheckupType] = useState<string>('');
  const [startDate, setStartDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [bookToDelete, setBookToDelete] = useState<number | null>(null);
  const [showFilters, setShowFilters] = useState(false);

  // ฟังก์ชันสำหรับโหลดข้อมูลสมุดตรวจสุขภาพ
  const loadBooks = async () => {
    try {
      const params: HealthCheckupBookSearchParams = {};
      
      if (patientId) params.patient_id = patientId;
      if (doctorId) params.doctor_id = doctorId;
      if (visitId) params.visit_id = visitId;
      if (checkupType) params.checkup_type = checkupType;
      if (startDate) params.start_date = startDate;
      if (endDate) params.end_date = endDate;
      
      const data = await getHealthCheckupBooks(params);
      setBooks(data);
    } catch (err) {
      console.error('เกิดข้อผิดพลาดในการโหลดข้อมูลสมุดตรวจสุขภาพ:', err);
    }
  };

  // โหลดข้อมูลเมื่อคอมโพเนนต์ถูกโหลด
  useEffect(() => {
    loadBooks();
  }, [patientId, doctorId, visitId, checkupType, startDate, endDate]);

  // ฟังก์ชันสำหรับเปิดไดอะล็อกยืนยันการลบ
  const handleOpenDeleteDialog = (id: number) => {
    setBookToDelete(id);
    setDeleteDialogOpen(true);
  };

  // ฟังก์ชันสำหรับปิดไดอะล็อกยืนยันการลบ
  const handleCloseDeleteDialog = () => {
    setDeleteDialogOpen(false);
    setBookToDelete(null);
  };

  // ฟังก์ชันสำหรับลบสมุดตรวจสุขภาพ
  const handleDeleteBook = async () => {
    if (bookToDelete) {
      try {
        await deleteHealthCheckupBook(bookToDelete);
        setDeleteDialogOpen(false);
        setBookToDelete(null);
        loadBooks(); // โหลดข้อมูลใหม่หลังจากลบ
      } catch (err) {
        console.error('เกิดข้อผิดพลาดในการลบสมุดตรวจสุขภาพ:', err);
      }
    }
  };

  // ฟังก์ชันสำหรับเปลี่ยนประเภทการตรวจสุขภาพ
  const handleCheckupTypeChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
    setCheckupType(event.target.value);
  };

  // ฟังก์ชันสำหรับรีเซ็ตตัวกรอง
  const handleResetFilters = () => {
    setCheckupType('');
    setStartDate('');
    setEndDate('');
  };

  // กรองสมุดตรวจสุขภาพตามคำค้นหา
  const filteredBooks = books.filter(book => 
    book.checkup_type.toLowerCase().includes(searchTerm.toLowerCase()) ||
    book.conclusion?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    book.recommendations?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // ฟังก์ชันสำหรับแสดงประเภทการตรวจสุขภาพ
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

  return (
    <div className="container mx-auto p-4">
      <div className="flex justify-between items-center mb-4">
        <h2 className="text-xl font-semibold">รายการสมุดตรวจสุขภาพ</h2>
        <div>
          <button 
            className="btn btn-outline mr-2"
            onClick={() => setShowFilters(!showFilters)}
          >
            {showFilters ? 'ซ่อนตัวกรอง' : 'แสดงตัวกรอง'}
          </button>
          <Link 
            to="/medical-documents/checkup-books/create"
            className="btn btn-primary"
          >
            สร้างสมุดตรวจสุขภาพใหม่
          </Link>
        </div>
      </div>

      {showFilters && (
        <div className="bg-base-200 p-4 rounded-lg mb-4">
          <h3 className="text-lg font-medium mb-2">ตัวกรอง</h3>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            <div>
              <label className="label">ประเภทการตรวจสุขภาพ</label>
              <select 
                className="select select-bordered w-full"
                value={checkupType}
                onChange={handleCheckupTypeChange}
              >
                <option value="">ทั้งหมด</option>
                <option value="annual">ตรวจสุขภาพประจำปี</option>
                <option value="pre_employment">ตรวจสุขภาพก่อนเข้าทำงาน</option>
                <option value="insurance">ตรวจสุขภาพเพื่อทำประกัน</option>
                <option value="driver_license">ตรวจสุขภาพเพื่อทำใบขับขี่</option>
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
          placeholder="ค้นหาสมุดตรวจสุขภาพ..." 
          className="input input-bordered w-full"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </div>

      {loading ? (
        <div className="text-center py-4">กำลังโหลดข้อมูล...</div>
      ) : error ? (
        <div className="alert alert-error">เกิดข้อผิดพลาด: {error}</div>
      ) : filteredBooks.length === 0 ? (
        <div className="text-center py-4">ไม่พบสมุดตรวจสุขภาพ</div>
      ) : (
        <div className="overflow-x-auto">
          <table className="table w-full">
            <thead>
              <tr>
                <th>รหัส</th>
                <th>วันที่ตรวจ</th>
                <th>ประเภทการตรวจ</th>
                <th>ผลสรุป</th>
                <th>การจัดการ</th>
              </tr>
            </thead>
            <tbody>
              {filteredBooks.map((book) => (
                <tr key={book.id}>
                  <td>{book.id}</td>
                  <td>{new Date(book.checkup_date).toLocaleDateString('th-TH')}</td>
                  <td>{renderCheckupType(book.checkup_type)}</td>
                  <td>
                    {book.conclusion ? (
                      book.conclusion.length > 50 ? 
                        `${book.conclusion.substring(0, 50)}...` : 
                        book.conclusion
                    ) : (
                      <span className="text-gray-400">ไม่มีข้อมูล</span>
                    )}
                  </td>
                  <td>
                    <div className="flex space-x-2">
                      <Link 
                        to={`/medical-documents/checkup-books/${book.id}`}
                        className="btn btn-info btn-xs"
                      >
                        ดู
                      </Link>
                      <Link 
                        to={`/medical-documents/checkup-books/${book.id}/edit`}
                        className="btn btn-warning btn-xs"
                      >
                        แก้ไข
                      </Link>
                      <button 
                        className="btn btn-error btn-xs"
                        onClick={() => handleOpenDeleteDialog(book.id)}
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

      {/* กล่องยืนยันการลบ */}
      {deleteDialogOpen && (
        <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
          <div className="bg-white p-6 rounded-lg max-w-md w-full">
            <h3 className="text-lg font-bold mb-4">ยืนยันการลบ</h3>
            <p className="mb-4">คุณแน่ใจหรือไม่ว่าต้องการลบสมุดตรวจสุขภาพนี้? การกระทำนี้ไม่สามารถย้อนกลับได้</p>
            <div className="flex justify-end space-x-2">
              <button 
                className="btn btn-outline"
                onClick={handleCloseDeleteDialog}
              >
                ยกเลิก
              </button>
              <button 
                className="btn btn-error"
                onClick={handleDeleteBook}
              >
                ลบ
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default HealthCheckupBookList; 