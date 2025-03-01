/**
 * AI Module Card Component
 * 
 * คอมโพเนนต์สำหรับแสดงการ์ดโมดูล AI ในหน้า Admin
 * แสดงข้อมูลสถิติและสถานะของโมดูล AI แต่ละตัว
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React from 'react';
import { 
  Card, 
  CardContent, 
  CardHeader, 
  CardTitle, 
  CardDescription, 
  CardFooter 
} from '@/components/ui/card';
import { 
  CircularProgressbar, 
  buildStyles 
} from 'react-circular-progressbar';
import 'react-circular-progressbar/dist/styles.css';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { 
  ArrowUpIcon, 
  ArrowDownIcon, 
  PlayIcon, 
  PauseIcon, 
  RefreshCwIcon, 
  SettingsIcon 
} from 'lucide-react';

// กำหนด interface สำหรับ props
interface AIModuleCardProps {
  id: string;
  name: string;
  description: string;
  type: 'diagnosis' | 'imaging' | 'prediction' | 'nlp' | 'optimization';
  status: 'active' | 'inactive' | 'training' | 'error';
  accuracy: number;
  usageCount: number;
  usageTrend: 'up' | 'down' | 'stable';
  usageTrendPercentage: number;
  lastUpdated: string;
  onToggleStatus: (id: string, currentStatus: string) => void;
  onRefreshModel: (id: string) => void;
  onOpenSettings: (id: string) => void;
}

// กำหนดสีตาม type ของโมดูล
const typeColors = {
  diagnosis: 'bg-blue-100 text-blue-800',
  imaging: 'bg-purple-100 text-purple-800',
  prediction: 'bg-green-100 text-green-800',
  nlp: 'bg-yellow-100 text-yellow-800',
  optimization: 'bg-pink-100 text-pink-800',
};

// กำหนดสีและข้อความตาม status
const statusConfig = {
  active: { color: 'bg-green-100 text-green-800', text: 'ใช้งานอยู่' },
  inactive: { color: 'bg-gray-100 text-gray-800', text: 'ไม่ได้ใช้งาน' },
  training: { color: 'bg-blue-100 text-blue-800', text: 'กำลังเทรน' },
  error: { color: 'bg-red-100 text-red-800', text: 'เกิดข้อผิดพลาด' },
};

// กำหนดสีและไอคอนตาม trend
const trendConfig = {
  up: { 
    icon: <ArrowUpIcon className="h-4 w-4 text-green-600" />, 
    color: 'text-green-600' 
  },
  down: { 
    icon: <ArrowDownIcon className="h-4 w-4 text-red-600" />, 
    color: 'text-red-600' 
  },
  stable: { 
    icon: null, 
    color: 'text-gray-600' 
  },
};

const AIModuleCard: React.FC<AIModuleCardProps> = ({
  id,
  name,
  description,
  type,
  status,
  accuracy,
  usageCount,
  usageTrend,
  usageTrendPercentage,
  lastUpdated,
  onToggleStatus,
  onRefreshModel,
  onOpenSettings,
}) => {
  // ฟังก์ชันสำหรับแปลงชื่อประเภทเป็นภาษาไทย
  const getTypeText = (type: string): string => {
    const typeMap: Record<string, string> = {
      diagnosis: 'วินิจฉัยโรค',
      imaging: 'วิเคราะห์ภาพ',
      prediction: 'ทำนาย',
      nlp: 'ประมวลผลภาษา',
      optimization: 'ปรับปรุงประสิทธิภาพ',
    };
    return typeMap[type] || type;
  };

  // ฟังก์ชันสำหรับแสดงไอคอนตาม status
  const getStatusIcon = (status: string) => {
    if (status === 'active') return <PlayIcon className="h-4 w-4" />;
    if (status === 'inactive') return <PauseIcon className="h-4 w-4" />;
    return null;
  };

  // ฟังก์ชันสำหรับแสดงข้อความ toggle ตาม status
  const getToggleText = (status: string): string => {
    return status === 'active' ? 'หยุดการทำงาน' : 'เริ่มการทำงาน';
  };

  // ฟังก์ชันสำหรับตรวจสอบว่าปุ่ม toggle สามารถกดได้หรือไม่
  const isToggleDisabled = (status: string): boolean => {
    return status === 'training' || status === 'error';
  };

  return (
    <Card className="w-full shadow-md hover:shadow-lg transition-shadow duration-300">
      <CardHeader className="pb-2">
        <div className="flex justify-between items-start">
          <div>
            <CardTitle className="text-xl font-bold">{name}</CardTitle>
            <CardDescription className="mt-1">{description}</CardDescription>
          </div>
          <Badge className={typeColors[type]}>
            {getTypeText(type)}
          </Badge>
        </div>
      </CardHeader>
      
      <CardContent className="pb-2">
        <div className="flex items-center justify-between mb-4">
          <Badge className={statusConfig[status].color}>
            {statusConfig[status].text}
          </Badge>
          <div className="text-sm text-gray-500">
            อัปเดตล่าสุด: {lastUpdated}
          </div>
        </div>
        
        <div className="grid grid-cols-2 gap-4">
          <div className="flex flex-col items-center">
            <div className="w-20 h-20 mb-2">
              <CircularProgressbar
                value={accuracy}
                text={`${accuracy}%`}
                styles={buildStyles({
                  textSize: '1.5rem',
                  pathColor: accuracy > 80 ? '#10B981' : accuracy > 60 ? '#F59E0B' : '#EF4444',
                  textColor: '#374151',
                  trailColor: '#E5E7EB',
                })}
              />
            </div>
            <div className="text-sm font-medium text-center">ความแม่นยำ</div>
          </div>
          
          <div className="flex flex-col justify-center">
            <div className="text-2xl font-bold">{usageCount.toLocaleString()}</div>
            <div className="flex items-center">
              <span className="text-sm mr-1">การใช้งาน</span>
              {trendConfig[usageTrend].icon}
              <span className={`text-sm ml-1 ${trendConfig[usageTrend].color}`}>
                {usageTrendPercentage}%
              </span>
            </div>
          </div>
        </div>
      </CardContent>
      
      <CardFooter className="flex justify-between pt-2">
        <Button
          variant="outline"
          size="sm"
          onClick={() => onToggleStatus(id, status)}
          disabled={isToggleDisabled(status)}
        >
          {getStatusIcon(status)}
          <span className="ml-1">{getToggleText(status)}</span>
        </Button>
        
        <div className="flex space-x-2">
          <Button
            variant="ghost"
            size="icon"
            onClick={() => onRefreshModel(id)}
            title="รีเฟรชโมเดล"
          >
            <RefreshCwIcon className="h-4 w-4" />
          </Button>
          
          <Button
            variant="ghost"
            size="icon"
            onClick={() => onOpenSettings(id)}
            title="ตั้งค่า"
          >
            <SettingsIcon className="h-4 w-4" />
          </Button>
        </div>
      </CardFooter>
    </Card>
  );
};

export default AIModuleCard; 