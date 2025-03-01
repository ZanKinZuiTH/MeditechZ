import React from 'react';
import HealthCheckupBookForm from '../components/HealthCheckupBookForm';

const CreateHealthCheckupBookPage: React.FC = () => {
  return (
    <div className="container mx-auto px-4 py-8">
      <HealthCheckupBookForm isEdit={false} />
    </div>
  );
};

export default CreateHealthCheckupBookPage; 