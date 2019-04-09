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

        //private ListView ItemsListView = new ListView();     // NWM czy to siądzie

        public ShoppingListsPage()
        {
            InitializeComponent();

            //ItemsListView.Effects

            BindingContext = viewModel = new ShoppingListsViewModel();
        }



        //async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        //{

        //    var item = args.SelectedItem as ShoppingList;
        //    if (item == null)
        //        return;

        //    await Navigation.PushAsync(new ShoppingListDetailPage(new ShoppingListDetailViewModel(item)));

        //    // Manually deselect item.
        //    ItemsListView.SelectedItem = null;
        //}

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewShoppingListPage()));
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    InitializeBindingContext();

        //    if (viewModel.MyShoppingLists.Count == 0)
        //        viewModel.LoadItemsCommand.Execute(null);
        //}

        private void InitializeBindingContext()
        {
            ShoppingListsViewModel viewModel = (BindingContext as ShoppingListsViewModel);
            if (viewModel != null && viewModel.MyShoppingLists.Count == 0)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
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
            //if (viewModel.MyShoppingLists.Count == 0)
            //    viewModel.LoadItemsCommand.Execute(null);
        }

        //private async void HamburgerButton_Clicked(object sender, EventArgs e)
        //{
        //    var page = Navigation.NavigationStack;
        //    page.
        //}

        //private void SettingsButton_Clicked(object sender, EventArgs e)
        //{

        //}

    }
}