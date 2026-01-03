import { loadPayPalSDK } from "./sdkLoader";
import { validateConfig } from "./config";
import { ERROR_CODES, BUTTON_STYLES } from "./constants";

export class PayPalService {
  constructor(config) {
    validateConfig(config);
    this.config = config;
    this.ready = false;
  }

  async initialize() {
    try {
      await loadPayPalSDK(this.config);

      if (!window.paypal) {
        throw new Error("PayPal SDK not loaded");
      }

      this.ready = true;
      return this.ready;
    } catch (error) {
      console.error("PayPal initialization error:", error);
      throw new Error(`Failed to initialize PayPal: ${error.message}`);
    }
  }

  isAvailable() {
    return this.ready && window.paypal !== undefined;
  }

  createOrder(amount, currency = null) {
    if (!this.isAvailable()) {
      throw new Error("PayPal is not available. Call initialize() first.");
    }

    const currencyCode = currency || this.config.currency;

    return window.paypal.Buttons({
      createOrder: (data, actions) => {
        return actions.order.create({
          purchase_units: [
            {
              amount: {
                value: amount.toFixed(2),
                currency_code: currencyCode,
              },
            },
          ],
        });
      },
    });
  }

  renderButton(containerId, options = {}) {
    if (!this.isAvailable()) {
      throw new Error("PayPal is not available. Call initialize() first.");
    }

    const {
      amount,
      currency,
      onApprove,
      onError,
      onCancel,
      style = {},
    } = options;

    const currencyCode = currency || this.config.currency;

    const buttonConfig = {
      style: {
        shape: style.shape || BUTTON_STYLES.SHAPE.RECT,
        color: style.color || BUTTON_STYLES.COLOR.GOLD,
        layout: style.layout || BUTTON_STYLES.LAYOUT.VERTICAL,
        label: style.label || BUTTON_STYLES.LABEL.PAYPAL,
      },
      createOrder: (data, actions) => {
        return actions.order.create({
          purchase_units: [
            {
              amount: {
                value: amount.toFixed(2),
                currency_code: currencyCode,
              },
            },
          ],
        });
      },
      onApprove: async (data, actions) => {
        try {
          const order = await actions.order.capture();
          if (onApprove) {
            onApprove(order);
          }
          return order;
        } catch (error) {
          if (onError) {
            onError(error.message || "Payment capture failed");
          }
          throw error;
        }
      },
      onError: (err) => {
        if (onError) {
          onError(err.message || "Payment error occurred");
        }
      },
      onCancel: (data) => {
        if (onCancel) {
          onCancel(data);
        }
      },
    };

    return window.paypal.Buttons(buttonConfig).render(containerId);
  }
}
