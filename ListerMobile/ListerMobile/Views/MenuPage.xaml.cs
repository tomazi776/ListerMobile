using ListerMobile.Models;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Ulubione, Title = MenuItemType.Ulubione.ToString() },
                new HomeMenuItem {Id = MenuItemType.Moje_Listy, Title=MenuItemType.Moje_Listy.ToString().Replace('_', ' ' )},
                new HomeMenuItem {Id = MenuItemType.Odebrane, Title = MenuItemType.Odebrane.ToString() },
                new HomeMenuItem {Id = MenuItemType.O_Aplikacji, Title=MenuItemType.O_Aplikacji.ToString().Replace('_', ' ' ) }
            };

            ListViewMenu.ItemsSource = menuItems;

            //ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}