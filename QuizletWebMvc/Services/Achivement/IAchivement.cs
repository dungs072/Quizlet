using QuizletWebMvc.ViewModels.Achivement;

namespace QuizletWebMvc.Services.Achivement
{
    public interface IAchivement
    {
        Task<List<LevelTerms>> GetLevelTerm(int UserId);
        Task<AchieveStatistics> GetAchieveStatistics(int UserId);
        Task<List<string>> GetSequenceDates(int userId);
        Task<bool> MarkAttendance(int userId);
    }
}
