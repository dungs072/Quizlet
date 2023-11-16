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

        public async Task<List<Badge>> GetBadges()
        {
            return await client.GetFromJsonAsync<List<Badge>>(API.API.AchivementUrl);
        }

        public async Task<Badge> GetBadge(int achievementId)
        {
            return await client.GetFromJsonAsync<Badge>(API.API.AchivementUrl + $"/{achievementId}");
        }
        public async Task<bool> UpdateBadge(Badge badge)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<Badge>(API.API.AchivementUrl, badge);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<bool> CreateBadge(Badge badge)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<Badge>(API.API.AchivementUrl, badge);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<bool> DeleteBadge(int achievementId)
        {
            HttpResponseMessage response = await client.DeleteAsync(API.API.AchivementUrl + $"/{achievementId}");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }

        public async Task<List<UserManagerViewModel>> GetUserManagers()
        {
            return await client.GetFromJsonAsync<List<UserManagerViewModel>>(API.API.UserManager);
        }
        public async Task<bool> UpdateUserState(UserState userState)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<UserState>(API.API.UserState, userState);
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
