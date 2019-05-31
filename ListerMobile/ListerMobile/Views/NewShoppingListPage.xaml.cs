using ListerMobile.Models;
using ListerMobile.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewShoppingListPage : TabbedPage
    {
        NewShoppingListViewModel viewModel;

        public ShoppingList ShoppingList { get; set; }
        public DateTime CurrentDate { get; set; }
        public List<string> NewListProducts = new List<string>();
        public bool IsNewVoiceListClicked { get; set; }

        public NewShoppingListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewShoppingListViewModel(Navigation);

            MessagingCenter.Subscribe<NewShoppingListViewModel, string>(this, "NewVoiceButtonClicked", (obj, args) =>
            {
                VoiceBodyEditor.Text = args;
            });

            MessagingCenter.Subscribe<NewShoppingListViewModel, string>(this, "AddVoiceButtonClicked", (obj, args) =>
            {
                VoiceBodyEditor.Text += args;
            });


            MessagingCenter.Subscribe<NewShoppingListViewModel, ShoppingList>(this, "InputNotValid", (obj, args) =>
            {
                DisplayAlert("Nie można zapisać pustej listy", "", "OK");
            });
        }
    }
}
