// src/pages/VehicleModelList.jsx
import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Link, useNavigate } from 'react-router-dom';
import vehicleModelStore from '../stores/VehicleModelStore';
import vehicleMakeStore from '../stores/VehicleMakeStore';
import Table from '../components/Table';
import { ROUTES } from '../routes';

export const VehicleModelList = observer(() => {
  const modelStore = vehicleModelStore;
  const makeStore = vehicleMakeStore;
  const navigate = useNavigate();

  // on mount: load makes (for dropdown) then models (with any makeFilter)
  useEffect(() => {
    (async () => {
      await makeStore.fetchMakes();
      await modelStore.fetchModels();
    })();
  }, []);

  const totalPages = modelStore.totalPages;
  const hasPrev = modelStore.pageNumber > 1;
  const hasNext = modelStore.pageNumber < totalPages;

  const handleEdit = id => navigate(`edit/${id}`);
  const handleDelete = async id => {
    const rec = modelStore.models.find(m => m.id === id);
    const label = rec ? rec.name : 'this model';
    if (!window.confirm(`Delete "${label}"?`)) return;

    await modelStore.deleteModel(id);
    // if last item on page removed, go back a page
    if (modelStore.models.length === 0 && modelStore.pageNumber > 1) {
      modelStore.setPage(modelStore.pageNumber - 1);
    }
  };

  const columns = [
    { key: 'name', label: 'Name' },
    { key: 'abrv', label: 'Abbreviation' },
    { key: 'vehicleMakeName', label: 'Make' },
  ];

  return (
    <div>
      <h2>Vehicle Models</h2>

      <div style={{ display: 'flex', gap: '1rem', marginBottom: '1rem', alignItems: 'center' }}>
        {/* page size */}
        <label>
          Results per page:
          <select
            value={modelStore.pageSize}
            onChange={e => {
              modelStore.setPageSize(Number(e.target.value));
              modelStore.setPage(1);
            }}
            style={{ marginLeft: '0.5rem' }}
          >
            {[5, 10, 20, 50].map(n => (
              <option key={n} value={n}>
                {n}
              </option>
            ))}
          </select>
        </label>

        <label>
          Filter by make:
          <select
            value={modelStore.makeFilter}
            onChange={e => {
              modelStore.setMakeFilter(e.target.value);
              modelStore.setPage(1);
            }}
            style={{ marginLeft: '0.5rem' }}
          >
            <option value="">All</option>
            {makeStore.makes.map(m => (
              <option key={m.id} value={m.id}>
                {m.name}
              </option>
            ))}
          </select>
        </label>

        <Link to={ROUTES.MODEL_CREATE}>Create</Link>
      </div>

      {modelStore.loading ? (
        <p>Loading…</p>
      ) : modelStore.totalCount === 0 ? (
        <p>No results found.</p>
      ) : (
        <>
          <Table
            columns={columns}
            data={modelStore.models.map(m => ({
              ...m,
              vehicleMakeName: makeStore.makes.find(x => x.id === m.vehicleMakeId)?.name ?? '—',
            }))}
            onSort={modelStore.setSort.bind(modelStore)}
            sortBy={modelStore.sortBy}
            sortDesc={modelStore.sortDescending}
            onRowEdit={handleEdit}
            onRowDelete={handleDelete}
          />

          <div style={{ marginTop: '1rem' }}>
            <button disabled={!hasPrev} onClick={() => modelStore.setPage(modelStore.pageNumber - 1)}>
              Prev
            </button>
            <span style={{ margin: '0 0.5rem' }}>
              Page {modelStore.pageNumber} of {totalPages}
            </span>
            <button disabled={!hasNext} onClick={() => modelStore.setPage(modelStore.pageNumber + 1)}>
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
});
