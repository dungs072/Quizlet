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
    }
}
