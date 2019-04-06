using ListerMobile.Models;
using ListerMobile.RestClient;
using System.Collections.Generic;
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
    }
}
