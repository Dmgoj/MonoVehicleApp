import React from 'react';

const MainLayout = ({ children }) => (
  <div>
    <header><h1>Vehicle App</h1></header>
    <main>{children}</main>
  </div>
);

export default MainLayout;