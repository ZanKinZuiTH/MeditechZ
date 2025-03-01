import { useState } from 'react';
import axios from 'axios';

// กำหนดประเภทข้อมูลสำหรับเอกสารทางการแพทย์
export interface MedicalDocument {
  id: number;
  patient_id: number;
  doctor_id: number;
  visit_id?: number;
  document_date: string;
  document_type: string;
  document_status: string;
  notes?: string;
  created_at: string;
  updated_at?: string;
  created_by_id: number;
  updated_by_id?: number;
}

// กำหนดประเภทข้อมูลสำหรับใบรับรองแพทย์
export interface MedicalCertificate {
  id: number;
  patient_id: number;
  doctor_id: number;
  visit_id?: number;
  certificate_date: string;
  certificate_type: string;
  diagnosis?: string;
  treatment?: string;
  rest_period_days?: number;
  rest_period_start?: string;
  rest_period_end?: string;
  doctor_license_no?: string;
  doctor_license_issue_date?: string;
  doctor2_id?: number;
  doctor2_license_no?: string;
  doctor2_license_issue_date?: string;
  vital_signs?: Record<string, any>;
  comments?: string;
  created_at: string;
  updated_at?: string;
  created_by_id: number;
  updated_by_id?: number;
}

// กำหนดประเภทข้อมูลสำหรับสมุดตรวจสุขภาพ
export interface HealthCheckupBook {
  id: number;
  patient_id: number;
  doctor_id: number;
  visit_id?: number;
  checkup_date: string;
  checkup_type: string;
  vital_signs?: Record<string, any>;
  physical_exam?: Record<string, any>;
  lab_results?: Record<string, any>;
  radiology_results?: Record<string, any>;
  ekg_results?: Record<string, any>;
  spirometry_results?: Record<string, any>;
  audiometry_results?: Record<string, any>;
  vision_test_results?: Record<string, any>;
  conclusion?: string;
  recommendations?: string;
  doctor_license_no?: string;
  doctor_signature?: string;
  created_at: string;
  updated_at?: string;
  created_by_id: number;
  updated_by_id?: number;
}

// กำหนดประเภทข้อมูลสำหรับการสร้างเอกสารใหม่
export type MedicalDocumentCreate = Omit<MedicalDocument, 'id' | 'created_at' | 'updated_at' | 'created_by_id' | 'updated_by_id'>;
export type MedicalCertificateCreate = Omit<MedicalCertificate, 'id' | 'created_at' | 'updated_at' | 'created_by_id' | 'updated_by_id'>;
export type HealthCheckupBookCreate = Omit<HealthCheckupBook, 'id' | 'created_at' | 'updated_at' | 'created_by_id' | 'updated_by_id'>;

// กำหนดประเภทข้อมูลสำหรับการอัปเดตเอกสาร
export type MedicalDocumentUpdate = Partial<MedicalDocumentCreate>;
export type MedicalCertificateUpdate = Partial<MedicalCertificateCreate>;
export type HealthCheckupBookUpdate = Partial<HealthCheckupBookCreate>;

// กำหนดประเภทข้อมูลสำหรับพารามิเตอร์การค้นหา
export interface MedicalDocumentSearchParams {
  patient_id?: number;
  doctor_id?: number;
  visit_id?: number;
  document_type?: string;
  start_date?: string;
  end_date?: string;
  skip?: number;
  limit?: number;
}

export interface MedicalCertificateSearchParams {
  patient_id?: number;
  doctor_id?: number;
  visit_id?: number;
  certificate_type?: string;
  start_date?: string;
  end_date?: string;
  skip?: number;
  limit?: number;
}

export interface HealthCheckupBookSearchParams {
  patient_id?: number;
  doctor_id?: number;
  visit_id?: number;
  checkup_type?: string;
  start_date?: string;
  end_date?: string;
  skip?: number;
  limit?: number;
}

// API URL
const API_URL = 'http://localhost:8000/api/v1';

