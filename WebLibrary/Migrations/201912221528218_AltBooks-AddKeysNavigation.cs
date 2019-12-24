namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AltBooksAddKeysNavigation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "AuthorsId", c => c.Int());
            AddColumn("dbo.Books", "GenresId", c => c.Int());
            CreateIndex("dbo.Books", "AuthorsId");
            CreateIndex("dbo.Books", "GenresId");
            AddForeignKey("dbo.Books", "AuthorsId", "dbo.Authors", "Id");
            AddForeignKey("dbo.Books", "GenresId", "dbo.Genres", "Id");
            DropColumn("dbo.Books", "AuthorId");
            DropColumn("dbo.Books", "GenreId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "GenreId", c => c.Int());
            AddColumn("dbo.Books", "AuthorId", c => c.Int());
            DropForeignKey("dbo.Books", "GenresId", "dbo.Genres");
            DropForeignKey("dbo.Books", "AuthorsId", "dbo.Authors");
            DropIndex("dbo.Books", new[] { "GenresId" });
            DropIndex("dbo.Books", new[] { "AuthorsId" });
            DropColumn("dbo.Books", "GenresId");
            DropColumn("dbo.Books", "AuthorsId");
        }
    }
}
