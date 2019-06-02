using ListerMobile.Models;
using ListerMobile.ViewModels;
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
        SendToUserViewModel viewModel;
        public SendPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SendToUserViewModel();

        }

        private void SendShoppingListToUser_Clicked(object sender, EventArgs e)
        {

            try
            {
                ImageButton btn = sender as ImageButton;
                var item = btn.BindingContext as User;
                if (item == null) return;

                MessagingCenter.Send(this, "SendToChosenUser", item);
            }
            catch (Exception ex)
            {

                Debug.Write(ex.InnerException);
            }

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