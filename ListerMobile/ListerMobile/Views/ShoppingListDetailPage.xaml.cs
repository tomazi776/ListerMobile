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
            BindingContext = viewModel;
        }
    }
}