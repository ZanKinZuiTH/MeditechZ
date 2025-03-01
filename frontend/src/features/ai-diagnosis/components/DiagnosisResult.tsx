/**
 * Diagnosis Result Component
 * 
 * คอมโพเนนต์สำหรับแสดงผลการวินิจฉัยโรคด้วย AI
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React, { useState } from 'react';
import { getConfidenceColor } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from '@/components/ui/card';
import {
  Accordion,
  AccordionContent,
  AccordionItem,
  AccordionTrigger,
} from '@/components/ui/accordion';
import { 
  PrinterIcon, 
  Share2Icon, 
  ClipboardCopyIcon, 
  CheckIcon, 
  RefreshCwIcon,
  AlertTriangleIcon
} from 'lucide-react';
import { useToast } from '@/components/ui/use-toast';

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

interface DiagnosisResultProps {
  result: DiagnosisResultData;
  symptoms: string[];
  onReset: () => void;
  onSave: () => void;
}

const DiagnosisResult: React.FC<DiagnosisResultProps> = ({
  result,
  symptoms,
  onReset,
  onSave
}) => {
  const { toast } = useToast();
  const [copied, setCopied] = useState(false);

  // ฟังก์ชันสำหรับคัดลอกผลการวินิจฉัยไปยังคลิปบอร์ด
  const handleCopyToClipboard = () => {
    const diagnosisText = `
การวินิจฉัยโรคด้วย AI

โรคที่วินิจฉัย: ${result.disease}
ความเชื่อมั่น: ${Math.round(result.confidence * 100)}%
คำอธิบาย: ${result.description}

อาการที่ระบุ:
${symptoms.map(s => `- ${s}`).join('\n')}

คำแนะนำ:
${result.recommendations.map(r => `- ${r}`).join('\n')}

การวินิจฉัยแยกโรค:
${result.differential_diagnoses.map(d => `- ${d.disease} (${Math.round(d.confidence * 100)}%): ${d.description}`).join('\n')}

คำเตือน: ผลการวินิจฉัยนี้เป็นเพียงการวินิจฉัยเบื้องต้นโดย AI ไม่สามารถใช้แทนการวินิจฉัยโดยแพทย์ได้ กรุณาปรึกษาแพทย์เพื่อการวินิจฉัยและการรักษาที่ถูกต้อง
    `;

    navigator.clipboard.writeText(diagnosisText).then(() => {
      setCopied(true);
      toast({
        title: 'คัดลอกสำเร็จ',
        description: 'ผลการวินิจฉัยถูกคัดลอกไปยังคลิปบอร์ดแล้ว',
      });
      
      setTimeout(() => setCopied(false), 2000);
    }).catch(err => {
      console.error('ไม่สามารถคัดลอกข้อความได้:', err);
      toast({
        title: 'เกิดข้อผิดพลาด',
        description: 'ไม่สามารถคัดลอกข้อความได้',
        variant: 'destructive',
      });
    });
  };

  // ฟังก์ชันสำหรับพิมพ์ผลการวินิจฉัย
  const handlePrint = () => {
    window.print();
  };

  // ฟังก์ชันสำหรับแชร์ผลการวินิจฉัย
  const handleShare = async () => {
    if (navigator.share) {
      try {
        await navigator.share({
          title: `ผลการวินิจฉัย: ${result.disease}`,
          text: `ผลการวินิจฉัยโรคด้วย AI: ${result.disease} (ความเชื่อมั่น ${Math.round(result.confidence * 100)}%)`,
        });
      } catch (err) {
        console.error('ไม่สามารถแชร์ได้:', err);
        toast({
          title: 'เกิดข้อผิดพลาด',
          description: 'ไม่สามารถแชร์ผลการวินิจฉัยได้',
          variant: 'destructive',
        });
      }
    } else {
      toast({
        title: 'ไม่รองรับ',
        description: 'เบราว์เซอร์ของคุณไม่รองรับการแชร์',
        variant: 'destructive',
      });
    }
  };

  // คำนวณระดับความเชื่อมั่น
  const confidenceLevel = () => {
    if (result.confidence >= 0.8) return 'สูง';
    if (result.confidence >= 0.6) return 'ปานกลาง';
    return 'ต่ำ';
  };

  // คำนวณสีของระดับความเชื่อมั่น
  const confidenceColor = getConfidenceColor(result.confidence);

  return (
    <div className="print:bg-white print:p-0">
      <Card className="mb-6 print:shadow-none">
        <CardHeader>
          <div className="flex justify-between items-start">
            <div>
              <CardTitle className="text-2xl">ผลการวินิจฉัย</CardTitle>
              <CardDescription>
                จากอาการที่คุณระบุ AI วินิจฉัยว่าคุณอาจเป็น
              </CardDescription>
            </div>
            <div className="print:hidden">
              <Button 
                className="flex items-center bg-transparent border border-gray-300 hover:bg-gray-100 text-gray-700 text-sm px-3 py-1 rounded"
                onClick={onReset}
              >
                <RefreshCwIcon className="h-4 w-4 mr-2" />
                วินิจฉัยใหม่
              </Button>
            </div>
          </div>
        </CardHeader>
        
        <CardContent>
          <div className="flex flex-col md:flex-row md:items-center md:justify-between mb-6">
            <div className="mb-4 md:mb-0">
              <h2 className="text-3xl font-bold text-primary">{result.disease}</h2>
              <div className="flex items-center mt-2">
                <div 
                  className="h-2.5 w-24 bg-gray-200 rounded-full mr-2"
                  style={{ 
                    background: `linear-gradient(to right, ${confidenceColor} ${result.confidence * 100}%, #e5e7eb ${result.confidence * 100}%)` 
                  }}
                />
                <span className="text-sm font-medium" style={{ color: confidenceColor }}>
                  ความเชื่อมั่น {Math.round(result.confidence * 100)}% ({confidenceLevel()})
                </span>
              </div>
            </div>
            
            <div className="flex flex-wrap gap-2 print:hidden">
              <Button 
                className="flex items-center bg-transparent border border-gray-300 hover:bg-gray-100 text-gray-700 text-sm px-3 py-1 rounded"
                onClick={handleCopyToClipboard}
              >
                {copied ? (
                  <CheckIcon className="h-4 w-4 mr-2" />
                ) : (
                  <ClipboardCopyIcon className="h-4 w-4 mr-2" />
                )}
                {copied ? 'คัดลอกแล้ว' : 'คัดลอก'}
              </Button>
              
              <Button 
                className="flex items-center bg-transparent border border-gray-300 hover:bg-gray-100 text-gray-700 text-sm px-3 py-1 rounded"
                onClick={handlePrint}
              >
                <PrinterIcon className="h-4 w-4 mr-2" />
                พิมพ์
              </Button>
              
              <Button 
                className="flex items-center bg-transparent border border-gray-300 hover:bg-gray-100 text-gray-700 text-sm px-3 py-1 rounded"
                onClick={handleShare}
              >
                <Share2Icon className="h-4 w-4 mr-2" />
                แชร์
              </Button>
              
              <Button 
                className="flex items-center bg-blue-600 hover:bg-blue-700 text-white text-sm px-3 py-1 rounded"
                onClick={onSave}
              >
                บันทึก
              </Button>
            </div>
          </div>
          
          <div className="mb-6">
            <h3 className="text-lg font-medium mb-2">คำอธิบาย</h3>
            <p className="text-gray-700">{result.description}</p>
          </div>
          
          <div className="mb-6">
            <h3 className="text-lg font-medium mb-2">อาการที่ระบุ</h3>
            <div className="flex flex-wrap gap-1">
              {symptoms.map((symptom, index) => (
                <span 
                  key={index} 
                  className="inline-flex items-center px-2.5 py-1.5 rounded-full text-sm bg-blue-100 text-blue-800"
                >
                  {symptom}
                </span>
              ))}
            </div>
          </div>
          
          <div className="mb-6">
            <h3 className="text-lg font-medium mb-2">คำแนะนำ</h3>
            <ul className="list-disc pl-5 space-y-1">
              {result.recommendations.map((recommendation, index) => (
                <li key={index} className="text-gray-700">{recommendation}</li>
              ))}
            </ul>
          </div>
          
          {result.differential_diagnoses.length > 0 && (
            <div>
              <h3 className="text-lg font-medium mb-2">การวินิจฉัยแยกโรค</h3>
              <p className="text-sm text-gray-500 mb-3">
                โรคอื่นๆ ที่อาจเป็นไปได้จากอาการที่คุณระบุ
              </p>
              
              <Accordion type="single" collapsible className="w-full">
                {result.differential_diagnoses.map((diagnosis, index) => (
                  <AccordionItem key={index} value={`item-${index}`}>
                    <AccordionTrigger className="hover:no-underline">
                      <div className="flex items-center">
                        <span className="font-medium">{diagnosis.disease}</span>
                        <div className="ml-3 flex items-center">
                          <div 
                            className="h-2 w-12 bg-gray-200 rounded-full mr-2"
                            style={{ 
                              background: `linear-gradient(to right, ${getConfidenceColor(diagnosis.confidence)} ${diagnosis.confidence * 100}%, #e5e7eb ${diagnosis.confidence * 100}%)` 
                            }}
                          />
                          <span className="text-sm text-gray-600">
                            {Math.round(diagnosis.confidence * 100)}%
                          </span>
                        </div>
                      </div>
                    </AccordionTrigger>
                    <AccordionContent>
                      <p className="text-gray-700 pl-1">{diagnosis.description}</p>
                    </AccordionContent>
                  </AccordionItem>
                ))}
              </Accordion>
            </div>
          )}
        </CardContent>
        
        <CardFooter className="flex flex-col">
          <div className="flex items-start p-4 bg-amber-50 text-amber-800 rounded-md w-full">
            <AlertTriangleIcon className="h-5 w-5 mr-2 flex-shrink-0 mt-0.5" />
            <div className="text-sm">
              <p className="font-semibold">คำเตือน:</p>
              <p>
                ผลการวินิจฉัยนี้เป็นเพียงการวินิจฉัยเบื้องต้นโดย AI 
                ไม่สามารถใช้แทนการวินิจฉัยโดยแพทย์ได้ กรุณาปรึกษาแพทย์เพื่อการวินิจฉัยและการรักษาที่ถูกต้อง
              </p>
            </div>
          </div>
        </CardFooter>
      </Card>
    </div>
  );
};

export default DiagnosisResult; 