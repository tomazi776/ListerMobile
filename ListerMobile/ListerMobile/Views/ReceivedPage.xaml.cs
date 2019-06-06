using ListerMobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListerMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReceivedPage : ContentPage
    {
        ShoppingListsViewModel viewModel;
        public bool IsReceivedPage { get; set; }
        public ReceivedPage()
        {
            IsReceivedPage = true;
            InitializeComponent();
            BindingContext = viewModel = new ShoppingListsViewModel(Navigation, IsReceivedPage);
        }
    }
}