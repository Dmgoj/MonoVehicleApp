import React, { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import FormField from '../components/FormField';
import ownerStore from '../stores/VehicleOwnerStore';
import makeStore from '../stores/VehicleMakeStore';
import modelStore from '../stores/VehicleModelStore';
import { ROUTES } from '../routes';

export const VehicleOwnerEdit = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [firstName, setFirst] = useState('');
  const [lastName, setLast] = useState('');
  const [dob, setDob] = useState('');
  const [makeId, setMake] = useState('');
  const [modelId, setModel] = useState('');
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    makeStore.fetchMakes();
    modelStore.fetchModels();
    fetch(`/api/VehicleOwners/${id}`)
      .then(r => r.json())
      .then(o => {
        setFirst(o.firstName);
        setLast(o.lastName);
        setDob(o.dob.slice(0, 10)); // yyyy-MM-dd
        setMake(o.vehicleMakeId.toString());
        setModel(o.vehicleModelId.toString());
      })
      .catch(() => {
        alert('Failed to load owner');
        navigate(ROUTES.OWNERS);
      });
  }, [id, navigate]);

  const handleSubmit = async e => {
    e.preventDefault();
    setSaving(true);
    try {
      await ownerStore.updateOwner(id, {
        firstName,
        lastName,
        dob: new Date(dob),
        vehicleMakeId: Number(makeId),
        vehicleModelId: Number(modelId),
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
      <h2>Edit Vehicle Owner</h2>
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

        <FormField label="Make">
          <select value={makeId} onChange={e => setMake(e.target.value)} required>
            <option value="">— select make —</option>
            {makeStore.makes.map(m => (
              <option key={m.id} value={m.id}>
                {m.name}
              </option>
            ))}
          </select>
        </FormField>

        <FormField label="Model">
          <select value={modelId} onChange={e => setModel(e.target.value)} required>
            <option value="">— select model —</option>
            {modelStore.models.map(m => (
              <option key={m.id} value={m.id}>
                {m.name}
              </option>
            ))}
          </select>
        </FormField>

        <button type="submit" disabled={saving}>
          {saving ? 'Saving…' : 'Update'}
        </button>
        <button type="button" onClick={() => navigate(ROUTES.OWNERS)} style={{ marginLeft: '0.5rem' }}>
          Cancel
        </button>
      </form>
    </div>
  );
};
