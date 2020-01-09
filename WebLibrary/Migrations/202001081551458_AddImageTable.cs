namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        ImageData = c.Binary(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Books", "ImageId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "ImageId");
            AddForeignKey("dbo.Books", "ImageId", "dbo.Images", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "ImageId", "dbo.Images");
            DropIndex("dbo.Books", new[] { "ImageId" });
            DropColumn("dbo.Books", "ImageId");
            DropTable("dbo.Images");
        }
    }
}
