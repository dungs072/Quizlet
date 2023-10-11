using Microsoft.AspNetCore.Components;
using QuizletWebMvc.ViewModels.User;
using System.Net.Http;
using System.Text.Json;
namespace QuizletWebMvc.Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient client;
        public LoginService(HttpClient client)
        {
            this.client = client;
            
        }

        public async Task<UserAccountViewModel> FindAccount(string username, string password)
        {
            var user = await client.GetFromJsonAsync<UserAccountViewModel>(API.API.UserUrl + $"/{username}/{password}");
            return user;

        }

        public async Task<bool> HasDuplicateGmail(string gmail)
        {
            HttpResponseMessage response = await client.GetAsync(API.API.UserUrlCheck + $"{gmail}");
            string jsonContent = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonContent))
            {
                bool isDuplicated = JsonSerializer.Deserialize<bool>(jsonContent);
                return isDuplicated;
            }
            return true;

        }

        public async Task RegisterUser(UserAccountViewModel userAccountView)
        {
            await client.PostAsJsonAsync<UserAccountViewModel>(API.API.UserUrl, userAccountView);
        }
    }
}
