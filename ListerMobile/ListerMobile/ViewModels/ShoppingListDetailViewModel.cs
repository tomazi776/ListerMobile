using ListerMobile.Models;
using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class ShoppingListDetailViewModel : BaseViewModel
    {
        private ShoppingList _shoppingList;
        public ShoppingList ShoppingList
        {
            get { return _shoppingList; }
            set { SetProperty(ref _shoppingList, value); }
        }

        private bool _isUpdateButtonClicked;
        public bool IsUpdateButtonClicked
        {
            get { return _isUpdateButtonClicked; }
            set
            {
                if (SetProperty(ref _isUpdateButtonClicked, value))
                {
                    SendListToAnotherPage();
                }
            }
        }

        public ShoppingListDetailViewModel(ShoppingList shoppingList = null)
        {
            Title = shoppingList?.Name;
            ShoppingList = shoppingList;
        }

        public ShoppingListDetailViewModel()
        {

        }

        private void SendListToAnotherPage()
        {
            MessagingCenter.Send(this, "UpdateShoppingList", ShoppingList);
        }
    }
}
