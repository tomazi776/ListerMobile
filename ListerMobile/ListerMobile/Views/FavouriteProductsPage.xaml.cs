using ListerMobile.Models;
using ListerMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouriteProductsPage : ContentPage
    {
        //public ObservableCollection<Product> Products { get; set; }
        FavouriteProductsViewModel viewModel;

        public FavouriteProductsPage(FavouriteProductsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
        }

        public FavouriteProductsPage()
        {
            InitializeComponent();

            var item = new Product("Czekolada grubasku.", null, null, "zdjecie");
            //{
            //    Name = "Czekolada grubasku.",
            //    Picture = "Czekolada grubasku."
            //};
            //Products.Add(item);

            viewModel = new FavouriteProductsViewModel(item);
            BindingContext = viewModel;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ShoppingList;
            if (item == null)
                return;

            await Navigation.PushAsync(new ShoppingListDetailPage(new ShoppingListDetailViewModel(item)));

            // Manually deselect item.
            FavItemsListView.SelectedItem = null;
        }
    }
}