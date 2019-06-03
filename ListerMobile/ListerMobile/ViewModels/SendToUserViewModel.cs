using ListerMobile.Helpers;
using ListerMobile.Models;
using ListerMobile.Services;
using ListerMobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public string LoggedUser { get; set; }
        public string ChosenUser { get; set; }
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
                   ChosenUser = arg;

                   if (ShoppingList.Users.Contains(ChosenUser))
                   {
                       MessagingCenter.Send(this, "AlreadySentAlert");

                       return;
                   }

                   EvaluateUsers();

                   SendShoppingList(ShoppingList);
               }
               catch (Exception ex)
               {
                   Debug.Write(ex.InnerException.Message);
               }
           });

            MessagingCenter.Subscribe<SendPage, string>(this, "SearchUserClicked", (sender, arg) =>
            {
                GetSearchedUser(arg);
            });
        }

        private void AssignAdequateValueTosers()
        {
            if (ShoppingList.Users != null)
            {
                return;
            }

            ShoppingList.Users = "";
        }

        private void EvaluateUsers()
        {
            if (!ShoppingList.Users.Contains(LoggedUser))
            {
                ShoppingList.Users = LoggedUser;
            }

            else
            {
                ShoppingList.Users += " " + ChosenUser;
            }
        }

        private void GetSearchedUser(string userName)
        {
            var searchedUser = AllUsers.Find(n => n.Name.Equals(userName));

            if (MyStorage.GetMyStorageInstance.FriendlyUsers.Contains(searchedUser))
            {
                return;
            }
            MyStorage.GetMyStorageInstance.FriendlyUsers.Add(searchedUser);

            KnownUsers = MyStorage.GetMyStorageInstance.FriendlyUsers;
        }

        private async void SendShoppingList(ShoppingList list)
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

            //var observableUsers = new ObservableCollection<User>(listofUsers);
            AllUsers = listofUsers;
        }



        private async void GetCurrentUser()
        {
            LoggedUser = await SecureStorage.GetAsync("loginToken");
        }
    }
}
