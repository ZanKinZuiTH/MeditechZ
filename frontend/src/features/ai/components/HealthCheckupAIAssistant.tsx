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
import { Accordion, AccordionContent, AccordionItem, AccordionTrigger } from '../../../components/ui/accordion';

interface PatientVitalSigns {
  temperature?: string;
  pulse?: string;
  respiratory_rate?: string;
  blood_pressure?: string;
  weight?: string;
  height?: string;
  bmi?: string;
  [key: string]: any;
}

interface PatientData {
  patient_id?: number;
  age?: number;
  gender?: string;
  weight?: number;
  height?: number;
  blood_type?: string;
  allergies?: string[];
  chronic_diseases?: string[];
  vital_signs?: PatientVitalSigns;
  [key: string]: any;
}

interface HealthCheckupAIAssistantProps {
  patientData?: PatientData;
  onDiagnosisComplete?: (diagnosisData: any) => void;
}

const HealthCheckupAIAssistant: React.FC<HealthCheckupAIAssistantProps> = ({ 
  patientData,
  onDiagnosisComplete
}) => {
  const [selectedSymptoms, setSelectedSymptoms] = useState<string[]>([]);
  const [activeTab, setActiveTab] = useState<string>('symptoms');
  const { diagnose, diagnosisResult, isLoading, error, reset } = useAiDiagnosis();

  const handleSymptomChange = (symptoms: string[]) => {
    setSelectedSymptoms(symptoms);
  };

  const handleDiagnose = async () => {
    if (selectedSymptoms.length === 0) return;
    
    await diagnose(selectedSymptoms, patientData);
    setActiveTab('results');
  };

  const handleApplyDiagnosis = () => {
    if (onDiagnosisComplete && diagnosisResult) {
      onDiagnosisComplete(diagnosisResult);
    }
  };

  const handleReset = () => {
    reset();
    setSelectedSymptoms([]);
    setActiveTab('symptoms');
  };

  const getVitalSignsStatus = (): 'normal' | 'warning' | 'critical' | 'unknown' => {
    if (!patientData?.vital_signs) return 'unknown';
    
    const vitalSigns = patientData.vital_signs;
    let hasWarning = false;
    
    // ตรวจสอบอุณหภูมิ
    const temp = parseFloat(vitalSigns.temperature || '0');
    if (temp > 37.5 || temp < 36.0) hasWarning = true;
    if (temp > 38.5 || temp < 35.0) return 'critical';
    
    // ตรวจสอบชีพจร
    const pulse = parseInt(vitalSigns.pulse || '0', 10);
    if (pulse > 100 || pulse < 60) hasWarning = true;
    if (pulse > 120 || pulse < 50) return 'critical';
    
    // ตรวจสอบอัตราการหายใจ
    const respRate = parseInt(vitalSigns.respiratory_rate || '0', 10);
    if (respRate > 20 || respRate < 12) hasWarning = true;
    if (respRate > 30 || respRate < 8) return 'critical';
    
    // ตรวจสอบความดันโลหิต
    const bp = vitalSigns.blood_pressure || '';
    if (bp) {
      const [systolic, diastolic] = bp.split('/').map(v => parseInt(v, 10));
      if (systolic > 140 || systolic < 90 || diastolic > 90 || diastolic < 60) hasWarning = true;
      if (systolic > 180 || systolic < 80 || diastolic > 120 || diastolic < 50) return 'critical';
    }
    
    return hasWarning ? 'warning' : 'normal';
  };

  const renderVitalSignsAlert = () => {
    const status = getVitalSignsStatus();
    
    if (status === 'unknown') return null;
    
    let alertType = '';
    let title = '';
    let description = '';
    
    switch (status) {
      case 'normal':
        alertType = 'bg-green-50 text-green-800 border-green-200';
        title = 'สัญญาณชีพปกติ';
        description = 'สัญญาณชีพของผู้ป่วยอยู่ในเกณฑ์ปกติ';
        break;
      case 'warning':
        alertType = 'bg-yellow-50 text-yellow-800 border-yellow-200';
        title = 'สัญญาณชีพผิดปกติเล็กน้อย';
        description = 'สัญญาณชีพของผู้ป่วยมีความผิดปกติเล็กน้อย ควรเฝ้าระวัง';
        break;
      case 'critical':
        alertType = 'bg-red-50 text-red-800 border-red-200';
        title = 'สัญญาณชีพผิดปกติมาก';
        description = 'สัญญาณชีพของผู้ป่วยมีความผิดปกติมาก ควรได้รับการดูแลทันที';
        break;
    }
    
    return (
      <div className={`p-4 rounded-md border ${alertType}`}>
        <h3 className="text-lg font-medium mb-2">{title}</h3>
        <p>{description}</p>
      </div>
    );
  };

  return (
    <Card className="w-full">
      <CardHeader>
        <CardTitle>ผู้ช่วย AI สำหรับการตรวจสุขภาพ</CardTitle>
        <CardDescription>
          ระบบ AI จะช่วยวิเคราะห์อาการและให้คำแนะนำเบื้องต้นสำหรับการตรวจสุขภาพ
        </CardDescription>
      </CardHeader>
      <CardContent>
        {renderVitalSignsAlert()}
        
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
              
              {patientData && (
                <Accordion type="single" collapsible className="w-full">
                  <AccordionItem value="vital-signs">
                    <AccordionTrigger>ข้อมูลสัญญาณชีพ</AccordionTrigger>
                    <AccordionContent>
                      <div className="grid grid-cols-2 gap-2">
                        {patientData.vital_signs?.temperature && (
                          <div>
                            <span className="font-medium">อุณหภูมิ:</span> {patientData.vital_signs.temperature} °C
                          </div>
                        )}
                        {patientData.vital_signs?.pulse && (
                          <div>
                            <span className="font-medium">ชีพจร:</span> {patientData.vital_signs.pulse} ครั้ง/นาที
                          </div>
                        )}
                        {patientData.vital_signs?.respiratory_rate && (
                          <div>
                            <span className="font-medium">อัตราการหายใจ:</span> {patientData.vital_signs.respiratory_rate} ครั้ง/นาที
                          </div>
                        )}
                        {patientData.vital_signs?.blood_pressure && (
                          <div>
                            <span className="font-medium">ความดันโลหิต:</span> {patientData.vital_signs.blood_pressure} mmHg
                          </div>
                        )}
                        {patientData.vital_signs?.weight && (
                          <div>
                            <span className="font-medium">น้ำหนัก:</span> {patientData.vital_signs.weight} kg
                          </div>
                        )}
                        {patientData.vital_signs?.height && (
                          <div>
                            <span className="font-medium">ส่วนสูง:</span> {patientData.vital_signs.height} cm
                          </div>
                        )}
                        {patientData.vital_signs?.bmi && (
                          <div>
                            <span className="font-medium">BMI:</span> {patientData.vital_signs.bmi}
                          </div>
                        )}
                      </div>
                    </AccordionContent>
                  </AccordionItem>
                </Accordion>
              )}
            </div>
          </TabsContent>
          
          <TabsContent value="results" className="mt-4">
            {diagnosisResult && (
              <DiagnosisResult 
                result={diagnosisResult} 
                onApply={handleApplyDiagnosis}
                applied={false}
              />
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

export default HealthCheckupAIAssistant; 