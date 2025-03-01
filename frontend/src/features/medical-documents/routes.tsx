import React from 'react';
import { RouteObject } from 'react-router-dom';

// Pages
import MedicalDocumentsPage from './pages/MedicalDocumentsPage';
import MedicalDocumentDetailPage from './pages/MedicalDocumentDetailPage';
import CreateMedicalDocumentPage from './pages/CreateMedicalDocumentPage';
import EditMedicalDocumentPage from './pages/EditMedicalDocumentPage';
import MedicalCertificateDetailPage from './pages/MedicalCertificateDetailPage';
import CreateMedicalCertificatePage from './pages/CreateMedicalCertificatePage';
import EditMedicalCertificatePage from './pages/EditMedicalCertificatePage';
import HealthCheckupBookDetailPage from './pages/HealthCheckupBookDetailPage';
import CreateHealthCheckupBookPage from './pages/CreateHealthCheckupBookPage';
import EditHealthCheckupBookPage from './pages/EditHealthCheckupBookPage';

// AI Pages
import { MedicalDocumentsAIPage, DocumentImportPage } from '../ai';

const medicalDocumentsRoutes: RouteObject[] = [
  {
    path: '/medical-documents',
    element: <MedicalDocumentsPage />
  },
  
  // Medical Document routes
  {
    path: '/medical-documents/documents/:id',
    element: <MedicalDocumentDetailPage />
  },
  {
    path: '/medical-documents/documents/create',
    element: <CreateMedicalDocumentPage />
  },
  {
    path: '/medical-documents/documents/:id/edit',
    element: <EditMedicalDocumentPage />
  },
  
  // Medical Certificate routes
  {
    path: '/medical-documents/certificates/:id',
    element: <MedicalCertificateDetailPage />
  },
  {
    path: '/medical-documents/certificates/create',
    element: <CreateMedicalCertificatePage />
  },
  {
    path: '/medical-documents/certificates/:id/edit',
    element: <EditMedicalCertificatePage />
  },
  
  // Health Checkup Book routes
  {
    path: '/medical-documents/checkup-books/:id',
    element: <HealthCheckupBookDetailPage />
  },
  {
    path: '/medical-documents/checkup-books/create',
    element: <CreateHealthCheckupBookPage />
  },
  {
    path: '/medical-documents/checkup-books/:id/edit',
    element: <EditHealthCheckupBookPage />
  },
  
  // AI Assistant routes
  {
    path: '/medical-documents/ai-assistant',
    element: <MedicalDocumentsAIPage />
  },
  
  // Document Import routes
  {
    path: '/medical-documents/import',
    element: <DocumentImportPage />
  }
];

export default medicalDocumentsRoutes; 