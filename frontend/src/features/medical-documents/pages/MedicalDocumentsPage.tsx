import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import MedicalDocumentList from '../components/MedicalDocumentList';
import MedicalCertificateList from '../components/MedicalCertificateList';
import HealthCheckupBookList from '../components/HealthCheckupBookList';
import { Box, Button } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import MedicalInformationIcon from '@mui/icons-material/MedicalInformation';
import HealthAndSafetyIcon from '@mui/icons-material/HealthAndSafety';
import SmartToyIcon from '@mui/icons-material/SmartToy';
import UploadFileIcon from '@mui/icons-material/UploadFile';

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`medical-documents-tabpanel-${index}`}
      aria-labelledby={`medical-documents-tab-${index}`}
      {...other}
    >
      {value === index && (
        <div className="p-4">
          {children}
        </div>
      )}
    </div>
  );
}

const MedicalDocumentsPage: React.FC = () => {
  const [tabValue, setTabValue] = useState(0);

  const handleTabChange = (newValue: number) => {
    setTabValue(newValue);
  };

  return (
    <div className="container mx-auto px-4 py-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-3xl font-bold">เอกสารทางการแพทย์</h1>
          <p className="text-gray-600 mt-2">จัดการเอกสารทางการแพทย์ทั้งหมดในระบบ</p>
        </div>
        <div className="flex space-x-2">
          <Box sx={{ display: 'flex', gap: 2, mb: 3 }}>
            <Button
              component={Link}
              to="/medical-documents/documents/create"
              variant="contained"
              startIcon={<AddIcon />}
            >
              สร้างเอกสารใหม่
            </Button>
            
            <Button
              component={Link}
              to="/medical-documents/certificates/create"
              variant="contained"
              startIcon={<MedicalInformationIcon />}
            >
              ออกใบรับรองแพทย์
            </Button>
            
            <Button
              component={Link}
              to="/medical-documents/checkup-books/create"
              variant="contained"
              startIcon={<HealthAndSafetyIcon />}
            >
              สร้างสมุดตรวจสุขภาพ
            </Button>
            
            <Button
              component={Link}
              to="/medical-documents/ai-assistant"
              variant="contained"
              color="secondary"
              startIcon={<SmartToyIcon />}
            >
              ผู้ช่วย AI
            </Button>
            
            <Button
              component={Link}
              to="/medical-documents/import"
              variant="contained"
              color="info"
              startIcon={<UploadFileIcon />}
            >
              นำเข้าเอกสารด้วย AI
            </Button>
          </Box>
        </div>
      </div>

      <div className="w-full">
        <div className="tabs tabs-boxed mb-4">
          <a 
            className={`tab ${tabValue === 0 ? 'tab-active' : ''}`}
            onClick={() => handleTabChange(0)}
          >
            เอกสารทั้งหมด
          </a>
          <a 
            className={`tab ${tabValue === 1 ? 'tab-active' : ''}`}
            onClick={() => handleTabChange(1)}
          >
            ใบรับรองแพทย์
          </a>
          <a 
            className={`tab ${tabValue === 2 ? 'tab-active' : ''}`}
            onClick={() => handleTabChange(2)}
          >
            สมุดตรวจสุขภาพ
          </a>
        </div>
        
        <div className="bg-white rounded-lg shadow-sm">
          <TabPanel value={tabValue} index={0}>
            <MedicalDocumentList />
          </TabPanel>
          <TabPanel value={tabValue} index={1}>
            <MedicalCertificateList />
          </TabPanel>
          <TabPanel value={tabValue} index={2}>
            <HealthCheckupBookList />
          </TabPanel>
        </div>
      </div>
    </div>
  );
};

export default MedicalDocumentsPage; 