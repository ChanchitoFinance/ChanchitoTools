# Payment Gateway Integration Modules

Fully decoupled, reusable payment integration modules (Google Pay & PayPal) that can be copied into any JavaScript project.

## Features

- Clean separation from UI frameworks (no Tailwind/styling dependencies)
- Configuration-based setup via environment variables
- React hooks for easy integration
- TypeScript-ready with JSDoc types
- Error handling
- Test and production environment support
- Multiple payment methods support

## Supported Payment Methods

- **Google Pay**: Mobile and web payments via Google Pay
- **PayPal**: PayPal balance, credit/debit cards, Venmo, and more

---

# Google Pay Integration

## Quick Start

### 1. Copy the Module

Copy the entire `src/features/google-pay/` directory into your project.

### 2. Install Dependencies

This module requires React:

```bash
npm install react
```

### 3. Configure Environment Variables

Create or update a `.env` file in your project root:

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
      buttonColor="black"
      buttonType="pay"
      className="your-custom-class"
    />
  );
}
```

## Google Pay API Reference

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

---

# PayPal Integration

## Quick Start

### 1. Copy the Module

Copy the entire `src/features/paypal/` directory into your project.

### 2. Install Dependencies

This module requires React:

```bash
npm install react
```

### 3. Get PayPal Credentials

#### For Testing (Sandbox):

1. Go to https://developer.paypal.com/
2. Log in with your PayPal account
3. Navigate to **Dashboard** → **Apps & Credentials**
4. Switch to **Sandbox** mode (toggle at the top)
5. Create an app or use the default one
6. Copy the **Sandbox Client ID**

#### For Production:

1. Same dashboard, but switch to **Production** mode
2. Create a production app
3. Copy the **Production Client ID**
4. Ensure your PayPal business account is verified

### 4. Configure Environment Variables

Add to your `.env` file:

```env
VITE_PAYPAL_CLIENT_ID=your_client_id_here
VITE_PAYPAL_ENVIRONMENT=sandbox
VITE_PAYPAL_CURRENCY=USD
VITE_PAYPAL_INTENT=capture
VITE_PAYPAL_LOCALE=en_US
VITE_PAYPAL_MERCHANT_ID=
VITE_PAYPAL_ENABLE_FUNDING=
VITE_PAYPAL_DISABLE_FUNDING=
```

**Configuration Options:**

- `VITE_PAYPAL_CLIENT_ID`: **(Required)** Your Client ID from PayPal Developer Dashboard
- `VITE_PAYPAL_ENVIRONMENT`: Use `sandbox` for testing, `production` for live transactions
- `VITE_PAYPAL_CURRENCY`: Currency code (USD, EUR, GBP, etc.)
- `VITE_PAYPAL_INTENT`: `capture` (immediate payment) or `authorize` (authorize first, capture later)
- `VITE_PAYPAL_LOCALE`: Language/region code (en_US, es_ES, etc.)
- `VITE_PAYPAL_MERCHANT_ID`: **(Optional)** Only needed for advanced tracking
- `VITE_PAYPAL_ENABLE_FUNDING`: **(Optional)** Force show payment methods (e.g., `venmo,credit`)
- `VITE_PAYPAL_DISABLE_FUNDING`: **(Optional)** Hide payment methods (e.g., `card,credit`)

**Note:** Leave `VITE_PAYPAL_ENABLE_FUNDING`, `VITE_PAYPAL_DISABLE_FUNDING`, and `VITE_PAYPAL_MERCHANT_ID` empty for default behavior (recommended for most use cases).

### 5. Integration Points

#### Option A: Using React Hook (Recommended)

```javascript
import { usePayPal } from "./features/paypal/hooks/usePayPal";

function CheckoutPage() {
  const { isReady, error, renderButton } = usePayPal({
    onSuccess: (orderData) => {
      console.log("Payment successful:", orderData);
    },
    onError: (error) => {
      console.error("Payment failed:", error);
    },
  });

  useEffect(() => {
    if (isReady) {
      renderButton("#paypal-button-container", 99.99);
    }
  }, [isReady, renderButton]);

  return (
    <div>
      <div id="paypal-button-container"></div>
      {error && <p>Error: {error}</p>}
    </div>
  );
}
```

#### Option B: Using Core Service Directly

```javascript
import { PayPalService } from "./features/paypal/core/PayPalService";
import { getPayPalConfig } from "./features/paypal/core/config";

const config = getPayPalConfig();
const paypalService = new PayPalService(config);

await paypalService.initialize();

if (paypalService.isAvailable()) {
  paypalService.renderButton("#paypal-button-container", {
    amount: 99.99,
    onApprove: (orderData) => console.log("Success:", orderData),
    onError: (error) => console.error("Error:", error),
  });
}
```

#### Option C: Using the React Button Component

```javascript
import { PayPalButton } from "./features/paypal/components/PayPalButton";

