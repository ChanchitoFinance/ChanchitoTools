import React from "react";
import { useNavigate } from "react-router-dom";
import { BookCard } from "../components/BookCard";
import { Cart } from "../components/Cart";
import { LibraryInfo } from "../components/LibraryInfo";
import { books } from "../data/books";
import { useBookStore } from "../context/BookStoreContext";

export const Shop = () => {
  const navigate = useNavigate();
  const {
    cart,
    addToCart,
    removeFromCart,
    getTotalPrice,
    isPurchased,
    getPurchasedCount,
  } = useBookStore();

  const merchantName =
    import.meta.env.VITE_GOOGLE_PAY_MERCHANT_NAME || "Book Store Demo";

  const handleAddToCart = (book) => {
    if (isPurchased(book.id)) {
      alert("You already own this book! You can download it from the library.");
      return;
    }

    const result = addToCart(book);
    if (!result.success) {
      alert(result.message);
    }
  };

  const handleCheckout = () => {
    navigate("/checkout");
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-white to-purple-50 p-4 md:p-8">
      <div className="max-w-7xl mx-auto">
        <header className="text-center mb-8">
          <h1 className="text-4xl md:text-5xl font-bold text-gray-800 mb-2">
            📚 {merchantName}
          </h1>
          <p className="text-gray-600">Google Pay Integration Testing</p>
          <p className="text-sm text-yellow-700 mt-2">
            ⚠️ Internal Testing Only - $1 per book for payment gateway
            integration testing
          </p>
        </header>

        <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
          <div className="lg:col-span-3">
            <h2 className="text-2xl font-bold mb-4">Available Books</h2>
            <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
              {books.map((book) => (
                <BookCard
                  key={book.id}
                  book={book}
                  onAddToCart={handleAddToCart}
                  isPurchased={isPurchased(book.id)}
                />
              ))}
            </div>
          </div>

          <div className="lg:col-span-1">
            <div className="sticky top-4">
              <Cart
                items={cart}
                totalPrice={getTotalPrice()}
                onCheckout={handleCheckout}
                onRemoveItem={removeFromCart}
              />
              <LibraryInfo count={getPurchasedCount()} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
