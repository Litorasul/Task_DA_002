using System.Text.Json;
using System.Text;

namespace API_Template.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;

        public HttpService(IHttpClientFactory httpClientFactory, string baseUrl)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _baseUrl = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
        }

        private HttpClient CreateClient() => _httpClientFactory.CreateClient();

        public async Task<T> GetAsync<T>(string endpoint)
        {
            return await SendRequestAsync<T>(() => CreateClient().GetAsync($"{_baseUrl}{endpoint}"));
        }

        public async Task<T> PostAsync<T>(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await SendRequestAsync<T>(() => CreateClient().PostAsync($"{_baseUrl}{endpoint}", content));
        }

        public async Task<T> PutAsync<T>(string endpoint, object data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await SendRequestAsync<T>(() => CreateClient().PutAsync($"{_baseUrl}{endpoint}", content));
        }

        public async Task DeleteAsync(string endpoint)
        {
            await SendRequestWithoutResponseAsync(() => CreateClient().DeleteAsync($"{_baseUrl}{endpoint}"));
        }

        private async Task<T> SendRequestAsync<T>(Func<Task<HttpResponseMessage>> httpRequest)
        {
            try
            {
                var response = await httpRequest();
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Error occurred during the request: {ex.Message}", ex);
            }
        }

        private async Task SendRequestWithoutResponseAsync(Func<Task<HttpResponseMessage>> httpRequest)
        {
            try
            {
                var response = await httpRequest();
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new HttpRequestException($"Error occurred during the request: {ex.Message}", ex);
            }
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorMessage}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent);
        }
    }
}
