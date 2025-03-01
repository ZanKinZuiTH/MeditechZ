import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import useMedicalDocumentsApi, { MedicalCertificateCreate, MedicalCertificate } from '../hooks/useMedicalDocumentsApi';
import { 
  Box, 
  Button, 
  Card, 
  CardContent, 
  Chip, 
  Divider, 
  FormControl, 
  FormHelperText, 
  Grid, 
  InputLabel, 
  MenuItem, 
  Select, 
  Stack, 
  TextField, 
  Typography,
  Alert,
  CircularProgress,
  Autocomplete,
  Paper
} from '@mui/material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { th } from 'date-fns/locale';
import { usePatients } from '../../patients/hooks/usePatientsApi';
import { useUsers } from '../../users/hooks/useUsersApi';
import { useAiDiagnosis } from '../../ai/hooks/useAiDiagnosis';
import { SymptomSelector } from '../../ai/components/SymptomSelector';
import { DiagnosisResult } from '../../ai/components/DiagnosisResult';
import MedicalCertificateAIAssistant from '../../ai/components/MedicalCertificateAIAssistant';

interface MedicalCertificateFormProps {
  isEdit: boolean;
}

const MedicalCertificateForm: React.FC<MedicalCertificateFormProps> = ({ isEdit }) => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const { 
    getMedicalCertificateById, 
    createMedicalCertificate, 
    updateMedicalCertificate,
    loading,
    error: apiError
  } = useMedicalDocumentsApi();

  const [formData, setFormData] = useState<MedicalCertificateCreate>({
    patient_id: 0,
    doctor_id: 0,
    certificate_date: new Date().toISOString().split('T')[0],
    certificate_type: 'sick_leave',
    diagnosis: '',
    treatment: '',
    rest_period_days: 0,
    rest_period_start: '',
    rest_period_end: '',
    doctor_license_no: '',
    doctor_license_issue_date: '',
    vital_signs: {
      temperature: '',
      pulse: '',
      respiratory_rate: '',
      blood_pressure: ''
    },
    comments: ''
  });

  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState<boolean>(false);

  const [selectedSymptoms, setSelectedSymptoms] = useState<string[]>([]);
  const [showDiagnosisResult, setShowDiagnosisResult] = useState(false);
  const [aiDiagnosisApplied, setAiDiagnosisApplied] = useState(false);

  const { patients, isLoading: isLoadingPatients } = usePatients();
  const { doctors, isLoading: isLoadingDoctors } = useUsers({ role: 'doctor' });
  const { 
    diagnose, 
    diagnosisResult, 
    isLoading: isDiagnosing, 
    error: diagnosisError 
  } = useAiDiagnosis();

  const [showAIAssistant, setShowAIAssistant] = useState<boolean>(false);

  useEffect(() => {
    if (isEdit && id) {
      const fetchMedicalCertificate = async () => {
        try {
          const data = await getMedicalCertificateById(parseInt(id));
          setFormData({
            patient_id: data.patient_id,
            doctor_id: data.doctor_id,
            visit_id: data.visit_id,
            certificate_date: data.certificate_date,
            certificate_type: data.certificate_type,
            diagnosis: data.diagnosis || '',
            treatment: data.treatment || '',
            rest_period_days: data.rest_period_days || 0,
            rest_period_start: data.rest_period_start || '',
            rest_period_end: data.rest_period_end || '',
            doctor_license_no: data.doctor_license_no || '',
            doctor_license_issue_date: data.doctor_license_issue_date || '',
            doctor2_id: data.doctor2_id,
            doctor2_license_no: data.doctor2_license_no || '',
            doctor2_license_issue_date: data.doctor2_license_issue_date || '',
            vital_signs: data.vital_signs || {
              temperature: '',
              pulse: '',
              respiratory_rate: '',
              blood_pressure: ''
            },
            comments: data.comments || ''
          });
        } catch (err) {
          console.error('เกิดข้อผิดพลาดในการดึงข้อมูลใบรับรองแพทย์:', err);
          setError('ไม่สามารถโหลดข้อมูลใบรับรองแพทย์ได้ โปรดลองอีกครั้งในภายหลัง');
        }
      };

      fetchMedicalCertificate();
    }
  }, [isEdit, id, getMedicalCertificateById]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleNestedInputChange = (category: string, field: string, value: string) => {
    if (category === 'vital_signs') {
      setFormData({
        ...formData,
        vital_signs: {
          ...formData.vital_signs,
          [field]: value
        }
      });
    }
  };

  const handleSelectChange = (e: React.ChangeEvent<{ name?: string; value: unknown }>) => {
    const name = e.target.name as keyof MedicalCertificateCreate;
    const value = e.target.value;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleDateChange = (name: string, date: Date | null) => {
    if (date) {
      setFormData(prev => ({ ...prev, [name]: date }));
    }
  };

  const handleSymptomChange = (symptoms: string[]) => {
    setSelectedSymptoms(symptoms);
  };

  const handleDiagnose = async () => {
    if (selectedSymptoms.length > 0) {
      await diagnose(selectedSymptoms, formData.patient_id ? { patient_id: formData.patient_id } : undefined);
      setShowDiagnosisResult(true);
    }
  };

  const handleApplyDiagnosis = () => {
    if (diagnosisResult) {
      setFormData(prev => ({
        ...prev,
        diagnosis: diagnosisResult.disease + ': ' + diagnosisResult.description,
        comments: [
          prev.comments || '',
          'การวินิจฉัยด้วย AI:',
          `- โรค: ${diagnosisResult.disease} (ความมั่นใจ: ${diagnosisResult.confidence * 100}%)`,
          `- คำอธิบาย: ${diagnosisResult.description}`,
          '- คำแนะนำ:',
          ...diagnosisResult.recommendations.map(rec => `  * ${rec}`)
        ].join('\n')
      }));
      setAiDiagnosisApplied(true);
    }
  };

  const handleDiagnosisComplete = (diagnosisData: any) => {
    const diagnosis = `${diagnosisData.disease} (ความมั่นใจ: ${Math.round(diagnosisData.confidence * 100)}%)\n${diagnosisData.description}`;
    const treatment = diagnosisData.recommendations.join('\n');
    const rest_period_days = diagnosisData.rest_period_days || 0;
    
    const today = new Date();
    const rest_period_start = today.toISOString().split('T')[0];
    
    const endDate = new Date(today);
    endDate.setDate(today.getDate() + rest_period_days);
    const rest_period_end = endDate.toISOString().split('T')[0];
    
    setFormData({
      ...formData,
      diagnosis,
      treatment,
      rest_period_days,
      rest_period_start,
      rest_period_end
    });
    
    setShowAIAssistant(false);
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setSuccess(false);

    try {
      if (isEdit && id) {
        await updateMedicalCertificate(parseInt(id), formData);
        setSuccess(true);
        setTimeout(() => {
          navigate(`/medical-documents/certificates/${id}`);
        }, 1500);
      } else {
        const newCertificate = await createMedicalCertificate(formData);
        setSuccess(true);
        setTimeout(() => {
          navigate(`/medical-documents/certificates/${newCertificate.id}`);
        }, 1500);
      }
    } catch (err) {
      console.error('เกิดข้อผิดพลาดในการบันทึกข้อมูล:', err);
      setError('ไม่สามารถบันทึกข้อมูลได้ โปรดตรวจสอบข้อมูลและลองอีกครั้ง');
    }
  };

  if (loading && isEdit) {
    return (
      <div className="flex justify-center p-8">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
      </div>
    );
  }

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={th}>
      <form onSubmit={handleSubmit}>
        <Card>
          <CardContent>
            <Typography variant="h6" gutterBottom>
              ข้อมูลทั่วไป
            </Typography>
            
            <Grid container spacing={3}>
              <Grid item xs={12} md={6}>
                <FormControl fullWidth required>
                  <InputLabel id="patient-label">ผู้ป่วย</InputLabel>
                  <Select
                    labelId="patient-label"
                    name="patient_id"
                    value={formData.patient_id || ''}
                    onChange={handleSelectChange}
                    label="ผู้ป่วย"
                    disabled={loading}
                  >
                    {isLoadingPatients ? (
                      <MenuItem value="">
                        <CircularProgress size={20} /> กำลังโหลด...
                      </MenuItem>
                    ) : (
                      patients?.map(patient => (
                        <MenuItem key={patient.id} value={patient.id}>
                          {patient.first_name} {patient.last_name} (HN: {patient.hn})
                        </MenuItem>
                      ))
                    )}
                  </Select>
                </FormControl>
              </Grid>
              
              <Grid item xs={12} md={6}>
                <FormControl fullWidth required>
                  <InputLabel id="doctor-label">แพทย์</InputLabel>
                  <Select
                    labelId="doctor-label"
                    name="doctor_id"
                    value={formData.doctor_id || ''}
                    onChange={handleSelectChange}
                    label="แพทย์"
                    disabled={loading}
                  >
                    {isLoadingDoctors ? (
                      <MenuItem value="">
                        <CircularProgress size={20} /> กำลังโหลด...
                      </MenuItem>
                    ) : (
                      doctors?.map(doctor => (
                        <MenuItem key={doctor.id} value={doctor.id}>
                          {doctor.first_name} {doctor.last_name}
                        </MenuItem>
                      ))
                    )}
                  </Select>
                </FormControl>
              </Grid>
              
              <Grid item xs={12} md={6}>
                <DatePicker
                  label="วันที่ออกใบรับรอง"
                  value={formData.certificate_date}
                  onChange={(date) => handleDateChange('certificate_date', date)}
                  disabled={loading}
                  slotProps={{ textField: { fullWidth: true, required: true } }}
                />
              </Grid>
              
              <Grid item xs={12} md={6}>
                <FormControl fullWidth required>
                  <InputLabel id="certificate-type-label">ประเภทใบรับรอง</InputLabel>
                  <Select
                    labelId="certificate-type-label"
                    name="certificate_type"
                    value={formData.certificate_type}
                    onChange={handleSelectChange}
                    label="ประเภทใบรับรอง"
                    disabled={loading}
                  >
                    <MenuItem value="sick_leave">ใบรับรองแพทย์ลาป่วย</MenuItem>
                    <MenuItem value="health_certificate">ใบรับรองแพทย์ตรวจสุขภาพ</MenuItem>
                    <MenuItem value="disability_certificate">ใบรับรองความพิการ</MenuItem>
                    <MenuItem value="fitness_certificate">ใบรับรองความสมบูรณ์ทางร่างกาย</MenuItem>
                    <MenuItem value="other">อื่นๆ</MenuItem>
                  </Select>
                </FormControl>
              </Grid>
            </Grid>
            
            <Box mt={4}>
              <Typography variant="h6" gutterBottom>
                การวินิจฉัยด้วย AI
              </Typography>
              
              <Paper variant="outlined" sx={{ p: 2, mb: 3 }}>
                <Typography variant="subtitle1" gutterBottom>
                  เลือกอาการของผู้ป่วย
                </Typography>
                
                <SymptomSelector 
                  selectedSymptoms={selectedSymptoms}
                  onChange={handleSymptomChange}
                  disabled={loading || isDiagnosing}
                />
                
                <Box mt={2} display="flex" justifyContent="flex-end">
                  <Button
                    variant="contained"
                    color="primary"
                    onClick={handleDiagnose}
                    disabled={selectedSymptoms.length === 0 || loading || isDiagnosing}
                    startIcon={isDiagnosing ? <CircularProgress size={20} color="inherit" /> : null}
                  >
                    {isDiagnosing ? 'กำลังวินิจฉัย...' : 'วินิจฉัยด้วย AI'}
                  </Button>
                </Box>
                
                {diagnosisError && (
                  <Alert severity="error" sx={{ mt: 2 }}>
                    เกิดข้อผิดพลาดในการวินิจฉัย: {diagnosisError}
                  </Alert>
                )}
                
                {showDiagnosisResult && diagnosisResult && (
                  <Box mt={2}>
                    <DiagnosisResult 
                      result={diagnosisResult} 
                      onApply={handleApplyDiagnosis}
                      applied={aiDiagnosisApplied}
                    />
                  </Box>
                )}
              </Paper>
            </Box>
            
            <Box mt={4}>
              <Typography variant="h6" gutterBottom>
                ข้อมูลการวินิจฉัยและการรักษา
              </Typography>
              
              <Grid container spacing={3}>
                <Grid item xs={12}>
                  <TextField
                    name="diagnosis"
                    label="การวินิจฉัย"
                    value={formData.diagnosis}
                    onChange={handleInputChange}
                    multiline
                    rows={4}
                    fullWidth
                    required
                    disabled={loading}
                  />
                </Grid>
                
                <Grid item xs={12}>
                  <TextField
                    name="treatment"
                    label="การรักษา"
                    value={formData.treatment}
                    onChange={handleInputChange}
                    multiline
                    rows={4}
                    fullWidth
                    disabled={loading}
                  />
                </Grid>
              </Grid>
            </Box>
            
            <Box mt={4}>
              <Typography variant="h6" gutterBottom>
                ระยะเวลาพักฟื้น
              </Typography>
              
              <Grid container spacing={3}>
                <Grid item xs={12} md={4}>
                  <TextField
                    name="rest_period_days"
                    label="จำนวนวันพักฟื้น"
                    type="number"
                    value={formData.rest_period_days}
                    onChange={handleInputChange}
                    fullWidth
                    disabled={loading}
                    InputProps={{ inputProps: { min: 0 } }}
                  />
                </Grid>
                
                <Grid item xs={12} md={4}>
                  <DatePicker
                    label="วันที่เริ่มพักฟื้น"
                    value={formData.rest_period_start}
                    onChange={(date) => handleDateChange('rest_period_start', date)}
                    disabled={loading}
                    slotProps={{ textField: { fullWidth: true } }}
                  />
                </Grid>
                
                <Grid item xs={12} md={4}>
                  <DatePicker
                    label="วันที่สิ้นสุดการพักฟื้น"
                    value={formData.rest_period_end || null}
                    onChange={(date) => handleDateChange('rest_period_end', date)}
                    disabled={loading}
                    slotProps={{ textField: { fullWidth: true } }}
                  />
                </Grid>
              </Grid>
            </Box>
            
            <Box mt={4}>
              <Typography variant="h6" gutterBottom>
                สัญญาณชีพ
              </Typography>
              
              <Grid container spacing={3}>
                <Grid item xs={12} md={3}>
                  <TextField
                    name="temperature"
                    label="อุณหภูมิ (°C)"
                    type="number"
                    value={formData.vital_signs?.temperature || ''}
                    onChange={(e) => handleNestedInputChange('vital_signs', 'temperature', e.target.value)}
                    fullWidth
                    disabled={loading}
                    InputProps={{ inputProps: { step: 0.1 } }}
                  />
                </Grid>
                
                <Grid item xs={12} md={3}>
                  <TextField
                    name="pulse"
                    label="ชีพจร (ครั้ง/นาที)"
                    type="number"
                    value={formData.vital_signs?.pulse || ''}
                    onChange={(e) => handleNestedInputChange('vital_signs', 'pulse', e.target.value)}
                    fullWidth
                    disabled={loading}
                  />
                </Grid>
                
                <Grid item xs={12} md={3}>
                  <TextField
                    name="respiratory_rate"
                    label="อัตราการหายใจ (ครั้ง/นาที)"
                    type="number"
                    value={formData.vital_signs?.respiratory_rate || ''}
                    onChange={(e) => handleNestedInputChange('vital_signs', 'respiratory_rate', e.target.value)}
                    fullWidth
                    disabled={loading}
                  />
                </Grid>
                
                <Grid item xs={12} md={3}>
                  <TextField
                    name="blood_pressure"
                    label="ความดันโลหิต (mmHg)"
                    value={formData.vital_signs?.blood_pressure || ''}
                    onChange={(e) => handleNestedInputChange('vital_signs', 'blood_pressure', e.target.value)}
                    fullWidth
                    disabled={loading}
                    placeholder="120/80"
                  />
                </Grid>
              </Grid>
            </Box>
            
            <Box mt={4}>
              <Typography variant="h6" gutterBottom>
                ข้อมูลเพิ่มเติม
              </Typography>
              
              <Grid container spacing={3}>
                <Grid item xs={12}>
                  <TextField
                    name="doctor_license_no"
                    label="เลขที่ใบอนุญาตประกอบวิชาชีพเวชกรรม"
                    value={formData.doctor_license_no}
                    onChange={handleInputChange}
                    fullWidth
                    disabled={loading}
                  />
                </Grid>
                
                <Grid item xs={12}>
                  <TextField
                    name="comments"
                    label="หมายเหตุ"
                    value={formData.comments}
                    onChange={handleInputChange}
                    multiline
                    rows={4}
                    fullWidth
                    disabled={loading}
                  />
                </Grid>
              </Grid>
            </Box>
            
            <div className="mb-6">
              <button
                type="button"
                className="btn btn-secondary w-full"
                onClick={() => setShowAIAssistant(!showAIAssistant)}
              >
                {showAIAssistant ? 'ซ่อน' : 'แสดง'} ผู้ช่วย AI สำหรับการวินิจฉัย
              </button>
              
              {showAIAssistant && (
                <div className="mt-4">
                  <MedicalCertificateAIAssistant 
                    patientData={{
                      patient_id: formData.patient_id,
                    }}
                    onDiagnosisComplete={handleDiagnosisComplete}
                  />
                </div>
              )}
            </div>
            
            {error && (
              <Alert severity="error" sx={{ mt: 3 }}>
                {error}
              </Alert>
            )}
            
            <Box mt={4} display="flex" justifyContent="flex-end">
              <Button
                type="submit"
                variant="contained"
                color="primary"
                disabled={loading}
                startIcon={loading ? <CircularProgress size={20} color="inherit" /> : null}
              >
                {loading ? 'กำลังบันทึก...' : isEdit ? 'อัปเดต' : 'สร้างใบรับรองแพทย์'}
              </Button>
            </Box>
          </CardContent>
        </Card>
      </form>
    </LocalizationProvider>
  );
};

export default MedicalCertificateForm; 