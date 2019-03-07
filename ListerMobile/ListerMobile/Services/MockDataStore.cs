using ListerMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListerMobile.Services
{
    public class MockDataStore : IDataStore<ShoppingList>
    {
        List<ShoppingList> shoppingLists;

        public MockDataStore()
        {
            User user1 = new User { Id = 1, Name = "Tomala" };

            shoppingLists = new List<ShoppingList>();
            var mockItems = new List<ShoppingList>
            {
                new ShoppingList { Id =1, CreationDate = DateTime.Today, Name = "Poniedzialek 04.03.19" , Body = "Mleko /n Kapusta /n Mamba /n Kał ludzki", User = user1  },
                new ShoppingList { Id =2, CreationDate = DateTime.Today, Name = "Wtorek 05.03.19", Body = "Ser /n Koperek /n Śluz /n  Żyletki", User = user1  },
                //new Item { Id = Guid.NewGuid().ToString(), Text = "Fifth item", Description="This is an item description." },
                //new Item { Id = Guid.NewGuid().ToString(), Text = "Sixth item", Description="This is an item description." },
            };

            foreach (var item in mockItems)
            {
                shoppingLists.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(ShoppingList item)
        {
            shoppingLists.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(ShoppingList item)
        {
            var oldItem = shoppingLists.Where((ShoppingList arg) => arg.Id == item.Id).FirstOrDefault();
            shoppingLists.Remove(oldItem);
            shoppingLists.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var oldItem = shoppingLists.Where((ShoppingList arg) => arg.Id == id).FirstOrDefault();
            shoppingLists.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ShoppingList> GetItemAsync(int id)
        {
            return await Task.FromResult(shoppingLists.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ShoppingList>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(shoppingLists);
        }
    }
}