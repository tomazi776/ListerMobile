namespace ListerWebServices.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PassPhraseToUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PassPhrase", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "PassPhrase");
        }
    }
}
