namespace ListerWebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modify_ShoppingListModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShoppingLists", "User_Id", "dbo.Users");
            DropIndex("dbo.ShoppingLists", new[] { "User_Id" });
            CreateTable(
                "dbo.UserShoppingLists",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        ShoppingList_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.ShoppingList_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.ShoppingLists", t => t.ShoppingList_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.ShoppingList_Id);
            
            DropColumn("dbo.ShoppingLists", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShoppingLists", "User_Id", c => c.Int());
            DropForeignKey("dbo.UserShoppingLists", "ShoppingList_Id", "dbo.ShoppingLists");
            DropForeignKey("dbo.UserShoppingLists", "User_Id", "dbo.Users");
            DropIndex("dbo.UserShoppingLists", new[] { "ShoppingList_Id" });
            DropIndex("dbo.UserShoppingLists", new[] { "User_Id" });
            DropTable("dbo.UserShoppingLists");
            CreateIndex("dbo.ShoppingLists", "User_Id");
            AddForeignKey("dbo.ShoppingLists", "User_Id", "dbo.Users", "Id");
        }
    }
}
