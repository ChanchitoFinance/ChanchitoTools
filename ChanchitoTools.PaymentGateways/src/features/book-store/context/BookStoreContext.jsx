import React, { createContext, useContext } from "react";
import { useCart } from "../hooks/useCart";
import { usePurchases } from "../hooks/usePurchases";

const BookStoreContext = createContext(null);

export const BookStoreProvider = ({ children }) => {
  const cartHook = useCart();
  const purchasesHook = usePurchases();

  const value = {
    ...cartHook,
    ...purchasesHook,
  };

  return (
    <BookStoreContext.Provider value={value}>
      {children}
    </BookStoreContext.Provider>
  );
};

export const useBookStore = () => {
  const context = useContext(BookStoreContext);
  if (!context) {
    throw new Error("useBookStore must be used within BookStoreProvider");
  }
  return context;
};
