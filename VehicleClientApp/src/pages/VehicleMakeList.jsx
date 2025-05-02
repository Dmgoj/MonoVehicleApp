import React, { useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import vehicleMakeStore from '../stores/VehicleMakeStore';
import Table from '../components/Table';
import FormField from '../components/FormField';

const VehicleMakeList = observer(() => {
  const store = vehicleMakeStore;
  const [name, setName] = useState('');
  const [abrv, setAbrv] = useState('');
  const [saving, setSaving] = useState(false);
  const [editingId, setEditingId] = useState(null);

  useEffect(() => { store.fetchMakes(); }, []);

  const handleSubmit = async e => {
    e.preventDefault();
    setSaving(true);
    try {
      if (editingId) {
        await store.updateMake(editingId, { name, abrv });
      } else {
        await store.createMake({ name, abrv });
      }
      setName('');
      setAbrv('');
      setEditingId(null);
    } catch {
      alert('Save failed');
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async id => {
    const make = store.makes.find(m => m.id === id);
    const label = make ? make.name : 'this make';
  
    if (!window.confirm(`Delete "${label}"?`)) return;
  
    await store.deleteMake(id);
  
    if (editingId === id) {
      setEditingId(null);
      setName('');
      setAbrv('');
    }
    
    await store.fetchMakes();
  
    if (store.makes.length === 0 && store.pageNumber > 1) {
      store.setPage(store.pageNumber - 1);
    }
  };
  

  const columns = [
    { key: 'name', label: 'Name' },
    { key: 'abrv', label: 'Abbreviation' }
  ];
 // Calculate total pages
const totalPages = Math.ceil(store.totalCount / store.pageSize);

// Enable “Prev” when not on the first page
const hasPrev = store.pageNumber > 1;

// Enable “Next” only when current page is less than total pages
const hasNext = store.pageNumber < totalPages;

  return (
    <div>
      <h2>Vehicle Makes</h2>

      {/* Inline create form */}
      <form onSubmit={handleSubmit} style={{ marginBottom: '1rem' }}>
        <FormField label="Name"><input value={name} onChange={e => setName(e.target.value)} required /></FormField>
        <FormField label="Abbreviation"><input value={abrv} onChange={e => setAbrv(e.target.value)} /></FormField>
        <button type="submit" disabled={saving}>
        {saving
            ? 'Saving…'
            : editingId
            ? 'Update'
            : 'Create'
        }
        </button>
        {editingId && (
    <a
      href="#"
      onClick={e => {
        e.preventDefault();
        setEditingId(null);
        setName('');
        setAbrv('');
      }}
      style={{
        marginLeft: '0.5rem',
        color: 'blue',
        textDecoration: 'underline',
        cursor: 'pointer'
      }}
    >
      Cancel
    </a>
  )}
      </form>


      {/* Controls: Results per page & Filter */}
    <div style={{ display: 'flex', gap: '1rem', marginBottom: '1rem', alignItems: 'center' }}>
        <label>
            Results per page:
            <select
            value={store.pageSize}
            onChange={e => store.setPageSize(Number(e.target.value))}
            style={{ marginLeft: '0.5rem' }}
            >
            {[5, 10, 20, 50].map(n => (
                <option key={n} value={n}>{n}</option>
            ))}
            </select>
        </label>

  <input
    placeholder="Filter by name..."
    value={store.filter}
    onChange={e => store.setFilter(e.target.value)}
    style={{ flex: 1, padding: '0.5rem' }}
  />
</div>


      {/* Table or No results */}
      {store.loading ? (
        <p>Loading…</p>
      ) : store.totalCount === 0 ? (
        <p>No results found.</p>
      ) : (
        <>        
         <Table
  columns={columns}
  data={store.makes}
  onSort={store.setSort.bind(store)}
  sortBy={store.sortBy}
  sortDesc={store.sortDescending}
  onRowClick={item => {
    setEditingId(item.id);
    setName(item.name);
    setAbrv(item.abrv);
  }}
  onRowDelete={handleDelete}
  selectedRowId={editingId}
/>
          <div style={{ marginTop: '1rem' }}>
            <button disabled={!hasPrev} onClick={() => store.setPage(store.pageNumber - 1)}>Prev</button>
            <span style={{ margin: '0 0.5rem' }}>{store.pageNumber} of {totalPages}</span>
            <button disabled={!hasNext} onClick={() => store.setPage(store.pageNumber + 1)}>Next</button>
          </div>
        </>
      )}
    </div>
  );
});

export default VehicleMakeList;