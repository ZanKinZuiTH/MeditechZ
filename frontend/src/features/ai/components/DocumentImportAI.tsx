import React, { useState, useRef } from 'react';
import { 
  Box, 
  Button, 
  Typography, 
  Paper, 
  CircularProgress, 
  Alert, 
  AlertTitle,
  Stepper,
  Step,
  StepLabel,
  Grid,
  Card,
  CardContent,
  Chip,
  Divider,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem
} from '@mui/material';
import { 
  CloudUpload as CloudUploadIcon, 
  Image as ImageIcon,
  TableChart as TableChartIcon,
  Save as SaveIcon,
  Check as CheckIcon,
  Error as ErrorIcon,
  AutoAwesome as AutoAwesomeIcon
} from '@mui/icons-material';

// ประเภทของเอกสารที่ระบบสามารถจดจำได้
export enum DocumentTemplateType {
  GENERAL_MEDICAL_RECORD = 'general_medical_record',
  LAB_RESULT = 'lab_result',
  PRESCRIPTION = 'prescription',
  MEDICAL_CERTIFICATE = 'medical_certificate',
  HEALTH_CHECKUP = 'health_checkup',
  CUSTOM = 'custom'
}

// ข้อมูลที่สกัดได้จากเอกสาร
export interface ExtractedDocumentData {
  documentType: DocumentTemplateType;
  recognizedTemplate?: string;
  confidence: number;
  fields: {
    [key: string]: {
      value: string;
      confidence: number;
    }
  };
  tableData?: Array<{
    [key: string]: string;
  }>;
  rawText?: string;
}

// คุณสมบัติของคอมโพเนนต์
export interface DocumentImportAIProps {
  onDataExtracted?: (data: ExtractedDocumentData) => void;
  onSaveDocument?: (data: ExtractedDocumentData, documentType: string) => Promise<boolean>;
  availableTemplates?: Array<{
    id: string;
    name: string;
    type: DocumentTemplateType;
  }>;
}

