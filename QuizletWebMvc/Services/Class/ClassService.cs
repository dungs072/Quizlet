using QuizletWebMvc.ViewModels.Class;
using QuizletWebMvc.ViewModels.Terminology;
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
    }
}
