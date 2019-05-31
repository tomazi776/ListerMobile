using ListerMobile.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class FavouriteProductsViewModel : BaseViewModel
    {
        public Product Product { get; set; }
        public ObservableCollection<Product> FavouriteProducts { get; set; }
        public Command LoadProductsCommand { get; set; }



        public FavouriteProductsViewModel(Product product = null)
        {
            Title = "Ulubione";

            //FavouriteProducts = new ObservableCollection<Product>();

            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewShoppingListPage, ShoppingList>(this, "AddFavouriteToList", async (obj, item) =>
            //{
            //    var clickedProduct = item as Product;
            //    FavouriteProducts.Add(clickedProduct);
            //    await DataStore.AddItemAsync(clickedProduct);
            //});
        }
    }
}
