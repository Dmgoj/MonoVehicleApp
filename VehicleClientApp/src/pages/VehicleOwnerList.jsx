// src/pages/VehicleOwnerList.jsx
import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Link, useNavigate } from 'react-router-dom';
import ownerStore from '../stores/VehicleOwnerStore';
import makeStore from '../stores/VehicleMakeStore';
import modelStore from '../stores/VehicleModelStore';
import Table from '../components/Table';
import FormField from '../components/FormField';
import { ROUTES } from '../routes';

export const VehicleOwnerList = observer(() => {
  const navigate = useNavigate();

  useEffect(() => {
    if (makeStore.makes.length === 0) makeStore.fetchMakes();
    if (modelStore.models.length === 0) modelStore.fetchModels();
    if (ownerStore.owners.length === 0) ownerStore.fetchOwners();
  }, []);

  const {
    makeFilter,
    modelFilter,
    firstNameFilter,
    lastNameFilter,
    sortedOwners,
    totalCount,
    pageNumber,
    pageSize,
    sortBy,
    sortDescending,
    loading,
    totalPages,
  } = ownerStore;

  const hasPrev = pageNumber > 1;
  const hasNext = pageNumber < totalPages;

  const handleEdit = id => navigate(`edit/${id}`);
  const handleDelete = async id => {
    const o = sortedOwners.find(x => x.id === id);
    if (!window.confirm(`Delete "${o.firstName} ${o.lastName}"?`)) return;
    await ownerStore.deleteOwner(id);
  };

  const columns = [
    { key: 'firstName', label: 'First Name' },
    { key: 'lastName', label: 'Last Name' },
    { key: 'dob', label: 'Date of Birth' },
    { key: 'makeList', label: 'Make' },
    { key: 'modelList', label: 'Model' },
  ];

  const rows = sortedOwners.map(o => ({
    ...o,
    makeList: o.cars.length ? o.cars.map(c => c.make).join(', ') : '—',
    modelList: o.cars.length ? o.cars.map(c => c.model).join(', ') : '—',
  }));

  return (
    <div>
      <h2>Vehicle Owners</h2>
      <div style={{ display: 'flex', gap: '1rem', marginBottom: '1rem', alignItems: 'center' }}>
        <FormField label="Make">
          <select value={makeFilter} onChange={e => ownerStore.setMakeFilter(e.target.value)}>
            <option value="">All</option>
            {makeStore.makes.map(m => (
              <option key={m.id} value={m.id}>
                {m.name}
              </option>
            ))}
          </select>
        </FormField>
        <FormField label="Model">
          <select value={modelFilter} onChange={e => ownerStore.setModelFilter(e.target.value)}>
            <option value="">All</option>
            {modelStore.models.map(m => (
              <option key={m.id} value={m.id}>
                {m.name}
              </option>
            ))}
          </select>
        </FormField>
        <FormField label="First Name">
          <input value={firstNameFilter} onChange={e => ownerStore.setFirstNameFilter(e.target.value)} />
        </FormField>
        <FormField label="Last Name">
          <input value={lastNameFilter} onChange={e => ownerStore.setLastNameFilter(e.target.value)} />
        </FormField>
        <FormField label="Per page">
          <select value={pageSize} onChange={e => ownerStore.setPageSize(Number(e.target.value))}>
            {[5, 10, 20, 50].map(n => (
              <option key={n} value={n}>
                {n}
              </option>
            ))}
          </select>
        </FormField>
        <Link to={ROUTES.OWNER_CREATE}>Create</Link>
      </div>

      {loading ? (
        <p>Loading…</p>
      ) : totalCount === 0 ? (
        <p>No results found.</p>
      ) : (
        <>
          <Table
            columns={columns}
            data={rows}
            onSort={ownerStore.setSort.bind(ownerStore)}
            sortBy={sortBy}
            sortDesc={sortDescending}
            onRowEdit={handleEdit}
            onRowDelete={handleDelete}
          />
          <div style={{ marginTop: '1rem' }}>
            <button disabled={!hasPrev} onClick={() => ownerStore.setPage(pageNumber - 1)}>
              Prev
            </button>
            <span style={{ margin: '0 0.5rem' }}>
              Page {pageNumber} of {totalPages}
            </span>
            <button disabled={!hasNext} onClick={() => ownerStore.setPage(pageNumber + 1)}>
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
});
