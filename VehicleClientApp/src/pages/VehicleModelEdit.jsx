import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useNavigate, useParams } from 'react-router-dom';
import FormField from '../components/FormField';
import vehicleModelStore from '../stores/VehicleModelStore';
import vehicleMakeStore from '../stores/VehicleMakeStore';
import { ROUTES } from '../routes';

export const VehicleModelEdit = observer(() => {
  const { id } = useParams();
  const navigate = useNavigate();
  const numId = Number(id);

  useEffect(() => {
    vehicleMakeStore.fetchMakes();
    vehicleModelStore.loadModelForEdit(numId);
  }, [numId]);

  const { currentModel, loading } = vehicleModelStore;

  const handleSubmit = async e => {
    e.preventDefault();
    await vehicleModelStore.saveEdit();
    navigate(ROUTES.MODELS);
  };

  if (loading) return <p>Loading…</p>;

  return (
    <div>
      <h2>Edit Vehicle Model</h2>
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
        <button type="submit">Update</button>
        <button type="button" onClick={() => navigate(ROUTES.MODELS)}>
          Cancel
        </button>
      </form>
    </div>
  );
});
