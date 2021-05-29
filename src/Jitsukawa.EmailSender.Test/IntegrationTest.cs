using Jitsukawa.EmailSender.Api;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jitsukawa.EmailSender.Test
{
    public class IntegrationTest : IClassFixture<WebAppFactory<Startup>>
    {
        protected readonly WebAppFactory<Startup> factory;
        public IntegrationTest(WebAppFactory<Startup> factory)
        {
            this.factory = factory;
        }

        protected HttpClient CreateClient(string apiKey)
        {
            var client = factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ApiKey", apiKey);
            return client;
        }

        protected static async Task<(HttpStatusCode Code, string Response)> Get(string url, HttpClient client)
        {
            var result = await client.GetAsync(url);
            var response = await result.Content.ReadAsStringAsync();

            return (result.StatusCode, response);
        }

        protected static async Task<(HttpStatusCode Code, string Response)> Post(string json, string url, HttpClient client)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PostAsync(url, content);
            var response = await result.Content.ReadAsStringAsync();

            return (result.StatusCode, response);
        }

        protected static async Task<(HttpStatusCode Code, string Response)> Put(string json, string url, HttpClient client)
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await client.PutAsync(url, content);
            var response = await result.Content.ReadAsStringAsync();

            return (result.StatusCode, response);
        }

        protected static async Task<(HttpStatusCode Code, string Response)> Delete(string url, HttpClient client)
        {
            var result = await client.DeleteAsync(url);
            var response = await result.Content.ReadAsStringAsync();

            return (result.StatusCode, response);
        }
    }
}
