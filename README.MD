# PayPal Subscriptions .NET SDK

__Welcome to PayPal Subscription .NET SDK.__ This repository 
contains PayPalSubscriptionNetSdk and samples for 
[PayPal V1 Subscriptions API](https://developer.paypal.com/docs/subscriptions/integrate/).

Currently, PayPal has not ported the Subscriptions API to Version 2 yet, so we have to use Version 1
API for subscription. To make matters even worse, PayPal has never released a stable 
Subscriptions .NET SDK, which makes it difficult for .NET developers to integrate PayPal 
Subscriptions API into .NET and .NET Core applications. Therefore, I decide to implement a 
simple PayPal Subscriptions .NET SDK, which simply exposes various static methods that allow you 
to easily integrate PayPal Subscriptions API into .NET or .NET Core applications. 

This SDK is implemented using the .NET Standard 2.0 Class Library, so it should work for .NET 4.6 or later and 
.NET Core 2.0 or later.

## Usage

### Binaries

It is not necessary to fork this repository for using the PayPal Subscriptions .NET SDK. I 
has published a NuGet package at https://www.nuget.org/packages/PayPalSubscriptionNetSdk/1.0.0.
You can install it in your .NET or .NET Core applications using the following command:

__PM> Install-Package PayPalSubscriptionNetSdk -Version 1.0.0__

For contributing to this repository or using the samples you can fork this repository.

### Set up Credentials

Get your client ID and client Secret by going to https://developer.paypal.com/developer/applications 
and generating a REST API app. Get Client ID and Secret from there.

This SDK implements a simple HTTP Client class called __RestClientV1__ using [RestSharp](https://github.com/restsharp/RestSharp). 
The class name "RestClientV1" means that we apply the RestClient implemented in RestSharp to PayPal Version 1 Subscriptions API.
This HTTP Client exposes three static properties that allow you to easily set up your PayPal credentials:

```
using PayPalSubscriptionNetSdk;

public class SetupSubscriptionClient
{
    public static void client()
    {
        RestClientV1.ClientId = "YOUR-PAYPAL-CLIENT-ID";
        RestClientV1.Secret = "YOUR-PAYPAL-CLIENT-SECRET";
        RestClientV1.BaseUrl = "https://api.sandbox.paypal.com";
    }
}
```

The client method, __SetupSubscriptionClient.client()__, needs only to be called once at 
your application startup. In my recent published book 
__[Practical PayPal Integration in ASP.NET Core](https://www.amazon.com/exec/obidos/ASIN/1695895576/unicadinccom-20)__, 
I explained how to use the dependency injection and/or service locator to set up this client in the __Startup.cs__ file of the ASP.NET Core
Razor-Pages application.

If you want to go to live, simply replace the client ID and Secret by your live account credentials, and 
change the __BaseUrl__ to the following:

```
RestClientV1.BaseUrl = "https://api.paypal.com";
```

## Examples

### Create a Product

This will create a product and print the product ID for the created product:

```
using Newtonsoft.Json;
using PayPalSubscriptionNetSdk.Subscriptions;
using RestSharp;
using System;

namespace Samples
{
    public class CreateProductSample
    {
        public static IRestResponse CreateProduct()
        {
            // Construct a Product object:
            var product = new Product
            {
                Name = "Video Streaming Service",
                Description = "Video streaming service",
                Type = ProductTypeEnum.SERVICE,
                Category = ProductCategoryEnum.SOFTWARE,
                ImageUrl = "https://example.com/streaming.jpg",
                HomeUrl = "https://example.com/home"
            };

            // call API using the static method ProductCreate() of the SDK and get a response for your call:
            var response = ProductResponse.ProductCreate(product);
            var result = JsonConvert.DeserializeObject<Product>(response.Content);
            Console.WriteLine("Status: {0}", response.StatusCode);
            Console.WriteLine("Product Id: {0}", result.Id);            
            Console.WriteLine("Links:");
            foreach (LinkDescriptionObject link in result.Links)
            {
                Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
            }
            return response;
        }
    }
}
```

Here is the output:

![image01](./samples/assets/image01.png)

### Create a Plan

This will create a subscription plan for a specified product ID and print the plan ID for the created plan:

```
using Newtonsoft.Json;
using PayPalSubscriptionNetSdk.Subscriptions;
using RestSharp;
using System;
using System.Collections.Generic;

namespace Samples
{
    public class CreatePlanSample
    {
        public static IRestResponse CreatePlan()
        {
            // specify the product ID:
            var productId = "PROD-2XJ74819Y3199382R";

            // construct a Plan object:
            var plan = BuildPlanBody(productId);

            // call API using the static method PlanCreate() of the SDK and get a response for your call:
            var response = PlanResponse.PlanCreate(plan);
            var result = JsonConvert.DeserializeObject<Plan>(response.Content);

            Console.WriteLine("Status: {0}", response.StatusCode);
            Console.WriteLine("Pan Id: {0}", result.Id);
            Console.WriteLine("Links:");
            foreach (LinkDescriptionObject link in result.Links)
            {
                Console.WriteLine("\t{0}: {1}\tCall Type: {2}", link.Rel, link.Href, link.Method);
            }
            return response;
        }

        private static Plan BuildPlanBody(string productId)
        {
            return new Plan()
            {
                ProductId = productId,
                Name = "Video Streaming Service Plan",
                Description = "Video Streaming Service basic plan",
                Status = PlanStatusEnum.ACTIVE,
                BillingCycles = new List<BillingCycle>()
                {
                    new BillingCycle()
                    {
                        Frequency = new Frequency()
                        {
                            IntervalUnit = "MONTH",
                            IntervalCount = 1
                        },
                        TenureType = BillingCycleTenureTypeEnum.TRIAL,
                        Sequence = 1,
                        TotalCycles = 1,
                        PricingScheme = new PricingScheme()
                        {
                            FixedPrice = new Currency()
                            {
                                Value = "10",
                                CurrencyCode = "USD"
                            }
                        }
                    },
                    new BillingCycle()
                    {
                        Frequency = new Frequency()
                        {
                            IntervalUnit = "MONTH",
                            IntervalCount = 1
                        },
                        TenureType = BillingCycleTenureTypeEnum.REGULAR,
                        Sequence = 2,
                        TotalCycles = 12,
                        PricingScheme = new PricingScheme()
                        {
                            FixedPrice = new Currency()
                            {
                                Value = "100",
                                CurrencyCode = "USD"
                            }
                        }
                    }
                },
                PaymentPreferences = new PaymentPreferences()
                {
                    AutoBillOutstanding = true,
                    SetupFee = new Currency()
                    {
                        Value = "10",
                        CurrencyCode = "USD"
                    },
                    SetupFeeFailureAction = PaymentPreferencesSetupFeeFailureActionEnum.CONTINUE,
                    PaymentFailureThreshold = 3
                },
                Taxes = new Taxes()
                {
                    Percentage = "10",
                    Inclusive = false
                }
            };
        }
    }
}
```

Here is the output:

![image01](./samples/assets/image02.png)

Note that you can associate multiple plans to a product. For example, a video streaming service can have several plans: 
trial, basic, standard, and premium.

### Create a Subscription

This will create a subscription for a specified plan in active status:

```
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
```
Here is the output:

![image01](./samples/assets/image03.png)

To redirect the buyer to log in to their PayPal account to approve the subscription, use the HATEOAS link from the response.

In our example, the HATEOAS URL is:

```
GET https://www.sandbox.paypal.com/webapps/billing/subscriptions?ba_token=BA-2T679733J71686017
```

Here, we use the Subscriptions API to create the subscription. Instead, you can also use PayPal Smart Payment Buttons to create the
subscription. PayPal recommends using Subscriptions with Smart Payment Buttons as it gives your buyers a simplified and secure 
subscription experience. 

## Step-By-Step Tutorial

Recently, I published a book 
__[Practical PayPal Integration in ASP.NET Core](https://www.amazon.com/exec/obidos/ASIN/1695895576/unicadinccom-20)__,
which provides a step-by-step introduction to integrating
PayPal Checkout and Subscription REST APIs into ASP.NET Core applications. It covers the following topics:

- Integrate and render PayPal Smart Payment Buttons, which provides the most relevant payment types.
- Explain how to use PayPal Checkout .NET SDK to complete PayPal transactions, including how to set up the development environment, how to create and manage orders, and how to capture and verify the transaction.
- Explain how to implement PayPal Subscriptions .NET SDK based on PayPal Subscriptions REST API. This SDK allows you to easily integrate PayPal Subscriptions into your .NET or .NET Core applications.
- Provide various examples to demonstrate how to use the Subscriptions SDK to create, update, and manage product, billing plan, and subscription.
- Demonstrate how to complete a subscription transaction using either the subscription API or Smart Payment buttons.


## Run Sample Projects

You can run the sample project using the following __program.cs__ class:

```
using System;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            SetupSubscriptionClient.client();

            CreateProductSample.CreateProduct();
            //CreatePlanSample.CreatePlan();
            //CreateSubscriptionSample.CreateSubscription();

            Console.ReadLine();
        }
    }
}
```

Here, you first call the __SetupSubscriptionClient.client()__ method to set up your credentials, and then call the __CreateProduct()__,
__CreatePlan()__, and __CreateSubscription()__ methods sequentially.

Note: Do not forget to update the __SetupSubscriptionClient.cs__ class with your own Sandbox credentials when executing the samples.

## License

Code released under [MIT LICENSE](./LICENSE.MD).

