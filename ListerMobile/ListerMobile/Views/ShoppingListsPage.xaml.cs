using ListerMobile.Models;
using ListerMobile.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShoppingListsPage : ContentPage
    {
        ShoppingListsViewModel viewModel;

        public ShoppingListsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ShoppingListsViewModel(Navigation);
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewShoppingListPage()));
        }

        private async void EditListButton_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            var item = btn.BindingContext as ShoppingList;
            if (item == null) return;
            await Navigation.PushAsync(new ShoppingListDetailPage(new ShoppingListDetailViewModel(item)));
        }

        private void DeleteListButton_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            var item = btn.BindingContext as ShoppingList;
            if (item == null) return;
            MessagingCenter.Send(this, "DeleteShoppingList", item);
        }

        private async void SendListButton_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            var item = btn.BindingContext as ShoppingList;
            if (item == null) return;


            await Navigation.PushModalAsync(new NavigationPage(new SendPage()));

            MessagingCenter.Send(this, "SendListButtonClicked", item);

        }
    }
}