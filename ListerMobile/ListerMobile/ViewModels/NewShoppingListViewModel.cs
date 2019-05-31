using ListerMobile.Models;
using ListerMobile.Services;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class NewShoppingListViewModel : BaseViewModel
    {

        private ShoppingList shoppingList = new ShoppingList();
        public ShoppingList ShoppingList
        {
            get { return shoppingList; }
            set { SetProperty(ref shoppingList, value); }
        }

        public bool IsNewVoiceListClicked { get; set; }
        public INavigation Navigation { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand NewVoiceListCommand { get; set; }
        public ICommand AddToVoiceListCommand { get; set; }
        private ISpeechToText speechRecongnitionInstance;


        public NewShoppingListViewModel(INavigation navigation)
        {
            Navigation = navigation;
            InitializeCommands();

            try
            {
                speechRecongnitionInstance = DependencyService.Get<ISpeechToText>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

        private void InitializeCommands()
        {
            CancelCommand = new Command(Cancel);
            SaveCommand = new Command(Save);
            NewVoiceListCommand = new Command(SpeakButton);
            AddToVoiceListCommand = new Command(SpeakAddButton);
        }

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
                MessagingCenter.Send(this, "NewVoiceButtonClicked", result);
                IsNewVoiceListClicked = false;
            }
            else
            {
                MessagingCenter.Send(this, "AddVoiceButtonClicked", result);
            }
        }

        private string GetTodaysDay()
        {
            var culture = new System.Globalization.CultureInfo("pl-PL");
            var day = culture.DateTimeFormat.GetDayName(DateTime.UtcNow.DayOfWeek);
            var upperDay = char.ToUpper(day[0]) + day.Substring(1).ToString();
            return upperDay;
        }

        async void Save()
        {
            if (ShoppingList.Body != null)
            {
                AdjustRecievedInput(ShoppingList.Body);

                try
                {
                    if (string.IsNullOrEmpty(ShoppingList.Name))
                    {
                        ShoppingList.Name = GetTodaysDay();

                    }
                    ShoppingList.CreationDate = DateTime.Today;
                    ShoppingList.User = await SecureStorage.GetAsync("loginToken");
                    MessagingCenter.Send(this, "AddShoppingList", ShoppingList);
                    await Navigation.PopModalAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                MessagingCenter.Send(this, "InputNotValid", ShoppingList);
            }

        }

        private void AdjustRecievedInput(string body)
        {
            ShoppingList.Body = body.TrimEnd('\r', '\n', ' ', ',', '.');
            ShoppingList.BodyHighlight = MakeHighlightFromBody(body);
        }

        async void Cancel()
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

        private void SpeakButton()
        {
            IsNewVoiceListClicked = true;
            InvokeConcreteImplementation();
        }

        private void SpeakAddButton()
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
