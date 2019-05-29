using ListerMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivedPage : ContentPage
    {
        ShoppingListsViewModel viewModel;

        public ReceivedPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ShoppingListsViewModel();
            viewModel.Title = "Odebrane Listy";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void GetShoppingListsForCurrentUser()
        {
            //var shoppinglistsService = new Shoppi
        }
    }
}