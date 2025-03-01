/**
 * Patient Search Component
 * 
 * คอมโพเนนต์สำหรับค้นหาผู้ป่วยในระบบ MeditechZ
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React, { useState } from 'react';
import { 
  Input 
} from '@/components/ui/input';
import { 
  Button 
} from '@/components/ui/button';
import { 
  Table, 
  TableBody, 
  TableCell, 
  TableHead, 
  TableHeader, 
  TableRow 
} from '@/components/ui/table';
import { 
  Card, 
  CardContent 
} from '@/components/ui/card';
import { 
  SearchIcon, 
  UserIcon 
} from 'lucide-react';
import { formatThaiDate } from '@/lib/utils';

// กำหนด interface สำหรับข้อมูลผู้ป่วย
interface PatientData {
  id?: number;
  first_name: string;
  last_name: string;
  id_card_number: string;
  date_of_birth: string;
  gender: string;
  blood_type?: string;
  address?: string;
  phone_number?: string;
  email?: string;
  emergency_contact_name?: string;
  emergency_contact_phone?: string;
  allergies?: string;
  chronic_diseases?: string;
}

interface PatientSearchProps {
  onPatientSelect: (patient: PatientData) => void;
}

const PatientSearch: React.FC<PatientSearchProps> = ({ onPatientSelect }) => {
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [searchBy, setSearchBy] = useState<string>('name'); // 'name', 'id_card', 'hn'
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [searchResults, setSearchResults] = useState<PatientData[]>([]);
  const [hasSearched, setHasSearched] = useState<boolean>(false);
  
  // ฟังก์ชันสำหรับการค้นหาผู้ป่วย
  const handleSearch = async () => {
    if (!searchTerm.trim()) {
      return;
    }
    
    setIsLoading(true);
    setHasSearched(true);
    
    try {
      // สร้าง query parameters
      const params = new URLSearchParams();
      params.append('search_term', searchTerm);
      params.append('search_by', searchBy);
      
      // ส่งคำขอไปยัง API
      const response = await fetch(`/api/v1/patients/search?${params.toString()}`);
      
      if (!response.ok) {
        throw new Error('เกิดข้อผิดพลาดในการค้นหา');
      }
      
      const data = await response.json();
      setSearchResults(data);
    } catch (error) {
      console.error('Error searching patients:', error);
      setSearchResults([]);
    } finally {
      setIsLoading(false);
    }
    
    // จำลองข้อมูลสำหรับการพัฒนา
    setTimeout(() => {
      const mockResults: PatientData[] = [
        {
          id: 1,
          first_name: 'สมชาย',
          last_name: 'ใจดี',
          id_card_number: '1234567890123',
          date_of_birth: '1990-01-01',
          gender: 'male',
          phone_number: '0812345678',
        },
        {
          id: 2,
          first_name: 'สมหญิง',
          last_name: 'รักดี',
          id_card_number: '9876543210987',
          date_of_birth: '1992-05-15',
          gender: 'female',
          phone_number: '0898765432',
        },
      ];
      
      setSearchResults(mockResults);
      setIsLoading(false);
    }, 1000);
  };
  
  // ฟังก์ชันสำหรับการเลือกผู้ป่วย
  const handleSelectPatient = (patient: PatientData) => {
    onPatientSelect(patient);
  };
  
  // ฟังก์ชันสำหรับการแปลงเพศเป็นภาษาไทย
  const getGenderText = (gender: string): string => {
    if (gender === 'male') return 'ชาย';
    if (gender === 'female') return 'หญิง';
    return 'ไม่ระบุ';
  };
  
  return (
    <div className="space-y-4">
      <div className="flex flex-col space-y-2 md:flex-row md:space-y-0 md:space-x-2">
        <div className="flex-1">
          <Input
            placeholder="ค้นหาผู้ป่วย..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            onKeyDown={(e) => {
              if (e.key === 'Enter') {
                handleSearch();
              }
            }}
          />
        </div>
        
        <div className="flex space-x-2">
          <select
            className="px-3 py-2 border rounded-md"
            value={searchBy}
            onChange={(e) => setSearchBy(e.target.value)}
          >
            <option value="name">ชื่อ-นามสกุล</option>
            <option value="id_card">เลขบัตรประชาชน</option>
            <option value="hn">HN</option>
          </select>
          
          <Button 
            onClick={handleSearch}
            disabled={isLoading}
          >
            <SearchIcon className="h-4 w-4 mr-2" />
            ค้นหา
          </Button>
        </div>
      </div>
      
      {isLoading ? (
        <div className="flex justify-center py-8">
          <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-gray-900"></div>
        </div>
      ) : hasSearched ? (
        searchResults.length > 0 ? (
          <Card>
            <CardContent className="p-0">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>HN</TableHead>
                    <TableHead>ชื่อ-นามสกุล</TableHead>
                    <TableHead>เลขบัตรประชาชน</TableHead>
                    <TableHead>วันเกิด</TableHead>
                    <TableHead>เพศ</TableHead>
                    <TableHead>เบอร์โทรศัพท์</TableHead>
                    <TableHead></TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {searchResults.map((patient) => (
                    <TableRow key={patient.id}>
                      <TableCell>{patient.id}</TableCell>
                      <TableCell>{`${patient.first_name} ${patient.last_name}`}</TableCell>
                      <TableCell>{patient.id_card_number}</TableCell>
                      <TableCell>{formatThaiDate(new Date(patient.date_of_birth))}</TableCell>
                      <TableCell>{getGenderText(patient.gender)}</TableCell>
                      <TableCell>{patient.phone_number || '-'}</TableCell>
                      <TableCell>
                        <Button 
                          variant="ghost" 
                          size="sm"
                          onClick={() => handleSelectPatient(patient)}
                        >
                          <UserIcon className="h-4 w-4 mr-2" />
                          เลือก
                        </Button>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </CardContent>
          </Card>
        ) : (
          <div className="text-center py-8">
            <p className="text-gray-500">ไม่พบข้อมูลผู้ป่วย</p>
          </div>
        )
      ) : null}
    </div>
  );
};

export default PatientSearch; 