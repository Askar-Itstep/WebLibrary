namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
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
            
            CreateTable(
                "dbo.OrderBooks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsersId = c.Int(),
                        BooksId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BooksId)
                .ForeignKey("dbo.Users", t => t.UsersId)
                .Index(t => t.UsersId)
                .Index(t => t.BooksId);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CountAuthorChoice = c.Int(nullable: false),
                        CountTitleChoice = c.Int(nullable: false),
                        CountGenreChoice = c.Int(nullable: false),
                        CountIsImageChoice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Books", "AuthorsId", c => c.Int());
            AddColumn("dbo.Books", "GenresId", c => c.Int());
            AddColumn("dbo.Books", "ImagesId", c => c.Int(nullable: false));
            CreateIndex("dbo.Books", "AuthorsId");
            CreateIndex("dbo.Books", "GenresId");
            CreateIndex("dbo.Books", "ImagesId");
            AddForeignKey("dbo.Books", "AuthorsId", "dbo.Authors", "Id");
            AddForeignKey("dbo.Books", "GenresId", "dbo.Genres", "Id");
            AddForeignKey("dbo.Books", "ImagesId", "dbo.Images", "Id", cascadeDelete: true);
            DropColumn("dbo.Books", "AuthorId");
            DropColumn("dbo.Books", "GenreId");
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
            
            AddColumn("dbo.Books", "GenreId", c => c.Int());
            AddColumn("dbo.Books", "AuthorId", c => c.Int());
            DropForeignKey("dbo.OrderBooks", "UsersId", "dbo.Users");
            DropForeignKey("dbo.OrderBooks", "BooksId", "dbo.Books");
            DropForeignKey("dbo.Books", "ImagesId", "dbo.Images");
            DropForeignKey("dbo.Books", "GenresId", "dbo.Genres");
            DropForeignKey("dbo.Books", "AuthorsId", "dbo.Authors");
            DropIndex("dbo.OrderBooks", new[] { "BooksId" });
            DropIndex("dbo.OrderBooks", new[] { "UsersId" });
            DropIndex("dbo.Books", new[] { "ImagesId" });
            DropIndex("dbo.Books", new[] { "GenresId" });
            DropIndex("dbo.Books", new[] { "AuthorsId" });
            DropColumn("dbo.Books", "ImagesId");
            DropColumn("dbo.Books", "GenresId");
            DropColumn("dbo.Books", "AuthorsId");
            DropTable("dbo.Statistics");
            DropTable("dbo.OrderBooks");
            DropTable("dbo.Images");
        }
    }
}
