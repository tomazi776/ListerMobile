namespace ListerMobile.Models
{
    public enum MenuItemType
    {
        Ulubione,
        Moje_Listy,
        Odebrane,
        O_Aplikacji
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
