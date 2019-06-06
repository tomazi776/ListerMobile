﻿using ListerMobile.Models;
using ListerMobile.RestClient;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ListerMobile.Services
{
    public class ShoppingListsService
    {
        private const string SHOPPING_LISTS_WEB_SERVICE_PATH = "/api/ShoppingLists/";
        private const string USERS_WEB_SERVICE_PATH = "/api/Users/";

        public async Task<ObservableCollection<ShoppingList>> GetShoppingListsAsync()
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var shoppingLists = await restClient.GetAsync(SHOPPING_LISTS_WEB_SERVICE_PATH);
            return shoppingLists;
        }

        public async Task<ObservableCollection<ShoppingList>> GetUserShoppingListsAsync(string username)
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var userShoppingLists = await restClient.GetAsync(SHOPPING_LISTS_WEB_SERVICE_PATH, username);
            return userShoppingLists;
        }

        public async Task DeleteShoppingListAsync(int id)
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var shoppingLists = await restClient.DeleteAsync(id, SHOPPING_LISTS_WEB_SERVICE_PATH);
        }

        public async Task PostShoppingListAsync(ShoppingList shoppingList)
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var shoppingLists = await restClient.PostAsync(shoppingList, SHOPPING_LISTS_WEB_SERVICE_PATH);
        }

        public async Task PutShoppingListAsync(int id, ShoppingList shoppingList)
        {
            RestClient<ShoppingList> restClient = new RestClient<ShoppingList>();
            var shoppingLists = await restClient.PutAsync(id, shoppingList, SHOPPING_LISTS_WEB_SERVICE_PATH);
        }
    }
}
