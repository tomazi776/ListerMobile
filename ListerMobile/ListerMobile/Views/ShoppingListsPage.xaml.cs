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
        //ShoppingListsViewModel viewModel;

        //private ListView ItemsListView = new ListView();     // NWM czy to siądzie

        public ShoppingListsPage()
        {
            InitializeComponent();
            //BindingContext = viewModel = new ShoppingListsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as ShoppingList;
            if (item == null)
                return;

            await Navigation.PushAsync(new ShoppingListDetailPage(new ShoppingListDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewShoppingListPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeBindingContext();

            //if (viewModel.ShoppingLists.Count == 0)
            //    viewModel.LoadItemsCommand.Execute(null);
        }

        private void InitializeBindingContext()
        {
            ShoppingListsViewModel viewModel = (BindingContext as ShoppingListsViewModel);
            if (viewModel != null && viewModel.ShoppingLists.Count == 0)
            {
                viewModel.LoadItemsCommand.Execute(null);
            }
        }
    }
}