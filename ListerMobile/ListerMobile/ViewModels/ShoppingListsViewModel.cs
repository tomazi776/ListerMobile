using ListerMobile.Models;
using ListerMobile.Services;
using ListerMobile.Views;
using System;
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
        public ObservableCollection<ShoppingList> MyOldShoppingLists { get; set; }
        public ObservableCollection<ShoppingList> ReceivedShoppingLists { get; set; } = new ObservableCollection<ShoppingList>();
        public Command LoadItemsCommand { get; set; }

        private ObservableCollection<ShoppingList> _myShoppingLists;
        public ObservableCollection<ShoppingList> MyShoppingLists
        {
            get { return _myShoppingLists; }
            set { SetProperty(ref _myShoppingLists, value); }
        }



        public ShoppingListsViewModel()
        {
            Title = "Moje Listy";
            //MyOldShoppingLists = new ObservableCollection<ShoppingList>();
            MyShoppingLists = new ObservableCollection<ShoppingList>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            // Adds newly created ShoppingList for display   ++ Add functionality for adding shoppingList to server at the same time
            MessagingCenter.Subscribe<NewShoppingListPage, ShoppingList>(this, "AddShoppingList", (obj, item) =>
            {
                var newShoppingList = item as ShoppingList;

                MyShoppingLists.Add(newShoppingList);

                //MakeHighlightBody();

                //await DataStore.AddItemAsync(newShoppingList);
            });

            //InitializeDataAsync();
        }

        private async Task InitializeDataAsync()
        {
            var shoppingListsServices = new ShoppingListsServices();
            MyShoppingLists = await shoppingListsServices.GetShoppingListsAsync();
            AdjustRecievedInput();
        }


        private void AdjustRecievedInput()           // move method to ShoppingListsViewModel
        {
            foreach (var item in MyShoppingLists)
            {
                item.Body = item.Body.TrimEnd('\r', '\n', ' ', ',', '.');
                item.BodyHighlight = MakeHighlightFromBody(item.Body);
                var listDate = DateTime.Parse(item.CreationDate.ToString()).ToString("dd.MM.yy");
                item.Name += " " + listDate;
            }
        }

        private string MakeHighlightFromBody(string body)       // move method to ShoppingListsViewModel
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

        static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select m.Value;

            return words.ToArray();
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                MyShoppingLists.Clear();
                //var items = await DataStore.GetItemsAsync(true);
                //  //var shoppingListsServices = new ShoppingListsServices();
                //  //var items = await shoppingListsServices.GetShoppingListsAsyn
                InitializeDataAsync();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        //async Task ExecuteDeleteItemCommand()
        //{
        //    try
        //    {
        //        var items = await DataStore.DeleteItemAsync()
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}

