// src/layouts/MainLayout.jsx
import React from 'react';
import { NavLink, Outlet } from 'react-router-dom';
import { ROUTES } from '../routes';

export const MainLayout = () => (
  <div>
    <header
      style={{
        padding: '1rem',
        borderBottom: '1px solid #ccc',
      }}
    >
      <h1 style={{ margin: 0 }}>Vehicle App</h1>
      <nav style={{ marginTop: '0.5rem', display: 'flex', gap: '1rem' }}>
        <NavLink
          to={ROUTES.MAKES}
          style={({ isActive }) => ({
            textDecoration: 'none',
            fontWeight: isActive ? 'bold' : 'normal',
          })}
        >
          Makes
        </NavLink>
        <NavLink
          to={ROUTES.MODELS}
          style={({ isActive }) => ({
            textDecoration: 'none',
            fontWeight: isActive ? 'bold' : 'normal',
          })}
        >
          Models
        </NavLink>
        <NavLink
          to={ROUTES.OWNERS}
          style={({ isActive }) => ({
            textDecoration: 'none',
            fontWeight: isActive ? 'bold' : 'normal',
          })}
        >
          Owners
        </NavLink>
      </nav>
    </header>

    <main style={{ padding: '1rem' }}>
      <Outlet />
    </main>
  </div>
);
