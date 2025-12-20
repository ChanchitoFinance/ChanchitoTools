export const getGooglePayConfig = () => {
  return {
    merchantId: import.meta.env.VITE_GOOGLE_PAY_MERCHANT_ID || "",
    merchantName: import.meta.env.VITE_GOOGLE_PAY_MERCHANT_NAME || "Demo Store",
    gateway: import.meta.env.VITE_GOOGLE_PAY_GATEWAY || "example",
    gatewayMerchantId:
      import.meta.env.VITE_GOOGLE_PAY_GATEWAY_MERCHANT_ID || "",
    environment: import.meta.env.VITE_GOOGLE_PAY_ENVIRONMENT || "TEST",
    currencyCode: import.meta.env.VITE_GOOGLE_PAY_CURRENCY || "USD",
    countryCode: import.meta.env.VITE_GOOGLE_PAY_COUNTRY || "US",
    allowedCardNetworks: ["MASTERCARD", "VISA", "AMEX", "DISCOVER"],
    allowedAuthMethods: ["PAN_ONLY", "CRYPTOGRAM_3DS"],
  };
};

export const validateConfig = (config) => {
  const required = [
    "merchantId",
    "merchantName",
    "gateway",
    "gatewayMerchantId",
  ];
  const missing = required.filter((key) => !config[key]);

  if (missing.length > 0) {
    throw new Error(
      `Missing required Google Pay configuration: ${missing.join(", ")}`
    );
  }

  if (!["TEST", "PRODUCTION"].includes(config.environment)) {
    throw new Error("Environment must be either TEST or PRODUCTION");
  }

  return true;
};
