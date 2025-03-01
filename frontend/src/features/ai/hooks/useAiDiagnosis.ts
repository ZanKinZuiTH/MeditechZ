import { useState } from 'react';
import axios from 'axios';
import { DiagnosisResultType } from '../components/DiagnosisResult';

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

interface DiagnosisRequest {
  symptoms: string[];
  patient_id?: number;
  patient_data?: PatientData;
}

export const useAiDiagnosis = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [diagnosisResult, setDiagnosisResult] = useState<DiagnosisResultType | null>(null);

  const diagnose = async (symptoms: string[], patientData?: PatientData) => {
    setIsLoading(true);
    setError(null);

    try {
      const request: DiagnosisRequest = {
        symptoms,
        ...(patientData?.patient_id && { patient_id: patientData.patient_id }),
        ...(patientData && { patient_data: patientData })
      };

      // ในสภาพแวดล้อมจริง จะต้องเรียกใช้ API
      // const response = await axios.post<DiagnosisResultType>(
      //   `${process.env.REACT_APP_API_URL}/api/v1/ai/diagnose`,
      //   request
      // );
      // setDiagnosisResult(response.data);

      // จำลองการเรียกใช้ API ด้วยข้อมูลตัวอย่าง
      await new Promise(resolve => setTimeout(resolve, 1500)); // จำลองความล่าช้าของเครือข่าย

      // สร้างผลลัพธ์จำลองตามอาการ
      let mockResult: DiagnosisResultType;

      if (symptoms.includes('ไข้') && symptoms.includes('ไอ') && symptoms.includes('เจ็บคอ')) {
        if (symptoms.includes('หายใจลำบาก')) {
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
              'ดื่มน้ำมากๆ'
            ],
            differential_diagnoses: [
              {
                disease: 'ไข้หวัดใหญ่',
                confidence: 0.65,
                description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสอินฟลูเอนซา'
              },
              {
                disease: 'ไข้หวัดธรรมดา',
                confidence: 0.45,
                description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสหลายชนิด'
              }
            ]
          };
        } else {
          mockResult = {
            disease: 'ไข้หวัดใหญ่',
            confidence: 0.75,
            description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสอินฟลูเอนซา',
            recommendations: [
              'พักผ่อนให้เพียงพอ',
              'ดื่มน้ำมากๆ',
              'ทานยาลดไข้ตามคำแนะนำของแพทย์',
              'หลีกเลี่ยงการสัมผัสใกล้ชิดกับผู้อื่นเพื่อป้องกันการแพร่เชื้อ',
              'สวมหน้ากากอนามัยเมื่อต้องอยู่ร่วมกับผู้อื่น'
            ],
            differential_diagnoses: [
              {
                disease: 'ไข้หวัดธรรมดา',
                confidence: 0.65,
                description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสหลายชนิด'
              },
              {
                disease: 'โควิด-19',
                confidence: 0.40,
                description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสโคโรนา SARS-CoV-2'
              }
            ]
          };
        }
      } else if (symptoms.includes('ปวดหัว') && symptoms.includes('คลื่นไส้')) {
        mockResult = {
          disease: 'ไมเกรน',
          confidence: 0.8,
          description: 'อาการปวดศีรษะข้างเดียวหรือสองข้าง มักมีอาการคลื่นไส้ร่วมด้วย',
          recommendations: [
            'พักผ่อนในที่เงียบและมืด',
            'ประคบเย็นหรือร้อนบริเวณที่ปวด',
            'ทานยาแก้ปวดตามคำแนะนำของแพทย์',
            'หลีกเลี่ยงสิ่งกระตุ้น เช่น แสงจ้า เสียงดัง กลิ่นฉุน',
            'พยายามนอนหลับให้เป็นเวลา'
          ],
          differential_diagnoses: [
            {
              disease: 'ปวดศีรษะจากความเครียด',
              confidence: 0.55,
              description: 'อาการปวดศีรษะที่เกิดจากความเครียดหรือความวิตกกังวล'
            },
            {
              disease: 'ปวดศีรษะคลัสเตอร์',
              confidence: 0.35,
              description: 'อาการปวดศีรษะรุนแรงที่เกิดเป็นชุดๆ มักปวดรอบดวงตาข้างเดียว'
            }
          ]
        };
      } else if (symptoms.includes('ท้องเสีย') && symptoms.includes('คลื่นไส้') && symptoms.includes('อาเจียน')) {
        mockResult = {
          disease: 'อาหารเป็นพิษ',
          confidence: 0.9,
          description: 'ภาวะที่เกิดจากการรับประทานอาหารที่ปนเปื้อนเชื้อแบคทีเรียหรือสารพิษ',
          recommendations: [
            'ดื่มน้ำมากๆ เพื่อป้องกันภาวะขาดน้ำ',
            'ดื่มสารละลายเกลือแร่',
            'รับประทานอาหารอ่อนๆ ย่อยง่าย',
            'หลีกเลี่ยงอาหารรสจัด อาหารมัน และนม',
            'พักผ่อนให้เพียงพอ',
            'หากอาการไม่ดีขึ้นภายใน 2 วัน ควรพบแพทย์'
          ],
          differential_diagnoses: [
            {
              disease: 'กระเพาะอาหารอักเสบ',
              confidence: 0.6,
              description: 'ภาวะที่เยื่อบุกระเพาะอาหารเกิดการอักเสบ'
            },
            {
              disease: 'ลำไส้แปรปรวน',
              confidence: 0.4,
              description: 'ความผิดปกติของลำไส้ที่ทำให้มีอาการปวดท้อง ท้องเสีย หรือท้องผูก'
            }
          ]
        };
      } else if (symptoms.includes('ผื่น')) {
        mockResult = {
          disease: 'ลมพิษ',
          confidence: 0.7,
          description: 'ผื่นนูนแดงคันที่เกิดจากการแพ้',
          recommendations: [
            'หลีกเลี่ยงสิ่งที่แพ้',
            'ทายาแก้แพ้ตามคำแนะนำของแพทย์',
            'ประคบเย็นบริเวณที่มีผื่น',
            'สวมเสื้อผ้าหลวมๆ ไม่ระคายผิว',
            'หากมีอาการรุนแรง เช่น หายใจลำบาก ควรรีบพบแพทย์ทันที'
          ],
          differential_diagnoses: [
            {
              disease: 'ผื่นแพ้สัมผัส',
              confidence: 0.55,
              description: 'ผื่นที่เกิดจากการสัมผัสสารที่แพ้โดยตรง'
            },
            {
              disease: 'โรคสะเก็ดเงิน',
              confidence: 0.3,
              description: 'โรคผิวหนังเรื้อรังที่ทำให้เกิดผื่นหนาสีแดงและมีสะเก็ด'
            }
          ]
        };
      } else {
        // กรณีไม่สามารถวินิจฉัยได้ชัดเจน
        mockResult = {
          disease: 'ไข้หวัดธรรมดา',
          confidence: 0.5,
          description: 'โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสหลายชนิด',
          recommendations: [
            'พักผ่อนให้เพียงพอ',
            'ดื่มน้ำมากๆ',
            'ทานยาลดไข้ตามอาการ',
            'หากอาการไม่ดีขึ้นภายใน 3-5 วัน ควรพบแพทย์'
          ],
          differential_diagnoses: [
            {
              disease: 'ภูมิแพ้',
              confidence: 0.4,
              description: 'ปฏิกิริยาของระบบภูมิคุ้มกันต่อสารก่อภูมิแพ้'
            }
          ]
        };
      }

      setDiagnosisResult(mockResult);
    } catch (err) {
      console.error('Error diagnosing symptoms:', err);
      setError(err instanceof Error ? err.message : 'เกิดข้อผิดพลาดในการวินิจฉัย');
    } finally {
      setIsLoading(false);
    }
  };

  return {
    diagnose,
    diagnosisResult,
    isLoading,
    error,
    reset: () => {
      setDiagnosisResult(null);
      setError(null);
    }
  };
};

export default useAiDiagnosis; 