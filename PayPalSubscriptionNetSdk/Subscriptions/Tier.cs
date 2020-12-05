using System;
using Newtonsoft.Json;

namespace PayPalSubscriptionNetSdk.Subscriptions
{
    public class Tier
    {
        [JsonProperty("starting_quantity")]
        public string StartingQuantity { get; set; }
        [JsonProperty("ending_quantity")]
        public string EndingQuantity { get; set; }
        [JsonProperty("amount")]
        public Currency Amount { get; set; }
    }
}
