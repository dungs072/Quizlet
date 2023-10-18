using QuizletWindows.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuizletWindows.API
{
    public class UserApi
    {
        private HttpClient httpClient;
        private static readonly Lazy<UserApi> lazyInstance = new Lazy<UserApi>(() => new UserApi());
        public static UserApi Instance {  get { return lazyInstance.Value; } } 
        public void SetHttpClient (HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<List<UserViewModel>> GetUsers()
        {
            var users = await httpClient.GetFromJsonAsync<List<UserViewModel>>(Api.UserUrl);
            return users;
        }
    }
}
