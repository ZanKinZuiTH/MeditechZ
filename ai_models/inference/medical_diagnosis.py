"""
MeditechZ AI Medical Diagnosis Module

โมดูลนี้ใช้สำหรับวิเคราะห์ข้อมูลทางการแพทย์และให้การวินิจฉัยเบื้องต้น
โดยใช้โมเดล AI ที่ผ่านการฝึกฝนด้วยข้อมูลทางการแพทย์

Author: ทีมพัฒนา MeditechZ
Version: 1.0.0
Date: March 1, 2025
"""

import os
import json
import numpy as np
from typing import Dict, List, Tuple, Optional, Union
from pathlib import Path
import logging

# ในสถานการณ์จริง จะต้อง import โมเดล AI ที่เหมาะสม
# import torch
# from transformers import AutoModelForSequenceClassification, AutoTokenizer

# สร้าง logger
logger = logging.getLogger("meditech.ai")
logger.setLevel(logging.INFO)

class MedicalDiagnosisAI:
    """
    คลาสสำหรับวิเคราะห์ข้อมูลทางการแพทย์และให้การวินิจฉัยเบื้องต้น
    
    คลาสนี้ใช้โมเดล AI ที่ผ่านการฝึกฝนด้วยข้อมูลทางการแพทย์
    เพื่อวิเคราะห์อาการและให้การวินิจฉัยเบื้องต้น
    """
    
    def __init__(self, model_path: str = "models/diagnosis"):
        """
        สร้าง instance ของ MedicalDiagnosisAI
        
        Args:
            model_path (str): พาธไปยังโมเดล AI
        """
        self.model_path = model_path
        self.model = None
        self.tokenizer = None
        self.symptoms_map = {}
        self.diseases_map = {}
        self.confidence_threshold = 0.7
        
        # โหลดข้อมูลการแมปอาการและโรค
        self._load_mappings()
        
        # โหลดโมเดล (ในสถานการณ์จริง)
        # self._load_model()
        
        logger.info(f"Initialized MedicalDiagnosisAI with model from {model_path}")
    
    def _load_mappings(self) -> None:
        """
        โหลดข้อมูลการแมปอาการและโรค
        
        ข้อมูลนี้ใช้สำหรับแปลงข้อมูลอาการเป็นรูปแบบที่โมเดลเข้าใจ
        และแปลงผลลัพธ์จากโมเดลเป็นชื่อโรค
        """
        try:
            # ในสถานการณ์จริง จะต้องโหลดจากไฟล์
            # with open(os.path.join(self.model_path, "symptoms_map.json"), "r", encoding="utf-8") as f:
            #     self.symptoms_map = json.load(f)
            
            # with open(os.path.join(self.model_path, "diseases_map.json"), "r", encoding="utf-8") as f:
            #     self.diseases_map = json.load(f)
            
            # ตัวอย่างข้อมูลจำลอง
            self.symptoms_map = {
                "ไข้": 0,
                "ไอ": 1,
                "เจ็บคอ": 2,
                "ปวดหัว": 3,
                "อ่อนเพลีย": 4,
                "หายใจลำบาก": 5,
                "คลื่นไส้": 6,
                "อาเจียน": 7,
                "ท้องเสีย": 8,
                "ปวดท้อง": 9,
                "ผื่น": 10,
                "ปวดกล้ามเนื้อ": 11,
                "ปวดข้อ": 12,
                "วิงเวียน": 13,
                "หนาวสั่น": 14
            }
            
            self.diseases_map = {
                0: {"name": "ไข้หวัดใหญ่", "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสอินฟลูเอนซา"},
                1: {"name": "ไข้หวัดธรรมดา", "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสหลายชนิด"},
                2: {"name": "โควิด-19", "description": "โรคติดเชื้อทางเดินหายใจที่เกิดจากเชื้อไวรัสโคโรนา SARS-CoV-2"},
                3: {"name": "ไข้เลือดออก", "description": "โรคติดเชื้อที่เกิดจากเชื้อไวรัสเดงกี่ ติดต่อผ่านยุงลาย"},
                4: {"name": "อาหารเป็นพิษ", "description": "ภาวะที่เกิดจากการรับประทานอาหารที่ปนเปื้อนเชื้อแบคทีเรียหรือสารพิษ"},
                5: {"name": "กระเพาะอาหารอักเสบ", "description": "ภาวะที่เยื่อบุกระเพาะอาหารเกิดการอักเสบ"},
                6: {"name": "ไมเกรน", "description": "อาการปวดศีรษะข้างเดียวหรือสองข้าง มักมีอาการคลื่นไส้ร่วมด้วย"},
                7: {"name": "ภูมิแพ้", "description": "ปฏิกิริยาของระบบภูมิคุ้มกันต่อสารก่อภูมิแพ้"},
                8: {"name": "ลมพิษ", "description": "ผื่นนูนแดงคันที่เกิดจากการแพ้"},
                9: {"name": "ปอดอักเสบ", "description": "การอักเสบของเนื้อปอด มักเกิดจากการติดเชื้อ"}
            }
            
            logger.info("Loaded symptom and disease mappings successfully")
        except Exception as e:
            logger.error(f"Error loading mappings: {e}")
            raise
    
    def _load_model(self) -> None:
        """
        โหลดโมเดล AI สำหรับวินิจฉัยโรค
        
        ในสถานการณ์จริง จะต้องโหลดโมเดลที่ผ่านการฝึกฝนแล้ว
        """
        try:
            # ในสถานการณ์จริง
            # self.tokenizer = AutoTokenizer.from_pretrained(self.model_path)
            # self.model = AutoModelForSequenceClassification.from_pretrained(self.model_path)
            # self.model.eval()
            
            logger.info("Model loaded successfully")
        except Exception as e:
            logger.error(f"Error loading model: {e}")
            raise
    
    def preprocess_symptoms(self, symptoms: List[str]) -> np.ndarray:
        """
        แปลงรายการอาการเป็นรูปแบบที่โมเดลเข้าใจ
        
        Args:
            symptoms (List[str]): รายการอาการของผู้ป่วย
            
        Returns:
            np.ndarray: เวกเตอร์ที่แทนอาการ
        """
        # สร้างเวกเตอร์ที่มีค่าเป็น 0 ทั้งหมด
        symptom_vector = np.zeros(len(self.symptoms_map))
        
        # กำหนดค่า 1 ให้กับอาการที่มี
        for symptom in symptoms:
            if symptom in self.symptoms_map:
                symptom_vector[self.symptoms_map[symptom]] = 1
        
        return symptom_vector
    
    def diagnose(self, symptoms: List[str], patient_data: Optional[Dict] = None) -> Dict:
        """
        วินิจฉัยโรคจากอาการและข้อมูลผู้ป่วย
        
        Args:
            symptoms (List[str]): รายการอาการของผู้ป่วย
            patient_data (Dict, optional): ข้อมูลผู้ป่วยเพิ่มเติม เช่น อายุ, เพศ, ประวัติการรักษา
            
        Returns:
            Dict: ผลการวินิจฉัย ประกอบด้วย
                - disease: ชื่อโรคที่วินิจฉัย
                - confidence: ความมั่นใจในการวินิจฉัย (0-1)
                - description: คำอธิบายโรค
                - recommendations: คำแนะนำเบื้องต้น
                - differential_diagnoses: การวินิจฉัยแยกโรคอื่นๆ ที่เป็นไปได้
        """
        # แปลงอาการเป็นรูปแบบที่โมเดลเข้าใจ
        symptom_vector = self.preprocess_symptoms(symptoms)
        
        # ในสถานการณ์จริง จะต้องใช้โมเดลทำนาย
        # with torch.no_grad():
        #     outputs = self.model(torch.tensor(symptom_vector).unsqueeze(0))
        #     probabilities = torch.softmax(outputs.logits, dim=1)[0]
        #     predicted_class = torch.argmax(probabilities).item()
        #     confidence = probabilities[predicted_class].item()
        
        # ตัวอย่างการจำลองผลลัพธ์
        # สร้างผลลัพธ์จำลองตามอาการ
        if "ไข้" in symptoms and "ไอ" in symptoms and "เจ็บคอ" in symptoms:
            if "หายใจลำบาก" in symptoms:
                predicted_class = 2  # โควิด-19
                confidence = 0.85
            else:
                predicted_class = 0  # ไข้หวัดใหญ่
                confidence = 0.75
        elif "ปวดหัว" in symptoms and "คลื่นไส้" in symptoms:
            predicted_class = 6  # ไมเกรน
            confidence = 0.8
        elif "ท้องเสีย" in symptoms and "คลื่นไส้" in symptoms and "อาเจียน" in symptoms:
            predicted_class = 4  # อาหารเป็นพิษ
            confidence = 0.9
        elif "ผื่น" in symptoms:
            predicted_class = 8  # ลมพิษ
            confidence = 0.7
        else:
            # กรณีไม่สามารถวินิจฉัยได้ชัดเจน
            predicted_class = 1  # ไข้หวัดธรรมดา
            confidence = 0.5
        
        # สร้างผลการวินิจฉัยแยกโรคอื่นๆ ที่เป็นไปได้
        differential_diagnoses = []
        for disease_id, disease_info in self.diseases_map.items():
            if disease_id != predicted_class:
                # จำลองค่าความมั่นใจสำหรับโรคอื่นๆ
                diff_confidence = np.random.uniform(0.1, confidence - 0.1)
                if diff_confidence > 0.3:  # แสดงเฉพาะโรคที่มีความเป็นไปได้พอสมควร
                    differential_diagnoses.append({
                        "disease": disease_info["name"],
                        "confidence": round(diff_confidence, 2),
                        "description": disease_info["description"]
                    })
        
        # เรียงลำดับตามความมั่นใจ
        differential_diagnoses.sort(key=lambda x: x["confidence"], reverse=True)
        
        # สร้างคำแนะนำเบื้องต้น
        recommendations = self._generate_recommendations(predicted_class, symptoms)
        
        # สร้างผลการวินิจฉัย
        diagnosis_result = {
            "disease": self.diseases_map[predicted_class]["name"],
            "confidence": round(confidence, 2),
            "description": self.diseases_map[predicted_class]["description"],
            "recommendations": recommendations,
            "differential_diagnoses": differential_diagnoses[:3]  # แสดงเฉพาะ 3 โรคแรก
        }
        
        logger.info(f"Diagnosis completed: {diagnosis_result['disease']} with confidence {diagnosis_result['confidence']}")
        
        return diagnosis_result
    
    def _generate_recommendations(self, disease_id: int, symptoms: List[str]) -> List[str]:
        """
        สร้างคำแนะนำเบื้องต้นตามโรคที่วินิจฉัย
        
        Args:
            disease_id (int): รหัสโรคที่วินิจฉัย
            symptoms (List[str]): รายการอาการของผู้ป่วย
            
        Returns:
            List[str]: รายการคำแนะนำเบื้องต้น
        """
        # คำแนะนำทั่วไป
        general_recommendations = [
            "พักผ่อนให้เพียงพอ",
            "ดื่มน้ำมากๆ",
            "หากอาการไม่ดีขึ้นภายใน 2-3 วัน ควรพบแพทย์"
        ]
        
        # คำแนะนำเฉพาะโรค
        specific_recommendations = {
            0: [  # ไข้หวัดใหญ่
                "ทานยาลดไข้ตามคำแนะนำของแพทย์",
                "หลีกเลี่ยงการสัมผัสใกล้ชิดกับผู้อื่นเพื่อป้องกันการแพร่เชื้อ",
                "สวมหน้ากากอนามัยเมื่อต้องอยู่ร่วมกับผู้อื่น"
            ],
            1: [  # ไข้หวัดธรรมดา
                "ทานยาลดไข้ตามคำแนะนำของแพทย์",
                "ใช้ยาพ่นจมูกหากมีอาการคัดจมูก",
                "ดื่มน้ำอุ่นผสมน้ำผึ้งและมะนาวเพื่อบรรเทาอาการเจ็บคอ"
            ],
            2: [  # โควิด-19
                "แยกตัวจากผู้อื่นทันที",
                "ตรวจหาเชื้อโควิด-19 ด้วยชุดตรวจ ATK หรือ RT-PCR",
                "ติดต่อสายด่วนโควิด-19 หรือสถานพยาบาลใกล้บ้าน",
                "ตรวจวัดระดับออกซิเจนในเลือดอย่างสม่ำเสมอ (ถ้ามีเครื่องวัด)"
            ],
            3: [  # ไข้เลือดออก
                "ทานยาลดไข้ตามคำแนะนำของแพทย์ (ห้ามทานยาแอสไพริน)",
                "ตรวจวัดระดับเกล็ดเลือด",
                "สังเกตจุดเลือดออกตามผิวหนัง",
                "ควรพบแพทย์โดยเร็วที่สุด"
            ],
            4: [  # อาหารเป็นพิษ
                "งดอาหารและน้ำ 1-2 ชั่วโมง แล้วค่อยๆ จิบน้ำ",
                "ทานอาหารอ่อนๆ เช่น ข้าวต้ม โจ๊ก",
                "ดื่มสารละลายเกลือแร่เพื่อป้องกันการขาดน้ำ"
            ],
            5: [  # กระเพาะอาหารอักเสบ
                "ทานอาหารอ่อนๆ และหลีกเลี่ยงอาหารรสจัด",
                "หลีกเลี่ยงเครื่องดื่มที่มีคาเฟอีนและแอลกอฮอล์",
                "ทานยาลดกรดตามคำแนะนำของแพทย์"
            ],
            6: [  # ไมเกรน
                "พักในที่เงียบและมืด",
                "ประคบเย็นบริเวณที่ปวด",
                "ทานยาแก้ปวดตามคำแนะนำของแพทย์"
            ],
            7: [  # ภูมิแพ้
                "หลีกเลี่ยงสารก่อภูมิแพ้",
                "ทานยาแก้แพ้ตามคำแนะนำของแพทย์",
                "ล้างจมูกด้วยน้ำเกลือ"
            ],
            8: [  # ลมพิษ
                "หลีกเลี่ยงการเกา",
                "ทานยาแก้แพ้ตามคำแนะนำของแพทย์",
                "ประคบเย็นบริเวณที่มีผื่น"
            ],
            9: [  # ปอดอักเสบ
                "ควรพบแพทย์โดยเร็วที่สุด",
                "พักผ่อนมากๆ",
                "ทานยาตามที่แพทย์สั่งอย่างเคร่งครัด"
            ]
        }
        
        # รวมคำแนะนำทั่วไปและคำแนะนำเฉพาะโรค
        recommendations = specific_recommendations.get(disease_id, []) + general_recommendations
        
        # เพิ่มคำแนะนำตามอาการ
        if "ไข้" in symptoms:
            recommendations.append("เช็ดตัวด้วยน้ำอุ่นเพื่อลดไข้")
        
        if "ไอ" in symptoms:
            recommendations.append("ดื่มน้ำอุ่นผสมน้ำผึ้งเพื่อบรรเทาอาการไอ")
        
        if "เจ็บคอ" in symptoms:
            recommendations.append("กลั้วคอด้วยน้ำเกลืออุ่นเพื่อบรรเทาอาการเจ็บคอ")
        
        return recommendations

# ตัวอย่างการใช้งาน
if __name__ == "__main__":
    # สร้าง instance ของ MedicalDiagnosisAI
    ai_diagnosis = MedicalDiagnosisAI()
    
    # ตัวอย่างอาการ
    symptoms = ["ไข้", "ไอ", "เจ็บคอ", "ปวดหัว"]
    
    # ตัวอย่างข้อมูลผู้ป่วย
    patient_data = {
        "age": 35,
        "gender": "male",
        "medical_history": ["ภูมิแพ้"]
    }
    
    # วินิจฉัยโรค
    diagnosis_result = ai_diagnosis.diagnose(symptoms, patient_data)
    
    # แสดงผลการวินิจฉัย
    print(f"Disease: {diagnosis_result['disease']}")
    print(f"Confidence: {diagnosis_result['confidence']}")
    print(f"Description: {diagnosis_result['description']}")
    print("Recommendations:")
    for rec in diagnosis_result['recommendations']:
        print(f"- {rec}")
    print("Differential Diagnoses:")
    for diff in diagnosis_result['differential_diagnoses']:
        print(f"- {diff['disease']} (Confidence: {diff['confidence']})") 