using Microsoft.AspNetCore.Mvc;
using QuizletTerminology.Models;
using QuizletTerminology.ViewModels;

namespace QuizletTerminology.Respository
{
    public interface ITermRespository
    {
        #region User
        IEnumerable<NGUOIDUNG> GetNGUOIDUNG();
        Task<List<UserManagerViewModel>> GetUserManagers();
        Task<bool> UpdateUserState(UserState user);
        Task<NGUOIDUNG> GetByMA_USER(int UserId);
        Task<NGUOIDUNG> GetUserByLogin(string Gmail, string Password);
        Task<bool> HasDuplicateEmail(string Gmail);
        Task<bool> Create(NGUOIDUNG nguoidung);
        Task<bool> Update(NGUOIDUNG nguoidung);
        Task<bool> ChangePassword(ChangePasswordViewModel model);
        Task<bool> ChangeGmail(ChangeGmailViewModel model);
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string userInput);
        Task<bool> Delete(int MA_USER);
        NGUOIDUNG IsEmailExist(string email);
        string GenerateRandomPassword(int length);
        string CreateGmailCode(string email);
        Task<bool> HandleForgetPassword(ForgetPasswordViewModel model);
        void HandleSendingDataToEmail(string toEmail, string subject, string body);

        #endregion

        #region Title
        Task<List<CHUDE>> GetCHUDEByUserId(int UserId);
        Task<CHUDE> GetCHUDEByTitleId(int TitleId);
        Task<bool> UpdateCHUDE(CHUDE chude);
        bool HasDuplicateTitleNamePerUserForUpdatee(int titleId, int userId, string titleName);
        Task<bool> HasDuplicateTitleNamePerUser(int userId, string titleName);
        Task<bool> HasDuplicateTitleNamePerUserForUpdate(int titleId, int userId, string titleName);
        IEnumerable<CHUDE> GetCHUDE();
        Task<bool> CreateCHUDE(CHUDE chude);
        Task<bool> DeleteCHUDE(int TitleId);


        #endregion

        #region Module
        IEnumerable<ClassLearningModuleViewModel> GetHOCPHANByListId(LearningModuleIdList idList);
        Task<int> CountTerms(int learningModuleId);
        IEnumerable<HOCPHAN> GetHOCPHANByTitleId(int TitleId);
        Task<HOCPHAN> GetHOCPHANByLearningModuleId(int learningModuleId);
        Task<bool> CreateHOCPHAN(HOCPHAN hocphan);
        bool HasDuplicatedTitleNamePerUser(int titleId, string learningModuleName);
        Task<bool> DeleteHOCPHAN(int learningModuleId);
        bool HasTerminologies(int learningModuleId);
        Task<bool> UpdateHOCPHAN(HOCPHAN hocphan);
        bool HasDuplicateModuleNamePerUserForUpdate(int learningModuleId, int titleId, string learningModuleName);
        //bool HasDuplicateTitleNamePerUserForUpdate(int learningModuleId, int titleId, string learningModuleName);
        Task<List<HOCPHAN>> GetHOCPHANOfUser(int userId);
        Task<int> CopyModule(CopyViewModel model);
        bool CheckDuplicateModuleName(int titleId, HOCPHAN hocphan);
        void CopyLearningModule(HOCPHAN fromHOCPHAN, HOCPHAN toHOCPHAN);
        void CopyTerminology(THETHUATNGU fromTHETHUATNGU, THETHUATNGU toTHETHUATNGU);
        #endregion

        #region Term
        Task<List<LevelTerms>> GetLevelTerms(int userId);
        Task<int> CountNumberTermsForLevel(int levelId, int userId);
        IEnumerable<THETHUATNGU> GetTHETHUATNGUByTitleId(int learningModuleId);
        Task<THETHUATNGU> GetTHUATNGUByLearningModuleId(int termId);
        Task<bool> CreateTHUATNGU(THETHUATNGU thuatngu);
        bool HasDuplicatedTermNamePerLearningModule(int learningModuleId, string termName);
        Task<bool> DeleteTHUATNGU(int termId);
        Task<bool> UpdateTHUATNGU(THETHUATNGU thuatngu);
        bool HasDuplicateTermNamePerLearningModuleForUpdate(int termId, int learningModuleId, string termName);
        void Shuffle<T>(List<T> list);
        List<ObjectivePack> GetObjectiveList(int learningModuleId);
        Task<bool> UpdateTHUATNGUTest(ResultQuestion resultQuestion);
        string ExtractFileNameFromUrl(string url);
        Task<AchieveLibrary> GetAchieveLibrary(int userId);
        Task<List<LEVELGHINHO>> GetListLEVELGHINHO();
        Task<LEVELGHINHO> GetLEVELGHINHO(int levelId);
        Task<bool> UpdateLEVELGHINHO(LEVELGHINHO level);
        #endregion
    }
}
