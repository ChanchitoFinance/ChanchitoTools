import { useState } from "react";

export const useCart = () => {
  const [cart, setCart] = useState([]);

  const addToCart = (book) => {
    const alreadyInCart = cart.find((item) => item.id === book.id);

    if (alreadyInCart) {
      return { success: false, message: "This book is already in your cart!" };
    }

    setCart([...cart, book]);
    return { success: true, message: "Book added to cart" };
  };

  const removeFromCart = (index) => {
    setCart(cart.filter((_, idx) => idx !== index));
  };

  const clearCart = () => {
    setCart([]);
  };

  const getTotalPrice = () => {
    return cart.reduce((sum, item) => sum + item.price, 0);
  };

  return {
    cart,
    addToCart,
    removeFromCart,
    clearCart,
    getTotalPrice,
  };
};
