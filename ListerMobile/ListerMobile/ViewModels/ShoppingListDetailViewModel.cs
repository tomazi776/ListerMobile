using ListerMobile.Models;

namespace ListerMobile.ViewModels
{
    public class ShoppingListDetailViewModel : BaseViewModel
    {
        public ShoppingList ShoppingList { get; set; }

        public ShoppingListDetailViewModel(ShoppingList shoppingList = null)
        {
            //ShoppingList.Name = shoppingList.Name;
            //ShoppingList.Body = shoppingList.Body;
            Title = shoppingList?.Name;
            ShoppingList = shoppingList;
        }
    }
}
