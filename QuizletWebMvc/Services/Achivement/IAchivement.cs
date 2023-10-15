using QuizletWebMvc.ViewModels.Achivement;

namespace QuizletWebMvc.Services.Achivement
{
    public interface IAchivement
    {
        Task<List<LevelTerms>> GetLevelTerm(int UserId);
    }
}
