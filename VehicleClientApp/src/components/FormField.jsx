import React from 'react';

const FormField = ({ label, children }) => (
  <div style={{ marginBottom: '1rem' }}>
    <label style={{ display: 'block', fontWeight: 'bold' }}>{label}</label>
    {children}
  </div>
);

export default FormField;
