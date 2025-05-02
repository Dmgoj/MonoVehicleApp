import React from 'react';

const Table = ({
  columns,
  data,
  onSort,
  sortBy,
  sortDesc,
  onRowClick,
  selectedRowId,
  onRowDelete
}) => (
  <table>
    <thead>
      <tr>
        {columns.map(col => (
          <th
            key={col.key}
            onClick={() => onSort(col.key)}
            style={{ cursor: 'pointer' }}
          >
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
            background: selectedRowId === item.id ? '#eef' : undefined
          }}
        >
          {columns.map(col => (
            <td key={col.key}>{item[col.key]}</td>
          ))}
           <td>
        {onRowDelete && (
          <span
            onClick={e => { 
              e.stopPropagation(); 
              onRowDelete(item.id); 
            }}
            style={{ color: 'red', cursor: 'pointer' }}
            title="Delete"
          >
            remove
          </span>
        )}
      </td>
        </tr>
      ))}
    </tbody>
  </table>
);

export default Table;
