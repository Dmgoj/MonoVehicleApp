import React from 'react';
import { Routes, Route } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import VehicleMakeList from './pages/VehicleMakeList';

const App = () => (
  <MainLayout>
    <Routes>
      <Route path="/vehicle-makes" element={<VehicleMakeList />} />
    </Routes>
  </MainLayout>
);

export default App;