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
            if (response.StatusCode == HttpStatusCode.NoContent)
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
        public async Task<List<LearningModuleViewModel>> GetModuleDatas(int classId,int titleId)
        {
            return await client.GetFromJsonAsync<List<LearningModuleViewModel>>(API.API.ClassModuleDetailOwn + $"{classId}/{titleId}");

        }
        public async Task<bool> AddModuleToClass(LearningModuleDetail detail)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<LearningModuleDetail>(API.API.ClassModuleAdd, detail);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<bool> DeleteModuleDetail(int classId, int moduleId)
        {
            HttpResponseMessage response = await client.DeleteAsync(API.API.ClassModuleAdd + $"/{classId}/{moduleId}");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Participant>> GetDetailParticipantClass(int classId)
        {
            return await client.GetFromJsonAsync<List<Participant>>(API.API.ClassParticipant + $"/{classId}");
        }

        public async Task<List<UserParticipant>> GetUserParticipant(int classId,string search,int currentUserId)
        {
            return await client.GetFromJsonAsync<List<UserParticipant>>(API.API.ClassParticipantSearch + $"/{classId}/{search}/{currentUserId}");
        }

        public async Task<bool> AddParticipantToClass(RegisterDetailClass detail)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<RegisterDetailClass>(API.API.ClassParticipantAdd, detail);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<bool> DeleteParticipantFromClass(int classId, int userId)
        {
            HttpResponseMessage response = await client.DeleteAsync(API.API.ClassParticipantAdd + $"/{classId}/{userId}");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }
        public async Task<List<Participant>> GetDetailPendingParticipantClass(int classId)
        {
            return await client.GetFromJsonAsync<List<Participant>>(API.API.ClassPendingParticipant + $"/{classId}");
        }

        public async Task<bool> UpdateRegisterDetail(RegisterDetailClass detail)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<RegisterDetailClass>(API.API.ClassPendingParticipant,detail);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }
        public async Task<RegisterDetailClass> GetDetailPendingParticipant(int classId,int userId)
        {
            return await client.GetFromJsonAsync<RegisterDetailClass>(API.API.ClassPendingParticipant + $"/{classId}/{userId}");
        }

        public async Task<List<RegisterClass>> GetRegisterClass(int userId,string search)
        {
            return await client.GetFromJsonAsync<List<RegisterClass>>(API.API.ClassRegister + $"/{userId}/{search}");
        }

        public async Task<List<ClassViewModel>> GetJoinClass(int userId)
        {
            return await client.GetFromJsonAsync<List<ClassViewModel>>(API.API.ClassJoin + $"/{userId}");
        }


        public async Task<int> CopyModule(CopyViewModel copy)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<CopyViewModel>(API.API.ClassCopyModule, copy);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return 2;
            }
            else if(response.StatusCode==HttpStatusCode.NoContent)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

    }
}
