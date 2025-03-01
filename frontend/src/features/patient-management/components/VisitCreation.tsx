/**
 * Visit Creation Component
 * 
 * คอมโพเนนต์สำหรับสร้างการเข้ารับบริการในระบบ MeditechZ
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React, { useState, useEffect } from 'react';
import { 
  Card, 
  CardContent 
} from '@/components/ui/card';
import { formatThaiDate, formatThaiTime } from '@/lib/utils';

// กำหนด interface สำหรับข้อมูลผู้ป่วย
interface PatientData {
  id?: number;
  first_name: string;
  last_name: string;
  id_card_number: string;
  date_of_birth: string;
  gender: string;
  blood_type?: string;
  address?: string;
  phone_number?: string;
  email?: string;
  emergency_contact_name?: string;
  emergency_contact_phone?: string;
  allergies?: string;
  chronic_diseases?: string;
}

// กำหนด interface สำหรับข้อมูลแพทย์
interface DoctorData {
  id: number;
  name: string;
  specialty: string;
  department: string;
}

// กำหนด interface สำหรับข้อมูลการเข้ารับบริการ
interface VisitData {
  patient_id?: number;
  doctor_id?: number;
  appointment_datetime: string;
  end_datetime: string;
  reason: string;
  notes?: string;
}

interface VisitCreationProps {
  visitData: VisitData;
  setVisitData: React.Dispatch<React.SetStateAction<VisitData>>;
  patientData: PatientData;
}

const VisitCreation: React.FC<VisitCreationProps> = ({ 
  visitData, 
  setVisitData,
  patientData
}) => {
  const [doctors, setDoctors] = useState<DoctorData[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);
  
  // ฟังก์ชันสำหรับการอัปเดตข้อมูล
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setVisitData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };
  
  // ฟังก์ชันสำหรับการอัปเดตวันที่และเวลา
  const handleDateTimeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    
    if (name === 'appointment_date') {
      // แยกเวลาจากค่าเดิม
      const currentDateTime = new Date(visitData.appointment_datetime);
      const hours = currentDateTime.getHours();
      const minutes = currentDateTime.getMinutes();
      
      // สร้างวันที่และเวลาใหม่
      const newDate = new Date(value);
      newDate.setHours(hours);
      newDate.setMinutes(minutes);
      
      setVisitData((prev) => ({
        ...prev,
        appointment_datetime: newDate.toISOString(),
      }));
    } else if (name === 'appointment_time') {
      // แยกวันที่จากค่าเดิม
      const currentDateTime = new Date(visitData.appointment_datetime);
      const year = currentDateTime.getFullYear();
      const month = currentDateTime.getMonth();
      const day = currentDateTime.getDate();
      
      // แยกชั่วโมงและนาทีจากค่าใหม่
      const [hours, minutes] = value.split(':').map(Number);
      
      // สร้างวันที่และเวลาใหม่
      const newDateTime = new Date(year, month, day, hours, minutes);
      
      setVisitData((prev) => ({
        ...prev,
        appointment_datetime: newDateTime.toISOString(),
      }));
      
      // คำนวณเวลาสิ้นสุดโดยเพิ่ม 30 นาที
      const endDateTime = new Date(newDateTime);
      endDateTime.setMinutes(endDateTime.getMinutes() + 30);
      
      setVisitData((prev) => ({
        ...prev,
        end_datetime: endDateTime.toISOString(),
      }));
    }
  };
  
  // ดึงข้อมูลแพทย์เมื่อคอมโพเนนต์ถูกโหลด
  useEffect(() => {
    const fetchDoctors = async () => {
      setIsLoading(true);
      
      try {
        const response = await fetch('/api/v1/users/doctors');
        
        if (!response.ok) {
          throw new Error('เกิดข้อผิดพลาดในการดึงข้อมูลแพทย์');
        }
        
        const data = await response.json();
        setDoctors(data);
      } catch (error) {
        console.error('Error fetching doctors:', error);
        // จำลองข้อมูลสำหรับการพัฒนา
        setDoctors([
          { id: 1, name: 'นพ. สมชาย ใจดี', specialty: 'อายุรแพทย์', department: 'อายุรกรรม' },
          { id: 2, name: 'พญ. สมหญิง รักดี', specialty: 'กุมารแพทย์', department: 'กุมารเวชกรรม' },
          { id: 3, name: 'นพ. มานะ สุขใจ', specialty: 'ศัลยแพทย์', department: 'ศัลยกรรม' },
        ]);
      } finally {
        setIsLoading(false);
      }
    };
    
    fetchDoctors();
    
    // ตั้งค่า patient_id จาก props
    if (patientData.id) {
      setVisitData((prev) => ({
        ...prev,
        patient_id: patientData.id,
      }));
    }
  }, [patientData.id]);
  
  // แปลงวันที่และเวลาสำหรับแสดงผล
  const appointmentDateTime = new Date(visitData.appointment_datetime);
  const appointmentDate = appointmentDateTime.toISOString().split('T')[0];
  const appointmentTime = appointmentDateTime.toTimeString().slice(0, 5);
  
  return (
    <div className="space-y-4">
      <Card>
        <CardContent className="p-6">
          <div className="mb-4">
            <h3 className="text-lg font-medium">ข้อมูลผู้ป่วย</h3>
            <div className="mt-2 p-3 bg-gray-50 rounded-md">
              <p><strong>ชื่อ-นามสกุล:</strong> {patientData.first_name} {patientData.last_name}</p>
              <p><strong>เลขบัตรประชาชน:</strong> {patientData.id_card_number}</p>
              <p><strong>วันเกิด:</strong> {formatThaiDate(new Date(patientData.date_of_birth))}</p>
            </div>
          </div>
          
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mt-6">
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                แพทย์ผู้ตรวจ <span className="text-red-500">*</span>
              </label>
              <select
                name="doctor_id"
                value={visitData.doctor_id || ''}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
                required
              >
                <option value="">เลือกแพทย์</option>
                {doctors.map((doctor) => (
                  <option key={doctor.id} value={doctor.id}>
                    {doctor.name} ({doctor.specialty})
                  </option>
                ))}
              </select>
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                วันที่นัดหมาย <span className="text-red-500">*</span>
              </label>
              <input
                type="date"
                name="appointment_date"
                value={appointmentDate}
                onChange={handleDateTimeChange}
                className="w-full px-3 py-2 border rounded-md"
                required
              />
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                เวลานัดหมาย <span className="text-red-500">*</span>
              </label>
              <input
                type="time"
                name="appointment_time"
                value={appointmentTime}
                onChange={handleDateTimeChange}
                className="w-full px-3 py-2 border rounded-md"
                required
              />
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                ระยะเวลา
              </label>
              <div className="px-3 py-2 border rounded-md bg-gray-50">
                30 นาที (เริ่ม {formatThaiTime(new Date(visitData.appointment_datetime))} - สิ้นสุด {formatThaiTime(new Date(visitData.end_datetime))})
              </div>
            </div>
          </div>
          
          <div className="space-y-2 mt-4">
            <label className="block text-sm font-medium">
              เหตุผลในการเข้ารับบริการ <span className="text-red-500">*</span>
            </label>
            <textarea
              name="reason"
              value={visitData.reason}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-md"
              rows={3}
              required
            />
          </div>
          
          <div className="space-y-2 mt-4">
            <label className="block text-sm font-medium">
              หมายเหตุ
            </label>
            <textarea
              name="notes"
              value={visitData.notes || ''}
              onChange={handleChange}
              className="w-full px-3 py-2 border rounded-md"
              rows={3}
            />
          </div>
        </CardContent>
      </Card>
    </div>
  );
};

export default VisitCreation; 