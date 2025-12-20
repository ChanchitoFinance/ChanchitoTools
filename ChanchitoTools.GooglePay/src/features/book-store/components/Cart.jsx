import React from "react";

export const Cart = ({ items, totalPrice, onCheckout, onRemoveItem }) => {
  if (items.length === 0) {
    return (
      <div className="bg-white rounded-lg shadow-md p-6">
        <h3 className="text-xl font-bold mb-4">Shopping Cart</h3>
        <p className="text-gray-500 text-center py-4">Your cart is empty</p>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow-md p-6">
      <h3 className="text-xl font-bold mb-4">Shopping Cart ({items.length})</h3>
      <div className="space-y-3 mb-4 max-h-96 overflow-y-auto">
        {items.map((item, idx) => (
          <div key={idx} className="flex gap-3 border-b pb-3">
            <img
              src={item.image}
              alt={item.title}
              className="w-16 h-20 object-cover rounded"
            />
            <div className="flex-1 min-w-0">
              <p className="font-semibold text-sm line-clamp-1">{item.title}</p>
              <p className="text-xs text-gray-600">{item.author}</p>
              <p className="font-bold text-green-600 mt-1">
                ${item.price.toFixed(2)}
              </p>
            </div>
            <button
              onClick={() => onRemoveItem(idx)}
              className="text-red-500 hover:text-red-700 self-start"
            >
              ✕
            </button>
          </div>
        ))}
      </div>
      <div className="border-t pt-3 mb-4">
        <div className="flex justify-between items-center text-lg font-bold">
          <span>Total:</span>
          <span className="text-green-600">${totalPrice.toFixed(2)}</span>
        </div>
      </div>
      <button
        onClick={onCheckout}
        className="w-full bg-green-600 hover:bg-green-700 text-white py-3 rounded-lg font-semibold transition"
      >
        Proceed to Checkout
      </button>
    </div>
  );
};
