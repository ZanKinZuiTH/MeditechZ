/**
 * Diagnosis History Component
 * 
 * คอมโพเนนต์สำหรับแสดงประวัติการวินิจฉัยโรคด้วย AI
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React from 'react';
import { formatThaiDateTime, getConfidenceColor } from '@/lib/utils';
import { Button } from '@/components/ui/button';
import { EyeIcon, FileTextIcon, ClockIcon } from 'lucide-react';

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

interface DiagnosisHistoryProps {
  history: DiagnosisHistoryItem[];
  onViewDetails: (item: DiagnosisHistoryItem) => void;
}

const DiagnosisHistory: React.FC<DiagnosisHistoryProps> = ({ 
  history,
  onViewDetails
}) => {
  return (
    <div className="space-y-4">
      <h2 className="text-xl font-semibold mb-4">ประวัติการวินิจฉัย</h2>
      
      {history.length === 0 ? (
        <div className="text-center py-8">
          <p className="text-gray-500">ไม่พบประวัติการวินิจฉัย</p>
        </div>
      ) : (
        <div className="space-y-4">
          {history.map((item) => (
            <div 
              key={item.id} 
              className="border rounded-lg p-4 hover:shadow-md transition-shadow"
            >
              <div className="flex justify-between items-start mb-2">
                <div>
                  <h3 className="font-medium text-lg">{item.diagnosis.disease}</h3>
                  <div className="flex items-center mt-1">
                    <div 
                      className="h-2 w-16 bg-gray-200 rounded-full mr-2"
                      style={{ 
                        background: `linear-gradient(to right, ${getConfidenceColor(item.diagnosis.confidence)} ${item.diagnosis.confidence * 100}%, #e5e7eb ${item.diagnosis.confidence * 100}%)` 
                      }}
                    />
                    <span className="text-sm text-gray-600">
                      ความเชื่อมั่น {Math.round(item.diagnosis.confidence * 100)}%
                    </span>
                  </div>
                </div>
                <div className="flex items-center text-sm text-gray-500">
                  <ClockIcon className="h-4 w-4 mr-1" />
                  <span>{formatThaiDateTime(item.created_at)}</span>
                </div>
              </div>
              
              <div className="mt-2">
                <div className="flex flex-wrap gap-1 mb-3">
                  {item.symptoms.map((symptom, index) => (
                    <span 
                      key={index} 
                      className="inline-flex items-center px-2 py-1 rounded-full text-xs bg-blue-100 text-blue-800"
                    >
                      {symptom}
                    </span>
                  ))}
                </div>
                
                <p className="text-sm text-gray-600 line-clamp-2 mb-3">
                  {item.diagnosis.description}
                </p>
                
                <div className="flex justify-between items-center">
                  <div className="text-sm text-gray-500">
                    <FileTextIcon className="h-4 w-4 inline mr-1" />
                    <span>{item.diagnosis.recommendations.length} คำแนะนำ</span>
                    {item.diagnosis.differential_diagnoses.length > 0 && (
                      <span className="ml-3">
                        • {item.diagnosis.differential_diagnoses.length} การวินิจฉัยแยกโรค
                      </span>
                    )}
                  </div>
                  
                  <Button 
                    className="flex items-center bg-transparent border border-gray-300 hover:bg-gray-100 text-gray-700 text-sm px-3 py-1 rounded"
                    onClick={() => onViewDetails(item)}
                  >
                    <EyeIcon className="h-4 w-4 mr-1" />
                    ดูรายละเอียด
                  </Button>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default DiagnosisHistory; 