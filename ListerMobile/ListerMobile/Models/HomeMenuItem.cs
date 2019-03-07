using System;
using System.Collections.Generic;
using System.Text;

namespace ListerMobile.Models
{
    public enum MenuItemType
    {
        Favourites,
        MyLists,
        Recieved,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
