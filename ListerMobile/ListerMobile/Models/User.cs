using System.Collections.Generic;

namespace ListerMobile.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ShoppingList> ShoppingLists { get; set; }

        public User()
        {

        }
        public User(string name)
        {
            Name = name;
        }
    }
}