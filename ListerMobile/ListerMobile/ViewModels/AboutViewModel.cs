using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ListerMobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public ICommand OpenWebCommand { get; }

        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

    }
}