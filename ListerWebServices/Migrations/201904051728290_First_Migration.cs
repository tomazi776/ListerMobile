namespace ListerWebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First_Migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ShoppingLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreationDate = c.DateTime(),
                        BodyHighlight = c.String(),
                        Body = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category = c.String(),
                        Price = c.Decimal(precision: 18, scale: 2),
                        Picture = c.String(),
                        IsFavourite = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductShoppingLists",
                c => new
                    {
                        Product_Id = c.Int(nullable: false),
                        ShoppingList_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_Id, t.ShoppingList_Id })
                .ForeignKey("dbo.Products", t => t.Product_Id, cascadeDelete: true)
                .ForeignKey("dbo.ShoppingLists", t => t.ShoppingList_Id, cascadeDelete: true)
                .Index(t => t.Product_Id)
                .Index(t => t.ShoppingList_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ShoppingLists", "User_Id", "dbo.Users");
            DropForeignKey("dbo.ProductShoppingLists", "ShoppingList_Id", "dbo.ShoppingLists");
            DropForeignKey("dbo.ProductShoppingLists", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductShoppingLists", new[] { "ShoppingList_Id" });
            DropIndex("dbo.ProductShoppingLists", new[] { "Product_Id" });
            DropIndex("dbo.ShoppingLists", new[] { "User_Id" });
            DropTable("dbo.ProductShoppingLists");
            DropTable("dbo.Users");
            DropTable("dbo.Products");
            DropTable("dbo.ShoppingLists");
        }
    }
}
