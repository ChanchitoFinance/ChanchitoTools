import { PAYPAL_SDK_BASE_URL } from "./constants";

let sdkLoadPromise = null;

export const buildPayPalSDKUrl = (config) => {
  const params = new URLSearchParams({
    "client-id": config.clientId,
    currency: config.currency,
    intent: config.intent,
  });

  if (config.locale) {
    params.append("locale", config.locale);
  }

  if (config.enableFunding) {
    params.append("enable-funding", config.enableFunding);
  }

  if (config.disableFunding) {
    params.append("disable-funding", config.disableFunding);
  }

  if (config.merchantId) {
    params.append("merchant-id", config.merchantId);
  }

  return `${PAYPAL_SDK_BASE_URL}?${params.toString()}`;
};

export const loadPayPalSDK = (config) => {
  const sdkUrl = buildPayPalSDKUrl(config);

  if (sdkLoadPromise) {
    return sdkLoadPromise;
  }

  if (window.paypal) {
    return Promise.resolve();
  }

  sdkLoadPromise = new Promise((resolve, reject) => {
    const existingScript = document.querySelector(
      `script[src^="${PAYPAL_SDK_BASE_URL}"]`
    );

    if (existingScript) {
      if (window.paypal) {
        resolve();
      } else {
        existingScript.addEventListener("load", () => resolve());
        existingScript.addEventListener("error", (err) => reject(err));
      }
      return;
    }

    const script = document.createElement("script");
    script.src = sdkUrl;
    script.async = true;

    script.onload = () => {
      if (window.paypal) {
        resolve();
      } else {
        reject(new Error("PayPal SDK loaded but API not available"));
      }
    };

    script.onerror = () => {
      reject(new Error("Failed to load PayPal SDK"));
    };

    document.body.appendChild(script);
  });

  return sdkLoadPromise;
};

export const isPayPalSDKLoaded = () => {
  return Boolean(window.paypal);
};
