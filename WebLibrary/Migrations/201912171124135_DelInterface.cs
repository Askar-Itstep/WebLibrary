namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelInterface : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Name", c => c.String());
        }
    }
}
