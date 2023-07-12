import React from 'react';
import logo from './logo.svg';
import './App.css';
import { createBrowserRouter,RouterProvider, BrowserRouter, Routes, Route } from "react-router-dom"
import Auth from "./pages/Auth"
import Home from "./pages/home"
import Root from "./routes/root";


const router = createBrowserRouter([
  {
    path: "/",
    element: <Root />,
  },
  {
    path: "/Auth",
    element: <Auth />,
  }
]);

function App() {
  return (
    <RouterProvider router={router} />
  );
}

export default App;
