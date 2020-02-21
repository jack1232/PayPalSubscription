using PayPalSubscriptionNetSdk;

namespace Samples
{
    public class SetupSubscriptionClient
    {
        public static void client()
        {
            RestClientV1.ClientId = "YOUR-PAYPAL-CLIENT-ID";
            RestClientV1.Secret = "YOUR-PAYPAL-CLIENT-SECRET";
            RestClientV1.BaseUrl = "https://api.sandbox.paypal.com";
        }
    }
}
