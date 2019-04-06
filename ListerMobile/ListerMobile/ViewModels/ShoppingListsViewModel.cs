using ListerMobile.Models;
using ListerMobile.Services;
using ListerMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class ShoppingListsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public ObservableCollection<ShoppingList> MyOldShoppingLists { get; set; }
        public ObservableCollection<ShoppingList> ReceivedShoppingLists { get; set; } = new ObservableCollection<ShoppingList>();
        public Command LoadItemsCommand { get; set; }

        private List<ShoppingList> _myShoppingLists;
        public List<ShoppingList> MyShoppingLists
        {
            get { return _myShoppingLists; }
            set { SetProperty(ref _myShoppingLists, value); }
        }



        public ShoppingListsViewModel()
        {
            Title = "Moje Listy";
            MyOldShoppingLists = new ObservableCollection<ShoppingList>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewShoppingListPage, ShoppingList>(this, "AddShoppingList", async (obj, item) =>
            {
                var newShoppingList = item as ShoppingList;
                MyOldShoppingLists.Add(newShoppingList);

                //MakeHighlightBody();

                await DataStore.AddItemAsync(newShoppingList);
            });

            //InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            var shoppingListsServices = new ShoppingListsServices();

            MyShoppingLists = await shoppingListsServices.GetShoppingListsAsync();
            var aaa = "ddd";
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
                MyOldShoppingLists.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    MyOldShoppingLists.Add(item);
                    ReceivedShoppingLists.Add(item);
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

        //async Task ExecuteDeleteItemCommand()
        //{
        //    try
        //    {
        //        var items = await DataStore.DeleteItemAsync()
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}
