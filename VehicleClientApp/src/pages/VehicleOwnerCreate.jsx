// src/pages/VehicleOwnerCreate.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import FormField from '../components/FormField';
import ownerStore from '../stores/VehicleOwnerStore';
import { ROUTES } from '../routes';

export const VehicleOwnerCreate = () => {
  const navigate = useNavigate();
  const [firstName, setFirst] = useState('');
  const [lastName, setLast] = useState('');
  const [dob, setDob] = useState('');
  const [saving, setSaving] = useState(false);

  const handleSubmit = async e => {
    e.preventDefault();
    setSaving(true);
    try {
      await ownerStore.createOwner({
        firstName,
        lastName,
        dob: new Date(dob),
      });
      navigate(ROUTES.OWNERS);
    } catch {
      alert('Save failed');
    } finally {
      setSaving(false);
    }
  };

  return (
    <div>
      <h2>Create Vehicle Owner</h2>
      <form onSubmit={handleSubmit}>
        <FormField label="First Name">
          <input value={firstName} onChange={e => setFirst(e.target.value)} required />
        </FormField>

        <FormField label="Last Name">
          <input value={lastName} onChange={e => setLast(e.target.value)} required />
        </FormField>

        <FormField label="Date of Birth">
          <input type="date" value={dob} onChange={e => setDob(e.target.value)} required />
        </FormField>

        <button type="submit" disabled={saving}>
          {saving ? 'Saving…' : 'Create'}
        </button>
        <button type="button" onClick={() => navigate(ROUTES.OWNERS)} style={{ marginLeft: '0.5rem' }}>
          Cancel
        </button>
      </form>
    </div>
  );
};
