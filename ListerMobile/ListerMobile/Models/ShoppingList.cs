using System;
using System.Collections.Generic;

namespace ListerMobile.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Name { get; set; } = "DayOfWeek";
        public DateTime CreationDate { get; set; }
        public string BodyHighlight { get; set; }
        public string Body { get; set; }
        public User User { get; set; }
        public virtual ICollection<Product> Products { get; set; }

        //public ShoppingListStatus Status { get; set; } = ShoppingListStatus.Draft;

        //public enum ShoppingListStatus
        //{
        //    Archived, Draft, Received, Saved, Sent
        //}
    }
}
