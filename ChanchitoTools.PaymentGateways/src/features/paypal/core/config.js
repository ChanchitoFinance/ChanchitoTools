export const getPayPalConfig = () => {
  return {
    clientId: import.meta.env.VITE_PAYPAL_CLIENT_ID || "",
    currency: import.meta.env.VITE_PAYPAL_CURRENCY || "USD",
    intent: import.meta.env.VITE_PAYPAL_INTENT || "capture",
    locale: import.meta.env.VITE_PAYPAL_LOCALE || "en_US",
    enableFunding: import.meta.env.VITE_PAYPAL_ENABLE_FUNDING || "",
    disableFunding: import.meta.env.VITE_PAYPAL_DISABLE_FUNDING || "",
    merchantId: import.meta.env.VITE_PAYPAL_MERCHANT_ID || "",
    environment: import.meta.env.VITE_PAYPAL_ENVIRONMENT || "sandbox",
  };
};

export const validateConfig = (config) => {
  const required = ["clientId"];
  const missing = required.filter((key) => !config[key]);

  if (missing.length > 0) {
    throw new Error(
      `Missing required PayPal configuration: ${missing.join(", ")}`
    );
  }

  const validIntents = ["capture", "authorize", "subscription"];
  if (!validIntents.includes(config.intent)) {
    throw new Error(
      `Invalid intent. Must be one of: ${validIntents.join(", ")}`
    );
  }

  const validEnvironments = ["sandbox", "production"];
  if (!validEnvironments.includes(config.environment)) {
    throw new Error(
      `Invalid environment. Must be one of: ${validEnvironments.join(", ")}`
    );
  }

  return true;
};
