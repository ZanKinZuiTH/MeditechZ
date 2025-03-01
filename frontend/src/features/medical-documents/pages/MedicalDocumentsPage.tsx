import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import MedicalDocumentList from '../components/MedicalDocumentList';
import MedicalCertificateList from '../components/MedicalCertificateList';
import HealthCheckupBookList from '../components/HealthCheckupBookList';

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
          {tabValue === 0 && (
            <Link to="/medical-documents/documents/create">
              <button className="btn btn-primary">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 mr-2" viewBox="0 0 20 20" fill="currentColor">
                  <path fillRule="evenodd" d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z" clipRule="evenodd" />
                </svg>
                สร้างเอกสารใหม่
              </button>
            </Link>
          )}
          {tabValue === 1 && (
            <Link to="/medical-documents/certificates/create">
              <button className="btn btn-primary">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 mr-2" viewBox="0 0 20 20" fill="currentColor">
                  <path fillRule="evenodd" d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z" clipRule="evenodd" />
                </svg>
                สร้างใบรับรองแพทย์
              </button>
            </Link>
          )}
          {tabValue === 2 && (
            <Link to="/medical-documents/checkup-books/create">
              <button className="btn btn-primary">
                <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 mr-2" viewBox="0 0 20 20" fill="currentColor">
                  <path fillRule="evenodd" d="M10 5a1 1 0 011 1v3h3a1 1 0 110 2h-3v3a1 1 0 11-2 0v-3H6a1 1 0 110-2h3V6a1 1 0 011-1z" clipRule="evenodd" />
                </svg>
                สร้างสมุดตรวจสุขภาพ
              </button>
            </Link>
          )}
          <Link 
            to="/medical-documents/ai-assistant" 
            className="btn btn-primary"
          >
            <span className="mr-2">🤖</span> ผู้ช่วย AI
          </Link>
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