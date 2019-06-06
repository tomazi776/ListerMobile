using ListerMobile.Models;
using ListerMobile.Services;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace ListerMobile.ViewModels
{

    public class ReceivedShoppingListViewModel : BaseViewModel
    {
        private ShoppingList _shoppingList;
        public ShoppingList ShoppingList
        {
            get { return _shoppingList; }
            set { SetProperty(ref _shoppingList, value); }
        }

        private ObservableCollection<ShoppingList> _receivedShoppingLists;
        public ObservableCollection<ShoppingList> ReceivedShoppingLists
        {
            get { return _receivedShoppingLists; }
            set { SetProperty(ref _receivedShoppingLists, value); }
        }

        private string _loggedUser;
        public string LoggedUser
        {
            get { return _loggedUser; }
            set { SetProperty(ref _loggedUser, value); }
        }


        public ReceivedShoppingListViewModel()
        {
            GetLoggedUser();
            ReceivedShoppingLists = new ObservableCollection<ShoppingList>();
            GetReceivedLists();
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

    }
}
