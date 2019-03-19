using ListerMobile.Models;
using ListerMobile.Services;
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
        public ShoppingList ShoppingList { get; set; }
        public List<string> NewListProducts = new List<string>();
        public bool IsNewVoiceListClicked { get; set; }
        private ISpeechToText speechRecongnitionInstance;

        public NewShoppingListPage()
        {
            InitializeComponent();
            BindingContext = this;

            ShoppingList = new ShoppingList
            {
                Id = 1,
                CreationDate = DateTime.Today,
                Name = GetTodaysDate(),
                Body = "- trzy - przykładowe - produkty - lipka - masełko - kalafior - lizaczek - maślanka",
                BodyHighlight = "- trzy - przykładowe - produkty"
            };

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

        private void SpeechToTextFinalResultRecieved(string args)
        {
            var recievedText = GetWords(args);
            string result = string.Empty;

            for (int i = 0; i < recievedText.Length; i++)
            {

                result += "- " + recievedText[i] + "\r\n";
            }

            CheckForVoiceButtonClicked(result);
            //VoiceBodyEditor.Text = result;
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

            string date = DateTime.Today.ToString("dd.MM.yy");
            var day = culture.DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
            var upperDay = char.ToUpper(day[0]) + day.Substring(1);
            string output = upperDay.ToString() + " " + date;
            return output;
        }



        async void Save_Clicked(object sender, EventArgs e)
        {
            ShoppingList.BodyHighlight = MakeHighlightFromBody(ShoppingList.Body);
            TrimEndingOfBody(ShoppingList.Body);
            MessagingCenter.Send(this, "AddShoppingList", ShoppingList);
            await Navigation.PopModalAsync();
        }

        private void TrimEndingOfBody(string body)
        {
            var a = body.Last();
            var b = "dupakl";
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        private string[] GetProductNamesFromBody(string body)
        {
            var temp = GetWords(body);
            return temp;
        }


        private string MakeHighlightFromBody(string body)
        {
            var bodyElement = GetWords(body);
            string result = string.Empty;
            for (int i = 0; i < bodyElement.Length; i++)
            {
                if (bodyElement.Length >= 4)
                {
                    result = "\r\n- " + bodyElement[0] + "\r\n- " + bodyElement[1] + "\r\n- " + bodyElement[2];
                    return result;
                }
                var temp = "\r\n- " + bodyElement[i];
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
            //MessagingCenter.Subscribe<IMessageSender, string>(this, "STT", (sender, args) => {

            //});
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