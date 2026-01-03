import { GOOGLE_PAY_SDK_URL } from "./constants";

let sdkLoadPromise = null;

export const loadGooglePaySDK = () => {
  if (sdkLoadPromise) {
    return sdkLoadPromise;
  }

  if (window.google?.payments?.api?.PaymentsClient) {
    return Promise.resolve();
  }

  sdkLoadPromise = new Promise((resolve, reject) => {
    const existingScript = document.querySelector(
      `script[src="${GOOGLE_PAY_SDK_URL}"]`
    );

    if (existingScript) {
      existingScript.addEventListener("load", resolve);
      existingScript.addEventListener("error", reject);
      return;
    }

    const script = document.createElement("script");
    script.src = GOOGLE_PAY_SDK_URL;
    script.async = true;

    script.onload = () => {
      if (window.google?.payments?.api?.PaymentsClient) {
        resolve();
      } else {
        reject(new Error("Google Pay SDK loaded but API not available"));
      }
    };

    script.onerror = () => {
      reject(new Error("Failed to load Google Pay SDK"));
    };

    document.body.appendChild(script);
  });

  return sdkLoadPromise;
};

export const isGooglePaySDKLoaded = () => {
  return Boolean(window.google?.payments?.api?.PaymentsClient);
};
