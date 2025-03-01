/**
 * Admin Dashboard
 * 
 * หน้าหลักของ Admin Panel แสดงข้อมูลสรุปและสถิติที่สำคัญของระบบ
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React, { useState, useEffect } from 'react';
import { 
  Chart as ChartJS, 
  CategoryScale, 
  LinearScale, 
  PointElement, 
  LineElement, 
  BarElement,
  Title, 
  Tooltip, 
  Legend, 
  ArcElement 
} from 'chart.js';
import { Line, Bar, Doughnut } from 'react-chartjs-2';
import { format } from 'date-fns';
import { th } from 'date-fns/locale';

// Register ChartJS components
ChartJS.register(
  CategoryScale, 
  LinearScale, 
  PointElement, 
  LineElement, 
  BarElement,
  Title, 
  Tooltip, 
  Legend,
  ArcElement
);

// ประเภทข้อมูลสำหรับ Dashboard stats
interface DashboardStats {
  totalPatients: number;
  newPatientsToday: number;
  appointmentsToday: number;
  pendingAppointments: number;
  activeUsers: number;
  systemHealth: 'healthy' | 'warning' | 'critical';
}

// ประเภทข้อมูลสำหรับ Chart data
interface ChartData {
  labels: string[];
  datasets: {
    label: string;
    data: number[];
    backgroundColor?: string | string[];
    borderColor?: string;
    borderWidth?: number;
  }[];
}

const Dashboard: React.FC = () => {
  // State สำหรับเก็บข้อมูลสถิติ
  const [stats, setStats] = useState<DashboardStats>({
    totalPatients: 0,
    newPatientsToday: 0,
    appointmentsToday: 0,
    pendingAppointments: 0,
    activeUsers: 0,
    systemHealth: 'healthy'
  });

  // State สำหรับเก็บข้อมูลกราฟผู้ป่วยรายวัน
  const [patientChartData, setPatientChartData] = useState<ChartData>({
    labels: [],
    datasets: [
      {
        label: 'ผู้ป่วยใหม่',
        data: [],
        borderColor: '#3b82f6',
        backgroundColor: 'rgba(59, 130, 246, 0.5)',
      }
    ]
  });

  // State สำหรับเก็บข้อมูลกราฟการนัดหมาย
  const [appointmentChartData, setAppointmentChartData] = useState<ChartData>({
    labels: [],
    datasets: [
      {
        label: 'การนัดหมาย',
        data: [],
        backgroundColor: '#10b981',
      }
    ]
  });

  // State สำหรับเก็บข้อมูลกราฟประเภทผู้ป่วย
  const [patientTypeChartData, setPatientTypeChartData] = useState<ChartData>({
    labels: ['ทั่วไป', 'ฉุกเฉิน', 'ผู้ป่วยประจำ', 'ผู้ป่วยใหม่', 'นัดติดตามผล'],
    datasets: [
      {
        label: 'ประเภทผู้ป่วย',
        data: [0, 0, 0, 0, 0],
        backgroundColor: [
          'rgba(255, 99, 132, 0.7)',
          'rgba(54, 162, 235, 0.7)',
          'rgba(255, 206, 86, 0.7)',
          'rgba(75, 192, 192, 0.7)',
          'rgba(153, 102, 255, 0.7)',
        ],
        borderWidth: 1,
      }
    ]
  });

  // โหลดข้อมูลเมื่อ component mount
  useEffect(() => {
    // ในสถานการณ์จริง จะต้องเรียก API เพื่อดึงข้อมูล
    // แต่ในตัวอย่างนี้จะใช้ข้อมูลจำลอง
    
    // จำลองข้อมูลสถิติ
    setStats({
      totalPatients: 12458,
      newPatientsToday: 24,
      appointmentsToday: 87,
      pendingAppointments: 15,
      activeUsers: 42,
      systemHealth: 'healthy'
    });

    // จำลองข้อมูลกราฟผู้ป่วยรายวัน
    const last7Days = Array.from({ length: 7 }, (_, i) => {
      const date = new Date();
      date.setDate(date.getDate() - (6 - i));
      return format(date, 'd MMM', { locale: th });
    });

    setPatientChartData({
      labels: last7Days,
      datasets: [
        {
          label: 'ผู้ป่วยใหม่',
          data: [18, 25, 20, 30, 15, 22, 24],
          borderColor: '#3b82f6',
          backgroundColor: 'rgba(59, 130, 246, 0.5)',
        }
      ]
    });

    // จำลองข้อมูลกราฟการนัดหมาย
    setAppointmentChartData({
      labels: last7Days,
      datasets: [
        {
          label: 'การนัดหมาย',
          data: [65, 72, 80, 58, 90, 75, 87],
          backgroundColor: '#10b981',
        }
      ]
    });

    // จำลองข้อมูลกราฟประเภทผู้ป่วย
    setPatientTypeChartData({
      labels: ['ทั่วไป', 'ฉุกเฉิน', 'ผู้ป่วยประจำ', 'ผู้ป่วยใหม่', 'นัดติดตามผล'],
      datasets: [
        {
          label: 'ประเภทผู้ป่วย',
          data: [350, 75, 580, 240, 310],
          backgroundColor: [
            'rgba(255, 99, 132, 0.7)',
            'rgba(54, 162, 235, 0.7)',
            'rgba(255, 206, 86, 0.7)',
            'rgba(75, 192, 192, 0.7)',
            'rgba(153, 102, 255, 0.7)',
          ],
          borderWidth: 1,
        }
      ]
    });
  }, []);

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold text-gray-800 mb-6">แดชบอร์ดผู้ดูแลระบบ</h1>
      
      {/* Stats Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
        <div className="bg-white rounded-lg shadow p-6">
          <h2 className="text-sm font-medium text-gray-500">ผู้ป่วยทั้งหมด</h2>
          <p className="text-3xl font-bold text-gray-800">{stats.totalPatients.toLocaleString()}</p>
          <div className="mt-2 flex items-center text-sm">
            <span className="text-green-500 font-medium">+{stats.newPatientsToday}</span>
            <span className="ml-2 text-gray-500">วันนี้</span>
          </div>
        </div>
        
        <div className="bg-white rounded-lg shadow p-6">
          <h2 className="text-sm font-medium text-gray-500">การนัดหมายวันนี้</h2>
          <p className="text-3xl font-bold text-gray-800">{stats.appointmentsToday}</p>
          <div className="mt-2 flex items-center text-sm">
            <span className="text-yellow-500 font-medium">{stats.pendingAppointments}</span>
            <span className="ml-2 text-gray-500">รอดำเนินการ</span>
          </div>
        </div>
        
        <div className="bg-white rounded-lg shadow p-6">
          <h2 className="text-sm font-medium text-gray-500">ผู้ใช้งานที่กำลังใช้งาน</h2>
          <p className="text-3xl font-bold text-gray-800">{stats.activeUsers}</p>
          <div className="mt-2 flex items-center text-sm">
            <span className={`font-medium ${
              stats.systemHealth === 'healthy' ? 'text-green-500' : 
              stats.systemHealth === 'warning' ? 'text-yellow-500' : 'text-red-500'
            }`}>
              {stats.systemHealth === 'healthy' ? 'ระบบทำงานปกติ' : 
               stats.systemHealth === 'warning' ? 'ระบบมีคำเตือน' : 'ระบบมีปัญหา'}
            </span>
          </div>
        </div>
      </div>
      
      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-8">
        <div className="bg-white rounded-lg shadow p-6">
          <h2 className="text-lg font-medium text-gray-800 mb-4">ผู้ป่วยใหม่ (7 วันล่าสุด)</h2>
          <Line 
            data={patientChartData} 
            options={{
              responsive: true,
              plugins: {
                legend: {
                  position: 'top' as const,
                },
              },
            }} 
          />
        </div>
        
        <div className="bg-white rounded-lg shadow p-6">
          <h2 className="text-lg font-medium text-gray-800 mb-4">การนัดหมาย (7 วันล่าสุด)</h2>
          <Bar 
            data={appointmentChartData} 
            options={{
              responsive: true,
              plugins: {
                legend: {
                  position: 'top' as const,
                },
              },
            }} 
          />
        </div>
      </div>
      
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        <div className="bg-white rounded-lg shadow p-6 lg:col-span-1">
          <h2 className="text-lg font-medium text-gray-800 mb-4">ประเภทผู้ป่วย</h2>
          <div className="h-64">
            <Doughnut 
              data={patientTypeChartData} 
              options={{
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                  legend: {
                    position: 'right' as const,
                  },
                },
              }} 
            />
          </div>
        </div>
        
        <div className="bg-white rounded-lg shadow p-6 lg:col-span-2">
          <h2 className="text-lg font-medium text-gray-800 mb-4">กิจกรรมล่าสุด</h2>
          <div className="overflow-x-auto">
            <table className="min-w-full divide-y divide-gray-200">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">เวลา</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">กิจกรรม</th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">ผู้ใช้</th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                <tr>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">10:45 น.</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-800">เพิ่มผู้ป่วยใหม่</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">คุณหมอสมชาย</td>
                </tr>
                <tr>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">10:30 น.</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-800">อัปเดตประวัติผู้ป่วย</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">พยาบาลสมศรี</td>
                </tr>
                <tr>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">10:15 น.</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-800">สร้างการนัดหมายใหม่</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">เจ้าหน้าที่วิภา</td>
                </tr>
                <tr>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">10:00 น.</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-800">เข้าสู่ระบบ</td>
                  <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">ผู้ดูแลระบบ</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard; 