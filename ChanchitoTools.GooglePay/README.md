# Google Pay Integration Module

A fully decoupled, reusable Google Pay integration that can be copied into any JavaScript project.

## Features

- Clean separation from UI frameworks (no Tailwind/styling dependencies)
- Configuration-based setup via environment variables
- React hooks for easy integration
- TypeScript-ready with JSDoc types
- Error handling
- Test and production environment support

## Quick Start

### 1. Copy the Module

You can copy the entire `src/features/google-pay/` directory into your project:

### 2. Install Dependencies

This module requires React:

```bash
npm install react
```

### 3. Configure Environment Variables

Create or update a `.env` file in your project root to include:

```env
VITE_GOOGLE_PAY_MERCHANT_ID=your_merchant_id_here
VITE_GOOGLE_PAY_GATEWAY=example
VITE_GOOGLE_PAY_GATEWAY_MERCHANT_ID=your_gateway_merchant_id
VITE_GOOGLE_PAY_MERCHANT_NAME=Your Store Name
VITE_GOOGLE_PAY_ENVIRONMENT=TEST
VITE_GOOGLE_PAY_CURRENCY=USD
VITE_GOOGLE_PAY_COUNTRY=US
```

**Important Configuration Options:**

- `VITE_GOOGLE_PAY_ENVIRONMENT`: Use `TEST` for testing, `PRODUCTION` for live transactions
- `VITE_GOOGLE_PAY_MERCHANT_ID`: Your Google Pay Merchant ID from Google Pay Business Console
- `VITE_GOOGLE_PAY_GATEWAY`: Your payment gateway (e.g., 'stripe', 'braintree', 'example')
- `VITE_GOOGLE_PAY_GATEWAY_MERCHANT_ID`: Your gateway's merchant identifier

### 4. Integration Points

#### Option A: Using React Hook (Recommended)

```javascript
import { useGooglePay } from "./features/google-pay/hooks/useGooglePay";

function CheckoutPage() {
  const { isReady, error, processPayment } = useGooglePay({
    onSuccess: (paymentData) => {
      console.log("Payment successful:", paymentData);
    },
    onError: (error) => {
      console.error("Payment failed:", error);
    },
  });

  const handlePay = () => {
    processPayment(99.99);
  };

  return (
    <div>
      {isReady && <button onClick={handlePay}>Pay with Google Pay</button>}
      {error && <p>Error: {error}</p>}
    </div>
  );
}
```

#### Option B: Using Core Service Directly

```javascript
import { GooglePayService } from "./features/google-pay/core/GooglePayService";
import { getGooglePayConfig } from "./features/google-pay/core/config";

const config = getGooglePayConfig();
const googlePayService = new GooglePayService(config);

await googlePayService.initialize();

if (googlePayService.isAvailable()) {
  const paymentData = await googlePayService.processPayment(99.99);
  console.log("Payment successful:", paymentData);
}
```

#### Option C: Using the React Button Component

```javascript
import { GooglePayButton } from "./features/google-pay/components/GooglePayButton";

function Checkout() {
  return (
    <GooglePayButton
      amount={99.99}
      onSuccess={(paymentData) => console.log("Success:", paymentData)}
      onError={(error) => console.error("Error:", error)}
      className="your-custom-class"
    />
  );
}
```

## API Reference

### Configuration Object

```javascript
{
  merchantId: string,
  merchantName: string,
  gateway: string,
  gatewayMerchantId: string,
  environment: 'TEST' | 'PRODUCTION',
  currencyCode: string,
  countryCode: string,
  allowedCardNetworks: string[],
  allowedAuthMethods: string[]
}
```

### GooglePayService Methods

- `initialize()`: Load Google Pay SDK and check availability
- `isAvailable()`: Returns boolean if Google Pay is ready
- `processPayment(amount)`: Process a payment and return payment data
- `createButton(onClick, options)`: Create official Google Pay button element

### useGooglePay Hook Returns

```javascript
{
  isReady: boolean,
  isProcessing: boolean,
  error: string | null,
  processPayment: (amount: number) => Promise<void>,
  reset: () => void
}
```

## Customization

### Styling the Button Component

The `GooglePayButton` component accepts a `className` prop for custom styling:

```javascript
<GooglePayButton
  amount={99.99}
  onSuccess={handleSuccess}
  onError={handleError}
  className="w-full rounded-lg shadow-md"
/>
```

### Custom Payment Configuration

Override default card networks and auth methods:

```javascript
const customConfig = {
  ...getGooglePayConfig(),
  allowedCardNetworks: ["VISA", "MASTERCARD"],
  allowedAuthMethods: ["CRYPTOGRAM_3DS"],
};

const service = new GooglePayService(customConfig);
```

## Testing

### Test Cards

In TEST environment, use Google Pay test cards:

- Add any valid card to your Google account
- Payments will not be charged in TEST mode

### Moving to Production

1. Update `.env`:

```env
VITE_GOOGLE_PAY_ENVIRONMENT=PRODUCTION
```

2. Ensure you have:
   - Valid production Merchant ID from Google Pay Business Console
   - Proper gateway integration configured
   - SSL certificate (HTTPS required)

## Troubleshooting

### Google Pay button not showing

- Check browser console for errors
- Verify environment variables are loaded
- Ensure HTTPS in production (localhost OK for testing)
- Check Google Pay is available in user's region

### Payment failing

- Verify gateway credentials are correct
- Check TEST/PRODUCTION environment matches your setup
- Review gateway documentation for proper configuration

## Advanced Usage

### Server-Side Verification

After receiving payment data, verify on your server:

```javascript
onSuccess: async (paymentData) => {
  const response = await fetch("/api/verify-payment", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ paymentData }),
  });

  const result = await response.json();
  if (result.verified) {
  }
};
```

### Multiple Currency Support

```javascript
const configEUR = {
  ...getGooglePayConfig(),
  currencyCode: "EUR",
  countryCode: "DE",
};
```

## Security Notes

- Never expose merchant credentials in client-side code
- Always verify payments on your server
- Use HTTPS in production
- Store sensitive config in environment variables
- Validate payment data before fulfilling orders

## Support

For Google Pay specific issues, consult:

- [Google Pay Web Documentation](https://developers.google.com/pay/api/web)
- [Google Pay Business Console](https://pay.google.com/business/console)

## License

This module is provided as-is for integration purposes.
