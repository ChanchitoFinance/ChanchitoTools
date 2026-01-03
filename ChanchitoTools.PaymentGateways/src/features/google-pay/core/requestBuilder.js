import {
  API_VERSION,
  API_VERSION_MINOR,
  PAYMENT_METHOD_TYPE,
  TOTAL_PRICE_STATUS,
} from "./constants";

export const buildBasePaymentMethod = (config) => {
  return {
    type: PAYMENT_METHOD_TYPE,
    parameters: {
      allowedAuthMethods: config.allowedAuthMethods,
      allowedCardNetworks: config.allowedCardNetworks,
    },
  };
};

export const buildTokenizationSpecification = (config) => {
  return {
    type: "PAYMENT_GATEWAY",
    parameters: {
      gateway: config.gateway,
      gatewayMerchantId: config.gatewayMerchantId,
    },
  };
};

export const buildIsReadyToPayRequest = (config) => {
  return {
    apiVersion: API_VERSION,
    apiVersionMinor: API_VERSION_MINOR,
    allowedPaymentMethods: [buildBasePaymentMethod(config)],
  };
};

export const buildPaymentDataRequest = (config, amount) => {
  const basePaymentMethod = buildBasePaymentMethod(config);

  return {
    apiVersion: API_VERSION,
    apiVersionMinor: API_VERSION_MINOR,
    allowedPaymentMethods: [
      {
        ...basePaymentMethod,
        tokenizationSpecification: buildTokenizationSpecification(config),
      },
    ],
    merchantInfo: {
      merchantId: config.merchantId,
      merchantName: config.merchantName,
    },
    transactionInfo: {
      totalPriceStatus: TOTAL_PRICE_STATUS.FINAL,
      totalPrice: amount.toFixed(2),
      currencyCode: config.currencyCode,
      countryCode: config.countryCode,
    },
  };
};
