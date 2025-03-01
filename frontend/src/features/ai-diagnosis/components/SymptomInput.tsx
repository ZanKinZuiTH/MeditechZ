/**
 * Symptom Input Component
 * 
 * คอมโพเนนต์สำหรับรับข้อมูลอาการจากผู้ใช้
 * ใช้ในหน้าวินิจฉัยโรคด้วย AI
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React, { useState, useEffect } from 'react';
import { 
  Card, 
  CardContent, 
  CardDescription, 
  CardFooter, 
  CardHeader, 
  CardTitle 
} from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { 
  PlusIcon, 
  XIcon, 
  SearchIcon, 
  AlertCircleIcon 
} from 'lucide-react';
import { 
  Command, 
  CommandEmpty, 
  CommandGroup, 
  CommandInput, 
  CommandItem, 
  CommandList 
} from '@/components/ui/command';
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover';
import { useToast } from '@/components/ui/use-toast';

// รายการอาการทั้งหมด (ในสถานการณ์จริง ควรดึงจาก API)
const ALL_SYMPTOMS = [
  'ไข้', 'ไอ', 'เจ็บคอ', 'ปวดหัว', 'คลื่นไส้', 'อาเจียน', 'ท้องเสีย',
  'ปวดกล้ามเนื้อ', 'อ่อนเพลีย', 'หายใจลำบาก', 'จมูกไม่ได้กลิ่น', 'ลิ้นไม่รับรส',
  'ผื่นขึ้น', 'ตาแดง', 'ปวดท้อง', 'เวียนหัว', 'ไอมีเสมหะ', 'ไอแห้ง',
  'หนาวสั่น', 'เหงื่อออกตอนกลางคืน', 'น้ำหนักลด', 'เบื่ออาหาร', 'ปวดข้อ',
  'ปวดหลัง', 'ปวดเมื่อยตามตัว', 'ชัก', 'สับสน', 'ซึมเศร้า', 'วิตกกังวล',
  'นอนไม่หลับ', 'ปัสสาวะบ่อย', 'ปัสสาวะแสบขัด', 'ท้องผูก', 'ท้องอืด',
  'แน่นหน้าอก', 'ใจสั่น', 'เป็นลม', 'ชาตามแขนขา', 'กล้ามเนื้ออ่อนแรง',
  'ปวดศีรษะข้างเดียว', 'แพ้แสง', 'คลื่นไส้อาเจียน', 'มองเห็นภาพซ้อน',
  'ตัวเหลือง', 'ตาเหลือง', 'ปวดใต้ชายโครงขวา', 'ปัสสาวะสีเข้ม',
  'อุจจาระสีซีด', 'คันตามตัว', 'บวมตามตัว', 'บวมที่ขา', 'บวมที่หน้า',
  'ไอเป็นเลือด', 'อุจจาระเป็นเลือด', 'อุจจาระดำ', 'ประจำเดือนมามาก',
  'ประจำเดือนมาไม่ปกติ', 'ปวดประจำเดือน', 'ตกขาว', 'ปวดเวลามีเพศสัมพันธ์',
  'แผลที่อวัยวะเพศ', 'ต่อมน้ำเหลืองโต', 'เจ็บหน้าอก', 'หัวใจเต้นเร็ว',
  'หัวใจเต้นช้า', 'หัวใจเต้นผิดจังหวะ', 'เหนื่อยง่าย', 'เหนื่อยเวลานอนราบ',
  'ขาบวมสองข้าง', 'ปวดน่อง', 'ขาบวมข้างเดียว', 'ผิวหนังเปลี่ยนสี',
  'แผลที่ผิวหนัง', 'ผื่นคัน', 'ผื่นแดง', 'ตุ่มพอง', 'ผมร่วง', 'เล็บผิดปกติ',
  'ปวดฟัน', 'เหงือกอักเสบ', 'แผลในปาก', 'ลิ้นเป็นฝ้า', 'ปากแห้ง', 'น้ำลายแห้ง'
];

interface SymptomInputProps {
  onSubmit: (symptoms: string[]) => void;
  isLoading: boolean;
}

const SymptomInput: React.FC<SymptomInputProps> = ({ onSubmit, isLoading }) => {
  // สถานะสำหรับเก็บอาการที่ผู้ใช้เลือก
  const [selectedSymptoms, setSelectedSymptoms] = useState<string[]>([]);
  // สถานะสำหรับเก็บคำค้นหา
  const [searchTerm, setSearchTerm] = useState('');
  // สถานะสำหรับเก็บอาการที่กรองแล้ว
  const [filteredSymptoms, setFilteredSymptoms] = useState<string[]>(ALL_SYMPTOMS);
  // สถานะสำหรับแสดงข้อความเตือน
  const [error, setError] = useState('');

  const { toast } = useToast();

  // กรองอาการตามคำค้นหา
  useEffect(() => {
    if (searchTerm.trim() === '') {
      setFilteredSymptoms(ALL_SYMPTOMS);
    } else {
      const filtered = ALL_SYMPTOMS.filter(symptom => 
        symptom.toLowerCase().includes(searchTerm.toLowerCase())
      );
      setFilteredSymptoms(filtered);
    }
  }, [searchTerm]);

  // เพิ่มอาการที่เลือก
  const handleAddSymptom = (symptom: string) => {
    if (!selectedSymptoms.includes(symptom)) {
      setSelectedSymptoms([...selectedSymptoms, symptom]);
      setError('');
    }
  };

  // ลบอาการที่เลือก
  const handleRemoveSymptom = (symptom: string) => {
    setSelectedSymptoms(selectedSymptoms.filter(s => s !== symptom));
  };

  // ล้างอาการทั้งหมด
  const handleClearAll = () => {
    setSelectedSymptoms([]);
  };

  // ส่งอาการไปวินิจฉัย
  const handleSubmit = () => {
    if (selectedSymptoms.length === 0) {
      setError('กรุณาเลือกอาการอย่างน้อย 1 อาการ');
      return;
    }
    
    if (selectedSymptoms.length > 10) {
      toast({
        title: 'คำเตือน',
        description: 'คุณเลือกอาการมากเกินไป อาจทำให้ผลการวินิจฉัยไม่แม่นยำ',
        variant: 'destructive',
      });
    }
    
    onSubmit(selectedSymptoms);
  };

  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle>ระบุอาการของคุณ</CardTitle>
        <CardDescription>
          เลือกอาการที่คุณกำลังประสบอยู่เพื่อรับการวินิจฉัยเบื้องต้นจาก AI
        </CardDescription>
      </CardHeader>
      
      <CardContent>
        <div className="space-y-4">
          <div className="min-h-20 p-4 border rounded-md bg-gray-50">
            {selectedSymptoms.length === 0 ? (
              <p className="text-gray-500 text-center">ยังไม่ได้เลือกอาการใด</p>
            ) : (
              <div className="flex flex-wrap gap-2">
                {selectedSymptoms.map((symptom) => (
                  <div 
                    key={symptom} 
                    className="flex items-center bg-blue-100 text-blue-800 px-3 py-1 rounded-full text-sm"
                  >
                    <span>{symptom}</span>
                    <button 
                      onClick={() => handleRemoveSymptom(symptom)}
                      className="ml-2 text-blue-600 hover:text-blue-800"
                    >
                      <XIcon className="h-3 w-3" />
                    </button>
                  </div>
                ))}
              </div>
            )}
          </div>
          
          {error && (
            <div className="text-red-500 text-sm">{error}</div>
          )}
          
          <div className="relative">
            <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
              <SearchIcon className="h-5 w-5 text-gray-400" />
            </div>
            <input
              type="text"
              placeholder="ค้นหาอาการ..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="pl-10 pr-4 py-2 w-full border rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
            />
          </div>
          
          <div className="max-h-60 overflow-y-auto border rounded-md p-2">
            {filteredSymptoms.length === 0 ? (
              <p className="text-gray-500 text-center py-4">ไม่พบอาการที่ค้นหา</p>
            ) : (
              <div className="grid grid-cols-2 md:grid-cols-3 gap-2">
                {filteredSymptoms.map((symptom) => (
                  <button
                    key={symptom}
                    onClick={() => handleAddSymptom(symptom)}
                    disabled={selectedSymptoms.includes(symptom)}
                    className={`text-left px-3 py-2 rounded-md text-sm flex items-center ${
                      selectedSymptoms.includes(symptom)
                        ? 'bg-blue-100 text-blue-800 cursor-not-allowed'
                        : 'hover:bg-gray-100'
                    }`}
                  >
                    {!selectedSymptoms.includes(symptom) && (
                      <PlusIcon className="h-3 w-3 mr-2 text-gray-500" />
                    )}
                    {symptom}
                  </button>
                ))}
              </div>
            )}
          </div>
        </div>
      </CardContent>
      
      <CardFooter className="flex justify-between">
        <Button 
          className="bg-transparent border border-gray-300 hover:bg-gray-100 text-gray-700 px-4 py-2 rounded"
          onClick={handleClearAll}
          disabled={selectedSymptoms.length === 0 || isLoading}
        >
          ล้างทั้งหมด
        </Button>
        <Button 
          className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded"
          onClick={handleSubmit}
          disabled={selectedSymptoms.length === 0 || isLoading}
        >
          {isLoading ? 'กำลังวินิจฉัย...' : 'วินิจฉัยอาการ'}
        </Button>
      </CardFooter>
    </Card>
  );
};

export default SymptomInput; 