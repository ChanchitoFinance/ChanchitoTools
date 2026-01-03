import { useState, useEffect, useRef, useCallback } from "react";
import { PayPalService } from "../core/PayPalService";
import { getPayPalConfig } from "../core/config";

export const usePayPal = ({
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
        const config = customConfig || getPayPalConfig();
        serviceRef.current = new PayPalService(config);

        const ready = await serviceRef.current.initialize();
        setIsReady(ready);

        if (!ready) {
          const errorMsg = "PayPal is not available";
          setError(errorMsg);
          onError?.(errorMsg);
        }
      } catch (err) {
        const errorMsg = err.message || "Failed to initialize PayPal";
        setError(errorMsg);
        onError?.(errorMsg);
      }
    };

    initializeService();
  }, [customConfig, onError]);

  const renderButton = useCallback(
    (containerId, amount, options = {}) => {
      if (!serviceRef.current?.isAvailable()) {
        const errorMsg = "PayPal is not available";
        setError(errorMsg);
        onError?.(errorMsg);
        return;
      }

      setIsProcessing(false);
      setError(null);

      try {
        return serviceRef.current.renderButton(containerId, {
          amount,
          ...options,
          onApprove: (orderData) => {
            setIsProcessing(false);
            onSuccess?.(orderData);
          },
          onError: (errorMsg) => {
            setIsProcessing(false);
            setError(errorMsg);
            onError?.(errorMsg);
          },
          onCancel: () => {
            setIsProcessing(false);
            setError("Payment cancelled by user");
          },
        });
      } catch (err) {
        const errorMsg = err.message || "Failed to render PayPal button";
        setError(errorMsg);
        onError?.(errorMsg);
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
    renderButton,
    reset,
  };
};
