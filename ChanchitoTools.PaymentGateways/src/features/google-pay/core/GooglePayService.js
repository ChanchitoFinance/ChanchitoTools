import { loadGooglePaySDK } from "./sdkLoader";
import { validateConfig } from "./config";
import {
  buildIsReadyToPayRequest,
  buildPaymentDataRequest,
} from "./requestBuilder";
import {
  ERROR_CODES,
  BUTTON_COLORS,
  BUTTON_TYPES,
  BUTTON_SIZE_MODES,
} from "./constants";

export class GooglePayService {
  constructor(config) {
    validateConfig(config);
    this.config = config;
    this.paymentsClient = null;
    this.ready = false;
  }

  async initialize() {
    try {
      await loadGooglePaySDK();

      this.paymentsClient = new window.google.payments.api.PaymentsClient({
        environment: this.config.environment,
      });

      const isReadyToPayRequest = buildIsReadyToPayRequest(this.config);
      const response = await this.paymentsClient.isReadyToPay(
        isReadyToPayRequest
      );

      this.ready = response.result === true;
      return this.ready;
    } catch (error) {
      console.error("Google Pay initialization error:", error);
      throw new Error(`Failed to initialize Google Pay: ${error.message}`);
    }
  }

  isAvailable() {
    return this.ready && this.paymentsClient !== null;
  }

  async processPayment(amount) {
    if (!this.isAvailable()) {
      throw new Error("Google Pay is not available. Call initialize() first.");
    }

    const paymentDataRequest = buildPaymentDataRequest(this.config, amount);

    try {
      const paymentData = await this.paymentsClient.loadPaymentData(
        paymentDataRequest
      );
      return paymentData;
    } catch (error) {
      if (error.statusCode === ERROR_CODES.CANCELED) {
        throw new Error("Payment cancelled by user");
      }
      throw new Error(
        `Payment failed: ${error.statusMessage || "Unknown error"}`
      );
    }
  }

  createButton(onClick, options = {}) {
    if (!this.isAvailable()) {
      throw new Error("Google Pay is not available. Call initialize() first.");
    }

    const buttonOptions = {
      onClick,
      buttonColor: options.buttonColor || BUTTON_COLORS.BLACK,
      buttonType: options.buttonType || BUTTON_TYPES.PAY,
      buttonSizeMode: options.buttonSizeMode || BUTTON_SIZE_MODES.FILL,
    };

    return this.paymentsClient.createButton(buttonOptions);
  }
}
