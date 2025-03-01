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

/**
 * ฟังก์ชันสำหรับรวม class names เข้าด้วยกัน
 * ใช้ clsx และ tailwind-merge เพื่อจัดการกับ class ที่ซ้ำซ้อนหรือขัดแย้งกัน
 * 
 * @param inputs - รายการ class names ที่ต้องการรวม
 * @returns รายการ class names ที่รวมแล้ว
 */
export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

/**
 * ฟังก์ชันสำหรับฟอร์แมตวันที่เป็นภาษาไทย
 * 
 * @param date - วันที่ที่ต้องการฟอร์แมต
 * @returns วันที่ในรูปแบบภาษาไทย
 */
export function formatThaiDate(date: Date | string): string {
  const d = typeof date === 'string' ? new Date(date) : date;
  
  const thaiMonths = [
    'มกราคม', 'กุมภาพันธ์', 'มีนาคม', 'เมษายน', 'พฤษภาคม', 'มิถุนายน',
    'กรกฎาคม', 'สิงหาคม', 'กันยายน', 'ตุลาคม', 'พฤศจิกายน', 'ธันวาคม'
  ];
  
  const day = d.getDate();
  const month = thaiMonths[d.getMonth()];
  const year = d.getFullYear() + 543; // แปลงเป็นปี พ.ศ.
  
  return `${day} ${month} ${year}`;
}

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
 * ฟังก์ชันสำหรับฟอร์แมตวันที่และเวลาเป็นภาษาไทย
 * 
 * @param date - วันที่และเวลาที่ต้องการฟอร์แมต
 * @returns วันที่และเวลาในรูปแบบภาษาไทย
 */
export function formatThaiDateTime(date: Date | string): string {
  return `${formatThaiDate(date)} ${formatThaiTime(date)}`;
}

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