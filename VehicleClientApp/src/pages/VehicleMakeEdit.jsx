import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import FormField from '../components/FormField';
import vehicleMakeStore from '../stores/VehicleMakeStore';

export const VehicleMakeEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [name, setName] = useState('');
  const [abrv, setAbrv] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    fetch(`/api/VehicleMakes/${id}`)
      .then(res => {
        if (!res.ok) throw new Error('Network response was not ok');
        return res.json();
      })
      .then(dto => {
        setName(dto.name);
        setAbrv(dto.abrv);
      })
      .catch(() => {
        alert('Failed to load vehicle make.');
        navigate('/');
      });
  }, [id]);

  const handleSubmit = async e => {
    e.preventDefault();
    setSaving(true);
    try {
      await vehicleMakeStore.updateMake(id, { name, abrv });
      navigate('/');
    } catch (err) {
      alert('Save failed.');
    } finally {
      setSaving(false);
    }
  };

  return (
    <div>
      <h2>Edit Vehicle Make</h2>
      <form onSubmit={handleSubmit} style={{ marginBottom: '1rem' }}>
        <FormField label="Name">
          <input value={name} onChange={e => setName(e.target.value)} required />
        </FormField>

        <FormField label="Abbreviation">
          <input value={abrv} onChange={e => setAbrv(e.target.value)} />
        </FormField>

        <button type="submit" disabled={saving}>
          {saving ? 'Saving…' : 'Update'}
        </button>
        <button type="button" onClick={() => navigate('/')} style={{ marginLeft: '0.5rem' }}>
          Cancel
        </button>
      </form>
    </div>
  );
};
