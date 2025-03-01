/**
 * Container Component
 * 
 * คอมโพเนนต์สำหรับกำหนดความกว้างและการจัดวางเนื้อหาให้อยู่ตรงกลางหน้าจอ
 * ใช้สำหรับครอบเนื้อหาในหน้าต่างๆ ของแอปพลิเคชัน
 * 
 * @author ทีมพัฒนา MeditechZ
 * @version 1.0.0
 * @date March 1, 2025
 */

import React from 'react';
import { cn } from '@/lib/utils';

interface ContainerProps extends React.HTMLAttributes<HTMLDivElement> {
  children: React.ReactNode;
}

export function Container({ 
  children, 
  className, 
  ...props 
}: ContainerProps) {
  return (
    <div 
      className={cn(
        "container mx-auto px-4 md:px-6 max-w-7xl", 
        className
      )} 
      {...props}
    >
      {children}
    </div>
  );
} 