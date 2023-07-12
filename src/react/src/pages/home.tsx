import React from 'react';
import logo from './logo.svg';
import { createBrowserRouter,RouterProvider } from "react-router-dom"

import Root from "../routes/root";


const router = createBrowserRouter([
  {
    path: "/root",
    element: <Root />,
  },
]);

function App() {
  return (
    <RouterProvider router={router} />
  );
}

export default App;
