using QuizletWebMvc.ViewModels.Class;
using QuizletWebMvc.ViewModels.Terminology;

namespace QuizletWebMvc.Services.Class
{
    public interface IClassService
    {
        Task<List<ClassViewModel>> GetClassesByUser(int UserId);
        Task<bool> CreateClass(ClassViewModel classViewModel);
        Task<bool> UpdateClass(ClassViewModel classViewModel);
    }
}
