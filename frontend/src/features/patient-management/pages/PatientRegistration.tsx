/**
 * Patient Registration Page
 * 
 * หน้าจอสำหรับลงทะเบียนผู้ป่วยใหม่ในระบบ MeditechZ
 * พัฒนาโดยนำแนวคิดจากระบบ MediTech เดิมมาปรับใช้
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Tabs, 
  TabsList, 
  TabsTrigger, 
  TabsContent 
} from '@/components/ui/tabs';
import { 
  Card, 
  CardContent, 
  CardDescription, 
  CardFooter, 
  CardHeader, 
  CardTitle 
} from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Container } from '@/components/ui/container';
import { useToast } from '@/components/ui/use-toast';
import { 
  ArrowLeftIcon, 
  ArrowRightIcon, 
  SaveIcon, 
  SearchIcon, 
  UserPlusIcon, 
  PrinterIcon 
} from 'lucide-react';

import PatientSearch from '../components/PatientSearch';
import PatientForm from '../components/PatientForm';
import VisitCreation from '../components/VisitCreation';

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

// กำหนด interface สำหรับข้อมูลการเข้ารับบริการ
interface VisitData {
  patient_id?: number;
  doctor_id?: number;
  appointment_datetime: string;
  end_datetime: string;
  reason: string;
  notes?: string;
}

const PatientRegistration: React.FC = () => {
  // สร้าง state สำหรับเก็บข้อมูลต่างๆ
  const [activeTab, setActiveTab] = useState<string>('search');
  const [selectedPatient, setSelectedPatient] = useState<PatientData | null>(null);
  const [patientData, setPatientData] = useState<PatientData>({
    first_name: '',
    last_name: '',
    id_card_number: '',
    date_of_birth: '',
    gender: '',
  });
  const [visitData, setVisitData] = useState<VisitData>({
    appointment_datetime: new Date().toISOString(),
    end_datetime: new Date(Date.now() + 30 * 60000).toISOString(), // เพิ่ม 30 นาที
    reason: '',
  });
  const [isLoading, setIsLoading] = useState<boolean>(false);
  
  const navigate = useNavigate();
  const { toast } = useToast();
  
  // ฟังก์ชันสำหรับการค้นหาผู้ป่วย
  const handlePatientSelect = (patient: PatientData) => {
    setSelectedPatient(patient);
    setPatientData(patient);
    setActiveTab('register');
  };
  
  // ฟังก์ชันสำหรับการเปลี่ยนแท็บ
  const handleTabChange = (value: string) => {
    if (value === 'visit' && !selectedPatient && !patientData.id) {
      toast({
        title: 'กรุณาลงทะเบียนผู้ป่วยก่อน',
        description: 'คุณต้องลงทะเบียนผู้ป่วยให้เรียบร้อยก่อนสร้างการเข้ารับบริการ',
        variant: 'destructive',
      });
      return;
    }
    setActiveTab(value);
  };
  
  // ฟังก์ชันสำหรับการบันทึกข้อมูลผู้ป่วย
  const handleSavePatient = async () => {
    try {
      setIsLoading(true);
      
      // ตรวจสอบข้อมูลที่จำเป็น
      if (!patientData.first_name || !patientData.last_name || !patientData.id_card_number || 
          !patientData.date_of_birth || !patientData.gender) {
        toast({
          title: 'ข้อมูลไม่ครบถ้วน',
          description: 'กรุณากรอกข้อมูลที่จำเป็นให้ครบถ้วน',
          variant: 'destructive',
        });
        setIsLoading(false);
        return;
      }
      
      // ส่งข้อมูลไปยัง API
      const response = await fetch('/api/v1/patients', {
        method: patientData.id ? 'PUT' : 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(patientData),
      });
      
      if (!response.ok) {
        throw new Error('เกิดข้อผิดพลาดในการบันทึกข้อมูล');
      }
      
      const savedPatient = await response.json();
      
      // อัปเดต state
      setPatientData(savedPatient);
      setSelectedPatient(savedPatient);
      
      toast({
        title: 'บันทึกข้อมูลสำเร็จ',
        description: 'ข้อมูลผู้ป่วยถูกบันทึกเรียบร้อยแล้ว',
      });
      
      // เปลี่ยนไปยังแท็บถัดไป
      setActiveTab('visit');
    } catch (error) {
      console.error('Error saving patient:', error);
      toast({
        title: 'เกิดข้อผิดพลาด',
        description: error instanceof Error ? error.message : 'ไม่สามารถบันทึกข้อมูลได้',
        variant: 'destructive',
      });
    } finally {
      setIsLoading(false);
    }
  };
  
  // ฟังก์ชันสำหรับการสร้างการเข้ารับบริการ
  const handleCreateVisit = async (printAfterSave: boolean = false) => {
    try {
      setIsLoading(true);
      
      // ตรวจสอบข้อมูลที่จำเป็น
      if (!visitData.reason) {
        toast({
          title: 'ข้อมูลไม่ครบถ้วน',
          description: 'กรุณาระบุเหตุผลในการเข้ารับบริการ',
          variant: 'destructive',
        });
        setIsLoading(false);
        return;
      }
      
      // เพิ่ม patient_id จาก state
      const visitPayload = {
        ...visitData,
        patient_id: patientData.id,
      };
      
      // ส่งข้อมูลไปยัง API
      const response = await fetch('/api/v1/appointments', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(visitPayload),
      });
      
      if (!response.ok) {
        throw new Error('เกิดข้อผิดพลาดในการสร้างการเข้ารับบริการ');
      }
      
      const savedVisit = await response.json();
      
      toast({
        title: 'สร้างการเข้ารับบริการสำเร็จ',
        description: 'การเข้ารับบริการถูกสร้างเรียบร้อยแล้ว',
      });
      
      // ถ้าต้องการพิมพ์หลังจากบันทึก
      if (printAfterSave) {
        // ส่งคำขอไปยัง API สำหรับการพิมพ์
        const printResponse = await fetch(`/api/v1/appointments/${savedVisit.id}/print`, {
          method: 'GET',
        });
        
        if (!printResponse.ok) {
          throw new Error('เกิดข้อผิดพลาดในการพิมพ์');
        }
        
        // สร้าง URL สำหรับไฟล์ PDF และเปิดในแท็บใหม่
        const blob = await printResponse.blob();
        const url = URL.createObjectURL(blob);
        window.open(url, '_blank');
      }
      
      // นำทางไปยังหน้ารายละเอียดการเข้ารับบริการ
      navigate(`/appointments/${savedVisit.id}`);
    } catch (error) {
      console.error('Error creating visit:', error);
      toast({
        title: 'เกิดข้อผิดพลาด',
        description: error instanceof Error ? error.message : 'ไม่สามารถสร้างการเข้ารับบริการได้',
        variant: 'destructive',
      });
    } finally {
      setIsLoading(false);
    }
  };
  
  // ฟังก์ชันสำหรับการอ่านบัตรประชาชน (จำลอง)
  const handleReadIDCard = () => {
    // จำลองการอ่านบัตรประชาชน
    toast({
      title: 'กำลังอ่านบัตรประชาชน',
      description: 'กรุณารอสักครู่...',
    });
    
    // จำลองการดึงข้อมูลจากบัตรประชาชน
    setTimeout(() => {
      const mockData: PatientData = {
        first_name: 'สมชาย',
        last_name: 'ใจดี',
        id_card_number: '1234567890123',
        date_of_birth: '1990-01-01',
        gender: 'male',
        address: '123 ถนนสุขุมวิท แขวงคลองเตย เขตคลองเตย กรุงเทพฯ 10110',
      };
      
      setPatientData(mockData);
      
      toast({
        title: 'อ่านบัตรประชาชนสำเร็จ',
        description: 'ข้อมูลจากบัตรประชาชนถูกนำเข้าเรียบร้อยแล้ว',
      });
    }, 2000);
  };
  
  return (
    <Container className="py-6">
      <Card className="w-full">
        <CardHeader>
          <CardTitle>ลงทะเบียนผู้ป่วย</CardTitle>
          <CardDescription>
            ลงทะเบียนผู้ป่วยใหม่หรือค้นหาผู้ป่วยที่มีอยู่แล้วในระบบ
          </CardDescription>
        </CardHeader>
        
        <CardContent>
          <Tabs value={activeTab} onValueChange={handleTabChange}>
            <TabsList className="grid w-full grid-cols-3">
              <TabsTrigger value="search" className="flex items-center">
                <SearchIcon className="h-4 w-4 mr-2" />
                ค้นหาผู้ป่วย
              </TabsTrigger>
              <TabsTrigger value="register" className="flex items-center">
                <UserPlusIcon className="h-4 w-4 mr-2" />
                ลงทะเบียนผู้ป่วย
              </TabsTrigger>
              <TabsTrigger value="visit" className="flex items-center">
                <PrinterIcon className="h-4 w-4 mr-2" />
                สร้างการเข้ารับบริการ
              </TabsTrigger>
            </TabsList>
            
            <TabsContent value="search">
              <PatientSearch onPatientSelect={handlePatientSelect} />
              
              <div className="flex justify-between mt-6">
                <Button 
                  variant="outline" 
                  onClick={handleReadIDCard}
                >
                  อ่านบัตรประชาชน
                </Button>
                
                <Button 
                  onClick={() => {
                    setPatientData({
                      first_name: '',
                      last_name: '',
                      id_card_number: '',
                      date_of_birth: '',
                      gender: '',
                    });
                    setSelectedPatient(null);
                    setActiveTab('register');
                  }}
                >
                  ลงทะเบียนผู้ป่วยใหม่
                  <ArrowRightIcon className="h-4 w-4 ml-2" />
                </Button>
              </div>
            </TabsContent>
            
            <TabsContent value="register">
              <PatientForm 
                patientData={patientData} 
                setPatientData={setPatientData} 
              />
              
              <div className="flex justify-between mt-6">
                <Button 
                  variant="outline" 
                  onClick={() => setActiveTab('search')}
                >
                  <ArrowLeftIcon className="h-4 w-4 mr-2" />
                  ย้อนกลับ
                </Button>
                
                <div className="flex space-x-2">
                  <Button 
                    variant="outline" 
                    onClick={handleReadIDCard}
                  >
                    อ่านบัตรประชาชน
                  </Button>
                  
                  <Button 
                    onClick={handleSavePatient}
                    disabled={isLoading}
                  >
                    <SaveIcon className="h-4 w-4 mr-2" />
                    {patientData.id ? 'บันทึกการเปลี่ยนแปลง' : 'ลงทะเบียนผู้ป่วย'}
                  </Button>
                </div>
              </div>
            </TabsContent>
            
            <TabsContent value="visit">
              <VisitCreation 
                visitData={visitData} 
                setVisitData={setVisitData}
                patientData={patientData}
              />
              
              <div className="flex justify-between mt-6">
                <Button 
                  variant="outline" 
                  onClick={() => setActiveTab('register')}
                >
                  <ArrowLeftIcon className="h-4 w-4 mr-2" />
                  ย้อนกลับ
                </Button>
                
                <div className="flex space-x-2">
                  <Button 
                    variant="outline"
                    onClick={() => handleCreateVisit(true)}
                    disabled={isLoading}
                  >
                    <PrinterIcon className="h-4 w-4 mr-2" />
                    สร้างและพิมพ์
                  </Button>
                  
                  <Button 
                    onClick={() => handleCreateVisit(false)}
                    disabled={isLoading}
                  >
                    สร้างการเข้ารับบริการ
                  </Button>
                </div>
              </div>
            </TabsContent>
          </Tabs>
        </CardContent>
        
        <CardFooter className="flex justify-between border-t pt-6">
          <Button 
            variant="outline" 
            onClick={() => navigate(-1)}
          >
            ยกเลิก
          </Button>
          
          <div className="text-sm text-gray-500">
            * ข้อมูลที่มีเครื่องหมาย * เป็นข้อมูลที่จำเป็นต้องกรอก
          </div>
        </CardFooter>
      </Card>
    </Container>
  );
};

export default PatientRegistration; 