namespace WebLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(maxLength: 100),
                        Price = c.Int(),
                        AuthorsId = c.Int(),
                        Pages = c.Int(),
                        GenresId = c.Int(),
                        ImagesId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Authors", t => t.AuthorsId)
                .ForeignKey("dbo.Genres", t => t.GenresId)
                .ForeignKey("dbo.Images", t => t.ImagesId)
                .Index(t => t.AuthorsId)
                .Index(t => t.GenresId)
                .Index(t => t.ImagesId);
            
            CreateTable(
                "dbo.Genres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
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
            DropTable("dbo.Statistics");
            DropTable("dbo.Users");
            DropTable("dbo.OrderBooks");
            DropTable("dbo.Images");
            DropTable("dbo.Genres");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
