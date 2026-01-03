import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import { Shop } from "../features/book-store/pages/Shop";
import { CheckoutPage } from "../features/book-store/pages/CheckoutPage";

export const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        index: true,
        element: <Shop />,
      },
      {
        path: "checkout",
        element: <CheckoutPage />,
      },
    ],
  },
]);
