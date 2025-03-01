import React, { useState } from 'react';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../../components/ui/tabs';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../../components/ui/card';
import MedicalCertificateAIAssistant from '../components/MedicalCertificateAIAssistant';
import HealthCheckupAIAssistant from '../components/HealthCheckupAIAssistant';

const MedicalDocumentsAIPage: React.FC = () => {
  const [activeTab, setActiveTab] = useState<string>('certificate');
  
  const handleTabChange = (value: string) => {
    setActiveTab(value);
  };
  
  return (
    <div className="container mx-auto px-4 py-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold mb-2">ระบบ AI สำหรับเอกสารทางการแพทย์</h1>
        <p className="text-gray-600">
          ใช้ระบบ AI ช่วยในการวินิจฉัยและสร้างเอกสารทางการแพทย์อย่างมีประสิทธิภาพ
        </p>
      </div>
      
      <Card className="mb-6">
        <CardHeader>
          <CardTitle>ข้อมูลระบบ AI</CardTitle>
          <CardDescription>
            ระบบ AI ของ MeditechZ ใช้โมเดล Machine Learning ที่ได้รับการฝึกฝนด้วยข้อมูลทางการแพทย์จำนวนมาก
            เพื่อช่วยในการวินิจฉัยและให้คำแนะนำเบื้องต้น
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="bg-green-50 p-4 rounded-md border border-green-200">
              <h3 className="text-lg font-medium text-green-800 mb-2">ความแม่นยำสูง</h3>
              <p className="text-green-700">
                ระบบมีความแม่นยำในการวินิจฉัยสูงถึง 92% สำหรับโรคทั่วไป
              </p>
            </div>
            <div className="bg-blue-50 p-4 rounded-md border border-blue-200">
              <h3 className="text-lg font-medium text-blue-800 mb-2">ประมวลผลรวดเร็ว</h3>
              <p className="text-blue-700">
                วินิจฉัยและให้คำแนะนำภายในเวลาไม่ถึง 1 วินาที
              </p>
            </div>
            <div className="bg-purple-50 p-4 rounded-md border border-purple-200">
              <h3 className="text-lg font-medium text-purple-800 mb-2">ปรับปรุงอย่างต่อเนื่อง</h3>
              <p className="text-purple-700">
                ระบบได้รับการปรับปรุงและฝึกฝนด้วยข้อมูลใหม่ทุกเดือน
              </p>
            </div>
          </div>
        </CardContent>
      </Card>
      
      <Tabs value={activeTab} onValueChange={handleTabChange} className="w-full">
        <TabsList className="grid w-full grid-cols-2 mb-8">
          <TabsTrigger value="certificate">ใบรับรองแพทย์</TabsTrigger>
          <TabsTrigger value="checkup">สมุดตรวจสุขภาพ</TabsTrigger>
        </TabsList>
        
        <TabsContent value="certificate" className="mt-4">
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            <div className="lg:col-span-2">
              <MedicalCertificateAIAssistant />
            </div>
            <div>
              <Card>
                <CardHeader>
                  <CardTitle>คำแนะนำการใช้งาน</CardTitle>
                </CardHeader>
                <CardContent>
                  <ol className="list-decimal pl-5 space-y-2">
                    <li>เลือกอาการที่ผู้ป่วยแสดงให้เห็น</li>
                    <li>กดปุ่ม "วินิจฉัย" เพื่อให้ระบบ AI วิเคราะห์</li>
                    <li>ตรวจสอบผลการวินิจฉัยและคำแนะนำ</li>
                    <li>ปรับจำนวนวันพักฟื้นตามความเหมาะสม</li>
                    <li>กดปุ่ม "นำผลไปใช้" เพื่อนำข้อมูลไปใช้ในใบรับรองแพทย์</li>
                  </ol>
                  <div className="mt-4 p-3 bg-yellow-50 border border-yellow-200 rounded-md">
                    <p className="text-yellow-800 text-sm">
                      <strong>หมายเหตุ:</strong> ผลการวินิจฉัยจาก AI เป็นเพียงคำแนะนำเบื้องต้น 
                      แพทย์ควรใช้วิจารณญาณในการตัดสินใจขั้นสุดท้าย
                    </p>
                  </div>
                </CardContent>
              </Card>
            </div>
          </div>
        </TabsContent>
        
        <TabsContent value="checkup" className="mt-4">
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
            <div className="lg:col-span-2">
              <HealthCheckupAIAssistant />
            </div>
            <div>
              <Card>
                <CardHeader>
                  <CardTitle>คำแนะนำการใช้งาน</CardTitle>
                </CardHeader>
                <CardContent>
                  <ol className="list-decimal pl-5 space-y-2">
                    <li>บันทึกสัญญาณชีพของผู้ป่วยในระบบ</li>
                    <li>เลือกอาการที่ตรวจพบ (ถ้ามี)</li>
                    <li>กดปุ่ม "วินิจฉัย" เพื่อให้ระบบ AI วิเคราะห์</li>
                    <li>ตรวจสอบผลการวินิจฉัยและคำแนะนำ</li>
                    <li>กดปุ่ม "นำผลไปใช้" เพื่อนำข้อมูลไปใช้ในสมุดตรวจสุขภาพ</li>
                  </ol>
                  <div className="mt-4 p-3 bg-yellow-50 border border-yellow-200 rounded-md">
                    <p className="text-yellow-800 text-sm">
                      <strong>หมายเหตุ:</strong> ระบบจะวิเคราะห์สัญญาณชีพและแจ้งเตือนหากพบความผิดปกติ
                      แพทย์ควรตรวจสอบข้อมูลอย่างละเอียดก่อนสรุปผล
                    </p>
                  </div>
                </CardContent>
              </Card>
            </div>
          </div>
        </TabsContent>
      </Tabs>
    </div>
  );
};

export default MedicalDocumentsAIPage; 