import React from 'react';
import { Box, Typography, Container, Breadcrumbs, Link, Paper } from '@mui/material';
import { Home as HomeIcon, Description as DescriptionIcon } from '@mui/icons-material';
import DocumentImportAI from '../components/DocumentImportAI';
import useDocumentImportAI from '../hooks/useDocumentImportAI';

/**
 * หน้าสำหรับนำเข้าเอกสารด้วย AI
 */
const DocumentImportPage: React.FC = () => {
  // ใช้ฮุค useDocumentImportAI
  const {
    processDocument,
    saveExtractedData,
    isProcessing,
    isSaving,
    error,
    availableTemplates,
    resetState
  } = useDocumentImportAI({
    onSuccess: (data) => {
      console.log('ประมวลผลเอกสารสำเร็จ:', data);
    },
    onError: (error) => {
      console.error('เกิดข้อผิดพลาด:', error);
    }
  });

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      {/* Breadcrumbs */}
      <Breadcrumbs aria-label="breadcrumb" sx={{ mb: 3 }}>
        <Link
          underline="hover"
          color="inherit"
          href="/dashboard"
          sx={{ display: 'flex', alignItems: 'center' }}
        >
          <HomeIcon sx={{ mr: 0.5 }} fontSize="inherit" />
          หน้าหลัก
        </Link>
        <Link
          underline="hover"
          color="inherit"
          href="/medical-documents"
          sx={{ display: 'flex', alignItems: 'center' }}
        >
          <DescriptionIcon sx={{ mr: 0.5 }} fontSize="inherit" />
          เอกสารทางการแพทย์
        </Link>
        <Typography color="text.primary" sx={{ display: 'flex', alignItems: 'center' }}>
          นำเข้าเอกสารด้วย AI
        </Typography>
      </Breadcrumbs>

      {/* หัวข้อหน้า */}
      <Box sx={{ mb: 4 }}>
        <Typography variant="h4" component="h1" gutterBottom>
          นำเข้าเอกสารด้วย AI
        </Typography>
        <Typography variant="body1" color="text.secondary">
          อัพโหลดภาพเอกสาร Excel หรือเอกสารทางการแพทย์เพื่อให้ AI ช่วยแยกข้อมูลและนำเข้าสู่ระบบ
        </Typography>
      </Box>

      {/* คำอธิบายการใช้งาน */}
      <Paper elevation={1} sx={{ p: 3, mb: 4 }}>
        <Typography variant="h6" gutterBottom>
          คำแนะนำการใช้งาน
        </Typography>
        <Typography variant="body2" paragraph>
          ระบบนี้ใช้ AI ในการวิเคราะห์และสกัดข้อมูลจากภาพเอกสาร เช่น ตารางข้อมูล Excel, ผลตรวจทางห้องปฏิบัติการ, ใบรับรองแพทย์ และเอกสารทางการแพทย์อื่นๆ
        </Typography>
        <Typography variant="body2" component="div">
          <ul>
            <li>อัพโหลดภาพเอกสารที่ต้องการนำเข้า (รองรับไฟล์ JPEG, PNG)</li>
            <li>เลือกเทมเพลตเอกสารหากทราบประเภท หรือปล่อยให้ระบบตรวจจับอัตโนมัติ</li>
            <li>ระบบจะประมวลผลและสกัดข้อมูลจากเอกสาร</li>
            <li>ตรวจสอบและแก้ไขข้อมูลที่สกัดได้ก่อนบันทึกเข้าสู่ระบบ</li>
          </ul>
        </Typography>
      </Paper>

      {/* คอมโพเนนต์ DocumentImportAI */}
      <DocumentImportAI
        onDataExtracted={(data) => console.log('ข้อมูลที่สกัดได้:', data)}
        onSaveDocument={saveExtractedData}
        availableTemplates={availableTemplates}
      />
    </Container>
  );
};

export default DocumentImportPage; 