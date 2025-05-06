import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { useNavigate } from 'react-router-dom';
import FormField from '../components/FormField';
import ownerStore from '../stores/VehicleOwnerStore';
import { ROUTES } from '../routes';

export const VehicleOwnerCreate = observer(() => {
  const navigate = useNavigate();

  useEffect(() => {
    ownerStore.currentOwner = { id: 0, firstName: '', lastName: '', dob: '' };
  }, []);

  const { currentOwner } = ownerStore;

  const handleSubmit = async e => {
    e.preventDefault();
    await ownerStore.createOwner({
      firstName: currentOwner.firstName,
      lastName: currentOwner.lastName,
      dob: new Date(currentOwner.dob),
    });
    navigate(ROUTES.OWNERS);
  };

  return (
    <div>
      <h2>Create Vehicle Owner</h2>
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
        <button type="submit">Create</button>
        <button type="button" onClick={() => navigate(ROUTES.OWNERS)}>
          Cancel
        </button>
      </form>
    </div>
  );
});
