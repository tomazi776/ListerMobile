using System;
using System.Collections.Generic;

namespace ListerMobile.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreationDate { get; set; } = null;
        public string BodyHighlight { get; set; }
        public string Body { get; set; }
        public string User { get; set; }
        public string Users { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
