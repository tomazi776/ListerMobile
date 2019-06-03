using ListerMobile.Helpers;
using ListerMobile.Models;
using ListerMobile.Services;
using ListerMobile.Views;
using System;
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

        public string LoggedUserName { get; set; }
        public string ChosenUserName { get; set; }
        public ShoppingList ShoppingList { get; set; } = new ShoppingList();
        public ShoppingList SendingShippingList { get; set; } = new ShoppingList();


        public SendToUserViewModel(ShoppingList list)
        {
            Title = "Wyślij do";
            ShoppingList = list;
            GetCurrentUser();
            AssignAdequateValueTosers();
            InitializeDataAsync();

            MessagingCenter.Subscribe<SendPage, string>(this, "SendToChosenUser", async (sender, arg) =>
           {
               try
               {
                   ChosenUserName = arg;
                   var sent = CheckIfAlreadySent();
                   if (!sent)
                   {
                       AssignUsersToShoppingList();
                       await SendShoppingList(ShoppingList);
                   }
               }
               catch (Exception ex)
               {
                   Debug.Write(ex.InnerException.Message);
               }
           });

            MessagingCenter.Subscribe<SendPage, string>(this, "SearchUserClicked", (sender, arg) =>
            {
                var searchedUser = AllUsers.Find(n => n.Name.Equals(arg));

                AddSearchedUser(arg, searchedUser);
            });
        }

        private bool CheckIfAlreadySent()
        {
            var listHasName = CheckIfHasName(ChosenUserName);

            if (listHasName)
            {
                MessagingCenter.Send(this, "AlreadySentAlert");

                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CheckIfHasName(string userName)
        {
            return ShoppingList.Users.Contains(userName) ? true : false;
        }

        private void AssignAdequateValueTosers()
        {
            if (ShoppingList.Users != null)
            {
                return;
            }

            ShoppingList.Users = "";
        }

        private void AssignUsersToShoppingList()
        {
            var listHasName = CheckIfHasName(LoggedUserName);
            if (!listHasName)
            {
                ShoppingList.Users = LoggedUserName;
                ShoppingList.Users += " " + ChosenUserName;
            }

            else
            {
                ShoppingList.Users += " " + ChosenUserName;
            }
        }

        private void AddSearchedUser(string userName, User user)
        {
            if (KnownUsers.Contains(user) || MyStorage.GetMyStorageInstance.FriendlyUsers.Contains(user))
            {
                return;
            }

            MyStorage.GetMyStorageInstance.FriendlyUsers.Add(user);
            KnownUsers = MyStorage.GetMyStorageInstance.FriendlyUsers;
        }

        private async Task SendShoppingList(ShoppingList list)
        {
            var shoppingListsService = new ShoppingListsService();
            await shoppingListsService.PutShoppingListAsync(list.Id, list);

            //MessagingCenter.Send(this, "ListHasBeenSent");
        }

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
            AllUsers = listofUsers;
        }

        private async void GetCurrentUser()
        {
            LoggedUserName = await SecureStorage.GetAsync("loginToken");
        }
    }
}
