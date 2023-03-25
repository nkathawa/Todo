using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Todo.Dtos;
using Todo.Enums;
using Todo.Models;

namespace Todo.SyncDataServices.Http
{
    public class HttpTodoDataClient : ITodoDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;

        public HttpTodoDataClient(HttpClient httpClient, IConfiguration configuration,
            UserManager<ApplicationUser> userManager)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<HttpResponseMessage> SendTodoItemFromWebToDb(TodoItemCreateDto createDto)
        {
            var user = await _userManager.FindByNameAsync("Admin");
            if (user != null)
            {
                var userId = user.Id;
                createDto.UserId = userId;
                createDto.Status = StatusType.OPEN;
                var httpContent = new StringContent(
                    JsonSerializer.Serialize(createDto),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync($"{_configuration["TodoApi"]}", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("--> Sync POST to Todo was OK");
                }
                else
                {
                    Console.WriteLine("--> Sync POST to Todo was not OK");
                }
                return response;
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}