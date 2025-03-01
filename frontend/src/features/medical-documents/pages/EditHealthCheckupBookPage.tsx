import React from 'react';
import HealthCheckupBookForm from '../components/HealthCheckupBookForm';

const EditHealthCheckupBookPage: React.FC = () => {
  return (
    <div className="container mx-auto px-4 py-8">
      <HealthCheckupBookForm isEdit={true} />
    </div>
  );
};

export default EditHealthCheckupBookPage; 