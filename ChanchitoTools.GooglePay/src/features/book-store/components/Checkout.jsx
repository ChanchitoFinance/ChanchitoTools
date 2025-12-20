import React, { useState } from "react";
import { GooglePayButton } from "../../google-pay";

export const Checkout = ({ cart, totalPrice, onBack, onPaymentComplete }) => {
  const [status, setStatus] = useState("");
  const [error, setError] = useState("");

  const handlePaymentSuccess = (paymentData) => {
    setStatus("processing");
    setError("");

    setTimeout(() => {
      setStatus("success");
      onPaymentComplete({
        paymentData,
        orderId: "ORD-" + Date.now(),
        amount: totalPrice,
        books: cart,
      });
    }, 1500);
  };

  const handlePaymentError = (errorMsg) => {
    setError(errorMsg);
    setStatus("");
  };

  if (status === "success") {
    return (
      <div className="bg-white rounded-lg shadow-md p-8 max-w-lg mx-auto text-center">
        <div className="text-6xl mb-4">✅</div>
        <h2 className="text-2xl font-bold mb-2 text-green-600">
          Payment Successful!
        </h2>
        <p className="text-gray-600 mb-6">
          Your books are now available for download
        </p>

        <div className="bg-gray-50 rounded-lg p-4 mb-6">
          <h3 className="font-semibold mb-3">Purchased Books:</h3>
          {cart.map((book, idx) => (
            <div key={idx} className="text-sm text-gray-700 mb-1">
              📚 {book.title}
            </div>
          ))}
        </div>

        <button
          onClick={onBack}
          className="bg-blue-600 hover:bg-blue-700 text-white px-6 py-3 rounded-lg transition font-semibold"
        >
          Continue Shopping
        </button>
      </div>
    );
  }

  return (
    <div className="bg-white rounded-lg shadow-md p-6 max-w-lg mx-auto">
      <div className="flex items-center justify-between mb-6">
        <h3 className="text-2xl font-bold">Checkout</h3>
        <button
          onClick={onBack}
          className="text-gray-500 hover:text-gray-700 flex items-center gap-1"
        >
          ← Back
        </button>
      </div>

      <div className="mb-6">
        <h4 className="font-semibold mb-3">Order Summary</h4>
        <div className="space-y-2">
          {cart.map((item, idx) => (
            <div
              key={idx}
              className="flex justify-between text-sm border-b pb-2"
            >
              <span className="flex-1">{item.title}</span>
              <span className="font-semibold">${item.price.toFixed(2)}</span>
            </div>
          ))}
        </div>
        <div className="border-t mt-3 pt-3 flex justify-between font-bold text-lg">
          <span>Total:</span>
          <span className="text-green-600">${totalPrice.toFixed(2)}</span>
        </div>
      </div>

      <div className="bg-yellow-50 border border-yellow-200 text-yellow-800 px-4 py-3 rounded mb-4 text-sm">
        <strong>⚠️ Internal Testing Only:</strong> This is a $1 test payment for
        Google Pay integration testing. Not a real commercial website.
      </div>

      {error && (
        <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded mb-4">
          <strong>Error:</strong> {error}
        </div>
      )}

      {status === "processing" && (
        <div className="text-center py-8">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Processing payment...</p>
        </div>
      )}

      {status !== "processing" && (
        <div className="mb-4">
          <GooglePayButton
            amount={totalPrice}
            onSuccess={handlePaymentSuccess}
            onError={handlePaymentError}
            className="w-full"
          />
        </div>
      )}

      <div className="bg-blue-50 border border-blue-200 text-blue-700 px-4 py-3 rounded text-sm">
        <strong>Test Mode:</strong> Using Google Pay TEST environment. Configure
        with your real Merchant ID in production.
      </div>
    </div>
  );
};
