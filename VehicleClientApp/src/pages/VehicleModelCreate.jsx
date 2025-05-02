// src/pages/VehicleModelCreate.jsx
import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import FormField from '../components/FormField';
import vehicleModelStore from '../stores/VehicleModelStore';
import vehicleMakeStore from '../stores/VehicleMakeStore';

export const VehicleModelCreate = () => {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [name, setName] = useState('');
  const [abrv, setAbrv] = useState('');
  const [makeId, setMakeId] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    vehicleMakeStore.fetchMakes();

    if (isEdit) {
      fetch(`/api/VehicleModels/${id}`)
        .then(res => {
          if (!res.ok) throw new Error('Failed to load');
          return res.json();
        })
        .then(dto => {
          setName(dto.name);
          setAbrv(dto.abrv);
          setMakeId(dto.vehicleMakeId.toString());
        })
        .catch(err => {
          alert(err.message);
          navigate('/models');
        });
    }
  }, [id, isEdit, navigate]);

  const handleSubmit = async e => {
    e.preventDefault();
    setSaving(true);

    try {
      const dto = {
        name: name.trim(),
        abrv: abrv.trim(),
        vehicleMakeId: Number(makeId),
      };

      if (isEdit) {
        await vehicleModelStore.updateModel(id, dto);
      } else {
        await vehicleModelStore.createModel(dto);
      }

      navigate('/models');
    } catch (error) {
      const data = error.response?.data;
      let msg = 'Save failed.';
      if (data?.errors) msg = Object.values(data.errors).flat().join('\n');
      else if (data?.detail) msg = data.detail;
      else if (data?.title) msg = data.title;
      else if (error.message) msg = error.message;
      alert(msg);
    } finally {
      setSaving(false);
    }
  };

  return (
    <div>
      <h2>{isEdit ? 'Edit' : 'Create'} Vehicle Model</h2>
      <form onSubmit={handleSubmit} style={{ marginBottom: '1rem' }}>
        <FormField label="Name">
          <input value={name} onChange={e => setName(e.target.value)} required />
        </FormField>

        <FormField label="Abbreviation">
          <input value={abrv} onChange={e => setAbrv(e.target.value)} />
        </FormField>

        <FormField label="Make">
          <select value={makeId} onChange={e => setMakeId(e.target.value)} required>
            <option value="">— select make —</option>
            {vehicleMakeStore.makes.map(m => (
              <option key={m.id} value={m.id}>
                {m.name}
              </option>
            ))}
          </select>
        </FormField>

        <button type="submit" disabled={saving}>
          {saving ? 'Saving…' : isEdit ? 'Update' : 'Create'}
        </button>
        <button type="button" onClick={() => navigate('/models')} style={{ marginLeft: '0.5rem' }}>
          Cancel
        </button>
      </form>
    </div>
  );
};
