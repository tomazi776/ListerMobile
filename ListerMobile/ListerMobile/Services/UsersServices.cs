using ListerMobile.Models;
using ListerMobile.RestClient;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ListerMobile.Services
{
    public class UsersServices
    {
        public async Task<ObservableCollection<User>> GetUsersAsync()
        {
            RestClient<User> restClient = new RestClient<User>();
            var users = await restClient.GetAsync();
            return users;
        }

        public async Task DeleteUserAsync(int id)
        {
            RestClient<User> restClient = new RestClient<User>();
            var isUserDeleted = await restClient.DeleteAsync(id);
        }

        public async Task PostUserAsync(User user)
        {
            RestClient<User> restClient = new RestClient<User>();
            var IsUserPosted = await restClient.PostAsync(user);
        }

        public async Task PutUserAsync(int id, User user)
        {
            RestClient<User> restClient = new RestClient<User>();
            var IsUserUpdated = await restClient.PutAsync(id, user);
        }
    }
}
