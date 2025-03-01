import { useState } from 'react';
import { DocumentTemplateType, ExtractedDocumentData } from '../components/DocumentImportAI';

interface UseDocumentImportAIProps {
  apiEndpoint?: string;
  onSuccess?: (data: ExtractedDocumentData) => void;
  onError?: (error: Error) => void;
}

interface UseDocumentImportAIReturn {
  processDocument: (imageFile: File, templateId?: string) => Promise<ExtractedDocumentData | null>;
  saveExtractedData: (data: ExtractedDocumentData, documentType: string) => Promise<boolean>;
  isProcessing: boolean;
  isSaving: boolean;
  error: string | null;
  availableTemplates: Array<{
    id: string;
    name: string;
    type: DocumentTemplateType;
  }>;
  resetState: () => void;
}

/**
 * ฮุคสำหรับจัดการการนำเข้าเอกสารและการประมวลผลด้วย AI
 */
const useDocumentImportAI = ({
  apiEndpoint = '/api/v1/document-import',
  onSuccess,
  onError
}: UseDocumentImportAIProps = {}): UseDocumentImportAIReturn => {
  const [isProcessing, setIsProcessing] = useState<boolean>(false);
  const [isSaving, setIsSaving] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);

  // เทมเพลตเอกสารที่มีอยู่ในระบบ (จำลองข้อมูล)
  const availableTemplates = [
    {
      id: 'template-001',
      name: 'แบบฟอร์มตรวจสุขภาพมาตรฐาน',
      type: DocumentTemplateType.HEALTH_CHECKUP
    },
    {
      id: 'template-002',
      name: 'ใบรับรองแพทย์ทั่วไป',
      type: DocumentTemplateType.MEDICAL_CERTIFICATE
    },
    {
      id: 'template-003',
      name: 'ผลตรวจทางห้องปฏิบัติการ',
      type: DocumentTemplateType.LAB_RESULT
    },
    {
      id: 'template-004',
      name: 'ใบสั่งยา',
      type: DocumentTemplateType.PRESCRIPTION
    }
  ];

  /**
   * ประมวลผลเอกสารด้วย AI
   * @param imageFile ไฟล์ภาพเอกสาร
   * @param templateId รหัสเทมเพลตเอกสาร (ถ้ามี)
   * @returns ข้อมูลที่สกัดได้จากเอกสาร
   */
  const processDocument = async (
    imageFile: File,
    templateId?: string
  ): Promise<ExtractedDocumentData | null> => {
    if (!imageFile) {
      setError('ไม่พบไฟล์ภาพ');
      return null;
    }

    setIsProcessing(true);
    setError(null);

    try {
      // สร้าง FormData สำหรับส่งไฟล์
      const formData = new FormData();
      formData.append('file', imageFile);
      
      if (templateId) {
        formData.append('templateId', templateId);
      }

      // จำลองการเรียก API
      // ในการใช้งานจริง ควรใช้ fetch หรือ axios เพื่อส่งข้อมูลไปยัง API
      // const response = await fetch(`${apiEndpoint}/process`, {
      //   method: 'POST',
      //   body: formData
      // });
      
      // if (!response.ok) {
      //   throw new Error(`การประมวลผลล้มเหลว: ${response.statusText}`);
      // }
      
      // const data = await response.json();

      // จำลองการประมวลผล (ในการใช้งานจริงให้ใช้ข้อมูลจาก API)
      await new Promise(resolve => setTimeout(resolve, 2000));
      
      // ข้อมูลจำลอง
      const mockData: ExtractedDocumentData = {
        documentType: DocumentTemplateType.HEALTH_CHECKUP,
        recognizedTemplate: 'แบบฟอร์มตรวจสุขภาพมาตรฐาน',
        confidence: 0.89,
        fields: {
          patientName: {
            value: 'นายสมชาย ใจดี',
            confidence: 0.95
          },
          patientID: {
            value: '1234567890123',
            confidence: 0.98
          },
          examinationDate: {
            value: '2025-03-01',
            confidence: 0.92
          },
          doctorName: {
            value: 'นพ.รักษา สุขภาพดี',
            confidence: 0.87
          }
        },
        tableData: [
          { test: 'ความดันโลหิต', result: '120/80 mmHg', normalRange: '90-120/60-80 mmHg', status: 'ปกติ' },
          { test: 'น้ำตาลในเลือด', result: '95 mg/dL', normalRange: '70-100 mg/dL', status: 'ปกติ' },
          { test: 'คอเลสเตอรอล', result: '210 mg/dL', normalRange: '<200 mg/dL', status: 'สูงกว่าปกติ' }
        ],
        rawText: 'ผลการตรวจสุขภาพ\nชื่อ-นามสกุล: นายสมชาย ใจดี\nเลขประจำตัวประชาชน: 1234567890123\nวันที่ตรวจ: 2025-03-01\nแพทย์ผู้ตรวจ: นพ.รักษา สุขภาพดี\n\nผลการตรวจ:\n1. ความดันโลหิต: 120/80 mmHg (ปกติ)\n2. น้ำตาลในเลือด: 95 mg/dL (ปกติ)\n3. คอเลสเตอรอล: 210 mg/dL (สูงกว่าปกติ)'
      };

      // เรียกใช้ callback onSuccess หากมีการกำหนด
      if (onSuccess) {
        onSuccess(mockData);
      }

      return mockData;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดที่ไม่ทราบสาเหตุ';
      setError(`เกิดข้อผิดพลาดในการประมวลผลเอกสาร: ${errorMessage}`);
      
      // เรียกใช้ callback onError หากมีการกำหนด
      if (onError && err instanceof Error) {
        onError(err);
      }
      
      return null;
    } finally {
      setIsProcessing(false);
    }
  };

  /**
   * บันทึกข้อมูลที่สกัดได้จากเอกสาร
   * @param data ข้อมูลที่สกัดได้
   * @param documentType ประเภทเอกสาร
   * @returns สถานะการบันทึก (true = สำเร็จ, false = ล้มเหลว)
   */
  const saveExtractedData = async (
    data: ExtractedDocumentData,
    documentType: string
  ): Promise<boolean> => {
    setIsSaving(true);
    setError(null);

    try {
      // จำลองการเรียก API
      // ในการใช้งานจริง ควรใช้ fetch หรือ axios เพื่อส่งข้อมูลไปยัง API
      // const response = await fetch(`${apiEndpoint}/save`, {
      //   method: 'POST',
      //   headers: {
      //     'Content-Type': 'application/json'
      //   },
      //   body: JSON.stringify({
      //     data,
      //     documentType
      //   })
      // });
      
      // if (!response.ok) {
      //   throw new Error(`การบันทึกล้มเหลว: ${response.statusText}`);
      // }
      
      // const result = await response.json();

      // จำลองการบันทึกข้อมูล
      await new Promise(resolve => setTimeout(resolve, 1500));
      
      return true;
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'เกิดข้อผิดพลาดที่ไม่ทราบสาเหตุ';
      setError(`เกิดข้อผิดพลาดในการบันทึกข้อมูล: ${errorMessage}`);
      
      // เรียกใช้ callback onError หากมีการกำหนด
      if (onError && err instanceof Error) {
        onError(err);
      }
      
      return false;
    } finally {
      setIsSaving(false);
    }
  };

  /**
   * รีเซ็ตสถานะทั้งหมด
   */
  const resetState = () => {
    setIsProcessing(false);
    setIsSaving(false);
    setError(null);
  };

  return {
    processDocument,
    saveExtractedData,
    isProcessing,
    isSaving,
    error,
    availableTemplates,
    resetState
  };
};

export default useDocumentImportAI; 