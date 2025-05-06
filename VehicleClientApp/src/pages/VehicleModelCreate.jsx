// src/pages/VehicleModelCreate.jsx
import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useNavigate, useParams } from 'react-router-dom';
import FormField from '../components/FormField';
import vehicleModelStore from '../stores/VehicleModelStore';
import vehicleMakeStore from '../stores/VehicleMakeStore';
import { ROUTES } from '../routes';

export const VehicleModelCreate = observer(() => {
  const { id } = useParams();
  const navigate = useNavigate();
  const isEdit = Boolean(id);

  useEffect(() => {
    vehicleMakeStore.fetchMakes();
    if (isEdit) {
      vehicleModelStore.loadModelForEdit(Number(id));
    } else {
      vehicleModelStore.initNew();
    }
  }, [id]);

  const { currentModel, loading } = vehicleModelStore;

  const handleSubmit = async e => {
    e.preventDefault();
    await vehicleModelStore.saveEdit();
    navigate(ROUTES.MODELS);
  };

  if (loading) return <p>Loading…</p>;

  return (
    <div>
      <h2>{isEdit ? 'Edit' : 'Create'} Vehicle Model</h2>
      <form onSubmit={handleSubmit}>
        <FormField label="Name">
          <input value={currentModel.name} onChange={e => vehicleModelStore.setEditName(e.target.value)} required />
        </FormField>
        <FormField label="Abbreviation">
          <input value={currentModel.abrv} onChange={e => vehicleModelStore.setEditAbrv(e.target.value)} />
        </FormField>
        <FormField label="Make">
          <select
            value={currentModel.vehicleMakeId}
            onChange={e => vehicleModelStore.setEditMakeId(e.target.value)}
            required
          >
            <option value="">— select make —</option>
            {vehicleMakeStore.makes.map(m => (
              <option key={m.id} value={m.id}>
                {m.name}
              </option>
            ))}
          </select>
        </FormField>
        <button type="submit">{isEdit ? 'Update' : 'Create'}</button>
        <button type="button" onClick={() => navigate(ROUTES.MODELS)}>
          Cancel
        </button>
      </form>
    </div>
  );
});
