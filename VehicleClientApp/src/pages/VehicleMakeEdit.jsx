// src/pages/VehicleMakeEdit.jsx
import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useNavigate, useParams } from 'react-router-dom';
import FormField from '../components/FormField';
import vehicleMakeStore from '../stores/VehicleMakeStore';
import { ROUTES } from '../routes';

export const VehicleMakeEdit = observer(() => {
  const { id } = useParams();
  const navigate = useNavigate();
  const numId = Number(id);

  useEffect(() => {
    vehicleMakeStore.loadMakeForEdit(numId);
  }, [numId]);

  const { currentMake, loading } = vehicleMakeStore;

  const handleSubmit = async e => {
    e.preventDefault();
    await vehicleMakeStore.saveEdit();
    navigate(ROUTES.MAKES);
  };

  if (loading) return <p>Loading…</p>;

  return (
    <div>
      <h2>Edit Vehicle Make</h2>
      <form onSubmit={handleSubmit}>
        <FormField label="Name">
          <input value={currentMake.name} onChange={e => vehicleMakeStore.setEditName(e.target.value)} required />
        </FormField>
        <FormField label="Abbreviation">
          <input value={currentMake.abrv} onChange={e => vehicleMakeStore.setEditAbrv(e.target.value)} />
        </FormField>
        <button type="submit">Update</button>
        <button type="button" onClick={() => navigate(ROUTES.MAKES)}>
          Cancel
        </button>
      </form>
    </div>
  );
});