function Checkout() {
  return (
    <PayPalButton
      amount={99.99}
      onSuccess={(orderData) => console.log("Success:", orderData)}
      onError={(error) => console.error("Error:", error)}
      style={{
        shape: "rect",
        color: "gold",
        layout: "vertical",
        label: "paypal",
      }}
      className="your-custom-class"
    />
  );
}
```

## PayPal API Reference

### Configuration Object

```javascript
{
  clientId: string,
  environment: 'sandbox' | 'production',
  currency: string,
  intent: 'capture' | 'authorize',
  locale: string,
  merchantId: string,
  enableFunding: string,
  disableFunding: string
}
```

### PayPalService Methods

- `initialize()`: Load PayPal SDK and check availability
- `isAvailable()`: Returns boolean if PayPal is ready
- `renderButton(containerId, options)`: Render PayPal button in a container
- `createOrder(amount, currency)`: Create a PayPal order

### usePayPal Hook Returns

```javascript
{
  isReady: boolean,
  isProcessing: boolean,
  error: string | null,
  renderButton: (containerId: string, amount: number, options?: object) => void,
  reset: () => void
}
```

### Button Style Options

```javascript
style: {
  shape: 'rect' | 'pill',
  color: 'gold' | 'blue' | 'silver' | 'white' | 'black',
  layout: 'vertical' | 'horizontal',
  label: 'paypal' | 'checkout' | 'pay' | 'buynow'
}
```

---

# Testing vs Production

## Understanding Test Environments

Both Google Pay and PayPal have separate test environments that use **fake money** and **test accounts**.

### Google Pay Testing

- **Environment:** `TEST`
- **Cards:** Any valid card added to your Google account
- **Charges:** No real charges are made
- **URL:** Same as production (handled by environment config)

### PayPal Testing (Sandbox)

- **Environment:** `sandbox`
- **Accounts:** Automatically created test accounts
- **Money:** Fictitious $9,999 USD per test account
- **URL:** https://sandbox.paypal.com (separate from production)

## PayPal Sandbox Accounts

When you create an app in the PayPal Developer Dashboard, PayPal automatically creates test accounts for you:

1. **Business Account (Merchant):** Receives payments from customers
2. **Personal Account (Buyer):** Makes payments to merchants

### Viewing Your Test Accounts

1. Go to https://developer.paypal.com/dashboard/accounts
2. You'll see pre-created sandbox accounts
3. Click on any account to see:
   - Email address
   - Password
   - Account balance

### Using Sandbox Accounts

When testing your app:

1. Run your app with `VITE_PAYPAL_ENVIRONMENT=sandbox`
2. Add items to cart and proceed to checkout
3. Click the PayPal button
4. A popup opens asking you to log in
5. **Use the Personal (Buyer) test account credentials** from the dashboard
6. Complete the payment
7. Money is transferred (fictitiously) from Personal to Business account

### Important Notes

- **Do NOT use your real PayPal account** for sandbox testing - it won't work
- Test accounts only work with Sandbox Client ID
- Real accounts only work with Production Client ID
- Mixing them will result in errors

## Moving to Production

### Google Pay

1. Update `.env`:

```env
VITE_GOOGLE_PAY_ENVIRONMENT=PRODUCTION
VITE_GOOGLE_PAY_MERCHANT_ID=your_production_merchant_id
```

2. Ensure you have:
   - Valid production Merchant ID from Google Pay Business Console
   - Proper gateway integration configured
   - SSL certificate (HTTPS required)

### PayPal

1. Get Production Client ID:

   - Dashboard → Apps & Credentials
   - Switch to **Production** mode
   - Create or select an app
   - Copy Production Client ID

2. Update `.env`:

```env
VITE_PAYPAL_CLIENT_ID=your_production_client_id
VITE_PAYPAL_ENVIRONMENT=production
```

3. Ensure your PayPal business account is verified

### Visual Indicators

The demo bookstore app shows test mode warnings automatically:

- **Sandbox/Test Mode:** Yellow warning banners appear
- **Production Mode:** No warning banners (clean checkout experience)

This is handled automatically by checking environment variables:

```javascript
const isGooglePayTest = import.meta.env.VITE_GOOGLE_PAY_ENVIRONMENT === "TEST";
const isPayPalTest = import.meta.env.VITE_PAYPAL_ENVIRONMENT === "sandbox";
const isTestMode = isGooglePayTest || isPayPalTest;
```

---

# Advanced Features

## Funding Sources (PayPal)

Control which payment methods appear in the PayPal button.

### Enable Specific Methods

```env
VITE_PAYPAL_ENABLE_FUNDING=venmo,paylater
```

This forces PayPal to show Venmo and Pay Later options (if available in the user's region).

### Disable Specific Methods

```env
VITE_PAYPAL_DISABLE_FUNDING=card,credit
```

This hides credit/debit cards and PayPal Credit, showing only PayPal balance.

### Available Funding Sources

- `card` - Credit and debit cards
- `credit` - PayPal Credit
- `venmo` - Venmo (US only)
- `paylater` - Pay in 4 or Pay Later options
- `bancontact` - Bancontact (Belgium)
- `blik` - BLIK (Poland)
- `eps` - EPS (Austria)
- `giropay` - giropay (Germany)
- `ideal` - iDEAL (Netherlands)
- `mercadopago` - Mercado Pago
- `mybank` - MyBank (Italy)
- `p24` - Przelewy24 (Poland)
- `sepa` - SEPA Direct Debit
- `sofort` - Sofort (Europe)

**Recommendation:** Leave both empty for most use cases to let PayPal automatically show the best options for each user.

## Multiple Payment Methods

The demo app shows how to offer both Google Pay and PayPal:

```javascript
import { GooglePayButton } from "./features/google-pay";
import { PayPalButton } from "./features/paypal";

