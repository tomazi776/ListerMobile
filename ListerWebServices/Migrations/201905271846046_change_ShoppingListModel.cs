namespace ListerWebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_ShoppingListModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShoppingLists", "User", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ShoppingLists", "User");
        }
    }
}
