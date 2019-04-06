using ListerMobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();

        public MainPage()
        {
            InitializeComponent();
            this.IsPresentedChanged += OnPresentedChanged;

            MasterBehavior = MasterBehavior.Popover;

            //MenuPages.Add((int)MenuItemType.About, (NavigationPage)Detail);
        }

        private void OnPresentedChanged(object sender, EventArgs e)
        {
            MessagingCenter.Subscribe<NewShoppingListPage, ShoppingList>(this, "AddShoppingList", async (obj, item) => { });

            if (this.IsPresented)
            {
                int i = 2;
            }
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Moje_Listy:
                        MenuPages.Add(id, new NavigationPage(new ShoppingListsPage()));
                        break;
                    case (int)MenuItemType.Ulubione:
                        MenuPages.Add(id, new NavigationPage(new FavouriteProductsPage()));
                        break;
                    case (int)MenuItemType.O_Aplikacji:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.Odebrane:
                        MenuPages.Add(id, new NavigationPage(new ReceivedPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
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

        //async void AddItem_Clicked(object sender, EventArgs e)
        //{
        //    await Navigation.PushModalAsync(new NavigationPage(new NewShoppingListPage()));
        //}

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    if (viewModel.ShoppingLists.Count == 0)
        //        viewModel.LoadItemsCommand.Execute(null);
        //}

    }
}