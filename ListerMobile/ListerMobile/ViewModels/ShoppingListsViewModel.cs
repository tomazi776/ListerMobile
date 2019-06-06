using ListerMobile.Models;
using ListerMobile.Services;
using ListerMobile.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class ShoppingListsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private const string SHOPPING_LISTS_PAGE_TITLE = "Moje Listy";
        private bool IsListRemoved { get; set; } = false;

        private ObservableCollection<ShoppingList> _myShoppingLists;
        public ObservableCollection<ShoppingList> MyShoppingLists
        {
            get { return _myShoppingLists; }
            set { SetProperty(ref _myShoppingLists, value); }
        }

        private ObservableCollection<ShoppingList> _receivedShoppingLists;
        public ObservableCollection<ShoppingList> ReceivedShoppingLists
        {
            get { return _receivedShoppingLists; }
            set { SetProperty(ref _receivedShoppingLists, value); }
        }

        public string LoggedUser { get; set; }

        /// <summary>
        /// Initializes Data fetched from server
        /// </summary>
        public ShoppingListsViewModel(INavigation navigation, bool isOtherView = false)
        {
            Title = SHOPPING_LISTS_PAGE_TITLE;
            MyShoppingLists = new ObservableCollection<ShoppingList>();
            GetLoggedUser();
            if (isOtherView)
            {
                Title = "Odebrane Listy";
                ReceivedShoppingLists = new ObservableCollection<ShoppingList>();
                GetReceivedLists();
            }

            Navigation = navigation;

            if (!isOtherView)
            {
                InitializeDataAsync();
            }


            MessagingCenter.Subscribe<NewShoppingListViewModel, ShoppingList>(this, "AddShoppingList", async (obj, item) =>
            {
                try
                {
                    var newShoppingList = item as ShoppingList;
                    var shoppingListsServices = new ShoppingListsService();
                    await shoppingListsServices.PostShoppingListAsync(newShoppingList);
                    MyShoppingLists.Add(newShoppingList);
                }
                catch (System.Exception ex)
                {

                    Debug.WriteLine(ex);
                }
            });

            MessagingCenter.Subscribe<ShoppingListDetailViewModel, ShoppingList>(this, "UpdateShoppingList", async (obj, item) =>
            {
                var newShoppingList = item as ShoppingList;
                var shoppingListsServices = new ShoppingListsService();

                await shoppingListsServices.PutShoppingListAsync(newShoppingList.Id, newShoppingList);
                await InitializeDataAsync();
            });

            MessagingCenter.Subscribe<ShoppingListsPage, ShoppingList>(this, "DeleteShoppingList", async (obj, item) =>
            {

                var shoppingList = item as ShoppingList;
                var shoppingListsServices = new ShoppingListsService();

                MyShoppingLists.Remove(shoppingList);
                await shoppingListsServices.DeleteShoppingListAsync(shoppingList.Id);
            });
        }

        private async void GetLoggedUser()
        {
            LoggedUser = await SecureStorage.GetAsync("loginToken");
        }

        private async void GetReceivedLists()
        {
            var shoppingListService = new ShoppingListsService();
            var receivedShoppingLists = await shoppingListService.GetUserShoppingListsAsync(LoggedUser);
            ReceivedShoppingLists = receivedShoppingLists;
        }

        private async Task InitializeDataAsync()
        {
            var shoppingListsServices = new ShoppingListsService();
            var shoppingLists = await shoppingListsServices.GetShoppingListsAsync();
            MyShoppingLists = shoppingLists;
        }
    }
}

