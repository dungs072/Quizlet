using QuizletWebMvc.ViewModels.Achivement;
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
        public async Task<bool> MarkAttendance(int userId)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<int>(API.API.MarkAttendance, userId);
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
