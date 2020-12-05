using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayPalSubscriptionNetSdk.Subscriptions
{
    public class UpdatePricingRequest
    {
        [JsonProperty("pricing_schemes")]
        public List<UpdatePricingSchemeRequest> UpdatePricingSchemeRequests { get; set; }
    }
}
