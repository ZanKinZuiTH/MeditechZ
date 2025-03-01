import React, { useState, useEffect } from 'react';
import { 
  Autocomplete, 
  Chip, 
  TextField, 
  Box, 
  Typography, 
  Paper,
  Grid,
  Checkbox,
  FormControlLabel,
  FormGroup
} from '@mui/material';

// รายการอาการที่ระบบรองรับ
const AVAILABLE_SYMPTOMS = [
  'ไข้',
  'ไอ',
  'เจ็บคอ',
  'ปวดหัว',
  'อ่อนเพลีย',
  'หายใจลำบาก',
  'คลื่นไส้',
  'อาเจียน',
  'ท้องเสีย',
  'ปวดท้อง',
  'ผื่น',
  'ปวดกล้ามเนื้อ',
  'ปวดข้อ',
  'วิงเวียน',
  'หนาวสั่น',
  'เหงื่อออกตอนกลางคืน',
  'น้ำหนักลด',
  'เบื่ออาหาร',
  'ปวดหลัง',
  'ปวดเมื่อยตามตัว',
  'ชา',
  'อาการบวม',
  'ตาแดง',
  'น้ำมูกไหล',
  'จมูกไม่ได้กลิ่น',
  'ลิ้นไม่รับรส',
  'ปัสสาวะบ่อย',
  'ปัสสาวะแสบขัด',
  'ปัสสาวะเป็นเลือด',
  'ท้องผูก'
];

// จัดกลุ่มอาการตามระบบร่างกาย
const SYMPTOM_CATEGORIES = {
  'ระบบทางเดินหายใจ': ['ไอ', 'เจ็บคอ', 'หายใจลำบาก', 'น้ำมูกไหล', 'จมูกไม่ได้กลิ่น'],
  'ระบบทางเดินอาหาร': ['คลื่นไส้', 'อาเจียน', 'ท้องเสีย', 'ปวดท้อง', 'เบื่ออาหาร', 'ท้องผูก', 'ลิ้นไม่รับรส'],
  'ระบบประสาท': ['ปวดหัว', 'วิงเวียน', 'ชา'],
  'ระบบกล้ามเนื้อและกระดูก': ['ปวดกล้ามเนื้อ', 'ปวดข้อ', 'ปวดหลัง', 'ปวดเมื่อยตามตัว'],
  'ระบบทางเดินปัสสาวะ': ['ปัสสาวะบ่อย', 'ปัสสาวะแสบขัด', 'ปัสสาวะเป็นเลือด'],
  'อาการทั่วไป': ['ไข้', 'อ่อนเพลีย', 'หนาวสั่น', 'เหงื่อออกตอนกลางคืน', 'น้ำหนักลด', 'อาการบวม'],
  'ผิวหนังและตา': ['ผื่น', 'ตาแดง']
};

interface SymptomSelectorProps {
  selectedSymptoms: string[];
  onChange: (symptoms: string[]) => void;
  disabled?: boolean;
}

export const SymptomSelector: React.FC<SymptomSelectorProps> = ({
  selectedSymptoms,
  onChange,
  disabled = false
}) => {
  const [viewMode, setViewMode] = useState<'list' | 'category'>('category');
  const [searchTerm, setSearchTerm] = useState('');
  const [filteredSymptoms, setFilteredSymptoms] = useState<string[]>(AVAILABLE_SYMPTOMS);

  useEffect(() => {
    if (searchTerm) {
      const filtered = AVAILABLE_SYMPTOMS.filter(
        symptom => symptom.toLowerCase().includes(searchTerm.toLowerCase())
      );
      setFilteredSymptoms(filtered);
    } else {
      setFilteredSymptoms(AVAILABLE_SYMPTOMS);
    }
  }, [searchTerm]);

  const handleToggleSymptom = (symptom: string) => {
    if (selectedSymptoms.includes(symptom)) {
      onChange(selectedSymptoms.filter(s => s !== symptom));
    } else {
      onChange([...selectedSymptoms, symptom]);
    }
  };

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value);
  };

  const handleRemoveSymptom = (symptom: string) => {
    onChange(selectedSymptoms.filter(s => s !== symptom));
  };

  return (
    <Box>
      <Box mb={2}>
        <TextField
          fullWidth
          variant="outlined"
          label="ค้นหาอาการ"
          value={searchTerm}
          onChange={handleSearchChange}
          disabled={disabled}
          placeholder="พิมพ์เพื่อค้นหาอาการ..."
          size="small"
        />
      </Box>

      <Box mb={2} display="flex" flexWrap="wrap" gap={1}>
        {selectedSymptoms.length > 0 ? (
          selectedSymptoms.map(symptom => (
            <Chip
              key={symptom}
              label={symptom}
              onDelete={disabled ? undefined : () => handleRemoveSymptom(symptom)}
              color="primary"
              variant="outlined"
              disabled={disabled}
            />
          ))
        ) : (
          <Typography color="text.secondary" variant="body2">
            ยังไม่ได้เลือกอาการใดๆ
          </Typography>
        )}
      </Box>

      {viewMode === 'list' ? (
        <Paper variant="outlined" sx={{ p: 2, maxHeight: 300, overflow: 'auto' }}>
          <FormGroup>
            <Grid container spacing={1}>
              {filteredSymptoms.map(symptom => (
                <Grid item xs={6} sm={4} md={3} key={symptom}>
                  <FormControlLabel
                    control={
                      <Checkbox
                        checked={selectedSymptoms.includes(symptom)}
                        onChange={() => handleToggleSymptom(symptom)}
                        disabled={disabled}
                        size="small"
                      />
                    }
                    label={symptom}
                  />
                </Grid>
              ))}
            </Grid>
          </FormGroup>
        </Paper>
      ) : (
        <Paper variant="outlined" sx={{ p: 2, maxHeight: 300, overflow: 'auto' }}>
          {Object.entries(SYMPTOM_CATEGORIES).map(([category, symptoms]) => {
            const filteredCategorySymptoms = symptoms.filter(symptom => 
              filteredSymptoms.includes(symptom)
            );
            
            if (filteredCategorySymptoms.length === 0) return null;
            
            return (
              <Box key={category} mb={2}>
                <Typography variant="subtitle2" gutterBottom color="primary">
                  {category}
                </Typography>
                <FormGroup>
                  <Grid container spacing={1}>
                    {filteredCategorySymptoms.map(symptom => (
                      <Grid item xs={6} sm={4} md={3} key={symptom}>
                        <FormControlLabel
                          control={
                            <Checkbox
                              checked={selectedSymptoms.includes(symptom)}
                              onChange={() => handleToggleSymptom(symptom)}
                              disabled={disabled}
                              size="small"
                            />
                          }
                          label={symptom}
                        />
                      </Grid>
                    ))}
                  </Grid>
                </FormGroup>
              </Box>
            );
          })}
        </Paper>
      )}
    </Box>
  );
};

export default SymptomSelector; 