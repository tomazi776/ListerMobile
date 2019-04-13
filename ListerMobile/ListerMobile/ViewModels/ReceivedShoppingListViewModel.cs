using ListerMobile.Models;
using System.Collections.ObjectModel;

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
    }
}
