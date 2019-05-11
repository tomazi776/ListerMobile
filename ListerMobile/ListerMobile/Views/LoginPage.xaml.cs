using ListerMobile.Models;
using ListerMobile.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    /// <summary>
    /// Uses Preferences for storing UserName and Id, with Secured Storage for storing Password
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public ObservableCollection<User> Users { get; set; }
        //private User user;

        //public User User
        //{
        //    get { return user; }
        //    set { user = value; }
        //}


        static int userId = Preferences.Get(nameof(UserId), 1);

        public int UserId
        {
            get => userId;
            set
            {
                userId = value;
                if (RememberMe)
                {
                    Preferences.Set(nameof(UserId), value);
                }

                OnPropertyChanged(nameof(RememberMe));
            }
        }



        public LoginPage()
        {
            InitializeComponent();
            Users = new ObservableCollection<User>();

            RememberSwitch.IsToggled = RememberMe;

            //GetUserDataFromStorage();

        }

        //private void GetUserDataFromStorage()
        //{
        //    // if there's not any user already             User = new User();

        //    //if there's an existing user
        //    Preferences.Get(nameof())

        //}

        bool rememberMe = false;
        public bool RememberMe
        {
            get => Preferences.Get(nameof(RememberMe), false);
            set
            {
                Preferences.Set(nameof(RememberMe), value);
                if (!value)
                {
                    Preferences.Set(nameof(UserName), string.Empty);
                }
                OnPropertyChanged(nameof(RememberMe));
            }
        }

        string username = Preferences.Get(nameof(UserName), string.Empty);

        public string UserName
        {
            get => username;
            set
            {
                username = value;
                if (RememberMe)
                {
                    Preferences.Set(nameof(UserName), value);
                }
                OnPropertyChanged(nameof(RememberMe));
            }
        }

        async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Brak internetu", "", "OK");
                return;
            }

            var isValid = true;

            if (string.IsNullOrEmpty(UserNameEntry.Text) || UserNameEntry.Text.Length < 5)
            {
                VisualStateManager.GoToState(UserNameEntry, "Nieprawidłowe");
                isValid = false;
            }

            if (string.IsNullOrEmpty(PasswordEntry.Text) || PasswordEntry.Text.Length < 5)
            {
                VisualStateManager.GoToState(PasswordEntry, "Nieprawidłowe");
                isValid = false;
            }



            if (isValid)
            {

                try
                {
                    await SecureStorage.SetAsync("loginToken", UserNameEntry.Text);
                    await SecureStorage.SetAsync("token", PasswordEntry.Text);
                    UserName = UserNameEntry.Text;


                    //MessagingCenter.Send(this, "AddUser", UserName);
                    var dupal = "ddddd";

                    await Navigation.PopModalAsync();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }


                //await DisplayAlert("Zalogowano!", "", "Dzięki zią");
                //await Clipboard.SetTextAsync("1234");
                //await Navigation.PushAsync(new MainPage());
            }


        }

        //private async void CheckForExistingUserNameInDB()
        //{
        //    var shoppingListsServices = new ShoppingListsServices();
        //    var lists = await shoppingListsServices.GetShoppingListsAsync();

        //    User us = new User(UserName);
        //    var list = new ShoppingList();
        //    list.User = us;
        //    var listUserName = list.User.Name;

        //    //var IsUserInDB = lists.Contains(list.User.Name.Equals(UserName));

        //    //if ()
        //    //{
        //    //    CreateNewUser(us);
        //    //}


        //    Globals.USER = new User();

        //    var userName = Preferences.Get(nameof(UserName), string.Empty);
        //    if (username.Equals(string.Empty))
        //    {

        //    }
        //    else
        //    {

        //    }

        //}

        private async void CreateNewUser(User usr)
        {
            var usersServices = new UsersServices();
            await usersServices.PostUserAsync(usr);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            try
            {
                var login = await SecureStorage.GetAsync("loginToken");
                UserNameEntry.Text = login;

                var password = await SecureStorage.GetAsync("token");
                PasswordEntry.Text = password;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Debug.WriteLine(e.NetworkAccess);
        }

    }
}