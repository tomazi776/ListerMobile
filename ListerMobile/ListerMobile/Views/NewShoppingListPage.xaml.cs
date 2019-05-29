using ListerMobile.Models;
using ListerMobile.Services;
using ListerMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
        //public User User { get; set; }
        public List<string> NewListProducts = new List<string>();
        public bool IsNewVoiceListClicked { get; set; }
        private ISpeechToText speechRecongnitionInstance;

        public NewShoppingListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewShoppingListViewModel(Navigation);





            //User = new User(Globals.USER.Name);
            //User = Globals.USER;

            CreateMockDataForList();

            try
            {
                speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            MessagingCenter.Subscribe<ISpeechToText, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });


            MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) =>
            {
                SpeechToTextFinalResultRecieved(args);
            });

        }

        private void CreateMockDataForList()
        {
            ShoppingList = new ShoppingList(1,
                GetTodaysDate(),
                DateTime.UtcNow,
                "- trzy - przykładowe - produkty",
                "- trzy - przykładowe - produkty - lipka - masełko - kalafior - lizaczek - maślanka",
                "TABALUGA");
            var zzz = "ssssssssssss";

            //Id = 1,
            //    CreationDate = DateTime.UtcNow,
            //    Name = GetTodaysDate(),
            //    Body = "- trzy - przykładowe - produkty - lipka - masełko - kalafior - lizaczek - maślanka",
            //    BodyHighlight = "- trzy - przykładowe - produkty"

            //GetLotggedUser();

        }

        //private async void GetLotggedUser()
        //{
        //    ShoppingList.User.Id = Preferences.Get("d", )
        //    ShoppingList.User.Name = await SecureStorage.GetAsync("loginToken");
        //}

        private void SpeechToTextFinalResultRecieved(string args)
        {
            var recievedText = GetWords(args);
            string result = string.Empty;

            for (int i = 0; i < recievedText.Length; i++)
            {

                result += "- " + recievedText[i] + "\n";
            }
            CheckForVoiceButtonClicked(result);
        }

        private void CheckForVoiceButtonClicked(string result)
        {
            if (IsNewVoiceListClicked)
            {
                VoiceBodyEditor.Text = result;
                IsNewVoiceListClicked = false;
            }
            else
            {
                VoiceBodyEditor.Text += result;
            }
        }

        private string GetTodaysDate()
        {
            var culture = new System.Globalization.CultureInfo("pl-PL");

            string date = DateTime.UtcNow.ToString("dd.MM.yy");
            var day = culture.DateTimeFormat.GetDayName(DateTime.UtcNow.DayOfWeek);
            var upperDay = char.ToUpper(day[0]) + day.Substring(1);
            string output = upperDay.ToString();
            return output;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            AdjustRecievedInput(ShoppingList.Body);
            //AdjustName();

            try
            {
                //ShoppingList.User = await SecureStorage.GetAsync("loginToken");
                //ShoppingList.User = Globals.USER;
                //var xxx = "dupal";
                //MessagingCenter.Send(this, "ShoppingListUserName", MyStorage.USER);
                //var sss = "dupal";

                // Create method for adding Creation Date to manually created list
                MessagingCenter.Send(this, "AddShoppingList", ShoppingList);

                await Navigation.PopModalAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //private void AdjustName()
        //{
        //    var date = DateTime.Today.ToString("dd.MM.yy");
        //    ShoppingList.Name += " " + date;
        //}

        private void AdjustRecievedInput(string body)
        {
            ShoppingList.Body = body.TrimEnd('\r', '\n', ' ', ',', '.');
            ShoppingList.BodyHighlight = MakeHighlightFromBody(body);
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
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

        static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select m.Value;

            return words.ToArray();
        }

        private void SpeakButton_Clicked(object sender, EventArgs e)
        {
            IsNewVoiceListClicked = true;
            InvokeConcreteImplementation();
        }

        private void SpeakAddButton_Clicked(object sender, EventArgs e)
        {
            InvokeConcreteImplementation();
        }

        private void InvokeConcreteImplementation()
        {
            try
            {
                speechRecongnitionInstance.StartSpeechToText();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}