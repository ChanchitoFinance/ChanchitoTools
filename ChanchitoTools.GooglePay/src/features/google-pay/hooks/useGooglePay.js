import { useState, useEffect, useRef, useCallback } from "react";
import { GooglePayService } from "../core/GooglePayService";
import { getGooglePayConfig } from "../core/config";

export const useGooglePay = ({
  onSuccess,
  onError,
  config: customConfig,
} = {}) => {
  const [isReady, setIsReady] = useState(false);
  const [isProcessing, setIsProcessing] = useState(false);
  const [error, setError] = useState(null);

  const serviceRef = useRef(null);

  useEffect(() => {
    const initializeService = async () => {
      try {
        const config = customConfig || getGooglePayConfig();
        serviceRef.current = new GooglePayService(config);

        const ready = await serviceRef.current.initialize();
        setIsReady(ready);

        if (!ready) {
          const errorMsg = "Google Pay is not available on this device";
          setError(errorMsg);
          onError?.(errorMsg);
        }
      } catch (err) {
        const errorMsg = err.message || "Failed to initialize Google Pay";
        setError(errorMsg);
        onError?.(errorMsg);
      }
    };

    initializeService();
  }, [customConfig, onError]);

  const processPayment = useCallback(
    async (amount) => {
      if (!serviceRef.current?.isAvailable()) {
        const errorMsg = "Google Pay is not available";
        setError(errorMsg);
        onError?.(errorMsg);
        return;
      }

      setIsProcessing(true);
      setError(null);

      try {
        const paymentData = await serviceRef.current.processPayment(amount);
        onSuccess?.(paymentData);
      } catch (err) {
        const errorMsg = err.message || "Payment failed";
        setError(errorMsg);
        onError?.(errorMsg);
      } finally {
        setIsProcessing(false);
      }
    },
    [onSuccess, onError]
  );

  const reset = useCallback(() => {
    setError(null);
    setIsProcessing(false);
  }, []);

  return {
    isReady,
    isProcessing,
    error,
    processPayment,
    reset,
  };
};
