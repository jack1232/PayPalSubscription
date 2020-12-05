using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PayPalSubscriptionNetSdk.Subscriptions
{
    public class UpdatePricingSchemeRequest
    {
        [JsonProperty("billing_cycle_sequence")]
        public int BillingCycleSequence { get; set; }
        [JsonProperty("pricing_scheme")]
        public PricingScheme PricingScheme { get; set; }
        [JsonProperty("roll_out_strategy")]
        public RollOutStrategy RollOutStrategy { get; set; }
        [JsonProperty("pricing_model")]
        public string Pricing_Model { get; set; }
        [JsonProperty("tiers")]
        public List<Tier> Tiers { get; set; }
    }
}
