using ListerMobile.Helpers;
using ListerMobile.Models;
using ListerMobile.Services;
using ListerMobile.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class ShoppingListsViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private const string SHOPPING_LISTS_PAGE_TITLE = "Moje Listy";
        public ObservableCollection<ShoppingList> ReceivedShoppingLists { get; set; } = new ObservableCollection<ShoppingList>();


        public bool HadBeenInitialized { get; set; }
        //public ObservableCollection<ShoppingList> ArchivedLists { get; set; } = new ObservableCollection<ShoppingList>();
        private bool IsListRemoved { get; set; } = false;
        private ShoppingList ListToBeRemoved { get; set; } = new ShoppingList();
        public Command LoadItemsCommand { get; set; }

        private ObservableCollection<ShoppingList> _myShoppingLists;
        public ObservableCollection<ShoppingList> MyShoppingLists
        {
            get { return _myShoppingLists; }
            set { SetProperty(ref _myShoppingLists, value); }
        }

        //private User user;

        //public User User
        //{
        //    get { return user; }
        //    set { SetProperty(ref user, value); }
        //}



        /// <summary>
        /// Initializes Data fetched from server
        /// </summary>
        public ShoppingListsViewModel()
        {
            Title = SHOPPING_LISTS_PAGE_TITLE;
            MyShoppingLists = new ObservableCollection<ShoppingList>();
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<LoginPage, string>(this, "AddUser", async (obj, item) =>
            //{
            //    User = new User();
            //    var userName = item as string;
            //    User.Name = userName;
            //    var dupaleek = "ssss";

            //});
            //var dupal = "ssss";


            // Adds newly created ShoppingList for display 
            MessagingCenter.Subscribe<NewShoppingListPage, ShoppingList>(this, "AddShoppingList", async (obj, item) =>
            {

                try
                {
                    var newShoppingList = item as ShoppingList;
                    newShoppingList.User = Globals.USER;


                    var shoppingListsServices = new ShoppingListsServices();

                    await shoppingListsServices.PostShoppingListAsync(newShoppingList);
                    MyShoppingLists.Add(newShoppingList);
                }
                catch (System.Exception ex)
                {

                    Debug.WriteLine(ex);
                }

            });


            MessagingCenter.Subscribe<ShoppingListDetailViewModel, ShoppingList>(this, "UpdateShoppingList", async (obj, item) =>
            {
                var newShoppingList = item as ShoppingList;
                var shoppingListsServices = new ShoppingListsServices();

                await shoppingListsServices.PutShoppingListAsync(newShoppingList.Id, newShoppingList);
                await InitializeDataAsync();
            });

            // Deletes selected shoppingList from the View and Server
            MessagingCenter.Subscribe<ShoppingListsPage, ShoppingList>(this, "DeleteShoppingList", async (obj, item) =>
            {

                var shoppingList = item as ShoppingList;
                var shoppingListsServices = new ShoppingListsServices();

                MyShoppingLists.Remove(shoppingList);
                await shoppingListsServices.DeleteShoppingListAsync(shoppingList.Id);

            });

            //MessagingCenter.Subscribe<StartingPage, bool>(this, "FetchServerData", async (obj, item) =>
            //{
            //    if (item)
            //    {
            //        await InitializeDataAsync();
            //    }

            //});

            InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            var shoppingListsServices = new ShoppingListsServices();
            var shoppingLists = await shoppingListsServices.GetShoppingListsAsync();
            AdjustBodyAndHighlightInput();
            MyShoppingLists = shoppingLists;
        }


        private void AdjustBodyAndHighlightInput()
        {
            foreach (var item in MyShoppingLists)
            {
                item.Body = item.Body.TrimEnd('\r', '\n', ' ', ',', '.');
                item.BodyHighlight = MakeHighlightFromBody(item.Body);
                if (item.CreationDate == null)
                {
                    return;
                }
            }
        }

        private string MakeHighlightFromBody(string body)
        {
            var bodyElement = GetWords(body);
            string result = string.Empty;
            for (int i = 0; i < bodyElement.Length; i++)
            {
                if (bodyElement.Length >= 4)
                {
                    result = "\n- " + bodyElement[0] + "\n- " + bodyElement[1] + "\n- " + bodyElement[2];
                    return result;
                }
                var temp = "\n- " + bodyElement[i];
                result += temp;
            }
            return result;
        }

        private static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select m.Value;

            return words.ToArray();
        }

        //async Task ExecuteLoadItemsCommand()
        //{
        //    if (IsBusy)
        //        return;

        //    IsBusy = true;

        //    try
        //    {
        //        await InitializeDataAsync();

        //        if (IsListRemoved)
        //        {
        //            //MessagingCenter.Subscribe<ShoppingListsPage, ShoppingList>(this, "DeleteShoppingList", (obj, item) =>
        //            //{
        //            //    ListToBeRemoved = item;
        //            //    RemoveList(ListToBeRemoved);

        //            //});

        //            //foreach (var item in MovedToArchiveLists)
        //            //{
        //            //    var list = MovedToArchiveLists[item.Id];
        //            //    MyShoppingLists.RemoveAt();

        //            //}

        //            RemoveList(ListToBeRemoved);


        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        //async Task ExecuteDeleteItemCommand()
        //{
        //    try
        //    {
        //        var items = await DataStore.DeleteItemAsync();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}

