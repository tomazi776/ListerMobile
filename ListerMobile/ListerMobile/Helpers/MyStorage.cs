namespace ListerMobile.Helpers
{
    public sealed class MyStorage
    {
        public string UserName { get; set; }

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
