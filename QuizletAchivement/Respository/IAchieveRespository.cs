using Microsoft.AspNetCore.Mvc;
using QuizletAchivement.Models;
using QuizletAchivement.ViewModels;

namespace QuizletAchivement.Respository
{
    public interface IAchieveRespository
    {
        #region Achivement
        Task<List<THANHTUU>> GetTHANHTUU();
        Task<THANHTUU> GetTHANHTUUById(int AchivementId);
        Task<bool> CreateTHANHTUU(THANHTUU thanhtuu);
        Task<bool> UpdateTHANHTUU(THANHTUU thanhtuu);
        Task<bool> DeleteTHANHTUU(int AchivementId);
        string ExtractFileNameFromUrl(string url);
        #endregion

        #region UserAchieve
        Task<AchieveStatistics> GetAchieveStatistics(int userId);
        Task GetLibraryStatistics(AchieveStatistics statistics, int userId);
        Task GetClassStatistics(AchieveStatistics statistics, int userId);
        Task GetSequenceStatistics(AchieveStatistics statistics, int userId);
        Task<List<string>> GetSequenceCalender(int userId);
        Task<bool> MarkAttendance(MarkAttendance mark);
        bool IsMarked(int userId);
        Task<List<Badge>> GetBadges(int userId);
        Task<int> GetLength();
        bool IsAchieve(int achievementId, int userId, Badge badge);
        Task<AchivementBadge> GetUpdateBadge(int userId, string typeBadge);
        Task<int> GetParticipantInAllClass(int userId);
        bool CheckIsExistBadge(int userId, int badgeId);
        Task<bool> AddUpdateBadge(AchieveBadge achieveBadge);
        #endregion
    }
}
