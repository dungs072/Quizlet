using Microsoft.AspNetCore.Mvc;
using QuizletClass.Models;
using QuizletClass.ViewModels;

namespace QuizletClass.Respository
{
    public interface IClassRespository
    {
        #region ForService
        Task<AchieveClass> GetAchieveClass(int userId);
        Task<int> CountAllParticipants(int userId);
        #endregion

        #region Class
        Task<IEnumerable<ClassViewModel>> GetLOP(int userId);
        Task<List<CHITIETDANGKILOP>> GetCHITIETDANGKILOPS(int classId);
        Task<List<CHITIETHOCPHAN>> GetCHITIETHOCPHANS(int classId);
        Task<ClassViewModel> GetLOPById(int ClassId);
        Task<bool> CreateLOP(ClassViewModel classView);
        bool HasDuplicateClassName(int userId, string className);
        Task<bool> UpdateLOP(ClassViewModel lop);
        bool HasDuplicateClassNameForUpdate(int userId, int classId, string className);
        Task<bool> DeleteLOP(int ClassId);

        #endregion

        #region DetailLearningModuleClass
        Task<IEnumerable<ClassLearningModuleViewModel>> GetLearningModuleClassDetail(int classId);
        IEnumerable<TitleViewModel> GetYourTitleData(int userId);
        Task<IEnumerable<ModuleDetailWithList>> GetYourModuleData(int classId, int titleId);
        bool CheckLearningModuleIsRegistered(int classId, int learningModuleId);
        Task<bool> AddModulesForClass(LearningModuleDetail learningModuleDetail);
        Task<bool> DeleteModuleDetail(int classId, int learningModuleId);

        #endregion

        #region Participants
        Task<IEnumerable<Participant>> GetParticipant(int classId);
        Task<IEnumerable<Participant>> GetPendingParticipant(int classId);
        Task<IEnumerable<MessageClassRegistration>> GetMessagePendingParticipant(int userId);
        Task<IEnumerable<UserParticipant>> GetParticipants(int classId, string search, int currentUserId);
        bool CheckUserHasRegisterToClass(int classId, int userId);
        Task<bool> AddParticipant(RegisterClassDetail registerClassDetail);
        Task<bool> DeleteUserParticipant(int classId, int userId);
        Task<bool> UpdateCHITIETDANGKI(RegisterClassDetail registerClassDetail);
        Task<RegisterClassDetail> GetCHITIETDANGKILOP(int classId, int userId);
        #endregion

        #region Register class
        Task<IEnumerable<RegisterClass>> GetRegisterClass(int userId, string search);
        Task<List<LOP>> GetLOPOfModule(int learningModuleId, int userId);
        Task<int> GetNumberTermsInModules(int learningModuleId);
        #endregion

        #region JoinClass
        Task<IEnumerable<ClassViewModel>> GetJoinClass(int userId);
        Task<List<LOP>> GetJoinLOP(int userId);
        #endregion

        #region Delete
        string CanDeleteLearningModule(int learningModuleId, int userId);
        #endregion
    }
}
