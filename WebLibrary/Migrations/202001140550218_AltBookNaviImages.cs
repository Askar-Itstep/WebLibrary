namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AltBookNaviImages : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Books", name: "ImageId", newName: "ImagesId");
            RenameIndex(table: "dbo.Books", name: "IX_ImageId", newName: "IX_ImagesId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Books", name: "IX_ImagesId", newName: "IX_ImageId");
            RenameColumn(table: "dbo.Books", name: "ImagesId", newName: "ImageId");
        }
    }
}
