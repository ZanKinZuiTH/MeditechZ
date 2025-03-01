import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableContainer, 
  TableHead, 
  TableRow, 
  Paper, 
  Button, 
  TextField, 
  Box, 
  Typography, 
  IconButton,
  Chip,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  SelectChangeEvent
} from '@mui/material';
import { 
  Add as AddIcon, 
  Edit as EditIcon, 
  Delete as DeleteIcon,
  Search as SearchIcon,
  FilterList as FilterIcon
} from '@mui/icons-material';
import { format } from 'date-fns';
import { th } from 'date-fns/locale';

import useMedicalDocumentsApi, { MedicalDocument, MedicalDocumentSearchParams } from '../hooks/useMedicalDocumentsApi';

interface MedicalDocumentListProps {
  patientId?: number;
  doctorId?: number;
  visitId?: number;
}

const MedicalDocumentList: React.FC<MedicalDocumentListProps> = ({ patientId, doctorId, visitId }) => {
  const { 
    loading, 
    error, 
    getMedicalDocuments, 
    deleteMedicalDocument 
  } = useMedicalDocumentsApi();
  
  const [documents, setDocuments] = useState<MedicalDocument[]>([]);
  const [searchTerm, setSearchTerm] = useState('');
  const [documentType, setDocumentType] = useState<string>('');
  const [startDate, setStartDate] = useState<string>('');
  const [endDate, setEndDate] = useState<string>('');
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [documentToDelete, setDocumentToDelete] = useState<number | null>(null);
  const [showFilters, setShowFilters] = useState(false);

  // ฟังก์ชันสำหรับโหลดข้อมูลเอกสารทางการแพทย์
  const loadDocuments = async () => {
    try {
      const params: MedicalDocumentSearchParams = {};
      
      if (patientId) params.patient_id = patientId;
      if (doctorId) params.doctor_id = doctorId;
      if (visitId) params.visit_id = visitId;
      if (documentType) params.document_type = documentType;
      if (startDate) params.start_date = startDate;
      if (endDate) params.end_date = endDate;
      
      const data = await getMedicalDocuments(params);
      setDocuments(data);
    } catch (err) {
      console.error('เกิดข้อผิดพลาดในการโหลดข้อมูลเอกสารทางการแพทย์:', err);
    }
  };

  // โหลดข้อมูลเมื่อคอมโพเนนต์ถูกโหลด
  useEffect(() => {
    loadDocuments();
  }, [patientId, doctorId, visitId, documentType, startDate, endDate]);

  // ฟังก์ชันสำหรับเปิดไดอะล็อกยืนยันการลบ
  const handleOpenDeleteDialog = (id: number) => {
    setDocumentToDelete(id);
    setDeleteDialogOpen(true);
  };

  // ฟังก์ชันสำหรับปิดไดอะล็อกยืนยันการลบ
  const handleCloseDeleteDialog = () => {
    setDeleteDialogOpen(false);
    setDocumentToDelete(null);
  };

  // ฟังก์ชันสำหรับลบเอกสารทางการแพทย์
  const handleDeleteDocument = async () => {
    if (documentToDelete) {
      try {
        await deleteMedicalDocument(documentToDelete);
        setDeleteDialogOpen(false);
        setDocumentToDelete(null);
        loadDocuments();
      } catch (err) {
        console.error('เกิดข้อผิดพลาดในการลบเอกสารทางการแพทย์:', err);
      }
    }
  };

  // ฟังก์ชันสำหรับเปลี่ยนประเภทเอกสาร
  const handleDocumentTypeChange = (event: SelectChangeEvent) => {
    setDocumentType(event.target.value);
  };

  // ฟังก์ชันสำหรับรีเซ็ตตัวกรอง
  const handleResetFilters = () => {
    setDocumentType('');
    setStartDate('');
    setEndDate('');
  };

  // กรองเอกสารตามคำค้นหา
  const filteredDocuments = documents.filter(doc => 
    doc.document_type.toLowerCase().includes(searchTerm.toLowerCase()) ||
    doc.notes?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    doc.document_status.toLowerCase().includes(searchTerm.toLowerCase())
  );

  // ฟังก์ชันสำหรับแสดงสถานะเอกสาร
  const renderStatus = (status: string) => {
    let color = 'default';
    
    switch (status) {
      case 'draft':
        color = 'warning';
        break;
      case 'completed':
        color = 'success';
        break;
      case 'signed':
        color = 'primary';
        break;
      case 'cancelled':
        color = 'error';
        break;
      default:
        color = 'default';
    }
    
    const statusText = {
      'draft': 'ฉบับร่าง',
      'completed': 'เสร็จสมบูรณ์',
      'signed': 'ลงนามแล้ว',
      'cancelled': 'ยกเลิก',
    }[status] || status;
    
    return <Chip label={statusText} color={color as any} size="small" />;
  };

  // ฟังก์ชันสำหรับแสดงประเภทเอกสาร
  const renderDocumentType = (type: string) => {
    const typeText = {
      'medical_certificate': 'ใบรับรองแพทย์',
      'health_checkup_book': 'สมุดตรวจสุขภาพ',
      'other': 'อื่นๆ',
    }[type] || type;
    
    return typeText;
  };

  return (
    <Box>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
        <Typography variant="h6" component="h2">
          รายการเอกสารทางการแพทย์
        </Typography>
        <Box>
          <Button 
            variant="outlined" 
            startIcon={<FilterIcon />} 
            onClick={() => setShowFilters(!showFilters)}
            sx={{ mr: 1 }}
          >
            ตัวกรอง
          </Button>
          <Button 
            variant="contained" 
            startIcon={<AddIcon />} 
            component={Link} 
            to="/medical-documents/create"
          >
            สร้างเอกสารใหม่
          </Button>
        </Box>
      </Box>

      {showFilters && (
        <Box sx={{ mb: 2, p: 2, bgcolor: 'background.paper', borderRadius: 1 }}>
          <Typography variant="subtitle1" gutterBottom>
            ตัวกรอง
          </Typography>
          <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
            <FormControl sx={{ minWidth: 200 }}>
              <InputLabel id="document-type-label">ประเภทเอกสาร</InputLabel>
              <Select
                labelId="document-type-label"
                value={documentType}
                label="ประเภทเอกสาร"
                onChange={handleDocumentTypeChange}
              >
                <MenuItem value="">ทั้งหมด</MenuItem>
                <MenuItem value="medical_certificate">ใบรับรองแพทย์</MenuItem>
                <MenuItem value="health_checkup_book">สมุดตรวจสุขภาพ</MenuItem>
                <MenuItem value="other">อื่นๆ</MenuItem>
              </Select>
            </FormControl>
            <TextField
              label="วันที่เริ่มต้น"
              type="date"
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              InputLabelProps={{ shrink: true }}
            />
            <TextField
              label="วันที่สิ้นสุด"
              type="date"
              value={endDate}
              onChange={(e) => setEndDate(e.target.value)}
              InputLabelProps={{ shrink: true }}
            />
            <Button variant="outlined" onClick={handleResetFilters}>
              รีเซ็ต
            </Button>
          </Box>
        </Box>
      )}

      <Box sx={{ mb: 2 }}>
        <TextField
          fullWidth
          variant="outlined"
          placeholder="ค้นหาเอกสาร..."
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
          InputProps={{
            startAdornment: <SearchIcon sx={{ color: 'action.active', mr: 1 }} />,
          }}
        />
      </Box>

      {loading ? (
        <Typography>กำลังโหลดข้อมูล...</Typography>
      ) : error ? (
        <Typography color="error">เกิดข้อผิดพลาด: {error}</Typography>
      ) : filteredDocuments.length === 0 ? (
        <Typography>ไม่พบเอกสารทางการแพทย์</Typography>
      ) : (
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>ประเภทเอกสาร</TableCell>
                <TableCell>วันที่</TableCell>
                <TableCell>สถานะ</TableCell>
                <TableCell>หมายเหตุ</TableCell>
                <TableCell align="right">การดำเนินการ</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {filteredDocuments.map((document) => (
                <TableRow key={document.id}>
                  <TableCell>{renderDocumentType(document.document_type)}</TableCell>
                  <TableCell>
                    {format(new Date(document.document_date), 'dd MMMM yyyy', { locale: th })}
                  </TableCell>
                  <TableCell>{renderStatus(document.document_status)}</TableCell>
                  <TableCell>{document.notes || '-'}</TableCell>
                  <TableCell align="right">
                    <IconButton 
                      component={Link} 
                      to={`/medical-documents/${document.id}`}
                      color="primary"
                      size="small"
                    >
                      <EditIcon fontSize="small" />
                    </IconButton>
                    <IconButton 
                      color="error" 
                      size="small"
                      onClick={() => handleOpenDeleteDialog(document.id)}
                    >
                      <DeleteIcon fontSize="small" />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      {/* ไดอะล็อกยืนยันการลบ */}
      <Dialog
        open={deleteDialogOpen}
        onClose={handleCloseDeleteDialog}
      >
        <DialogTitle>ยืนยันการลบเอกสาร</DialogTitle>
        <DialogContent>
          <DialogContentText>
            คุณต้องการลบเอกสารนี้ใช่หรือไม่? การดำเนินการนี้ไม่สามารถเรียกคืนได้
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDeleteDialog}>ยกเลิก</Button>
          <Button onClick={handleDeleteDocument} color="error" autoFocus>
            ลบ
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default MedicalDocumentList; 