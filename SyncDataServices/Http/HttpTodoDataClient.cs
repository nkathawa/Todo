using System.Text;
using System.Text.Json;
using Todo.Dtos;

namespace Todo.SyncDataServices.Http
{
    public class HttpTodoDataClient : ITodoDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpTodoDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<HttpResponseMessage> SendTodoItemFromWebToDb(TodoItemCreateDto createDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(createDto),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync($"{_configuration["TodoApi"]}", httpContent);

            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to Todo was OK");
            }
            else
            {
                Console.WriteLine("--> Sync POST to Todo was not OK");
            }
            return response;
        }
    }
}