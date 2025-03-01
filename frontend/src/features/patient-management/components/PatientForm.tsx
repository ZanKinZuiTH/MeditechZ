/**
 * Patient Form Component
 * 
 * คอมโพเนนต์สำหรับกรอกข้อมูลผู้ป่วยในระบบ MeditechZ
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React from 'react';
import { 
  Card, 
  CardContent 
} from '@/components/ui/card';
import { 
  Accordion, 
  AccordionContent, 
  AccordionItem, 
  AccordionTrigger 
} from '@/components/ui/accordion';

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

interface PatientFormProps {
  patientData: PatientData;
  setPatientData: React.Dispatch<React.SetStateAction<PatientData>>;
}

const PatientForm: React.FC<PatientFormProps> = ({ 
  patientData, 
  setPatientData 
}) => {
  // ฟังก์ชันสำหรับการอัปเดตข้อมูล
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setPatientData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };
  
  return (
    <div className="space-y-4">
      <Card>
        <CardContent className="p-6">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                ชื่อ <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                name="first_name"
                value={patientData.first_name}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
                required
              />
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                นามสกุล <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                name="last_name"
                value={patientData.last_name}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
                required
              />
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                เลขบัตรประชาชน <span className="text-red-500">*</span>
              </label>
              <input
                type="text"
                name="id_card_number"
                value={patientData.id_card_number}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
                required
                maxLength={13}
              />
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                วันเกิด <span className="text-red-500">*</span>
              </label>
              <input
                type="date"
                name="date_of_birth"
                value={patientData.date_of_birth}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
                required
              />
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                เพศ <span className="text-red-500">*</span>
              </label>
              <select
                name="gender"
                value={patientData.gender}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
                required
              >
                <option value="">เลือกเพศ</option>
                <option value="male">ชาย</option>
                <option value="female">หญิง</option>
                <option value="other">อื่นๆ</option>
              </select>
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                หมู่เลือด
              </label>
              <select
                name="blood_type"
                value={patientData.blood_type || ''}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
              >
                <option value="">เลือกหมู่เลือด</option>
                <option value="A">A</option>
                <option value="B">B</option>
                <option value="AB">AB</option>
                <option value="O">O</option>
              </select>
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                เบอร์โทรศัพท์
              </label>
              <input
                type="tel"
                name="phone_number"
                value={patientData.phone_number || ''}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
                maxLength={10}
              />
            </div>
            
            <div className="space-y-2">
              <label className="block text-sm font-medium">
                อีเมล
              </label>
              <input
                type="email"
                name="email"
                value={patientData.email || ''}
                onChange={handleChange}
                className="w-full px-3 py-2 border rounded-md"
              />
            </div>
          </div>
          
          <Accordion type="single" collapsible className="mt-6">
            <AccordionItem value="address">
              <AccordionTrigger>ที่อยู่</AccordionTrigger>
              <AccordionContent>
                <div className="space-y-4 pt-2">
                  <div className="space-y-2">
                    <label className="block text-sm font-medium">
                      ที่อยู่
                    </label>
                    <textarea
                      name="address"
                      value={patientData.address || ''}
                      onChange={handleChange}
                      className="w-full px-3 py-2 border rounded-md"
                      rows={3}
                    />
                  </div>
                </div>
              </AccordionContent>
            </AccordionItem>
            
            <AccordionItem value="emergency">
              <AccordionTrigger>ข้อมูลติดต่อฉุกเฉิน</AccordionTrigger>
              <AccordionContent>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4 pt-2">
                  <div className="space-y-2">
                    <label className="block text-sm font-medium">
                      ชื่อผู้ติดต่อฉุกเฉิน
                    </label>
                    <input
                      type="text"
                      name="emergency_contact_name"
                      value={patientData.emergency_contact_name || ''}
                      onChange={handleChange}
                      className="w-full px-3 py-2 border rounded-md"
                    />
                  </div>
                  
                  <div className="space-y-2">
                    <label className="block text-sm font-medium">
                      เบอร์โทรศัพท์ผู้ติดต่อฉุกเฉิน
                    </label>
                    <input
                      type="tel"
                      name="emergency_contact_phone"
                      value={patientData.emergency_contact_phone || ''}
                      onChange={handleChange}
                      className="w-full px-3 py-2 border rounded-md"
                      maxLength={10}
                    />
                  </div>
                </div>
              </AccordionContent>
            </AccordionItem>
            
            <AccordionItem value="medical">
              <AccordionTrigger>ข้อมูลทางการแพทย์</AccordionTrigger>
              <AccordionContent>
                <div className="space-y-4 pt-2">
                  <div className="space-y-2">
                    <label className="block text-sm font-medium">
                      ประวัติการแพ้ยา/อาหาร
                    </label>
                    <textarea
                      name="allergies"
                      value={patientData.allergies || ''}
                      onChange={handleChange}
                      className="w-full px-3 py-2 border rounded-md"
                      rows={3}
                    />
                  </div>
                  
                  <div className="space-y-2">
                    <label className="block text-sm font-medium">
                      โรคประจำตัว
                    </label>
                    <textarea
                      name="chronic_diseases"
                      value={patientData.chronic_diseases || ''}
                      onChange={handleChange}
                      className="w-full px-3 py-2 border rounded-md"
                      rows={3}
                    />
                  </div>
                </div>
              </AccordionContent>
            </AccordionItem>
          </Accordion>
        </CardContent>
      </Card>
    </div>
  );
};

export default PatientForm; 