function Checkout() {
  const [selectedMethod, setSelectedMethod] = useState("googlepay");

  return (
    <div>
      <select onChange={(e) => setSelectedMethod(e.target.value)}>
        <option value="googlepay">Google Pay</option>
        <option value="paypal">PayPal</option>
      </select>

      {selectedMethod === "googlepay" && (
        <GooglePayButton amount={99.99} onSuccess={handleSuccess} />
      )}

      {selectedMethod === "paypal" && (
        <PayPalButton amount={99.99} onSuccess={handleSuccess} />
      )}
    </div>
  );
}
```

## Custom Payment Configuration

### Google Pay - Custom Card Networks

Override default card networks and auth methods:

```javascript
const customConfig = {
  ...getGooglePayConfig(),
  allowedCardNetworks: ["VISA", "MASTERCARD"],
  allowedAuthMethods: ["CRYPTOGRAM_3DS"],
};

const service = new GooglePayService(customConfig);
```

### PayPal - Custom Button Styling

```javascript
<PayPalButton
  amount={99.99}
  onSuccess={handleSuccess}
  style={{
    shape: "pill",
    color: "blue",
    layout: "horizontal",
    label: "checkout",
  }}
/>
```

### Multiple Currency Support

Both modules support multiple currencies:

```env
VITE_GOOGLE_PAY_CURRENCY=EUR
VITE_GOOGLE_PAY_COUNTRY=DE

VITE_PAYPAL_CURRENCY=EUR
VITE_PAYPAL_LOCALE=de_DE
```

---

# Troubleshooting

## Google Pay Issues

### Button not showing

- Check browser console for errors
- Verify environment variables are loaded
- Ensure HTTPS in production (localhost OK for testing)
- Check Google Pay is available in user's region

### Payment failing

- Verify gateway credentials are correct
- Check TEST/PRODUCTION environment matches your setup
- Review gateway documentation for proper configuration

## PayPal Issues

### Button not loading

- Verify Client ID is correct and matches environment (sandbox vs production)
- Check browser console for SDK loading errors
- Ensure you're not blocking PayPal scripts with ad blockers

### Cannot log in during testing

- Make sure you're using **sandbox test account** credentials, not your real PayPal account
- Check that `VITE_PAYPAL_ENVIRONMENT=sandbox` is set correctly
- Verify test account credentials in Developer Dashboard

### Payment declined

- Test accounts should have $9,999 balance by default
- Try creating a new test account if balance is depleted
- Check that test account is active in Developer Dashboard

### Mixing environments error

```
❌ Sandbox Client ID + Real PayPal Account = ERROR
✅ Sandbox Client ID + Sandbox Test Account = OK
✅ Production Client ID + Real PayPal Account = OK
```

## General Issues

### Environment variables not loading

- Restart your dev server after changing `.env`
- Verify `.env` is in project root
- Check that variables start with `VITE_` for Vite projects

### CORS errors

- PayPal and Google Pay SDKs are loaded from their CDNs
- Ensure your domain is properly configured in production
- For Google Pay, add your domain to the Business Console

---

# Security Notes

- Never expose merchant credentials in client-side code
- Always verify payments on your server
- Use HTTPS in production
- Store sensitive config in environment variables
- Validate payment data before fulfilling orders
- **Never commit `.env` files to version control** - add to `.gitignore`

## Server-Side Verification

After receiving payment data, always verify on your server:

```javascript
onSuccess: async (paymentData) => {
  const response = await fetch("/api/verify-payment", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ paymentData }),
  });

  const result = await response.json();
  if (result.verified) {
    // Fulfill order
  }
};
```

---

# Support

## Google Pay

- [Google Pay Web Documentation](https://developers.google.com/pay/api/web)
- [Google Pay Business Console](https://pay.google.com/business/console)

## PayPal

- [PayPal Developer Documentation](https://developer.paypal.com/docs/)
- [PayPal Developer Dashboard](https://developer.paypal.com/dashboard)
- [PayPal Sandbox Testing](https://developer.paypal.com/tools/sandbox/)

---

# License

These modules are provided as-is for integration purposes.
