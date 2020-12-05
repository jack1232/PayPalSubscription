using System;
using Newtonsoft.Json;

namespace PayPalSubscriptionNetSdk.Subscriptions
{
    public class RollOutStrategy
    {
        [JsonProperty("effective_time")]
        public string EffectiveTime { get; set; }
        [JsonProperty("process_change_from")]
        public string ProcessChangeFrom { get; set; }
    }
}
