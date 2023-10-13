using QuizletWebMvc.ViewModels.Class;
using System.Net;

namespace QuizletWebMvc.Services.Class
{
    public class ClassService : IClassService
    {
        private readonly HttpClient client;
        public ClassService(HttpClient client)
        {
            this.client = client;
        }
        public async Task<List<ClassViewModel>> GetClassesByUser(int UserId)
        {
            return await client.GetFromJsonAsync<List<ClassViewModel>>(API.API.ClassUrl + $"/{UserId}");
        }
        public async Task<ClassViewModel> GetClass(int classId)
        {
            return await client.GetFromJsonAsync<ClassViewModel>(API.API.ClassUrlFind + $"{classId}");
        }
        public async Task<bool> CreateClass(ClassViewModel classViewModel)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<ClassViewModel>(API.API.ClassUrl, classViewModel);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<bool> UpdateClass(ClassViewModel classViewModel)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<ClassViewModel>(API.API.ClassUrl, classViewModel);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> DeleteClass(int classId)
        {
            HttpResponseMessage response = await client.DeleteAsync(API.API.ClassUrl + $"/{classId}");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }

        public async Task<List<ClassLearningModuleViewModel>> GetDetailLearningModuleClass(int classId)
        {
            return await client.GetFromJsonAsync<List<ClassLearningModuleViewModel>>(API.API.ClassDetailOwn + $"{classId}");
        }

        public async Task<List<TitleChoiceViewModel>> GetTitleDatas(int userId)
        {
            return await client.GetFromJsonAsync<List<TitleChoiceViewModel>>(API.API.ClassTitleDetailOwn + $"{userId}");
           
        }
        public async Task<List<LearningModuleViewModel>> GetModuleDatas(int titleId)
        {
            return await client.GetFromJsonAsync<List<LearningModuleViewModel>>(API.API.ClassModuleDetailOwn + $"{titleId}");

        }
    }
}
