using ListerMobile.Models;
using ListerMobile.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class ShoppingListsViewModel : BaseViewModel
    {
        public ObservableCollection<ShoppingList> ShoppingLists { get; set; } = new ObservableCollection<ShoppingList>();
        public Command LoadItemsCommand { get; set; }

        public ShoppingListsViewModel()
        {
            Title = "Moje Listki";
            //ShoppingLists = new ObservableCollection<ShoppingList>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewShoppingListPage, ShoppingList>(this, "AddShoppingList", async (obj, item) =>
            {
                var newShoppingList = item as ShoppingList;
                ShoppingLists.Add(newShoppingList);

                //MakeHighlightBody();

                await DataStore.AddItemAsync(newShoppingList);
            });
        }



        ///// <summary>
        ///// Zrobić Interfejs do tej metody i tej z MockData
        ///// </summary>
        ///// <param name="shoppingList"></param>
        //private void MakeHighlightBody(ShoppingList shoppingList)
        //{
        //    var body = shoppingList.Body;
        //    body.IndexOf()

        //    return firstFewElements.ElementAt(0) + Environment.NewLine + firstFewElements.ElementAt(1) + Environment.NewLine + firstFewElements.ElementAt(2) + Environment.NewLine;
        //}

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                ShoppingLists.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    ShoppingLists.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
