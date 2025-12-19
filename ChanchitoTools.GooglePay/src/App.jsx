import React from "react";
import { Outlet } from "react-router-dom";
import { BookStoreProvider } from "./features/book-store";

const App = () => {
  return (
    <BookStoreProvider>
      <Outlet />
    </BookStoreProvider>
  );
};

export default App;
