import React from "react";
import { useGooglePay } from "../hooks/useGooglePay";

export const GooglePayButton = ({
  amount,
  onSuccess,
  onError,
  buttonColor = "black",
  buttonType = "pay",
  className = "",
  loadingText = "Loading Google Pay...",
}) => {
  const { isReady, processPayment, error } = useGooglePay({
    onSuccess,
    onError: (err) => {
      onError?.(err);
    },
  });

  const handleClick = () => {
    processPayment(amount);
  };

  return (
    <div className={className}>
      <div style={{ width: "100%", minHeight: "48px" }}>
        {isReady ? (
          <button
            onClick={handleClick}
            style={{
              width: "100%",
              height: "48px",
              backgroundColor: buttonColor === "black" ? "#000" : "#fff",
              color: buttonColor === "black" ? "#fff" : "#000",
              border: buttonColor === "white" ? "1px solid #ccc" : "none",
              borderRadius: "4px",
              cursor: "pointer",
              fontSize: "14px",
              fontWeight: "500",
            }}
          >
            {buttonType === "pay" ? "Pay with " : "Buy with "}
            <span style={{ fontWeight: "700" }}>G</span>
            <span style={{ color: "#4285F4" }}>o</span>
            <span style={{ color: "#EA4335" }}>o</span>
            <span style={{ color: "#FBBC04" }}>g</span>
            <span style={{ color: "#4285F4" }}>l</span>
            <span style={{ color: "#34A853" }}>e</span>
            <span style={{ fontWeight: "700" }}> Pay</span>
          </button>
        ) : (
          <div
            style={{
              textAlign: "center",
              padding: "16px",
              color: "#666",
            }}
          >
            <div
              style={{
                display: "inline-block",
                width: "32px",
                height: "32px",
                border: "3px solid #f3f3f3",
                borderTop: "3px solid #3498db",
                borderRadius: "50%",
                animation: "spin 1s linear infinite",
                marginBottom: "8px",
              }}
            ></div>
            <p style={{ margin: 0, fontSize: "14px" }}>{loadingText}</p>
            <style>{`
              @keyframes spin {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(360deg); }
              }
            `}</style>
          </div>
        )}
      </div>
      {error && (
        <div
          style={{
            marginTop: "8px",
            padding: "8px",
            backgroundColor: "#fee",
            border: "1px solid #fcc",
            borderRadius: "4px",
            color: "#c33",
            fontSize: "12px",
          }}
        >
          {error}
        </div>
      )}
    </div>
  );
};
