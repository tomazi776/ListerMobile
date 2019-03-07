using ListerMobile.Models;
using ListerMobile.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListDetailPage : ContentPage
    {
        ShoppingListDetailViewModel viewModel;

        public ShoppingListDetailPage(ShoppingListDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ShoppingListDetailPage()
        {
            InitializeComponent();

            //var item = new ShoppingList
            //{
            //    Name = "Lista zakupów 1",
            //    Body = "To jest body takiej jednej listy zakupów."
            //};

            //viewModel = new ShoppingListDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}