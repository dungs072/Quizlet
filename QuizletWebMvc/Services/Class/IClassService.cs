using QuizletWebMvc.ViewModels.Class;
using QuizletWebMvc.ViewModels.Terminology;

namespace QuizletWebMvc.Services.Class
{
    public interface IClassService
    {
        Task<List<ClassViewModel>> GetClassesByUser(int UserId);
        Task<ClassViewModel> GetClass(int classId);
        Task<bool> CreateClass(ClassViewModel classViewModel);
        Task<bool> UpdateClass(ClassViewModel classViewModel);
        Task<bool> DeleteClass(int classId);

        Task<List<ClassLearningModuleViewModel>> GetDetailLearningModuleClass(int classId);
        Task<List<TitleChoiceViewModel>> GetTitleDatas(int userId);
        Task<List<LearningModuleViewModel>> GetModuleDatas(int classId, int titleId);
        Task<bool> AddModuleToClass(LearningModuleDetail detail);
        Task<bool> DeleteModuleDetail(int classId, int moduleId);

        Task<List<Participant>> GetDetailParticipantClass(int classId);
        Task<List<UserParticipant>> GetUserParticipant(int classId, string search, int currentUserId);
        Task<bool> AddParticipantToClass(RegisterDetailClass detail);
        Task<bool> DeleteParticipantFromClass(int classId, int userId);

        Task<List<Participant>> GetDetailPendingParticipantClass(int classId);
        Task<bool> UpdateRegisterDetail(RegisterDetailClass detail);
        Task<RegisterDetailClass> GetDetailPendingParticipant(int classId, int userId);

        Task<List<RegisterClass>> GetRegisterClass(int userId, string search);
    }
}
