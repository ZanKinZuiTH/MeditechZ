import React, { useState } from 'react';
import { 
  Card, 
  CardContent, 
  CardHeader, 
  CardTitle, 
  CardDescription, 
  CardFooter 
} from '../../../components/ui/card';
import { Button } from '../../../components/ui/button';
import { Tabs, TabsContent, TabsList, TabsTrigger } from '../../../components/ui/tabs';
import { useAiDiagnosis } from '../hooks/useAiDiagnosis';
import { SymptomSelector } from './SymptomSelector';
import { DiagnosisResult } from './DiagnosisResult';

interface PatientData {
  patient_id?: number;
  age?: number;
  gender?: string;
  weight?: number;
  height?: number;
  blood_type?: string;
  allergies?: string[];
  chronic_diseases?: string[];
  [key: string]: any;
}

interface MedicalCertificateAIAssistantProps {
  patientData?: PatientData;
  onDiagnosisComplete?: (diagnosisData: any) => void;
}

const MedicalCertificateAIAssistant: React.FC<MedicalCertificateAIAssistantProps> = ({ 
  patientData,
  onDiagnosisComplete
}) => {
  const [selectedSymptoms, setSelectedSymptoms] = useState<string[]>([]);
  const [activeTab, setActiveTab] = useState<string>('symptoms');
  const [restDays, setRestDays] = useState<number>(0);
  const { diagnose, diagnosisResult, isLoading, error, reset } = useAiDiagnosis();

  const handleSymptomChange = (symptoms: string[]) => {
    setSelectedSymptoms(symptoms);
  };

  const handleDiagnose = async () => {
    if (selectedSymptoms.length === 0) return;
    
    await diagnose(selectedSymptoms, patientData);
    setActiveTab('results');
    
    // ประมาณจำนวนวันพักฟื้นตามความรุนแรงของโรค
    if (diagnosisResult) {
      const severity = diagnosisResult.confidence;
      if (severity > 0.8) {
        setRestDays(7); // อาการหนัก
      } else if (severity > 0.5) {
        setRestDays(3); // อาการปานกลาง
      } else {
        setRestDays(1); // อาการเล็กน้อย
      }
    }
  };

  const handleApplyDiagnosis = () => {
    if (onDiagnosisComplete && diagnosisResult) {
      // เพิ่มข้อมูลวันพักฟื้นเข้าไปในผลการวินิจฉัย
      const enhancedDiagnosis = {
        ...diagnosisResult,
        rest_period_days: restDays
      };
      onDiagnosisComplete(enhancedDiagnosis);
    }
  };

  const handleReset = () => {
    reset();
    setSelectedSymptoms([]);
    setRestDays(0);
    setActiveTab('symptoms');
  };

  const handleRestDaysChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = parseInt(e.target.value);
    setRestDays(isNaN(value) ? 0 : value);
  };

  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle>ผู้ช่วย AI สำหรับใบรับรองแพทย์</CardTitle>
        <CardDescription>
          ระบบ AI จะช่วยวินิจฉัยอาการและให้คำแนะนำสำหรับการออกใบรับรองแพทย์
        </CardDescription>
      </CardHeader>
      <CardContent>
        {error && (
          <div className="p-4 rounded-md border bg-red-50 text-red-800 border-red-200 mb-4">
            <h3 className="text-lg font-medium mb-2">เกิดข้อผิดพลาด</h3>
            <p>{error}</p>
          </div>
        )}
        
        <Tabs value={activeTab} onValueChange={setActiveTab} className="w-full">
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="symptoms">อาการ</TabsTrigger>
            <TabsTrigger value="results" disabled={!diagnosisResult}>ผลการวินิจฉัย</TabsTrigger>
          </TabsList>
          
          <TabsContent value="symptoms" className="mt-4">
            <div className="space-y-4">
              <div>
                <h3 className="text-lg font-medium mb-2">เลือกอาการที่พบ</h3>
                <SymptomSelector 
                  selectedSymptoms={selectedSymptoms} 
                  onChange={handleSymptomChange}
                  disabled={isLoading}
                />
              </div>
            </div>
          </TabsContent>
          
          <TabsContent value="results" className="mt-4">
            {diagnosisResult && (
              <div className="space-y-4">
                <DiagnosisResult 
                  result={diagnosisResult} 
                  onApply={handleApplyDiagnosis}
                  applied={false}
                />
                
                <div className="p-4 rounded-md border bg-blue-50 text-blue-800 border-blue-200">
                  <h3 className="text-lg font-medium mb-2">คำแนะนำสำหรับใบรับรองแพทย์</h3>
                  <div className="space-y-2">
                    <p>ระบบแนะนำให้ผู้ป่วยพักฟื้นเป็นเวลา {restDays} วัน</p>
                    <div className="flex items-center space-x-2">
                      <label htmlFor="rest-days" className="whitespace-nowrap">ปรับจำนวนวันพักฟื้น:</label>
                      <input
                        id="rest-days"
                        type="number"
                        min="0"
                        max="30"
                        value={restDays}
                        onChange={handleRestDaysChange}
                        className="input input-bordered w-20"
                      />
                      <span className="ml-1">วัน</span>
                    </div>
                  </div>
                </div>
              </div>
            )}
          </TabsContent>
        </Tabs>
      </CardContent>
      <CardFooter className="flex justify-between">
        <Button variant="outline" onClick={handleReset} disabled={isLoading}>
          ล้างข้อมูล
        </Button>
        {activeTab === 'symptoms' ? (
          <Button onClick={handleDiagnose} disabled={selectedSymptoms.length === 0 || isLoading}>
            {isLoading ? 'กำลังวินิจฉัย...' : 'วินิจฉัย'}
          </Button>
        ) : (
          <Button onClick={handleApplyDiagnosis} disabled={!diagnosisResult}>
            นำผลไปใช้
          </Button>
        )}
      </CardFooter>
    </Card>
  );
};

export default MedicalCertificateAIAssistant; 