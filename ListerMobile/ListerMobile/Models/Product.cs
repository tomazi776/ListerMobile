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
        public bool? IsFavourite { get; set; } = false;

        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }

        public Product(string name, string category = null, decimal? price = null, string pic = null, bool? isFavourite = null)
        {
            Name = name;
            Category = category;
            Price = price;
            Picture = pic;
            IsFavourite = isFavourite;
        }

    }
}
