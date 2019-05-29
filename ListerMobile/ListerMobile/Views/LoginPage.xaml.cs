using ListerMobile.Helpers;
using ListerMobile.Models;
using ListerMobile.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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

        private User FoundUser { get; set; }



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
            GetUsersAsync();


            RememberMe = RememberSwitch.IsToggled;
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
                //VisualStateManager.GoToState(UserNameEntry, "Nieprawidłowe");
                isValid = false;
            }

            if (string.IsNullOrEmpty(PasswordEntry.Text) || PasswordEntry.Text.Length < 5)
            {
                //VisualStateManager.GoToState(PasswordEntry, "Nieprawidłowe");
                isValid = false;
            }

            //User usr = new User(UserName, PasswordEntry.Text);



            if (isValid && CredentialsMatch())
            {
                try
                {
                    await SecureStorage.SetAsync("loginToken", UserNameEntry.Text);
                    await SecureStorage.SetAsync("token", PasswordEntry.Text);
                    UserName = UserNameEntry.Text;
                    MyStorage.GetMyStorageInstance.UserName = UserName;

                    //MessagingCenter.Send(this, "AddUser", UserName);
                    var dupal = "ddddd";

                    await Navigation.PopModalAsync();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("TU MASZ BŁĄD TUMOKU:" + ex);
                }


                //await DisplayAlert("Zalogowano!", "", "Dzięki zią");
                //await Clipboard.SetTextAsync("1234");
                //await Navigation.PushAsync(new MainPage());
            }

            await DisplayAlert("Login lub hasło nieprawidłowe!", "", "Spróbuj jeszcze raz");

        }

        private async void GetUsersAsync()
        {
            var usersServices = new UsersServices();
            Users = await usersServices.GetUsersAsync();
            var dupal = "ssss";
        }

        private bool CredentialsMatch()
        {
            var userExists = CheckForUser(UserNameEntry.Text);
            var passPhraseMatch = ComparePassPhrase(PasswordEntry.Text);

            if (userExists && passPhraseMatch)
            {
                Debug.WriteLine("Credentials CORRECT!");
                return true;
            }
            else
            {
                Debug.WriteLine("Credentials do not match");
                return false;
            }
        }







        private async void RegisterUser(User user)
        {
            var usersServices = new UsersServices();
            await usersServices.PostUserAsync(user);
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

        private void ShowRegistrationFormButton_Clicked(object sender, EventArgs e)
        {
            GridLogin.IsVisible = false;
            GridLogin.IsEnabled = false;

            GridRegistration.IsVisible = true;
            GridRegistration.IsEnabled = true;

        }


        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {


            var inputValid = await ValidateInput();


            var hashedPassPhrase = GetHashString(PasswordRegistrationEntry.Text);
            var newUser = new User(UserNameRegistrationEntry.Text, hashedPassPhrase);
            var userExists = CheckForUser(UserNameRegistrationEntry.Text);
            var ddd = "ddd";
            if (!userExists && inputValid)
            {
                RegisterUser(newUser);
                GetUsersAsync();
                await DisplayAlert("Rejestracja udana!", "", "OK");
                GoBackToLogin();

            }
            else if (inputValid && userExists)
            {
                await DisplayAlert("Użytkownik o takiej nazwie już istnieje, podaj inną", "", "Ok");
            }
        }

        private async Task<bool> ValidateInput()
        {
            if (string.IsNullOrEmpty(UserNameRegistrationEntry.Text) || UserNameRegistrationEntry.Text.Length < 5)
            {
                await DisplayAlert("Nazwa nie moze być pusta, ani krótsza niż 5 znaków", "", "Ok");
                return false;
            }

            if (string.IsNullOrEmpty(PasswordRegistrationEntry.Text) || PasswordRegistrationEntry.Text.Length < 5)
            {
                await DisplayAlert("Hasło nie moze być puste, ani krótsze niż 5 znaków", "", "Ok");
                return false;
            }

            else
            {
                return true;
            }
        }

        private bool CheckForUser(string userName)
        {
            var matchingUser = Users.FirstOrDefault(u => u.Name.Equals(userName));

            if (Users.Contains(matchingUser))
            {
                FoundUser = matchingUser;
                return true;
            }
            else
            {
                Debug.WriteLine("UserName does not match any in Database");
                return false;
            }

        }

        private bool ComparePassPhrase(string inputPhrase)
        {
            var hashedPassPhrase = GetHashString(inputPhrase);
            var ddd = "dupalek";
            //var matchingUserPassPhrase = Users.FirstOrDefault(n => n.PassPhrase.Equals(hashedPasPhrase));

            var match = FoundUser.PassPhrase.Equals(hashedPassPhrase);

            if (match)
            {
                return true;
            }
            else
            {
                Debug.WriteLine("Passphrase does not match any in Database");
                return false;
            }
        }

        private void BackToLogin_Clicked(object sender, EventArgs e)
        {
            GoBackToLogin();
        }

        private void GoBackToLogin()
        {
            GridRegistration.IsVisible = false;
            GridRegistration.IsEnabled = false;

            GridLogin.IsVisible = true;
            GridLogin.IsEnabled = true;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}