const DocumentImportAI: React.FC<DocumentImportAIProps> = ({
  onDataExtracted,
  onSaveDocument,
  availableTemplates = []
}) => {
  // สถานะต่างๆ
  const [activeStep, setActiveStep] = useState<number>(0);
  const [isProcessing, setIsProcessing] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  const [uploadedImage, setUploadedImage] = useState<string | null>(null);
  const [extractedData, setExtractedData] = useState<ExtractedDocumentData | null>(null);
  const [selectedTemplate, setSelectedTemplate] = useState<string>('');
  const [isSaving, setIsSaving] = useState<boolean>(false);
  const [saveSuccess, setSaveSuccess] = useState<boolean>(false);
  const [documentType, setDocumentType] = useState<string>('');
  
  // Ref สำหรับ input file
  const fileInputRef = useRef<HTMLInputElement>(null);

  // ขั้นตอนในการนำเข้าเอกสาร
  const steps = ['อัพโหลดภาพเอกสาร', 'ประมวลผลด้วย AI', 'ตรวจสอบและแก้ไข', 'บันทึกข้อมูล'];

  // จำลองการอัพโหลดไฟล์
  const handleFileUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    setError(null);
    const file = event.target.files?.[0];
    
    if (!file) {
      return;
    }
    
    // ตรวจสอบประเภทไฟล์
    if (!file.type.includes('image/')) {
      setError('กรุณาอัพโหลดไฟล์ภาพเท่านั้น (JPEG, PNG, etc.)');
      return;
    }
    
    // จำลองการแสดงภาพที่อัพโหลด
    const reader = new FileReader();
    reader.onload = (e) => {
      setUploadedImage(e.target?.result as string);
    };
    reader.readAsDataURL(file);
  };

  // จำลองการประมวลผลด้วย AI
  const processImage = async () => {
    if (!uploadedImage) {
      setError('กรุณาอัพโหลดภาพก่อน');
      return;
    }
    
    setIsProcessing(true);
    setError(null);
    
    try {
      // จำลองการเรียก API สำหรับประมวลผลภาพด้วย AI
      await new Promise(resolve => setTimeout(resolve, 2000));
      
      // ข้อมูลจำลองที่ได้จากการประมวลผล
      const mockExtractedData: ExtractedDocumentData = {
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
      
      setExtractedData(mockExtractedData);
      
      // เรียกใช้ callback หากมีการกำหนด
      if (onDataExtracted) {
        onDataExtracted(mockExtractedData);
      }
      
      // ไปยังขั้นตอนถัดไป
      setActiveStep(2);
    } catch (err) {
      setError('เกิดข้อผิดพลาดในการประมวลผลภาพ: ' + (err instanceof Error ? err.message : 'ไม่ทราบสาเหตุ'));
    } finally {
      setIsProcessing(false);
    }
  };

  // จำลองการบันทึกข้อมูล
  const handleSaveDocument = async () => {
    if (!extractedData) {
      setError('ไม่มีข้อมูลที่จะบันทึก');
      return;
    }
    
    setIsSaving(true);
    setError(null);
    
    try {
      // เรียกใช้ callback สำหรับการบันทึกหากมีการกำหนด
      if (onSaveDocument) {
        const success = await onSaveDocument(extractedData, documentType || extractedData.documentType);
        setSaveSuccess(success);
      } else {
        // จำลองการบันทึกสำเร็จ
        await new Promise(resolve => setTimeout(resolve, 1500));
        setSaveSuccess(true);
      }
      
      // ไปยังขั้นตอนถัดไป
      setActiveStep(3);
    } catch (err) {
      setError('เกิดข้อผิดพลาดในการบันทึกข้อมูล: ' + (err instanceof Error ? err.message : 'ไม่ทราบสาเหตุ'));
    } finally {
      setIsSaving(false);
    }
  };

  // เริ่มต้นใหม่
  const handleReset = () => {
    setActiveStep(0);
    setUploadedImage(null);
    setExtractedData(null);
    setError(null);
    setSaveSuccess(false);
    setSelectedTemplate('');
    setDocumentType('');
  };

  // เริ่มการประมวลผลด้วย AI
  const startProcessing = () => {
    setActiveStep(1);
    processImage();
  };

  // แสดงเนื้อหาตามขั้นตอนปัจจุบัน
  const getStepContent = (step: number) => {
    switch (step) {
      case 0:
        return (
          <Box sx={{ textAlign: 'center', p: 3 }}>
            <input
              type="file"
              accept="image/*"
              style={{ display: 'none' }}
              ref={fileInputRef}
              onChange={handleFileUpload}
            />
            <Button
              variant="contained"
              startIcon={<CloudUploadIcon />}
              onClick={() => fileInputRef.current?.click()}
              sx={{ mb: 2 }}
            >
              อัพโหลดภาพเอกสาร
            </Button>
            
            {uploadedImage && (
              <Box sx={{ mt: 2 }}>
                <Typography variant="subtitle1" gutterBottom>
                  ภาพที่อัพโหลด:
                </Typography>
                <Box
                  component="img"
                  src={uploadedImage}
                  alt="Uploaded document"
                  sx={{
                    maxWidth: '100%',
                    maxHeight: '400px',
                    border: '1px solid #ddd',
                    borderRadius: 1
                  }}
                />
                <Box sx={{ mt: 2 }}>
                  <FormControl fullWidth sx={{ mb: 2 }}>
                    <InputLabel>เลือกเทมเพลตเอกสาร (ถ้ามี)</InputLabel>
                    <Select
                      value={selectedTemplate}
                      onChange={(e) => setSelectedTemplate(e.target.value as string)}
                      label="เลือกเทมเพลตเอกสาร (ถ้ามี)"
                    >
                      <MenuItem value="">
                        <em>ตรวจจับอัตโนมัติ</em>
                      </MenuItem>
                      {availableTemplates.map((template) => (
                        <MenuItem key={template.id} value={template.id}>
                          {template.name}
                        </MenuItem>
                      ))}
                    </Select>
                  </FormControl>
                  
                  <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AutoAwesomeIcon />}
                    onClick={startProcessing}
                  >
                    เริ่มประมวลผลด้วย AI
                  </Button>
                </Box>
              </Box>
            )}
          </Box>
        );
      
      case 1:
        return (
          <Box sx={{ textAlign: 'center', p: 3 }}>
            <CircularProgress size={60} />
            <Typography variant="h6" sx={{ mt: 2 }}>
              กำลังประมวลผลภาพด้วย AI...
            </Typography>
            <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
              ระบบกำลังวิเคราะห์ภาพและสกัดข้อมูลจากเอกสาร
            </Typography>
          </Box>
        );
      
      case 2:
        return (
          <Box sx={{ p: 2 }}>
            {extractedData && (
              <>
                <Paper elevation={3} sx={{ p: 2, mb: 3 }}>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="h6">
                      ข้อมูลที่สกัดได้
                    </Typography>
                    <Chip
                      icon={<AutoAwesomeIcon />}
                      label={`ความมั่นใจ: ${(extractedData.confidence * 100).toFixed(0)}%`}
                      color={extractedData.confidence > 0.8 ? "success" : extractedData.confidence > 0.6 ? "warning" : "error"}
                    />
                  </Box>
                  
                  <Grid container spacing={2}>
                    <Grid item xs={12} md={6}>
                      <Card variant="outlined" sx={{ mb: 2 }}>
                        <CardContent>
                          <Typography variant="subtitle1" gutterBottom>
                            ประเภทเอกสารที่ตรวจพบ
                          </Typography>
                          <Typography variant="body1">
                            {extractedData.recognizedTemplate || 'ไม่สามารถระบุได้'}
                          </Typography>
                          
                          <FormControl fullWidth sx={{ mt: 2 }}>
                            <InputLabel>เลือกประเภทเอกสาร</InputLabel>
                            <Select
                              value={documentType || extractedData.documentType}
                              onChange={(e) => setDocumentType(e.target.value)}
                              label="เลือกประเภทเอกสาร"
                            >
                              <MenuItem value={DocumentTemplateType.GENERAL_MEDICAL_RECORD}>เวชระเบียนทั่วไป</MenuItem>
                              <MenuItem value={DocumentTemplateType.LAB_RESULT}>ผลตรวจทางห้องปฏิบัติการ</MenuItem>
                              <MenuItem value={DocumentTemplateType.PRESCRIPTION}>ใบสั่งยา</MenuItem>
                              <MenuItem value={DocumentTemplateType.MEDICAL_CERTIFICATE}>ใบรับรองแพทย์</MenuItem>
                              <MenuItem value={DocumentTemplateType.HEALTH_CHECKUP}>ผลตรวจสุขภาพ</MenuItem>
                              <MenuItem value={DocumentTemplateType.CUSTOM}>กำหนดเอง</MenuItem>
                            </Select>
                          </FormControl>
                        </CardContent>
                      </Card>
                    </Grid>
                    
                    <Grid item xs={12} md={6}>
                      <Card variant="outlined">
                        <CardContent>
                          <Typography variant="subtitle1" gutterBottom>
                            ข้อมูลหลัก
                          </Typography>
                          {Object.entries(extractedData.fields).map(([key, field]) => (
                            <Box key={key} sx={{ mb: 2 }}>
                              <TextField
                                fullWidth
                                label={key.replace(/([A-Z])/g, ' $1').replace(/^./, str => str.toUpperCase())}
                                defaultValue={field.value}
                                helperText={`ความมั่นใจ: ${(field.confidence * 100).toFixed(0)}%`}
                                variant="outlined"
                                size="small"
                              />
                            </Box>
                          ))}
                        </CardContent>
                      </Card>
                    </Grid>
                    
                    {extractedData.tableData && extractedData.tableData.length > 0 && (
                      <Grid item xs={12}>
                        <Card variant="outlined">
                          <CardContent>
                            <Typography variant="subtitle1" gutterBottom>
                              ข้อมูลตาราง
                            </Typography>
                            <Box sx={{ overflowX: 'auto' }}>
                              <table style={{ width: '100%', borderCollapse: 'collapse' }}>
                                <thead>
                                  <tr>
                                    {Object.keys(extractedData.tableData[0]).map((header) => (
                                      <th key={header} style={{ border: '1px solid #ddd', padding: '8px', textAlign: 'left' }}>
                                        {header.replace(/([A-Z])/g, ' $1').replace(/^./, str => str.toUpperCase())}
                                      </th>
                                    ))}
                                  </tr>
                                </thead>
                                <tbody>
                                  {extractedData.tableData.map((row, index) => (
                                    <tr key={index}>
                                      {Object.values(row).map((cell, cellIndex) => (
                                        <td key={cellIndex} style={{ border: '1px solid #ddd', padding: '8px' }}>
                                          {cell}
                                        </td>
                                      ))}
                                    </tr>
                                  ))}
                                </tbody>
                              </table>
                            </Box>
                          </CardContent>
                        </Card>
                      </Grid>
                    )}
                  </Grid>
                </Paper>
                
                <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                  <Button
                    variant="contained"
                    color="primary"
                    startIcon={<SaveIcon />}
                    onClick={handleSaveDocument}
                  >
                    บันทึกข้อมูล
                  </Button>
                </Box>
              </>
            )}
          </Box>
        );
      
      case 3:
        return (
          <Box sx={{ textAlign: 'center', p: 3 }}>
            {saveSuccess ? (
              <>
                <CheckIcon color="success" sx={{ fontSize: 60 }} />
                <Typography variant="h6" color="success.main" sx={{ mt: 2 }}>
                  บันทึกข้อมูลสำเร็จ
                </Typography>
                <Typography variant="body1" sx={{ mt: 1 }}>
                  ข้อมูลได้ถูกบันทึกเข้าสู่ระบบเรียบร้อยแล้ว
                </Typography>
                <Button
                  variant="contained"
                  color="primary"
                  onClick={handleReset}
                  sx={{ mt: 3 }}
                >
                  นำเข้าเอกสารใหม่
                </Button>
              </>
            ) : (
              <>
                <ErrorIcon color="error" sx={{ fontSize: 60 }} />
                <Typography variant="h6" color="error" sx={{ mt: 2 }}>
                  เกิดข้อผิดพลาดในการบันทึกข้อมูล
                </Typography>
                <Typography variant="body1" sx={{ mt: 1 }}>
                  กรุณาลองใหม่อีกครั้ง
                </Typography>
                <Button
                  variant="contained"
                  color="primary"
                  onClick={() => setActiveStep(2)}
                  sx={{ mt: 3 }}
                >
                  กลับไปแก้ไข
                </Button>
              </>
            )}
          </Box>
        );
      
      default:
        return 'ขั้นตอนไม่ถูกต้อง';
    }
  };

  return (
    <Box sx={{ width: '100%' }}>
      <Paper elevation={3} sx={{ p: 3, mb: 3 }}>
        <Typography variant="h5" gutterBottom>
          นำเข้าเอกสารด้วย AI
        </Typography>
        <Typography variant="body2" color="text.secondary" gutterBottom>
          อัพโหลดภาพเอกสาร Excel หรือเอกสารทางการแพทย์เพื่อให้ AI ช่วยแยกข้อมูลและนำเข้าสู่ระบบ
        </Typography>
        
        <Stepper activeStep={activeStep} sx={{ mt: 3, mb: 4 }}>
          {steps.map((label) => (
            <Step key={label}>
              <StepLabel>{label}</StepLabel>
            </Step>
          ))}
        </Stepper>
        
        {error && (
          <Alert severity="error" sx={{ mb: 3 }}>
            <AlertTitle>ข้อผิดพลาด</AlertTitle>
            {error}
          </Alert>
        )}
        
        {getStepContent(activeStep)}
      </Paper>
    </Box>
  );
};

export default DocumentImportAI; 