/**
 * AI Diagnosis Page
 * 
 * หน้าสำหรับวินิจฉัยโรคด้วย AI
 * ผู้ใช้สามารถระบุอาการและรับผลการวินิจฉัยเบื้องต้นจาก AI
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React, { useState } from 'react';
import { Container } from '@/components/ui/container';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs';
import { Button } from '@/components/ui/button';
import { useToast } from '@/components/ui/use-toast';
import { Loader2Icon, HistoryIcon, ArrowLeftIcon } from 'lucide-react';

// นำเข้าคอมโพเนนต์ที่เกี่ยวข้อง
import SymptomInput from '../components/SymptomInput';
import DiagnosisResult from '../components/DiagnosisResult';
import DiagnosisHistory from '../components/DiagnosisHistory';

// กำหนด interface สำหรับการวินิจฉัยแยกโรค
interface DifferentialDiagnosis {
  disease: string;
  confidence: number;
  description: string;
}

// กำหนด interface สำหรับผลการวินิจฉัย
interface DiagnosisResultData {
  disease: string;
  confidence: number;
  description: string;
  recommendations: string[];
  differential_diagnoses: DifferentialDiagnosis[];
}

// กำหนด interface สำหรับประวัติการวินิจฉัย
interface DiagnosisHistoryItem {
  id: number;
  symptoms: string[];
  diagnosis: DiagnosisResultData;
  created_at: string;
}

const AIDiagnosisPage: React.FC = () => {
  // สถานะสำหรับเก็บอาการที่ผู้ใช้เลือก
  const [symptoms, setSymptoms] = useState<string[]>([]);
  // สถานะสำหรับเก็บผลการวินิจฉัย
  const [diagnosisResult, setDiagnosisResult] = useState<DiagnosisResultData | null>(null);
  // สถานะสำหรับแสดงสถานะการโหลด
  const [isLoading, setIsLoading] = useState(false);
  // สถานะสำหรับเก็บประวัติการวินิจฉัย
  const [diagnosisHistory, setDiagnosisHistory] = useState<DiagnosisHistoryItem[]>([]);
  // สถานะสำหรับแสดงสถานะการโหลดประวัติ
  const [isLoadingHistory, setIsLoadingHistory] = useState(false);
  // สถานะสำหรับแท็บที่เลือก
  const [activeTab, setActiveTab] = useState('diagnose');

  const { toast } = useToast();

  // ฟังก์ชันสำหรับวินิจฉัยอาการ
  const handleDiagnose = async (selectedSymptoms: string[]) => {
    setSymptoms(selectedSymptoms);
    setIsLoading(true);
    
    try {
      // ในสถานการณ์จริง จะต้องเรียก API
      // const response = await fetch('/api/v1/ai-diagnosis/diagnose', {
      //   method: 'POST',
      //   headers: { 'Content-Type': 'application/json' },
      //   body: JSON.stringify({ symptoms: selectedSymptoms })
      // });
      // if (!response.ok) throw new Error('Failed to diagnose');
      // const data = await response.json();
      // setDiagnosisResult(data);
      
      // จำลองการเรียก API
      await new Promise(resolve => setTimeout(resolve, 2000));
      
      // ตัวอย่างข้อมูลจำลอง
      let mockResult: DiagnosisResultData;
      
      if (selectedSymptoms.includes('ไข้') && selectedSymptoms.includes('ไอ') && selectedSymptoms.includes('เจ็บคอ')) {
        if (selectedSymptoms.includes('หายใจลำบาก')) {
          mockResult = {
            disease: 'โควิด-19',
            confidence: 0.85,
            description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสโคโรนา SARS-CoV-2',
            recommendations: [
              'แยกตัวจากผู้อื่นทันที',
              'ตรวจหาเชื้อโควิด-19 ด้วยชุดตรวจ ATK หรือ RT-PCR',
              'ติดต่อสายด่วนโควิด-19 หรือสถานพยาบาลใกล้บ้าน',
              'ตรวจวัดระดับออกซิเจนในเลือดอย่างสม่ำเสมอ (ถ้ามีเครื่องวัด)',
              'พักผ่อนให้เพียงพอ',
              'ดื่มน้ำมากๆ',
              'หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์',
              'เช็ดตัวด้วยน้ำอุ่นเพื่อลดไข้',
              'ดื่มน้ำอุ่นผสมน้ำผึ้งเพื่อบรรเทาอาการไอ',
              'กลั้วคอด้วยน้ำเกลืออุ่นเพื่อบรรเทาอาการเจ็บคอ'
            ],
            differential_diagnoses: [
              {
                disease: 'ไข้หวัดใหญ่',
                confidence: 0.65,
                description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสอินฟลูเอนซา'
              },
              {
                disease: 'ปอดอักเสบ',
                confidence: 0.45,
                description: 'การอักเสบของเนื้อปอด มักเกิดจากการติดเชื้อ'
              }
            ]
          };
        } else {
          mockResult = {
            disease: 'ไข้หวัดใหญ่',
            confidence: 0.75,
            description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสอินฟลูเอนซา',
            recommendations: [
              'ทานยาลดไข้ตามคำแนะนำของแพทย์',
              'หลีกเลี่ยงการสัมผัสใกล้ชิดกับผู้อื่นเพื่อป้องกันการแพร่เชื้อ',
              'สวมหน้ากากอนามัยเมื่อต้องอยู่ร่วมกับผู้อื่น',
              'พักผ่อนให้เพียงพอ',
              'ดื่มน้ำมากๆ',
              'หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์',
              'เช็ดตัวด้วยน้ำอุ่นเพื่อลดไข้',
              'ดื่มน้ำอุ่นผสมน้ำผึ้งเพื่อบรรเทาอาการไอ',
              'กลั้วคอด้วยน้ำเกลืออุ่นเพื่อบรรเทาอาการเจ็บคอ'
            ],
            differential_diagnoses: [
              {
                disease: 'ไข้หวัดธรรมดา',
                confidence: 0.6,
                description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสหลายชนิด'
              },
              {
                disease: 'โควิด-19',
                confidence: 0.4,
                description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสโคโรนา SARS-CoV-2'
              }
            ]
          };
        }
      } else if (selectedSymptoms.includes('ปวดหัว') && selectedSymptoms.includes('คลื่นไส้')) {
        mockResult = {
          disease: 'ไมเกรน',
          confidence: 0.8,
          description: 'อาการปวดศีรษะข้างเดียวหรือสองข้าง มักมีอาการคลื่นไส้ร่วมด้วย',
          recommendations: [
            'พักในที่เงียบและมืด',
            'ประคบเย็นบริเวณที่ปวด',
            'ทานยาแก้ปวดตามคำแนะนำของแพทย์',
            'พักผ่อนให้เพียงพอ',
            'ดื่มน้ำมากๆ',
            'หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์'
          ],
          differential_diagnoses: [
            {
              disease: 'ความดันโลหิตสูง',
              confidence: 0.4,
              description: 'ภาวะที่ความดันในหลอดเลือดแดงสูงกว่าปกติ'
            }
          ]
        };
      } else if (selectedSymptoms.includes('ท้องเสีย') && selectedSymptoms.includes('คลื่นไส้') && selectedSymptoms.includes('อาเจียน')) {
        mockResult = {
          disease: 'อาหารเป็นพิษ',
          confidence: 0.9,
          description: 'ภาวะที่เกิดจากการรับประทานอาหารที่ปนเปื้อนเชื้อแบคทีเรียหรือสารพิษ',
          recommendations: [
            'งดอาหารและน้ำ 1-2 ชั่วโมง แล้วค่อยๆ จิบน้ำ',
            'ทานอาหารอ่อนๆ เช่น ข้าวต้ม โจ๊ก',
            'ดื่มสารละลายเกลือแร่เพื่อป้องกันการขาดน้ำ',
            'พักผ่อนให้เพียงพอ',
            'หากอาการไม่ดีขึ้นภายใน 24 ชั่วโมง ควรพบแพทย์'
          ],
          differential_diagnoses: [
            {
              disease: 'กระเพาะอาหารอักเสบ',
              confidence: 0.6,
              description: 'ภาวะที่เยื่อบุกระเพาะอาหารเกิดการอักเสบ'
            },
            {
              disease: 'ลำไส้อักเสบ',
              confidence: 0.5,
              description: 'การอักเสบของลำไส้ ซึ่งอาจเกิดจากการติดเชื้อหรือสาเหตุอื่น'
            }
          ]
        };
      } else {
        mockResult = {
          disease: 'ไข้หวัดธรรมดา',
          confidence: 0.5,
          description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสหลายชนิด',
          recommendations: [
            'ทานยาลดไข้ตามคำแนะนำของแพทย์',
            'ใช้ยาพ่นจมูกหากมีอาการคัดจมูก',
            'ดื่มน้ำอุ่นผสมน้ำผึ้งและมะนาวเพื่อบรรเทาอาการเจ็บคอ',
            'พักผ่อนให้เพียงพอ',
            'ดื่มน้ำมากๆ',
            'หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์'
          ],
          differential_diagnoses: []
        };
      }
      
      setDiagnosisResult(mockResult);
      
      // เพิ่มผลการวินิจฉัยลงในประวัติ
      const newHistoryItem: DiagnosisHistoryItem = {
        id: Date.now(),
        symptoms: selectedSymptoms,
        diagnosis: mockResult,
        created_at: new Date().toISOString()
      };
      
      setDiagnosisHistory(prev => [newHistoryItem, ...prev]);
    } catch (error) {
      console.error('Error diagnosing symptoms:', error);
      toast({
        title: 'เกิดข้อผิดพลาด',
        description: 'ไม่สามารถวินิจฉัยอาการได้ กรุณาลองใหม่อีกครั้ง',
        variant: 'destructive',
      });
    } finally {
      setIsLoading(false);
    }
  };

  // ฟังก์ชันสำหรับรีเซ็ตการวินิจฉัย
  const handleReset = () => {
    setSymptoms([]);
    setDiagnosisResult(null);
  };

  // ฟังก์ชันสำหรับบันทึกผลการวินิจฉัย
  const handleSaveDiagnosis = () => {
    // ในสถานการณ์จริง จะต้องส่งข้อมูลไปยัง API เพื่อบันทึกลงฐานข้อมูล
    toast({
      title: 'บันทึกผลการวินิจฉัยสำเร็จ',
      description: 'ผลการวินิจฉัยถูกบันทึกลงในประวัติแล้ว',
    });
  };

  // ฟังก์ชันสำหรับโหลดประวัติการวินิจฉัย
  const handleLoadHistory = async () => {
    setIsLoadingHistory(true);
    
    try {
      // ในสถานการณ์จริง จะต้องเรียก API
      // const response = await fetch('/api/v1/ai-diagnosis/history');
      // if (!response.ok) throw new Error('Failed to load history');
      // const data = await response.json();
      // setDiagnosisHistory(data);
      
      // จำลองการเรียก API
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // ใช้ประวัติที่มีอยู่แล้ว (ในสถานการณ์จริง จะต้องดึงจาก API)
    } catch (error) {
      console.error('Error loading diagnosis history:', error);
      toast({
        title: 'เกิดข้อผิดพลาด',
        description: 'ไม่สามารถโหลดประวัติการวินิจฉัยได้',
        variant: 'destructive',
      });
    } finally {
      setIsLoadingHistory(false);
    }
  };

  // โหลดประวัติเมื่อเปลี่ยนแท็บเป็น "ประวัติ"
  const handleTabChange = (value: string) => {
    setActiveTab(value);
    if (value === 'history' && diagnosisHistory.length === 0) {
      handleLoadHistory();
    }
  };

  return (
    <Container className="py-8">
      <div className="mb-6">
        <h1 className="text-3xl font-bold mb-2">วินิจฉัยโรคด้วย AI</h1>
        <p className="text-gray-500">
          ระบุอาการของคุณเพื่อรับการวินิจฉัยเบื้องต้นจาก AI
        </p>
      </div>
      
      <Tabs value={activeTab} onValueChange={handleTabChange}>
        <TabsList className="mb-6">
          <TabsTrigger value="diagnose">วินิจฉัยโรค</TabsTrigger>
          <TabsTrigger value="history" className="flex items-center">
            <HistoryIcon className="h-4 w-4 mr-2" />
            ประวัติการวินิจฉัย
          </TabsTrigger>
        </TabsList>
        
        <TabsContent value="diagnose" className="mt-0">
          {!diagnosisResult ? (
            <div className="max-w-2xl mx-auto">
              <SymptomInput onSubmit={handleDiagnose} isLoading={isLoading} />
              
              {isLoading && (
                <div className="flex justify-center items-center mt-8">
                  <Loader2Icon className="h-8 w-8 animate-spin text-primary mr-2" />
                  <span>กำลังวินิจฉัยอาการ...</span>
                </div>
              )}
            </div>
          ) : (
            <div className="max-w-4xl mx-auto">
              <DiagnosisResult 
                result={diagnosisResult} 
                symptoms={symptoms} 
                onReset={handleReset}
                onSave={handleSaveDiagnosis}
              />
            </div>
          )}
        </TabsContent>
        
        <TabsContent value="history" className="mt-0">
          {isLoadingHistory ? (
            <div className="flex justify-center items-center h-64">
              <Loader2Icon className="h-8 w-8 animate-spin text-primary mr-2" />
              <span>กำลังโหลดประวัติการวินิจฉัย...</span>
            </div>
          ) : (
            <div>
              {diagnosisHistory.length > 0 ? (
                <DiagnosisHistory 
                  history={diagnosisHistory}
                  onViewDetails={(item) => {
                    setSymptoms(item.symptoms);
                    setDiagnosisResult(item.diagnosis);
                    setActiveTab('diagnose');
                  }}
                />
              ) : (
                <div className="flex flex-col items-center justify-center h-64">
                  <p className="text-xl text-gray-500 mb-4">ไม่พบประวัติการวินิจฉัย</p>
                  <Button onClick={() => setActiveTab('diagnose')}>
                    <ArrowLeftIcon className="h-4 w-4 mr-2" />
                    กลับไปวินิจฉัยโรค
                  </Button>
                </div>
              )}
            </div>
          )}
        </TabsContent>
      </Tabs>
      
      <div className="mt-8 text-center text-sm text-gray-500">
        <p>
          <strong>คำเตือน:</strong> ผลการวินิจฉัยนี้เป็นเพียงการวินิจฉัยเบื้องต้นโดย AI 
          ไม่สามารถใช้แทนการวินิจฉัยโดยแพทย์ได้ กรุณาปรึกษาแพทย์เพื่อการวินิจฉัยและการรักษาที่ถูกต้อง
        </p>
      </div>
    </Container>
  );
};

export default AIDiagnosisPage; 