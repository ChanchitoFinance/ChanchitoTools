import React from "react";

export const BookCard = ({ book, onAddToCart, isPurchased }) => {
  const handleDownload = () => {
    window.open(book.pdf, "_blank");
  };

  return (
    <div className="bg-white rounded-lg shadow-md overflow-hidden hover:shadow-xl transition">
      <img
        src={book.image}
        alt={book.title}
        className="w-full h-64 object-cover"
      />
      <div className="p-4">
        <h3 className="text-lg font-bold mb-1 line-clamp-2">{book.title}</h3>
        <p className="text-sm text-gray-600 mb-2">{book.author}</p>
        <p className="text-xs text-gray-500 mb-3 line-clamp-2">
          {book.description}
        </p>

        <div className="flex items-center justify-between">
          <span className="text-xl font-bold text-green-600">
            ${book.price.toFixed(2)}
          </span>
          {isPurchased ? (
            <button
              onClick={handleDownload}
              className="bg-green-600 hover:bg-green-700 text-white px-4 py-2 rounded-lg text-sm transition flex items-center gap-1"
            >
              📥 Download PDF
            </button>
          ) : (
            <button
              onClick={() => onAddToCart(book)}
              className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg text-sm transition"
            >
              Add to Cart
            </button>
          )}
        </div>
      </div>
    </div>
  );
};
