using ListerMobile.Helpers;
using ListerMobile.Models;
using ListerMobile.Services;
using ListerMobile.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class SendToUserViewModel : BaseViewModel
    {
        private List<User> allUsers = new List<User>();
        public List<User> AllUsers
        {
            get { return allUsers; }
            set { SetProperty(ref allUsers, value); }
        }

        private ObservableCollection<User> knownusers = new ObservableCollection<User>();
        public ObservableCollection<User> KnownUsers
        {
            get { return knownusers; }
            set { SetProperty(ref knownusers, value); }
        }

        public User LoggedUser { get; set; }
        public User ChosenUser { get; set; }

        public ShoppingList ShoppingList { get; set; } = new ShoppingList();

        //ICommand SendToSelectedUserCommand { get; set; }

        public SendToUserViewModel()
        {
            Title = "Wyślij do";
            //SendToSelectedUserCommand = new Command(SendShoppingList);
            //AssignLoggedUserName();
            InitialiseUsers();
            InitializeDataAsync();

            MessagingCenter.Subscribe<ShoppingListsPage, ShoppingList>(this, "SendListButtonClicked", (obj, item) =>
            {
                ShoppingList = item;
                ShoppingList.Users = new List<User>();
                var d = "s";
            });

            MessagingCenter.Subscribe<SendPage, User>(this, "SendToChosenUser", async (sender, arg) =>
            {
                var user = arg as User;
                ChosenUser = user;

                LoggedUser.Name = MyStorage.GetMyStorageInstance.LoggedUser.Name;
                LoggedUser.PassPhrase = MyStorage.GetMyStorageInstance.LoggedUser.PassPhrase;

                if (ShoppingList.Users.Contains(LoggedUser) || ShoppingList.Users.Contains(ChosenUser))
                {
                    return;
                }

                ShoppingList.Users.Add(LoggedUser);
                var rrrrr = "ssss";
                ShoppingList.Users.Add(ChosenUser);
                var dupelek = "ssss";

                foreach (var item in ShoppingList.Users)
                {
                    Debug.Write(item.Name);
                }

                var shoppingListsService = new ShoppingListsService();
                await shoppingListsService.PutShoppingListAsync(ShoppingList.Id, ShoppingList);

            });

            MessagingCenter.Subscribe<SendPage, string>(this, "SearchUserClicked", (sender, arg) =>
            {
                GetSearchedUser(arg);
            });
        }

        private void InitialiseUsers()
        {
            LoggedUser = new User();
            ChosenUser = new User();
        }

        //private async void AssignLoggedUserName()
        //{
        //    LoggedUser.Name = await SecureStorage.GetAsync("loginToken");
        //}

        private void GetSearchedUser(string userName)
        {
            var searchedUser = AllUsers.Find(n => n.Name.Equals(userName));

            if (MyStorage.GetMyStorageInstance.FriendlyUsers.Contains(searchedUser))
            {
                return;
            }
            MyStorage.GetMyStorageInstance.FriendlyUsers.Add(searchedUser);

            KnownUsers = MyStorage.GetMyStorageInstance.FriendlyUsers;

            //foreach (var user in MyStorage.GetMyStorageInstance.FriendlyUsers)
            //{
            //    KnownUsers.Add(searchedUser);
            //}



            //KnownUsers.Add(searchedUser);
        }

        //private void SendShoppingList(object list)
        //{
        //    //var shoppingListService = new ShoppingListsService();
        //    var dupal = "sss";
        //}

        private async void InitializeDataAsync()
        {
            UsersService usersService = new UsersService();
            var users = await usersService.GetUsersAsync();
            FilterUsersFromCurrent(users);
            KnownUsers = MyStorage.GetMyStorageInstance.FriendlyUsers;
        }

        private async void FilterUsersFromCurrent(ObservableCollection<User> users)
        {
            var loggedUserName = await SecureStorage.GetAsync("loginToken");
            IEnumerable<User> enumerableUsers = users;
            var listofUsers = new List<User>(enumerableUsers);
            var thisUser = listofUsers.Find(n => n.Name.Equals(loggedUserName));
            listofUsers.Remove(thisUser);

            //var observableUsers = new ObservableCollection<User>(listofUsers);
            AllUsers = listofUsers;
        }



        private async Task<string> GetCurrentUser()
        {
            return await SecureStorage.GetAsync("loginToken");
        }
    }
}
