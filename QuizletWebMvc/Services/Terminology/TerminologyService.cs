
using QuizletWebMvc.ViewModels.Terminology;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace QuizletWebMvc.Services.Terminology
{
    public class TerminologyService : ITerminologyService
    {
        private readonly HttpClient client;
        public TerminologyService(HttpClient client)
        {
            this.client = client;
        }

        #region Title
        public async Task<List<TitleViewModel>> GetTitles()
        {
            return await client.GetFromJsonAsync<List<TitleViewModel>>(API.API.TitleUrl);
        }
        public async Task<List<TitleViewModel>> GetTitlesBaseOnUserId(int UserId)
        {
            return await client.GetFromJsonAsync<List<TitleViewModel>>(API.API.TitleUrlUser + $"{UserId}");
        }
        public async Task CreateTitle(TitleViewModel titleViewModel)
        {
            await client.PostAsJsonAsync<TitleViewModel>(API.API.TitleUrl,titleViewModel);
        }
        public async Task<bool> HasDuplicateTitlePerUser(int userId, string titleName)
        {
            HttpResponseMessage response = await client.GetAsync(API.API.TitleUrlCheck + $"{userId}/{titleName}");
            string jsonContent = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonContent))
            {
                bool isDuplicated = JsonSerializer.Deserialize<bool>(jsonContent);
                return isDuplicated;
            }
            return true;

        }
        public async Task<bool> HasDuplicateTitlePerUserForUpdate(int titleId, int userId, string titleName)
        {
            HttpResponseMessage response = await client.GetAsync(API.API.TitleUrlCheck + $"{titleId}/{userId}/{titleName}");
            string jsonContent = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonContent))
            {
                bool isDuplicated = JsonSerializer.Deserialize<bool>(jsonContent);
                return isDuplicated;
            }
            return true;
        }
        public async Task<TitleViewModel> GetTitleViewModel(int titleId)
        {

            var title = await client.GetFromJsonAsync<TitleViewModel>(API.API.TitleUrlFind + $"{titleId}");
            return title;
        }
        public async Task UpdateTitle(TitleViewModel titleViewModel)
        {
            await client.PutAsJsonAsync<TitleViewModel>(API.API.TitleUrl, titleViewModel);
        }
    

        public async Task DeleteTitle(int TitleId)
        {
            await client.DeleteAsync(API.API.TitleUrl+$"/{TitleId}");
        }
        #endregion


        #region LearningModule
        public async Task<LearningModuleViewModel2> GetLearningModuleViewModel(int learningModuleId)
        {
            var learningModule = await client.GetFromJsonAsync<LearningModuleViewModel2>(API.API.LearningModuleUrlFind + $"{learningModuleId}");
            return learningModule;
        }
        public async Task<List<LearningModuleViewModel2>> GetLearningModuleByTitleId(int TitleId)
        {
            return await client.GetFromJsonAsync<List<LearningModuleViewModel2>>(API.API.LearningModuleUrl + $"/{TitleId}");
        }
        public async Task<bool> CreateLearningModule(LearningModuleViewModel2 learningModuleViewModel)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<LearningModuleViewModel2>(API.API.LearningModuleUrl, learningModuleViewModel);
            if (response.StatusCode == HttpStatusCode.BadRequest) 
            {
                return false;
            }
            else 
            {
                return true;
            }

        }
        public async Task<bool> DeleteLearningModule(int LearningModuleId)
        {
            HttpResponseMessage response = await client.DeleteAsync(API.API.LearningModuleUrl + $"/{LearningModuleId}");
            if(response.StatusCode== HttpStatusCode.BadRequest) 
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateLearningModule(LearningModuleViewModel2 learningModuleViewModel)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<LearningModuleViewModel2>(API.API.LearningModuleUrl, learningModuleViewModel);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Term
        public async Task<List<TermViewModel>> GetTermByLearningModuleId(int learningModuleId)
        {
            return await client.GetFromJsonAsync<List<TermViewModel>>(API.API.TermUrl + $"/{learningModuleId}");
        }
        public async Task<bool> CreateTerm(TermViewModel termViewModel)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync<TermViewModel>(API.API.TermUrl, termViewModel);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
        public async Task<bool> DeleteTerm(int termId)
        {
            HttpResponseMessage response = await client.DeleteAsync(API.API.TermUrl + $"/{termId}");
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateTerm(TermViewModel termViewModel)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync<TermViewModel>(API.API.TermUrl, termViewModel);
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return false;
            }
            return true;
        }
        public async Task<TermViewModel> GetTermViewModel(int termId)
        {
            var term = await client.GetFromJsonAsync<TermViewModel>(API.API.TermUrlFind + $"{termId}");
            return term;
        }
        #endregion

        #region Practice & Test
        public async Task<List<ObjectivePack>> GetObjectivePacks(int learningModuleId)
        {
            return await client.GetFromJsonAsync<List<ObjectivePack>>(API.API.TermUrlObjective + $"{learningModuleId}");
        }
        #endregion
    }
}
