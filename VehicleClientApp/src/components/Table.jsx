import React from 'react';

const Table = ({
  columns = [],
  data = [],
  onSort,
  sortBy,
  sortDesc,
  onRowClick,
  selectedRowId,
  onRowEdit,
  onRowDelete,
}) => (
  <table>
    <thead>
      <tr>
        {columns.map(col => (
          <th key={col.key} onClick={() => onSort(col.key)} style={{ cursor: 'pointer' }}>
            {col.label}
            {sortBy === col.key ? (sortDesc ? ' 🔽' : ' 🔼') : ''}
          </th>
        ))}
      </tr>
    </thead>
    <tbody>
      {data.map(item => (
        <tr
          key={item.id}
          onClick={() => onRowClick && onRowClick(item)}
          style={{
            cursor: onRowClick ? 'pointer' : undefined,
            background: selectedRowId === item.id ? '#eef' : undefined,
          }}
        >
          {columns.map(col => (
            <td key={col.key}>{item[col.key]}</td>
          ))}
          <td>
            <button
              onClick={e => {
                e.stopPropagation();
                onRowEdit(item.id);
              }}
              style={{ color: 'black', border: '1px solid black', background: 'transparent' }}
            >
              Edit
            </button>
            {onRowDelete && (
              <button
                onClick={e => {
                  e.stopPropagation();
                  onRowDelete(item.id);
                }}
                style={{ marginLeft: 8, color: 'red', border: 'none', background: 'transparent' }}
              >
                Remove
              </button>
            )}
          </td>
        </tr>
      ))}
    </tbody>
  </table>
);

export default Table;
