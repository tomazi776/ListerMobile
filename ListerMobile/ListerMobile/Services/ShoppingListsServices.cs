using ListerMobile.Models;
using ListerMobile.RestClient;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ListerMobile.Services
{
    public class ShoppingListsServices
    {
        public async Task<ObservableCollection<ShoppingList>> GetShoppingListsAsync()
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var shoppingLists = await restClient.GetAsync();
            return shoppingLists;
        }

        public async Task DeleteShoppingListAsync(int id)
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var shoppingLists = await restClient.DeleteAsync(id);
        }

        public async Task PostShoppingListAsync(ShoppingList shoppingList)
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var shoppingLists = await restClient.PostAsync(shoppingList);
        }

        public async Task PutShoppingListAsync(int id, ShoppingList shoppingList)
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var shoppingLists = await restClient.PutAsync(id, shoppingList);
        }
    }
}
