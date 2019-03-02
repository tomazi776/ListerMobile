using System.Collections.Generic;

namespace ListerMobile.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal? Price { get; set; }
        public string Picture { get; set; }
        public bool IsFavourite { get; set; }

        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
