using Firebase.Auth;
using QuizletWebMvc.ViewModels.Achivement;
using QuizletWebMvc.ViewModels.Class;
using System.Net;

namespace QuizletWebMvc.Services.Achivement
{
    public class Achivement:IAchivement
    {
        private readonly HttpClient client;
        public Achivement(HttpClient client)
        {
            this.client = client;
        }
        public async Task<List<LevelTerms>> GetLevelTerm(int UserId)
        {
            return await client.GetFromJsonAsync<List<LevelTerms>>(API.API.AchivementUser + $"/{UserId}");
        }
        public async Task<AchieveStatistics> GetAchieveStatistics(int UserId)
        {
            return await client.GetFromJsonAsync<AchieveStatistics>(API.API.AchieveStatistics + $"/{UserId}");
        }

        public async Task<List<string>> GetSequenceDates(int userId)
        {
            return await client.GetFromJsonAsync<List<string>>(API.API.SequenceCalender + $"/{userId}");
        }
        public async Task<bool> MarkAttendance(MarkAttendance mark)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<MarkAttendance>(API.API.MarkAttendance, mark);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<List<Badge>> GetBadges(int UserId)
        {
            return await client.GetFromJsonAsync<List<Badge>>(API.API.Badges + $"/{UserId}");
        }
        public async Task<AchivementBadge> AchieveBadge(int userId, string typeBadge)
        {
            return await client.GetFromJsonAsync<AchivementBadge>(API.API.AchieveBadge + $"/{userId}/{typeBadge}");
        }
        public async Task<bool> AddUpdateAchieve(AchieveBadge detail)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<AchieveBadge>(API.API.AchieveBadge, detail);
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