// Hook สำหรับเชื่อมต่อกับ API เอกสารทางการแพทย์
export const useMedicalDocumentsApi = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // ฟังก์ชันสำหรับดึงข้อมูลเอกสารทางการแพทย์ทั้งหมด
  const getMedicalDocuments = async (params?: MedicalDocumentSearchParams) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get<MedicalDocument[]>(`${API_URL}/medical-documents/`, { params });
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการดึงข้อมูลเอกสารทางการแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับดึงข้อมูลเอกสารทางการแพทย์ตาม ID
  const getMedicalDocumentById = async (id: number) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get<MedicalDocument>(`${API_URL}/medical-documents/${id}`);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการดึงข้อมูลเอกสารทางการแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับสร้างเอกสารทางการแพทย์ใหม่
  const createMedicalDocument = async (data: MedicalDocumentCreate) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.post<MedicalDocument>(`${API_URL}/medical-documents/`, data);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการสร้างเอกสารทางการแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับอัปเดตเอกสารทางการแพทย์
  const updateMedicalDocument = async (id: number, data: MedicalDocumentUpdate) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.put<MedicalDocument>(`${API_URL}/medical-documents/${id}`, data);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการอัปเดตเอกสารทางการแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับลบเอกสารทางการแพทย์
  const deleteMedicalDocument = async (id: number) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.delete<MedicalDocument>(`${API_URL}/medical-documents/${id}`);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการลบเอกสารทางการแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับดึงข้อมูลใบรับรองแพทย์ทั้งหมด
  const getMedicalCertificates = async (params?: MedicalCertificateSearchParams) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get<MedicalCertificate[]>(`${API_URL}/medical-documents/certificates/`, { params });
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการดึงข้อมูลใบรับรองแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับดึงข้อมูลใบรับรองแพทย์ตาม ID
  const getMedicalCertificateById = async (id: number) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get<MedicalCertificate>(`${API_URL}/medical-documents/certificates/${id}`);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการดึงข้อมูลใบรับรองแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับสร้างใบรับรองแพทย์ใหม่
  const createMedicalCertificate = async (data: MedicalCertificateCreate) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.post<MedicalCertificate>(`${API_URL}/medical-documents/certificates/`, data);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการสร้างใบรับรองแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับอัปเดตใบรับรองแพทย์
  const updateMedicalCertificate = async (id: number, data: MedicalCertificateUpdate) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.put<MedicalCertificate>(`${API_URL}/medical-documents/certificates/${id}`, data);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการอัปเดตใบรับรองแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับลบใบรับรองแพทย์
  const deleteMedicalCertificate = async (id: number) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.delete<MedicalCertificate>(`${API_URL}/medical-documents/certificates/${id}`);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการลบใบรับรองแพทย์';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับดึงข้อมูลสมุดตรวจสุขภาพทั้งหมด
  const getHealthCheckupBooks = async (params?: HealthCheckupBookSearchParams) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get<HealthCheckupBook[]>(`${API_URL}/medical-documents/checkup-books/`, { params });
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการดึงข้อมูลสมุดตรวจสุขภาพ';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับดึงข้อมูลสมุดตรวจสุขภาพตาม ID
  const getHealthCheckupBookById = async (id: number) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.get<HealthCheckupBook>(`${API_URL}/medical-documents/checkup-books/${id}`);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการดึงข้อมูลสมุดตรวจสุขภาพ';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับสร้างสมุดตรวจสุขภาพใหม่
  const createHealthCheckupBook = async (data: HealthCheckupBookCreate) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.post<HealthCheckupBook>(`${API_URL}/medical-documents/checkup-books/`, data);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการสร้างสมุดตรวจสุขภาพ';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับอัปเดตสมุดตรวจสุขภาพ
  const updateHealthCheckupBook = async (id: number, data: HealthCheckupBookUpdate) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.put<HealthCheckupBook>(`${API_URL}/medical-documents/checkup-books/${id}`, data);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการอัปเดตสมุดตรวจสุขภาพ';
      setError(errorMessage);
      throw err;
    }
  };

  // ฟังก์ชันสำหรับลบสมุดตรวจสุขภาพ
  const deleteHealthCheckupBook = async (id: number) => {
    setLoading(true);
    setError(null);
    try {
      const response = await axios.delete<HealthCheckupBook>(`${API_URL}/medical-documents/checkup-books/${id}`);
      setLoading(false);
      return response.data;
    } catch (err) {
      setLoading(false);
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการลบสมุดตรวจสุขภาพ';
      setError(errorMessage);
      throw err;
    }
  };

  return {
    loading,
    error,
    // เอกสารทางการแพทย์ทั่วไป
    getMedicalDocuments,
    getMedicalDocumentById,
    createMedicalDocument,
    updateMedicalDocument,
    deleteMedicalDocument,
    // ใบรับรองแพทย์
    getMedicalCertificates,
    getMedicalCertificateById,
    createMedicalCertificate,
    updateMedicalCertificate,
    deleteMedicalCertificate,
    // สมุดตรวจสุขภาพ
    getHealthCheckupBooks,
    getHealthCheckupBookById,
    createHealthCheckupBook,
    updateHealthCheckupBook,
    deleteHealthCheckupBook,
  };
};

export default useMedicalDocumentsApi; 