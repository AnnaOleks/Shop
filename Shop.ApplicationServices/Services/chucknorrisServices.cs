using Shop.Core.Dto;
using Shop.Core.ServiceInterface;
using System.Net.Http;
using System.Text.Json;

namespace Shop.ApplicationServices.Services
{
    public class chucknorrisServices : IchucknorrisServices
    {
        private readonly HttpClient _httpClient;

        public chucknorrisServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<chucknorrisDto> GetRandomAsync()
        {
            string apiUrl = "";

            // Делаем запрос к API
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            // Читаем ответ
            var json = await response.Content.ReadAsStringAsync();

            // Десериализация
            var joke = JsonSerializer.Deserialize<chucknorrisDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return joke;
        }
    }
}
