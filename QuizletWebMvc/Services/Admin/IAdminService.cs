using QuizletWebMvc.ViewModels.Admin;

namespace QuizletWebMvc.Services.Admin
{
    public interface IAdminService
    {
        Task<List<LevelTerm>> GetLevelTerm();
        Task<LevelTerm> GetLevelTerm(int levelId);
        Task<bool> UpdateLevelTerm(LevelTerm levelTerm);

        Task<List<Badge>> GetBadges();
        Task<Badge> GetBadge(int achievementId);
        Task<bool> UpdateBadge(Badge badge);
        Task<bool> CreateBadge(Badge badge);
    }
}
