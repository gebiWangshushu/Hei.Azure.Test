using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hei.Infrastructure
{
    public class AzureTranslate
    {
        private static readonly string key = "e005c6b471034c41ac029a485db48a8c";

        private static readonly string endpoint = "https://api.cognitive.microsofttranslator.com";
        //  private static readonly string endpoint = "https://api.translator.azure.cn";

        public async Task Execute(string textToTranslate, string fromLang = "en", string targetLang = "cn")
        {
            // Input and output languages are defined as parameters.
            string route = $"/translate?api-version=3.0&from={fromLang}&to={targetLang}";
            //string route = "/translate?api-version=3.0&from=en&to=fr&to=zu";
            //string textToTranslate = "I would really like to drive your car around the block a few times!";
            object[] body = new object[] { new { Text = textToTranslate } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(endpoint + route);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                // Send the request and get response.
                HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                // Read response as a string.
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
            }
        }
    }
}