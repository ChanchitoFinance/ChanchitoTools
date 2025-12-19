import React, { useState, useEffect } from "react";

const config = {
  merchantId:
    import.meta.env.VITE_GOOGLE_PAY_MERCHANT_ID || "12345678901234567890",
  merchantName: import.meta.env.VITE_MERCHANT_NAME || "Book Store Demo",
  gatewayMerchantId:
    import.meta.env.VITE_GATEWAY_MERCHANT_ID || "exampleGatewayMerchantId",
};

const books = [
  {
    id: 1,
    title: "Los osigatitos",
    author: "Max Brayan",
    price: 1.0,
    image:
      "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQFWStFNrzSl0XidGN2pp8vTxwKaG5qWM3XqQ&s",
    pdf: "https://doem.org.br/ba/modelo/arquivos/pdfviewer/0b517cdc5f9850e3782051c82e7f3234?name=lorem-ipsum.pdf",
    description: "A peach and goma book for lovers",
  },
];

const BookCard = ({ book, onAddToCart, isPurchased }) => {
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

const Cart = ({ items, onCheckout, onRemoveItem }) => {
  const total = items.reduce((sum, item) => sum + item.price, 0);

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
          <span className="text-green-600">${total.toFixed(2)}</span>
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

const GooglePayButton = ({ amount, onSuccess, onError }) => {
  const [isReady, setIsReady] = useState(false);
  const [paymentsClient, setPaymentsClient] = useState(null);

  useEffect(() => {
    const script = document.createElement("script");
    script.src = "https://pay.google.com/gp/p/js/pay.js";
    script.async = true;
    script.onload = () => initGooglePay();
    document.body.appendChild(script);

    return () => {
      const existingScript = document.querySelector(
        'script[src="https://pay.google.com/gp/p/js/pay.js"]'
      );
      if (existingScript) {
        document.body.removeChild(existingScript);
      }
    };
  }, []);

  const initGooglePay = () => {
    if (window.google && window.google.payments && window.google.payments.api) {
      const client = new window.google.payments.api.PaymentsClient({
        environment: "TEST",
      });

      setPaymentsClient(client);

      const isReadyToPayRequest = {
        apiVersion: 2,
        apiVersionMinor: 0,
        allowedPaymentMethods: [
          {
            type: "CARD",
            parameters: {
              allowedAuthMethods: ["PAN_ONLY", "CRYPTOGRAM_3DS"],
              allowedCardNetworks: ["MASTERCARD", "VISA", "AMEX", "DISCOVER"],
            },
          },
        ],
      };

      client
        .isReadyToPay(isReadyToPayRequest)
        .then((response) => {
          if (response.result) {
            setIsReady(true);
            createButton(client);
          } else {
            onError("Google Pay is not available on this device");
          }
        })
        .catch((err) => {
          console.error("Error checking Google Pay availability:", err);
          onError("Google Pay is not available");
        });
    }
  };

  const createButton = (client) => {
    const button = client.createButton({
      onClick: () => processPayment(client),
      buttonColor: "black",
      buttonType: "pay",
      buttonSizeMode: "fill",
    });

    const container = document.getElementById("google-pay-button-container");
    if (container) {
      container.innerHTML = "";
      container.appendChild(button);
    }
  };

  const processPayment = (client) => {
    const paymentDataRequest = {
      apiVersion: 2,
      apiVersionMinor: 0,
      allowedPaymentMethods: [
        {
          type: "CARD",
          parameters: {
            allowedAuthMethods: ["PAN_ONLY", "CRYPTOGRAM_3DS"],
            allowedCardNetworks: ["MASTERCARD", "VISA", "AMEX", "DISCOVER"],
          },
          tokenizationSpecification: {
            type: "PAYMENT_GATEWAY",
            parameters: {
              gateway: "example",
              gatewayMerchantId: config.gatewayMerchantId,
            },
          },
        },
      ],
      merchantInfo: {
        merchantId: config.merchantId,
        merchantName: config.merchantName,
      },
      transactionInfo: {
        totalPriceStatus: "FINAL",
        totalPrice: amount.toFixed(2),
        currencyCode: "USD",
        countryCode: "US",
      },
    };

    client
      .loadPaymentData(paymentDataRequest)
      .then((paymentData) => {
        console.log("Payment data received:", paymentData);
        onSuccess(paymentData);
      })
      .catch((err) => {
        console.error("Payment error:", err);
        if (err.statusCode === "CANCELED") {
          onError("Payment cancelled by user");
        } else {
          onError("Payment failed: " + (err.statusMessage || "Unknown error"));
        }
      });
  };

  return (
    <div className="w-full">
      <div
        id="google-pay-button-container"
        className="w-full min-h-[48px]"
      ></div>
      {!isReady && (
        <div className="text-center text-gray-500 py-4">
          <div className="inline-block animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600 mb-2"></div>
          <p>Loading Google Pay...</p>
        </div>
      )}
    </div>
  );
};

const Checkout = ({ cart, onBack, onPaymentComplete }) => {
  const [status, setStatus] = useState("");
  const [error, setError] = useState("");

  const total = cart.reduce((sum, item) => sum + item.price, 0);

  const handlePaymentSuccess = (paymentData) => {
    setStatus("processing");
    setError("");

    setTimeout(() => {
      setStatus("success");
      onPaymentComplete({
        paymentData,
        orderId: "ORD-" + Date.now(),
        amount: total,
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
          <span className="text-green-600">${total.toFixed(2)}</span>
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
            amount={total}
            onSuccess={handlePaymentSuccess}
            onError={handlePaymentError}
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

const App = () => {
  const [cart, setCart] = useState([]);
  const [purchasedBooks, setPurchasedBooks] = useState([]);
  const [view, setView] = useState("shop");

  const handleAddToCart = (book) => {
    const alreadyInCart = cart.find((item) => item.id === book.id);
    const alreadyPurchased = purchasedBooks.includes(book.id);

    if (alreadyPurchased) {
      alert("You already own this book! You can download it from the library.");
      return;
    }

    if (alreadyInCart) {
      alert("This book is already in your cart!");
      return;
    }

    setCart([...cart, book]);
  };

  const handleRemoveItem = (index) => {
    setCart(cart.filter((_, idx) => idx !== index));
  };

  const handleCheckout = () => {
    setView("checkout");
  };

  const handlePaymentComplete = (paymentInfo) => {
    console.log("Payment completed:", paymentInfo);

    const newPurchasedIds = paymentInfo.books.map((book) => book.id);
    setPurchasedBooks([...purchasedBooks, ...newPurchasedIds]);

    setTimeout(() => {
      setCart([]);
      setView("shop");
    }, 3000);
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-50 via-white to-purple-50 p-4 md:p-8">
      <div className="max-w-7xl mx-auto">
        <header className="text-center mb-8">
          <h1 className="text-4xl md:text-5xl font-bold text-gray-800 mb-2">
            📚 {config.merchantName}
          </h1>
          <p className="text-gray-600">Google Pay Integration Testing</p>
          <p className="text-sm text-yellow-700 mt-2">
            ⚠️ Internal Testing Only - $1 per book for payment gateway
            integration testing
          </p>
        </header>

        {view === "shop" && (
          <div className="grid grid-cols-1 lg:grid-cols-4 gap-6">
            <div className="lg:col-span-3">
              <h2 className="text-2xl font-bold mb-4">Available Books</h2>
              <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-4">
                {books.map((book) => (
                  <BookCard
                    key={book.id}
                    book={book}
                    onAddToCart={handleAddToCart}
                    isPurchased={purchasedBooks.includes(book.id)}
                  />
                ))}
              </div>
            </div>

            <div className="lg:col-span-1">
              <div className="sticky top-4">
                <Cart
                  items={cart}
                  onCheckout={handleCheckout}
                  onRemoveItem={handleRemoveItem}
                />

                {purchasedBooks.length > 0 && (
                  <div className="bg-green-50 border border-green-200 rounded-lg p-4 mt-4">
                    <h4 className="font-semibold text-green-800 mb-2">
                      ✓ Your Library ({purchasedBooks.length})
                    </h4>
                    <p className="text-sm text-green-700">
                      Books marked with "Download PDF" are yours!
                    </p>
                  </div>
                )}
              </div>
            </div>
          </div>
        )}

        {view === "checkout" && (
          <Checkout
            cart={cart}
            onBack={() => setView("shop")}
            onPaymentComplete={handlePaymentComplete}
          />
        )}
      </div>
    </div>
  );
};

export default App;
