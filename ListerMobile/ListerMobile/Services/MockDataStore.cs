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
        List<string> ProductsOfFirstList = new List<string>();
        List<string> ProductsOfSecondList = new List<string>();

        public MockDataStore()
        {
            User user1 = new User { Id = 1, Name = "Tomala" };

            Product[] FirstListProducts = new Product[]
            {
                 new Product("- Kupak") ,
                 new Product("- Kapusta"),
                 new Product("- Czekolada"),

                 new Product("- CZYRAK"),
                 new Product("- PUCIO"),
                 new Product("- LUL")

            };

            Product[] SecondListProducts = new Product[]
            {

                 new Product("- Ser") ,
                 new Product("- Koperek"),
                 new Product("- Śluz"),

                 new Product("- Żyletki"),
                 new Product("- Wino"),
                 new Product("- Świece zapachowe")

            };


            foreach (var item in FirstListProducts)
            {
                ProductsOfFirstList.Add(item.Name.ToString());
            }

            foreach (var item in SecondListProducts)
            {
                ProductsOfSecondList.Add(item.Name.ToString());
            }


            shoppingLists = new List<ShoppingList>();
            var mockItems = new List<ShoppingList>
            {
                new ShoppingList { Id =1, CreationDate = DateTime.Today, Name = "Poniedzialek 04.03.19" ,Products = FirstListProducts , Body = string.Join("" + Environment.NewLine, ProductsOfFirstList.ToArray()),  BodyHighlight = TakeElements(ProductsOfFirstList)  , User = user1  },
                new ShoppingList { Id =2, CreationDate = DateTime.Today, Name = "Wtorek 05.03.19",Products = SecondListProducts, Body = string.Join("" + Environment.NewLine, ProductsOfSecondList.ToArray()) , BodyHighlight = TakeElements(ProductsOfSecondList), User = user1  },
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

        private void MakeBody(IEnumerable<string> fromList)
        {
            string.Join("" + Environment.NewLine, fromList.ToArray());
        }


        private string TakeElements(List<string> list)
        {
            var firstFewElements = list.Take(3);
            return firstFewElements.ElementAt(0) + Environment.NewLine + firstFewElements.ElementAt(1) + Environment.NewLine + firstFewElements.ElementAt(2) + Environment.NewLine;
        }
    }
}