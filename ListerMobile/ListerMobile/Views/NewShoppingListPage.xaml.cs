using ListerMobile.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewShoppingListPage : TabbedPage
    {
        public ShoppingList ShoppingList { get; set; }

        public NewShoppingListPage()
        {
            InitializeComponent();

            ShoppingList = new ShoppingList
            {
                Name = "Shopping List name",
                Body = "To jest body description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddShoppingList", ShoppingList);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}