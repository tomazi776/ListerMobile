using ListerMobile.ViewModels;
using System;
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

        private async void UpdateList_Clicked(object sender, EventArgs e)
        {
            viewModel.IsUpdateButtonClicked = true;
            await Navigation.PopAsync();
        }

        private async void CancelUpdateList_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}