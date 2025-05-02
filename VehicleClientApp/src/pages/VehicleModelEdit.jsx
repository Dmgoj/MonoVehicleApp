import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import FormField from '../components/FormField';
import vehicleModelStore from '../stores/VehicleModelStore';
import vehicleMakeStore from '../stores/VehicleMakeStore';

export const VehicleModelEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [name, setName] = useState('');
  const [abrv, setAbrv] = useState('');
  const [makeId, setMakeId] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    vehicleMakeStore.fetchMakes();

    fetch(`/api/VehicleModels/${id}`)
      .then(res => {
        if (!res.ok) throw new Error('Network response was not ok');
        return res.json();
      })
      .then(dto => {
        setName(dto.name);
        setAbrv(dto.abrv);
        setMakeId(dto.vehicleMakeId.toString());
      })
      .catch(() => {
        alert('Failed to load vehicle model.');
        navigate('/models');
      });
  }, [id, navigate]);

  const handleSubmit = async e => {
    e.preventDefault();
    setSaving(true);
    try {
      await vehicleModelStore.updateModel(id, {
        name: name.trim(),
        abrv: abrv.trim(),
        vehicleMakeId: Number(makeId),
      });
      navigate('/models');
    } catch (err) {
      alert('Save failed.');
    } finally {
      setSaving(false);
    }
  };

  return (
    <div>
      <h2>Edit Vehicle Model</h2>
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
          {saving ? 'Saving…' : 'Update'}
        </button>
        <button type="button" onClick={() => navigate('/models')} style={{ marginLeft: '0.5rem' }}>
          Cancel
        </button>
      </form>
    </div>
  );
};
