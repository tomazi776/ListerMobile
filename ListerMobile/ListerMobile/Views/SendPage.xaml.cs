using ListerMobile.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendPage : ContentPage
    {
        public string UserNameEntryText { get; set; }

        public SendPage()
        {
            InitializeComponent();
        }

        private void SendShoppingListToUser_Clicked(object sender, EventArgs e)
        {
            ImageButton btn = sender as ImageButton;
            var item = btn.BindingContext as ShoppingList;
            if (item == null) return;

            MessagingCenter.Send(this, "SendToUser");
        }

        private void SearchUser_Clicked(object sender, EventArgs e)
        {


            try
            {
                //Button btn = sender as Button;
                //var item = btn.BindingContext as string;
                //if (item == null) return;



                UserNameEntryText = SearchEntry.Text;

                MessagingCenter.Send(this, "SearchUserClicked", UserNameEntryText);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }

        }
    }
}