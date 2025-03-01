/**
 * Utility Functions
 * 
 * ไฟล์รวมฟังก์ชันช่วยเหลือต่างๆ สำหรับใช้ในโปรเจค
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import { type ClassValue, clsx } from 'clsx';
import { twMerge } from 'tailwind-merge';
import { format } from 'date-fns';
import { th } from 'date-fns/locale';

/**
 * ฟังก์ชันสำหรับรวม class names
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

/**
 * ฟังก์ชันสำหรับแปลงวันที่เป็นรูปแบบภาษาไทย
 * @param date วันที่ที่ต้องการแปลง
 * @param formatStr รูปแบบการแสดงผล (ค่าเริ่มต้น: 'd MMMM yyyy')
 * @returns วันที่ในรูปแบบภาษาไทย
 */
export const formatThaiDate = (date: Date | string, formatStr: string = 'd MMMM yyyy'): string => {
  try {
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return format(dateObj, formatStr, { locale: th });
  } catch (error) {
    console.error('Error formatting date:', error);
    return 'วันที่ไม่ถูกต้อง';
  }
};

/**
 * ฟังก์ชันสำหรับฟอร์แมตเวลาเป็นภาษาไทย
 * 
 * @param date - วันที่และเวลาที่ต้องการฟอร์แมต
 * @returns เวลาในรูปแบบภาษาไทย
 */
export function formatThaiTime(date: Date | string): string {
  const d = typeof date === 'string' ? new Date(date) : date;
  
  const hours = d.getHours().toString().padStart(2, '0');
  const minutes = d.getMinutes().toString().padStart(2, '0');
  
  return `${hours}:${minutes} น.`;
}

/**
 * ฟังก์ชันสำหรับแปลงวันที่เป็นรูปแบบภาษาไทยพร้อมเวลา
 * สามารถกำหนดรูปแบบการแสดงผลได้ หรือใช้ค่าเริ่มต้น
 * 
 * @param date - วันที่และเวลาที่ต้องการฟอร์แมต
 * @param formatStr - รูปแบบการแสดงผล (ถ้าไม่ระบุจะใช้ 'd MMMM yyyy เวลา HH:mm น.')
 * @returns วันที่และเวลาในรูปแบบภาษาไทย
 */
export function formatThaiDateTime(date: Date | string, formatStr?: string): string {
  try {
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    
    if (formatStr) {
      return format(dateObj, formatStr, { locale: th });
    }
    
    return format(dateObj, 'd MMMM yyyy เวลา HH:mm น.', { locale: th });
  } catch (error) {
    console.error('Error formatting date time:', error);
    return 'วันที่และเวลาไม่ถูกต้อง';
  }
}

/**
 * ฟังก์ชันสำหรับแปลงวันที่เป็นรูปแบบย่อภาษาไทย
 * @param date วันที่ที่ต้องการแปลง
 * @returns วันที่ในรูปแบบย่อภาษาไทย (วว/ดด/ปปปป)
 */
export const formatShortThaiDate = (date: Date | string): string => {
  try {
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return format(dateObj, 'dd/MM/yyyy');
  } catch (error) {
    console.error('Error formatting short date:', error);
    return 'วันที่ไม่ถูกต้อง';
  }
};

/**
 * ฟังก์ชันสำหรับแปลงวันที่จาก string เป็น Date object
 * @param dateString string วันที่ในรูปแบบ ISO (YYYY-MM-DD)
 * @returns Date object
 */
export const parseISODate = (dateString: string): Date => {
  try {
    return new Date(dateString);
  } catch (error) {
    console.error('Error parsing date:', error);
    return new Date();
  }
};

/**
 * ฟังก์ชันสำหรับตรวจสอบว่าวันที่ถูกต้องหรือไม่
 * @param date วันที่ที่ต้องการตรวจสอบ
 * @returns boolean ค่าความถูกต้องของวันที่
 */
export const isValidDate = (date: any): boolean => {
  if (!date) return false;
  
  const d = new Date(date);
  return !isNaN(d.getTime());
};

/**
 * ฟังก์ชันสำหรับคำนวณอายุจากวันเกิด
 * @param birthDate วันเกิด
 * @returns อายุเป็นปี
 */
export const calculateAge = (birthDate: Date | string): number => {
  try {
    const birthDateObj = typeof birthDate === 'string' ? new Date(birthDate) : birthDate;
    const today = new Date();
    let age = today.getFullYear() - birthDateObj.getFullYear();
    const monthDiff = today.getMonth() - birthDateObj.getMonth();
    
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDateObj.getDate())) {
      age--;
    }
    
    return age;
  } catch (error) {
    console.error('Error calculating age:', error);
    return 0;
  }
};

/**
 * ฟังก์ชันสำหรับแปลงเปอร์เซ็นต์เป็นสี
 * ใช้สำหรับแสดงระดับความเชื่อมั่นในการวินิจฉัย
 * 
 * @param percentage - ค่าเปอร์เซ็นต์ (0-1)
 * @returns รหัสสี HEX
 */
export function getConfidenceColor(percentage: number): string {
  if (percentage >= 0.8) {
    return '#10b981'; // สีเขียว - ความเชื่อมั่นสูง
  } else if (percentage >= 0.6) {
    return '#f59e0b'; // สีส้ม - ความเชื่อมั่นปานกลาง
  } else {
    return '#ef4444'; // สีแดง - ความเชื่อมั่นต่ำ
  }
}

/**
 * ฟังก์ชันสำหรับตัดข้อความให้สั้นลงและเพิ่ม ... ท้ายข้อความ
 * 
 * @param text - ข้อความที่ต้องการตัด
 * @param maxLength - ความยาวสูงสุดที่ต้องการ
 * @returns ข้อความที่ถูกตัดให้สั้นลง
 */
export function truncateText(text: string, maxLength: number): string {
  if (text.length <= maxLength) {
    return text;
  }
  
  return text.slice(0, maxLength) + '...';
} 