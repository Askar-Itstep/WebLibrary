namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AltImagesId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "ImagesId", "dbo.Images");
            DropIndex("dbo.Books", new[] { "ImagesId" });
            AlterColumn("dbo.Books", "ImagesId", c => c.Int());
            CreateIndex("dbo.Books", "ImagesId");
            AddForeignKey("dbo.Books", "ImagesId", "dbo.Images", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Books", "ImagesId", "dbo.Images");
            DropIndex("dbo.Books", new[] { "ImagesId" });
            AlterColumn("dbo.Books", "ImagesId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "ImagesId");
            AddForeignKey("dbo.Books", "ImagesId", "dbo.Images", "Id", cascadeDelete: true);
        }
    }
}
