using Microsoft.AspNetCore.Components;
using QuizletWebMvc.ViewModels.User;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
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
        public async Task<UserAccountViewModel> GetProfile(int userId)
        {
            var user = await client.GetFromJsonAsync<UserAccountViewModel>(API.API.UserUrl + $"/{userId}");
            return user;
        }
        public async Task<bool> UpdateProfile(UserAccountViewModel user)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<UserAccountViewModel>(API.API.UserUrl, user);
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> ChangePassword(ChangePasswordViewModel model)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<ChangePasswordViewModel>(API.API.UserChangePassword, model);
            if(response.StatusCode == HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;

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
        public async Task<string> GetEmailCode(string email)
        {
            return await client.GetStringAsync(API.API.UserEmailExist+$"/{email}");
        }

        public async Task<bool> HandleForgetPassword(ForgetPasswordViewModel model)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<ForgetPasswordViewModel>(API.API.UserForgetPassword, model);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return false;
            }
            return true;
        }
    }
}
