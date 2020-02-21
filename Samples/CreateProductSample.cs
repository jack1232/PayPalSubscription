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
            // Construct a Product object
            var product = new Product
            {
                Name = "Video Streaming Service",
                Description = "Video streaming service",
                Type = ProductTypeEnum.SERVICE,
                Category = ProductCategoryEnum.SOFTWARE,
                ImageUrl = "https://example.com/streaming.jpg",
                HomeUrl = "https://example.com/home"
            };

            // call API using the static method ProductCreate() of the SDK and get a response for your call
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
