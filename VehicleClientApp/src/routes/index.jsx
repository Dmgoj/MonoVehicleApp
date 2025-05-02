import { createBrowserRouter } from 'react-router-dom';
import { MainLayout } from '../layouts/MainLayout';
import { VehicleMakeCreate } from '../pages/VehicleMakeCreate';
import { VehicleMakeEdit } from '../pages/VehicleMakeEdit';
import { VehicleMakeList } from '../pages/VehicleMakeList';
import { VehicleModelCreate } from '../pages/VehicleModelCreate';
import { VehicleModelEdit } from '../pages/VehicleModelEdit';
import { VehicleModelList } from '../pages/VehicleModelList';
import { VehicleOwnerList } from '../pages/VehicleOwnerList';
import { VehicleOwnerCreate } from '../pages/VehicleOwnerCreate';
import { VehicleOwnerEdit } from '../pages/VehicleOwnerEdit';

export const ROUTES = {
  HOME: '/',
  CREATE: '/create',
  EDIT: 'edit/:id',

  MODELS: '/models',
  MODEL_CREATE: '/models/create',
  MODEL_EDIT: '/models/edit/:id',

  OWNERS: '/owners',
  OWNER_CREATE: '/owners/create',
  OWNER_EDIT: '/owners/edit/:id',
};

export const router = createBrowserRouter([
  {
    path: ROUTES.HOME,
    element: <MainLayout />,
    children: [
      {
        index: true,
        element: <VehicleMakeList />,
      },
      {
        path: ROUTES.CREATE,
        element: <VehicleMakeCreate />,
      },
      {
        path: ROUTES.EDIT,
        element: <VehicleMakeEdit />,
      },
      {
        path: ROUTES.MODELS,
        element: <VehicleModelList />,
      },
      {
        path: ROUTES.MODEL_CREATE,
        element: <VehicleModelCreate />,
      },
      {
        path: ROUTES.MODEL_EDIT,
        element: <VehicleModelEdit />,
      },
      {
        path: ROUTES.OWNERS,
        element: <VehicleOwnerList />,
      },
      {
        path: ROUTES.OWNER_CREATE,
        element: <VehicleOwnerCreate />,
      },
      {
        path: ROUTES.OWNER_EDIT,
        element: <VehicleOwnerEdit />,
      },
    ],
  },
]);
