import { Counter } from "./components/Counter";
import { FetchData } from "./components/FetchData";
import { Home } from "./components/Home";

const AppRoutes = [
  {
    index: true,
        element: <FetchData />
  },
  {
    path: '/counter',
    element: <Counter />
  },
  {
      path: '/home',
      element: <Home />
  }
];

export default AppRoutes;
