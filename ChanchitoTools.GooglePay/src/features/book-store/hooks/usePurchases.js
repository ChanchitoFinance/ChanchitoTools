import { useState } from "react";

export const usePurchases = () => {
  const [purchasedBooks, setPurchasedBooks] = useState([]);

  const addPurchasedBooks = (bookIds) => {
    setPurchasedBooks([...purchasedBooks, ...bookIds]);
  };

  const isPurchased = (bookId) => {
    return purchasedBooks.includes(bookId);
  };

  const getPurchasedCount = () => {
    return purchasedBooks.length;
  };

  return {
    purchasedBooks,
    addPurchasedBooks,
    isPurchased,
    getPurchasedCount,
  };
};
