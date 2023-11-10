using QuizletWebMvc.ViewModels.Admin;
using System.Net;

namespace QuizletWebMvc.Services.Admin
{
    public class AdminService:IAdminService
    {
        private readonly HttpClient client;
        public AdminService(HttpClient client)
        {
            this.client = client;
        }
        public async Task<List<LevelTerm>> GetLevelTerm()
        {
            return await client.GetFromJsonAsync<List<LevelTerm>>(API.API.AdminLevelTermUrl);
        }

        public async Task<LevelTerm> GetLevelTerm(int levelId)
        {
            return await client.GetFromJsonAsync<LevelTerm>(API.API.AdminLevelTermUrl + $"/{levelId}");
        }
        public async Task<bool> UpdateLevelTerm(LevelTerm levelTerm)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<LevelTerm>(API.API.AdminLevelTermUrl, levelTerm);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
