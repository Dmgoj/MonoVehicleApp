import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import FormField from '../components/FormField';
import vehicleMakeStore from '../stores/VehicleMakeStore';

export const VehicleMakeCreate = () => {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [name, setName] = useState('');
  const [abrv, setAbrv] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (isEdit) {
      vehicleMakeStore.fetchMakes();
      fetch(`/api/VehicleMakes/${id}`)
        .then(r => r.json())
        .then(dto => {
          setName(dto.name);
          setAbrv(dto.abrv);
        })
        .catch(() => alert('Failed to load'));
    }
  }, [id]);

  const handleSubmit = async e => {
    e.preventDefault();
    setSaving(true);
    try {
      const dto = { name, abrv };
      if (isEdit) {
        await vehicleMakeStore.updateMake(id, dto);
      } else {
        await vehicleMakeStore.createMake(dto);
      }
      navigate('/');
    } catch (error) {
      // extract validation msg as before…
      alert('Save failed');
    } finally {
      setSaving(false);
    }
  };

  return (
    <div>
      <h2>{isEdit ? 'Edit' : 'Create'} Vehicle Make</h2>
      <form onSubmit={handleSubmit} style={{ marginBottom: '1rem' }}>
        <FormField label="Name">
          <input value={name} onChange={e => setName(e.target.value)} required />
        </FormField>

        <FormField label="Abbreviation">
          <input value={abrv} onChange={e => setAbrv(e.target.value)} />
        </FormField>

        <button type="submit" disabled={saving}>
          {saving ? 'Saving…' : isEdit ? 'Update' : 'Create'}
        </button>
        <button type="button" onClick={() => navigate('/')} style={{ marginLeft: '0.5rem' }}>
          Cancel
        </button>
      </form>
    </div>
  );
};
