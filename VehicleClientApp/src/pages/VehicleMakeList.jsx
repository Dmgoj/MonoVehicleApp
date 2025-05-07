import React, { useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import vehicleMakeStore from '../stores/VehicleMakeStore';
import Table from '../components/Table';
import { Link, useNavigate } from 'react-router-dom';

export const VehicleMakeList = observer(() => {
  const store = vehicleMakeStore;

  const totalPages = Math.ceil(store.totalCount / store.pageSize);
  const hasPrev = store.pageNumber > 1;
  const hasNext = store.pageNumber < totalPages;

  const navigate = useNavigate();

  const handleEdit = id => {
    console.log(id);
    navigate(`edit/${id}`);
  };
  const handleDelete = async id => {
    const make = store.makes.find(m => m.id === id);
    const label = make ? make.name : 'this make';

    if (!window.confirm(`Delete "${label}"?`)) return;

    await store.deleteMake(id);
    await store.fetchMakes();

    if (store.makes.length === 0 && store.pageNumber > 1) {
      store.setPage(store.pageNumber - 1);
    }
  };

  const columns = [
    { key: 'name', label: 'Name' },
    { key: 'abrv', label: 'Abbreviation' },
  ];

  useEffect(() => {
    if (store.makes.length === 0) {
      store.fetchMakes();
    }
  }, []);

  return (
    <div>
      <h2>Vehicle Makes</h2>

      <div style={{ display: 'flex', gap: '1rem', marginBottom: '1rem', alignItems: 'center' }}>
        <label>
          Results per page:
          <select
            value={store.pageSize}
            onChange={e => store.setPageSize(Number(e.target.value))}
            style={{ marginLeft: '0.5rem' }}
          >
            {[5, 10, 20, 50].map(n => (
              <option key={n} value={n}>
                {n}
              </option>
            ))}
          </select>
        </label>

        <input
          placeholder="Filter by name..."
          value={store.filter}
          onChange={e => store.setFilter(e.target.value)}
          style={{ flex: 1, padding: '0.5rem' }}
        />
        <Link to="create">Create</Link>
      </div>

      {store.loading ? (
        <p>Loading…</p>
      ) : store.totalCount === 0 ? (
        <p>No results found.</p>
      ) : (
        <>
          <Table
            columns={columns}
            data={store.sortedMakes}
            onSort={store.setClientSort.bind(store)}
            sortBy={store.sortBy}
            sortDesc={store.sortDescending}
            onRowDelete={handleDelete}
            onRowEdit={handleEdit}
          />

          <div style={{ marginTop: '1rem' }}>
            <button disabled={!hasPrev} onClick={() => store.setPage(store.pageNumber - 1)}>
              Prev
            </button>
            <span style={{ margin: '0 0.5rem' }}>
              Page {store.pageNumber} of {totalPages}
            </span>
            <button disabled={!hasNext} onClick={() => store.setPage(store.pageNumber + 1)}>
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
});
