namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AltOrderBooks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderBooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        BookId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BookId);
            
            DropTable("dbo.UseBooks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UseBooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(),
                        BookId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.OrderBooks", "UserId", "dbo.Users");
            DropForeignKey("dbo.OrderBooks", "BookId", "dbo.Books");
            DropIndex("dbo.OrderBooks", new[] { "BookId" });
            DropIndex("dbo.OrderBooks", new[] { "UserId" });
            DropTable("dbo.OrderBooks");
        }
    }
}
