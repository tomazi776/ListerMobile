using ListerMobile.Models;
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
            MessagingCenter.Send(this, "AddShoppingList", ShoppingList);
            await Navigation.PopModalAsync();
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

    }
}