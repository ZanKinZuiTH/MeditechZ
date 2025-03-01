/**
 * AI Modules Management Page
 * 
 * หน้าจัดการโมดูล AI ในส่วน Admin
 * แสดงรายการโมดูล AI ทั้งหมดในระบบ พร้อมสถานะและการจัดการ
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React, { useState, useEffect } from 'react';
import { 
  Tabs, 
  TabsContent, 
  TabsList, 
  TabsTrigger 
} from '@/components/ui/tabs';
import { 
  Card, 
  CardContent, 
  CardDescription, 
  CardHeader, 
  CardTitle 
} from '@/components/ui/card';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';
import { 
  PlusIcon, 
  SearchIcon, 
  RefreshCwIcon 
} from 'lucide-react';
import { useToast } from '@/components/ui/use-toast';

// นำเข้าคอมโพเนนต์ AIModuleCard ที่เราสร้างไว้
import AIModuleCard from '../components/AIModuleCard';

// กำหนด interface สำหรับข้อมูลโมดูล AI
interface AIModule {
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
}

// ข้อมูลจำลองสำหรับโมดูล AI
const mockAIModules: AIModule[] = [
  {
    id: 'ai-diag-001',
    name: 'ระบบวินิจฉัยโรคทั่วไป',
    description: 'วินิจฉัยโรคทั่วไปจากอาการและประวัติผู้ป่วย',
    type: 'diagnosis',
    status: 'active',
    accuracy: 87,
    usageCount: 1245,
    usageTrend: 'up',
    usageTrendPercentage: 12,
    lastUpdated: '01/03/2025'
  },
  {
    id: 'ai-img-001',
    name: 'วิเคราะห์ภาพเอกซเรย์ปอด',
    description: 'ตรวจหาความผิดปกติจากภาพเอกซเรย์ปอด',
    type: 'imaging',
    status: 'active',
    accuracy: 92,
    usageCount: 876,
    usageTrend: 'up',
    usageTrendPercentage: 8,
    lastUpdated: '28/02/2025'
  },
  {
    id: 'ai-pred-001',
    name: 'ทำนายความเสี่ยงโรคหัวใจ',
    description: 'ประเมินความเสี่ยงการเกิดโรคหัวใจจากข้อมูลผู้ป่วย',
    type: 'prediction',
    status: 'inactive',
    accuracy: 78,
    usageCount: 543,
    usageTrend: 'down',
    usageTrendPercentage: 5,
    lastUpdated: '25/02/2025'
  },
  {
    id: 'ai-nlp-001',
    name: 'วิเคราะห์บันทึกแพทย์',
    description: 'สกัดข้อมูลสำคัญจากบันทึกของแพทย์',
    type: 'nlp',
    status: 'training',
    accuracy: 65,
    usageCount: 321,
    usageTrend: 'stable',
    usageTrendPercentage: 0,
    lastUpdated: '01/03/2025'
  },
  {
    id: 'ai-opt-001',
    name: 'จัดตารางนัดหมายอัตโนมัติ',
    description: 'ปรับปรุงประสิทธิภาพการจัดตารางนัดหมายผู้ป่วย',
    type: 'optimization',
    status: 'error',
    accuracy: 73,
    usageCount: 189,
    usageTrend: 'down',
    usageTrendPercentage: 15,
    lastUpdated: '27/02/2025'
  }
];

const AIModules: React.FC = () => {
  // สร้าง state สำหรับเก็บข้อมูลโมดูล AI
  const [modules, setModules] = useState<AIModule[]>([]);
  // สร้าง state สำหรับการค้นหา
  const [searchQuery, setSearchQuery] = useState('');
  // สร้าง state สำหรับการกรองตามประเภท
  const [activeTab, setActiveTab] = useState('all');
  // สร้าง state สำหรับแสดงสถานะการโหลด
  const [isLoading, setIsLoading] = useState(true);

  const { toast } = useToast();

  // โหลดข้อมูลโมดูล AI เมื่อคอมโพเนนต์ถูกโหลด
  useEffect(() => {
    const fetchAIModules = async () => {
      try {
        // ในสถานการณ์จริง จะต้องเรียก API
        // const response = await fetch('/api/ai-modules');
        // const data = await response.json();
        // setModules(data);

        // จำลองการโหลดข้อมูล
        setTimeout(() => {
          setModules(mockAIModules);
          setIsLoading(false);
        }, 1000);
      } catch (error) {
        console.error('Error fetching AI modules:', error);
        toast({
          title: 'เกิดข้อผิดพลาด',
          description: 'ไม่สามารถโหลดข้อมูลโมดูล AI ได้',
          variant: 'destructive',
        });
        setIsLoading(false);
      }
    };

    fetchAIModules();
  }, [toast]);

  // กรองโมดูลตามการค้นหาและประเภท
  const filteredModules = modules.filter(module => {
    const matchesSearch = 
      module.name.toLowerCase().includes(searchQuery.toLowerCase()) || 
      module.description.toLowerCase().includes(searchQuery.toLowerCase());
    
    const matchesType = activeTab === 'all' || module.type === activeTab;
    
    return matchesSearch && matchesType;
  });

  // ฟังก์ชันสำหรับเปลี่ยนสถานะโมดูล
  const handleToggleStatus = (id: string, currentStatus: string) => {
    const newStatus = currentStatus === 'active' ? 'inactive' : 'active';
    
    // อัปเดต state
    setModules(prevModules => 
      prevModules.map(module => 
        module.id === id ? { ...module, status: newStatus as any } : module
      )
    );
    
    // แสดงข้อความแจ้งเตือน
    toast({
      title: 'อัปเดตสถานะสำเร็จ',
      description: `โมดูล ${modules.find(m => m.id === id)?.name} ถูกเปลี่ยนเป็น ${newStatus === 'active' ? 'ใช้งานอยู่' : 'ไม่ได้ใช้งาน'}`,
    });
    
    // ในสถานการณ์จริง จะต้องส่งคำขอไปยัง API
    // await fetch(`/api/ai-modules/${id}/status`, {
    //   method: 'PATCH',
    //   headers: { 'Content-Type': 'application/json' },
    //   body: JSON.stringify({ status: newStatus })
    // });
  };

  // ฟังก์ชันสำหรับรีเฟรชโมเดล
  const handleRefreshModel = (id: string) => {
    toast({
      title: 'เริ่มรีเฟรชโมเดล',
      description: `กำลังรีเฟรชโมเดล ${modules.find(m => m.id === id)?.name}`,
    });
    
    // ในสถานการณ์จริง จะต้องส่งคำขอไปยัง API
    // await fetch(`/api/ai-modules/${id}/refresh`, { method: 'POST' });
  };

  // ฟังก์ชันสำหรับเปิดการตั้งค่าโมเดล
  const handleOpenSettings = (id: string) => {
    // ในสถานการณ์จริง จะต้องนำทางไปยังหน้าตั้งค่า
    console.log(`Open settings for module ${id}`);
    // navigate(`/admin/ai-modules/${id}/settings`);
  };

  // ฟังก์ชันสำหรับเพิ่มโมดูลใหม่
  const handleAddNewModule = () => {
    // ในสถานการณ์จริง จะต้องนำทางไปยังหน้าเพิ่มโมดูล
    console.log('Add new AI module');
    // navigate('/admin/ai-modules/new');
  };

  return (
    <div className="container mx-auto py-6">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-3xl font-bold">จัดการโมดูล AI</h1>
          <p className="text-gray-500 mt-1">
            ดูและจัดการโมดูล AI ทั้งหมดในระบบ
          </p>
        </div>
        <Button onClick={handleAddNewModule}>
          <PlusIcon className="h-4 w-4 mr-2" />
          เพิ่มโมดูลใหม่
        </Button>
      </div>

      <Card className="mb-6">
        <CardHeader className="pb-3">
          <CardTitle>ค้นหาและกรอง</CardTitle>
          <CardDescription>
            ค้นหาโมดูล AI ตามชื่อหรือคำอธิบาย และกรองตามประเภท
          </CardDescription>
        </CardHeader>
        <CardContent>
          <div className="flex flex-col md:flex-row gap-4">
            <div className="relative flex-1">
              <SearchIcon className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
              <Input
                placeholder="ค้นหาโมดูล AI..."
                className="pl-10"
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
              />
            </div>
            <div className="flex items-center">
              <Button 
                variant="outline" 
                size="icon"
                onClick={() => {
                  setIsLoading(true);
                  setTimeout(() => {
                    setModules(mockAIModules);
                    setIsLoading(false);
                  }, 500);
                }}
                title="รีเฟรชข้อมูล"
              >
                <RefreshCwIcon className="h-4 w-4" />
              </Button>
            </div>
          </div>
        </CardContent>
      </Card>

      <Tabs defaultValue="all" value={activeTab} onValueChange={setActiveTab}>
        <TabsList className="mb-6">
          <TabsTrigger value="all">ทั้งหมด</TabsTrigger>
          <TabsTrigger value="diagnosis">วินิจฉัยโรค</TabsTrigger>
          <TabsTrigger value="imaging">วิเคราะห์ภาพ</TabsTrigger>
          <TabsTrigger value="prediction">ทำนาย</TabsTrigger>
          <TabsTrigger value="nlp">ประมวลผลภาษา</TabsTrigger>
          <TabsTrigger value="optimization">ปรับปรุงประสิทธิภาพ</TabsTrigger>
        </TabsList>

        <TabsContent value="all" className="mt-0">
          {renderModuleGrid(filteredModules)}
        </TabsContent>
        
        <TabsContent value="diagnosis" className="mt-0">
          {renderModuleGrid(filteredModules)}
        </TabsContent>
        
        <TabsContent value="imaging" className="mt-0">
          {renderModuleGrid(filteredModules)}
        </TabsContent>
        
        <TabsContent value="prediction" className="mt-0">
          {renderModuleGrid(filteredModules)}
        </TabsContent>
        
        <TabsContent value="nlp" className="mt-0">
          {renderModuleGrid(filteredModules)}
        </TabsContent>
        
        <TabsContent value="optimization" className="mt-0">
          {renderModuleGrid(filteredModules)}
        </TabsContent>
      </Tabs>
    </div>
  );

  // ฟังก์ชันสำหรับแสดงกริดของโมดูล AI
  function renderModuleGrid(modules: AIModule[]) {
    if (isLoading) {
      return (
        <div className="flex justify-center items-center h-64">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary"></div>
        </div>
      );
    }

    if (modules.length === 0) {
      return (
        <div className="flex flex-col items-center justify-center h-64">
          <p className="text-xl text-gray-500 mb-4">ไม่พบโมดูล AI</p>
          <Button variant="outline" onClick={() => setSearchQuery('')}>
            ล้างการค้นหา
          </Button>
        </div>
      );
    }

    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {modules.map((module) => (
          <AIModuleCard
            key={module.id}
            {...module}
            onToggleStatus={handleToggleStatus}
            onRefreshModel={handleRefreshModel}
            onOpenSettings={handleOpenSettings}
          />
        ))}
      </div>
    );
  }
};

export default AIModules; 