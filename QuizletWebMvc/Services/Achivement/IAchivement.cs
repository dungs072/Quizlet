using QuizletWebMvc.ViewModels.Achivement;

namespace QuizletWebMvc.Services.Achivement
{
    public interface IAchivement
    {
        Task<List<LevelTerms>> GetLevelTerm(int UserId);
        Task<AchieveStatistics> GetAchieveStatistics(int UserId);
        Task<List<string>> GetSequenceDates(int userId);
        Task<bool> MarkAttendance(MarkAttendance mark);
        Task<List<Badge>> GetBadges(int UserId);
        Task<AchivementBadge> AchieveBadge(int userId, string typeBadge);
        Task<bool> AddUpdateAchieve(AchieveBadge detail);
    }
}
