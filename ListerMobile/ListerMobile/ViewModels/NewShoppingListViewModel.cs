using ListerMobile.Models;
using System.Collections.ObjectModel;

namespace ListerMobile.ViewModels
{
    public class NewShoppingListViewModel : BaseViewModel
    {
        public ObservableCollection<Product> FavouriteProducts { get; set; } = new ObservableCollection<Product>();


    }
}
