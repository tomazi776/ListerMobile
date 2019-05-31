
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartingPage : ContentPage
    {
        public StartingPage()
        {
            InitializeComponent();

        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    MessagingCenter.Send<StartingPage, bool>(this, "FetchServerData", true);

        //}
    }
}