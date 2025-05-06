import React, { useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import FormField from '../components/FormField';
import ownerStore from '../stores/VehicleOwnerStore';
import { ROUTES } from '../routes';

export const VehicleOwnerEdit = observer(() => {
  const { id } = useParams();
  const navigate = useNavigate();
  const numId = Number(id);

  useEffect(() => {
    ownerStore.loadOwnerForEdit(numId);
  }, [numId]);

  const { currentOwner, loading } = ownerStore;

  const handleSubmit = async e => {
    e.preventDefault();
    await ownerStore.saveEdit();
    navigate(ROUTES.OWNERS);
  };

  if (loading) return <p>Loading…</p>;

  return (
    <div>
      <h2>Edit Vehicle Owner</h2>
      <form onSubmit={handleSubmit}>
        <FormField label="First Name">
          <input value={currentOwner.firstName} onChange={e => ownerStore.setEditFirstName(e.target.value)} required />
        </FormField>
        <FormField label="Last Name">
          <input value={currentOwner.lastName} onChange={e => ownerStore.setEditLastName(e.target.value)} required />
        </FormField>
        <FormField label="Date of Birth">
          <input type="date" value={currentOwner.dob} onChange={e => ownerStore.setEditDob(e.target.value)} required />
        </FormField>
        <button type="submit">Update</button>
        <button type="button" onClick={() => navigate(ROUTES.OWNERS)}>
          Cancel
        </button>
      </form>
    </div>
  );
});
