using Newtonsoft.Json;
using PayPalSubscriptionNetSdk.Subscriptions;
using RestSharp;
using System;

namespace Samples
{
    public class CreateSubscriptionSample
    {
        public static IRestResponse CreateSubscription()
        {
            // specify a plan ID:
            var planId = "P-55V69445RP894091SLZHQBIA";

            // Construct a Subscription object:
            var subscription = BuildSubscriptionBody(planId);

            // call API using the static method SubscriptionCreate() of the SDK and get a response for your call:
            var response = SubscriptionResponse.SubscriptionCreate(subscription);
            var result = JsonConvert.DeserializeObject<Subscription>(response.Content);

            Console.WriteLine("Status: {0}", response.StatusCode);
            Console.WriteLine("Pan Id: {0}", result.Id);
            Console.WriteLine("Links:");
            foreach (LinkDescriptionObject link in result.Links)
            {
                Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
            }

            return response;
        }

        private static Subscription BuildSubscriptionBody(string planid)
        {
            return new Subscription()
            {
                PlanId = planid,
                StartTime = "2020-03-01T06:00:00Z",
                ShippingAmount = new Currency
                {
                    Value = "10.00",
                    CurrencyCode = "USD"
                },
                Subscriber = new Subscriber()
                {
                    Name = new Name
                    {
                        GivenName = "John",
                        Surname = "Doe"
                    },
                    EmailAddress = "customer@example.com",
                    ShippingAddress = new ShippingAddress
                    {
                        Name = new FullName
                        {
                            Fullname = "John Doe"
                        },
                        Address = new Address
                        {
                            AddressLine1 = "2211 N First Street",
                            AddressLine2 = "Building 17",
                            AdminArea2 = "San Jose",
                            AdminArea1 = "CA",
                            PostalCode = "95131",
                            CountryCode = "US"
                        }
                    }
                },
                ApplicationContext = new ApplicationContext()
                {
                    BrandName = "Walmart",
                    Locale = "en-US",
                    ShippingPreference = ApplicationContextShippingPreferenceEnum.SET_PROVIDED_ADDRESS,
                    UserAction = ApplicationContextUserActionEnum.SUBSCRIBE_NOW,
                    PaymentMethod = new PaymentMethod()
                    {
                        PayerSelected = "PAYPAL",
                        PayeePreferred = "IMMEDIATE_PAYMENT_REQUIRED"
                    },
                    ReturnUrl = "https://example.com/returnUrl",
                    CancelUrl = "https://example.com/cancelUrl"
                }
            };
        }
    }
}
