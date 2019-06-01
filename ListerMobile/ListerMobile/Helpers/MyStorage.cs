using ListerMobile.Models;
using System.Collections.ObjectModel;

namespace ListerMobile.Helpers
{
    public sealed class MyStorage
    {
        public string UserName { get; set; }
        public ObservableCollection<User> FriendlyUsers { get; set; } = new ObservableCollection<User>();

        private MyStorage()
        {

        }

        private static MyStorage oInstance = null;

        public static MyStorage GetMyStorageInstance
        {
            get
            {
                if (oInstance == null)
                {
                    oInstance = new MyStorage();
                }
                return oInstance;
            }
        }
    }
}
