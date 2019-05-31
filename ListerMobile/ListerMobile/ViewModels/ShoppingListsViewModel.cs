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
        public ObservableCollection<ShoppingList> ReceivedShoppingLists { get; set; } = new ObservableCollection<ShoppingList>();


        public bool HadBeenInitialized { get; set; }
        private bool IsListRemoved { get; set; } = false;
        public Command LoadItemsCommand { get; set; }

        private ObservableCollection<ShoppingList> _myShoppingLists;
        public ObservableCollection<ShoppingList> MyShoppingLists
        {
            get { return _myShoppingLists; }
            set { SetProperty(ref _myShoppingLists, value); }
        }

        /// <summary>
        /// Initializes Data fetched from server
        /// </summary>
        public ShoppingListsViewModel()
        {
            Title = SHOPPING_LISTS_PAGE_TITLE;
            MyShoppingLists = new ObservableCollection<ShoppingList>();

            InitializeDataAsync();


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

        private async Task InitializeDataAsync()
        {
            var shoppingListsServices = new ShoppingListsService();
            var shoppingLists = await shoppingListsServices.GetShoppingListsAsync();

            var userName = await SecureStorage.GetAsync("loginToken");
            //var shoppingLists = await shoppingListsServices.GetUserShoppingListsAsync(userName);     //TODO  Modify service to get all the ShoppingLists for currently logged User

            MyShoppingLists = shoppingLists;
        }
    }
}

