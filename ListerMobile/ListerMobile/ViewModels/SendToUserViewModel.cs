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

        //ICommand SendToSelectedUserCommand { get; set; }

        public SendToUserViewModel(ShoppingList list)
        {
            Title = "Wyślij do";
            ShoppingList = list;


            AssignAdequateValueTosers();

            var ddd = "dddddddd";
            //SendToSelectedUserCommand = new Command(SendShoppingList);
            //AssignLoggedUserName();
            GetCurrentUser();
            InitializeDataAsync();

            //MessagingCenter.Subscribe<ShoppingListsPage, ShoppingList>(this, "SendListButtonClicked", (obj, item) =>
            //{

            //    try
            //    {
            //        //ShoppingList.Id = item.Id;
            //        //ShoppingList.Name = item.Name;
            //        //ShoppingList.Body = item.Body;
            //        //ShoppingList.BodyHighlight = item.BodyHighlight;
            //        //ShoppingList.CreationDate = item.CreationDate;
            //        //ShoppingList.User = item.User;

            //        ShoppingList = item;



            //        var d = "s";
            //    }
            //    catch (System.Exception)
            //    {

            //        throw;
            //    }

            //});

            MessagingCenter.Subscribe<SendPage, string>(this, "SendToChosenUser", async (sender, arg) =>
           {
               try
               {
                   var userName = arg;
                   ChosenUser = userName;

                   EvaluateUsers();

                   //if (ShoppingList.Users.Contains(LoggedUser) || ShoppingList.Users.Contains(ChosenUser))
                   //{
                   //    return;
                   //}

                   var shoppingListsService = new ShoppingListsService();
                   await shoppingListsService.PutShoppingListAsync(ShoppingList.Id, ShoppingList);
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
            if (ShoppingList.Users != string.Empty )
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

            if (ShoppingList.Users.Contains(ChosenUser))
            {
                return;
            }
            else
            {
                ShoppingList.Users += " " + ChosenUser;
            }
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



        private async void GetCurrentUser()
        {
            LoggedUser = await SecureStorage.GetAsync("loginToken");
        }
    }
}
