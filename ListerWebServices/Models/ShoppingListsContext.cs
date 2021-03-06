﻿using ListerMobile.Models;
using System.Data.Entity;

namespace ListerWebServices.Models
{
    public class ShoppingListsContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public ShoppingListsContext() : base("name=ShoppingListsContext")
        {
        }


        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer<ShoppingListsContext>(null);
        //    base.OnModelCreating(modelBuilder);
        //}


        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
