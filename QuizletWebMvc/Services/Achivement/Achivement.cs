using QuizletWebMvc.ViewModels.Achivement;

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
    }
}
