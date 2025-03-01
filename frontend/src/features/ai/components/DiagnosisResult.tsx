import React from 'react';
import { 
  Box, 
  Button, 
  Card, 
  CardContent, 
  Chip, 
  Divider, 
  List, 
  ListItem, 
  ListItemText, 
  Paper, 
  Stack, 
  Typography,
  Alert,
  LinearProgress
} from '@mui/material';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import WarningIcon from '@mui/icons-material/Warning';
import InfoIcon from '@mui/icons-material/Info';
import MedicalServicesIcon from '@mui/icons-material/MedicalServices';

// ประเภทข้อมูลสำหรับผลการวินิจฉัย
export interface DiagnosisResultType {
  disease: string;
  confidence: number;
  description: string;
  recommendations: string[];
  differential_diagnoses: Array<{
    disease: string;
    confidence: number;
    description: string;
  }>;
}

interface DiagnosisResultProps {
  result: DiagnosisResultType;
  onApply: () => void;
  applied: boolean;
}

export const DiagnosisResult: React.FC<DiagnosisResultProps> = ({
  result,
  onApply,
  applied = false
}) => {
  // แปลงค่าความมั่นใจเป็นเปอร์เซ็นต์
  const confidencePercent = Math.round(result.confidence * 100);
  
  // กำหนดสีตามระดับความมั่นใจ
  const getConfidenceColor = (confidence: number) => {
    if (confidence >= 0.8) return 'success.main';
    if (confidence >= 0.6) return 'info.main';
    if (confidence >= 0.4) return 'warning.main';
    return 'error.main';
  };
  
  // กำหนดไอคอนตามระดับความมั่นใจ
  const getConfidenceIcon = (confidence: number) => {
    if (confidence >= 0.8) return <CheckCircleIcon color="success" />;
    if (confidence >= 0.6) return <InfoIcon color="info" />;
    if (confidence >= 0.4) return <WarningIcon color="warning" />;
    return <WarningIcon color="error" />;
  };

  return (
    <Paper variant="outlined" sx={{ p: 2 }}>
      <Box mb={2}>
        <Typography variant="h6" gutterBottom display="flex" alignItems="center" gap={1}>
          <MedicalServicesIcon color="primary" />
          ผลการวินิจฉัยด้วย AI
        </Typography>
        
        <Alert 
          severity={confidencePercent >= 70 ? "success" : "info"} 
          sx={{ mb: 2 }}
        >
          ผลการวินิจฉัยนี้เป็นเพียงการประมวลผลเบื้องต้นจาก AI เท่านั้น ควรได้รับการยืนยันจากแพทย์ผู้เชี่ยวชาญ
        </Alert>
      </Box>
      
      <Card variant="outlined" sx={{ mb: 3 }}>
        <CardContent>
          <Box display="flex" justifyContent="space-between" alignItems="center" mb={1}>
            <Typography variant="h6" color="primary">
              {result.disease}
            </Typography>
            <Chip 
              label={`ความมั่นใจ ${confidencePercent}%`}
              color={confidencePercent >= 80 ? "success" : confidencePercent >= 60 ? "info" : "warning"}
              variant="outlined"
              icon={getConfidenceIcon(result.confidence)}
            />
          </Box>
          
          <LinearProgress 
            variant="determinate" 
            value={confidencePercent} 
            color={confidencePercent >= 80 ? "success" : confidencePercent >= 60 ? "info" : "warning"}
            sx={{ height: 8, borderRadius: 4, mb: 2 }}
          />
          
          <Typography variant="body1" paragraph>
            {result.description}
          </Typography>
          
          <Divider sx={{ my: 2 }} />
          
          <Typography variant="subtitle1" gutterBottom>
            คำแนะนำเบื้องต้น
          </Typography>
          
          <List dense disablePadding>
            {result.recommendations.map((recommendation, index) => (
              <ListItem key={index} disableGutters>
                <ListItemText 
                  primary={recommendation}
                  primaryTypographyProps={{ variant: 'body2' }}
                />
              </ListItem>
            ))}
          </List>
        </CardContent>
      </Card>
      
      {result.differential_diagnoses.length > 0 && (
        <Box mb={3}>
          <Typography variant="subtitle1" gutterBottom>
            การวินิจฉัยแยกโรคอื่นๆ ที่เป็นไปได้
          </Typography>
          
          <Stack spacing={1}>
            {result.differential_diagnoses.map((diagnosis, index) => (
              <Paper key={index} variant="outlined" sx={{ p: 1.5 }}>
                <Box display="flex" justifyContent="space-between" alignItems="center" mb={0.5}>
                  <Typography variant="subtitle2">
                    {diagnosis.disease}
                  </Typography>
                  <Chip 
                    label={`${Math.round(diagnosis.confidence * 100)}%`}
                    size="small"
                    variant="outlined"
                    sx={{ color: getConfidenceColor(diagnosis.confidence) }}
                  />
                </Box>
                <Typography variant="body2" color="text.secondary">
                  {diagnosis.description}
                </Typography>
              </Paper>
            ))}
          </Stack>
        </Box>
      )}
      
      <Box display="flex" justifyContent="flex-end">
        <Button
          variant="contained"
          color="primary"
          onClick={onApply}
          disabled={applied}
          startIcon={applied ? <CheckCircleIcon /> : null}
        >
          {applied ? 'นำไปใช้แล้ว' : 'นำผลการวินิจฉัยไปใช้'}
        </Button>
      </Box>
    </Paper>
  );
};

export default DiagnosisResult; 