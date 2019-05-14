using ListerMobile.Models;
using ListerMobile.RestClient;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ListerMobile.Services
{
    public class UsersServices
    {
        private const string USERS_WEB_SERVICE_PATH = "/api/Users/";

        public async Task<ObservableCollection<User>> GetUsersAsync()
        {
            RestClient<User> restClient = new RestClient<User>();
            var users = await restClient.GetAsync(USERS_WEB_SERVICE_PATH);
            return users;
        }

        public async Task DeleteUserAsync(int id)
        {
            RestClient<User> restClient = new RestClient<User>();
            var isUserDeleted = await restClient.DeleteAsync(id, USERS_WEB_SERVICE_PATH);
        }

        public async Task PostUserAsync(User user)
        {
            RestClient<User> restClient = new RestClient<User>();
            var IsUserPosted = await restClient.PostAsync(user, USERS_WEB_SERVICE_PATH);
        }

        public async Task PutUserAsync(int id, User user)
        {
            RestClient<User> restClient = new RestClient<User>();
            var IsUserUpdated = await restClient.PutAsync(id, user, USERS_WEB_SERVICE_PATH);
        }
    }
}
