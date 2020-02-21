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
            // specify the product ID
            var productId = "PROD-2XJ74819Y3199382R";

            // construct a Plan object:
            var plan = BuildPlanBody(productId);

            // call API using the static method PlanCreate() of the SDK and get a response for your call
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
