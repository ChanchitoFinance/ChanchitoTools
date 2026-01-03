import React from "react";
import { useNavigate } from "react-router-dom";
import { Checkout } from "../components/Checkout";
import { useBookStore } from "../context/BookStoreContext";

export const CheckoutPage = () => {
  const navigate = useNavigate();
  const { cart, getTotalPrice, clearCart, addPurchasedBooks } = useBookStore();

  const handleBack = () => {
    navigate("/");
  };

  const handlePaymentComplete = (paymentInfo) => {
    console.log("Payment completed:", paymentInfo);

    const newPurchasedIds = paymentInfo.books.map((book) => book.id);
    addPurchasedBooks(newPurchasedIds);

    setTimeout(() => {
      clearCart();
      navigate("/");
    }, 3000);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-white to-purple-50 p-4 md:p-8">
      <div className="max-w-7xl mx-auto">
        <Checkout
          cart={cart}
          totalPrice={getTotalPrice()}
          onBack={handleBack}
          onPaymentComplete={handlePaymentComplete}
        />
      </div>
    </div>
  );
};
