import React, { useEffect, useRef } from "react";
import { usePayPal } from "../hooks/usePayPal";

export const PayPalButton = ({
  amount,
  currency,
  onSuccess,
  onError,
  style = {},
  className = "",
}) => {
  const containerRef = useRef(null);
  const containerId = useRef(`paypal-button-${Date.now()}`).current;
  const hasRendered = useRef(false);

  const { isReady, renderButton } = usePayPal({
    onSuccess,
    onError,
  });

  useEffect(() => {
    if (isReady && containerRef.current && !hasRendered.current && amount > 0) {
      hasRendered.current = true;

      renderButton(`#${containerId}`, amount, {
        currency,
        style,
      });
    }
  }, [isReady, amount, currency, style, renderButton, containerId]);

  if (!isReady) {
    return (
      <div className={`flex items-center justify-center py-4 ${className}`}>
        <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return <div ref={containerRef} id={containerId} className={className}></div>;
};
